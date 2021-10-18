using Encountify.Models;
using System;
using Xamarin.Forms;

namespace Encountify.ViewModels
{
    [QueryProperty(nameof(CoordX), "CoordX")]
    [QueryProperty(nameof(CoordY), "CoordY")]
    public class NewLocationViewModel : BaseViewModel
    {
        private string name;
        private string description;
        private double coordX;
        private double coordY;
        private string category;

        public NewLocationViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            SelectCommand = new Command(OnSelect);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(name)
                && !String.IsNullOrWhiteSpace(description);
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

        public double CoordX
        {
            get => coordX;
            set => SetProperty(ref coordX, value);
        }

        public double CoordY
        {
            get => coordY;
            set => SetProperty(ref coordY, value);
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
                CoordX = CoordX,
                CoordY = CoordY,
                Category = (int)CategoryConverter.ConvertStringToCategory(Category)
            };

            await DataStore.AddAsync(location);

            await Shell.Current.GoToAsync("..");
        }

        private async void OnSelect()
        {
            await Shell.Current.GoToAsync("MapPage");
        }
    }
}
