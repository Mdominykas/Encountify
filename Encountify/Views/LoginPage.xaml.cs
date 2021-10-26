using Encountify.CustomRenderer;
using Encountify.Models;
using Encountify.Services;
using SQLite;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Encountify.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class LoginPage : ContentPage, INotifyPropertyChanged
    {
        String LastSession = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DatabaseAccessConstants.LastSessionJSONName);

        public LoginPage()
        {
            InitializeComponent();
            try
            {
                GetLastSession();
            }
            catch
            {
                // Skip if there is no file
            }
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), DatabaseAccessConstants.UserDatabaseName);
            SQLiteConnection db = new SQLiteConnection(dbPath);

            try
            {
                var data = db.Table<User>();
                var data1 = data.Where(x => x.Username == Username.Text && x.Password == Password.Text).FirstOrDefault();
                if (data1 != null && data1.Id != 0)
                {
                    App.UserID = data1.Id;
                    App.UserName = data1.Username;
                    App.UserEmail = data1.Email;
                    App.UserPassword = data1.Password;
                    OnLogin?.Invoke();
                    DependencyService.Get<MessagePopup>().ShortAlert("Logged in successfully");
                    await Shell.Current.GoToAsync("//HomePage");
                    DeleteSession();
                    if (Remember.IsChecked) SaveSession();
                    else ResetValues();
                    ResetFocus();
                }
                else
                {
                    DependencyService.Get<MessagePopup>().ShortAlert("Username or Password invalid");
                }

            }
            catch
            {
                DependencyService.Get<MessagePopup>().ShortAlert("Username or Password invalid");
            }
            db.Close();
        }

        public static event Action OnLogin;

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//RegisterPage");
            ResetFocus();
        }

        private void GetLastSession()
        {
            User user = JsonConvert.DeserializeObject<User>(File.ReadAllText(LastSession));
            Username.Text = user.Username;
            Password.Text = user.Password;
            Remember.IsChecked = true;
        }

        private void SaveSession()
        {
            User user = new User
            {
                Id = App.UserID,
                Username = App.UserName,
                Email = App.UserEmail,
                Password = App.UserPassword
            };
            string json = JsonConvert.SerializeObject(user);
            File.WriteAllText(LastSession, json);
        }

        private void DeleteSession()
        {
            File.Delete(LastSession);
        }

        private async void OnForgotPasswordClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//ForgotPasswordPage");
            ResetFocus();
        }

        private void ResetValues()
        {
            Username.Text = string.Empty;
            Password.Text = string.Empty;
            Remember.IsChecked = false;
        }

        private void ResetFocus()
        {
            Username.Unfocus();
            Password.Unfocus();
        }

    }
}
