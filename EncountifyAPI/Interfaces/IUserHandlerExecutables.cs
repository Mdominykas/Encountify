using EncountifyAPI.Models;
using System.Collections.Generic;

namespace EncountifyAPI.Interfaces
{
    public interface IUserHandlerExecutables
    {
        List<User> ExecuteUserReader(string connectionString, string query, int? id = null, string? username = null, string? password = null, string? email = null, bool? isAdmin = null, byte[] picture = null, int? points = null);
        void ExecuteUserQuery(string connectionString, string query, int? id = null, string? username = null, string? password = null, string? email = null, bool? isAdmin = null, byte[] picture = null, int? points = null);
    }
}