using EncountifyAPI.Models;
using System.Collections.Generic;

namespace EncountifyAPI.Interfaces
{
    public interface IAchievmentExecutables
    {
        List<Achievment> ExecuteAchievmentReader(string connectionString, string query, int? id = null, string? name = null, string? description = null, int? category = null, string? image = null);
        void ExecuteAchievmentQuery(string connectionString, string query, int? id = null, string? name = null, string? description = null, int? category = null, string? image = null);
    }
}
