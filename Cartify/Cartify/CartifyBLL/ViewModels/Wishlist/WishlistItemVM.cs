namespace CartifyBLL.ViewModels.Wishlist;

public class WishlistItemVM
{
    public int ProductId { get; set; }
    public required string ProductName { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
}