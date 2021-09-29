using Android.App;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using Android.Widget;
using Android.Views;
using System.IO;
using System;
using SQLite;
using Android.Graphics.Drawables;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using Android;
using Android.Content.PM;
using Android.Graphics;
using CaptainDroid.TvgLib;

namespace Encounter1
{
    [Activity(Label = "EncounterMe", Theme = "@style/EncounterMe.LightMode", MainLauncher = true)]
    
    
    public class MainActivity : AppCompatActivity
    {
        EditText txtUsername;
        EditText txtPassword;
        TextView btnCreate;
        Button btnSign;
        private AnimationDrawable animationDrawable;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            TextView header = FindViewById<TextView>(Resource.Id.login_header);
            Tvg.Change(header, Resources.GetColor(Resource.Color.encounter_accent_1), Resources.GetColor(Resource.Color.encounter_accent_2));

            btnSign = FindViewById<Button>(Resource.Id.button_login);
            btnCreate = FindViewById<TextView>(Resource.Id.button_register);
            txtUsername = FindViewById<EditText>(Resource.Id.button_login_username);
            txtPassword = FindViewById<EditText>(Resource.Id.button_login_password);
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
                string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Users.db3");
                var db = new SQLiteConnection(dbPath);
                var data = db.Table<LoginTable>();
                var data1 = data.Where(x => x.Username == txtUsername.Text && x.Password == txtPassword.Text).FirstOrDefault();
                if(data1 != null)
                {
                    Toast.MakeText(this, "Logged in successfully", ToastLength.Short).Show();
                    Intent intent = new Intent(this.ApplicationContext, typeof(MenuActivity));
                    intent.PutExtra("userName", data1.Username);
                    StartActivity(intent);
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
            output += "Creating Database if it doesn't exists";
            string dpPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Users.db3"); 
            _ = new SQLiteConnection(dpPath);
            output += "\n Database Created....";
            return output;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}