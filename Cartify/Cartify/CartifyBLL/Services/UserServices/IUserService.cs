using CartifyBLL.ViewModels.Account;
using CartifyBLL.ViewModels.User;
using CartifyDAL.Entities.user;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace CartifyBLL.Services.UserServices
{
    public interface IUserService
    {
        Task<ProfileVM> GetUserProfileAsync(string userId);
        Task<IdentityResult> UpdateUserProfileAsync(string userId, EditProfileVM model);
        Task<IdentityResult> ChangePasswordAsync(string userId, string oldPassword, string newPassword);
        Task<(bool Succeeded, string AvatarUrl, IEnumerable<string> Errors)> UploadAvatarAsync(string userId, IFormFile avatar);
        Task<IdentityResult> AddAddressAsync(string userId, AddressVM model);
        Task<IdentityResult> UpdateAddressAsync(string userId, AddressVM model);
        Task<IdentityResult> DeleteAddressAsync(string userId, int addressId);
        Task<IdentityResult> SetDefaultAddressAsync(string userId, int addressId);
        Task<AddressVM> GetAddressAsync(string userId, int addressId);
        Task<List<UserVM>> GetAllUsersAsync();
        Task<UserVM?> GetUserByIdAsync(string id);
        Task<bool> UpdateUserAsync(UserVM model);
        Task<bool> DeleteUserAsync(string id);
        Task<bool> RestoreUserAsync(string id);
        Task<bool> DeactivateUserAsync(string id);


    }
}