using CartifyDAL.Entities.order;

namespace CartifyDAL.Repo.Abstraction
{
    public interface IOrderItemRepo
    {
        (bool Success, string? ErrorMessage) Create(OrderItem orderItem);
        (List<OrderItem>? Items, string? ErrorMessage) GetAll();
        (OrderItem? Item, string? ErrorMessage) GetById(int id);
        (bool Success, string? ErrorMessage) Update(OrderItem orderItem);
        (bool Success, string? ErrorMessage) Delete(int id);
    }
}