using Encountify.Models;
using Encountify.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Locations = Xamarin.Essentials.Location;

namespace Encountify.ViewModels
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class LocationDetailViewModel : BaseViewModel
    {
        private long id;
        private string name;
        private string description;
        private double longitude;
        private double latitude;
        private string category;
        private string distance;
        private Location location;

        public int Id
        {
            get => (int)id;
            set
            {
                int temp = (int)id;
                SetProperty(ref temp, value);
                LoadLocationId(value);
            }
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public string Distance
        {
            get => distance;
            set => SetProperty(ref distance, value);
        }

        public double Longitude
        {
            get => longitude;
            set => SetProperty(ref longitude, value);
        }

        public double Latitude
        {
            get => latitude;
            set => SetProperty(ref latitude, value);
        }

        public string Category
        {
            get => category;
            set => SetProperty(ref category, value);
        }

        public Map Map { get; set; }

        public async void LoadLocationId(int Id)
        {
            try
            {
                location = await DataStore.GetAsync(Id);
                Id = location.Id;
                Name = location.Name;
                Description = location.Description;
                Longitude = location.Longitude;
                Latitude = location.Latitude;
                Distance = await DistanceCounter.GetFormattedDistance(new Locations(location.Latitude, location.Longitude));
                Category = CategoryConverter.ConvertCategoryToString((Category)location.Category);
                LoadMarker(Map, location);
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Location");
            }
        }

        static public async void LoadMarker(Map map, Location location)
        {

            Geocoder geoCoder = new Geocoder();
            Position position = new Position(location.Latitude, location.Longitude);
            MapSpan mapSpan = new MapSpan(position, 0.01, 0.01);

            map.MoveToRegion(mapSpan, true);

            IEnumerable<string> possibleAddresses = await geoCoder.GetAddressesForPositionAsync(position);
            string address = possibleAddresses.FirstOrDefault();

            Pin pin = new Pin()
            {
                Label = location.Name,
                Address = address,
                Position = position,
            };

            if (!map.Pins.Contains(pin))
            {
                map.Pins.Add(pin);
            }
        }
    }
}
