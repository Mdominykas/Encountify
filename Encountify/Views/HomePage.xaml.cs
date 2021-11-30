using Encountify.Services;
using Encountify.ViewModels;
using System;
using System.IO;
using Xamarin.Forms;

namespace Encountify.Views
{
    public partial class HomePage : ContentPage
    {
        HomePageViewModel viewModel;
        public HomePage()
        {
            InitializeComponent();
            BindingContext = viewModel = new HomePageViewModel();
            viewModel.ImageOpenClose = new Image();
            viewModel.ImageOpenClose.Source = ImageSource.FromStream(() => new MemoryStream(App.UserPicture));
        }

        private async void OnLocationsClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LocationsNearUserPage");
        }
    }
}
