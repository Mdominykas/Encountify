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
        public static DatabaseAccess<User> Repository = null;

        public BaseViewModel()
        {
            if (DataStore == null)
            {
                DataStore = new DatabaseAccess<Location>();
                LoadLocationDummyData();
            }

            if (Repository == null)
            {
                Repository = new DatabaseAccess<User>();
                LoadUsers();
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
                new Location("Vilniaus katedra", coordX : 54.685849042698216, coordY :25.287750880122083, category: (int) Category.Cathedral),
                new Location("Gedimino bokštas", coordX : 54.68667445192699, coordY : 25.29056883194689, category : (int) Category.Castle ),
                new Location("Vilniaus Šv. Onos bažnyčia", coordX : 54.68378573230062, coordY : 25.292650881640785, category : (int) Category.Church ),
                new Location("Trys kryžiai", coordX : 54.68740766559662, coordY : 25.29771489238211, category: (int) Category.Monument ),
                new Location("Gedimino prospektas", coordX : 54.68644019450281, coordY : 25.285441103636185, category : (int) Category.Street),
                new Location("Lietuvos Respublikos Prezidento kanceliarija", coordX : 54.68383535000863, coordY : 25.286685648648888, category : (int)Category.None),
                new Location("Sereikiškių parko Bernardinų sodas", coordX : 54.68413305498285, coordY : 25.29522580235671, category : (int) Category.Park ),
                new Location("Lietuvos nacionalinis dailės muziejus", coordX : 54.68130476957157, coordY : 25.289818468853266, category : (int) Category.Museum ),
                new Location("Užupio angelas", coordX : 54.68035, coordY : 25.29515, category : (int) Category.Monument),
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

        public  void  LoadUsers()
        {
            DatabaseAccess<User> data = new DatabaseAccess<User>();
            var userList = (List<User>)data.GetAllAsync().Result;

            foreach(User user in userList)
            {
                Repository.AddAsync(user);
                Debug.WriteLine(user.Username + user.Id);
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
