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
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IEnumerable<User> Get()
        {
            var users = GetUsers();
            return users;
        }

        private IEnumerable<User> GetUsers()
        {
            var users = new List<User>();
            using (var connection = new SqlConnection(_configuration.GetConnectionString("EncountifyAPIContext")))
            {
                connection.Open();
                using SqlCommand command = new SqlCommand("SELECT Id, Username, Email, Password, IsAdmin, Image FROM Users", connection);
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var user = new User()
                    {
                        Id = (int)reader["Id"],
                        Username = reader["Username"].ToString(),
                        Email = reader["Email"].ToString(),
                        Password = reader["Password"].ToString(),
                        IsAdmin = Convert.ToBoolean((int)reader["IsAdmin"]),
                        Image = reader["Image"].ToString(),
                    };
                    users.Add(user);
                }
            }
            return users;
        }
    }
}
