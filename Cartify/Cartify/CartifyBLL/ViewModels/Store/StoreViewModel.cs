namespace CartifyPLL.Models
{
    public class StoreViewModel
    {
        public List<CartifyBLL.ViewModels.Product.ProductDTO> Products { get; set; }
        public List<CartifyDAL.Entities.category.Category> Categories { get; set; }
    }
}
