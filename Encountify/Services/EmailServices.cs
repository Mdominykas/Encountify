using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Threading.Tasks;

namespace Encountify.Services
{
    public class EmailServices
    {
        // TODO: Get data from .env or somewhere else instead of writing it here
        private const string Host = "smtp.gmail.com";
        private const int Port = 465;
        private const string AuthorName = "Encountify";
        private const string AuthorEmail = "encountify@gmail.com";
        private const string AuthorPassword = "Encountify123.";
        private const SecureSocketOptions Ssl = SecureSocketOptions.SslOnConnect;

        public MimeMessage Message(string RecipientName, string RecipientEmail, string Subject, string Content)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress(AuthorName, AuthorEmail));
            message.To.Add(new MailboxAddress(RecipientName, RecipientEmail));
            message.Subject = Subject;
            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = Content;
            message.Body = bodyBuilder.ToMessageBody();
            return message;
        }

        public async Task SendEmail(string Content, string RecipientName = AuthorName, string RecipientEmail = AuthorEmail, string Subject = "Untitled")
        {
            MimeMessage message = Message(RecipientName, RecipientEmail, Subject, Content);

            using (var client = new SmtpClient())
            {
                client.CheckCertificateRevocation = false;
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
                await client.ConnectAsync(Host, Port, Ssl);
                await client.AuthenticateAsync(userName: AuthorEmail, password: AuthorPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
