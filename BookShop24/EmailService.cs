using MimeKit;
using MailKit.Net.Smtp;

namespace BookShop24
{
    public class EmailService
    {
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "web2023@ro.ru"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.rambler.ru", 465, true);
                await client.AuthenticateAsync("web2023@ro.ru", "Souznaya144");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
} //pop.rambler.ru  smtp.rambler.ru