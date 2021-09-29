using Encountify.Models;
using Encountify.ViewModels;
using Xamarin.Forms;

namespace Encountify.Views
{
    public partial class NewLocationPage : ContentPage
    {
        public Location location { get; set; }

        public NewLocationPage()
        {
            InitializeComponent();
            BindingContext = new NewLocationViewModel();
        }
    }
}