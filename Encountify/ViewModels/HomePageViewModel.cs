using Encountify.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace Encountify.ViewModels
{
    class HomePageViewModel : BaseViewModel
    {
        ImageSource _downloadedImageSource;

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
