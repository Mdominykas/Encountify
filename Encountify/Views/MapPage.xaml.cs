using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.GoogleMaps;
using Plugin.Geolocator;
using System.Diagnostics;
using System;
using Encountify.Services;
using System.Collections.Generic;
using Encountify.Models;
using Encountify.ViewModels;

namespace Encountify.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        MapViewModel _viewModel;

        public MapPage()
        {
            InitializeComponent();

            map.MapClicked += async (sender, e) =>
            {
                var lat = e.Point.Latitude;
                var lng = e.Point.Longitude;
                await Shell.Current.GoToAsync($"..?Latitude={lat}&Longitude={lng}");
            };
            BindingContext = _viewModel = new MapViewModel();
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

        static public void LoadMarker(Map map, Marker marker, Color color)
        {
            Pin pin = new Pin()
            {
                Icon = BitmapDescriptorFactory.DefaultMarker(color),
                Label = marker.Name,
                Position = new Position(marker.Lattitude, marker.Longitude),
            };

            if (!map.Pins.Contains(pin))
            {
                map.Pins.Add(pin);
            }
        }

        public void LoadMarkersFromDb(Map map)
        {
            var access = new DatabaseAccess<Location>();
            var locationList = access.GetAllAsync().Result;
            foreach (var s in locationList)
            {
                var marker = new Marker(s.Name, s.Latitude, s.Longitude);
                LoadMarker(map, marker, SelectMarkerColor(s.Category));
            }
        }

        //Color pallet very limited due to BitmapDescriptorFactory class, custom rendered will be needed eventually
        public static Color SelectMarkerColor(int category)
        {
            Dictionary<Category, Color> dictionary = new Dictionary<Category, Color>();
            var bluePins = Category.Aquaria;
            dictionary[bluePins] = Color.Blue;
            if ((category & (int)Category.Aquaria) != 0) return Color.Blue;
            else if ((category & (int)(Category.Beach | Category.AmusementPark)) != 0) return Color.Yellow;
            else if ((category & (int)(Category.BotanicalGarden | Category.Park | Category.Zoo)) != 0) return Color.Green;
            else if ((category & (int)(Category.Casino)) != 0) return Color.Orange;
            else if ((category & (int)(Category.Cathedral| Category.Castle | Category.Church | Category.Fort)) != 0) return Color.SaddleBrown;
            else if ((category & (int)(Category.Memorial | Category.Monument)) != 0) return Color.GhostWhite;
            else if ((category & (int)(Category.Museum)) != 0) return Color.LightGoldenrodYellow;
            else if ((category & (int)(Category.Resort)) != 0) return Color.Cyan;
            else if ((category & (int)(Category.SportFacility)) != 0) return Color.OrangeRed;
            else if ((category & (int)(Category.Street)) != 0) return Color.SlateGray;
            else if ((category & (int)(Category.Other)) != 0) return Color.Red;
            else return Color.Violet;
        }
    }

    public struct Marker
    {
        public string Name;
        public double Longitude, Lattitude;
        public Marker(string name, double lattitude, double longitude)
        {
            Name = name;
            Longitude = longitude;
            Lattitude = lattitude;
        }
    }
}