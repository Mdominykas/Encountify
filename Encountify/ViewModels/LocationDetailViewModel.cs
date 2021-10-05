using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace Encountify.ViewModels
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class LocationDetailViewModel : BaseViewModel
    {
        private int id;
        private string name;
        private string description;
        private double coordX;
        private double coordY;

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

        public async void LoadLocationId(int Id)
        {
            try
            {
                var location = await DataStore.GetAsync(Id);
                Id = location.Id;
                Name = location.Name;
                Description = location.Description;
                CoordX = location.CoordX;
                CoordY = location.CoordY;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Location");
            }
        }
    }
}
