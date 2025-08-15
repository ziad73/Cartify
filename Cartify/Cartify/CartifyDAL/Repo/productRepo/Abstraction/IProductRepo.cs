using CartifyDAL.Entities.category;
using CartifyDAL.Entities.product;

namespace CartifyDAL.Repo.productRepo.Abstraction
{
    public interface IProductRepo
    {
        Task<(bool, string?)> Create(Product product);
        Task<(List<Product>, string?)> GetAll();
        Task<(Product, string?)> GetById(int productId);
        Task<(bool, string?)> Update(Product product);
        Task<(bool, string?)> Delete(int productId);
        Task ReduceStockAsync(int productId, int quantity);
        public (bool Success, string? ErrorMessage) IncreaseStock(int productId, int quantity);
    }
}
