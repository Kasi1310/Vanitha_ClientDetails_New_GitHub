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
    public partial class frmACHDeductFees : System.Web.UI.Page
    {
        clsACHDeductFees objclsACHDeductFees;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (Request.QueryString["ID"] != null)
                    {
                        clsCommon objclsCommon = new clsCommon();
                        hdnID.Value = objclsCommon.Decrypt(Request.QueryString["ID"].ToString());
                        LoadTextboxValus();
                    }
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(GetType(), "myscript", "alert('Invalid Link');", true);
                }
            }
        }

        private void LoadTextboxValus()
        {
            objclsACHDeductFees = new clsACHDeductFees();
            DataTable dt = new DataTable();

            objclsACHDeductFees.ID = Convert.ToInt32(hdnID.Value.Trim());
            dt = objclsACHDeductFees.SelectACHDeductFees();

            if (dt != null && dt.Rows.Count != 0)
            {
                txtClientName.Text = dt.Rows[0]["ClientName"].ToString().Trim();
                rdolstFundsClientByACH.SelectedValue = dt.Rows[0]["FundsClientByACH"].ToString().Trim() == "YES" ? "true" : "false";
                rdolstEndofMonthInvoice.SelectedValue = dt.Rows[0]["EndofMonthInvoice"].ToString().Trim() == "YES" ? "true" : "false";
                rdolstEndofMonthFunds.SelectedValue = dt.Rows[0]["EndofMonthFunds"].ToString().Trim() == "YES" ? "true" : "false";
                txtNameOfBank.Text = SSTCryptographer.Decrypt(dt.Rows[0]["NameOfBank"].ToString().Trim(), "Medicount");
                txtRoutingNumber.Text = SSTCryptographer.Decrypt(dt.Rows[0]["RoutingNumber"].ToString().Trim(), "Medicount");
                txtAccountNumber.Text = SSTCryptographer.Decrypt(dt.Rows[0]["AccountNumber"].ToString().Trim(), "Medicount");
                txtName1.Text = dt.Rows[0]["Name1"].ToString().Trim();
                txtEmailAddress1.Text = dt.Rows[0]["EmailAddress1"].ToString().Trim();
                txtName2.Text = dt.Rows[0]["Name2"].ToString().Trim();
                txtEmailAddress2.Text = dt.Rows[0]["EmailAddress2"].ToString().Trim();
                txtName3.Text = dt.Rows[0]["Name3"].ToString().Trim();
                txtEmailAddress3.Text = dt.Rows[0]["EmailAddress3"].ToString().Trim();
                txtName4.Text = dt.Rows[0]["Name4"].ToString().Trim();
                txtEmailAddress4.Text = dt.Rows[0]["EmailAddress4"].ToString().Trim();
                txtName5.Text = dt.Rows[0]["Name5"].ToString().Trim();
                txtEmailAddress5.Text = dt.Rows[0]["EmailAddress5"].ToString().Trim();
                btnSubmit.Visible = false;
            }
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            objclsACHDeductFees = new clsACHDeductFees();

            objclsACHDeductFees.ClientName = txtClientName.Text.Trim();
            objclsACHDeductFees.IsFundsClientByACH = Convert.ToBoolean(rdolstFundsClientByACH.SelectedValue.Trim());
            objclsACHDeductFees.IsEndofMonthInvoice = Convert.ToBoolean(rdolstEndofMonthInvoice.SelectedValue.Trim());
            objclsACHDeductFees.IsEndofMonthFunds = Convert.ToBoolean(rdolstEndofMonthFunds.SelectedValue.Trim());

            objclsACHDeductFees.NameOfBank = SSTCryptographer.Encrypt(txtNameOfBank.Text.Trim(), "Medicount");
            objclsACHDeductFees.RoutingNumber = SSTCryptographer.Encrypt(txtRoutingNumber.Text.Trim(), "Medicount");
            objclsACHDeductFees.AccountNumber = SSTCryptographer.Encrypt(txtAccountNumber.Text.Trim(), "Medicount");

            objclsACHDeductFees.Name1 = txtName1.Text.Trim();
            objclsACHDeductFees.EmailAddress1 = txtEmailAddress1.Text.Trim();
            objclsACHDeductFees.Name2 = txtName2.Text.Trim();
            objclsACHDeductFees.EmailAddress2 = txtEmailAddress2.Text.Trim();
            objclsACHDeductFees.Name3 = txtName3.Text.Trim();
            objclsACHDeductFees.EmailAddress3 = txtEmailAddress3.Text.Trim();
            objclsACHDeductFees.Name4 = txtName4.Text.Trim();
            objclsACHDeductFees.EmailAddress4 = txtEmailAddress4.Text.Trim();
            objclsACHDeductFees.Name5 = txtName5.Text.Trim();
            objclsACHDeductFees.EmailAddress5 = txtEmailAddress5.Text.Trim();

            int ID = objclsACHDeductFees.InsertACHDeductFees();

            clsSendMail objclsSendMail = new clsSendMail();

            objclsSendMail.SendMail(ConfigurationManager.AppSettings["ACHDeductFees.email"].ToString(), "", "", "MEDICOUNT: Clients ACH's Invoices", MailBody(ID));


            ClientScript.RegisterStartupScript(GetType(), "myscript", "alert('Submitted Successfully');", true);

            Response.Redirect(Request.Url.AbsoluteUri);
        }

        private string MailBody(int ID)
        {
            clsCommon objclsCommon = new clsCommon();
            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            int lastIndex = url.LastIndexOf("/");
            url = url.Substring(0, lastIndex) + "/frmACHDeductFees.aspx?ID=" + objclsCommon.Encrypt(ID.ToString());

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
            sb.Append("Please find the Clients ACH's Invoices Link.");
            sb.Append("<br /><br />");
            sb.Append(url + "<br /><br />");
            sb.Append("Thanks,<br /> ");
            //sb.Append("CMS Admin");
            sb.Append("</td>");
            sb.Append("</tr>");
            //sb.Append("<tr>");
            //sb.Append("<img src='http://cms.medicount.com/img/MedicountHeaderLogo.png' width='150px' />");
            //sb.Append("</tr>");
            sb.Append("</table>");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("</body>");
            sb.Append("</html>");

            return sb.ToString();
        }
    }
}