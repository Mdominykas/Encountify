using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Encountify.Models
{
    public class Achievment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Category { get; set; }
        public string Image { get; set; }
    }
}
