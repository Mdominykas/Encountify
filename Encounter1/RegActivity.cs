using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SQLite;
using Android.Graphics.Drawables;
using Xamarin.Forms.Platform.Android;


namespace Encounter1
{
    [Activity(Label = "RegActivity")]
    public class RegActivity : Activity
    {
        EditText txtUsername;
        EditText txtPassword;
        Button btnCreate;
        EditText confirmPassword;
        EditText txtEmail;
        private AnimationDrawable animationDrawable;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.NewUser);

            animationDrawable = (Android.Graphics.Drawables.AnimationDrawable)Resources.GetDrawable(Resource.Drawable.background);
            LinearLayout img = (LinearLayout)FindViewById(Resource.Id.linearLayout2);
            img.SetBackground(animationDrawable);

            animationDrawable.SetEnterFadeDuration(4000);
            animationDrawable.SetExitFadeDuration(4000);
            animationDrawable.Start();

            // Create your application here  
            txtUsername = (EditText)FindViewById<EditText>(Resource.Id.editText2);
            txtPassword = (EditText)FindViewById<EditText>(Resource.Id.editText1);
            confirmPassword = (EditText)FindViewById<EditText>(Resource.Id.editText3);
            txtEmail = (EditText)FindViewById<EditText>(Resource.Id.editText4);
            btnCreate = FindViewById<Button>(Resource.Id.buttonCreate);
            btnCreate.Click += Btncreate_Click;
        }

        private Boolean RegistrationPassword()
        {
            var userReg = (string)txtPassword.Text;

            if (string.IsNullOrEmpty(userReg))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private Boolean RegistrationConfirmPassword()
        {
            var userReg = confirmPassword.Text;

            if (string.IsNullOrEmpty(userReg))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private Boolean Verify()
        {
            if (((string)txtPassword.Text).Equals((string)confirmPassword.Text))
            {
                return true;
            }
            else
                return false;
        }

        private bool VerifyPassword()
        {
            if (!RegistrationPassword() | !RegistrationConfirmPassword() | !Verify())
            {
                Toast.MakeText(this, "Passwords do not match", ToastLength.Short).Show();
                return false;
            }
            return true;
        }

        public bool ValidEmail()
        {
            if(Android.Util.Patterns.EmailAddress.Matcher((string)txtEmail.Text).Matches())
            {
                return true;
            }
            else
            {
                Toast.MakeText(this, "Invalid mail format", ToastLength.Short).Show();
                return false;
            }
        }



        private void Btncreate_Click(object sender, EventArgs e)
        {
            VerifyPassword();
            ValidEmail();

            if (VerifyPassword() && ValidEmail())
            {
                try
                {
                    string dpPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Users.db3");
                    var db = new SQLiteConnection(dpPath);
                    var data = db.Table<LoginTable>();
                    var dataUser = data.Where(x => x.Username == txtUsername.Text).FirstOrDefault();
                    if (dataUser == null)
                    {
                        LoginTable tbl = new LoginTable
                        {
                            Username = txtUsername.Text,
                            Password = txtPassword.Text,
                            Email = txtEmail.Text
                        };

                        db.Insert(tbl);

                        Toast.MakeText(this, "Record Added Successfully.....", ToastLength.Short).Show();
                        StartActivity(typeof(MainActivity));
                    }
                    else
                    {
                        Toast.MakeText(this, "Username unavailable", ToastLength.Short).Show();

                    }
                }
                catch (Exception ex)
                {
                    Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
                }
                
            }
        }

    }
}






