using CartifyBLL.Helper;
using CartifyBLL.Services.UserServices;
using CartifyBLL.ViewModels.Account;
using CartifyDAL.Entities.user;
using CartifyDAL.Repo.userRepo.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Security.Claims;

namespace CartifyBLL.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IUserRepo userRepo;
        private readonly UserManager<User> userManager;

        public UserService(IUserRepo userRepo, UserManager<User> userManager)
        {
            this.userRepo = userRepo;
            this.userManager = userManager;
        }

        public async Task<ProfileVM> GetUserProfileAsync(string userId)
        {
            var user = await userRepo.GetByIdAsync(userId);

            if (user == null)
                throw new Exception("User not found");

            var totalOrders = user.Orders?.Count ?? 0;
            var loyaltyPoints = totalOrders * 10;

            return new ProfileVM
            {
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                AvatarUrl = user.AvatarUrl,
                MemberSince = user.JoinDate,
                TotalOrders = totalOrders,
                LoyaltyPoints = loyaltyPoints,
                DefaultShippingAddress = user.Addresses?.FirstOrDefault(a => a.IsDefault),
                EmailVerified = user.IsEmailVerified,
                Addresses = user.Addresses?.Select(a => a.ToAddressVM()).ToList(),
                PhoneVerified = !string.IsNullOrEmpty(user.PhoneNumber)
            };
        }

        public async Task<IdentityResult> UpdateUserProfileAsync(string userId, EditProfileVM model)
        {
            var user = await userRepo.GetByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;

            return await userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            var user = await userRepo.GetByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            return await userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task<(bool Succeeded, string AvatarUrl, IEnumerable<string> Errors)> UploadAvatarAsync(string userId, IFormFile avatar)
        {
            var user = await userRepo.GetByIdAsync(userId);
            if (user == null)
                return (false, "", new[] { "User not found" });

            var extension = Path.GetExtension(avatar.FileName);
            var fileName = $"avatar_{userId}{extension}";
            var folderPath = Path.Combine("wwwroot", "uploads", "avatars");
            Directory.CreateDirectory(folderPath);
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await avatar.CopyToAsync(stream);
            }

            user.AvatarUrl = $"/uploads/avatars/{fileName}";
            var result = await userManager.UpdateAsync(user);
            return (result.Succeeded, user.AvatarUrl, result.Errors.Select(e => e.Description));
        }

        public async Task<IdentityResult> AddAddressAsync(string userId, AddressVM model)
        {
            var user = await userRepo.GetByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            var address = new UserAddress
            {
                StreetAddress = model.StreetAddress,
                City = model.City,
                State = model.State,
                PostalCode = model.PostalCode,
                Country = model.Country,
                PhoneNumber = model.PhoneNumber,
                IsDefault = model.IsDefault,
                Name = model.Name
            };

            if (user.Addresses == null)
            {
                user.Addresses = new List<UserAddress>();
            }

            // If this is the first address or marked as default, set as default
            if (model.IsDefault || !user.Addresses.Any())
            {
                foreach (var existingAddress in user.Addresses)
                {
                    existingAddress.IsDefault = false;
                }
                address.IsDefault = true;
            }

            user.Addresses.Add(address);
            return await userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> UpdateAddressAsync(string userId, AddressVM model)
        {
            var user = await userRepo.GetByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            var address = user.Addresses?.FirstOrDefault(a => a.Id == model.Id);
            if (address == null)
                return IdentityResult.Failed(new IdentityError { Description = "Address not found" });

            address.StreetAddress = model.StreetAddress;
            address.City = model.City;
            address.State = model.State;
            address.PostalCode = model.PostalCode;
            address.Country = model.Country;
            address.PhoneNumber = model.PhoneNumber;
            address.Name = model.Name;

            if (model.IsDefault)
            {
                foreach (var existingAddress in user.Addresses)
                {
                    existingAddress.IsDefault = false;
                }
                address.IsDefault = true;
            }

            return await userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteAddressAsync(string userId, int addressId)
        {
            var user = await userRepo.GetByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            var address = user.Addresses?.FirstOrDefault(a => a.Id == addressId);
            if (address == null)
                return IdentityResult.Failed(new IdentityError { Description = "Address not found" });

            user.Addresses.Remove(address);
            return await userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> SetDefaultAddressAsync(string userId, int addressId)
        {
            var user = await userRepo.GetByIdAsync(userId);
            if (user == null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });

            var address = user.Addresses?.FirstOrDefault(a => a.Id == addressId);
            if (address == null)
                return IdentityResult.Failed(new IdentityError { Description = "Address not found" });

            // Reset all other addresses to non-default
            foreach (var addr in user.Addresses)
            {
                addr.IsDefault = false;
            }

            // Set the selected address as default
            address.IsDefault = true;

            return await userManager.UpdateAsync(user);
        }
        public async Task<AddressVM> GetAddressAsync(string userId, int addressId)
        {
            var user = await userRepo.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("User not found");

            var address = user.Addresses?.FirstOrDefault(a => a.Id == addressId);
            if (address == null)
                throw new Exception("Address not found");

            return new AddressVM
            {
                Id = address.Id,
                Name = address.Name,
                StreetAddress = address.StreetAddress,
                City = address.City,
                State = address.State,
                PostalCode = address.PostalCode,
                Country = address.Country,
                PhoneNumber = address.PhoneNumber,
                IsDefault = address.IsDefault
            };
        }
    }
}
