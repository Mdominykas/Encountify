using SQLite;

namespace Encountify.Models
{
    public class Location
    {
        public int Id { get; set; }

        [MaxLength(30), Unique]

        public string Name { get; set; }

        public string Description { get; set; }

        public double CoordX { get; set; }

        public double CoordY { get; set; }
    }
}