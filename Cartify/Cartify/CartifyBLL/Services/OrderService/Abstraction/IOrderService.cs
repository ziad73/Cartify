using CartifyBLL.ViewModels.Orders;

namespace CartifyBLL.Services.OrderService.Abstraction;

public interface IOrderService
{ 
   (List<ManageOrdersVm>, string?) GetAllOrders(); 
   (OrderDetailsVm, string?) GetOrderById(int orderId); 
   (bool, string?) ChangeOrderStatus(int orderId, string newStatus, string modifiedBy);
  (List<ManageOrdersVm> orders, string? error) GetOrdersForUser(string username);


    
}
