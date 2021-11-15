using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Encountify.Models
{
    public class VisitedLocations
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LocationId { get; set; }
        public int Points { get; set; }
    }
}
