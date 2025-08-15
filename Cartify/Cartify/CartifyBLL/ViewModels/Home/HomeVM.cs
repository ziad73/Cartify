using CartifyBLL.ViewModels.Product;


namespace CartifyBLL.ViewModels.Home
{
    public class HomeVM
    {
        public List<ProductDTO> NewProducts { get; set; }
        public List<ProductDTO> CategoryProducts { get; set; }
    }
}
