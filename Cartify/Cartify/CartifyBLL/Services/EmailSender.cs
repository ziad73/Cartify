using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CartifyBLL.Services
{
    public class EmailSender
    {
        //add "builder.Services.AddScoped<Signing.Services.EmailSender>();" in Program.cs after "builder.Services.AddControllersWithViews();"

        private readonly string smtpServer = "smtp.gmail.com";
        private readonly int smtpPort = 587;
        private readonly string smtpUser = "zzezzo424@gmail.com"; 
        private readonly string smtpPass = "dvpz gqdk felm kvrp";// Use App Password, not your Gmail password

        public async Task SendVerificationCodeAsync(string toEmail, string code)
        {
            var subject = "Your Verification Code";
            var body = $"<h3>Your verification code is:</h3><h2>{code}</h2>";
            await SendEmailAsync(toEmail, subject, body);
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            using var client = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };
            var mail = new MailMessage(smtpUser, toEmail, subject, body)
            {
                IsBodyHtml = true
            };
            await client.SendMailAsync(mail);
        }
    }
}