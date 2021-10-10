using Encountify.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Encountify.Services
{
    public class LocationDataStore : IDataStore<Location>
    {
        List<Location> Locations;
        private readonly LocationDatabaseAccess LocationDatabase;

        public LocationDataStore()
        {
            LocationDatabase = new LocationDatabaseAccess();
            Locations = LocationDatabase.GetLocationList();
            LoadDummyData();

        }

        public async Task<bool> AddAsync(Location location)
        {
            bool result = LocationDatabase.AddLocation(location);
            Locations = LocationDatabase.GetLocationList();
            return await Task.FromResult(result);
        }

        public async Task<bool> UpdateAsync(Location location)
        {
            bool result = LocationDatabase.UpdateLocation(location);
            Locations = LocationDatabase.GetLocationList();
            return await Task.FromResult(result);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (Locations.FirstOrDefault(s => s.Id == id) == null)
                return false;
            bool result = LocationDatabase.DeleteLocation(Locations[id]);
            Locations = LocationDatabase.GetLocationList();
            return await Task.FromResult(result);
        }

        public async Task<Location> GetAsync(int id)
        {
            return await Task.FromResult(Locations.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Location>> GetAllAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(LocationDatabase.GetLocationList());
        }

        public List<Location> DummyLocationList()
        {
            return new List<Location>()
            {
                new Location() { Name = "Vilniaus katedra", CoordX = 54.685849042698216, CoordY = 25.287750880122083, Category = "Cathedral" },
                new Location() { Name = "Gedimino bokštas", CoordX = 54.68667445192699, CoordY = 25.29056883194689, Category = "Castle" },
                new Location() { Name = "Vilniaus Šv. Onos bažnyčia", CoordX = 54.68378573230062, CoordY = 25.292650881640785, Category = "Church" },
                new Location() { Name = "Trys kryžiai", CoordX = 54.68740766559662, CoordY = 25.29771489238211, Category = "Monument" },
                new Location() { Name = "Gedimino prospektas", CoordX = 54.68644019450281, CoordY = 25.285441103636185, Category = "Street" },
                new Location() { Name = "Lietuvos Respublikos Prezidento kanceliarija", CoordX = 54.68383535000863, CoordY = 25.286685648648888, Category = "Tourist trap" },
                new Location() { Name = "Sereikiškių parko Bernardinų sodas", CoordX = 54.68413305498285, CoordY = 25.29522580235671, Category = "Park" },
                new Location() { Name = "Lietuvos nacionalinis dailės muziejus", CoordX = 54.68130476957157, CoordY = 25.289818468853266, Category = "Museum" },
                new Location() { Name = "Užupio angelas", CoordX = 54.68035, CoordY = 25.29515, Category = "Monument"}
            };
        }


        public void LoadDummyData()
        {
            var locationList = DummyLocationList();
            foreach (Location location in locationList)
            {
                bool res = LocationDatabase.AddLocation(location);
            }
        }
    }
}