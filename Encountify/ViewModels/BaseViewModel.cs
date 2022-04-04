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
        public static ILocationAccess LocationData = null;
        public static IAchievmentAccess AchievmentData = null;
        public static IAssignedAchievmentAccess AssignedAchievmentData = null;

        public BaseViewModel()
        {
            if (LocationData == null)
            {
                LocationData = DependencyService.Get<ILocationAccess>();
            }
            if (AchievmentData == null)
            {
                AchievmentData = DependencyService.Get<IAchievmentAccess>();
            }
            if (AssignedAchievmentData == null)
            {
                AssignedAchievmentData = DependencyService.Get<IAssignedAchievmentAccess>();
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
