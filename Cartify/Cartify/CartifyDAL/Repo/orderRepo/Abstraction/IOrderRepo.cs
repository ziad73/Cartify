using CartifyDAL.Entities.order;

namespace CartifyDAL.Repo.Abstraction
{
    public interface IOrderRepo
    {
        (bool Success, string? ErrorMessage) Create(Order order, List<OrderItem>? orderItems = null);
        (List<Order>? Orders, string? ErrorMessage) GetAll();
        (Order? Order, string? ErrorMessage) GetById(int id);
        (bool Success, string? ErrorMessage) Update(Order order);
        (bool Success, string? ErrorMessage) Delete(int id);
    }
}