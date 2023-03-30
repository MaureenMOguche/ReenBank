using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RB.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MimeMessage emailtosend = new MimeMessage();
            emailtosend.From.Add(MailboxAddress.Parse("oguchemaureenm@gmail.com"));
            emailtosend.To.Add(MailboxAddress.Parse(email));
            emailtosend.Subject = subject;
            emailtosend.Body = new TextPart(MimeKit.Text.TextFormat.Html){ Text = htmlMessage };
            

            using (var emailClient = new SmtpClient())
            {
                emailClient.Connect("smtp.gmail.com", 465, true);
                emailClient.Authenticate("oguchemaureenm@gmail.com", "fijtvyjxobihihej");
                emailClient.Send(emailtosend);
                emailClient.Disconnect(true);
            }
        
            return Task.CompletedTask;
        }
    }
}
