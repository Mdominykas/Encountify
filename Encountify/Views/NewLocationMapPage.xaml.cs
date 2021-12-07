using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;
using Locations = Xamarin.Essentials.Location;

namespace Encountify.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewLocationMapPage : ContentPage
    {
        bool _firstTime = true;
        public double lat { get; set; } = 0;
        public double lng { get; set; } = 0;
        public NewLocationMapPage()
        {
            InitializeComponent(); 
        }

        protected override async void OnAppearing()
        {
            Locations userLocation = await Geolocation.GetLastKnownLocationAsync();

            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(userLocation.Latitude, userLocation.Longitude), Distance.FromMeters(1000)));
        }

        async void positionMap_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var m = (Xamarin.Forms.GoogleMaps.Map)sender;
            if(m.VisibleRegion != null)
            {
                if(_firstTime == true)
                {
                    _firstTime = false;
                    return;
                }

                lat = m.VisibleRegion.Center.Latitude;
                lng = m.VisibleRegion.Center.Longitude;

                Geocoder geoCoder = new Geocoder();
                Position position = new Position(lat, lng);
                IEnumerable<string> possibleAddresses = await geoCoder.GetAddressesForPositionAsync(position);
                string address = possibleAddresses.FirstOrDefault();

                Latitude.Text = lat.ToString();
                Longitude.Text = lng.ToString();
                Address.Text = address;
            }
        }

        private async void Button_Clicked(object sender, System.EventArgs e)
        {
            await Shell.Current.GoToAsync($"..?Latitude={lat}&Longitude={lng}");
        }
    }
}