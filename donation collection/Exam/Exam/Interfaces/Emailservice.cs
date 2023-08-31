using MailKit.Net.Smtp;
using MimeKit;

namespace Exam.Interfaces
{
    public class EmailService
    {
        private const string SmtpHost = "mail.mailtest.radixweb.net";
        private const int SmtpPort = 465;
        private const string SmtpUsername = "testdotnet@mailtest.radixweb.net";
        private const string SmtpPassword = "Radix@web#8";

        public void SendEmail(string recipient, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("", SmtpUsername));
            message.To.Add(new MailboxAddress("", recipient));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = body;


            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                client.Connect(SmtpHost, SmtpPort);
                client.Authenticate(SmtpUsername, SmtpPassword);
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
