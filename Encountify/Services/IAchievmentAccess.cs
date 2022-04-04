using Encountify.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Encountify.Services
{
    public interface IAchievmentAccess
    {
        Task<bool> AddAsync(Achievment achievment);
        Task<bool> UpdateAsync(Achievment achievment);
        Task<bool> DeleteAsync(int id);
        Task<int> DeleteAllAsync();
        Task<Achievment> GetAsync(int id);
        Task<IEnumerable<Achievment>> GetAllAsync(bool forceRefresh = false);
    }
}
