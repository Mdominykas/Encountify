using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Encountify.Models
{
    public class AssignedAchievment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AchievmentId { get; set; }
    }
}
