using System;
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
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LocationsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IEnumerable<Location> Get()
        {
            var locations = GetLocations();
            return locations;
        }

        private IEnumerable<Location> GetLocations()
        {
            var locations = new List<Location>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("EncountifyAPIContext")))
            {
                connection.Open();
                using SqlCommand command = new SqlCommand("SELECT Id, Name, Description, Longitude, Latitude, Category, Image FROM Locations", connection);
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var location = new Location()
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"].ToString(),
                        Description = reader["Description"].ToString(),
                        Longitude = (float)reader["Longitude"],
                        Latitude = (float)reader["Latitude"],
                        Category = (int)reader["Category"],
                        Image = reader["Image"].ToString(),
                    };
                    locations.Add(location);
                }
            }
            return locations;
        }
    }
}
