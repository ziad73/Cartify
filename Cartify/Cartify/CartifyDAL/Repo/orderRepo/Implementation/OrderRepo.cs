using Cartify.DAL.DataBase;
using CartifyDAL.Entities.order;

using CartifyDAL.Repo.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace CartifyDAL.Repo.Implementation
{
    public class OrderRepo : IOrderRepo
    {
         private readonly CartifyDbContext _db;

        public OrderRepo(CartifyDbContext db)
        {
            _db = db;
        }

        public (bool Success, string? ErrorMessage) Create(Order order, List<OrderItem>? orderItems = null)
        {
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                _db.Order.Add(order);

                if (orderItems != null && orderItems.Any())
                {
                    foreach (var item in orderItems)
                    {
                        // Attach the order ID after the order is tracked
                        item.Order = order;
                        _db.OrderItem.Add(item);
                    }
                }

                _db.SaveChanges();
                transaction.Commit();
                return (true, null);
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                var inner = ex;
                while (inner.InnerException != null)
                    inner = inner.InnerException;
                return (false, inner.Message);
            }
        }

        public (List<Order>? Orders, string? ErrorMessage) GetAll()
        {
            try
            {
                var orders = _db.Order
                    .Include(o=>o.User)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .Where(a => !a.IsDeleted)
                    .ToList();
                return (orders, null);
            }
            catch (Exception ex)
            {
                var inner = ex;
                while (inner.InnerException != null)
                    inner = inner.InnerException;
                return (null, inner.Message);
            }
        }

        public (Order? Order, string? ErrorMessage) GetById(int id)
        {
            try
            {
                var order = _db.Order
                    .Include(o => o.User)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .FirstOrDefault(a => a.OrderId == id && !a.IsDeleted);
                if (order == null)
                {
                    return (null, "Order not found");
                }
                return (order, null);
            }
            catch (Exception ex)
            {
                var inner = ex;
                while (inner.InnerException != null)
                    inner = inner.InnerException;
                return (null, inner.Message);
            }
        }

        public (bool Success, string? ErrorMessage) Update(Order order)
        {
            try
            {
                var existingOrder = _db.Order.FirstOrDefault(a => a.OrderId == order.OrderId && !a.IsDeleted);
                if (existingOrder == null)
                {
                    return (false, "Order not found");
                }

                existingOrder.Update(
                    order.OrderStatus,
                    order.ShippingMethod,
                    order.TrackingNumber,
                    order.ShippingCost,
                    order.Tax,
                    order.ModifiedBy
                );

                _db.SaveChanges();
                return (true, null);
            }
            catch (Exception ex)
            {
                var inner = ex;
                while (inner.InnerException != null)
                    inner = inner.InnerException;
                return (false, inner.Message);
            }
        }

        public (bool Success, string? ErrorMessage) Delete(int id)
        {
            try
            {
                var order = _db.Order.FirstOrDefault(a => a.OrderId == id);
                if (order == null)
                {
                    return (false, "Order not found");
                }
                order.Delete(order.DeletedBy);
                _db.SaveChanges();
                return (true, null);
            }
            catch (Exception ex)
            {
                var inner = ex;
                while (inner.InnerException != null)
                    inner = inner.InnerException;
                return (false, inner.Message);
            }
        }
    }
}