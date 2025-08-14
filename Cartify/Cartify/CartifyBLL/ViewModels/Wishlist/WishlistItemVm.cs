namespace CartifyBLL.ViewModels.Wishlist;

public class WishlistItemVm
{
    public int WishListId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductImageUrl { get; set; }
    public double ProductPrice { get; set; }
    public string Category { get; set; }
    public bool IsAvailable { get; set; }
    public int StockQuantity { get; set; }
    public DateTime AddedOn { get; set; }
    public string ProductUrl => $"/Product/Details/{ProductId}";
}