using Encountify.Models;
using Encountify.Services;
using Encountify.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Locations = Xamarin.Essentials.Location;

namespace Encountify.ViewModels
{
    class LocationsNearUserViewModel : BaseViewModel
    {
        private Location _locationTapped;
        public ObservableCollection<NearUserCell> NearLocations { get; }
        public Command LoadLocationsNearUser { get; }
        public Command<Location> LocationSelected { get; }

        public LocationsNearUserViewModel()
        {
            NearLocations = new ObservableCollection<NearUserCell>();
            LoadLocationsNearUser = new Command(async () => await ExecuteLoadLocationsNearUser());
            LocationSelected = new Command<Location>(OnLocationTapped);
        }

        async Task ExecuteLoadLocationsNearUser()
        {
            IsBusy = true;

            try
            {
                NearLocations.Clear();
                var nearLocationsCreation = new NearUserCreation();
                var list = await nearLocationsCreation.CreateListAsync();

                foreach(var itiem in list)
                {
                    NearLocations.Add(new NearUserCell(itiem));
                }

                if (NearLocations.Count == 0 ) //IT WORKS
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Location disabled, cannot show locations near user. Showing all locations", "OK");
                    await Shell.Current.GoToAsync("//LocationsPage");
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
            TappedLocation = null;
        }

        public Location TappedLocation
        {
            get => _locationTapped;
            set
            {
                SetProperty(ref _locationTapped, value);
                OnLocationTapped(value);
            }
        }

        async void OnLocationTapped(Location obj)
        {
            if (obj == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(LocationDetailPage)}?{nameof(LocationDetailViewModel.Id)}={obj.Id}");
        }
        
    }
}
