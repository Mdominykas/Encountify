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
            List<String> listItems = getLocationList();
            ArrayAdapter adapter = new ArrayAdapter(this,
                Android.Resource.Layout.SimpleListItem1, listItems);
            locationList.Adapter = adapter;
        }

        List<String> getLocationList()
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "locationList.db3");
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<Location>();
            loadDummyData(db);
            var table = db.Table<Location>();
            List<string> locationNameList = new List<String>();
            foreach (var s in table)
            {
                locationNameList.Add(s.locationName);
            }
            return locationNameList;
        }

        void loadDummyData(SQLiteConnection db)
        {
            if(db.Table<Location>().Count() == 0)
            {
                var location1 = new Location() { locationName = "Katedra" };
                var location2 = new Location() { locationName = "Gedimino bokštas" };
                db.Insert(location1);
                db.Insert(location2);
            }
        }
    }
}