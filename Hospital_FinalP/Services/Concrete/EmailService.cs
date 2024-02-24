using Hospital_FinalP.DTOs.EmailSender;
using Hospital_FinalP.Services.Abstract;
using Humanizer;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MimeKit.Text;
using System.Net.Mail;
using System.Net;
using SmtpClient = System.Net.Mail.SmtpClient;

namespace Hospital_FinalP.Services.Concrete
{
    public class EmailService : IEmailService
    {

        public async Task SendEmailAsync(string fromDisplayName, string to, string subject, string body, string? from = null)

        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(from);
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    smtp.Credentials = new NetworkCredential("demo1flight@gmail.com", "crvglwtaqkfpejxv");
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mail);
                }
            }
        }
    }
}

