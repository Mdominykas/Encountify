using Encountify.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Encountify.Services
{
    public interface ILocation
    {
        Task<bool> AddAsync(Location user);
        Task<bool> UpdateAsync(Location user);
        Task<bool> DeleteAsync(int id);
        Task<int> DeleteAllAsync();
        Task<Location> GetAsync(int id);
        Task<IEnumerable<Location>> GetAllAsync();
    }
}
