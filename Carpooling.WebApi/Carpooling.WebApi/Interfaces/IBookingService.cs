using Carpooling.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carpooling.WebApi.Interfaces
{
    public interface IBookingService
    {
        Task<IReadOnlyCollection<Booking>> GetAllAsync();
        Task CreateAsync(Booking newBooking);
        Task<Booking?> GetByIdAsync(string id);
        Task<bool> UpdateAsync(string id, Booking updatedBooking);
        Task<bool> DeleteAsync(string id);
    }
}
