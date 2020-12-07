using Ehr.Common.Constraint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Web;

namespace Ehr.Common.Tools
{
    public class EZMailer
    {
        public void DoSend(String server, int port, int timeout, Boolean SSL, String From, String To, String Subject, String Content, String smtpuser, String smtppass)
        {
            SmtpClient client = new SmtpClient();
            client.Port = port;
            client.Host = server;
            client.EnableSsl = SSL;
            //client.Timeout = timeout;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(smtpuser, smtppass);// ý e là lỗi này à
            MailMessage mm = new MailMessage(From, To, Subject, Content);
            mm.IsBodyHtml = true;
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            client.Send(mm);
        }
		public static void SendEmail ( String server,int port,int timeout,Boolean SSL,String From,String To,String Subject,String Content,String smtpuser,String smtppass,string cc,string bcc )
		{
			try
			{

				Thread email = new Thread ( delegate ( )
				  {
					  SendAsyncEmail ( server,port,timeout,SSL,From,To,Subject,Content,smtpuser,smtppass,cc,bcc );
				  } );
				email.IsBackground = true;
				email.Start ( );
			}
			catch
			{

			}
		}
		public static void SendAsyncEmail(String server, int port, int timeout, Boolean SSL, String From, String To, String Subject, String Content, String smtpuser, String smtppass, string cc, string bcc)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                client.Port = port;
                client.Host = server;
                client.EnableSsl = SSL;
                client.Timeout = timeout;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(smtpuser, smtppass);
                MailMessage mm = new MailMessage(From, To, Subject, Content);

                if (cc!=null && cc != "")
                {
                    string[] tempcc = cc.Replace(",", ";").Split(';');
                    foreach (string s in tempcc)
                    {
                        if (s != "")
                            mm.CC.Add(new MailAddress(s));
                    }

                }
                if (bcc!=null && bcc != "")
                {
                    string[] tempcc = bcc.Replace(",", ";").Split(';');
                    foreach (string s in tempcc)
                    {
                        if (s != "")
                            mm.Bcc.Add(new MailAddress(s));
                    }

                }
                mm.IsBodyHtml = true;
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                client.Send(mm);
            }
            catch
            {
            }
        }

        public void DoSend(String server, int port, int timeout, Boolean SSL, String From, String To, String Subject, String Content, String smtpuser, String smtppass, string cc, string bcc)
        {
            try
            {
                SmtpClient client = new SmtpClient();
                client.Port = port;
                client.Host = server;
                client.EnableSsl = SSL;
                client.Timeout = timeout;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential(smtpuser, smtppass);
                MailMessage mm = new MailMessage(From, To, Subject, Content);

                if (cc!=null && cc != "")
                {
                    string[] tempcc = cc.Replace(",", ";").Split(';');
                    foreach (string s in tempcc)
                    {
                        if (s != "")
                            mm.CC.Add(new MailAddress(s));
                    }

                }
                if (bcc!=null && bcc != "")
                {
                    string[] tempcc = bcc.Replace(",", ";").Split(';');
                    foreach (string s in tempcc)
                    {
                        if (s != "")
                            mm.Bcc.Add(new MailAddress(s));
                    }

                }
                mm.IsBodyHtml = true;
                mm.BodyEncoding = UTF8Encoding.UTF8;
                mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                client.Send(mm);
            }
            catch
            {
            }
        }

        public string ReplaceText(string content,string username,string interviewdate,string project, string location,string receiptjobDate,string vacancy,string city,string time
            ,string recruiter,string interviewer,string phoneinterviewer,string hotlineproject,string emailproject)
        {
            content = content.Replace("[Username]", username);
            content = content.Replace("[InterviewDate]", interviewdate);
            content = content.Replace("[Project]", project);
            content = content.Replace("[Location]", location);
            content = content.Replace("[ReceiptJobDate]", receiptjobDate);
            content = content.Replace("[Vacancy]", vacancy);
            content = content.Replace("[City]", city);
            content = content.Replace("[Time]", time);
            content = content.Replace("[Recruiter]", recruiter);
            content = content.Replace("[Interviewer]", interviewer);
            content = content.Replace("[PhoneInterviewer]", phoneinterviewer);
            content = content.Replace("[HotlineProject]", hotlineproject);
            content = content.Replace("[EmailProject]", emailproject);
            return content;
        }
        public string ReplaceSubject(string subject,string customer, string city,string round)
        {
            subject = subject.Replace("[Customer]", customer);
            subject = subject.Replace("[City]", city);
            subject = subject.Replace("[Round]", round);
            return subject;
        }
    }
}