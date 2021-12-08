using Encountify.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Encountify.Services
{
    public interface IVisitedLocationAccess
    {
        Task<bool> AddAsync(VisitedLocations visLoc);
        Task<VisitedLocations> GetAsync(int id);
        Task<IEnumerable<VisitedLocations>> GetAllAsync();
        Task<IEnumerable<VisitedLocations>> GetLastsAsync(int id, int numberOfLocations = 1);
    }
}
