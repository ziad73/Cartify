using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CartifyDAL.Entities.product;
using CartifyDAL.Entities.user;

namespace CartifyDAL.Entities.Wishlist;

public class Wishlist
{
    public Wishlist(string userId, string createdBy)
    {
        UserId = userId;
        CreatedBy = createdBy;
        CreatedOn = DateTime.Now;
        IsDeleted = false;
        Items = new List<WishlistItem>();
    }

    [Key]
    public int WishListId { get; private set; }

    [Required]
    public string UserId { get; private set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; private set; }

    public ICollection<WishlistItem> Items { get; set; }

    [Required]
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