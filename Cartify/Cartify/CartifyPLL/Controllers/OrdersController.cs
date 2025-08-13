using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using CartifyBLL.Services.OrderService.Abstraction;
using CartifyBLL.ViewModels.Orders;

namespace Cartify.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public IActionResult Index()
        {
            var(orders,error) = _orderService.GetOrdersForUser(User.Identity?.Name ?? string.Empty);
            if (error != null)
            {
                ModelState.AddModelError(string.Empty, error);
                return View(new List<ManageOrdersVm>());
            }

            return View(orders);
        }

        public IActionResult Details(int id)
        {
            var (order, error) = _orderService.GetOrderById(id);
            if (error != null)
                return NotFound();
            return View(order);
        }

        public IActionResult Cancel(int id)
        {
            var (order, error) = _orderService.GetOrderById(id);
            if (error != null || order.CustomerEmail != User.Identity?.Name)
            {
                return NotFound();
            }

            if (order.OrderStatus == "Pending" || order.OrderStatus == "Processing")
            {
                var (success, cancelError) = _orderService.ChangeOrderStatus(id, "Cancelled", User.Identity?.Name ?? "System");
                if (!success)
                {
                    ModelState.AddModelError(string.Empty, cancelError ?? "Unable to cancel order");
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}