using System;
using System.Collections.Generic;
using EncountifyAPI.Models;
using EncountifyAPI.Interfaces;
using System.Data.SqlClient;

namespace EncountifyAPI.Controllers
{
    public class VisitedControllerExecutables : IVisitedExecutables
    {
        public List<VisitedLocation> ExecuteVisitedLocationReader(string connectionString, string query, int? id = null, int? userId = null, int? locationId = null, int? points = null)
        {
            List<VisitedLocation> visits = new List<VisitedLocation>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using SqlCommand command = new SqlCommand(query, connection);

                if (id != null) command.Parameters.AddWithValue("@id", id ?? default(int));
                if (userId != null) command.Parameters.AddWithValue("@userId", userId ?? default(int));
                if (locationId != null) command.Parameters.AddWithValue("@locationId", locationId ?? default(int));
                if (points != null) command.Parameters.AddWithValue("@points", points ?? default(int));

                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    visits.Add(ParseVisitedLocation(reader));
                }
            }
            return visits;
        }

        public void ExecuteVisitedLocationQuery(string connectionString, string query, int? id = null, int? userId = null, int? locationId = null, int? points = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using SqlCommand command = new SqlCommand(query, connection);

                if (id != null) command.Parameters.AddWithValue("@id", id ?? default(int));
                if (userId != null) command.Parameters.AddWithValue("@userId", userId ?? default(int));
                if (locationId != null) command.Parameters.AddWithValue("@locationId", locationId ?? default(int));
                if (points != null) command.Parameters.AddWithValue("@points", points ?? default(int));

                command.ExecuteNonQuery();
            }
        }


        public static VisitedLocation ParseVisitedLocation(SqlDataReader reader)
        {
            VisitedLocation visit = new VisitedLocation()
            {
                Id = (int)reader["Id"],
                UserId = (int)reader["UserId"],
                LocationId = (int)reader["LocationId"],
                Points = (int)reader["Points"]
            };
            return visit;
        }

    }
}