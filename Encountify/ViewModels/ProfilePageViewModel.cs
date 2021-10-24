using Encountify.Models;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace Encountify.ViewModels
{
    [QueryProperty(nameof(Id), nameof(Id))]
    class ProfilePageViewModel : BaseViewModel
    {
        private int id;
        private string username;
        private string email;
        private string password;

        public int Id
        {
            get => id;
            set
            {
                id = App.UserID;
                LoadProfile(App.UserID);
            }
        }

        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);
        }

        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public async void LoadProfile(int id)
        {
            try
            {
                var profile = await Repository.GetAsync(id);
                Id = profile.Id;
                Username = profile.Username;
                Password = profile.Password;
                Email = profile.Email;

            }
            catch(Exception)
            {
                Debug.WriteLine("Profile cannot be loaded");
            }
            
        }
    }
}
