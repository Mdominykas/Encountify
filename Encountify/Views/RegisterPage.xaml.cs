using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using SQLite;
using Encountify.Models;
using Encountify.CustomRenderer;
using System.Text.RegularExpressions;
using Xunit.Sdk;

namespace Encountify.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        User user = new User();

        public RegisterPage()
        {
            InitializeComponent();
            Username.ReturnCommand = new Command(() => Email.Focus());
            Email.ReturnCommand = new Command(() => Password.Focus());
            Password.ReturnCommand = new Command(() => PasswordConfirm.Focus());
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            if(ValidateFields())
            {
                    var dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Users.db3");
                    SQLiteConnection db = new SQLiteConnection(dbPath);
                    var data = db.Table<User>();
                    var dataUser = data.Where(x => x.Username == Username.Text).FirstOrDefault();
                    if (dataUser == null)
                    {
                        var dataUser2 = data.Where(x => x.Email == Email.Text).FirstOrDefault();

                        if (dataUser2 == null)
                        {
                            user.Username = Username.Text;
                            user.Password = Password.Text;
                            user.Email = Email.Text;
                            RegisterUser(user);
                            await Navigation.PushAsync(new LoginPage());

                            Username.Text = string.Empty;
                            Email.Text = string.Empty;
                            Password.Text = string.Empty;
                            PasswordConfirm.Text = string.Empty;
                        }
                    else
                    {
                        await DisplayAlert("Warning", "Email in use", "OK");
                    }

                    }
                    else
                    {
                        await DisplayAlert("Warning","Username in use", "OK");
                    }
            }
        }
        

        public void RegisterUser(User user)
        {
            var dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Users.db3");
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
            var userNameValidation = new Regex(@"^[a-zaA-Z][a-zA-Z0-9]{2,15}$");
            if (string.IsNullOrEmpty(username))
            {
                DisplayAlert("Error", "Username cannot be empty", "OK");
                return false;
            }
            if(!userNameValidation.IsMatch(username))
            {
                DisplayAlert("Error", "Username should start with letters and has to be 3-16 long", "OK");
                return false;
            }
            else
            {
                return true;
            }
        }

        private Boolean ValidPassword(string password)
        {   
            var hasUpperCase = new Regex(@"[A-Z]+");
            var hasLowerCase = new Regex(@"[a-z]+");
            var hasNumbers = new Regex(@"[0-9]+");
            var hasSpecialSymbols = new Regex(@"[!@#$%^&*`~()_+=\[{\]};:<>|.?,-]");
            var hasRequiredLength = new Regex(@".{8,15}");


            if (!hasUpperCase.IsMatch(password))
            {
                DisplayAlert("Error", "Password should contain at least one upper case letter", "OK");
                return false;
            }
            if(!hasLowerCase.IsMatch(password))
            {
                DisplayAlert("Error", "Password should contain at least one lower case letter", "OK");
                return false;
            }
            if(!hasNumbers.IsMatch(password))
            {
                DisplayAlert ("Error", "Password should contain at least one number", "OK");
                return false;
            }
            if(!hasSpecialSymbols.IsMatch(password))
            {
                DisplayAlert("Error","Password should contain at least one special symbol", "OK");
                return false;
            }
            if(!hasRequiredLength.IsMatch(password))
            {
                DisplayAlert ("Error","Password should not be shorter than 8 or longer than 15 symbols", "OK");
                return false;
            }
            if(string.IsNullOrEmpty(password))
            {
                DisplayAlert("Error", "Password field cannot be empty", "OK");
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
            if (!ValidPassword(password))
            {
                return false;
            }
            else if (!PasswordsMatch(password, passwordconfirm))
            {
                DisplayAlert("Error", "Passwords do not match", "OK");
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
                DisplayAlert("Error", "Invalid mail format", "OK");
                return false;
            }
        }

        public bool ValidateTerms()
        {
            if (!Terms.IsChecked)
            {
                DisplayAlert("Warning", "You have to agree to the Terms of Service ", "OK");
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool ValidateFields()
        {
            if(ValidUsername((string)Username.Text) & VerifyPassword((string)Password.Text, 
                (string)PasswordConfirm.Text) & ValidEmail((string)Email.Text) & ValidateTerms())
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}