using Encountify.Models;
using Encountify.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace Encountify.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        //public IDataStore<Location> DataStore => DependencyService.Get<IDataStore<Location>>();
        public static DatabaseAccess<Location> DataStore = null;

        public BaseViewModel()
        {
            if (DataStore == null)
            {
                DataStore = new DatabaseAccess<Location>();
                LoadLocationDummyData();
            }
        }

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public List<Location> DummyLocationList()
        {
            return new List<Location>()
            {
                new Location() { Name = "Vilniaus katedra", CoordX = 54.685849042698216, CoordY = 25.287750880122083, Category = (int) Category.Cathedral },
                new Location() { Name = "Gedimino bokštas", CoordX = 54.68667445192699, CoordY = 25.29056883194689, Category = (int) Category.Castle },
                new Location() { Name = "Vilniaus Šv. Onos bažnyčia", CoordX = 54.68378573230062, CoordY = 25.292650881640785, Category = (int) Category.Church },
                new Location() { Name = "Trys kryžiai", CoordX = 54.68740766559662, CoordY = 25.29771489238211, Category = (int) Category.Monument },
                new Location() { Name = "Gedimino prospektas", CoordX = 54.68644019450281, CoordY = 25.285441103636185, Category = (int) Category.Street },
                new Location() { Name = "Lietuvos Respublikos Prezidento kanceliarija", CoordX = 54.68383535000863, CoordY = 25.286685648648888, Category = (int)Category.None},
                new Location() { Name = "Sereikiškių parko Bernardinų sodas", CoordX = 54.68413305498285, CoordY = 25.29522580235671, Category = (int) Category.Park },
                new Location() { Name = "Lietuvos nacionalinis dailės muziejus", CoordX = 54.68130476957157, CoordY = 25.289818468853266, Category = (int) Category.Museum },
                new Location() { Name = "Užupio angelas", CoordX = 54.68035, CoordY = 25.29515, Category = (int) Category.Monument}
            };
        }

        public void LoadLocationDummyData()
        {
            var locationList = DummyLocationList();
            foreach (Location location in locationList)
            {
                DataStore.AddAsync(location);
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
