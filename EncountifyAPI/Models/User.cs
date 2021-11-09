using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EncountifyAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public string Image { get; set; }
    }
}
