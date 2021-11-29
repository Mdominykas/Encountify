using Encountify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Locations = Xamarin.Essentials.Location;

namespace Encountify.Services
{
    class NearUserCreation
    {
        public class Distance
        {
            public int LocationId { get; set; }
            public double LocationDistance { get; set; }
            public string FDistance { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }

        public async Task<List<NearUser>> CreateListAsync()
        {
            DatabaseAccess<Location> locationData = new DatabaseAccess<Location>();
            List<Location> locations = (List<Location>)locationData.GetAllAsync().Result;
            
            var distances = new List<Distance>();

            List<Char> remove = new List<char> { 'k', 'm', 'y', 'd', 'i'};

            foreach (var location in locations)
            {
                distances = locations.Select(itiem => new Distance
                {
                    LocationId = itiem.Id,
                    Latitude = itiem.Latitude,
                    Longitude = itiem.Longitude,
                    LocationDistance = 0,
                    FDistance = ""
                }).ToList();
            }

            foreach (var location in distances)
            {
                var distance = await DistanceCounter.GetFormattedDistance(new Locations(location.Latitude, location.Longitude));
                if (!distance.Equals("Could not get distance"))
                {   
                    var _distance = string.Concat(distance.Split(remove.ToArray()));

                    if (Double.TryParse(_distance, out double dist))
                    {
                        location.LocationDistance = dist;
                        location.FDistance = distance;
                    }
                }
            }

            var query = distances.Join(
                            locations,
                            distance => distance.LocationId,
                            location => location.Id,
                            (distance, location) => new
                            {
                                Id = distance.LocationId,
                                Name = location.Name,
                                Distance = distance.LocationDistance,
                                FormattedDistance = distance.FDistance
                            });

            List<NearUser> result = new List<NearUser>();
            
                foreach (var res in query)
                {
                    string end = new string(res.FormattedDistance.Where(c => c != ',' && (c < '0' || c > '9')).ToArray());
                    if (end.Equals(" km") && res.Distance < 3.00 || end.Equals(" m") && res.Distance < 100.00 ||
                        end.Equals(" yd") && res.Distance < 3280 || end.Equals(" mi") && res.Distance < 0.06)
                        {
                            result.Add(new NearUser()
                            {
                                LocationId = res.Id,
                                LocationName = res.Name,
                                Distance = res.Distance,
                                FormattedDistance = res.FormattedDistance,
                                Points = 100
                            });
                        }
                }
                return result;
            }
        }
}
