using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EncountifyAPI.Models;
using EncountifyAPI.Interfaces;
using Microsoft.Extensions.Configuration;

namespace EncountifyAPI.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationExecutables _locationHandling;
        private readonly string _connectionString;

        public LocationsController(IConfiguration configuration, ILocationExecutables locationHandling)
        {
            _connectionString = configuration.GetConnectionString("EncountifyAPIContext");
            _locationHandling = locationHandling;
        }

        /// <summary>
        /// Get all locations
        /// </summary>
        [HttpGet]
        public IEnumerable<Location> GetAllLocations()
        {
            return _locationHandling.ExecuteLocationReader(_connectionString, "SELECT * FROM Locations");
        }

        /// <summary>
        /// Get location with specified id
        /// </summary>
        [HttpGet("Id/{id}")]
        public IEnumerable<Location> GetLocation(int id)
        {
            List<Location> locations = _locationHandling.ExecuteLocationReader(_connectionString, "SELECT * FROM Locations WHERE Id = @id", id: id);
            yield return locations.FirstOrDefault();
        }

        /// <summary>
        /// Get location with specified location name
        /// </summary>
        [HttpGet("Name/{locationname}")]
        public IEnumerable<Location> GetLocation(string name)
        {
            List<Location> locations = _locationHandling.ExecuteLocationReader(_connectionString, "SELECT * FROM Locations WHERE CONVERT(VARCHAR, Name) = @name", name: name);
            yield return locations.FirstOrDefault();
        }

        /// <summary>
        /// Add a new location
        /// </summary>
        [HttpPost]
        public IEnumerable<Location> AddLocation(string name, string description = "", double latitude = 0, double longitude = 0, int category = 0, string image = "")
        {
            _locationHandling.ExecuteLocationQuery(_connectionString, "INSERT INTO Locations VALUES (@name, @description, @latitude, @longitude, @category, @image)", name: name, description: description, latitude: latitude, longitude: longitude, category: category, image:image);
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
            _locationHandling.ExecuteLocationQuery(_connectionString, "UPDATE Locations SET Name = @name WHERE Id = @id", id: id, name: name);
            return GetLocation(id);
        }

        /// <summary>
        /// Edit an existing location's description
        /// </summary>
        [HttpPut("{id}/Description")]
        public IEnumerable<Location> EditLocationDescription(int id, string description)
        {
            _locationHandling.ExecuteLocationQuery(_connectionString, "UPDATE Locations SET Description = @description WHERE Id = @id", id: id, description: description);
            return GetLocation(id);
        }

        /// <summary>
        /// Edit an existing location's latitude
        /// </summary>
        [HttpPut("{id}/Latitude")]
        public IEnumerable<Location> EditLocationLatitude(int id, double latitude)
        {
            _locationHandling.ExecuteLocationQuery(_connectionString, "UPDATE Locations SET Latitude = @latitude WHERE Id = @id", id: id, latitude: latitude);
            return GetLocation(id);
        }

        /// <summary>
        /// Edit an existing location's longitude
        /// </summary>
        [HttpPut("{id}/Longitude")]
        public IEnumerable<Location> EditLocationLongitude(int id, double longitude)
        {
            _locationHandling.ExecuteLocationQuery(_connectionString, "UPDATE Locations SET Longitude = @longitude WHERE Id = @id", id: id, longitude: longitude);
            return GetLocation(id);
        }

        /// <summary>
        /// Edit an existing location's category
        /// </summary>
        [HttpPut("{id}/Category")]
        public IEnumerable<Location> EditLocationCategory(int id, int category)
        {
            _locationHandling.ExecuteLocationQuery(_connectionString, "UPDATE Locations SET Category = @category WHERE Id = @id", id: id, category: category);
            return GetLocation(id);
        }

        /// <summary>
        /// Edit an existing location's image
        /// </summary>
        [HttpPut("{id}/Image")]
        public IEnumerable<Location> EditLocationImage(int id, string image)
        {
            _locationHandling.ExecuteLocationQuery(_connectionString, "UPDATE Locations SET Image = @image WHERE Id = @id", id: id, image: image);
            return GetLocation(id);
        }

        /// <summary>
        /// Delete all locations
        /// </summary>
        [HttpDelete]
        public IEnumerable<Location> DeleteAllLocations()
        {
            _locationHandling.ExecuteLocationQuery(_connectionString, "DELETE FROM Locations");
            return GetAllLocations();
        }

        /// <summary>
        /// Delete location with specified Id
        /// </summary>
        [HttpDelete("{id}")]
        public IEnumerable<Location> DeleteLocation(int id)
        {
            _locationHandling.ExecuteLocationQuery(_connectionString, "DELETE FROM Locations WHERE Id = @id", id);
            return GetLocation(id);
        }
    }
}
