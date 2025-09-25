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
    public partial class frmContractRequest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            clsContractRequest objclsContractRequest = new clsContractRequest();
            //objclsContractRequest.ID = 0;
            objclsContractRequest.ClientName = txtClient.Text.Trim();
            objclsContractRequest.AEName = txtAEName.Text.Trim();
            objclsContractRequest.ChiefName = txtChiefName.Text.Trim();
            objclsContractRequest.NameOfEntity = txtNameofClient.Text.Trim();
            objclsContractRequest.ClientServicesAgreement = rdolstClientServicesAgreement.SelectedValue.Trim();
            objclsContractRequest.FeeRecommended = txtFeeRecommended.Text.Trim();
            objclsContractRequest.LengthOfClientServicesAgreement = txtLengthOfClientServicesAgreement.Text.Trim();
            objclsContractRequest.AdditionalEquipment = txtAdditionalEquipment.Text.Trim();
            objclsContractRequest.ALSProtocalRequiredCSAProtocol = txtALSProtocalRequiredCSAProtocol.Text.Trim();
            objclsContractRequest.AddressOfEntity = txtAddressOfEntity.Text.Trim();
            objclsContractRequest.CountyName = txtCountyName.Text.Trim();
            objclsContractRequest.State = txtState.Text.Trim();
            objclsContractRequest.EmailAddress = txtEmailAddress.Text.Trim();
            objclsContractRequest.ChangeHealthcareClient = txtChangeHealthCareClient.Text.Trim();
            objclsContractRequest.OtherComments = txtOtherComments.Text.Trim();
            objclsContractRequest.LastUpdatedBy = "";

            objclsContractRequest.InsertClientContractRequest();

            divContent.Style.Add("display", "none");
            lblMessage.Style.Add("display", "block");

            clsSendMail objclsSendMail = new clsSendMail();

            string To = ConfigurationManager.AppSettings["ClientContractRequest.To.email"].ToString().Trim();
            string CC = ConfigurationManager.AppSettings["ClientContractRequest.CC.email"].ToString().Trim();
            string BCC = ConfigurationManager.AppSettings["ClientContractRequest.BCC.email"].ToString().Trim();

            if (objclsSendMail.SendMail(To, CC, BCC, "CONTRACT REQUEST FORM - " + txtClient.Text.Trim(), MailBody()))
            {
                divContent.Style.Add("display", "none");
                lblMessage.Style.Add("display", "block");
            }
            else
            {
                divContent.Style.Add("display", "none");
                lblError.Style.Add("display", "block");
            }

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.AbsoluteUri);
        }

        private string MailBody()
        {
            StringBuilder sb = new StringBuilder();

            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            string imagePath = "";
            int lastIndex = url.LastIndexOf("/");
            //imagePath = url.Substring(0, lastIndex) + "/Images/Logo.jpg";
            imagePath = url.Substring(0, lastIndex) + "/Images/";

            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("<style type='text/css'>");
            sb.AppendLine(".lblText {font-family: Calibri;font-weight: bold;font-size: 20px;height: 30px;padding-left: 10px;}");
            sb.AppendLine(".lblValue {font-family: Calibri;font-size: 18px;height: 30px;padding-left: 10px;}");
            sb.AppendLine("</style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body lang=EN-US style='word-wrap:break-word;'>");
            sb.AppendLine("<div>");
            sb.AppendLine("<center>");
            sb.AppendLine("<table border=1 cellspacing=0 cellpadding=0 style='width:1000px;'>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td style='width:600px;'>");
            sb.AppendLine("<img src='" + imagePath + "Logo.jpg' />");
            sb.AppendLine("</td>");
            sb.AppendLine("<td style='width: 400px; background-color: #4472c4; padding: 0 20px 0 20px;' align='center'>");
            sb.AppendLine("<span style='font-family: Calibri; font-weight: bold; font-size: 34px; color: white;'>CONTRACT REQUEST FORM</span>");
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td style='height: 100px; background-color: #4472c4;' align='center'>");
            sb.AppendLine("<span style='font-family: Calibri; font-weight: bold; font-size: 34px; color: white;'>NAME OF CLIENT:</span>");
            sb.AppendLine("</td>");
            sb.AppendLine("<td style='font-family: Calibri; font-weight: bold; font-size: 34px;' align='center'>");
            sb.AppendLine(txtClient.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td colspan='2' align='center' style='background-color: #ffc000;'>");
            sb.AppendLine("<span style='font-family: Calibri; font-weight: bold; font-size: 22px; height: 30px;'>");
            sb.AppendLine("Please provide the following information for the Contracts");
            sb.AppendLine("</span>");
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("Account Executive name(s):");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(txtAEName.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("Name of the chief:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(txtChiefName.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("Name of the Client - Confirm:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(txtNameofClient.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("Client Services Agreement:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(rdolstClientServicesAgreement.SelectedValue.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("Fee % - Recommended:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(txtFeeRecommended.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("Length of Client Services Agreement:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(txtLengthOfClientServicesAgreement.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("Additional equipment or software applications that need to be included in the fee and or Client Services Agreement:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(txtAdditionalEquipment.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("ALS Protocol required in CSA:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(txtALSProtocalRequiredCSAProtocol.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("Address of entity for cover letter for return of the CSA:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(txtAddressOfEntity.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("County Name:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(txtCountyName.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("State:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(txtState.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("Email address to send Docusign CSA:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(txtEmailAddress.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("Change Healthcare Client:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(txtChangeHealthCareClient.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("Other Comments:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(txtOtherComments.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</table>");
            sb.AppendLine("</center>");
            sb.AppendLine("</div>");

            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;margin:0;line-height:18.0pt;'>");
            sb.AppendLine("<b><span style = 'font-size:14pt;'> &nbsp;</span></b></p>");
            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;margin:0;line-height:18.0pt;'>");
            sb.AppendLine("<b><span style = 'font-size:14pt;'> Thank you. <br>");
            sb.AppendLine("</span></b><b><span style = 'color:#009094;font-size:16pt;'> Medicount Management, Inc.</span></b></p>");

            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            sb.AppendLine("<br />");

            return sb.ToString();
        }
    }
}