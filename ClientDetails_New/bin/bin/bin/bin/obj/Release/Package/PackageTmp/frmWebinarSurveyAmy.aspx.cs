using System;
using ClientDetails.App_Code;

namespace ClientDetails
{
    public partial class frmWebinarSurveyAmy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            clsWebinarSurveyAmy objclsWebinarSurveyAmy = new clsWebinarSurveyAmy();

            objclsWebinarSurveyAmy.Name = txtName.Text.Trim();
            objclsWebinarSurveyAmy.Email = txtEmail.Text.Trim();
            objclsWebinarSurveyAmy.Question1 = rdolstQuestion1.SelectedValue;
            objclsWebinarSurveyAmy.Question2 = rdolstQuestion2.SelectedValue;
            objclsWebinarSurveyAmy.Question3 = rdolstQuestion3.SelectedValue;
            objclsWebinarSurveyAmy.Question4 = txtQuestion4.Text.Trim();
            objclsWebinarSurveyAmy.Question5 = txtQuestion5.Text.Trim();

            objclsWebinarSurveyAmy.InsertWebinarSurveyAmy();

            divControls.Visible = false;
            lblMessage.Text = "Thank you for your feedback!";
        }
    }
}