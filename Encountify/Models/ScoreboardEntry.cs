using System;
using System.Collections.Generic;
using System.Text;

namespace Encountify.Models
{
    public class ScoreboardEntry : IComparable<ScoreboardEntry>
    {
        public string Name { get; set; } = "";
        public int UserId { get; set; }
        public int Score { get; set; } = 0;
        public int CompareTo(ScoreboardEntry other)
        {
            if (this.Score < other.Score)
                return -1;
            else if (this.Score > other.Score)
                return 1;
            else
                return this.Name.CompareTo(other.Name);
        }
    }
}
