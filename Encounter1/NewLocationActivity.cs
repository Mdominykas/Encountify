using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Encounter1
{
    [Activity(Label = "NewLocationActivity")]
    public class NewLocationActivity : Activity
    {
        private Button _btnCreateNewLocation;
        private EditText _locationName, _locationX, _locationY;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.NewLocation);
            _btnCreateNewLocation = FindViewById<Button>(Resource.Id.button1);
            _btnCreateNewLocation.Click += OnCreateNewLocationButtonClicked;
            _locationName = FindViewById<EditText>(Resource.Id.editText1);
            _locationX = FindViewById<EditText>(Resource.Id.editText2);
            _locationY = FindViewById<EditText>(Resource.Id.editText3);
        }

        private void OnCreateNewLocationButtonClicked(object sender, EventArgs e)
        {
            var newLocation = ParseNewLocation();
            if (newLocation != null)
            {
                AddNewLocationToDatabase(newLocation);
                Toast.MakeText(this, "New nocation created", ToastLength.Short).Show();
                StartActivity(typeof(LocationListActivity));
            }
            else
            {
                Toast.MakeText(this, "Incorrect arguments", ToastLength.Short).Show();
            }
        }

        private LocationTable ParseNewLocation()
        {
            string locationName = _locationName.Text;
            if (String.IsNullOrEmpty(locationName))
                return null;
            double xCoord, yCoord;
            if (!Double.TryParse(_locationX.Text, out xCoord))
                return null;
            if (!Double.TryParse(_locationY.Text, out yCoord))
                return null;
            return new LocationTable { LocationName = _locationName.Text, LocationCoordX = xCoord, LocationCoordY = yCoord };
        }

        private void AddNewLocationToDatabase(LocationTable location)
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "locationList.db3");
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<LocationTable>();
            db.Insert(location);
        }

    }
}