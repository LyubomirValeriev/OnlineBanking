using System.Net.Mail;
using System.Net;

namespace OnlineBanking
{
    public class EmailService
    {
        public EmailService() { }

        public void SendEmail(string to, string subject, string message)
        {
            var client = new SmtpClient("smtp.mailtrap.io", 2525)
            {
                Credentials = new NetworkCredential("c9c09a3e2daa7f", "856506f9a8d8d1"),
                EnableSsl = true
            };
            client.Send("onlinebanking@test.com", to, subject, message);
        }
    }
}
