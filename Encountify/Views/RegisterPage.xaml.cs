using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using SQLite;
using Encountify.Models;
using Encountify.CustomRenderer;

namespace Encountify.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {

            if (ValidUsername((string)Username.Text) & VerifyPassword((string)Password.Text, (string)PasswordConfirm.Text) & ValidEmail((string)Email.Text))
            {
                User user = new User()
                {
                    Username = Username.Text,
                    Password = Password.Text,
                    Email = Email.Text
                };

                RegisterUser(user);

                await Shell.Current.GoToAsync("//LoginPage");
            }

        }

        public void RegisterUser(User user)
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Users.db3");
            SQLiteConnection db = new SQLiteConnection(dbPath);

            try
            {
                db.Insert(user);
            }
            catch
            {
                db.CreateTable<User>();
                db.Insert(user);
            }
            db.Commit();
            db.Close();
            DependencyService.Get<MessagePopup>().ShortAlert("Record Added Successfully.....");
        }

        private Boolean ValidUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private Boolean ValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private Boolean PasswordsMatch(string password, string passwordconfirm)
        {
            if ((password).Equals(passwordconfirm))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool VerifyPassword(string password, string passwordconfirm)
        {
            if (!ValidPassword(password) | !ValidPassword(passwordconfirm))
            {
                DependencyService.Get<MessagePopup>().ShortAlert("Password is invalid");
                return false;
            }
            else if (!PasswordsMatch(password, passwordconfirm))
            {
                DependencyService.Get<MessagePopup>().ShortAlert("Passwords do not match");
                return false;
            }
            return true;
        }

        public bool ValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                DependencyService.Get<MessagePopup>().ShortAlert("Invalid mail format");
                return false;
            }
        }
    }
}