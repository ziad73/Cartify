using Cartify.DAL.DataBase;
using CartifyDAL.Entities.order;

using CartifyDAL.Repo.Abstraction;

namespace CartifyDAL.Repo.Implementation
{
    public class OrderRepo : IOrderRepo
    {
        private readonly CartifyDbContext db;

        public OrderRepo(CartifyDbContext db)
        {
            this.db = db;
        }

        public (bool, string?) Create(Order order)
        {
            try
            {
                db.Order.Add(order);
                db.SaveChanges();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (List<Order>, string?) GetAll()
        {
            try
            {
                var orders = db.Order
                    .Where(a => !a.IsDeleted)
                    .ToList();
                return (orders, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public (Order, string?) GetById(int id)
        {
            try
            {
                var order = db.Order
                    .FirstOrDefault(a => a.OrderId == id && !a.IsDeleted);
                if (order == null)
                {
                    return (null, "Order not found");
                }
                return (order, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public (bool, string?) Update(Order order)
        {
            try
            {
                var existingOrder = db.Order.FirstOrDefault(a => a.OrderId == order.OrderId && !a.IsDeleted);
                if (existingOrder == null)
                {
                    return (false, "Order not found");
                }
                existingOrder.Update(order.OrderStatus, order.ShippingMethod, order.TrackingNumber, order.ShippingCost, order.Tax, order.ModifiedBy);
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
                var order = db.Order.FirstOrDefault(a => a.OrderId == id);
                if (order == null)
                {
                    return (false, "Order not found");
                }
                order.Delete(order.DeletedBy);
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