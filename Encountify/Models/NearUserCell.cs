using System;
using System.Collections.Generic;
using System.Text;

namespace Encountify.Models
{
    public class NearUserCell
    {
        public int LocationId { get; set; } 
        public string LocationName { get; set; } 
        public double Distance { get; set; }

        public NearUserCell (NearUser itiem)
        {
            LocationId = itiem.LocationId;
            LocationName = itiem.LocationName;
            Distance = itiem.Distance;
        }
    }
}
