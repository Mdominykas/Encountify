using Encountify.CustomRenderer;
using Encountify.Models;
using Encountify.Services;
using SQLite;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;

namespace Encountify.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    
    public partial class LoginPage : ContentPage
    {

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), DatabaseAccessConstants.UserDatabaseName);
            SQLiteConnection db = new SQLiteConnection(dbPath);

            try
            {
                var data = db.Table<User>();
                var data1 = data.Where(x => x.Username == Username.Text && x.Password == Password.Text).FirstOrDefault();
                //db.Close();
                if (data1 != null && data1.Id != 0)
                {

                    App.UserID = data1.Id;
                    DependencyService.Get<MessagePopup>().ShortAlert("Logged in successfully");
                    Debug.WriteLine("Id:" + data1.Id + " appId:" + App.UserID);
                    await Shell.Current.GoToAsync("//HomePage");
                }
                else
                {
                    DependencyService.Get<MessagePopup>().ShortAlert("Username or Password invalid");
                }
                db.Close();
            }
            catch
            {
                DependencyService.Get<MessagePopup>().ShortAlert("Username or Password invalid");
            }
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//RegisterPage");
        }

    }
}