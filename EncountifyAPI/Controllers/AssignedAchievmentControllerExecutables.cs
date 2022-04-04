using EncountifyAPI.Interfaces;
using EncountifyAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EncountifyAPI.Controllers
{
    public class AssignedAchievmentControllerExecutables : IAssignedAchievmentExecutables
    {
        public List<AssignedAchievment> ExecuteAssignedAchievmentReader(string connectionString, string query, int? id = null, int? userId = null, int? achievmentId = null)
        {
            List<AssignedAchievment> assignments = new List<AssignedAchievment>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using SqlCommand command = new SqlCommand(query, connection);

                if (id != null) command.Parameters.AddWithValue("@id", id ?? default(int));
                if (userId != null) command.Parameters.AddWithValue("@userId", userId ?? default(int));
                if (achievmentId != null) command.Parameters.AddWithValue("@achievmentId", achievmentId ?? default(int));

                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    assignments.Add(ParseAssignedAchievment(reader));
                }
            }
            return assignments;
        }

        public void ExecuteAssignedAchievmentQuery(string connectionString, string query, int? id = null, int? userId = null, int? achievmentId = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using SqlCommand command = new SqlCommand(query, connection);

                if (id != null) command.Parameters.AddWithValue("@id", id ?? default(int));
                if (userId != null) command.Parameters.AddWithValue("@userId", userId ?? default(int));
                if (achievmentId != null) command.Parameters.AddWithValue("@achievmentId", achievmentId ?? default(int));

                command.ExecuteNonQuery();
            }
        }


        public static AssignedAchievment ParseAssignedAchievment(SqlDataReader reader)
        {
            AssignedAchievment assignment = new AssignedAchievment()
            {
                Id = (int)reader["Id"],
                UserId = (int)reader["UserId"],
                AchievmentId = (int)reader["AchievmentId"],
            };
            return assignment;
        }
    }
}
