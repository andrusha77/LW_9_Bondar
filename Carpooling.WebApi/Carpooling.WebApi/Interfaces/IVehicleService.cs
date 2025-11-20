
using Carpooling.WebApi.Models;

namespace Carpooling.WebApi.Interfaces
{
    public interface IVehicleService
    {
        Task<IReadOnlyCollection<Vehicle>> GetAllAsync();
        Task CreateAsync(Vehicle newVehicle);
        Task<bool> UpdateAsync(string id, Vehicle updatedVehicle);
        Task<bool> DeleteAsync(string id);
        Task<Vehicle?> GetByIdAsync(string id);
    }
}
