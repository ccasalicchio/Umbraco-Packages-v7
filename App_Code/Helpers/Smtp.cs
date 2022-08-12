using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;
namespace RevistaUFO.Helpers
{
    /// <summary>
    /// Summary description for Smtp
    /// </summary>
    public class Smtp
    {
        string emailMessageHtml;
        System.Net.Mail.SmtpClient SmtpServer;
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Hostname { get; set; }
        public bool UseSSL { get; set; }
        public string Subject { get; set; }
        public string From { get; set; }
        public string FromName { get; set; }
        public string To { get; set; }
        public string ToName { get; set; }
        public string Message { get; set; }
        public string UsingTemplate(string template, object values)
        {
            foreach (var prop in values.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                template = template.Replace("#" + prop.Name + "#", values.GetType().GetProperty(prop.Name).GetValue(values, null).ToString());
            return template;
        }
        void SetupServer()
        {
            try
            {
                SmtpServer = new System.Net.Mail.SmtpClient();
                SmtpServer.Port = Port;
                SmtpServer.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

                SmtpServer.UseDefaultCredentials = string.IsNullOrEmpty(Username) && string.IsNullOrEmpty(Password);

                if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
                    SmtpServer.Credentials = new System.Net.NetworkCredential(Username, Password);

                SmtpServer.Host = Hostname;
                SmtpServer.EnableSsl = UseSSL;
            }
            catch
            {
                SmtpServer = null;
            }

        }
        public bool Send()
        {
            SetupServer();

            if (SmtpServer != null)
            {
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();

                mail.Subject = Subject;
                mail.From = new System.Net.Mail.MailAddress(From, FromName);
                mail.IsBodyHtml = true;

                mail.To.Add(new System.Net.Mail.MailAddress(To, ToName));
                mail.Body = Message;

                try
                {
                    SmtpServer.Send(mail);
                    return true;
                }
                catch (Exception ex)
                {
                    Debug.Write(ex.Message);
                    return false;
                }
                finally
                {
                    SmtpServer.Dispose();
                }
            }
            return false;

        }

    }
}