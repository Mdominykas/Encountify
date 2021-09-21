using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Encounter1
{
    [Activity(Label = "MapActivity")]
    public class MapActivity : Activity, IOnMapReadyCallback
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
            map.MyLocationEnabled = true;
        }
    }
}