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
    public partial class frmAmbulanceSupplementalPayment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            int intID = 0;
            clsAmbulanceSupplementalPayment objclsAmbulanceSupplementalPayment = new clsAmbulanceSupplementalPayment();
            objclsAmbulanceSupplementalPayment.ClientName = txtClientName.Text.Trim();
            objclsAmbulanceSupplementalPayment.Name = txtName.Text.Trim();
            objclsAmbulanceSupplementalPayment.EmailAddress = txtEmailAddress.Text.Trim();
            objclsAmbulanceSupplementalPayment.PhoneNumber = txtPhoneNumber.Text.Trim();
            objclsAmbulanceSupplementalPayment.IsGeneralInformation = chkGeneralInformation.Checked;
            objclsAmbulanceSupplementalPayment.IsCallBack = chkCallBack.Checked;
            objclsAmbulanceSupplementalPayment.SpecificQuestion = txtSpecificQuestion.Text.Trim();
            intID = objclsAmbulanceSupplementalPayment.InsertAmbulanceSupplementalPayment();
            if (intID != 0)
            {
                divcontainer.Style.Add("display", "none");
                divMessage.Style.Remove("display");
            }

            clsSendMail objclsSendMail = new clsSendMail();
            objclsSendMail.SendMail(ConfigurationManager.AppSettings["ASPP.To.email"].ToString()
                , ConfigurationManager.AppSettings["ASPP.CC.email"].ToString()
                , ConfigurationManager.AppSettings["ASPP.BCC.email"].ToString(), "OHIO ASPP EMAIL INQUIRY", MailBody());
        }


        private string MailBody()
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
            sb.AppendLine("<p align='center' style='margin: 0in; text - align:center; background:#009094'><b><span style='font-size:30.0pt;color:white'>OHIO ASPP EMAIL INQUIRY</span></b><o:p></o:p></p>");

            sb.AppendLine("<table align = 'left' border = '1' cellspacing = '0' cellpadding = '0' style = 'width:98.72%;'>");
            sb.AppendLine("<tbody>");

            sb.AppendLine("<tr>");
            sb.AppendLine("<td>");
            sb.AppendLine("Medicount Client Name:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td>");
            sb.AppendLine(txtClientName.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");

            sb.AppendLine("<tr>");
            sb.AppendLine("<td>");
            sb.AppendLine("Name:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td>");
            sb.AppendLine(txtName.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");

            sb.AppendLine("<tr>");
            sb.AppendLine("<td>");
            sb.AppendLine("Email Address:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td>");
            sb.AppendLine(txtEmailAddress.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");

            sb.AppendLine("<tr>");
            sb.AppendLine("<td>");
            sb.AppendLine("Phone Number:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td>");
            sb.AppendLine(txtPhoneNumber.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");


            sb.AppendLine("<tr>");
            sb.AppendLine("<td>");
            sb.AppendLine("For more information on the program:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td>");
            if (chkGeneralInformation.Checked && chkCallBack.Checked)
            {
                sb.AppendLine("General information & A Call Back");
            }
            else if (chkGeneralInformation.Checked)
            {
                sb.AppendLine("General information");
            }
            else if (chkCallBack.Checked)
            {
                sb.AppendLine("A Call Back");
            }
            else
            {
                sb.AppendLine("");
            }
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");


            sb.AppendLine("<tr>");
            sb.AppendLine("<td>");
            sb.AppendLine("A specific question:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td>");
            sb.AppendLine(txtSpecificQuestion.Text.Trim());
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

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.AbsoluteUri);
        }
    }
}