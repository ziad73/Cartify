
using CartifyBLL.ViewModels.Product;


namespace CartifyBLL.Services.Product.Abstraction
{
    public interface IProductService
    {
        Task<(bool, string?)> Create(CreateProduct createProduct);
        Task<(List<ProductDTO>, string?)> GetAll();
        Task<(ProductDTO, string?)> GetById(int ProductId);
        Task<(bool, string?)> Update(CreateProduct createProduct);
        Task<(bool, string?)> Delete(int ProductId);
    }
}
