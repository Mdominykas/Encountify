using Encountify.Views;
using System;
using Xamarin.Forms;

namespace Encountify
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("LocationDetailPage", typeof(LocationDetailPage));
            Routing.RegisterRoute("NewLocationPage", typeof(NewLocationPage));
            Routing.RegisterRoute("MapPage", typeof(MapPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}