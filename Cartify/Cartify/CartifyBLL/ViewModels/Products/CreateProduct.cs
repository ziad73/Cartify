using Microsoft.AspNetCore.Http;

namespace CartifyBLL.ViewModels.Product
{
    public class CreateProduct
    {
            public int ProductId { get; set; }
            public int CategoryId { get; set; }
            public string Name { get;  set; }
            public int StockQuantity { get; set; }
            public double Price { get; set; }
            public string Description { get; set; }
            public bool IsActive { get;  set; }
            public string? CreatedBy { get; set; }
            public string? ModifiedBy { get;  set; }
            public string? DeletedBy { get;  set; }
            public List<IFormFile>? Images { get; set; }
            public string? ImageUrl { get; set; }
    }
}
