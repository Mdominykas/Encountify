using Android.App;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using Android.Views;
using Android.Widget;
using System.IO;
using System;
using SQLite;

namespace Encounter1
{
    [Activity(Label = "EncounterMe", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        EditText txtUsername;
        EditText txtPassword;
        Button btnCreate;
        Button btnSign;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            // Set our view from the "main" layout resource

            btnSign = FindViewById<Button>(Resource.Id.button1);
            btnCreate = FindViewById<Button>(Resource.Id.button2);
            txtUsername = FindViewById<EditText>(Resource.Id.editText1);
            txtPassword = FindViewById<EditText>(Resource.Id.editText2);
            btnSign.Click += BtnSign_Click;
            btnCreate.Click += BtnCreate_Click;
            CreateDB();

        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(RegActivity));
        }
        private void BtnSign_Click(object sender, EventArgs e)
        {
            try
            {
                string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "users.db3");
                var db = new SQLiteConnection(dbPath);
                var data = db.Table<LoginTable>();
                var data1 = data.Where(x => x.username == txtUsername.Text && x.password == txtPassword.Text).FirstOrDefault();
                if(data1 != null)
                {
                    Toast.MakeText(this, "Logged in successfully", ToastLength.Short).Show();
                    StartActivity(typeof(MenuActivity));
                }
                else
                {
                    Toast.MakeText(this, "Username or Password invalid", ToastLength.Short).Show();
                }
            }
            catch(Exception exc)
            {
                Toast.MakeText(this, exc.ToString(), ToastLength.Short).Show();

            }
        }
        public string CreateDB()
        {
            var output = "";
            output += "Creating Databse if it doesnt exists";
            string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "users.db3"); //Create New Database  
            var db = new SQLiteConnection(dpPath);
            output += "\n Database Created....";
            return output;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}