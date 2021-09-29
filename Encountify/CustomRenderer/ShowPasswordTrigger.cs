// Turn <Button> into a trigger for password viewing

using System.ComponentModel;
using Xamarin.Forms;

namespace Encountify.CustomRenderer
{
    class ShowPasswordTrigger : TriggerAction<Button>, INotifyPropertyChanged
    {
        public string ShowIcon { get; set; }
        public string HideIcon { get; set; }

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