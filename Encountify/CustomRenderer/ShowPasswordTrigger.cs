using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace Encountify.CustomRenderer
{
    class ShowPasswordTrigger : TriggerAction<Button>, INotifyPropertyChanged
    {
        public string ShowIcon { get; set; }
        public string HideIcon { get; set; }

        string _icon;
        bool _hidePassword = true;

        public bool HidePassword
        {
            get => _hidePassword;
            set
            {
                if (_hidePassword != value)
                {
                    _hidePassword = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HidePassword)));
                }
            }
        }

        protected override void Invoke(Button sender)
        {
            sender.Text = HidePassword ? ShowIcon : HideIcon;
            HidePassword = !HidePassword;
        }

        public event PropertyChangedEventHandler PropertyChanged;


    }
}
