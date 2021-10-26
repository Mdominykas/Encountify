using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using SQLite;
using Encountify.Models;
using Encountify.CustomRenderer;
using Encountify.Services;
using System.Text.RegularExpressions;
using Xunit.Sdk;
using Encountify.ViewModels;
using MimeKit;
using MailKit.Net.Smtp;
using System.Diagnostics;
using System.Security.Authentication;
using MailKit.Security;

namespace Encountify.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPasswordPage : ContentPage
    {

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
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "Users.db3");
            SQLiteConnection db = new SQLiteConnection(dbPath);
            db.CreateTable<User>();
            var data = db.Table<User>();
            var dataUser = data.Where(x => x.Email == Email.Text).FirstOrDefault();
            if (dataUser != null)
            {
                SendEmail(dataUser);
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