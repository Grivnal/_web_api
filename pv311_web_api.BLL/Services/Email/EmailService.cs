using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace pv311_web_api.BLL.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _email;

        public EmailService(IConfiguration configuration)
        {
            _email = configuration["SmtpSettings:Email"] ?? "";
            string password = configuration["SmtpSettings:Password"] ?? "";
            string host = configuration["SmtpSettings:Host"] ?? "";
            int port = int.Parse(configuration["SmtpSettings:Port"] ?? "");

            _smtpClient = new SmtpClient(host, port);
            _smtpClient.EnableSsl = true;
            _smtpClient.Credentials = new NetworkCredential(_email, password);
        }


        public async Task SendMailAsync(MailMessage message)
        {
            await _smtpClient.SendMailAsync(message);
        }

        public async Task SendMailAsync(string to, string subject, string body, bool isHtml = false)
        {
            var message = new MailMessage();
            message.From = new MailAddress(_email);
            message.To.Add(to);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = isHtml;
            await SendMailAsync(message);
        }
    }
}