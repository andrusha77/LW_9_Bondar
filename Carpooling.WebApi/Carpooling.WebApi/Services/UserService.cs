
using Carpooling.WebApi.Interfaces;
using Carpooling.WebApi.Models;
using Carpooling.WebApi.Repositories;

namespace Carpooling.WebApi.Services
{
    public class UserService:IUserService
    {
        private readonly IRepository<User> _repo;
        public UserService(IRepository<User> repo)
        {
            _repo = repo;
        }
        public async Task<IReadOnlyCollection<User>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<User?> GetByIdAsync(string id) => await _repo.GetByIdAsync(id);
        public async Task CreateAsync (User newUser) => await _repo.CreateAsync(newUser);
        public async Task<bool> UpdateAsync(string id, User updatedUser) => await _repo.UpdateAsync(id, updatedUser);
        public async Task<bool> DeleteAsync(string id) => await _repo.DeleteAsync(id);
        public async Task<User?> GetByEmailAsync(string email)
        {
            var allUsers = await _repo.GetAllAsync();
            return allUsers.FirstOrDefault(u => u.Email == email);
        }

    }
}
