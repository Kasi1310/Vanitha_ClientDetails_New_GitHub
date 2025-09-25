using ClientDetails.App_Code;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClientDetails
{
    public partial class frmChargeDetailsApproval : System.Web.UI.Page
    {
        clsCommon objclsCommon;
        clsChargeDetails objclsChargeDetails;
        clsSendMail objclsSendMail;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Code"] == null && Request.QueryString["Status"] == null)
            {

            }
            else
            {
                DataTable dt = new DataTable();
                objclsCommon = new clsCommon();
                objclsChargeDetails = new clsChargeDetails();

                dt = objclsChargeDetails.UpdateChargeDetails(Request.QueryString["Status"].ToString().Trim(), int.Parse(objclsCommon.Decrypt(Request.QueryString["Code"].ToString().Trim())));


                if (dt != null && dt.Rows.Count == 1)
                {
                    string To = "";
                    objclsSendMail = new clsSendMail();

                    if (Request.QueryString["Status"].ToString().Trim().ToUpper() == "APPROVED")
                    {
                        To = ConfigurationManager.AppSettings["ChargeDetails.email"].ToString();
                    }
                    else
                    {
                        To = dt.Rows[0]["AEEmail"].ToString().Trim();
                    }

                    objclsSendMail.SendMail(To, "", "", "MEDICOUNT: Charge Details Client Status", MailBody(dt.Rows[0]["CompanyName"].ToString().Trim(), Request.QueryString["Status"].ToString().Trim()));
                }
                ////ClientScript.RegisterClientScriptBlock(GetType(), "myscript", "alert('1');window.close();", true);


                this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "window.close()", true);

            }
        }

        private string MailBody(string CompanyName, string Status)
        {

            StringBuilder sb = new StringBuilder();

            //string url = HttpContext.Current.Request.Url.AbsoluteUri;
            //string imagePath = "";
            //int lastIndex = url.LastIndexOf("/");
            ////imagePath = url.Substring(0, lastIndex) + "/Images/Logo.jpg";
            //imagePath = url.Substring(0, lastIndex) + "/Images/";
            //url = url.Substring(0, lastIndex) + "/frmClientDetails.aspx?ClientID=" + objclsCommon.Encrypt(ClientID);

            sb.AppendLine("<html><head></head><body>");
            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;margin:0;line-height:18.0pt;'>");
            sb.AppendLine("<b><span style = 'font-size:14pt;'> &nbsp;</span></b></p>");
            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;margin:0;line-height:18.0pt;'>");

            if (Status.ToUpper() == "APPROVED")
            {
                sb.AppendLine("<span style = 'font-size:14pt;'> Hi, <br>" + CompanyName + " approved the charge rates details.<br></span>");
            }
            else
            {
                sb.AppendLine("<span style = 'font-size:14pt;'> Hi, <br>" + CompanyName + " rejected the charge rate details. Please contact the client for rejection reasons.<br></span>");
            }
            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;margin:0;line-height:18.0pt;'>");
            sb.AppendLine("<b><span style = 'font-size:14pt;'> &nbsp;</span></b></p>");
            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;margin:0;line-height:18.0pt;'>");
            sb.AppendLine("<b><span style = 'font-size:14pt;'> Thank you. <br>");
            sb.AppendLine("</span></b></p>");

            sb.AppendLine("</body></html>");



            sb.AppendLine("<br>");
            return sb.ToString();
        }
    }
}