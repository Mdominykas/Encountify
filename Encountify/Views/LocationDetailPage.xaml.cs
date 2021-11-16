using Encountify.ViewModels;
using Xamarin.Forms;

namespace Encountify.Views
{
    public partial class LocationDetailPage : ContentPage
    {
        LocationDetailViewModel _locationDetailViewModel;

        public LocationDetailPage()
        {
            InitializeComponent();
            BindingContext = _locationDetailViewModel = new LocationDetailViewModel();
            _locationDetailViewModel.Map = myMap;
        }
    }
}