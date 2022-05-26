using Encountify.Views;
using System;
using Xamarin.Forms;

namespace Encountify
{
    public partial class AdminShell : Shell
    {
        public AdminShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("LocationDetailPage", typeof(LocationDetailPage));
            Routing.RegisterRoute("NewLocationPage", typeof(NewLocationPage));
            Routing.RegisterRoute("MapPage", typeof(MapPage));
            Routing.RegisterRoute("LocationsNearUserPage", typeof(LocationsNearUserPage));
            Routing.RegisterRoute("NewLocationMapPage", typeof(NewLocationMapPage));
            Routing.RegisterRoute("NewPostPage", typeof(NewPostPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Quit", "You want to log out?", "Yes");
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}