namespace CartifyBLL.ViewModels.Wishlist;

public class WishlistVM
{
    public int WishlistId { get; set; }
    public string UserId { get; set; }
    public List<WishlistItemVM> Items { get; set; }
}