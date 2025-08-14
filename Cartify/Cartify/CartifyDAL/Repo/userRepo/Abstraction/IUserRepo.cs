using Cartify.DAL.DataBase;
using CartifyDAL.Entities.user;
using Microsoft.AspNetCore.Identity;

namespace CartifyDAL.Repo.userRepo.Abstraction
{
    public interface IUserRepo
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> AddAsync(User user, string password);
        Task<bool> RemovePasswordAsync(User user);
        Task<bool> AddPasswordAsync(User user, string newPassword);
        Task<bool> UpdateAsync(User user);
        Task<User?> GetByIdAsync(string id);
        Task<IList<string>> GetRolesAsync(User user);
    }

}
