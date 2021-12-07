using Encountify.Models;
using Xamarin.Forms;

namespace Encountify.ViewModels
{
    [QueryProperty(nameof(Longitude), "Longitude")]
    [QueryProperty(nameof(Latitude), "Latitude")]
    public class NewLocationViewModel : BaseViewModel
    {
        private string name;
        private string description;
        private double longitude;
        private double latitude;
        private string category;

        public NewLocationViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            SelectCommand = new Command(OnSelect);
            PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !string.IsNullOrWhiteSpace(name)
                && !string.IsNullOrWhiteSpace(description);
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

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public Command SelectCommand { get; }

        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            Location location = new Location()
            {
                Name = Name,
                Description = Description,
                Latitude = Latitude,
                Longitude = Longitude,
                Category = (int)CategoryConverter.ConvertStringToCategory(Category)
            };

            await LocationData.AddAsync(location);

            await Shell.Current.GoToAsync("..");
        }

        private async void OnSelect()
        {
            await Shell.Current.GoToAsync("MapPage");
        }
    }
}
