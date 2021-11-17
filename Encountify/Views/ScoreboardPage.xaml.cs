using Encountify.Models;
using Encountify.Services;
using Encountify.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Encountify.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    // commenting cause I can't understand what it does
    public partial class ScoreboardPage : ContentPage
    {
        ScoreboardPageViewModel _viewModel;
        public ScoreboardPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ScoreboardPageViewModel();
            //_viewModel.CreateScoreboard();
        }

        protected override async void OnAppearing()
        {
            //_viewModel.CreateScoreboard();

            var a = 5;
        }
    }
}