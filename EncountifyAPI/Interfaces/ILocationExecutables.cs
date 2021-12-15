using EncountifyAPI.Models;
using System.Collections.Generic;

namespace EncountifyAPI.Interfaces
{
    public interface ILocationExecutables
    {
        List<Location> ExecuteLocationReader(string connectionString, string query, int? id = null, string? name = null, string? description = null, double? latitude = null, double? longitude = null, int? category = null, string? image = null);
        void ExecuteLocationQuery(string connectionString, string query, int? id = null, string name = null, string description = null, double? latitude = null, double? longitude = null, int? category = null, string image = null);
    }
}