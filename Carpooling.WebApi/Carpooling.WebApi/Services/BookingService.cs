using Carpooling.WebApi.Interfaces;
using Carpooling.WebApi.Repositories;
using Carpooling.WebApi.Models;

namespace Carpooling.WebApi.Services
{
    public class BookingService : IBookingService
    {
        private readonly IRepository<Booking> _repo;
        public BookingService(IRepository<Booking> repo)
        {
            _repo = repo;
        }
        public async Task<IReadOnlyCollection<Booking>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<Booking?> GetByIdAsync(string id) => await _repo.GetByIdAsync(id);
        public async Task CreateAsync(Booking booking) => await _repo.CreateAsync(booking);
        public async Task<bool> UpdateAsync(string id, Booking updatedBooking) => await _repo.UpdateAsync(id, updatedBooking);
        public async Task<bool> DeleteAsync(string id) => await _repo.DeleteAsync(id);

    }
}
