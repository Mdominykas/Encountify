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
            return ExecuteLocationReader("SELECT * FROM Locations");
        }

        /// <summary>
        /// Get location with specified id
        /// </summary>
        [HttpGet("Id/{id}")]
        public IEnumerable<Location> GetLocation(int id)
        {
            List<Location> locations = ExecuteLocationReader("SELECT * FROM Locations WHERE Id = @id", id: id);
            yield return locations.FirstOrDefault();
        }

        /// <summary>
        /// Get location with specified location name
        /// </summary>
        [HttpGet("Name/{locationname}")]
        public IEnumerable<Location> GetLocation(string name)
        {
            List<Location> locations = ExecuteLocationReader("SELECT * FROM Locations WHERE CONVERT(VARCHAR, Name) = @name", name: name);
            yield return locations.FirstOrDefault();
        }

        /// <summary>
        /// Add a new location
        /// </summary>
        [HttpPost]
        public IEnumerable<Location> AddLocation(string name, string description = "", double latitude = 0, double longitude = 0, int category = 0, string image = "")
        {
            ExecuteLocationQuery("INSERT INTO Locations VALUES (@name, @description, @latitude, @longitude, @category, @image)", name: name, description: description, latitude: latitude, longitude: longitude, category: category, image:image);
            return GetLocation(name);
        }

        /// <summary>
        /// Edit an existing location
        /// </summary>
        [HttpPut("{id}")]
        public IEnumerable<Location> EditLocation(int id, string? name = null, string? description = null, double? latitude = null, double? longitude = null, int? category = null, string? image = null)
        {
            if (name != null) EditLocationName(id, name);
            if (description != null) EditLocationDescription(id, description);
            if (latitude != null) EditLocationLatitude(id, latitude ?? default(double));
            if (longitude != null) EditLocationLongitude(id, longitude ?? default(double));
            if (category != null) EditLocationCategory(id, category ?? default(int));
            if (image != null) EditLocationImage(id, image);

            return GetLocation(id);
        }


        /// <summary>
        /// Edit an existing location's name
        /// </summary>
        [HttpPut("{id}/Name")]
        public IEnumerable<Location> EditLocationName(int id, string name)
        {
            ExecuteLocationQuery("UPDATE Locations SET Name = @name WHERE Id = @id", id: id, name: name);
            return GetLocation(id);
        }

        /// <summary>
        /// Edit an existing location's description
        /// </summary>
        [HttpPut("{id}/Description")]
        public IEnumerable<Location> EditLocationDescription(int id, string description)
        {
            ExecuteLocationQuery("UPDATE Locations SET Description = @description WHERE Id = @id", id: id, description: description);
            return GetLocation(id);
        }

        /// <summary>
        /// Edit an existing location's latitude
        /// </summary>
        [HttpPut("{id}/Latitude")]
        public IEnumerable<Location> EditLocationLatitude(int id, double latitude)
        {
            ExecuteLocationQuery("UPDATE Locations SET Latitude = @latitude WHERE Id = @id", id: id, latitude: latitude);
            return GetLocation(id);
        }

        /// <summary>
        /// Edit an existing location's longitude
        /// </summary>
        [HttpPut("{id}/Longitude")]
        public IEnumerable<Location> EditLocationLongitude(int id, double longitude)
        {
            ExecuteLocationQuery("UPDATE Locations SET Longitude = @longitude WHERE Id = @id", id: id, longitude: longitude);
            return GetLocation(id);
        }

        /// <summary>
        /// Edit an existing location's category
        /// </summary>
        [HttpPut("{id}/Category")]
        public IEnumerable<Location> EditLocationCategory(int id, int category)
        {
            ExecuteLocationQuery("UPDATE Locations SET Category = @category WHERE Id = @id", id: id, category: category);
            return GetLocation(id);
        }

        /// <summary>
        /// Edit an existing location's image
        /// </summary>
        [HttpPut("{id}/Image")]
        public IEnumerable<Location> EditLocationImage(int id, string image)
        {
            ExecuteLocationQuery("UPDATE Locations SET Image = @image WHERE Id = @id", id: id, image: image);
            return GetLocation(id);
        }

        /// <summary>
        /// Delete all locations
        /// </summary>
        [HttpDelete]
        public IEnumerable<Location> DeleteAllLocations()
        {
            ExecuteLocationQuery("DELETE FROM Locations");
            return GetAllLocations();
        }

        /// <summary>
        /// Delete location with specified Id
        /// </summary>
        [HttpDelete("{id}")]
        public IEnumerable<Location> DeleteLocation(int id)
        {
            ExecuteLocationQuery("DELETE FROM Locations WHERE Id = @id", id);
            return GetLocation(id);
        }

        private List<Location> ExecuteLocationReader(string query, int? id = null, string? name = null, string? description = null, double? latitude = null, double? longitude = null, int? category = null, string? image = null)
        {
            List<Location> locations = new List<Location>();
            using (var connection = new SqlConnection(ConnectionString))
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

        private void ExecuteLocationQuery(string query, int? id = null, string name = null, string description = null, double? latitude = null, double? longitude = null, int? category = null, string image = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
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

        private static Location ParseLocation(SqlDataReader reader)
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
