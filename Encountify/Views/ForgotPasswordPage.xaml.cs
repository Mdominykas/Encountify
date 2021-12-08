using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Encountify.Models;
using Encountify.Services;
using System.Linq;

namespace Encountify.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPasswordPage : ContentPage
    {
        IUserAccess DataStore; //new DatabaseAccess<User>();

        public ForgotPasswordPage()
        {
            DataStore = DependencyService.Get<IUserAccess>();
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
            ResetValues();
            ResetFocus();
        }

        private async void OnSendPasswordClicked(object sender, EventArgs e)
        {
            var users = await DataStore.GetAllAsync(true);
            var user = users.Where(x => x.Email == Email.Text).FirstOrDefault();
            if (user != null)
            {
                var services = new EmailServices();
                await services.SendEmail(
                    RecipientName: user.Username, 
                    RecipientEmail: user.Email,
                    Subject: "Forgot password?",
                    Content: $"<b>Your username:</b> {user.Username}<br><b>Your password:</b> {user.Password}");
                await DisplayAlert("Success!", $"Email was sent to {Email.Text}", "OK");
            }
            else
            {
                await DisplayAlert("Warning", "No such email", "OK");
            }
        }

        private void ResetValues()
        {
            Email.Text = string.Empty;
        }

        private void ResetFocus()
        {
            Email.Unfocus();
        }
    }
}