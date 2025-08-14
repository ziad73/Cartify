using AutoMapper;
using CartifyBLL.Services.OrderService.Abstraction;
using CartifyBLL.ViewModels.Orders;
using CartifyDAL.Repo.Abstraction;
using Microsoft.Extensions.Logging;

namespace CartifyBLL.Services.OrderService.Implementation;

public class OrderService : IOrderService
{
    private readonly IOrderRepo _orderRepo;
private readonly IOrderItemRepo _orderItemRepo;
private readonly IMapper _mapper;
private readonly ILogger<OrderService> _logger;

public OrderService(IOrderRepo orderRepo, IOrderItemRepo orderItemRepo, IMapper mapper, ILogger<OrderService> logger)
{
    _orderRepo = orderRepo;
    _orderItemRepo = orderItemRepo;
    _mapper = mapper;
    _logger = logger;
}

public (List<ManageOrdersVm>, string?) GetAllOrders()
{
    try
    {
        _logger.LogInformation("Retrieving all orders");
        var (orders, error) = _orderRepo.GetAll();
        if (error != null)
        {
            _logger.LogError("Error retrieving orders: {Error}", error);
            return (null, error);
        }

        var (orderItems, itemError) = _orderItemRepo.GetAll();
        if (itemError != null)
        {
            _logger.LogError("Error retrieving order items: {Error}", itemError);
            return (null, itemError);
        }

        var orderViewModels = _mapper.Map<List<ManageOrdersVm>>(orders);
        foreach (var orderViewModel in orderViewModels)
        {
            var items = orderItems.Where(i => i.OrderId == orderViewModel.OrderId && !i.IsDeleted).ToList();
            orderViewModel.TotalAmount = items.Sum(i => i.Quantity * (i.Price - i.Discount)) + orders.First(o => o.OrderId == orderViewModel.OrderId).ShippingCost + orders.First(o => o.OrderId == orderViewModel.OrderId).Tax;
        }

        _logger.LogInformation("Retrieved {Count} orders", orderViewModels.Count);
        return (orderViewModels, null);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Unexpected error in GetAllOrders");
        return (null, $"Unexpected error: {ex.Message}");
    }
}

public (OrderDetailsVm, string?) GetOrderById(int orderId)
{
    try
    {
        _logger.LogInformation("Retrieving order {OrderId}", orderId);
        var (order, error) = _orderRepo.GetById(orderId);
        if (error != null)
        {
            _logger.LogError("Error retrieving order {OrderId}: {Error}", orderId, error);
            return (null, error);
        }

        var (orderItems, itemError) = _orderItemRepo.GetAll();
        if (itemError != null)
        {
            _logger.LogError("Error retrieving order items for order {OrderId}: {Error}", orderId, itemError);
            return (null, itemError);
        }

        var items = orderItems.Where(i => i.OrderId == orderId && !i.IsDeleted).ToList();
        var orderViewModel = _mapper.Map<OrderDetailsVm>(order);
        orderViewModel.OrderItems = _mapper.Map<List<OrderItemVm>>(items);
        orderViewModel.TotalAmount = items.Sum(i => i.Quantity * (i.Price - i.Discount)) + order.ShippingCost + order.Tax;

        _logger.LogInformation("Retrieved order {OrderId}", orderId);
        return (orderViewModel, null);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Unexpected error in GetOrderById for {OrderId}", orderId);
        return (null, $"Unexpected error: {ex.Message}");
    }
}

public (bool, string?) ChangeOrderStatus(int orderId, string newStatus, string modifiedBy)
{
    try
    {
        _logger.LogInformation("Changing status for order {OrderId} to {Status}", orderId, newStatus);
        var (order, error) = _orderRepo.GetById(orderId);
        if (error != null)
        {
            _logger.LogError("Error retrieving order {OrderId}: {Error}", orderId, error);
            return (false, error);
        }
        order.Update(newStatus, order.ShippingMethod, order.TrackingNumber, order.ShippingCost, order.Tax, modifiedBy);
        var result = _orderRepo.Update(order);
        if (!result.Item1)
        {
            _logger.LogError("Error updating order {OrderId}: {Error}", orderId, result.Item2);
            return result;
        }
        _logger.LogInformation("Order {OrderId} status updated to {Status}", orderId, newStatus);
        return (true, null);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Unexpected error in ChangeOrderStatus for {OrderId}", orderId);
        return (false, $"Unexpected error: {ex.Message}");
    }
}

public (List<ManageOrdersVm>, string?) GetOrdersForUser(string username)
{
    try
    {
        // Get all orders from repo
        var (orders, error) = _orderRepo.GetAll();
        if (error != null) return (null, error);

        // Get all order items
        var (orderItems, itemError) = _orderItemRepo.GetAll();
        if (itemError != null) return (null, itemError);

       
        var orderViewModels = _mapper.Map<List<ManageOrdersVm>>(orders);

        // Filter by the current user's email from the ViewModel
        var userOrders = orderViewModels
            .Where(o => o.CustomerEmail == username)
            .ToList();

        // Calculate totals and item counts
        foreach (var orderVM in userOrders)
        {
            var items = orderItems
                .Where(i => i.OrderId == orderVM.OrderId && !i.IsDeleted)
                .ToList();

            var matchingOrder = orders.First(o => o.OrderId == orderVM.OrderId);

            orderVM.TotalAmount =
                items.Sum(i => i.Quantity * (i.Price - i.Discount))
                + matchingOrder.ShippingCost
                + matchingOrder.Tax;

            orderVM.ItemCount = items.Sum(i => i.Quantity);
        }

        return (userOrders, null);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error retrieving user orders");
        return (null, $"Unexpected error: {ex.Message}");
    }
}
}