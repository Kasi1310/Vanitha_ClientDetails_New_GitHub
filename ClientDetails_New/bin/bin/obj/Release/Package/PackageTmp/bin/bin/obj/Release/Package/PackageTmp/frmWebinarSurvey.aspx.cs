using System;
using ClientDetails.App_Code;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClientDetails
{
    public partial class frmWebinarSurvey : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            clsWebinarSurvey objclsWebinarSurvey = new clsWebinarSurvey();

            objclsWebinarSurvey.Name = txtName.Text.Trim();
            objclsWebinarSurvey.Email = txtEmail.Text.Trim();
            objclsWebinarSurvey.CompanyName = txtCompanyName.Text.Trim();
            objclsWebinarSurvey.Question1 = rdolstQuestion1.SelectedValue;
            objclsWebinarSurvey.Question2 = rdolstQuestion2.SelectedValue;
            objclsWebinarSurvey.Question3 = rdolstQuestion3.SelectedValue;
            objclsWebinarSurvey.Question4 = txtQuestion4.Text.Trim();

            objclsWebinarSurvey.InsertWebinarSurvey();

            divControls.Visible = false;
            lblMessage.Text = "Thank you for your feedback!";
        }
    }
}