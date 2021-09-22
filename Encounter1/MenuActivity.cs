using Android;
using Android.App;
using Android.Content.PM;
using Android.Gms.Maps;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.Core.Content;
using System;

namespace Encounter1
{
    [Activity(Label = "MenuActivity")]
    public class MenuActivity : Activity
    {

        const int RequestLocationId = 0;
        readonly string[] LocationPermissions =
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation
        };
        private Button btnLocationList;
        private Button btnMap;
        protected override void OnStart()
        {
            base.OnStart();

            if ((int)Build.VERSION.SdkInt >= 23)
            {
                if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted)
                {
                    RequestPermissions(LocationPermissions, RequestLocationId);
                }

            }
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Menu);

            TextView currentCharacterName = FindViewById<TextView>(Resource.Id.textViewUserName);
            currentCharacterName.Text = Intent.Extras.GetString("userName");
            btnLocationList = FindViewById<Button>(Resource.Id.button1);
            btnLocationList.Click += OnLocationListButtonClicked;
            btnMap = FindViewById<Button>(Resource.Id.button2);
            btnMap.Click += OnMapButtonClicked;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {

            if (requestCode == RequestLocationId)
            {
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) == (int)Permission.Granted)
                {
                    Toast.MakeText(this, "Permissions granted successfully", ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(this, "Logged in successfully", ToastLength.Short).Show();
                }
            }
            else
            {
                Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void OnLocationListButtonClicked(object sender, EventArgs e)
        {
            StartActivity(typeof(LocationListActivity));
        }

        private void OnMapButtonClicked(object sender, EventArgs e)
        {
            StartActivity(typeof(MapActivity));
        }

    }
}