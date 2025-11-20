using Carpooling.WebApi.Interfaces;
using Carpooling.WebApi.Models;
using Carpooling.WebApi.Repositories;

namespace Carpooling.WebApi.Services
{
    public class VehicleService: IVehicleService
    {
        private readonly IRepository<Vehicle> _repo;
        public VehicleService(IRepository<Vehicle> repo)
        {
            _repo = repo;
        }
        public async Task<IReadOnlyCollection<Vehicle>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<Vehicle?> GetByIdAsync(string id) => await _repo.GetByIdAsync(id);
        public async Task CreateAsync (Vehicle newVehicle) => await _repo.CreateAsync(newVehicle);
        public async Task<bool> UpdateAsync(string id, Vehicle updatedVehicle) => await _repo.UpdateAsync(id, updatedVehicle);
        public async Task<bool> DeleteAsync(string id) => await _repo.DeleteAsync(id);

    }
}
