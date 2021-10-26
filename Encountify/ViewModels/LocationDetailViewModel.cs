using Encountify.Models;
using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace Encountify.ViewModels
{
    [QueryProperty(nameof(Id), nameof(Id))]
    public class LocationDetailViewModel : BaseViewModel
    {
        private long id;
        private string name;
        private string description;
        private double coordX;
        private double coordY;
        private string category;


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
                Category = CategoryConverter.ConvertCategoryToString((Category)location.Category);
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Location");
            }
        }
    }
}
