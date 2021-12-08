using Encountify.Models;
using Encountify.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Encountify.ViewModels
{
    class HomePageViewModel : BaseViewModel
    {
        private ImageSource _downloadedImageSource;
        public Command OnImageChangeCommand { get; }
        IUserAccess users;

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
            byte[] newPicture = null;
            try
            {
                var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions { Title = "Please pick a photo" });
                var newFile = Path.Combine(FileSystem.CacheDirectory, result.FileName);
                try
                {
                    // documentation says that this using FullPath might cause errors
                    File.Copy(result.FullPath, newFile, true);
                }
                catch (FileNotFoundException fnfe)
                {
                    newFile = null;
                    Debug.WriteLine(fnfe.ToString());
                }
                catch (IOException iox)
                {
                    Debug.WriteLine(iox.Message);
                }
                ImageOpenClose.Source = ImageSource.FromFile(newFile);
                newPicture = File.ReadAllBytes(newFile);
            }
            catch (PermissionException pEx)
            {
                // Permissions not granted
                Debug.WriteLine("you don't have permissions");
                Debug.WriteLine(pEx.ToString());

                //await DisplayAlert("Error", "Image can not be selected without permissions", "Ok");

            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature is not supported on the device
                Debug.WriteLine("feature is not supported");
                Debug.WriteLine(fnsEx.ToString());
            }
            catch (NullReferenceException nrE)
            {
                // No image has been selected
                Debug.WriteLine("no image has been selected");
                Debug.WriteLine(nrE.ToString());
            }
            if (newPicture != null)
            {
                Debug.WriteLine("Commenting these lines as API currently does not seems to be good for images");
                Debug.WriteLine("Will try to deal with it later on");
/*                users = DependencyService.Get<IUser>(); //new DatabaseAccess<User>();
                var newData = await users.GetAsync(App.UserID);
                newData.Picture = newPicture;
                await users.UpdateAsync(newData);
*/            }
        }

        private Task DisplayAlert(string v1, string v2, string v3)
        {
            throw new NotImplementedException();
        }
    }
}
