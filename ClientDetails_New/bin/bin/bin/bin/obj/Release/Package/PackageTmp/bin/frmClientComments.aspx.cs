using ClientDetails.App_Code;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClientDetails
{
    public partial class frmClientComments : System.Web.UI.Page
    {
        clsClientComments objclsClientComments;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            objclsClientComments = new clsClientComments();
            objclsClientComments.Name = txtName.Text.Trim();
            objclsClientComments.MailID = txtMailID.Text.Trim();
            objclsClientComments.Comments = txtComments.Text.Trim();
            objclsClientComments.InsertClientComments();

            string To = ConfigurationManager.AppSettings["Signature.To.email"].ToString();
            string CC = ConfigurationManager.AppSettings["Signature.CC.email"].ToString();
            string BCC = ConfigurationManager.AppSettings["Signature.BCC.email"].ToString();

            clsSendMail objclsSendMail = new clsSendMail();
            objclsSendMail.SendMail(To, CC, BCC, "Signature Compliance Questions", MailBody(txtName.Text.Trim(), txtMailID.Text.Trim(), txtComments.Text.Trim()));

            Response.Redirect(Request.Url.AbsoluteUri);
        }

        private string MailBody(string Name, string MailID, string Comments)
        {
            clsCommon objclsCommon = new clsCommon();
            StringBuilder sb = new StringBuilder();

            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            string imagePath = "";
            int lastIndex = url.LastIndexOf("/");
            //imagePath = url.Substring(0, lastIndex) + "/Images/Logo.jpg";
            imagePath = url.Substring(0, lastIndex) + "/Images/";
            //url = url.Substring(0, lastIndex) + "/frmChargeDetailsApproval.aspx?Code=" + objclsCommon.Encrypt(ID) + "&Status=";

            sb.AppendLine("<html><head></head><body>");
            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;margin:0;line-height:18.0pt;'>");
            sb.AppendLine("<b><span style = 'font-size:14pt;'> &nbsp;</span></b></p>");
            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;margin:0;line-height:18.0pt;'>");



            sb.AppendLine("<table border = '1' cellspacing = '0' cellpadding = '0' style = 'border-collapse: collapse; margin-left: 134.7pt; border-style: none; transform: scale(0.788091, 0.788091); transform-origin: left top;' min-scale = '0.7880910683012259'>");
            sb.AppendLine("<tbody><tr style = 'height:100%;'>");
            sb.AppendLine("<td valign = 'top' style = 'width:543.3pt;height:100%;padding:0 5.4pt;border:3pt solid #009094;padding-bottom: 10pt; padding-top: 10pt;'>");
            sb.AppendLine("<p align = 'center' style = 'font-size:11pt;font-family:Calibri,sans-serif;text-align:center;margin:0;'>");
            sb.AppendLine("<img data-imagetype = 'AttachmentByCid' originalsrc = 'cid:image002.png@611A0A90.001E98B3' data-custom = 'AAMkAGNmNzBjNDUyLTMxOTktNDlkNi05MWI3LTNkMmM2YjQ4OWQ1YgBGAAAAAAA%2B8aw%2BdMZ%2BSpckUHEBZ2e6BwCXJ8bZhveiSKRhV94WGI35AAAAAAEMAACXJ8bZhveiSKRhV94WGI35AACzf2H3AAABEgAQANNFAfYNzjhIpiuNpExuOu4%3D' naturalheight = '0' naturalwidth = '0' src ='" + imagePath + "Logo.png' width = '549' height = '140'");
            sb.AppendLine("id='x_Picture 3' crossorigin='use-credentials' style='cursor: pointer; '></p>");
            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;margin:0;'> &nbsp;</p>");
            sb.AppendLine("<p align='center' style='margin: 0in; text - align:center; background:#009094'><b><span style='font-size:30.0pt;color:white'>Signature Compliance Questions</span></b><o:p></o:p></p>");

            sb.AppendLine("<table align = 'left' border = '1' cellspacing = '0' cellpadding = '0' style = 'width:98.72%;'>");
            sb.AppendLine("<tbody>");

            sb.AppendLine("<tr>");
            sb.AppendLine("<td>");
            sb.AppendLine("Name:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td>");
            sb.AppendLine(Name);
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");

            sb.AppendLine("<tr>");
            sb.AppendLine("<td>");
            sb.AppendLine("Email Address:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td>");
            sb.AppendLine(MailID);
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");


            sb.AppendLine("<tr>");
            sb.AppendLine("<td>");
            sb.AppendLine("Submit your questions:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td>");
            sb.AppendLine(Comments);
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");

            sb.AppendLine("</tbody></table>");
            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;margin:0;'></p>");
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</tbody></table>");


            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;margin:0;line-height:18.0pt;'>");
            sb.AppendLine("<b><span style = 'font-size:14pt;'> &nbsp;</span></b></p>");
            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;margin:0;line-height:18.0pt;'>");
            sb.AppendLine("<b><span style = 'font-size:14pt;'> Thank you. <br>");
            sb.AppendLine("</span></b><b><span style = 'color:#009094;font-size:16pt;'> Medicount Management, Inc.</span></b></p>");

            sb.AppendLine("</body></html>");

            sb.AppendLine("<br>");
            return sb.ToString();
        }
    }
}