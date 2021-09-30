﻿using SQLite;

namespace Encountify.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Username { get; set; }

        [MaxLength(25)]

        public string Password { get; set; }

        public string Email { get; set; }
    }
}