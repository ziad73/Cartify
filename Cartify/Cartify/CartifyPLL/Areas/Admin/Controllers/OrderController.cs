using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CartifyBLL.Services.OrderService.Abstraction;
using CartifyBLL.ViewModels.Orders;

namespace Cartify.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            var (orders, error) =  _orderService.GetAllOrders();
            if (error != null)
            {
                ModelState.AddModelError(string.Empty, error);
                return View(new List<ManageOrdersVm>());
            }
            return View(orders);
        }

        public IActionResult Details(int id)
        {
            var (order, error) =  _orderService.GetOrderById(id);
            if (error != null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpPost]
        public IActionResult ChangeStatus(int id, string status)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var (success, error) = _orderService.ChangeOrderStatus(id, status, User.Identity?.Name ?? "System");
            if (!success)
            {
                ModelState.AddModelError(string.Empty, error ?? "Failed to update order status");
                return BadRequest();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}