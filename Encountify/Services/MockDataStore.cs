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
            if (db.Table<Location>().Count() <= 7)
            {
                var location1 = new Location() { Name = "Vilniaus katedra", CoordX = 54.685849042698216, CoordY = 25.287750880122083 };
                var location2 = new Location() { Name = "Gedimino bokštas", CoordX = 54.68667445192699, CoordY = 25.29056883194689 };
                var location3 = new Location() { Name = "Vilniaus Šv. Onos bažnyčia", CoordX = 54.68378573230062, CoordY = 25.292650881640785 };
                var location4 = new Location() { Name = "Trys kryžiai", CoordX = 54.68740766559662, CoordY = 25.29771489238211 };
                var location5 = new Location() { Name = "Gedimino prospektas", CoordX = 54.68644019450281, CoordY = 25.285441103636185 };
                var location6 = new Location() { Name = "Lietuvos Respublikos Prezidento kanceliarija", CoordX = 54.68383535000863, CoordY = 25.286685648648888 };
                var location7 = new Location() { Name = "Sereikiškių parko Bernardinų sodas", CoordX = 54.68413305498285, CoordY = 25.29522580235671 };
                var location8 = new Location() { Name = "Lietuvos nacionalinis dailės muziejus", CoordX = 54.68130476957157, CoordY = 25.289818468853266 };

                db.Insert(location1);
                db.Insert(location2);
                db.Insert(location3);
                db.Insert(location4);
                db.Insert(location5);
                db.Insert(location6);
                db.Insert(location7);
                db.Insert(location8);
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