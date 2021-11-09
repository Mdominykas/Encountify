using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EncountifyAPI.Models;

namespace EncountifyAPI.Data
{
    public class EncountifyAPIContext : DbContext
    {
        public EncountifyAPIContext (DbContextOptions<EncountifyAPIContext> options)
            : base(options)
        {
        }

        public DbSet<EncountifyAPI.Models.User> User { get; set; }

        public DbSet<EncountifyAPI.Models.Location> Location { get; set; }
    }
}
