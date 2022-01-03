using EncountifyAPI.Models;
using System.Collections.Generic;

namespace EncountifyAPI.Interfaces
{
    public interface IVisitedExecutables
    {
        List<VisitedLocation> ExecuteVisitedLocationReader(string connectionString, string query, int? id = null, int? userId = null, int? locationId = null, int? points = null);
        void ExecuteVisitedLocationQuery(string connectionString, string query, int? id = null, int? userId = null, int? locationId = null, int? points = null);
    }
}