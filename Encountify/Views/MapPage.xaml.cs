using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.GoogleMaps;
using System.IO;
using SQLite;
using Encountify.Models;
using Plugin.Geolocator;

namespace Encountify.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        public MapPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            LoadMarkersFromDb(map);
            try
            {
                var locator = CrossGeolocator.Current;
                var position = await locator.GetPositionAsync();
                map.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.GoogleMaps.Position(position.Latitude, position.Longitude), Distance.FromMeters(5000)));
            }
            catch
            {
            }
        }

        public void LoadMarker(Map map, string title, double latitude, double longitude)
        {
            Pin marker = new Pin()
            {
                Label = title,
                Position = new Xamarin.Forms.GoogleMaps.Position(latitude, longitude)
            };
            map.Pins.Add(marker);
        }

        public void LoadMarkersFromDb(Map map)
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Locations.db3");
            SQLiteConnection db = new SQLiteConnection(dbPath);
            db.CreateTable<Location>();
            var table = db.Table<Location>();
            foreach (var s in table)
            {
                LoadMarker(map, s.Name, s.CoordX, s.CoordY);
            }
        }
    }
}