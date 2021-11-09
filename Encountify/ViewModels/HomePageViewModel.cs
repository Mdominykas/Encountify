using System.Diagnostics;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Encountify.ViewModels
{
    class HomePageViewModel : BaseViewModel
    {
        private ImageSource _downloadedImageSource;
        private Stream _currentStream;
        public Command OnImageChangeCommand { get; }

        public HomePageViewModel()
        {
            OnImageChangeCommand = new Command(OnChangeImageButtonClicked);

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

        private async void OnChangeImageButtonClicked()
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions { Title = "Please pick a photo" });
                var newFile = Path.Combine(FileSystem.CacheDirectory, result.FileName);
                ImageOpenClose.Source = ImageSource.FromFile(newFile);
            }
            catch (PermissionException pEx)
            {
                // Permissions not granted
                Debug.WriteLine("you don't have permissions");
                Debug.WriteLine(pEx.ToString());
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature is not supported on the device
            }
        }

    }
}
