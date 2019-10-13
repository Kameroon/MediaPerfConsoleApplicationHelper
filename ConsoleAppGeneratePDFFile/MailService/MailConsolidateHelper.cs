using System;
using System.Linq;
using System.Net.Mail;

namespace ConsoleAppGeneratePDFFile.MailService
{
    public static class MailConsolidateHelper
    {
        /// <summary>
        /// --   --
        /// </summary>
        /// <param name="body"></param>
        /// <param name="htmlTemplate"></param>
        /// <param name="currentMonth"></param>
        /// <param name="toEmail"></param>
        /// <returns></returns>
        public static string BuilBody(string body, string htmlTemplate,
            string currentMonth, string toEmail)
        {
            body = htmlTemplate.Replace("{E}", toEmail.Split('@')[0]);
            body = body.Replace("{C}", currentMonth);
            body += "\n\rCeci est un message automatique.";

            return body;
        }

        /// <summary>
        /// --  --
        /// </summary>
        /// <param name="adminEmail"></param>
        /// <param name="mailMessage"></param>
        public static void CheckCcAndBcc(string adminEmail, MailMessage mailMessage)
        {
            Console.WriteLine("Destiné à : " + mailMessage.To.FirstOrDefault());
            mailMessage.Subject += " - To : " + mailMessage.To.FirstOrDefault();
            mailMessage.To.Clear();
            mailMessage.To.Add(adminEmail);
            mailMessage.CC.Clear();
            mailMessage.Bcc.Clear();
            mailMessage.Bcc.Add(adminEmail);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void BuildMailDetail(IEmailMessage emailMessage, string corpsDuMail,
            string sujet, string itemValue, string itemKey, string bcc,
            string cc, string adminEmail, bool isPreviewEmail)
        {
            emailMessage.MailBody = corpsDuMail;
            emailMessage.Suject = sujet;
            emailMessage.FilePath = itemValue;
            emailMessage.ToEmail = itemKey;
            emailMessage.Bcc = bcc;
            emailMessage.Cc = cc;
            emailMessage.AdminEmail = adminEmail;
            emailMessage.IsPreviewMail = isPreviewEmail;
        }
    }
}
