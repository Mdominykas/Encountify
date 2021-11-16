using Encountify.Services;
using SQLite;

namespace Encountify.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public byte[] Picture { get; set; } = ImageCreator.GetDefaultImage();
    }
}