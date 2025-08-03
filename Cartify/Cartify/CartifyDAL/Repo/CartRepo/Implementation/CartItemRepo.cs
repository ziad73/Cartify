
//using Cartify.DAL.DataBase;
//using CartifyDAL.Entities.cart;
//using CartifyDAL.Repo.cartRepo.Abstraction;

//namespace CartifyDAL.Repo.cartRepo.Implementation
//{
//    internal class CartItemRepo : ICartItemRepo
//    {
//        private readonly CartifyDbContext db;

//        public CartItemRepo(CartifyDbContext db)
//        {
//            this.db = db;
//        }

//        public (bool, string?) Create(CartItem cartItem)
//        {
//            try
//            {
//                db.CartItem.Add(cartItem);
//                db.SaveChanges();
//                return (true, null);
//            }
//            catch(Exception ex)
//            {
//                return (false, ex.Message);
//            }
//        }

//        public (bool, string?) Delete(int cartItemId)
//        {
//            try
//            {
//                var existingCartItem = db.CartItem.FirstOrDefault(a => a.CartitemId == cartItemId && !a.IsDeleted);
//                if (existingCartItem == null)
//                {
//                    return (false, "Cart item not found");
//                }
//                existingCartItem.Delete(existingCartItem.DeletedBy);
//                db.SaveChanges();
//                return (true, null);
//            }
//            catch (Exception ex)
//            {
//                return (false, ex.Message);
//            }
//        }

//        public (List<CartItem>, string?) GetAll()
//        {
//            try
//            {
//                var cartItems = db.CartItem.Where(a => !a.IsDeleted).ToList();
//                return (cartItems, null);
//            }
//            catch (Exception ex)
//            {
//                return (null, ex.Message);
//            }
//        }

//        public (CartItem, string?) GetById(int cartItemId)
//        {
//            try
//            {
//                var cartitem = db.CartItem.FirstOrDefault(a => a.CartitemId == cartItemId && !a.IsDeleted);
//                if (cartitem == null)
//                {
//                    return (null, "Order item not found");
//                }
//                return (cartitem, null);
//            }
//            catch (Exception ex)
//            {
//                return (null, ex.Message);
//            }
//        }

//        public (bool, string?) Update(CartItem cartItem)
//        {
//            try
//            {
//                var existingCartItem = db.CartItem.FirstOrDefault(a => a.CartitemId == cartItem.CartitemId && !a.IsDeleted);
//                if (existingCartItem == null)
//                {
//                    return (false, "Cart item not found");
//                }
//                existingCartItem.Update(cartItem.Quantity, cartItem.ModifiedBy);
//                db.SaveChanges();
//                return (true, null);
//            }
//            catch (Exception ex)
//            {
//                return (false, ex.Message);
//            }
//        }
//    }
//}
