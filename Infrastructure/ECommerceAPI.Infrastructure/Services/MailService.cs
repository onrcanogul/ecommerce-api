using ECommerceAPI.Application.Abstractions.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Services
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

       

        public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendMailAsync(new[] { to },subject,body,isBodyHtml);
        }

        public async Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            MailMessage mail = new();
            mail.Subject = subject;
            mail.IsBodyHtml = isBodyHtml;
            mail.Body = body;
            foreach(var to in tos)
              mail.To.Add(to);

            mail.From = new(_configuration["Mail:Username"], "E-Commerce Application", System.Text.Encoding.UTF8);

            SmtpClient smtp = new()
            {
                Credentials = new NetworkCredential(_configuration["Mail:Username"], _configuration["Mail:Password"]),
                Port = 587,
                Host = "smtp.gmail.com",
                EnableSsl = true,
            };            // bu değerler appsettingsde olsa daha iyi hatta envde olsa daha daha iyi özellikle username password.
            await smtp.SendMailAsync(mail);
            
        }

        public async Task SendPasswordResetMailAsync(string to, string userId, string resetToken)
        {
            string mail = $"Merhaba <br> Eğer yeni bir şifre talebinde bulunduysanız aşağıdaki linkten şifrenizi yenileyebilirsiniz <br> <strong> <a target=\"_blank\" href=\"{_configuration["AngularClientUrl"]}/update-password/{userId}/{resetToken}\">Şifrenizi yenilemek için tıklayınız...</a> </strong> <br><br><br><span> NOT: Eğer ki bu talep tarafınızca gerçekleşmemişse lütfen bu maili ciddiye almayınız.</span><br><br>ECommerce-Application";
        
            await SendMailAsync(to, "Reset Password", mail);
            
        }
        public async Task SendCompletedOrderMailAsync(string to,string orderCode, DateTime orderDate, string userName)
        {
            string mail = $"Merhaba {userName} <br> {orderDate} tarihinde vermiş olduğunuz {orderCode} kodlu siparişiniz tamamlanmış ve kargo firmasına verilmiştir.";
            await SendMailAsync(to, "Sipariş Durumu", mail, true);
        }
    }
}
