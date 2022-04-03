using EncountifyAPI.Interfaces;
using EncountifyAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace EncountifyAPI.Controllers
{
    [Route("API/[controller]")]
    [ApiController]
    public class AchievmentsController : ControllerBase
    {
        private readonly IAchievmentExecutables _achievmentHandling;
        private readonly string _connectionString;

        public AchievmentsController(IConfiguration configuration, IAchievmentExecutables achievmentHandling)
        {
            _connectionString = configuration.GetConnectionString("EncountifyAPIContext");
            _achievmentHandling = achievmentHandling;
        }

        /// <summary>
        /// Get all achievments
        /// </summary>
        [HttpGet]
        public IEnumerable<Achievment> GetAllAchievments()
        {
            return _achievmentHandling.ExecuteAchievmentReader(_connectionString, "SELECT * FROM Achievments");
        }

        /// <summary>
        /// Get achievment with specified id
        /// </summary>
        [HttpGet("Id/{id}")]
        public IEnumerable<Achievment> GetAchievment(int id)
        {
            List<Achievment> achievment = _achievmentHandling.ExecuteAchievmentReader(_connectionString, "SELECT * FROM Achievments WHERE Id = @id", id: id);
            yield return achievment.FirstOrDefault();
        }

        /// <summary>
        /// Get achievment with specified achievment name
        /// </summary>
        [HttpGet("Name/{achievmentname}")]
        public IEnumerable<Achievment> GetAchievment(string name)
        {
            List<Achievment> achievment = _achievmentHandling.ExecuteAchievmentReader(_connectionString, "SELECT * FROM Achievments WHERE CONVERT(VARCHAR, Name) = @name", name: name);
            yield return achievment.FirstOrDefault();
        }

        /// <summary>
        /// Add a new achievment
        /// </summary>
        [HttpPost]
        public IEnumerable<Achievment> AddAchievment(string name, string description = "", int category = 0, string image = "")
        {
            _achievmentHandling.ExecuteAchievmentQuery(_connectionString, "INSERT INTO Achievments VALUES (@name, @description, @category, @image)", name: name, description: description, category: category, image: image);
            return GetAchievment(name);
        }

        /// <summary>
        /// Edit an existing achievment
        /// </summary>
        [HttpPut("{id}")]
        public IEnumerable<Achievment> EditAchievment(int id, string? name = null, string? description = null, int? category = null, string? image = null)
        {
            if (name != null) EditAchievmentName(id, name);
            if (description != null) EditAchievmentDescription(id, description);
            if (category != null) EditAchievmentCategory(id, category ?? default(int));
            if (image != null) EditAchievmentImage(id, image);

            return GetAchievment(id);
        }


        /// <summary>
        /// Edit an existing achievment's name
        /// </summary>
        [HttpPut("{id}/Name")]
        public IEnumerable<Achievment> EditAchievmentName(int id, string name)
        {
            _achievmentHandling.ExecuteAchievmentQuery(_connectionString, "UPDATE Achievments SET Name = @name WHERE Id = @id", id: id, name: name);
            return GetAchievment(id);
        }

        /// <summary>
        /// Edit an existing achievment's description
        /// </summary>
        [HttpPut("{id}/Description")]
        public IEnumerable<Achievment> EditAchievmentDescription(int id, string description)
        {
            _achievmentHandling.ExecuteAchievmentQuery(_connectionString, "UPDATE Achievments SET Description = @description WHERE Id = @id", id: id, description: description);
            return GetAchievment(id);
        }

        /// <summary>
        /// Edit an existing achievment's category
        /// </summary>
        [HttpPut("{id}/Category")]
        public IEnumerable<Achievment> EditAchievmentCategory(int id, int category)
        {
            _achievmentHandling.ExecuteAchievmentQuery(_connectionString, "UPDATE Achievments SET Category = @category WHERE Id = @id", id: id, category: category);
            return GetAchievment(id);
        }

        /// <summary>
        /// Edit an existing achievment's image
        /// </summary>
        [HttpPut("{id}/Image")]
        public IEnumerable<Achievment> EditAchievmentImage(int id, string image)
        {
            _achievmentHandling.ExecuteAchievmentQuery(_connectionString, "UPDATE Achievments SET Image = @image WHERE Id = @id", id: id, image: image);
            return GetAchievment(id);
        }

        /// <summary>
        /// Delete all achievment
        /// </summary>
        [HttpDelete]
        public IEnumerable<Achievment> DeleteAllAchievments()
        {
            _achievmentHandling.ExecuteAchievmentQuery(_connectionString, "DELETE FROM Achievments");
            return GetAllAchievments();
        }

        /// <summary>
        /// Delete achievment with specified Id
        /// </summary>
        [HttpDelete("{id}")]
        public IEnumerable<Achievment> DeleteAchievment(int id)
        {
            _achievmentHandling.ExecuteAchievmentQuery(_connectionString, "DELETE FROM Achievments WHERE Id = @id", id);
            return GetAchievment(id);
        }
    }
}
