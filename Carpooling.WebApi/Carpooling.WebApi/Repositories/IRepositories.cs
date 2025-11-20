using System.Collections.Generic;
using System.Threading.Tasks;

namespace Carpooling.WebApi.Repositories
{

    public interface IRepository<T>
    {
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<T?> GetByIdAsync(string id);
        Task CreateAsync(T entity);
        Task<bool> UpdateAsync(string id, T entity);
        Task<bool> DeleteAsync(string id);
    }


}
