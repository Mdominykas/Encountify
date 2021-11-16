using SQLite;

namespace Encountify.Models
{
    public class Location
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public double Latitude { get; set; } = 0;

        public double Longitude { get; set; } = 0;

        public int Category { get; set; } = 0;

        public Location() { }

        public Location(string name, string description = "", double latitude = 0.0,  double longitude = 0.0, int category = 0)
        {
            Name = name;
            Description = description;
            Latitude = latitude;
            Longitude = longitude;
            Category = category;
        }
    }
}