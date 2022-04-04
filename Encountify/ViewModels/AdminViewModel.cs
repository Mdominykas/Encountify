using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Encountify.Models;
using Encountify.Views;
using Xamarin.Forms;

namespace Encountify.ViewModels
{
    public class AdminViewModel : BaseViewModel
    {
        private Location _selectedLocation;

        public ObservableCollection<Location> Locations { get; }
        public Command LoadLocationsCommand { get; }
        public Command AcceptLocationCommand { get; }
        public Command DenyLocationCommand { get; }
        public Command<Location> LocationTapped { get; }

        public AdminViewModel()
        {
            Locations = new ObservableCollection<Location>();

            LoadLocationsCommand = new Command(async () => await ExecuteLoadLocationsCommand());

            LocationTapped = new Command<Location>(OnLocationSelected);

            AcceptLocationCommand = new Command(OnAcceptLocation);

            DenyLocationCommand = new Command(OnDenyLocation);
        }
        private async Task ExecuteLoadLocationsCommand()
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

        private async void OnLocationSelected(Location location)
        {
            if (location == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(LocationDetailPage)}?{nameof(LocationDetailViewModel.Id)}={location.Id}");
        }

        private async void OnAcceptLocation()
        {
            //TODO
        }

        private async void OnDenyLocation()
        {
            //TODO
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
    }
}
