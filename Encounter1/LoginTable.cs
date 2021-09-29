using SQLite;

namespace Encounter1
{
    public class LoginTable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Username { get; set; }

        [MaxLength(25)]

        public string Password { get; set; }

        public string Email { get; set; }
    }
}