﻿using Cartify.DAL.DataBase;
using CartifyBLL.Mapper;
using CartifyBLL.Services;
using CartifyBLL.Services.CartService.Abstraction;
using CartifyBLL.Services.CartService.Implementation;
using CartifyBLL.Services.CategoryServices;
using CartifyBLL.Services.CheckoutService.Abstraction;
using CartifyBLL.Services.CheckoutService.Implementation;
using CartifyBLL.Services.OrderService.Abstraction;
using CartifyBLL.Services.OrderService.Implementation;
using CartifyBLL.Services.Product.Abstraction;
using CartifyBLL.Services.Product.Impelementation;
using CartifyBLL.Services.ProductReviewServices.Abstraction;
using CartifyBLL.Services.ProductReviewServices.Impelementation;
using CartifyBLL.Services.SearchService.Abstraction;
using CartifyBLL.Services.SearchService.Implementation;
using CartifyBLL.Services.UserServices;
using CartifyBLL.Services.WishlistService.Abstraction;
using CartifyBLL.Services.WishlistService.Implementation;
using CartifyDAL.Entities.user;
using CartifyDAL.Repo.Abstraction;
using CartifyDAL.Repo.cartRepo.Abstraction;
using CartifyDAL.Repo.cartRepo.Implementation;
using CartifyDAL.Repo.categoryRepo.Abstraction;
using CartifyDAL.Repo.CategoryRepo.Implementation;
using CartifyDAL.Repo.Implementation;
using CartifyDAL.Repo.productRepo.Abstraction;
using CartifyDAL.Repo.ProductRepo.Implementation;
using CartifyDAL.Repo.ProductReviewRepo.Abstraction;
using CartifyDAL.Repo.ProductReviewRepo.Implementation;
using CartifyDAL.Repo.SearchRepo.Abstraction;
using CartifyDAL.Repo.SearchRepo.Implementation;
using CartifyDAL.Repo.userRepo.Abstraction;
using CartifyDAL.Repo.userRepo.Impelementaion;
using CartifyDAL.Repo.WishlistRepo.Abstraction;
using CartifyDAL.Repo.WishlistRepo.Implementation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;

namespace CartifyPLL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure Authentication
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();
            //Optionally configure cookie settings
            var googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
            var clientId = googleAuthNSection["ClientId"];
            var clientSecret = googleAuthNSection["ClientSecret"];

            // Register Google only if credentials exist
            if (!string.IsNullOrEmpty(clientId) && !string.IsNullOrEmpty(clientSecret))
            {
                builder.Services.AddAuthentication()
                    .AddGoogle(options =>
                    {
                        options.ClientId = clientId;
                        options.ClientSecret = clientSecret;
                    });
            }


            // Add MVC services
            builder.Services.AddControllersWithViews();

            // Configure DbContext FIRST (before other services that depend on it)
            builder.Services.AddDbContext<CartifyDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection")));

            // Configure Identity
            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                // Password settings
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // User settings
                options.User.RequireUniqueEmail = true;

                // Sign-in settings
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            })
            .AddEntityFrameworkStores<CartifyDbContext>()
            .AddDefaultTokenProviders();

            // Configure AutoMapper
            builder.Services.AddAutoMapper(x => x.AddProfile(new DomainProfile()));

            // Register Core Services
            builder.Services.AddScoped<EmailSender>();
            builder.Services.AddScoped<SeedService>();

            // Register Account & User Services
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IUserService, UserService>();

            // Register Repository Layer
            builder.Services.AddScoped<IUserRepo, UserRepo>();
            builder.Services.AddScoped<ICategoryRepo, CategoryRepo>();
            builder.Services.AddScoped<IProductRepo, ProductRepo>();
            builder.Services.AddScoped<ICartRepo, CartRepo>();
            builder.Services.AddScoped<ICartItemRepo, CartItemRepo>();
            builder.Services.AddScoped<IWishlistRepo, WishlistRepo>();
            builder.Services.AddScoped<ISearchRepo, SearchRepo>();
            builder.Services.AddScoped<IOrderRepo, OrderRepo>();
            builder.Services.AddScoped<IWishlistItemRepo, WishlistItemRepo>();
            
            // ADD THE MISSING REPOSITORY REGISTRATION
            builder.Services.AddScoped<IOrderItemRepo, OrderItemRepo>();

            // Register Business Logic Layer (Services)
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<IWishlistService, WishlistService>();
            builder.Services.AddScoped<ISearchService, SearchService>();
            builder.Services.AddScoped<IOrderService, OrderService>();

            // productReview Repo
            builder.Services.AddScoped<IProductReviewRepo, ProductReviewRepo>();
            // productReview services
            builder.Services.AddScoped<IProductReviewServices, ProductReviewServices>();
            // Register CheckoutService ONCE (you had it twice)
            builder.Services.AddScoped<ICheckoutService, CheckoutService>();

            var app = builder.Build();
            
            // Seed database
            await SeedService.SeedDatabase(app.Services);

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=AdminDashboard}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}