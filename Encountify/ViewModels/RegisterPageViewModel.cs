using Encountify.Models;
using System;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Encountify.ViewModels
{
    class RegisterPageViewModel : BaseViewModel
    {
        public ICommand ForgotPasswordCommand => new Command(OnForgotPassword);
        public Command CancelCommand { get; }

        public RegisterPageViewModel()
        {
            CancelCommand = new Command(OnCancel);
        }

        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void OnForgotPassword()
        {
            string[] lines = System.IO.File.ReadAllLines(@"ServiceTerms.txt");
            StringBuilder sb = new StringBuilder();
            foreach (string line in lines)
                sb.AppendLine(line);
            await App.Current.MainPage.DisplayAlert("Service terms", sb.ToString(), "OK");
        }

    }
}
