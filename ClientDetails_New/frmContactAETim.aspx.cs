using ClientDetails.App_Code;
using System;
using System.Configuration;

namespace ClientDetails
{
    public partial class frmContactAETim : System.Web.UI.Page
    {
        clsContactAE objclsContactAE;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            objclsContactAE = new clsContactAE();


            string To = ConfigurationManager.AppSettings["ContactAETim.To.email"].ToString();

            objclsContactAE.InsertSendContactAE(txtName.Text.Trim(), txtClientName.Text.Trim(), txtClientName.Text.Trim()
                , txtPhoneNumber.Text.Trim(), To, txtComments.Text.Trim());

            Response.Redirect(Request.Url.AbsoluteUri);
        }
    }
}