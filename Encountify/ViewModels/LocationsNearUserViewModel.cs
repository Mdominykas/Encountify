using Encountify.Models;
using Encountify.Services;
using Encountify.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Encountify.ViewModels
{
    class LocationsNearUserViewModel : BaseViewModel
    {
        private NearUserCell _locationTapped;

        public ObservableCollection<NearUserCell> NearLocations { get; }
        public Command LoadLocationsNearUser { get; }
        public Command<NearUserCell> LocationSelected { get; }

        public LocationsNearUserViewModel()
        {
            NearLocations = new ObservableCollection<NearUserCell>();
            LoadLocationsNearUser = new Command(async () => await ExecuteLoadLocationsNearUser());
            LocationSelected = new Command<NearUserCell>(OnLocationTapped);
        }

        async Task ExecuteLoadLocationsNearUser()
        {
            IsBusy = true;

            try
            {
                NearLocations.Clear();
                var nearLocationsCreation = new NearUserCreation();
                var list = await nearLocationsCreation.CreateListAsync();

                foreach (var item in list)
                {
                    NearLocations.Add(new NearUserCell(item));
                }

                if (NearLocations.Count == 0)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "Location disabled, cannot show locations near user. Showing all locations", "OK");
                    await Shell.Current.GoToAsync("//LocationsPage");
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Location disabled, cannot show locations near user. Showing all locations", "OK");
                await Shell.Current.GoToAsync("//LocationsPage");
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

        public NearUserCell TappedLocation
        {
            get => _locationTapped;
            set
            {
                SetProperty(ref _locationTapped, value);
                OnLocationTapped(value);
            }
        }

        async void OnLocationTapped(NearUserCell obj)
        {
            if (obj == null)
            {
                return;
            }
            await Shell.Current.GoToAsync($"{nameof(LocationDetailPage)}?{nameof(LocationDetailViewModel.Id)}={obj.LocationId}");
        }
        
    }
}
