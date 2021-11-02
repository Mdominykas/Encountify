using Encountify.ViewModels;
using Xamarin.Forms;

namespace Encountify.Views
{
    public partial class LocationDetailPage : ContentPage
    {
        public LocationDetailPage()
        {
            InitializeComponent();
            BindingContext = new LocationDetailViewModel();
        }

/*        protected override async void OnAppearing()
        {
            
        }
*/    }
}