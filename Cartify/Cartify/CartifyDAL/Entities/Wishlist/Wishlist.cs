using CartifyDAL.Entities.user;

namespace CartifyDAL.Entities.Wishlist;

public class Wishlist
{
   
        public int Id { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<WishlistItem> Items { get; set; }
    

}