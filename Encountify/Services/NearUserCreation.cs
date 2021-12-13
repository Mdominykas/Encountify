using Encountify.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
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
            ILocationAccess locationData = DependencyService.Get<ILocationAccess>(); ;
            DatabaseAccess<VisitedLocations> visitedLocationData = new DatabaseAccess<VisitedLocations>();

            List<Location> allLocations = (List<Location>)locationData.GetAllAsync().Result;
            List<VisitedLocations> visitedLocations = (List<VisitedLocations>)visitedLocationData.GetAllAsync().Result;

            var distances = new List<Distance>();

            var locations = allLocations.Where(location => !visitedLocations.Exists(location2 => location2.LocationId == location.Id));

            distances = await CreateDistancesList(locations, distances);

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

            var orderedResult = query.OrderBy(location => location.Distance);

            List<NearUser> result = new List<NearUser>();

            foreach (var res in orderedResult)
            {
                if(res.Distance < 3000.00)
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

        public async Task<List<Distance>> CreateDistancesList(IEnumerable<Location> locations, List<Distance> distances)
        {
            foreach (var location in locations)
            {
                distances = locations.Select(item => new Distance
                {
                    LocationId = item.Id,
                    Latitude = item.Latitude,
                    Longitude = item.Longitude,
                    LocationDistance = 0,
                    FDistance = ""
                }).ToList();
            }

            foreach (var location in distances)
            {
                var distance = await DistanceCounter.GetFormattedDistance(new Locations(location.Latitude, location.Longitude));
                if (!distance.Equals("Could not get distance"))
                {
                    var _distance = DistanceCounter.ConvertedToMetersDistance(distance);
                    location.LocationDistance = _distance;
                    location.FDistance = distance;
                }
            }

            return distances;
        }

    }
}