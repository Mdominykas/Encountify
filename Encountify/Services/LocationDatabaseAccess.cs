using Encountify.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Encountify.Services
{
    //it may can be made generic for all databases and or merged with IDataStore
    //however it might become a huge hussle if we when adding databases with different logic
    class LocationDatabaseAccess
    {
        public List<Location> GetLocationList()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), 
                DatabaseAccessConstants.LocationDatabaseName);
            SQLiteConnection db = new SQLiteConnection(dbPath);
            //haven't tested it, however we may not need to create new SQLiteConnection in every request

            db.CreateTable<Location>();

            var table = db.Table<Location>();

            List<Location> locationlist = new List<Location>();
            foreach (Location s in table)
            {
                locationlist.Add(s);
            }

            return locationlist;
        }

        public bool AddLocation(Location location)
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DatabaseAccessConstants.LocationDatabaseName);
            SQLiteConnection db = new SQLiteConnection(dbPath);

            List<Location> locations = GetLocationList();

            foreach(Location oldLocation in locations)
            {
                if (String.Equals(location.Name, oldLocation.Name))
                    return false;
            }

            int result;
            try
            {
                result = db.Insert(location);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return false;
            }
            return result == 1;
        }

        public bool DeleteLocation(Location location)
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DatabaseAccessConstants.LocationDatabaseName);
            SQLiteConnection db = new SQLiteConnection(dbPath);

            return db.Delete(location.Id) == 1;
        }

        public int DeleteAllLocations()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DatabaseAccessConstants.LocationDatabaseName);
            SQLiteConnection db = new SQLiteConnection(dbPath);

            return db.DeleteAll<Location>();
        }

        public bool UpdateLocation(Location location)
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DatabaseAccessConstants.LocationDatabaseName);
            SQLiteConnection db = new SQLiteConnection(dbPath);

            return db.Update(location) == 1;
        }
    }
}
