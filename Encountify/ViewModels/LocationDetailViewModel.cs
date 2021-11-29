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
        private int id;
        private string name;
        private string description;
        private double latitude;
        private double longitude;
        private string category;
        private string distance;
        private string address;
        private Location location;

        public int Id
        {
            get => id;
            set
            {
                SetProperty(ref id, value);
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

        public double Latitude
        {
            get => latitude;
            set => SetProperty(ref latitude, value);
        }

        public double Longitude
        {
            get => longitude;
            set => SetProperty(ref longitude, value);
        }

        public string Category
        {
            get => category;
            set => SetProperty(ref category, value);
        }

        public string Address
        {
            get => address;
            set => SetProperty(ref address, value);
        }

        public Map Map { get; set; }

        public async void LoadLocationId(int Id)
        {
            Geocoder geoCoder = new Geocoder();
            try
            {
                location = await DataStore.GetAsync(Id);
                Id = location.Id;
                Name = location.Name;
                Description = location.Description;
                Latitude = location.Latitude;
                Longitude = location.Longitude;
                Position position = new Position(location.Latitude, location.Longitude);
                IEnumerable<string> possibleAddress = await geoCoder.GetAddressesForPositionAsync(position);
                Address = possibleAddress.FirstOrDefault();
                Distance = await DistanceCounter.GetFormattedDistance(new Locations(location.Latitude, location.Longitude));
                Category = CategoryConverter.ConvertCategoryToString((Category)location.Category);
                LoadMarker(Map, location, position, Address);
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Location");
            }
        }

        static public void LoadMarker(Map map, Location location, Position position, string address)
        {
            MapSpan mapSpan = new MapSpan(position, 0.01, 0.01);

            map.MoveToRegion(mapSpan, true);

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
