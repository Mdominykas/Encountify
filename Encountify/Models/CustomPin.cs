using System.Collections.Generic;
using Xamarin.Forms.Maps;

namespace Encountify.Models
{
    public class CustomPin : Pin
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public Dictionary<string, object> Atts { get; set; }
    }
}
