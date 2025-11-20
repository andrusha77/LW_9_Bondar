using Carpooling.WebApi.Models;

namespace Carpooling.WebApi.Interfaces
{
    public interface IRideService
    {
        Task<IReadOnlyCollection<Ride>> GetAllAsync();
        Task CreateAsync(Ride newRide);
        Task<Ride?> GetByIdAsync(string id);
        Task<bool> DeleteAsync(string id);

        Task<bool> UpdateAsync(string id, Ride updatedRide);
        
    }
}
