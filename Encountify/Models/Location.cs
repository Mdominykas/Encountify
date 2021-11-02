using SQLite;

namespace Encountify.Models
{
    public class Location
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public double Longitude { get; set; } = 0;

        public double Lattitude { get; set; } = 0;

        public int Category { get; set; } = 0;

        public Location() { }

        public Location(string name, string description = "", double longitude = 0.0, double lattitude = 0.0, int category = 0)
        {
            Name = name;
            Description = description;
            Longitude = longitude;
            Lattitude = lattitude;
            Category = category;
        }
    }
}