using CartifyDAL.Entities.order;

namespace CartifyDAL.Repo.Abstraction
{
    public interface IOrderRepo
    {
        (bool, string?) Create(Order order);
        (List<Order>, string?) GetAll();
        (Order, string?) GetById(int id);
        (bool, string?) Update(Order order);
        (bool, string?) Delete(int id);
    }
}