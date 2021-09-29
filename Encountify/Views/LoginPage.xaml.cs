using Encountify.CustomRenderer;
using Encountify.Models;
using SQLite;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Users.db3");
            SQLiteConnection db = new SQLiteConnection(dbPath);

            try
            {
                var data = db.Table<User>();
                var data1 = data.Where(x => x.Username == Username.Text && x.Password == Password.Text).FirstOrDefault();
                db.Close();
                if (data1 != null)
                {
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
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//RegisterPage");
        }
    }
}