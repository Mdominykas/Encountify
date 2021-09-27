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
        ExpandableListView locationList;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.LocationList);
            locationList = FindViewById<ExpandableListView>(Resource.Id.expandableListView1);
            List<String> listItems = GetLocationList();

            var items = CreateDummyLocationTableList();
            locationList.SetAdapter(new MyAdapter(this, items));
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

        private List<LocationTable> CreateDummyLocationTableList()
        {
            return new List<LocationTable> { new LocationTable() { LocationName = "Vilniaus katedra", LocationCoordX = 54.685849042698216, LocationCoordY = 25.287750880122083 }, new LocationTable() { LocationName = "Gedimino bokštas", LocationCoordX = 54.68667445192699, LocationCoordY = 25.29056883194689 } };
        }

        private void LoadDummyData(SQLiteConnection db)
        {
            if(db.Table<LocationTable>().Count() == 0)
            {
                var locationList = CreateDummyLocationTableList();
                foreach(var location in locationList)
                {
                    db.Insert(location);
                }
            }
        }
    }

    public class MyAdapter : BaseExpandableListAdapter
    {
        private readonly Context _context;
        private List<IGrouping<string, LocationTable>> _myThingsGrouped;

        public MyAdapter(Context context, List<LocationTable> myThings) : base()
        {
            _context = context;
            _myThingsGrouped = myThings.GroupBy(i => i.LocationName).ToList();
        }

        public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
        {
            return null;
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            return childPosition;
        }

        public override int GetChildrenCount(int groupPosition)
        {
            return _myThingsGrouped[groupPosition].Count();
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            var curLocation = _myThingsGrouped[groupPosition].ElementAt(childPosition);
            return new TextView(_context) { Text = curLocation.LocationName + "\n(" + curLocation.LocationCoordX + 
                ", " + curLocation.LocationCoordY + ")\n"};
        }

        public override Java.Lang.Object GetGroup(int groupPosition)
        {
            return null;
        }

        public override long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            var group = _myThingsGrouped[groupPosition];
            return new TextView(_context)
            {
                Text = group.Key
            };
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }

        public override int GroupCount => _myThingsGrouped.Count;

        public override bool HasStableIds => true;
    }
}
