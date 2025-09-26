using ClientDetails.App_Code;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Office2013.Excel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.WebRequestMethods;

namespace ClientDetails
{
    public partial class frmMonthlyRunVolume : System.Web.UI.Page
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

            if (Request.QueryString["T"] != null)
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
            string AEMailId = "";
            string ToMailID = "";
            if (dt != null && dt.Rows.Count > 0)
            {
                string CompanyData = dt.Rows[0]["CompanyName"].ToString().Trim();
                AEMailId=dt.Rows[0]["AcctExecMailId"].ToString().Trim();
                

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
            if (hdnType.Value == "CAE")
            {
                ToMailID = AEMailId;
                sb.Append("Please find the " + lblClientName.Text.Trim() + " Question(s) for AE. ");
            }
            else if (hdnType.Value == "Reply")
            {
                ToMailID = ConfigurationManager.AppSettings["LRS.To.email"].ToString().Trim();
                sb.Append("Please find the " + lblClientName.Text.Trim() + " Comments/Issues.");
            }
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

            string text = hdnType.Value == "CAE" ? "question(s)" : "comment";
            string rawTitle = text.ToLower();
            string title = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(rawTitle);

            if (objclsLowRunSummary.Comment.Trim() != "")
            {
                clsLowRunSummaryToZohoCRM objSendDataToZoho = new clsLowRunSummaryToZohoCRM();
                string accessToken = objSendDataToZoho.GetAccessTokenFromRefreshToken();
                if (accessToken != "")
                {
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
                            var responseString = objSendDataToZoho.MakeZohoApiRequest("POST", zohoNotesUrl, accessToken, noteJson);

                            var json = JObject.Parse(responseString);
                            var status = json["data"]?[0]?["status"]?.ToString();

                            if (status == "success")
                            {
                                var noteId = json["data"]?[0]?["details"]?["id"]?.ToString();
                                string script = $@"
                                        <script>
                                            Swal.fire({{
                                                title: 'Thank you!',
                                                text: 'Your {title} has been added successfully.',
                                                icon: 'success',
                                                confirmButtonText: 'OK'
                                            }});
                                        </script>";

                                ClientScript.RegisterStartupScript(this.GetType(), "swal", script, false);

                                // Clear and disable controls (server-side)
                                txtComment.Enabled = false;
                                btnSubmit.Enabled = false;

                                Console.WriteLine($"Note added successfully. ID: {noteId}");
                            }
                            else
                            {
                                var message = json["data"]?[0]?["message"]?.ToString();
                                string script = $@"
                                            <script>
                                                Swal.fire({{
                                                    title: 'Failed to {title}',
                                                    text: '{message?.Replace("'", "\\'")}',
                                                    icon: 'error',
                                                    confirmButtonText: 'OK'
                                                }});
                                            </script>";

                                ClientScript.RegisterStartupScript(this.GetType(), "swalError", script, false);
                                Console.WriteLine($"Failed to add note. Message: {message}");
                            }
                        }
                    }
                }
            }
            else
            {
                string script = $@"
                                <script>
                                    Swal.fire({{
                                        title: '{title} is required',
                                        text: 'Please enter a {text} before submitting.',
                                        icon: 'warning',
                                        confirmButtonText: 'OK'
                                    }});
                                </script>";

                ClientScript.RegisterStartupScript(this.GetType(), "swal", script, false);
            }

            clsSendMail objclsSendMail = new clsSendMail();
            objclsSendMail.SendMailITNotification(ToMailID, "", ""
                , lblClientName.Text.Trim() + " MONTHLY RUN VOLUME", sb.ToString());

        }
    }
}