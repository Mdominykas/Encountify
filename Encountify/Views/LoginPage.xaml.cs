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

namespace Encountify.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    
    public partial class LoginPage : ContentPage, INotifyPropertyChanged
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
                if (data1 != null && data1.Id != 0)
                {
                    App.UserID = data1.Id;
                    App.UserName = data1.Username;
                    App.UserEmail = data1.Email;
                    App.UserPassword = data1.Password;
                    OnLogin?.Invoke();
                    DependencyService.Get<MessagePopup>().ShortAlert("Logged in successfully");
                    await Shell.Current.GoToAsync("//HomePage");
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
        }

    }
}