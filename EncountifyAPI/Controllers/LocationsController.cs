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
            return ExecuteReader("SELECT * FROM Locations");
        }

        /// <summary>
        /// Get location with specified id
        /// </summary>
        [HttpGet("Id/{id}")]
        public IEnumerable<Location> GetLocation(int id)
        {
            return ExecuteReader("SELECT * FROM Locations WHERE Id = @id", id: id);
        }

        /// <summary>
        /// Get location with specified location name
        /// </summary>
        [HttpGet("Name/{locationname}")]
        public IEnumerable<Location> GetLocation(string name)
        {
            return ExecuteReader("SELECT * FROM Locations WHERE CONVERT(VARCHAR, Name) = @name", name: name);
        }

        /// <summary>
        /// Add a new location
        /// </summary>
        [HttpPost]
        public IEnumerable<Location> AddLocation(string name, string description = "", float longitude = 0, float latitude = 0, int category = 0, string image = "")
        {

            ExecuteQuery("INSERT INTO Locations VALUES (@name, @description, @longitude, @latitude, @category, @image)", name: name, description: description, longitude: longitude, latitude: latitude, category: category, image:image);
            return GetLocation(name);
        }

        /// <summary>
        /// Edit an existing location
        /// </summary>
        [HttpPut("{id}")]
        public IEnumerable<Location> EditLocation(int id, string name = "", string description = "", float longitude = 0, float latitude = 0, int category = 0, string image = "")
        {
            if (name != "") EditLocationName(id, name);
            if (description != "") EditLocationDescription(id, description);
            if (longitude != 0) EditLocationLongitude(id, longitude);
            if (latitude != 0) EditLocationLatitude(id, latitude);
            if (category != 0) EditLocationCategory(id, category);
            if (image != "") EditLocationImage(id, image);

            return GetLocation(id);
        }


        /// <summary>
        /// Edit an existing location's name
        /// </summary>
        [HttpPut("{id}/Name")]
        public IEnumerable<Location> EditLocationName(int id, string name)
        {
            ExecuteQuery("UPDATE Locations SET Name = @name WHERE Id = @id", id: id, name: name);
            return GetLocation(id);
        }

        /// <summary>
        /// Edit an existing location's description
        /// </summary>
        [HttpPut("{id}/Description")]
        public IEnumerable<Location> EditLocationDescription(int id, string description)
        {
            ExecuteQuery("UPDATE Locations SET Description = @description WHERE Id = @id", id: id, description: description);
            return GetLocation(id);
        }

        /// <summary>
        /// Edit an existing location's longitude
        /// </summary>
        [HttpPut("{id}/Longitude")]
        public IEnumerable<Location> EditLocationLongitude(int id, float longitude)
        {
            ExecuteQuery("UPDATE Locations SET Longitude = @longitude WHERE Id = @id", id: id, longitude: longitude);
            return GetLocation(id);
        }

        /// <summary>
        /// Edit an existing location's latitude
        /// </summary>
        [HttpPut("{id}/Latitude")]
        public IEnumerable<Location> EditLocationLatitude(int id, float latitude)
        {
            ExecuteQuery("UPDATE Locations SET Latitude = @latitude WHERE Id = @id", id: id, latitude: latitude);
            return GetLocation(id);
        }

        /// <summary>
        /// Edit an existing location's category
        /// </summary>
        [HttpPut("{id}/Category")]
        public IEnumerable<Location> EditLocationCategory(int id, int category)
        {
            ExecuteQuery("UPDATE Locations SET Category = @category WHERE Id = @id", id: id, category: category);
            return GetLocation(id);
        }

        /// <summary>
        /// Edit an existing location's image
        /// </summary>
        [HttpPut("{id}/Image")]
        public IEnumerable<Location> EditLocationImage(int id, string image)
        {
            ExecuteQuery("UPDATE Locations SET Image = @image WHERE Id = @id", id: id, image: image);
            return GetLocation(id);
        }

        /// <summary>
        /// Delete all locations
        /// </summary>
        [HttpDelete]
        public IEnumerable<Location> DeleteAllLocations()
        {
            ExecuteQuery("DELETE FROM Locations");
            return GetAllLocations();
        }

        /// <summary>
        /// Delete location with specified Id
        /// </summary>
        [HttpDelete("{id}")]
        public IEnumerable<Location> DeleteLocation(int id)
        {
            ExecuteQuery("DELETE FROM Locations WHERE Id = @id", id);
            return GetLocation(id);
        }

        private List<Location> ExecuteReader(string query, int id = -1, string name = null, string description = null, float longitude = -1, float latitude = -1, int category = -1, string image = null)
        {
            List<Location> locations = new List<Location>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using SqlCommand command = new SqlCommand(query, connection);

                if (id != -1) command.Parameters.AddWithValue("@id", id);
                if (name != null) command.Parameters.AddWithValue("@name", name);
                if (name != null) command.Parameters.AddWithValue("@description", description);
                if (longitude != -1) command.Parameters.AddWithValue("@longitude", longitude);
                if (latitude != -1) command.Parameters.AddWithValue("@latitude", latitude);
                if (category != -1) command.Parameters.AddWithValue("@category", category);
                if (image != null) command.Parameters.AddWithValue("@image", image);

                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    locations.Add(ParseLocation(reader));
                }
            }
            return locations;
        }

        private void ExecuteQuery(string query, int id = -1, string name = null, string description = null, float longitude = -1, float latitude = -1, int category = -1, string image = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using SqlCommand command = new SqlCommand(query, connection);

                if (id != -1) command.Parameters.AddWithValue("@id", id);
                if (name != null) command.Parameters.AddWithValue("@name", name);
                if (name != null) command.Parameters.AddWithValue("@description", description);
                if (longitude != -1) command.Parameters.AddWithValue("@longitude", longitude);
                if (latitude != -1) command.Parameters.AddWithValue("@latitude", latitude);
                if (category != -1) command.Parameters.AddWithValue("@category", category);
                if (image != null) command.Parameters.AddWithValue("@image", image);

                command.ExecuteNonQuery();
            }
        }

        private static Location ParseLocation(SqlDataReader reader)
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
