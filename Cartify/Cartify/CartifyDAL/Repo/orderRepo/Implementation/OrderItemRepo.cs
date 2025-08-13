using CartifyDAL.Entities.order;
using CartifyDAL.Repo.Abstraction;
using Cartify.DAL.DataBase;
namespace CartifyDAL.Repo.Implementation
{

    public class OrderItemRepo : IOrderItemRepo
    {
        private readonly CartifyDbContext db;

        public OrderItemRepo(CartifyDbContext db)
        {
            this.db = db;
        }

        public (bool, string?) Create(OrderItem orderItem)
        {
            try
            {
                db.OrderItem.Add(orderItem);
                db.SaveChanges();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (List<OrderItem>, string?) GetAll()
        {
            try
            {
                var orderItems = db.OrderItem
                    .Where(a => !a.IsDeleted)
                    .ToList();
                return (orderItems, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public (OrderItem, string?) GetById(int id)
        {
            try
            {
                var orderItem = db.OrderItem
                    .FirstOrDefault(a => a.OrderItemId == id && !a.IsDeleted);
                if (orderItem == null)
                {
                    return (null, "Order item not found");
                }
                return (orderItem, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public (bool, string?) Update(OrderItem orderItem)
        {
            try
            {
                var existingOrderItem = db.OrderItem.FirstOrDefault(a => a.OrderItemId == orderItem.OrderItemId && !a.IsDeleted);
                if (existingOrderItem == null)
                {
                    return (false, "Order item not found");
                }
                existingOrderItem.Update(orderItem.Quantity, orderItem.Price, orderItem.Discount, orderItem.ModifiedBy);
                db.SaveChanges();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (bool, string?) Delete(int id)
        {
            try
            {
                var orderItem = db.OrderItem.FirstOrDefault(a => a.OrderItemId == id);
                if (orderItem == null)
                {
                    return (false, "Order item not found");
                }
                orderItem.Delete(orderItem.DeletedBy);
                db.SaveChanges();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}