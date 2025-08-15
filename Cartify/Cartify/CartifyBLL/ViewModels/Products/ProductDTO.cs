
using CartifyDAL.Entities.category;
using Microsoft.AspNetCore.Http;

namespace CartifyBLL.ViewModels.Product
{
    public class ProductDTO
    {
        public ProductDTO()
        {
            
        }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public string Category { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<IFormFile>? Images { get; set; }
        public string? ImageUrl { get; set; }
        public bool InWishlist { get; set; }
        public bool IsNew
        {
            get
            {
                return (DateTime.Now - CreatedOn).TotalDays <= 5;
            }
        }
    }
}
