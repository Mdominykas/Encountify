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
    public class UsersController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly IUserHandlerExecutables _userHandling;

        public UsersController(IConfiguration configuration, IUserHandlerExecutables userHandling)
        {
            _connectionString = configuration.GetConnectionString("EncountifyAPIContext");
            _userHandling = userHandling;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        [HttpGet]
        public IEnumerable<User> GetAllUsers()
        {
            return _userHandling.ExecuteUserReader(_connectionString, "SELECT * FROM Users");
        }

        /// <summary>
        /// Get user with specified id
        /// </summary>
        [HttpGet("Id/{id}")]
        public IEnumerable<User> GetUser(int id)
        {
            List<User> users = _userHandling.ExecuteUserReader(_connectionString, "SELECT * FROM Users WHERE Id = @id", id: id);
            yield return users.FirstOrDefault();
        }

        /// <summary>
        /// Get user with specified username
        /// </summary>
        [HttpGet("Username/{username}")]
        public IEnumerable<User> GetUser(string username)
        {
            List<User> users = _userHandling.ExecuteUserReader(_connectionString, "SELECT * FROM Users WHERE CONVERT(VARCHAR, Username) = @username", username: username);
            yield return users.FirstOrDefault();
        }

        /// <summary>
        /// Add a new user
        /// </summary>
        [HttpPost]
        public IEnumerable<User> AddUser(string username, string password, string email)
        {
            _userHandling.ExecuteUserReader(_connectionString, "INSERT INTO Users VALUES (@username, @password, @email, 0, NULL, CURRENT_TIMESTAMP, 0)", username: username, password: password, email: email);
            return GetUser(username);
        }

        /// <summary>
        /// Edit an existing user
        /// </summary>
        [HttpPut("{id}")]
        public IEnumerable<User> EditUser(int id, string? username = null, string? password = null, string? email = null)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                if (username != null) EditUserName(id, username);
                if (password != null) EditUserPassword(id, password);
                if (email != null) EditUserEmail(id, email);
            }
            return GetUser(id);
        }


        /// <summary>
        /// Edit an existing user's username
        /// </summary>
        [HttpPut("{id}/Username")]
        public IEnumerable<User> EditUserName(int id, string username)
        {
            _userHandling.ExecuteUserReader(_connectionString, "UPDATE Users SET Username = @username WHERE Id = @id", id: id, username: username);
            return GetUser(id);
        }

        /// <summary>
        /// Edit an existing user's password
        /// </summary>
        [HttpPut("{id}/Password")]
        public IEnumerable<User> EditUserPassword(int id, string password)
        {
            _userHandling.ExecuteUserReader(_connectionString, "UPDATE Users SET Password = @password WHERE Id = @id", id: id, password: password);
            return GetUser(id);
        }

        /// <summary>
        /// Edit an existing user's email
        /// </summary>
        [HttpPut("{id}/Email")]
        public IEnumerable<User> EditUserEmail(int id, string email)
        {
            _userHandling.ExecuteUserReader(_connectionString, "UPDATE Users SET Email = @email WHERE Id = @id", id: id, email: email);
            return GetUser(id);
        }

        /// <summary>
        /// Edit an existing user's permissions
        /// </summary>
        [HttpPut("{id}/IsAdmin")]
        public IEnumerable<User> EditUserIsAdmin(int id, bool isAdmin)
        {
            _userHandling.ExecuteUserReader(_connectionString, "UPDATE Users SET IsAdmin = @isAdmin WHERE Id = @id", id: id, isAdmin: isAdmin);
            return GetUser(id);
        }

        /// <summary>
        /// Edit an existing user's picture
        /// </summary>
        [HttpPut("{id}/Picture")]
        public IEnumerable<User> EditUserPicture(int id, byte[] picture)
        {
            _userHandling.ExecuteUserReader(_connectionString, "UPDATE Users SET Picture = @picture WHERE Id = @id", id: id, picture: picture);
            return GetUser(id);
        }

        /// <summary>
        /// Delete all users
        /// </summary>
        [HttpDelete]
        public void DeleteAllUsers()
        {
            _userHandling.ExecuteUserReader(_connectionString, "DELETE FROM Users");
        }

        /// <summary>
        /// Delete user with specified Id
        /// </summary>
        [HttpDelete("{id}")]
        public IEnumerable<User> DeleteUser(int id)
        {
            _userHandling.ExecuteUserReader(_connectionString, "DELETE FROM Users WHERE Id = @id", id);
            return GetUser(id);
        }
    }
}
