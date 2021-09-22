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
            db.CreateTable<Location>();
            LoadDummyData(db);
            var table = db.Table<Location>();
            List<string> locationNameList = new List<String>();
            foreach (var s in table)
            {
                locationNameList.Add(s.LocationName);
            }
            return locationNameList;
        }

        void LoadDummyData(SQLiteConnection db)
        {
            if(db.Table<Location>().Count() == 0)
            {
                var location1 = new Location() { LocationName = "Katedra" };
                var location2 = new Location() { LocationName = "Gedimino bokštas" };
                db.Insert(location1);
                db.Insert(location2);
            }
        }
    }
}