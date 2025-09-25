using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace ClientDetails.App_Code
{
    public class clsSendMail
    {

        //Live Send mail
        public bool SendMail(string To, string CC, string BCC, string Subject, string Body, string[] Attachement = null)
        {
            try
            {
                //To = "arengasamy@medicount.com";
                //To= "vanithac@medicount.com";
                //CC = "";
                //BCC = "";

                if (Attachement==null)
                {
                    Attachement = new string[] { };
                }
                
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress("lscott@medicount.com");// ("itnotifications@medicount.com");
                                                                               //mailMessage.From = new MailAddress("itnotifications@medicount.com");

                    mailMessage.Subject = Subject;
                    mailMessage.Body = Body;
                    mailMessage.IsBodyHtml = true;

                    foreach (var ToMailID in To.Split(','))
                    {
                        mailMessage.To.Add(new MailAddress(ToMailID));
                    }

                    if (CC != "")
                    {
                        foreach (var CCMailID in CC.Split(','))
                        {
                            mailMessage.CC.Add(new MailAddress(CCMailID));
                        }
                    }
                    if (BCC != "")
                    {
                        foreach (var BCCMailID in BCC.Split(','))
                        {
                            mailMessage.Bcc.Add(new MailAddress(BCCMailID));
                        }
                    }
                    mailMessage.Bcc.Add(new MailAddress("lscott@medicount.com"));

                    foreach (var att in Attachement)
                    {
                        mailMessage.Attachments.Add(new Attachment(att));
                    }

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "medicount-com.mail.protection.outlook.com";//"smtp.office365.com";
                                                                            //smtp.Host = "smtp.office365.com";
                    System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                    NetworkCred.UserName = "lscott@medicount.com";// "itnotifications @medicount.com";
                    NetworkCred.Password = "@Kilan1988";// "d0nt @skm3";
                                                        //NetworkCred.UserName = "itnotifications@medicount.com";
                                                        //NetworkCred.Password = "d0nt@skm3";
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = NetworkCred;
                    smtp.EnableSsl = true;
                    smtp.Port = 25;//587;
                    smtp.Send(mailMessage);

                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool SendMailITNotification(string To, string CC, string BCC, string Subject, string Body, string[] Attachement = null)
        {
            try
            {
                //To = "jsmith@medicount.com";
                //To = "arengasamy@medicount.com";
                //To = "vanithac@medicount.com";
                //CC = "vanithac@medicount.com";
                CC = "";
                BCC = "";


                if (Attachement == null)
                {
                    Attachement = new string[] { };
                }

                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = new MailAddress("itnotifications@medicount.com");

                    mailMessage.Subject = Subject;
                    mailMessage.Body = Body;
                    mailMessage.IsBodyHtml = true;

                    foreach (var ToMailID in To.Split(','))
                    {
                        mailMessage.To.Add(new MailAddress(ToMailID));
                    }

                    if (CC != "")
                    {
                        foreach (var CCMailID in CC.Split(','))
                        {
                            mailMessage.CC.Add(new MailAddress(CCMailID));
                        }
                    }
                    if (BCC != "")
                    {
                        foreach (var BCCMailID in BCC.Split(','))
                        {
                            mailMessage.Bcc.Add(new MailAddress(BCCMailID));
                        }
                    }

                    foreach (var att in Attachement)
                    {
                        mailMessage.Attachments.Add(new Attachment(att));
                    }

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.office365.com";
                    System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
                    NetworkCred.UserName = "itnotifications@medicount.com";
                    NetworkCred.Password = "**92|charge|TRIES|rolled|65**";
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = NetworkCred;
                    smtp.EnableSsl = true;
                    smtp.Port = 25;
                    smtp.Send(mailMessage);
                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //public void SendMail(string To, string CC, string Subject, string Body)
        //{
        //    //To = "arengasamy@medicount.com";
        //    To = "vanithac@medicount.com";

        //    using (MailMessage mailMessage = new MailMessage())
        //    {
        //        mailMessage.From = new MailAddress("ivr@medicount.com");

        //        mailMessage.Subject = Subject;
        //        mailMessage.Body = Body;
        //        mailMessage.IsBodyHtml = true;
        //        mailMessage.To.Add(new MailAddress(To));

        //        SmtpClient smtp = new SmtpClient();
        //        smtp.Host = "smtp.office365.com";
        //        System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
        //        NetworkCred.UserName = "ivr@medicount.com";
        //        NetworkCred.Password = "-Ab-34P31FGZfXxbOBfgwlcrDOah-VIo";
        //        smtp.UseDefaultCredentials = false;
        //        smtp.Credentials = NetworkCred;
        //        smtp.EnableSsl = true;
        //        smtp.Port = 587;
        //        smtp.Send(mailMessage);
        //    }
        //}
    }
}