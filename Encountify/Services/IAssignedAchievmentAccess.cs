using Encountify.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Encountify.Services
{
    public interface IAssignedAchievmentAccess
    {
        Task<bool> AddAsync(AssignedAchievment assignedAchievment);
        Task<bool> UpdateAsync(AssignedAchievment assignedAchievment);
        Task<bool> DeleteAsync(int id);
        Task<int> DeleteAllAsync();
        Task<AssignedAchievment> GetAsync(int id);
        Task<IEnumerable<AssignedAchievment>> GetAllAsync(bool forceRefresh = false);
    }
}
