using Encountify.Services;
using Encountify.ViewModels;
using System;
using System.IO;
using System.Reflection;
using Xamarin.Forms;

namespace Encountify.Views
{
    public partial class HomePage : ContentPage
    {
        HomePageViewModel viewModel;
        public HomePage()
        {
            InitializeComponent();
            BindingContext = viewModel = new HomePageViewModel();
            viewModel.ImageOpenClose = new Image();
            viewModel.ImageOpenClose.Source = ImageCreator.GetDefaultImageStream();
        }

        public void OnScaleRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            if((e as CheckedChangedEventArgs).Value)
                DistanceCounter.ChangeScale();
        }
    }
}