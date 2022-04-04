using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EncountifyAPI.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using EncountifyAPI.Interfaces;

namespace EncountifyAPI.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class VisitedLocationsController : ControllerBase
    {
        private readonly string ConnectionString;
        private readonly IVisitedExecutables _visitedHandler;

        public VisitedLocationsController(IConfiguration configuration, IVisitedExecutables visitedHandler)
        {
            ConnectionString = configuration.GetConnectionString("EncountifyAPIContext");
            _visitedHandler = visitedHandler;
        }

        /// <summary>
        /// Get all visited locations
        /// </summary>
        [HttpGet]
        public IEnumerable<VisitedLocation> GetAllVisitedLocations()
        {
            return _visitedHandler.ExecuteVisitedLocationReader(ConnectionString, "SELECT * FROM VisitedLocations");
        }

        /// <summary>
        /// Get visited location by Id
        /// </summary>
        [HttpGet("{id}")]
        public IEnumerable<VisitedLocation> GetVisitedLocation(int id)
        {
            return _visitedHandler.ExecuteVisitedLocationReader(ConnectionString, "SELECT * FROM VisitedLocations WHERE Id = @id", id: id);
        }

        /// <summary>
        /// Get user's visited locations
        /// </summary>
        [HttpGet("User/{userId}")]
        public IEnumerable<VisitedLocation> GetUserVisitedLocations(int? userId)
        {
            return _visitedHandler.ExecuteVisitedLocationReader(ConnectionString, "SELECT * FROM VisitedLocations WHERE UserId = @userId", userId: userId);
        }

        /// <summary>
        /// Get user's last visited location
        /// </summary>
        [HttpGet("Last/{userId}")]
        public IEnumerable<VisitedLocation> GetUserLastVisitedLocation(int? userId)
        {
            List<VisitedLocation> visits = _visitedHandler.ExecuteVisitedLocationReader(ConnectionString, "SELECT * FROM VisitedLocations WHERE UserId = @userId", userId: userId);
            yield return visits.LastOrDefault();
        }

        /// <summary>
        /// Get user's last few visited location
        /// </summary>
        [HttpGet("Last/{userId}/{numberOfLocations}")]
        public IEnumerable<VisitedLocation> GetUserLastsVisitedLocation(int? userId, int numberOfLocations)
        {
            List<VisitedLocation> visits = _visitedHandler.ExecuteVisitedLocationReader(ConnectionString, "SELECT * FROM VisitedLocations WHERE UserId = @userId", userId: userId);
            return visits.Skip(visits.Count - numberOfLocations);
        }

        /// <summary>
        /// Get user's first visited location
        /// </summary>
        [HttpGet("First/{userId}")]
        public IEnumerable<VisitedLocation> GetUserFirstVisitedLocation(int? userId)
        {
            List<VisitedLocation> visits = _visitedHandler.ExecuteVisitedLocationReader(ConnectionString, "SELECT * FROM VisitedLocations WHERE UserId = @userId", userId: userId);
            yield return visits.FirstOrDefault();
        }

        /// <summary>
        /// Get user's first few visited location
        /// </summary>
        [HttpGet("First/{userId}/{numberOfLocations}")]
        public IEnumerable<VisitedLocation> GetUserFirstsVisitedLocation(int? userId, int numberOfLocations)
        {
            List<VisitedLocation> visits = _visitedHandler.ExecuteVisitedLocationReader(ConnectionString, "SELECT * FROM VisitedLocations WHERE UserId = @userId", userId: userId);
            return visits.Take(numberOfLocations);
        }

        /// <summary>
        /// Get locations's visitors
        /// </summary>
        [HttpGet("Location/{locationId}")]
        public IEnumerable<VisitedLocation> GetVisitedLocationUsers(int? locationId)
        {
            return _visitedHandler.ExecuteVisitedLocationReader(ConnectionString, "SELECT * FROM VisitedLocations WHERE LocationId = @locationId", locationId: locationId);
        }

        /// <summary>
        /// Add a new visit
        /// </summary>
        [HttpPost]
        public IEnumerable<VisitedLocation> AddVisitedLocation(int userId, int locationId, int? points = 0)
        {
            _visitedHandler.ExecuteVisitedLocationReader(ConnectionString, "INSERT INTO VisitedLocations VALUES (@userId, @locationId, @points)", userId: userId, locationId: locationId, points: points);
            return GetUserLastVisitedLocation(userId);
        }

        /// <summary>
        /// Edit visit data
        /// </summary>
        [HttpPut("{id}")]
        public IEnumerable<VisitedLocation> EditVisitedLocation(int id, int? locationId = null, int? userId = null, int? points = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                if (locationId != null) EditVisitedLocationId(id, locationId);
                if (userId != null) EditVisitedLocationUser(id, userId);
                if (points != null) EditVisitedLocationPoints(id, points);
            }
            return GetVisitedLocation(id);
        }

        /// <summary>
        /// Edit an existing visited location's Id
        /// </summary>
        [HttpPut("{id}/Location")]
        public IEnumerable<VisitedLocation> EditVisitedLocationId(int id, int? locationId)
        {
            _visitedHandler.ExecuteVisitedLocationReader(ConnectionString, "UPDATE VisitedLocations SET LocationId = @locationId WHERE Id = @id", id: id, locationId: locationId);
            return GetVisitedLocation(id);
        }

        /// <summary>
        /// Edit an existing visited location's User Id
        /// </summary>
        [HttpPut("{id}/User")]
        public IEnumerable<VisitedLocation> EditVisitedLocationUser(int id, int? userId)
        {
            _visitedHandler.ExecuteVisitedLocationReader(ConnectionString, "UPDATE VisitedLocations SET UserId = @userId WHERE Id = @userId", id: id, userId: userId);
            return GetVisitedLocation(id);
        }

        /// <summary>
        /// Edit an existing visited location's points
        /// </summary>
        [HttpPut("{id}/Points")]
        public IEnumerable<VisitedLocation> EditVisitedLocationPoints(int id, int? points)
        {
            _visitedHandler.ExecuteVisitedLocationReader(ConnectionString, "UPDATE VisitedLocations SET Points = @points WHERE Id = @id", id: id, points: points);
            return GetVisitedLocation(id);
        }

        /// <summary>
        /// Delete all visited locations
        /// </summary>
        [HttpDelete]
        public void DeleteVisitedLocations()
        {
            _visitedHandler.ExecuteVisitedLocationReader(ConnectionString, "DELETE FROM VisitedLocations");
        }

        /// <summary>
        /// Delete use's visited locations
        /// </summary>
        [HttpDelete("{id}")]
        public IEnumerable<VisitedLocation> DeleteVisitedLocation(int id)
        {
            _visitedHandler.ExecuteVisitedLocationReader(ConnectionString, "DELETE FROM VisitedLocations WHERE Id = @id", id);
            return GetVisitedLocation(id);
        }
    }
}
