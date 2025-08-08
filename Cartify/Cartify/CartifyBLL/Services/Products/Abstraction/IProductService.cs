
using CartifyBLL.ViewModels.Product;


namespace CartifyBLL.Services.Product.Abstraction
{
    public interface IProductService
    {
        (bool, string?) Create(CreateProduct createProduct);
        (List<ProductDTO>, string?) GetAll();
        (ProductDTO, string?) GetById(int ProductId);
        (bool, string?) Update(CreateProduct createProduct);
        (bool, string?) Delete(int ProductId);
    }
}
