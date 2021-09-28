using Encountify.Views;
using System;
using Xamarin.Forms;

namespace Encountify
{

    public partial class AppShell : Xamarin.Forms.Shell
    {

        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("RegisterPage", typeof(RegisterPage));
            Routing.RegisterRoute("ItemDetailPage", typeof(ItemDetailPage));
            Routing.RegisterRoute("NewItemPage", typeof(NewItemPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }

    }

}