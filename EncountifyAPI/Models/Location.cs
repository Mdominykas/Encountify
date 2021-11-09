using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EncountifyAPI.Models
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int Category { get; set; }
        public string Image { get; set; }
    }
}
