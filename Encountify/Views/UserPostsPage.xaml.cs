using Encountify.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Encountify.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserPostsPage : ContentPage
    {
        UserPostsViewModel _viewModel;

        public UserPostsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new UserPostsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}