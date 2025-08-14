using CartifyDAL.Entities.order;
using CartifyDAL.Repo.Abstraction;
using Cartify.DAL.DataBase;
using Microsoft.EntityFrameworkCore;

namespace CartifyDAL.Repo.Implementation
{

    public class OrderItemRepo : IOrderItemRepo
    {
       private readonly CartifyDbContext _db;

        public OrderItemRepo(CartifyDbContext db)
        {
            _db = db;
        }

        public (bool Success, string? ErrorMessage) Create(OrderItem orderItem)
        {
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                _db.OrderItem.Add(orderItem);
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

        public (List<OrderItem>? Items, string? ErrorMessage) GetAll()
        {
            try
            {
                var items = _db.OrderItem
                    .Include(i => i.Order)
                    .Where(a => !a.IsDeleted)
                    .ToList();
                return (items, null);
            }
            catch (Exception ex)
            {
                var inner = ex;
                while (inner.InnerException != null)
                    inner = inner.InnerException;
                return (null, inner.Message);
            }
        }

        public (OrderItem? Item, string? ErrorMessage) GetById(int id)
        {
            try
            {
                var item = _db.OrderItem
                    .Include(i => i.Order)
                    .FirstOrDefault(a => a.OrderItemId == id && !a.IsDeleted);
                if (item == null)
                {
                    return (null, "Order item not found");
                }
                return (item, null);
            }
            catch (Exception ex)
            {
                var inner = ex;
                while (inner.InnerException != null)
                    inner = inner.InnerException;
                return (null, inner.Message);
            }
        }

        public (bool Success, string? ErrorMessage) Update(OrderItem orderItem)
        {
            try
            {
                var existing = _db.OrderItem.FirstOrDefault(a => a.OrderItemId == orderItem.OrderItemId && !a.IsDeleted);
                if (existing == null)
                {
                    return (false, "Order item not found");
                }

                existing.Update(
                    orderItem.Quantity,
                    orderItem.Price,
                    orderItem.Discount,
                    orderItem.ModifiedBy
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
                var item = _db.OrderItem.FirstOrDefault(a => a.OrderItemId == id);
                if (item == null)
                {
                    return (false, "Order item not found");
                }
                item.Delete(item.DeletedBy);
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