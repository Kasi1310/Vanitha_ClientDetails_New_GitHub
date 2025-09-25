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
    public partial class frmNoticeAnnouncement : System.Web.UI.Page
    {
        clsNoticeAnnouncement objclsNoticeAnnouncement;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            objclsNoticeAnnouncement = new clsNoticeAnnouncement();
            objclsNoticeAnnouncement.Name = txtName.Text.Trim();
            objclsNoticeAnnouncement.EmailID = txtEmailID.Text.Trim();
            objclsNoticeAnnouncement.IsNotice = rdolstNotice.SelectedValue == "1" ? true : false;
            objclsNoticeAnnouncement.IsClientFireChief = rdolstClientFireChief.SelectedValue == "1" ? true : false;
            objclsNoticeAnnouncement.IsAllClientContacts = rdolstAllClientContacts.SelectedValue == "1" ? true : false;
            objclsNoticeAnnouncement.IsClientEntireFire = rdolstClientEntireFire.SelectedValue == "1" ? true : false;
            objclsNoticeAnnouncement.IsClientFiscalOfficer = rdolstClientFiscalOfficer.SelectedValue == "1" ? true : false;
            // objclsNoticeAnnouncement.IsAllClientContactsZOHO = rdolstAllClientContactsZOHO.SelectedValue == "1" ? true : false;
            objclsNoticeAnnouncement.IsSpecificClientContactsZOHO = rdolstSpecificClientContactsZOHO.SelectedValue == "1" ? true : false;
            //objclsNoticeAnnouncement.IsALLPotentialClients = rdolstALLPotentialClients.SelectedValue == "1" ? true : false;
            objclsNoticeAnnouncement.Comments = txtComments.Text.Trim();
            objclsNoticeAnnouncement.InsertNoticeAnnouncement();

            clsSendMail objclsSendMail = new clsSendMail();

            objclsSendMail.SendMail(ConfigurationManager.AppSettings["NoticeAnnouncement.email"].ToString(), "", "", "MEDICOUNT: Notice Announcement", MailBody());

            Response.Redirect(Request.Url.AbsoluteUri);
        }

        private string MailBody()
        {
            clsCommon objclsCommon = new clsCommon();

            StringBuilder sb = new StringBuilder();
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("<title></title>");
            sb.Append("<meta charset='utf-8' />");
            sb.Append("</head>");
            sb.Append("<body>");
            sb.Append("<br />");
            sb.Append("<table width='50%'>");
            sb.Append("<tr>");
            sb.Append("<td>");
            sb.Append("Hi, <br /><br />");
            sb.Append("Please find the Employee Notice/Announcement.");
            sb.Append("<br /><br />");
            sb.Append("Name:" + txtName.Text.Trim() + "<br /><br />");
            sb.Append("Email ID:" + txtEmailID.Text.Trim() + "<br /><br />");
            sb.Append("Do you want the below notice to go out? :" + (rdolstNotice.SelectedValue == "1" ? "Yes" : "No") + "<br /><br />");
            sb.Append("If no, to whom?    Client-Fire Chief, Only:" + (rdolstClientFireChief.SelectedValue == "1" ? "Yes" : "No") + "<br /><br />");
            sb.Append("To all Client contacts:" + (rdolstAllClientContacts.SelectedValue == "1" ? "Yes" : "No") + "<br /><br />");
            sb.Append("Client-Entire Fire Dept:" + (rdolstClientEntireFire.SelectedValue == "1" ? "Yes" : "No") + "<br /><br />");
            sb.Append("Client-Fiscal Officer:" + (rdolstClientFiscalOfficer.SelectedValue == "1" ? "Yes" : "No") + "<br /><br />");
            sb.Append("State Specific Client Contacts in ZOHO:" + (rdolstSpecificClientContactsZOHO.SelectedValue == "1" ? "Yes" : "No") + "<br /><br />");
            //sb.Append("ALL Potential Clients:" + (rdolstALLPotentialClients.SelectedValue == "1" ? "Yes" : "No") + "<br /><br />");
            sb.Append("Comments:" + txtComments.Text.Trim() + "<br /><br />");
            sb.Append("Thanks,<br /> ");
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("</table>");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.AbsoluteUri);
        }
    }
}