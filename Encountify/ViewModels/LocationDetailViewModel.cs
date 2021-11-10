using Encountify.Models;
using Encountify.Services;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace Encountify.ViewModels
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class LocationDetailViewModel : BaseViewModel
    {
        private long id;
        private string name;
        private string description;
        private double latitude;
        private double longitude;
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

        public Map Map { get; set; }

        public async void LoadLocationId(int Id)
        {
            try
            {
                location = await DataStore.GetAsync(Id);
                Id = location.Id;
                Name = location.Name;
                Description = location.Description;
                Latitude = location.Latitude;
                Longitude = location.Longitude;
                Distance = await DistanceCounter.GetFormattedDistance(location);
                Category = CategoryConverter.ConvertCategoryToString((Category)location.Category);
                LoadMarker(Map, location);
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Location");
            }
        }

        static public void LoadMarker(Map map, Location location)
        {
            Position position = new Position(location.Latitude, location.Longitude);
            MapSpan mapSpan = new MapSpan(position, 0.01, 0.01);
            map.MoveToRegion(mapSpan, true);
            Pin pin = new Pin()
            {
                Label = location.Name,
                Position = position,
            };

            if (!map.Pins.Contains(pin))
            {
                map.Pins.Add(pin);
            }
        }
    }
}
