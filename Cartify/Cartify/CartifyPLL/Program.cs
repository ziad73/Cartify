﻿using Cartify.DAL.DataBase;
using CartifyBLL.Services;
using CartifyBLL.Services.UserServices;
using CartifyDAL.Entities.user;
using CartifyDAL.Repo.userRepo.Abstraction;
using CartifyDAL.Repo.userRepo.Impelementaion;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using CartifyBLL.Services.CartService;
using CartifyBLL.Services.CheckoutService;
using CartifyBLL.Services.Wishlist.Abstraction;
using CartifyBLL.Services.Wishlist.Implementation;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace CartifyPLL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
.AddCookie()
.AddGoogle(options =>
{
    var googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
    options.ClientId = googleAuthNSection["ClientId"];
    options.ClientSecret = googleAuthNSection["ClientSecret"];
});


            // Add services to the container.
            builder.Services.AddControllersWithViews();


            builder.Services.AddScoped<IAccountService, AccountService>();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ICartService, CartService>();
            builder.Services.AddScoped<ICheckoutService, CheckoutService>();
            builder.Services.AddScoped<IWishlisrService, WishlistService>();


            // Register repositories
            builder.Services.AddScoped<IUserRepo, UserRepo>();

            // Add Identity services
            builder.Services.AddScoped<EmailSender>();

            //builder.Services.AddScoped<SeedService>(); // If used during seeding

            builder.Services.AddDbContext<CartifyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection")));

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

            var app = builder.Build();
            await SeedService.SeedDatabase(app.Services);


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();// Add authentication middleware


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
