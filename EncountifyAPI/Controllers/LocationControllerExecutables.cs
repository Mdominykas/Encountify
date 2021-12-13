using System;
using System.Collections.Generic;
using EncountifyAPI.Models;
using EncountifyAPI.Interfaces;
using System.Data.SqlClient;

namespace EncountifyAPI.Controllers
{
    public class LocationControllerExecutables : ILocationExecutables
    {
       public List<Location> ExecuteLocationReader(string connectionString, string query, int? id = null, string? name = null, string? description = null, double? latitude = null, double? longitude = null, int? category = null, string? image = null)
        {
            List<Location> locations = new List<Location>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using SqlCommand command = new SqlCommand(query, connection);

                if (id != null) command.Parameters.AddWithValue("@id", id ?? default(int));
                if (name != null) command.Parameters.AddWithValue("@name", name ?? default(string));
                if (description != null) command.Parameters.AddWithValue("@description", description ?? default(string));
                if (latitude != null) command.Parameters.AddWithValue("@latitude", latitude ?? default(double));
                if (longitude != null) command.Parameters.AddWithValue("@longitude", longitude ?? default(double));
                if (category != null) command.Parameters.AddWithValue("@category", category ?? default(int));
                if (image != null) command.Parameters.AddWithValue("@image", image ?? default(string));

                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    locations.Add(ParseLocation(reader));
                }
                connection.Close();
            }
            return locations;
        }

        public void ExecuteLocationQuery(string connectionString, string query, int? id = null, string name = null, string description = null, double? latitude = null, double? longitude = null, int? category = null, string image = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using SqlCommand command = new SqlCommand(query, connection);

                if (id != null) command.Parameters.AddWithValue("@id", id ?? default(int));
                if (name != null) command.Parameters.AddWithValue("@name", name ?? default(string));
                if (description != null) command.Parameters.AddWithValue("@description", description ?? default(string));
                if (latitude != null) command.Parameters.AddWithValue("@latitude", latitude ?? default(double));
                if (longitude != null) command.Parameters.AddWithValue("@longitude", longitude ?? default(double));
                if (category != null) command.Parameters.AddWithValue("@category", category ?? default(int));
                if (image != null) command.Parameters.AddWithValue("@image", image ?? default(string));

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static Location ParseLocation(SqlDataReader reader)
        {
            Location location = new Location()
            {
                Id = (int)reader["Id"],
                Name = reader["Name"].ToString(),
                Description = reader["Description"].ToString(),
                Latitude = Convert.ToDouble(reader["Latitude"]),
                Longitude = Convert.ToDouble(reader["Longitude"]),
                Category = (int)reader["Category"],
                Image = reader["Image"].ToString(),
            };
            return location;
        }
    }
}