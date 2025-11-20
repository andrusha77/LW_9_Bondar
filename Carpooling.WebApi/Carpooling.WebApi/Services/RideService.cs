using  Carpooling.WebApi.Interfaces;
using Carpooling.WebApi.Repositories;
using Carpooling.WebApi.Models;

namespace Carpooling.WebApi.Services
{
    public class RideService : IRideService
    {
        private readonly IRepository<Ride> _repo;
        public RideService(IRepository<Ride> repo)
        {
            _repo = repo;
        }
        public async Task<IReadOnlyCollection<Ride>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<Ride?> GetByIdAsync(string id) => await _repo.GetByIdAsync(id);
        public async Task CreateAsync(Ride newRide) => await _repo.CreateAsync(newRide);
        public async Task<bool> UpdateAsync(string id, Ride updatedRide) => await _repo.UpdateAsync(id, updatedRide);
        public async Task<bool> DeleteAsync(string id) => await _repo.DeleteAsync(id);

    }
}
