using System.ComponentModel;
using Encountify.Services;
using Encountify.Views;

namespace Encountify.ViewModels
{
    class ProfilePageViewModel : BaseViewModel, INotifyPropertyChanged
    {
        public bool IsCurrentScaleInMeters
        {
            get => DistanceCounter.IsInMeters;
        }
        public bool IsCurrentScaleInYards
        {
            get => !DistanceCounter.IsInMeters;
        }

        public ProfilePageViewModel()
        {
            LoginPage.OnLogin += OnLogin;
        }

        private void OnLogin()
        {
            OnPropertyChanged(nameof(Id));
            OnPropertyChanged(nameof(Username));
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(Password));
        }

        public int Id
        {
            get => App.UserID;
        }

        public string Username
        {
            get => App.UserName;
        }

        public string Email
        {
            get => App.UserEmail;
        }

        public string Password
        {
            get => App.UserPassword;
        }
        public void ChangeScale()
        {
            DistanceCounter.ChangeScale();
        }
    }
}
