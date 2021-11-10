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
        private readonly string ConnectionString;

        public UsersController(IConfiguration configuration)
        {
            ConnectionString = configuration.GetConnectionString("EncountifyAPIContext");
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users";
                using SqlCommand command = new SqlCommand(query, connection);
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(ParseUser(reader));
                }
            }
            return users;
        }

        /// <summary>
        /// Get user with specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("id/{id}")]
        public User GetUser(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE Id = @id";
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                using SqlDataReader reader = command.ExecuteReader();
                return ParseUser(reader);
            }
        }

        /// <summary>
        /// Get user with specified username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("username/{username}")]
        public User GetUser(string username)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Users WHERE CONVERT(VARCHAR, Username) = @username";
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", username);
                using SqlDataReader reader = command.ExecuteReader();
                return ParseUser(reader);
            }
        }

        /// <summary>
        /// Add a new user
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="isAdmin"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpPost("")]
        public User AddUser(string username, string password, string email, bool isAdmin = false, string image = "")
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "INSERT INTO Users VALUES (@username, @password, @email, @isAdmin, @image)";
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@isAdmin", isAdmin ? 1 : 0);
                command.Parameters.AddWithValue("@image", image);
                command.ExecuteNonQuery();                
            }
            return GetUser(username);
        }

        /// <summary>
        /// Edit an existing user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="isAdmin"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public User EditUser(int id, string username = "", string password = "", string email = "", bool? isAdmin = null, string image = "")
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                if (username != "")
                {
                    User user = EditUserName(id, username);
                }
                if (password != "")
                {
                    User user = EditUserPassword(id, password);
                }
                if (email != "")
                {
                    User user = EditUserEmail(id, email);
                }
                if (isAdmin != null)
                {
                    User user = EditUserIsAdmin(id, (bool)isAdmin);
                }
                if (image != "")
                {
                    User user = EditUserImage(id, image);
                }
            }
            return GetUser(id);
        }


        /// <summary>
        /// Edit an existing user's username
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPut("{id}/username")]
        public User EditUserName(int id, string username)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE Users SET Username = @username WHERE Id = @id";
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@username", username);
                command.ExecuteNonQuery();
            }
            return GetUser(id);
        }

        /// <summary>
        /// Edit an existing user's password
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPut("{id}/password")]
        public User EditUserPassword(int id, string password)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE Users SET Password = @password WHERE Id = @id";
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@password", password);
                command.ExecuteNonQuery();
            }
            return GetUser(id);
        }

        /// <summary>
        /// Edit an existing user's email
        /// </summary>
        /// <param name="id"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPut("{id}/email")]
        public User EditUserEmail(int id, string email)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE Users SET Email = @email WHERE Id = @id";
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@email", email);
                command.ExecuteNonQuery();
            }
            return GetUser(id);
        }

        /// <summary>
        /// Edit an existing user's permissions
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        [HttpPut("{id}/isAdmin")]
        public User EditUserIsAdmin(int id, bool isAdmin)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "UPDATE Users SET IsAdmin = @isAdmin WHERE Id = @id";
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@isAdmin", isAdmin);
                command.ExecuteNonQuery();
            }
            return GetUser(id);
        }

        /// <summary>
        /// Edit an existing user's image
        /// </summary>
        /// <param name="id"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpPut("{id}/image")]
        public User EditUserImage(int id, string image)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();                
                string query = "UPDATE Users SET Image = @image WHERE Id = @id";
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@image", image);
                command.ExecuteNonQuery();
            }
            return GetUser(id);
        }

        /// <summary>
        /// Delete all users
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public IEnumerable<User> DeleteAllUsers()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "DELETE FROM Users";
                using SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
            return GetAllUsers();
        }

        /// <summary>
        /// Delete user with specified Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public User DeleteUser(int id)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                string query = "DELETE FROM Users WHERE Id = @id";
                using SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
            return GetUser(id);
        }


        private User ParseUser(SqlDataReader reader)
        {
            User user = new User()
            {
                Id = (int)reader["Id"],
                Username = reader["Username"].ToString(),
                Email = reader["Email"].ToString(),
                Password = reader["Password"].ToString(),
                IsAdmin = Convert.ToBoolean((int)reader["IsAdmin"]),
                Image = reader["Image"].ToString(),
            };
            return user;
        }
    }
}
