using Encountify.Models;
using Encountify.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Encountify.ViewModels
{
    public class LocationsViewModel : BaseViewModel
    {
        private Location _selectedLocation;

        public ObservableCollection<Location> Locations { get; }
        public Command LoadLocationsCommand { get; }
        public Command AddLocationCommand { get; }
        public Command<Location> LocationTapped { get; }

        public LocationsViewModel()
        {
            Locations = new ObservableCollection<Location>();
            LoadLocationsCommand = new Command(async () => await ExecuteLoadLocationsCommand());

            LocationTapped = new Command<Location>(OnLocationSelected);

            AddLocationCommand = new Command(OnAddLocation);
        }

        async Task ExecuteLoadLocationsCommand()
        {
            IsBusy = true;

            try
            {
                Locations.Clear();
                var locations = await LocationData.GetAllAsync();
                foreach (var location in locations)
                {
                    Locations.Add(location);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedLocation = null;
        }

        public Location SelectedLocation
        {
            get => _selectedLocation;
            set
            {
                SetProperty(ref _selectedLocation, value);
                OnLocationSelected(value);
            }
        }

        private async void OnAddLocation(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewLocationPage));
        }

        async void OnLocationSelected(Location location)
        {
            if (location == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(LocationDetailPage)}?{nameof(LocationDetailViewModel.Id)}={location.Id}");
        }
    }
}