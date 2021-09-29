// Enable toast messages

namespace Encountify.CustomRenderer
{
    public interface MessagePopup
    {
        void LongAlert(string message);
        void ShortAlert(string message);
    }
}
