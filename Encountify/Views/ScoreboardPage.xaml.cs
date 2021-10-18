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
    public partial class ScoreboardPage : ContentPage
    {
        ScoreboardPageViewModel _viewModel;
        public ScoreboardPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ScoreboardPageViewModel();
        }
    }
}