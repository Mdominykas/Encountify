using System.Collections.Generic;
using System.Threading.Tasks;

namespace Encountify.Services
{
    //generic class for every needed place
    public interface IDataStore<T>
    {
        Task<bool> AddAsync(T location);
        Task<bool> UpdateAsync(T location);
        Task<bool> DeleteAsync(int id);
        Task<T> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(bool forceRefresh = false);
    }
}
