namespace Encountify.Models
{
    public class NearUserCell
    {
        public int LocationId { get; set; } 
        public string LocationName { get; set; } 
        public string Distance { get; set; }
        public string Points { get; set; }

        public NearUserCell (NearUser item)
        {
            LocationId = item.LocationId;
            LocationName = item.LocationName;
            Distance = item.FormattedDistance + " away";
            Points = item.Points + " points✨";
        }
    }
}
