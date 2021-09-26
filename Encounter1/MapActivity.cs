using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static Android.Gms.Maps.GoogleMap;

namespace Encounter1
{
    [Activity(Label = "MapActivity")]
    public class MapActivity : FragmentActivity, IOnMapReadyCallback
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Map);

            var mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            mapFragment.GetMapAsync(this);
        }

        public void OnMapReady(GoogleMap map)
        {
            
            map.UiSettings.ZoomControlsEnabled = true;
            map.UiSettings.CompassEnabled = true;
            map.UiSettings.MapToolbarEnabled = true;
            map.UiSettings.ZoomControlsEnabled = true;
            map.UiSettings.RotateGesturesEnabled = true;
            map.MyLocationEnabled = true;
            map.MoveCamera(CameraUpdateFactory.ZoomIn());

            LoadMarkersFromDb(map);
        }

        public void LoadMarker(GoogleMap map, string title, double latitude, double longtitude)
        {
            MarkerOptions markerOpt = new MarkerOptions();
            markerOpt.SetPosition(new LatLng(latitude, longtitude));
            markerOpt.SetTitle(title);
            markerOpt.SetSnippet("This is a test");
            map.AddMarker(markerOpt);
        }

        public void LoadMarkersFromDb(GoogleMap map)
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "locationList.db3");
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<LocationTable>();
            var table = db.Table<LocationTable>();
            foreach (var s in table)
            {
                LoadMarker(map, s.LocationName, s.LocationCoordX, s.LocationCoordY);
            }
        }
    }
}
