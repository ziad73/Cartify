using CartifyDAL.Entities.product;

namespace CartifyDAL.Entities.Wishlist;

public class WishlistItem
{
    public WishlistItem(int wishlistId, int productId, string createdBy)
    {
        WishlistId = wishlistId;
        ProductId = productId;
        CreatedBy = createdBy;
        CreatedOn = DateTime.Now;
        IsDeleted = false;
    }

    public int Id { get; set; }
    public int WishlistId { get; set; }
    public Wishlist Wishlist { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }

    public string CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedOn { get; private set; }
    public string? DeletedBy { get; private set; }

    public void Delete(string deletedBy)
    {
        IsDeleted = true;
        DeletedBy = deletedBy;
        DeletedOn = DateTime.Now;
    }
    
  
}