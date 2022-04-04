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
    public partial class AchievmentsPage : ContentPage
    {
        AchievmentsViewModel _viewModel;

        public AchievmentsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new AchievmentsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}