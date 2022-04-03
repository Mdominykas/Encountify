using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EncountifyAPI.Models
{
    public class AssignedAchievment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AchievmentId { get; set; }
        public DateTime AssignmentDate { get; set; }
    }
}
