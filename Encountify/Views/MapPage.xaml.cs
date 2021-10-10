using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.GoogleMaps;
using System.IO;
using SQLite;
using Encountify.Models;
using Plugin.Geolocator;
using System.Diagnostics;
using System;

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
            catch (Exception) // Later will need to find a more elegant way on how to handle this on xamarin forms
            {
                Debug.WriteLine("Permisions for location were not granted");
            }
        }

        static public void LoadMarker(Map map, string title, double latitude, double longitude, Color color)
        {

            Pin marker = new Pin()
            {
                Icon = BitmapDescriptorFactory.DefaultMarker(color),
                Label = title,
                Position = new Position(latitude, longitude),
            };

            if (!map.Pins.Contains(marker))
            {
                map.Pins.Add(marker);
            }
        }

        public void LoadMarkersFromDb(Map map)
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Locations.db3");
            SQLiteConnection db = new SQLiteConnection(dbPath);
            db.CreateTable<Location>();
            var table = db.Table<Location>();

            foreach (var s in table)
            {
                LoadMarker(map, s.Name, s.CoordX, s.CoordY, SelectMarkerColor(s.Category));
            }
        }

        //Color pallet very limited due to BitmapDescriptorFactory class, custom rendered will be needed eventually
        public static Color SelectMarkerColor(string category)
        {
            if (category == "Aquaria")
                return Color.Blue;
            else if (category == "Beach" || category == "Amusement park")
                return Color.Yellow;
            else if (category == "Botanical garden" || category == "Park" || category == "Zoo")
                return Color.Green;
            else if (category == "Casino" || category == "Casino hotel")
                return Color.Orange;
            else if (category == "Cathedral" || category == "Castle" || category == "Church" || category == "Fort" || category == "Heritage railway")
                return Color.SaddleBrown;
            else if (category == "Memorial" || category == "Monument")
                return Color.GhostWhite;
            else if (category == "Museum" || category == "Tourist trap")
                return Color.LightGoldenrodYellow;
            else if (category == "Ski area" || category == "Resort")
                return Color.Cyan;
            else if (category == "Sports facility")
                return Color.OrangeRed;
            else if (category == "Street")
                return Color.SlateGray;
            else if (category == "Other")
                return Color.Red;
            else
                return Color.Violet;
        }
    }
}