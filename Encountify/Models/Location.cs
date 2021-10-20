using SQLite;

namespace Encountify.Models
{
    public class Location
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string Description { get; set; } = "";

        public double CoordX { get; set; } = 0;

        public double CoordY { get; set; } = 0;

        public int Category { get; set; } = 0;

        public Location() { }

        public Location(string name, string description = "", double coordX = 0.0, double coordY = 0.0, int category = 0)
        {
            Name = name;
            Description = description;
            CoordX = coordX;
            CoordY = coordY;
            Category = category;
        }
    }
}