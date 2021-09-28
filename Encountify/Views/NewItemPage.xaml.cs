using Encountify.Models;
using Encountify.ViewModels;
using Xamarin.Forms;

namespace Encountify.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Location location { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}