using Encountify.Models;
using Encountify.ViewModels;
using Xamarin.Forms;

namespace Encountify.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}