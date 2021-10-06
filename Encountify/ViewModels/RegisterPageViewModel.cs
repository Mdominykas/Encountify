using System.IO;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace Encountify.ViewModels
{
    class RegisterPageViewModel : BaseViewModel
    {
        public ICommand ReadServiceTermsCommand => new Command(OnReadServiceTerms);
        public Command CancelCommand { get; }

        public RegisterPageViewModel()
        {
            CancelCommand = new Command(OnCancel);
        }

        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void OnReadServiceTerms()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(LoadResourceText)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("Encountify.ServiceTerms.txt");
            string terms = "";
            using (var reader = new StreamReader(stream))
            {
                terms = reader.ReadToEnd();
            }
            await App.Current.MainPage.DisplayAlert("Service terms", terms, "OK");
        }

    }
}
