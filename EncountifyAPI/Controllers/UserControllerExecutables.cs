using System;
using System.Collections.Generic;
using EncountifyAPI.Models;
using EncountifyAPI.Interfaces;
using System.Data.SqlClient;
using System.Data;
using System.Text;

namespace EncountifyAPI.Controllers
{
    public class UserControllerExecutables : IUserHandlerExecutables
    {
        public List<User> ExecuteUserReader(string connectionString, string query, int? id = null, string? username = null, string? password = null, string? email = null, bool? isAdmin = null, byte[] picture = null, int? points = null)
        {
            List<User> users = new List<User>();
            using (var connection = new SqlConnection(connectionString))
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

        public void ExecuteUserQuery(string connectionString, string query, int? id = null, string? username = null, string? password = null, string? email = null, bool? isAdmin = null, byte[] picture = null, int? points = null)
        {
            using (var connection = new SqlConnection(connectionString))
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

        public static User ParseUser(SqlDataReader reader)
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