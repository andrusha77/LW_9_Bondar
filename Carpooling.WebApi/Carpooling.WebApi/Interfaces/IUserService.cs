using Carpooling.WebApi.Models;

namespace Carpooling.WebApi.Interfaces
{
    public interface IUserService
    {
        Task<IReadOnlyCollection<User>> GetAllAsync();
        Task CreateAsync(User newUser);
        Task<bool> UpdateAsync(string id, User updatedUser);
        Task<bool> DeleteAsync(string id);
        Task<User?> GetByEmailAsync(string Email);
        Task<User?> GetByIdAsync(string id);
    }
}
