using Android.App;
using Android.Gms.Maps;
using Android.OS;

namespace Encounter1
{
    [Activity(Label = "MenuActivity")]
    public class MenuActivity : Activity, IOnMapReadyCallback
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Menu);

            var mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.map);
            mapFragment.GetMapAsync(this);

            //TextView currentCharacterName = FindViewById<TextView>(Resource.Id.textViewUserName);
            //currentCharacterName.Text = Intent.Extras.GetString("userName");

            // remainder of code omitted
        }

        public void OnMapReady(GoogleMap map)
        {
            // Do something with the map, i.e. add markers, move to a specific location, etc.
            map.UiSettings.ZoomControlsEnabled = true;
            map.UiSettings.CompassEnabled = true;
            map.MyLocationEnabled = true;
        }

    }
}