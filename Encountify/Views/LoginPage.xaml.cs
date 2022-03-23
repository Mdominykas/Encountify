using Encountify.CustomRenderer;
using Encountify.Models;
using Encountify.Services;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.ComponentModel;
using Newtonsoft.Json;
using System.Linq;

namespace Encountify.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class LoginPage : ContentPage, INotifyPropertyChanged
    {
        String LastSession = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), DatabaseAccessConstants.LastSessionJSONName);
        IUserAccess DataStore;   //new DatabaseAccess<User>();

        public LoginPage()
        {
            DataStore = DependencyService.Get<IUserAccess>();
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
            var users = await DataStore.GetAllAsync(true);

            try
            {
                var user = users.Where(x => x.Username == Username.Text && x.Password == Password.Text).FirstOrDefault();
                if (user != null && user.Id != 0)
                {
                    App.UserID = user.Id;
                    App.UserName = user.Username;
                    App.UserEmail = user.Email;
                    App.IsAdmin = user.IsAdmin;
                    App.UserPassword = user.Password;
                    App.UserPicture = user.Picture;
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
