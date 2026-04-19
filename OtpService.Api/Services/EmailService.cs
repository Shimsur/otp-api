using System.Net;
using System.Net.Mail;

namespace OtpService.Api.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                var smtpServer = _config["EmailSettings:SmtpServer"];
                var port = int.Parse(_config["EmailSettings:Port"]);
                var senderEmail = _config["EmailSettings:SenderEmail"];
                var password = _config["EmailSettings:Password"];

                var client = new SmtpClient(smtpServer, port)
                {
                    Credentials = new NetworkCredential(senderEmail, password),
                    EnableSsl = true
                };

                var mail = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mail.To.Add(toEmail);

                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw new Exception("Email sending failed: " + ex.Message);
            }
        }
    }
}