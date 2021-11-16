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
using System.Text;
using System.Data;

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
            return ExecuteUserReader("SELECT * FROM Users");
        }

        /// <summary>
        /// Get user with specified id
        /// </summary>
        [HttpGet("Id/{id}")]
        public IEnumerable<User> GetUser(int id)
        {
            List<User> users = ExecuteUserReader("SELECT * FROM Users WHERE Id = @id", id: id);
            yield return users.FirstOrDefault();
        }

        /// <summary>
        /// Get user with specified username
        /// </summary>
        [HttpGet("Username/{username}")]
        public IEnumerable<User> GetUser(string username)
        {
            List<User> users = ExecuteUserReader("SELECT * FROM Users WHERE CONVERT(VARCHAR, Username) = @username", username: username);
            yield return users.FirstOrDefault();
        }

        /// <summary>
        /// Add a new user
        /// </summary>
        [HttpPost("")]
        public IEnumerable<User> AddUser(string username, string password, string email)
        {
            ExecuteUserQuery("INSERT INTO Users VALUES (@username, @password, @email, 0, NULL, CURRENT_TIMESTAMP, 0)", username: username, password: password, email: email);
            return GetUser(username);
        }

        /// <summary>
        /// Edit an existing user
        /// </summary>
        [HttpPut("{id}")]
        public IEnumerable<User> EditUser(int id, string? username = null, string? password = null, string? email = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
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
            ExecuteUserQuery("UPDATE Users SET Username = @username WHERE Id = @id", id: id, username: username);
            return GetUser(id);
        }

        /// <summary>
        /// Edit an existing user's password
        /// </summary>
        [HttpPut("{id}/Password")]
        public IEnumerable<User> EditUserPassword(int id, string password)
        {
            ExecuteUserQuery("UPDATE Users SET Password = @password WHERE Id = @id", id: id, password: password);
            return GetUser(id);
        }

        /// <summary>
        /// Edit an existing user's email
        /// </summary>
        [HttpPut("{id}/Email")]
        public IEnumerable<User> EditUserEmail(int id, string email)
        {
            ExecuteUserQuery("UPDATE Users SET Email = @email WHERE Id = @id", id: id, email: email);
            return GetUser(id);
        }

        /// <summary>
        /// Edit an existing user's permissions
        /// </summary>
        [HttpPut("{id}/IsAdmin")]
        public IEnumerable<User> EditUserIsAdmin(int id, bool isAdmin)
        {
            ExecuteUserQuery("UPDATE Users SET IsAdmin = @isAdmin WHERE Id = @id", id: id, isAdmin: isAdmin);
            return GetUser(id);
        }

        /// <summary>
        /// Edit an existing user's picture
        /// </summary>
        [HttpPut("{id}/Picture")]
        public IEnumerable<User> EditUserPicture(int id, byte[] picture)
        {
            ExecuteUserQuery("UPDATE Users SET Picture = @picture WHERE Id = @id", id: id, picture: picture);
            return GetUser(id);
        }

        /// <summary>
        /// Delete all users
        /// </summary>
        [HttpDelete]
        public void DeleteAllUsers()
        {
            ExecuteUserQuery("DELETE FROM Users");
        }

        /// <summary>
        /// Delete user with specified Id
        /// </summary>
        [HttpDelete("{id}")]
        public IEnumerable<User> DeleteUser(int id)
        {
            ExecuteUserQuery("DELETE FROM Users WHERE Id = @id", id);
            return GetUser(id);
        }

        private List<User> ExecuteUserReader(string query, int? id = null, string? username = null, string? password = null, string? email = null, bool? isAdmin = null, byte[] picture = null, int? points = null)
        {
            List<User> users = new List<User>();
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using SqlCommand command = new SqlCommand(query, connection);

                if (id != null) command.Parameters.AddWithValue("@id", id ?? default(int));
                if (username != null) command.Parameters.AddWithValue("@username", username ?? default(string));
                if (password != null) command.Parameters.AddWithValue("@password", password ?? default(string));
                if (email != null) command.Parameters.AddWithValue("@email", email ?? default(string));
                if (isAdmin != null) command.Parameters.AddWithValue("@isAdmin", isAdmin ?? default(bool));
                if (picture?.Length > 0) command.Parameters.Add("@picture", SqlDbType.Image).Value = picture;
                if (points != null) command.Parameters.AddWithValue("@points", points ?? default(int));

                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(ParseUser(reader));
                }
            }
            return users;
        }

        private void ExecuteUserQuery(string query, int? id = null, string? username = null, string? password = null, string? email = null, bool? isAdmin = null, byte[] picture = null, int? points = null)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using SqlCommand command = new SqlCommand(query, connection);

                if (id != null) command.Parameters.AddWithValue("@id", id ?? default(int));
                if (username != null) command.Parameters.AddWithValue("@username", username ?? default(string));
                if (password != null) command.Parameters.AddWithValue("@password", password ?? default(string));
                if (email != null) command.Parameters.AddWithValue("@email", email ?? default(string));
                if (isAdmin != null) command.Parameters.AddWithValue("@isAdmin", isAdmin ?? default(bool));
                if (picture?.Length > 0) command.Parameters.Add("@picture", SqlDbType.Image).Value = picture;
                if (points != null) command.Parameters.AddWithValue("@points", points ?? default(int));

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
                Picture = Encoding.ASCII.GetBytes(reader["Picture"].ToString()),
                DateCreated = (DateTime)reader["DateCreated"],
                Points = (int)reader["Points"]
            };
            return user;
        }
    }
}
