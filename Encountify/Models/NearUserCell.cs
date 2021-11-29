namespace Encountify.Models
{
    public class NearUserCell
    {
        public int LocationId { get; set; } 
        public string LocationName { get; set; } 
        public string Distance { get; set; }
        public string Points { get; set; }

        public NearUserCell (NearUser itiem)
        {
            LocationId = itiem.LocationId;
            LocationName = itiem.LocationName;
            Distance = itiem.FormattedDistance + " away";
            Points = itiem.Points + "points✨";
        }
    }
}
