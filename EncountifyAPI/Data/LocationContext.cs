using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EncountifyAPI.Models;

namespace EncountifyAPI.Data
{
    public class LocationContext : DbContext
    {
        public LocationContext (DbContextOptions<LocationContext> options)
            : base(options)
        {
        }

        public DbSet<EncountifyAPI.Models.Location> Location { get; set; }
    }
}
