using AutoMapper;
using CartifyBLL.Services.Product.Abstraction;
using CartifyBLL.ViewModels.Product;
using CartifyDAL.Entities.product;
using CartifyDAL.Repo.productRepo.Abstraction;
using CartifyDAL.Repo.ProductRepo.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace CartifyBLL.Services.Product.Impelementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepo productRepo;
        private readonly IMapper mapper;


        public ProductService(IProductRepo productRepo, IMapper _mapper)
        {
            this.productRepo = productRepo;
            mapper = _mapper;
        }
        public (bool, string?) Create(CreateProduct createProduct)
        {
            try
            {
                if (createProduct == null)
                    return (false, "Invalid product data");
                if (createProduct.Price <= 0)
                    return (false, "Price must be greater than 0.");

                

                var imageUrls = new List<string>();

                if (createProduct.Images != null && createProduct.Images.Any())
                {
                    var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files");

                    if (!Directory.Exists(imagesFolder))
                        Directory.CreateDirectory(imagesFolder);

                    foreach (var image in createProduct.Images)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                        var filePath = Path.Combine(imagesFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            image.CopyTo(stream);
                        }

                        imageUrls.Add("/Files/" + fileName);
                    }

                    
                    createProduct.ImageUrl = string.Join("|", imageUrls);
                }

                var product = mapper.Map<CartifyDAL.Entities.product.Product>(createProduct);

                var result = productRepo.Create(product);
                return (result);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (bool, string?) Delete(int ProductId)
        {
            return productRepo.Delete(ProductId);
        }

        public (List<ProductDTO>, string?) GetAll()
        {
            var result = productRepo.GetAll();
            if (result.Item2 != null)
                return (null, null);
            var productDTOs = mapper.Map<List<ProductDTO>>(result.Item1);
            return (productDTOs, null);
        }

        public (ProductDTO, string?) GetById(int ProductId)
        {
            var result = productRepo.GetById(ProductId);
            if (result.Item2 != null)
                return (null, null);

            var productDTO = mapper.Map<ProductDTO>(result.Item1);
            return (productDTO, null);
        }

        public (bool, string?) Update(CreateProduct createProduct)
        {
            if (createProduct.Price <= 0)
                return (false, "Price must be greater than 0.");

            string imagesFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files");
            List<string> imageUrls = new();

            if (createProduct.Images != null && createProduct.Images.Any())
            {
                foreach (var image in createProduct.Images)
                {
                    if (image != null && image.Length > 0)
                    {
                        string uniqueName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                        string filePath = Path.Combine(imagesFolderPath, uniqueName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            image.CopyTo(stream);
                        }

                        imageUrls.Add("/Files/" + uniqueName);
                    }
                }

                
                createProduct.ImageUrl = string.Join("|", imageUrls);
            }

            var product = mapper.Map<CartifyDAL.Entities.product.Product>(createProduct);
            return productRepo.Update(product);
        }
    }
}
