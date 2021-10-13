using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.GoogleMaps;
using Plugin.Geolocator;
using Encountify.Services;


namespace Encountify.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {

        public MapPage()
        {
            InitializeComponent();

            map.MapClicked += async (sender, e) =>
            {
                var lat = e.Point.Latitude;
                var lng = e.Point.Longitude;
                await Shell.Current.GoToAsync($"..?CoordX={lat}&CoordY={lng}");
            };
        }
        
        protected override async void OnAppearing()
        {
            try
            {
                LoadMarkersFromDb(map);
                var locator = CrossGeolocator.Current;
                var position = await locator.GetPositionAsync();
                map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMeters(5000)));
                map.Cluster();
          
            }
            catch
            {
            }
        }

        static public void LoadMarker(Map map, string title, double latitude, double longitude)
        {
            Pin marker = new Pin()
            {
                Label = title,
                Position = new Position(latitude, longitude)
            };

            if(!map.Pins.Contains(marker))
            {
                map.Pins.Add(marker);
            }
        }

        public void LoadMarkersFromDb(Map map)
        {
            var access = new LocationDatabaseAccess();
            var locationList = access.GetLocationList();

            foreach (var s in locationList)
            {
                LoadMarker(map, s.Name, s.CoordX, s.CoordY);
            }
        }

    }
}
