using System;
using System.Collections.Generic;
using System.Web;
namespace ContactForm
{
    /// <summary>
    /// Summary description for Mailer
    /// </summary>
    public class Mailer
    {
        private string subject,body,from,from_name,to,to_name,smtp_usr,smtp_pwd,smtp_host;
        private int port;

        public Mailer()
        {
            //Definir valores padrão
            //Assunto
            subject = "Email recebido";
            //Corpo do email
            body="<h3>Email Recebido</h3><p><h3>Nome</h3>{0}<h3>E-mail</h3>{1}<h3>Fone</h3>{2}<h3>Cidade</h3>{3}<h3>Estado</h3>{4}<h3>Assunto</h3>{5}<h3>Categoria</h3>{6}<h3>Mensagem</h3>{7}<h3>CPF</h3>{8}<h3>Número do conselho</h3>{9}<h3>Tipo de Cliente</h3>{10}</p>";
            //Remetente
            from="testeremetente@gsk.com";
            from_name="Mailer do Site";
            //Destinatário
            to="desenvolvimento@ewinfo.com.br";
            to_name="Webmaster do Site";
            //Dados do Servidor
            smtp_usr=""; //usuário smtp
            smtp_pwd=""; //senha smtp
            smtp_host="mc.uspln2.aops-EDS.com"; //servidor smtp
            port=25; //porta de envio
        }
        public string SendMail(string[] userData)
        {
            var name = userData[0];
            var email = userData[1];
            var phone = userData[2];
            var city = userData[3];
            var state = userData[4];
            var subject = userData[5];
            var subSubject = userData[6];
            var message = userData[7];
            var cpf = userData[8];
            var permit = userData[9];
            var type = userData[10];
      
		    System.Net.Mail.SmtpClient SmtpServer = new System.Net.Mail.SmtpClient();
		    SmtpServer.Port = port;
		    SmtpServer.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
		    SmtpServer.UseDefaultCredentials = true;
		    SmtpServer.Credentials = new System.Net.NetworkCredential(smtp_usr,smtp_pwd);
		    SmtpServer.Host = smtp_host;
		
		    System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
		
		    mail.From = new System.Net.Mail.MailAddress(from, from_name);
		    mail.To.Add(new System.Net.Mail.MailAddress(to,to_name));
		    mail.Subject = subject;
		    mail.Body = string.Format(body,name,email,phone,city,state,subject,subSubject,message,cpf,permit,type);
		    mail.IsBodyHtml = true;
		
		    try
		    {
			    SmtpServer.Send(mail);
			    return "OK";
		    }
		    catch (Exception)
		    {
			    return "Erro ao enviar a mensage. tente novamente.";
		    }
        }
    }
}