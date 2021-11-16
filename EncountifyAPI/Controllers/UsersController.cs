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
    [Route("API/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly string ConnectionString;

        public UsersController(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("EncountifyAPIContext");
        }

        /// <summary>
        /// Get all users
        /// </summary>
        [HttpGet]
        public IEnumerable<User> GetAllUsers()
        {
            return ExecuteReader("SELECT * FROM Users");
        }

        /// <summary>
        /// Get user with specified id
        /// </summary>
        [HttpGet("Id/{id}")]
        public IEnumerable<User> GetUser(int id)
        {
            return ExecuteReader("SELECT * FROM Users WHERE Id = @id", id: id);
        }

        /// <summary>
        /// Get user with specified username
        /// </summary>
        [HttpGet("Username/{username}")]
        public IEnumerable<User> GetUser(string username)
        {
            return ExecuteReader("SELECT * FROM Users WHERE CONVERT(VARCHAR, Username) = @username", username: username);
        }

        /// <summary>
        /// Add a new user
        /// </summary>
        [HttpPost("")]
        public IEnumerable<User> AddUser(string username, string password, string email, bool isAdmin = false, string image = "")
        {
            ExecuteQuery("INSERT INTO Users VALUES (@username, @password, @email, @isAdmin, @image", username: username, password: password, email: email, isAdmin: isAdmin, image: image);
            return GetUser(username);
        }

        /// <summary>
        /// Edit an existing user
        /// </summary>
        [HttpPut("{id}")]
        public IEnumerable<User> EditUser(int id, string username = "", string password = "", string email = "", bool? isAdmin = null, string image = "")
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                if (username != "") EditUserName(id, username);
                if (password != "") EditUserPassword(id, password);
                if (email != "") EditUserEmail(id, email);
                if (isAdmin != null) EditUserIsAdmin(id, (bool)isAdmin);
                if (image != "") EditUserImage(id, image);
            }
            return GetUser(id);
        }


        /// <summary>
        /// Edit an existing user's username
        /// </summary>
        [HttpPut("{id}/Username")]
        public IEnumerable<User> EditUserName(int id, string username)
        {
            ExecuteQuery("UPDATE Users SET Username = @username WHERE Id = @id", id: id, username: username);
            return GetUser(id);
        }

        /// <summary>
        /// Edit an existing user's password
        /// </summary>
        [HttpPut("{id}/Password")]
        public IEnumerable<User> EditUserPassword(int id, string password)
        {
            ExecuteQuery("UPDATE Users SET Password = @password WHERE Id = @id", id: id, password: password);
            return GetUser(id);
        }

        /// <summary>
        /// Edit an existing user's email
        /// </summary>
        [HttpPut("{id}/Email")]
        public IEnumerable<User> EditUserEmail(int id, string email)
        {
            ExecuteQuery("UPDATE Users SET Email = @email WHERE Id = @id", id: id, email: email);
            return GetUser(id);
        }

        /// <summary>
        /// Edit an existing user's permissions
        /// </summary>
        [HttpPut("{id}/IsAdmin")]
        public IEnumerable<User> EditUserIsAdmin(int id, bool isAdmin)
        {
            ExecuteQuery("UPDATE Users SET IsAdmin = @isAdmin WHERE Id = @id", id: id, isAdmin: isAdmin);
            return GetUser(id);
        }

        /// <summary>
        /// Edit an existing user's image
        /// </summary>
        [HttpPut("{id}/Image")]
        public IEnumerable<User> EditUserImage(int id, string image)
        {
            ExecuteQuery("UPDATE Users SET Image = @image WHERE Id = @id", id: id, image: image);
            return GetUser(id);
        }

        /// <summary>
        /// Delete all users
        /// </summary>
        [HttpDelete]
        public IEnumerable<User> DeleteAllUsers()
        {
            ExecuteQuery("DELETE FROM Users");
            return GetAllUsers();
        }

        /// <summary>
        /// Delete user with specified Id
        /// </summary>
        [HttpDelete("{id}")]
        public IEnumerable<User> DeleteUser(int id)
        {
            ExecuteQuery("DELETE FROM Users WHERE Id = @id", id);
            return GetUser(id);
        }

        private List<User> ExecuteReader(string query, int id = -1, string username = "", string password = null, string email = null, bool? isAdmin = null, string image = null)
        {
            List<User> users = new List<User>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using SqlCommand command = new SqlCommand(query, connection);

                if (id != -1) command.Parameters.AddWithValue("@id", id);
                if (username != null) command.Parameters.AddWithValue("@username", username);
                if (password != null) command.Parameters.AddWithValue("@password", password);
                if (email != null) command.Parameters.AddWithValue("@email", email);
                if (isAdmin != null) command.Parameters.AddWithValue("@isAdmin", isAdmin);
                if (image != null) command.Parameters.AddWithValue("@image", image);

                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(ParseUser(reader));
                }
            }
            return users;
        }

        private void ExecuteQuery(string query, int id = -1, string username = "", string password = null, string email = null, bool? isAdmin = null, string image = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using SqlCommand command = new SqlCommand(query, connection);

                if (id != -1) command.Parameters.AddWithValue("@id", id);
                if (username != null) command.Parameters.AddWithValue("@username", username);
                if (password != null) command.Parameters.AddWithValue("@password", password);
                if (email != null) command.Parameters.AddWithValue("@email", email);
                if (isAdmin != null) command.Parameters.AddWithValue("@isAdmin", isAdmin);
                if (image != null) command.Parameters.AddWithValue("@image", image);

                command.ExecuteNonQuery();
            }
        }


        private static User ParseUser(SqlDataReader reader)
        {
            User user = new User()
            {
                Id = (int)reader["Id"],
                Username = reader["Username"].ToString(),
                Email = reader["Email"].ToString(),
                Password = reader["Password"].ToString(),
                IsAdmin = Convert.ToBoolean(reader["IsAdmin"]),
                Image = reader["Image"].ToString(),
            };
            return user;
        }
    }
}
