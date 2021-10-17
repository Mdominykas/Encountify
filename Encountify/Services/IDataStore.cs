using System.Collections.Generic;
using System.Threading.Tasks;

namespace Encountify.Services
{
    //generic class for every needed place
    public interface IDataStore<T>
    {
        string DatabaseName();
        Task<bool> AddAsync(T element);
        Task<bool> UpdateAsync(T element);
        Task<bool> DeleteAsync(int id);
        Task<int> DeleteAllAsync();
        Task<T> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync(bool forceRefresh = false);

    }
}
