using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EncountifyAPI.Data;
using EncountifyAPI.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EncountifyAPI.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly string ConnectionString;

        public LocationsController(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("EncountifyAPIContext");
        }

        /// <summary>
        /// Get all locations
        /// </summary>
        [HttpGet]
        public IEnumerable<Location> GetAllLocations()
        {
            List<Location> locations = new List<Location>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Locations";
                using SqlCommand command = new SqlCommand(query, connection);
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    locations.Add(ParseLocation(reader));
                }
            }
            return locations;
        }

        /// <summary>
        /// Get location with specified id
        /// </summary>
        [HttpGet("Id/{id}")]
        public Location GetLocation(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Locations WHERE Id = @id";
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                using SqlDataReader reader = command.ExecuteReader();
                return ParseLocation(reader);
            }
        }

        /// <summary>
        /// Get location with specified locationname
        /// </summary>
        [HttpGet("Name/{locationname}")]
        public Location GetLocation(string name)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Locations WHERE CONVERT(VARCHAR, Name) = @name";
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", name);
                using SqlDataReader reader = command.ExecuteReader();
                return ParseLocation(reader);
            }
        }

        /// <summary>
        /// Add a new location
        /// </summary>
        [HttpPost]
        public Location AddLocation(string name, string description = "", float longitude = 0, float latitude = 0, int category = 0, string image = "")
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO Locations VALUES (@name, @description, @longitude, @latitude, @category, @image)";
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@description", description);
                command.Parameters.AddWithValue("@longitude", longitude);
                command.Parameters.AddWithValue("@latitude", latitude);
                command.Parameters.AddWithValue("@category", category);
                command.Parameters.AddWithValue("@image", image);
                command.ExecuteNonQuery();
            }
            return GetLocation(name);
        }

        /// <summary>
        /// Edit an existing location
        /// </summary>
        [HttpPut("{id}")]
        public Location EditLocation(int id, string name = "", string description = "", float longitude = 0, float latitude = 0, int category = 0, string image = "")
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                if (name != "")
                {
                    Location location = EditLocationName(id, name);
                }
                if (description != "")
                {
                    Location location = EditLocationDescription(id, description);
                }
                if (longitude != 0)
                {
                    Location location = EditLocationLongitude(id, longitude);
                }
                if (latitude != 0)
                {
                    Location location = EditLocationLatitude(id, latitude);
                }
                if (category != 0)
                {
                    Location location = EditLocationCategory(id, category);
                }
                if (image != "")
                {
                    Location location = EditLocationImage(id, image);
                }
            }
            return GetLocation(id);
        }


        /// <summary>
        /// Edit an existing location's name
        /// </summary>
        [HttpPut("{id}/Name")]
        public Location EditLocationName(int id, string name)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE Locations SET Name = @name WHERE Id = @id";
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@name", name);
                command.ExecuteNonQuery();
            }
            return GetLocation(id);
        }

        /// <summary>
        /// Edit an existing location's description
        /// </summary>
        [HttpPut("{id}/Description")]
        public Location EditLocationDescription(int id, string description)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE Locations SET Description = @description WHERE Id = @id";
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@description", description);
                command.ExecuteNonQuery();
            }
            return GetLocation(id);
        }

        /// <summary>
        /// Edit an existing location's longitude
        /// </summary>
        [HttpPut("{id}/Longitude")]
        public Location EditLocationLongitude(int id, float longitude)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE Locations SET Longitude = @longitude WHERE Id = @id";
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@longitude", longitude);
                command.ExecuteNonQuery();
            }
            return GetLocation(id);
        }

        /// <summary>
        /// Edit an existing location's latitude
        /// </summary>
        [HttpPut("{id}/Latitude")]
        public Location EditLocationLatitude(int id, float latitude)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE Locations SET Latitude = @latitude WHERE Id = @id";
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@latitude", latitude);
                command.ExecuteNonQuery();
            }
            return GetLocation(id);
        }

        /// <summary>
        /// Edit an existing location's category
        /// </summary>
        [HttpPut("{id}/Category")]
        public Location EditLocationCategory(int id, int category)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE Locations SET Category = @category WHERE Id = @id";
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@category", category);
                command.ExecuteNonQuery();
            }
            return GetLocation(id);
        }

        /// <summary>
        /// Edit an existing location's image
        /// </summary>
        [HttpPut("{id}/Image")]
        public Location EditLocationImage(int id, string image)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE Locations SET Image = @image WHERE Id = @id";
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@image", image);
                command.ExecuteNonQuery();
            }
            return GetLocation(id);
        }

        /// <summary>
        /// Delete all locations
        /// </summary>
        [HttpDelete]
        public IEnumerable<Location> DeleteAllLocations()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "DELETE FROM Locations";
                using SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
            return GetAllLocations();
        }

        /// <summary>
        /// Delete location with specified Id
        /// </summary>
        [HttpDelete("{id}")]
        public Location DeleteLocation(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "DELETE FROM Locations WHERE Id = @id";
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
            return GetLocation(id);
        }

        private Location ParseLocation(SqlDataReader reader)
        {
            Location location = new Location()
            {
                Id = (int)reader["Id"],
                Name = reader["Name"].ToString(),
                Description = reader["Description"].ToString(),
                Longitude = Convert.ToDouble(reader["Longitude"]),
                Latitude = Convert.ToDouble(reader["Latitude"]),
                Category = (int)reader["Category"],
                Image = reader["Image"].ToString(),
            };
            return location;
        }
    }
}
