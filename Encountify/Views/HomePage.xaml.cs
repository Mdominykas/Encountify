using Encountify.Services;
using System;
using Xamarin.Forms;

namespace Encountify.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        public void OnColorsRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            if((e as CheckedChangedEventArgs).Value)
                DistanceCounter.ChangeScale();
        }

    }
}