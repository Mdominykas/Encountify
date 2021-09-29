using Encountify.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Encountify.Services
{
    public class MockDataStore : IDataStore<Location>
    {
        readonly List<Location> locations;

        public MockDataStore()
        {
            locations = GetLocationList();
        }

        public async Task<bool> AddLocationAsync(Location location)
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Locations.db3");
            SQLiteConnection db = new SQLiteConnection(dbPath);

            db.Insert(location);
            db.Commit();

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateLocationAsync(Location location)
        {
            var oldLocation = locations.Where((Location arg) => arg.Id == location.Id).FirstOrDefault();
            locations.Remove(oldLocation);
            locations.Add(location);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteLocationAsync(int id)
        {
            var old = locations.Where((Location arg) => arg.Id == id).FirstOrDefault();
            locations.Remove(old);

            return await Task.FromResult(true);
        }

        public async Task<Location> GetLocationAsync(int id)
        {
            return await Task.FromResult(locations.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Location>> GetLocationsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(locations);
        }

        public void LoadDummyData(SQLiteConnection db)
        {
            if (db.Table<Location>().Count() == 0)
            {
                var location1 = new Location() { Name = "Vilniaus katedra", CoordX = 54.685849042698216, CoordY = 25.287750880122083 };
                var location2 = new Location() { Name = "Gedimino bokštas", CoordX = 54.68667445192699, CoordY = 25.29056883194689 };
                db.Insert(location1);
                db.Insert(location2);
            }
        }

        public List<Location> GetLocationList()
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Locations.db3");
            SQLiteConnection db = new SQLiteConnection(dbPath);

            db.CreateTable<Location>();
            LoadDummyData(db);

            var table = db.Table<Location>();

            List<Location> locationlist = new List<Location>();

            foreach (var s in table)
            {
                locationlist.Add(s);
            }

            return locationlist;
        }

    }
}