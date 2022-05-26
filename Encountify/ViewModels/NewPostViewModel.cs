using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Encountify.ViewModels
{
    class NewPostViewModel : BaseViewModel
    {
        private string name;
        private string description;

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        public NewPostViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(description);
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

        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            /*Location location = new Location()
            {
                Name = Name,
                Description = Description,
                Latitude = Latitude,
                Longitude = Longitude,
                Category = (int)CategoryConverter.ConvertStringToCategory(Category)
            };

            await LocationData.AddAsync(location);*/

            await Shell.Current.GoToAsync("..");
        }
    }
}
