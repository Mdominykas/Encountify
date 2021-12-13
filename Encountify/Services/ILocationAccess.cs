using Encountify.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Encountify.Services
{
    public interface ILocationAccess
    {
        Task<bool> AddAsync(Location location); 
        Task<bool> UpdateAsync(Location location);
        Task<bool> DeleteAsync(int id);                 
        Task<int> DeleteAllAsync();
        Task<Location> GetAsync(int id);
        Task<IEnumerable<Location>> GetAllAsync();
    }
}
