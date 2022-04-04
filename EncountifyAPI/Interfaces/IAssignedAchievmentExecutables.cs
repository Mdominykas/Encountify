using EncountifyAPI.Models;
using System;
using System.Collections.Generic;

namespace EncountifyAPI.Interfaces
{
    public interface IAssignedAchievmentExecutables
    {
        List<AssignedAchievment> ExecuteAssignedAchievmentReader(string connectionString, string query, int? id = null, int? userId = null, int? achievmentId = null);
        void ExecuteAssignedAchievmentQuery(string connectionString, string query, int? id = null, int? userId = null, int? achievmentId = null);
    }
}
