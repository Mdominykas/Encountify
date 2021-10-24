using System;
using System.Diagnostics;
using Encountify.ViewModels;
using Xamarin.Forms;

namespace Encountify.Views
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            BindingContext = new ProfilePageViewModel();
            Debug.WriteLine("AppId:" + App.UserID);
            
        }

        private async void LogOutButton_onClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Quit", "You want to log out?", "Yes");
            await Navigation.PushAsync(new LoginPage()); 
        }
    }
}