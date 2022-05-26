using Encountify.Models;
using Encountify.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Encountify.ViewModels
{
    public class UserPostsViewModel : BaseViewModel
    {
        private UserPostsViewModel _selectedPost;

        public ObservableCollection<UserPost> Posts { get; }
        public Command LoadPostsCommand { get; }
        public Command AddPostCommand { get; }

        public UserPostsViewModel()
        {
            Posts = new ObservableCollection<UserPost>();
            LoadPostsCommand = new Command(async () => await ExecuteLoadPostsCommand());

            AddPostCommand = new Command(OnAddPost);
        }

        async Task ExecuteLoadPostsCommand()
        {
            IsBusy = true;

            Posts.Clear();

            Posts.Add(new UserPost()
            {
                User = "John Smith",
                Description = "I had so much fun visiting Gediminas tower!"
            });

            Posts.Add(new UserPost()
            {
                User = "Helen",
                Description = "I don't recommend going to (insert bad place)."
            });

            IsBusy = false;
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        private async void OnAddPost(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewPostPage));
        }
    }
}
