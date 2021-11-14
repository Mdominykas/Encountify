﻿using System;
using Encountify.ViewModels;
using Xamarin.Forms;
using System.IO;

namespace Encountify.Views
{
    public partial class ProfilePage : ContentPage
    {
        ProfilePageViewModel viewModel;
        public ProfilePage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ProfilePageViewModel();
            viewModel.ImageOpenClose = new Image();
            viewModel.ImageOpenClose.Source = ImageSource.FromStream(() => new MemoryStream(App.UserPicture));
        }

        private async void LogOutButton_onClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Quit", "You want to log out?", "Yes");
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}