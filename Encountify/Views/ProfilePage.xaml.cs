using System;
using System.Diagnostics;
using Encountify.Services;
using Encountify.ViewModels;
using Xamarin.Forms;

namespace Encountify.Views
{
    public partial class ProfilePage : ContentPage
    {
        private ProfilePageViewModel _viewModel;
        public ProfilePage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new ProfilePageViewModel();
        }

        private async void LogOutButton_onClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Quit", "You want to log out?", "Yes");
            await Shell.Current.GoToAsync("//LoginPage");
        }

        public void OnScaleRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            if ((e as CheckedChangedEventArgs).Value)
            {
                _viewModel?.ChangeScale();
            }
        }

    }
}