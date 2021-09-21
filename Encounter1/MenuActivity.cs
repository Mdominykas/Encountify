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

namespace Encounter1
{
    [Activity(Label = "MenuActivity")]
    public class MenuActivity : Activity
    {
        Button btnLocationList;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Menu);
            TextView currentCharacterName = FindViewById<TextView>(Resource.Id.textViewUserName);
            currentCharacterName.Text = Intent.Extras.GetString("userName");
            btnLocationList = FindViewById<Button>(Resource.Id.button1);
            btnLocationList.Click += OnLocationListButtonClicked;
        }

        private void OnLocationListButtonClicked(object sender, EventArgs e)
        {
            StartActivity(typeof(LocationListActivity));
        }
    }
}