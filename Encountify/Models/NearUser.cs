namespace Encountify.Models
{
    public class NearUser
    {

        public int LocationId { get; set; }        
        public string LocationName { get; set; }
        public double Distance { get; set; }
        public string FormattedDistance { get; set; }
        public int Points { get; set; }
    }
}
