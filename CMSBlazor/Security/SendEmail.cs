using System;
using System.Net;
using System.Net.Mail;

namespace CMSBlazor.Security
{
    public static class SendEmail
    {
        private static string? randomCode;
        //[AllowAnonymous]
        public static string? sendEmail(string email, string subject, string messageBody)
        {

            string from, to, password, code;
            //============Random==============
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_+@";
            var stringChars = new char[8];
            var random = new Random();
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            randomCode = new String(stringChars);
            //===================================
            code = "<h3 style=color:Blue;>" + randomCode + "</h3>";
            MailMessage message = new MailMessage();
            to = email;
            from = "samerrazzaqt@gmail.com";
            password = "hshwyakynarqznet";
            messageBody = messageBody + code;
            message.To.Add(to);
            message.From = new MailAddress(from);
            message.Body = messageBody;
            message.Subject = subject;
            message.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.EnableSsl = true;
            smtp.Port = 587;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential(from, password);
            try
            {
                smtp.Send(message);
                return randomCode;
            }
            catch { }
            return null;
        }

    }
}

