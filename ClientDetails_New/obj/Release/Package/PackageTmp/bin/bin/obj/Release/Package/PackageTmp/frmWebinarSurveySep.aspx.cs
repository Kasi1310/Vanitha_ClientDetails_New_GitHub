using System;
using ClientDetails.App_Code;

namespace ClientDetails
{
    public partial class frmWebinarSurveySep : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            clsWebinarSurveySep objclsWebinarSurveySep = new clsWebinarSurveySep();

            objclsWebinarSurveySep.Name = txtName.Text.Trim();
            objclsWebinarSurveySep.Email = txtEmail.Text.Trim();
            objclsWebinarSurveySep.CompanyName = txtCompanyName.Text.Trim();
            objclsWebinarSurveySep.Question1 = rdolstQuestion1.SelectedValue;
            objclsWebinarSurveySep.Question2 = rdolstQuestion2.SelectedValue;
            objclsWebinarSurveySep.Question3 = rdolstQuestion3.SelectedValue;
            objclsWebinarSurveySep.Question4 = txtQuestion4.Text.Trim();

            objclsWebinarSurveySep.InsertWebinarSurveySep();

            divControls.Visible = false;
            lblMessage.Text = "Thank you for your feedback!";
        }
    }
}