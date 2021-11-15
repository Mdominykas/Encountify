using Encountify.Models;
using Encountify.Services;
using Encountify.ViewModels;
using System;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.GoogleMaps;
using Locations = Xamarin.Essentials.Location;
using Geolocation = Xamarin.Essentials.Geolocation;
using DistanceUnits = Xamarin.Essentials.DistanceUnits;
using GeolocationRequest = Xamarin.Essentials.GeolocationRequest;
using GeolocationAccuracy = Xamarin.Essentials.GeolocationAccuracy;

namespace Encountify.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        MapViewModel _viewModel = new MapViewModel();
        private CancellationTokenSource cts;

        public MapPage()
        {
            InitializeComponent();
            BindingContext = _viewModel;

            map.MapClicked += async (sender, e) =>
            {
                double lat = e.Point.Latitude;
                double lng = e.Point.Longitude;
                await Shell.Current.GoToAsync($"..?Latitude={lat}&Longitude={lng}");
            };

            map.PinClicked += async (sender, e) =>
            {
                var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(1));
                cts = new CancellationTokenSource();
                Locations location = await Geolocation.GetLocationAsync(request, cts.Token);

                Locations pinLocation = new Locations(e.Pin.Position.Latitude, e.Pin.Position.Longitude);
                Locations personLocation = new Locations(location.Latitude, location.Longitude);

                var distance = Locations.CalculateDistance(pinLocation, personLocation, DistanceUnits.Kilometers);

                if (distance <= 0.03)
                {
                    var access = new DatabaseAccess<Location>();
                    var locationList = await access.GetAllAsync();

                    Location visited = locationList.FirstOrDefault(s => s.Name == e.Pin.Label);
                    if(visited != null)
                    {
                        // hardcoded 100, it might be nice to change to something later on
                        VisitedLocations newVisit = new VisitedLocations() { LocationId = visited.Id, UserId = App.UserID, Points = 100};
                        var visitedAccess = new DatabaseAccess<VisitedLocations>();
                        await visitedAccess.AddAsync(newVisit);
                    }
                    await DisplayAlert($"You visited {e.Pin.Label}!" , distance.ToString(), "OK");
                }
            };
        }

        protected override async void OnAppearing()
        {
            try
            {
                LoadMarkersFromDb(map);

                var request = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(1));
                cts = new CancellationTokenSource();
                Locations location = await Geolocation.GetLocationAsync(request, cts.Token);

                map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromMeters(1000)));
                //map.Cluster();

            }
            catch (Exception) // Later will need to find a more elegant way on how to handle this on xamarin forms
            {
                Debug.WriteLine("Permisions for location were not granted");
            }
        }

        protected override void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
            base.OnDisappearing();
        }

        static public async void LoadMarker(Map map, Marker marker, Color color)
        {
            Geocoder geoCoder = new Geocoder();
            Position position = new Position(marker.Latitude, marker.Longitude);

            IEnumerable<string> possibleAddresses = await geoCoder.GetAddressesForPositionAsync(position);
            string address = possibleAddresses.FirstOrDefault();

            if(address == null)
            {
                address = "Unknown";
            }

            Pin pin = new Pin()
            {
                Icon = BitmapDescriptorFactory.DefaultMarker(color),
                Label = marker.Name,
                Address = address,
                Position = position,
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
            else if ((category & (int)(Category.Cathedral | Category.Castle | Category.Church | Category.Fort)) != 0) return Color.SaddleBrown;
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
        public double Latitude, Longitude;
        public Marker(string name, double latitude, double longitude)

        {
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}