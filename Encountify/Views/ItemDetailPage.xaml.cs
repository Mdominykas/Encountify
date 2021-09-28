using Encountify.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Encountify.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}