using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Encountify.Models;
using Encountify.Services;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Linq;

namespace Encountify.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPasswordPage : ContentPage
    {
        public static DatabaseAccess<User> DataStore = new DatabaseAccess<User>();

        public ForgotPasswordPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
            ResetValues();
            ResetFocus();
        }

        private async void OnSendPasswordClicked(object sender, EventArgs e)
        {
            var users = await DataStore.GetAllAsync(true);
            var user = users.Where(x => x.Email == Email.Text).FirstOrDefault();
            if (user != null)
            {
                SendEmail(user);
                await DisplayAlert("Success!", $"Email was sent to {Email.Text}", "OK");
            }
            else
            {
                await DisplayAlert("Warning", "No such email", "OK");
            }
        }

        private async void SendEmail(User user)
        {
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("Encountify", "encountify@gmail.com"));
            mailMessage.To.Add(new MailboxAddress(user.Username, Email.Text));
            mailMessage.Subject = "Forgot password?";
            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $"<b>Your username:</b> {user.Username}<br><b>Your password:</b> {user.Password}";
            mailMessage.Body = bodyBuilder.ToMessageBody();

            // TODO: Get data from .env instead of writing it here
            // TODO: Move to a separate class
            using (var client = new SmtpClient())
            {
                client.CheckCertificateRevocation = false;
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                client.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
                client.Authenticate("encountify@gmail.com", "Encountify123.");
                await client.SendAsync(mailMessage);
                client.Disconnect(true);
            }
        }

        private void ResetValues()
        {
            Email.Text = string.Empty;
        }

        private void ResetFocus()
        {
            Email.Unfocus();
        }
    }
}