using AutoMapper;
using Cartify.DAL.DataBase;
using CartifyBLL.Services.CartService.Abstraction;
using CartifyBLL.Services.CheckoutService.Abstraction;
using CartifyBLL.Services.UserServices;
using CartifyBLL.ViewModels.Account;
using CartifyBLL.ViewModels.Cart;
using CartifyBLL.ViewModels.Checkout;
using CartifyDAL.Entities.order;
using CartifyDAL.Repo.Abstraction;
using Microsoft.Extensions.Logging;

namespace CartifyBLL.Services.CheckoutService.Implementation;

public class CheckoutService : ICheckoutService
{
    private readonly ICartService _cartService;
    private readonly IUserService _userService;
    private readonly IOrderRepo _orderRepo;
    private readonly IOrderItemRepo _orderItemRepo;
    private readonly IMapper _mapper;

    public CheckoutService(
        ICartService cartService,
        IUserService userService,
        IOrderRepo orderRepo,
        IOrderItemRepo orderItemRepo,
        IMapper mapper)
    {
        _cartService = cartService;
        _userService = userService;
        _orderRepo = orderRepo;
        _orderItemRepo = orderItemRepo;
        _mapper = mapper;
    }

    public (CheckoutVm, string?) GetCheckoutData(string userId)
    {
        try
        {
            var checkoutVM = new CheckoutVm();

            // Get user cart
            var (cart, cartError) = _cartService.GetUserCart(userId);
            if (!string.IsNullOrEmpty(cartError))
                return (checkoutVM, cartError);

            if (cart.IsEmpty)
                return (checkoutVM, "Cart is empty");

            checkoutVM.Cart = cart;

            // Get user profile to load addresses
            var userProfile = _userService.GetUserProfileAsync(userId).Result;
            if (userProfile != null)
            {
                checkoutVM.UserAddresses = userProfile.Addresses ?? new List<AddressVM>();

                // Set default address if exists
                var defaultAddress = checkoutVM.UserAddresses.FirstOrDefault(a => a.IsDefault);
                if (defaultAddress != null)
                    checkoutVM.SelectedAddressId = defaultAddress.Id;
            }

            return (checkoutVM, null);
        }
        catch (Exception ex)
        {
            return (new CheckoutVm(), ex.Message);
        }
    }

    public (bool, string?) ValidateCheckout(CheckoutVm model)
    {
        try
        {
            if (model.Cart.IsEmpty)
                return (false, "Cart is empty");

            if (!model.UseNewAddress && model.SelectedAddressId <= 0)
                return (false, "Please select a shipping address");

            if (model.UseNewAddress)
            {
                if (string.IsNullOrWhiteSpace(model.NewAddress.StreetAddress) ||
                    string.IsNullOrWhiteSpace(model.NewAddress.City) ||
                    string.IsNullOrWhiteSpace(model.NewAddress.State) ||
                    string.IsNullOrWhiteSpace(model.NewAddress.PostalCode) ||
                    string.IsNullOrWhiteSpace(model.NewAddress.Country))
                    return (false, "Please fill all address fields");
            }

            if (string.IsNullOrWhiteSpace(model.PaymentMethod))
                return (false, "Please select a payment method");

            // Basic payment validation (in real app, use proper payment processor)
            if (model.PaymentMethod == "CreditCard")
            {
                if (string.IsNullOrWhiteSpace(model.CardNumber) ||
                    string.IsNullOrWhiteSpace(model.CardHolderName) ||
                    string.IsNullOrWhiteSpace(model.ExpiryMonth) ||
                    string.IsNullOrWhiteSpace(model.ExpiryYear) ||
                    string.IsNullOrWhiteSpace(model.CVV))
                    return (false, "Please fill all payment details");
            }

            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public (OrderConfirmationVm, string?) ProcessOrder(CheckoutVm model, string userId)
    {
        try
        {
            // 1. Validate
            var (isValid, validationError) = ValidateCheckout(model);
            if (!isValid)
                return (null, validationError);

            // 2. Add new address if requested 
            if (model.UseNewAddress) // bug need to fix
            {
                var result = _userService.AddAddressAsync(userId, model.NewAddress).Result;
                if (!result.Succeeded)
                    return (null, "Failed to add shipping address");

                var userProfile = _userService.GetUserProfileAsync(userId).Result;
                var newAddress = userProfile?.Addresses?.OrderByDescending(a => a.Id).FirstOrDefault();
                if (newAddress != null)
                    model.SelectedAddressId = newAddress.Id;
            }

            // 3. Create order (required non-null fields set)
            var order = new Order(
                orderStatus: "Pending",
                shippingMethod: "Standard Shipping",
                trackingNumber: GenerateTrackingNumber(),
                shippingCost: model.ShippingCost,
                tax: model.Tax,
                createdBy: string.IsNullOrWhiteSpace(userId) ? "Guest" : userId
            )
            {
                UserId = userId
            };

            // 4. Build the list of OrderItem instances (do NOT set OrderId here; OrderRepo will attach them)
            var orderItems = new List<OrderItem>();
            foreach (var cartItem in model.Cart.Items)
            {
                var oi = new OrderItem(
                    quantity: cartItem.Quantity,
                    price: cartItem.ProductPrice,
                    discount: 0,
                    createdBy: string.IsNullOrWhiteSpace(userId) ? "Guest" : userId
                )
                {
                    ProductId = cartItem.ProductId
                };
                orderItems.Add(oi);
            }

            // 5. Save order + items in single transaction (use repository overload)
            var (orderCreated, orderError) = _orderRepo.Create(order, orderItems);
            if (!orderCreated)
                return (null, orderError ?? "Failed to create order");

            // 6. Build confirmation view model BEFORE clearing the cart (so we still have readable product names/prices)
            var confirmation = new OrderConfirmationVm
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                OrderTotal = model.Total,
                PaymentMethod = model.PaymentMethod,
                OrderStatus = "Confirmed",
                TrackingNumber = order.TrackingNumber,
                // reuse cart VM items for display (they already contain product names/prices)
                OrderItems = model.Cart.Items
            };

            // 7. Clear cart (best-effort, don't fail the order if this fails)
            var (cartCleared, clearError) = _cartService.ClearCart(userId);
            if (!cartCleared)
                Console.WriteLine($"Warning: Failed to clear cart for user {userId}: {clearError}");

            return (confirmation, null);
        }
        catch (Exception ex)
        {
            // return deepest DB message to the caller (Controller will show it for AJAX or TempData)
            Exception deepest = ex;
            while (deepest.InnerException != null) deepest = deepest.InnerException;
            return (null, deepest.Message);
        }
    }



    public (OrderConfirmationVm, string?) GetOrderConfirmation(int orderId, string userId)
    {
        try
        {
            var (order, error) = _orderRepo.GetById(orderId);
            if (!string.IsNullOrEmpty(error) || order == null)
                return (null, "Order not found");

            // Verify ownership
            if (order.UserId != userId)
                return (null, "Unauthorized");

            var confirmation = new OrderConfirmationVm
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                OrderTotal = (order.OrderItems?.Sum(oi => oi.Price * oi.Quantity) ?? 0) + order.Tax + order.ShippingCost,
                OrderStatus = order.OrderStatus,
                TrackingNumber = order.TrackingNumber,
                OrderItems = order.OrderItems?.Select(oi => new CartItemVm
                {
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    ProductPrice = oi.Price,
                    ProductName = $"Product #{oi.ProductId}" // placeholder if product nav not loaded
                }).ToList() ?? new List<CartItemVm>()
            };

            return (confirmation, null);
        }
        catch (Exception ex)
        {
            // Walk down to the deepest exception
            Exception deepest = ex;
            while (deepest.InnerException != null)
                deepest = deepest.InnerException;

            return (null, $"DB ERROR: {deepest.Message}");
        }
    }

    private string GenerateTrackingNumber()
    {
        return $"TRK{DateTime.Now:yyyyMMdd}{new Random().Next(1000, 9999)}";
    }
}