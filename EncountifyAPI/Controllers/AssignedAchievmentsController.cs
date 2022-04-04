using EncountifyAPI.Interfaces;
using EncountifyAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EncountifyAPI.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class AssignedAchievmentsController : ControllerBase
    {
        private readonly string ConnectionString;
        private readonly IAssignedAchievmentExecutables _assignedHandler;

        public AssignedAchievmentsController(IConfiguration configuration, IAssignedAchievmentExecutables assignedHandler)
        {
            ConnectionString = configuration.GetConnectionString("EncountifyAPIContext");
            _assignedHandler = assignedHandler;
        }

        /// <summary>
        /// Get all visited locations
        /// </summary>
        [HttpGet]
        public IEnumerable<AssignedAchievment> GetAllAssignedAchievments()
        {
            return _assignedHandler.ExecuteAssignedAchievmentReader(ConnectionString, "SELECT * FROM AssignedAchievments");
        }

        /// <summary>
        /// Get visited location by Id
        /// </summary>
        [HttpGet("{id}")]
        public IEnumerable<AssignedAchievment> GetAssignedAchievment(int id)
        {
            return _assignedHandler.ExecuteAssignedAchievmentReader(ConnectionString, "SELECT * FROM AssignedAchievments WHERE Id = @id", id: id);
        }

        /// <summary>
        /// Get user's visited locations
        /// </summary>
        [HttpGet("User/{userId}")]
        public IEnumerable<AssignedAchievment> GetUserAssignedAchievments(int? userId)
        {
            return _assignedHandler.ExecuteAssignedAchievmentReader(ConnectionString, "SELECT * FROM AssignedAchievments WHERE UserId = @userId", userId: userId);
        }

        /// <summary>
        /// Get user's last visited location
        /// </summary>
        [HttpGet("Last/{userId}")]
        public IEnumerable<AssignedAchievment> GetUserLastAssignedAchievment(int? userId)
        {
            List<AssignedAchievment> achievments = _assignedHandler.ExecuteAssignedAchievmentReader(ConnectionString, "SELECT * FROM AssignedAchievments WHERE UserId = @userId", userId: userId);
            yield return achievments.LastOrDefault();
        }

        /// <summary>
        /// Get locations's visitors
        /// </summary>
        [HttpGet("Achievment/{achievmentId}")]
        public IEnumerable<AssignedAchievment> GetAssignedAchievmentsUsers(int? achievmentId)
        {
            return _assignedHandler.ExecuteAssignedAchievmentReader(ConnectionString, "SELECT * FROM AssignedAchievments WHERE AchievmentId = @achievmentId", achievmentId: achievmentId);
        }

        /// <summary>
        /// Add a new visit
        /// </summary>
        [HttpPost]
        public IEnumerable<AssignedAchievment> AddAssignedAchievment(int userId, int achievmentId)
        {
            _assignedHandler.ExecuteAssignedAchievmentReader(ConnectionString, "INSERT INTO AssignedAchievments VALUES (@userId, @achievmentId)", userId: userId, achievmentId: achievmentId);
            return GetUserLastAssignedAchievment(userId);
        }

        /// <summary>
        /// Edit visit data
        /// </summary>
        [HttpPut("{id}")]
        public IEnumerable<AssignedAchievment> EditAssignedAchievment(int id, int? achievmentId = null, int? userId = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                if (achievmentId != null) EditAssignedAchievmentId(id, achievmentId);
                if (userId != null) EditAssignedAchievmentUser(id, userId);
            }
            return GetAssignedAchievment(id);
        }

        /// <summary>
        /// Edit an existing visited location's Id
        /// </summary>
        [HttpPut("{id}/Achievment")]
        public IEnumerable<AssignedAchievment> EditAssignedAchievmentId(int id, int? achievmentId)
        {
            _assignedHandler.ExecuteAssignedAchievmentReader(ConnectionString, "UPDATE AssignedAchievments SET AchievmentId = @achievmentId WHERE Id = @id", id: id, achievmentId: achievmentId);
            return GetAssignedAchievment(id);
        }

        /// <summary>
        /// Edit an existing visited location's User Id
        /// </summary>
        [HttpPut("{id}/User")]
        public IEnumerable<AssignedAchievment> EditAssignedAchievmentUser(int id, int? userId)
        {
            _assignedHandler.ExecuteAssignedAchievmentReader(ConnectionString, "UPDATE AssignedAchievments SET UserId = @userId WHERE Id = @id", id: id, userId: userId);
            return GetAssignedAchievment(id);
        }

        /// <summary>
        /// Delete all visited locations
        /// </summary>
        [HttpDelete]
        public void DeleteAssignedAchievment()
        {
            _assignedHandler.ExecuteAssignedAchievmentReader(ConnectionString, "DELETE FROM AssignedAchievments");
        }

        /// <summary>
        /// Delete use's visited locations
        /// </summary>
        [HttpDelete("{id}")]
        public IEnumerable<AssignedAchievment> DeleteAssignedAchievment(int id)
        {
            _assignedHandler.ExecuteAssignedAchievmentReader(ConnectionString, "DELETE FROM AssignedAchievments WHERE Id = @id", id);
            return GetAssignedAchievment(id);
        }
    }
}
