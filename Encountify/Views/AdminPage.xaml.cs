using Encountify.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Encountify.Views
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminPage : ContentPage
    {
        AdminViewModel _viewAdmin;

        public AdminPage()
        {
            InitializeComponent();

            BindingContext = _viewAdmin = new AdminViewModel();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewAdmin.OnAppearing();
        }
    }
}