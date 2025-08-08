﻿using CartifyDAL.Entities.cart;
using CartifyDAL.Entities.category;
using CartifyDAL.Entities.order;
using CartifyDAL.Entities.payment;
using CartifyDAL.Entities.product;
using CartifyDAL.Entities.productCart;
using CartifyDAL.Entities.user;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using CartifyDAL.Entities.Wishlist;


namespace Cartify.DAL.DataBase
{
    public class CartifyDbContext : IdentityDbContext<User>
    {
        public CartifyDbContext(DbContextOptions<CartifyDbContext> options) : base(options) { }
      
        public DbSet<Cart> Cart { get; set; }
        public DbSet<CartItem> CartItem { get; set; }
        public DbSet<ProductCart> ProductCart { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductReview> ProductReview { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAddress> UserAddress { get; set; }
        public DbSet<UserPayment> UserPayment { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<PaymentMethod> PaymentMethod { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //Setting Composite Key of ProductCart class
            modelBuilder.Entity<ProductCart>()
                .HasKey(pc => new { pc.CartId, pc.ProductId });
            //M-to-M Relatioship
            modelBuilder.Entity<ProductCart>()
                .HasOne(p => p.product)
                .WithMany(pc => pc.productCarts)
                .HasForeignKey(pc => pc.ProductId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProductCart>()
                .HasOne(c => c.cart)
                .WithMany(pc => pc.productCarts)
                .HasForeignKey(pc => pc.CartId).OnDelete(DeleteBehavior.NoAction);
            
            modelBuilder.Entity<User>()
            .Property(u => u.Gender)
            .HasConversion<string>();
        }
    }
}
