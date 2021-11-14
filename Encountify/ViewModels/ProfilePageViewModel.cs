using Encountify.Models;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using System.Threading.Tasks;
using Encountify.Services;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using Encountify.Views;

namespace Encountify.ViewModels
{
    class ProfilePageViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private ImageSource _downloadedImageSource;

        public ProfilePageViewModel()
        {
            LoginPage.OnLogin += OnLogin;
            OnPropertyChanged(nameof(DownloadedImageSource));
        }

        private void OnLogin()
        {
            OnPropertyChanged(nameof(Id));
            OnPropertyChanged(nameof(Username));
            OnPropertyChanged(nameof(Email));
            OnPropertyChanged(nameof(Password));
            OnPropertyChanged(nameof(DownloadedImageSource));
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

        public ImageSource DownloadedImageSource
        {
            get => _downloadedImageSource;
            set => SetProperty(ref _downloadedImageSource, value);
        }

        private Image _imageOpenClose;
        public Image ImageOpenClose
        {
            get
            {
                return _imageOpenClose;
            }
            set
            {
                _imageOpenClose = value;
                OnPropertyChanged();
            }
        }
    }
}
