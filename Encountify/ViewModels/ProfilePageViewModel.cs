using Encountify.Models;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using System.Threading.Tasks;
using Encountify.Services;
using System.Collections.Generic;
using System.Linq;

namespace Encountify.ViewModels
{
    class ProfilePageViewModel : BaseViewModel
    {
        private int id;
        private string username;
        private string email;
        private string password;

        public int Id
        {
            get
            {
                id = App.UserID;
                return id;
            }
            set
            {
                SetProperty(ref id, value);
                LoadProfile(value);
            }
        }

        public string Username
        {
            get
            {
                username = App.UserName;
                return username;
            }
            set => SetProperty(ref username, value);
        }

        public string Email
        {
            get
            {
                email = App.UserEmail;
                return email;
            }
            set => SetProperty(ref email, value);
        }

        public string Password
        {
            get
            {
                password = App.UserPassword;
                return password;
            }
            set => SetProperty(ref password, value);
        }

        public async void LoadProfile(int id)
        {
            try
            {
                User profile = new User();
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
