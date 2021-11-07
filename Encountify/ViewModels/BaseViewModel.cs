using Encountify.Models;
using Encountify.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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
                new Location("Vilniaus katedra", latitude : 54.685849042698216, longitude :25.287750880122083, category: (int) Category.Cathedral),
                new Location("Gedimino bokštas", latitude : 54.68667445192699, longitude : 25.29056883194689, category : (int) Category.Castle ),
                new Location("Vilniaus Šv. Onos bažnyčia", latitude : 54.68378573230062, longitude : 25.292650881640785, category : (int) Category.Church ),
                new Location("Trys kryžiai", latitude : 54.68740766559662, longitude : 25.29771489238211, category: (int) Category.Monument ),
                new Location("Gedimino prospektas", latitude : 54.68644019450281, longitude : 25.285441103636185, category : (int) Category.Street),
                new Location("Lietuvos Respublikos Prezidento kanceliarija", latitude : 54.68383535000863, longitude : 25.286685648648888, category : (int)Category.None),
                new Location("Sereikiškių parko Bernardinų sodas", latitude : 54.68413305498285, longitude : 25.29522580235671, category : (int) Category.Park ),
                new Location("Lietuvos nacionalinis dailės muziejus", latitude : 54.68130476957157, longitude : 25.289818468853266, category : (int) Category.Museum ),
                new Location("Užupio angelas", latitude : 54.68035, longitude : 25.29515, category : (int) Category.Monument),
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
