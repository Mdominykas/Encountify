using Encountify.Models;
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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewPostPage : ContentPage
    {
        public UserPost userPost { get; set; }

        public NewPostPage()
        {
            InitializeComponent();
            BindingContext = new NewPostViewModel();
        }
    }
}