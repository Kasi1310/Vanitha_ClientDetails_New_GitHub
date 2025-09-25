using ClientDetails.App_Code;
using DocumentFormat.OpenXml.ExtendedProperties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.WebRequestMethods;

namespace ClientDetails
{
    public partial class frmLowRunSummary : System.Web.UI.Page
    {
        clsLowRunSummary objclsLowRunSummary;
        string zohoApiUrl = ConfigurationManager.AppSettings["ZohoApiUrl"].ToString();
        string zohoNotesUrl = ConfigurationManager.AppSettings["ZohoNotesUrl"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            clsCommon objclsCommon = new clsCommon();
            if (!IsPostBack)
            {
                try
                {
                    if (Request.QueryString["ID"] != null)
                    {
                        hdnID.Value = objclsCommon.Decrypt(Request.QueryString["ID"].ToString());
                        LoadValues();
                    }
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterStartupScript(GetType(), "myscript", "alert('Invalid Link');", true);
                }
            }

            lblComment.Visible = false;
            lblReasonForLowRuns.Visible = false;

            if (Request.QueryString["T"]!=null)
            {
                hdnType.Value = objclsCommon.Decrypt(Request.QueryString["T"].ToString());
                if (hdnType.Value == "CAE")
                {                    
                    lblReasonForLowRuns.Visible = true;
                }
                else if (hdnType.Value == "Reply")
                {
                    lblComment.Visible = true;   
                }
            }
        }

        private void LoadValues()
        {
            objclsLowRunSummary = new clsLowRunSummary();
            DataTable dt = new DataTable();
            objclsLowRunSummary.ID = int.Parse(hdnID.Value.Trim());
            dt = objclsLowRunSummary.SelectLowSummaryReport();

            if (dt != null && dt.Rows.Count > 0)
            {
                lblClientName.Text = dt.Rows[0]["CompanyName"].ToString().Trim();
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            objclsLowRunSummary = new clsLowRunSummary();
            objclsLowRunSummary.ID = int.Parse(hdnID.Value.Trim());
            objclsLowRunSummary.Mode = hdnType.Value;
            objclsLowRunSummary.Comment = txtComment.Text.Trim();
            string CompanyId = "";

            DataTable dt = new DataTable();
            
            dt = objclsLowRunSummary.SelectLowSummaryReport();

            if (dt != null && dt.Rows.Count > 0)
            {
                string CompanyData = dt.Rows[0]["CompanyName"].ToString().Trim();
                Match match = Regex.Match(CompanyData, @"(\d+)$");

                if (match.Success)
                {
                    CompanyId = int.Parse(match.Value).ToString();
                }
                else
                {
                    CompanyId = 0.ToString();
                }
            }
            else
            {
                CompanyId = 0.ToString();
            }

            objclsLowRunSummary.UpdateLowRunSummaryReport();

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
            sb.Append("Please find the AE replay for the Client " + lblClientName.Text.Trim());
            sb.Append("<br /><br />");
            sb.Append(txtComment.Text.Trim() + "<br /><br />");
            sb.Append("Thanks,<br /> ");
            sb.Append("Medicount");
            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("</table>");
            sb.Append("<br />");
            sb.Append("<br />");
            sb.Append("</body>");
            sb.Append("</html>");

            clsLowRunSummaryToZohoCRM objSendDataToZoho = new clsLowRunSummaryToZohoCRM();
            string accessToken = objSendDataToZoho.GetAccessTokenFromRefreshToken();
            string searchUrl = $"{zohoApiUrl}/Accounts/search?criteria=(Account_Number:equals:{Uri.EscapeDataString(CompanyId)})";
            var searchResponse = objSendDataToZoho.MakeZohoApiRequest("GET", searchUrl, accessToken);
            if (searchResponse != null) // Old = Zoho Data & New = CCMS Changed Data
            {
                JObject responseJson = JObject.Parse(searchResponse);
                JArray records = (JArray)responseJson["data"];
                if (records != null && records.Count > 0)
                {
                    string recordId = records[0]["id"].ToString();

                    var notePayload = new
                    {
                        data = new[]
                        {
                            new
                            {
                                Note_Title = "",
                                Note_Content = objclsLowRunSummary.Comment,
                                Parent_Id = recordId,
                                se_module = "Accounts"
                            }
                        }
                    };


                    string noteJson = JsonConvert.SerializeObject(notePayload);
                    objSendDataToZoho.MakeZohoApiRequest("POST", zohoNotesUrl, accessToken, noteJson);
                }
            }

               

            clsSendMail objclsSendMail = new clsSendMail();
            objclsSendMail.SendMailITNotification(ConfigurationManager.AppSettings["LRS.To.email"].ToString().Trim(), "", "", lblClientName.Text.Trim() + " LOW RUN SUMMARY", sb.ToString());

        }
    }
}