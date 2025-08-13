namespace CartifyBLL.ViewModels.Wishlist;

public class WishlistVm
{
    public string UserId { get; set; }
    public List<WishlistItemVm> Items { get; set; } = new();
    public int ItemCount => Items.Count;
    public bool IsEmpty => !Items.Any();
}