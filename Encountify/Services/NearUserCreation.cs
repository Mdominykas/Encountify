using Encountify.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Locations = Xamarin.Essentials.Location;

namespace Encountify.Services
{
    class NearUserCreation
    {
        public class Distance
        {
            public int LocationId { get; set; }
            public string Name { get; set; }
            public double LocationDistance { get; set; }
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
                    Name = itiem.Name,
                    Latitude = itiem.Latitude,
                    Longitude = itiem.Longitude,
                    LocationDistance = 0
                }).ToList();
            }

            foreach (var location in distances)
            {
                var distance = await DistanceCounter.GetFormattedDistance(new Locations(location.Latitude, location.Longitude));
                if (!distance.Equals("Could not get distance"))
                {
                    Debug.WriteLine("Pradzia " + distance + location.Name);
                    
                    var _distance = string.Concat(distance.Split(remove.ToArray()));
                    
                    Debug.WriteLine("Tada " + _distance + location.Name);
                    double _dist, dist;
                    if (Double.TryParse(_distance, out _dist))
                    {
                        dist = _dist;
                        Debug.WriteLine("Pakeista " + dist);

                        location.LocationDistance = dist;

                        Debug.WriteLine("Sudeta " + location.LocationId + location.Name + dist);
                    }
                }
            }

            foreach (var itiem in distances)
            {
                Debug.WriteLine("Vietos " + itiem.LocationId + itiem.Name + itiem.LocationDistance);
            }

            var query = distances.Join(
                            locations,
                            distance => distance.LocationId,
                            location => location.Id,
                            (distance, location) => new
                            {
                                Id = distance.LocationId,
                                Name = location.Name,
                                Distance = distance.LocationDistance
                            });

            foreach (var itiem in query)
            {
                Debug.WriteLine("Nariai " + itiem.Id + itiem.Name + itiem.Distance);
            }



            List<NearUser> result = new List<NearUser>();
            double  dis = 3.0;

                foreach(var res in query)
                {
                Debug.WriteLine("Galas" + res.Name + res.Distance);
                if (res.Distance < dis)
                {
                    result.Add(new NearUser()
                    {
                        LocationId = res.Id,
                        LocationName = res.Name,
                        Distance = res.Distance
                    });
                }
                }

                return result;
            }
        }
}
