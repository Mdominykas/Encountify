using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using System.IO;

namespace Encounter1
{
    [Activity(Label = "LocationListActivity")]
    public class LocationListActivity : Activity
    {
        ListView locationList;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LocationList);
            locationList = FindViewById<ListView>(Resource.Id.locationListView1);
            List<String> listItems = GetLocationList();
            ArrayAdapter adapter = new ArrayAdapter(this,
                Android.Resource.Layout.SimpleListItem1, listItems);
            locationList.Adapter = adapter;
        }

        List<string> GetLocationList()
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "locationList.db3");
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<LocationTable>();
            LoadDummyData(db);
            var table = db.Table<LocationTable>();
            List<string> locationNameList = new List<String>();
            foreach (var s in table)
            {
                locationNameList.Add(s.LocationName);
            }
            return locationNameList;
        }

        void LoadDummyData(SQLiteConnection db)
        {
            if(db.Table<LocationTable>().Count() == 0)
            {
                var location1 = new LocationTable() { LocationName = "Vilniaus katedra", LocationCoordX = 54.685849042698216, LocationCoordY = 25.287750880122083 };
                var location2 = new LocationTable() { LocationName = "Gedimino bokštas", LocationCoordX = 54.68667445192699, LocationCoordY = 25.29056883194689 };
                db.Insert(location1);
                db.Insert(location2);
            }
        }
    }
}