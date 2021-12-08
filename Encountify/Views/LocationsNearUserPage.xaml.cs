using Xamarin.Forms;
using Encountify.ViewModels;

namespace Encountify.Views
{
    public partial class LocationsNearUserPage : ContentPage
    {
        LocationsNearUserViewModel _viewModel;

        public LocationsNearUserPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new LocationsNearUserViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}