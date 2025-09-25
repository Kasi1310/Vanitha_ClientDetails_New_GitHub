using ClientDetails.App_Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClientDetails
{
    public partial class frmMedicountClientSurvey : System.Web.UI.Page
    {
        clsMedicountClientSurvey objclsMedicountClientSurvey;
        DataTable dt;
        bool IsSurveyFilled;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IsSurveyFilled = false;

                if (Request.QueryString["ID"] != null)
                {
                    hdnClientID.Value = Request.QueryString["ID"].ToString();

                    objclsMedicountClientSurvey = new clsMedicountClientSurvey();
                    dt = new DataTable();
                    objclsMedicountClientSurvey.ClientID = int.Parse(hdnClientID.Value);

                    dt = objclsMedicountClientSurvey.CheckSurveyFilled();

                    if (dt == null || dt.Rows.Count == 0)
                    {
                        lblMessage.Style.Add("color", "red");
                        lblMessage.Text = "Invalid Link";
                        divControls.Visible = false;
                        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "alert('Invalid Link');window.close();", true);
                    }
                    else if (dt.Rows.Count > 0 && Convert.ToBoolean(dt.Rows[0]["IsSurveyFilled"].ToString()))
                    {
                        IsSurveyFilled = true;

                        lblMessage.Style.Add("color", "red");
                        lblMessage.Text = "Survey already filled";
                        divControls.Visible = false;
                        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "alert('Survey already filled');window.close();", true);
                    }

                }
                else
                {
                    lblMessage.Style.Add("color", "red");
                    lblMessage.Text = "Invalid Link";
                    divControls.Visible = false;
                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "alert('Invalid Link');window.close();", true);
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            clsMedicountClientSurvey objclsMedicountClientSurvey = new clsMedicountClientSurvey();
            objclsMedicountClientSurvey.ClientID = int.Parse(hdnClientID.Value.Trim());
            objclsMedicountClientSurvey.Question1 = rdolstQuestion1.SelectedValue;
            objclsMedicountClientSurvey.Question2 = rdolstQuestion2.SelectedValue;
            objclsMedicountClientSurvey.Question3 = rdolstQuestion3.SelectedValue;
            objclsMedicountClientSurvey.Question4 = rdolstQuestion4.SelectedValue;
            objclsMedicountClientSurvey.Question5 = rdolstQuestion5.SelectedValue;
            objclsMedicountClientSurvey.Question6 = rdolstQuestion6.SelectedValue;
            objclsMedicountClientSurvey.Question7 = rdolstQuestion7.SelectedValue;
            objclsMedicountClientSurvey.Question8a = rdolstQuestion8a.SelectedValue;
            objclsMedicountClientSurvey.Question8b = rdolstQuestion8b.SelectedValue;
            objclsMedicountClientSurvey.Question8c = rdolstQuestion8c.SelectedValue;
            objclsMedicountClientSurvey.Question8d = rdolstQuestion8d.SelectedValue;
            objclsMedicountClientSurvey.Question9 = hdnQuestion9.Value;
            objclsMedicountClientSurvey.Question10Name = txtName.Text.Trim();
            objclsMedicountClientSurvey.Question10AgencyName = txtAgencyName.Text.Trim();
            objclsMedicountClientSurvey.Question10Email = txtEmail.Text.Trim();
            objclsMedicountClientSurvey.Question10Phone = txtPhone.Text.Trim();

            objclsMedicountClientSurvey.InsertMedicountClientSurvey();

            divControls.Visible = false;
            lblMessage.Text = "Thank you for your feedback!";
        }
    }
}