using EncountifyAPI.Interfaces;
using EncountifyAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace EncountifyAPI.Controllers
{
    public class AchievmentControllerExecutables : IAchievmentExecutables
    {
        public List<Achievment> ExecuteAchievmentReader(string connectionString, string query, int? id = null, string name = null, string description = null, int? category = null, string image = null)
        {
            List<Achievment> achievments = new List<Achievment>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using SqlCommand command = new SqlCommand(query, connection);

                if (id != null) command.Parameters.AddWithValue("@id", id ?? default(int));
                if (name != null) command.Parameters.AddWithValue("@name", name ?? default(string));
                if (description != null) command.Parameters.AddWithValue("@description", description ?? default(string));
                if (category != null) command.Parameters.AddWithValue("@category", category ?? default(int));
                if (image != null) command.Parameters.AddWithValue("@image", image ?? default(string));

                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    achievments.Add(ParseAchievment(reader));
                }
                connection.Close();
            }
            return achievments;
        }

        public void ExecuteAchievmentQuery(string connectionString, string query, int? id = null, string name = null, string description = null, int? category = null, string image = null)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using SqlCommand command = new SqlCommand(query, connection);

                if (id != null) command.Parameters.AddWithValue("@id", id ?? default(int));
                if (name != null) command.Parameters.AddWithValue("@name", name ?? default(string));
                if (description != null) command.Parameters.AddWithValue("@description", description ?? default(string));
                if (category != null) command.Parameters.AddWithValue("@category", category ?? default(int));
                if (image != null) command.Parameters.AddWithValue("@image", image ?? default(string));

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static Achievment ParseAchievment(SqlDataReader reader)
        {
            Achievment achievment = new Achievment()
            {
                Id = (int)reader["Id"],
                Name = reader["Name"].ToString(),
                Description = reader["Description"].ToString(),
                Category = (int)reader["Category"],
                Image = reader["Image"].ToString(),
            };
            return achievment;
        }
    }
}
