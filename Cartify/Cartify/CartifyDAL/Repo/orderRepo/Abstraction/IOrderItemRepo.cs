using CartifyDAL.Entities.order;

namespace CartifyDAL.Repo.Abstraction
{
    public interface IOrderItemRepo
    {
        (bool, string?) Create(OrderItem orderItem);
        (List<OrderItem>, string?) GetAll();
        (OrderItem, string?) GetById(int id);
        (bool, string?) Update(OrderItem orderItem);
        (bool, string?) Delete(int id);
    }
}