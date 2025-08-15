using AutoMapper;
using CartifyBLL.Services.Product.Abstraction;
using CartifyBLL.ViewModels.Product;
using CartifyDAL.Entities.product;
using CartifyDAL.Repo.productRepo.Abstraction;
using CartifyDAL.Repo.ProductRepo.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
        public async Task<(bool, string?)> Create(CreateProduct createProduct)
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
                            await image.CopyToAsync(stream);
                        }

                        imageUrls.Add("/Files/" + fileName);
                    }

                    
                    createProduct.ImageUrl = string.Join("|", imageUrls);
                }

                var product = mapper.Map<CartifyDAL.Entities.product.Product>(createProduct);

                var result = await productRepo.Create(product);
                return (result);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public async Task<(bool, string?)> Delete(int ProductId)
        {
            return await productRepo.Delete(ProductId);
        }

        public async Task<(List<ProductDTO>, string?)> GetAll()
        {
            var result = await productRepo.GetAll();
            if (result.Item2 != null)
                return (null, result.Item2);
            var productDTOs = mapper.Map<List<ProductDTO>>(result.Item1);
            return (productDTOs, null);
        }

        public async Task<(ProductDTO, string?)> GetById(int ProductId)
        {
            var result = await productRepo.GetById(ProductId);
            if (result.Item2 != null)
                return (null, result.Item2);

            var productDTO = mapper.Map<ProductDTO>(result.Item1);
            return (productDTO, null);
        }

        public async Task<(bool, string?)> Update(CreateProduct createProduct)
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
                            await image.CopyToAsync(stream);
                        }

                        imageUrls.Add("/Files/" + uniqueName);
                    }
                }

                
                createProduct.ImageUrl = string.Join("|", imageUrls);
            }

            var product = mapper.Map<CartifyDAL.Entities.product.Product>(createProduct);
            return await productRepo.Update(product);
        }
    }
}
