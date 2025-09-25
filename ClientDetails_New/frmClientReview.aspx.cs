using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Office2013.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using iText.Html2pdf;
using iText.Html2pdf.Attach.Impl.Tags;
using iText.Html2pdf.Resolver.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout.Font;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;



namespace ClientDetails
{

    public partial class frmClientReview : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["EsoToZohoConnectionString"].ToString();
        string zohoAuthUrl= ConfigurationManager.AppSettings["ZohoAuthenticationUrl"].ToString();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindClientNumbers();
                
            }

        }
       
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                // Create XML from form data
                string xmlData = GenerateXmlFromForm();

                // Save to database
                int reviewId = SaveToDatabase(xmlData);

                // Show success message
                ScriptManager.RegisterStartupScript(this, GetType(), "showSuccess",
                    "swal('Success', 'Client review saved successfully! ID: " + reviewId + "', 'success');", true);
            }
            catch (Exception ex)
            {
                // Show error message
                ScriptManager.RegisterStartupScript(this, GetType(), "showError",
                    "swal('Error', 'An error occurred while saving the review: " +
                    HttpUtility.JavaScriptStringEncode(ex.Message) + "', 'error');", true);
            }
        }

        private string GenerateXmlFromForm()
        {
            StringBuilder xmlBuilder = new StringBuilder();
            using (XmlWriter writer = XmlWriter.Create(xmlBuilder))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("ClientReview");

                // MEETING DETAILS
                writer.WriteElementString("ClientNumber", ddlClientNumber.Text);
                writer.WriteElementString("ClientName", txtClientName.Text);
                writer.WriteElementString("MeetingDate", txtMeetingDate.Text);
                writer.WriteElementString("AccountExecutive", txtAccountExecutive.Text);
                writer.WriteElementString("Email", txtEmail.Text);
                writer.WriteElementString("Phone", txtPhone.Text);
                writer.WriteElementString("RecipientReceivingName", txtRecipientReceivingName.Text);
                writer.WriteElementString("RecipientReceivingTitle", txtRecipientReceivingTitle.Text);
                writer.WriteElementString("RecipientReceivingEmail", txtRecipientReceivingEmail.Text);
                // writer.WriteElementString("DateOfService", txtDateofService.Text);

                // PERFORMANCE METRICS
                writer.WriteElementString("ClientRevenueNumberStartDate", txtClientRevenueNumberStartDate.Text);
                writer.WriteElementString("ClientRevenueNumberEndDate", txtClientRevenueNumberEndDate.Text);
                writer.WriteElementString("ReportType", txtReportType.Text);
                writer.WriteElementString("Transports", txtTransports.Text);                
                writer.WriteElementString("RevenuePerTransport", txtRevenuePerTransport.Text);                
                writer.WriteElementString("Charges", txtCharges.Text);
                writer.WriteElementString("Payments", txtPayments.Text);
                writer.WriteElementString("Adjustments", txtAdjustments.Text);                
                writer.WriteElementString("WriteOffs", txtWriteOffs.Text);
                writer.WriteElementString("CollectionRate", txtCollectionRate.Text);
                writer.WriteElementString("ClientComments", txtClientComments.Text);
                writer.WriteElementString("AEComments", txtAEComments.Text);

                // YOUR CHARGE RATES
                writer.WriteElementString("DateOfLastRateChange", txtDateOfLastRateChange.Text);
                writer.WriteElementString("Bls", txtBls.Text);
                writer.WriteElementString("BlsNe", txtBlsNe.Text);                
                writer.WriteElementString("Als", txtAls.Text);
                writer.WriteElementString("AlsNe", txtAlsNe.Text);
                writer.WriteElementString("Als2", txtAls2.Text);  
                writer.WriteElementString("Mileage", txtMileage.Text);
                writer.WriteElementString("RateChanges", rbRateChangesYes.Checked ? "Yes" : "No");
                writer.WriteElementString("NonTransport", rbNonTransportYes.Checked ? "Yes" : "No");
                writer.WriteElementString("BillingClientComments", txtBillingClientComments.Text);
                writer.WriteElementString("BillingAEComments", txtBillingAEComments.Text);

                // CONTRACT STATUS
                writer.WriteElementString("ContractRenewalDate", txtContractRenewalDate.Text);
                writer.WriteElementString("ContractCurrentFeeRate", txtContractCurrentFeeRate.Text);
                writer.WriteElementString("ContractStatusClientComments", txtContrackStatusClientComments.Text);
                writer.WriteElementString("ContractStatusAEComments", txtContrackStatusAEComments.Text);

                // PERSONNEL CHANGES
                    // CURRENT & NEW CHIEF Details
                writer.WriteElementString("CurrentChiefName", txtCurrentChiefName.Text);
                writer.WriteElementString("ChiefCorrect", rbChiefCorrectYes.Checked ? "Yes" : "No");
                writer.WriteElementString("NewChiefName", txtNewChiefName.Text);
                writer.WriteElementString("NewChiefEmail", txtNewChiefEmail.Text);
                writer.WriteElementString("NewChiefPhone", txtNewChiefPhone.Text);

                //// CURRENT & NEW FISCAL OFFICER Details
                writer.WriteElementString("CurrentFiscalOfficer", txtCurrentFiscalOfficer.Text);
                writer.WriteElementString("FiscalCorrect", rbFiscalCorrectYes.Checked ? "Yes" : "No");
                writer.WriteElementString("NewFiscalName", txtNewFiscalName.Text);
                writer.WriteElementString("NewFiscalEmail", txtNewFiscalEmail.Text);
                writer.WriteElementString("NewFiscalPhone", txtNewFiscalPhone.Text);

                //CURRENT & NEW AuthorizedOfficial Details
                writer.WriteElementString("CurrentAuthorizedOfficial_1", txtCurrentAuthorizedOfficial_1.Text);                
                writer.WriteElementString("rbOfficialCorrectYes", rbOfficialCorrectYes.Checked ? "Yes" : "No");
                writer.WriteElementString("NewAuthorizedName_1", txtNewAuthorizedName_1.Text);
                writer.WriteElementString("NewAuthorizedEmail_1", txtNewAuthorizedEmail_1.Text);
                writer.WriteElementString("NewAuthorizedPhone_1", txtNewAuthorizedPhone_1.Text);

                writer.WriteElementString("CurrentAuthorizedOfficial_2", txtCurrentAuthorizedOfficial_2.Text);
                writer.WriteElementString("rbOfficialCorrectYes_2", rbOfficialCorrectYes_2.Checked ? "Yes" : "No");
                writer.WriteElementString("NewAuthorizedName_2", txtNewAuthorizedName_2.Text);
                writer.WriteElementString("NewAuthorizedEmail_2", txtNewAuthorizedEmail_2.Text);
                writer.WriteElementString("NewAuthorizedPhone_2", txtNewAuthorizedPhone_2.Text);

                // ADDRESS(ADD) INFORMATION                
                    // Billing Address
                writer.WriteElementString("BillingStreet", txtBillingStreet.Text);
                writer.WriteElementString("BillingCity", txtBillingCity.Text);
                writer.WriteElementString("BillingState", txtBillingState.Text);
                writer.WriteElementString("BillingZip", txtBillingZip.Text);

                // Add Billing Address               
                writer.WriteElementString("BillingStreetAdd", txtBillingStreet.Text);
                writer.WriteElementString("BillingCityAdd", txtNewBillingCityAdd.Text);
                writer.WriteElementString("BillingStateAdd", txtNewBillingStateAdd.Text);
                writer.WriteElementString("BillingZipAdd", txtNewBillingZipCode.Text);

                //writer.WriteElementString("PhysicalStreet", txtPhysicalStreet.Text);
                //writer.WriteElementString("PhysicalCity", txtPhysicalCity.Text);
                //writer.WriteElementString("PhysicalState", txtPhysicalState.Text);
                //writer.WriteElementString("PhysicalZip", txtPhysicalZip.Text);

                // Mailing Address
                writer.WriteElementString("MailStreet", txtMailStreet.Text);
                writer.WriteElementString("MailCity", txtMailCity.Text);
                writer.WriteElementString("MailState", txtMailState.Text);
                writer.WriteElementString("MailZip", txtMailZip.Text);
                    
                //Add Billing Address
                writer.WriteElementString("MailStreetAdd", txtNewMailingStreetAdd.Text);
                writer.WriteElementString("MailCityAdd", txtNewMailingCityAdd.Text);
                writer.WriteElementString("MailStateAdd", txtNewMailingStateAdd.Text);
                writer.WriteElementString("MailZipAdd", txtNewMailingZipCode.Text);

                //Insurance Pay To Address
                writer.WriteElementString("InsuranceStreet", txtInsuranceStreet.Text);
                writer.WriteElementString("InsuranceCity", txtInsuranceCity.Text);
                writer.WriteElementString("InsuranceState", txtInsuranceState.Text);
                writer.WriteElementString("InsuranceZip", txtInsuranceZip.Text);

                //Add Insurance Pay To Address
                writer.WriteElementString("InsuranceStreetAdd", txtNewInsurancePayToStreetAdd.Text);
                writer.WriteElementString("InsuranceCityAdd", txtInsurancePayToCityAdd.Text);
                writer.WriteElementString("InsuranceStateAdd", txtInsurancePayToStateAdd.Text);
                writer.WriteElementString("InsuranceZipAdd", txtNewInsuranceToPayZipCode.Text);

                // Demographic Changes
                writer.WriteElementString("BusinessClosed", rbBusinessClosedYes.Checked ? "Yes" : "No");
                writer.WriteElementString("NursingHome", rbNursingHomeYes.Checked ? "Yes" : "No");
                writer.WriteElementString("DemographicChangesClientComments", txtDemographicChangesClientComments.Text);
                writer.WriteElementString("DemographicChangesAEComments", txtDemographicChangesAEComments.Text);

                // Information from Medicount
                writer.WriteElementString("ClientPortal", rbPortalYes.Checked ? "Yes" : "No");
                writer.WriteElementString("EmailAlert", rbAlertYes.Checked ? "Yes" : "No");
                writer.WriteElementString("ExclusionaryList", rbExclusionaryListYes.Checked ? "Yes" : "No");
                writer.WriteElementString("PCRReport", pcrReportBtnYes.Checked ? "Yes" : "No");
                writer.WriteElementString("ReconcileYourRuns", rbReconciliationYes.Checked ? "Yes" : "No");
                writer.WriteElementString("ReconcileComments", txtReconcileComments.Text);
                writer.WriteElementString("IfmClientComments", txtInformationFromMedicountClientComment.Text);
                writer.WriteElementString("IfmAEComments", txtInformationFromMedicountAEComment.Text);
                writer.WriteElementString("RunsReviewed", txtRunsReviewed.Text);
                writer.WriteElementString("RunsNotMet", txtRunsNotMet.Text);

                // CLIENT REVIEW INTERVAL
                string reviewInterval = "";
                if (rbIntervalQuarterly.Checked) reviewInterval = "Quarterly";
                else if (rbIntervalSemiAnnual.Checked) reviewInterval = "Semi-Annual";
                else if (rbIntervalYearly.Checked) reviewInterval = "Yearly";
                writer.WriteElementString("ReviewInterval", reviewInterval);
                writer.WriteElementString("NextReviewDate", txtNextReviewDate.Text);

                //// ePCR
                ////writer.WriteElementString("EpcrName", txtEpcrName.Text);
                //writer.WriteElementString("Reconciliation", rbReconciliationYes.Checked ? "Yes" : "No");
                //// writer.WriteElementString("ReconciliationDate", txtReconciliationDate.Text);

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            return xmlBuilder.ToString();
        }


        private int SaveToDatabase(string xmlData)
        {
            int reviewId = 0;

            // Get the connection string from web.config by name
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            //string connectionString = ConfigurationManager.ConnectionStrings["TestConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_InsertClientReview", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    SqlParameter xmlParam = new SqlParameter("@XmlData", SqlDbType.Xml);
                    xmlParam.Value = xmlData;
                    command.Parameters.Add(xmlParam);

                    SqlParameter idParam = new SqlParameter("@ReviewID", SqlDbType.Int);
                    idParam.Direction = ParameterDirection.Output;
                    command.Parameters.Add(idParam);

                    connection.Open();
                    command.ExecuteNonQuery();

                    reviewId = Convert.ToInt32(idParam.Value);
                }
            }

            return reviewId;
        }


        private string GetFullHtml()
        {// <link href='file:///C:/Users/harih/OneDrive/Desktop/ClientDetails1%20-%20Copy/ClientDetails1/Content/bootstrap.css' rel='stylesheet' />
            // <link href='file:///C:/inetpub/utility/ClientReviewForm_Test/Content/bootstrap.css' rel='stylesheet' />
            string innerHtml = GetFormHtml(); // this should return rendered HTML of your form
            return $@"
                    <!DOCTYPE html>
                    <html>
                    <head>
                        <meta charset='utf-8' />
                        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
                       <link href='file:///C:/inetpub/utility/ClientReviewForm_Test/Content/bootstrap.css' rel='stylesheet' />
                    </head>
                    <body>
                    {innerHtml}
                    </body>
                    </html>";
        }

        protected void btnGeneratePdf_Click(object sender, EventArgs e)
        {
            try
            {
                string fullHtml = GetFullHtml();
                string htmlPath = SaveHtmlToTempFile(fullHtml);
                byte[] pdf = ConvertHtmlToPdf(htmlPath);

                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment; filename=ClientReview.pdf");
                Response.BinaryWrite(pdf);
                Response.End();
            }
            catch (Exception ex)
            {
                Response.Write("<pre>" + Server.HtmlEncode(ex.ToString()) + "</pre>");
            }
        }

        private string SaveHtmlToTempFile(string html)
        {
            string folder = Server.MapPath("~/Temp/");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string path = System.IO.Path.Combine(folder, "ClientReview_" + DateTime.Now.Ticks + ".html");
            File.WriteAllText(path, html);
            return path;
        }


        public string GetFormHtml()
        {
            using (var sw = new StringWriter())
            {
                HtmlTextWriter htmlWriter = new HtmlTextWriter(sw);
                this.Form.RenderControl(htmlWriter); // Renders your form including current input values
                return sw.ToString();
            }
        }

        private byte[] ConvertHtmlToPdf(string htmlFilePath)
        {
            string wkhtmlPath = @"C:\Program Files\wkhtmltopdf\bin\wkhtmltopdf.exe";

            // file:/// URI format
            string inputFile = "file:///" + htmlFilePath.Replace("\\", "/");
            string outputPdf = System.IO.Path.ChangeExtension(htmlFilePath, ".pdf");

            var startInfo = new ProcessStartInfo
            {
                FileName = wkhtmlPath,
                Arguments = $"--enable-local-file-access --page-size A4 --viewport-size 1280x1024 \"{inputFile}\" \"{outputPdf}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (var process = Process.Start(startInfo))
            {
                string stderr = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    throw new Exception("wkhtmltopdf error: " + stderr);
                }
            }

            return File.ReadAllBytes(outputPdf);
        }

        // Helper method to clear the form (optional)
        private void ClearForm()
        {
            foreach (System.Web.UI.Control control in form1.Controls)
            {
                if (control is TextBox)
                {
                    ((TextBox)control).Text = string.Empty;
                }
            }
        }
     
        protected void toggleNonTransportComment(object sender, EventArgs e)
        {
            bool show = rbNonTransportYes.Checked;

            lblNontransportComments.Visible = false;
            txtNonTransportComment.Visible = false;

            // Optional to force update panel refresh
            upNonTransportComment.Update();
        }

        protected void ChangedChiefName(object sender, EventArgs e)
        {
            if (rbChiefCorrectNo.Checked)
            {

                //lblNewChiefName
                lblNewChiefName.Visible = false;
                txtNewChiefName.Visible = false;

                lblNewChiefEmail.Visible = false;
                txtNewChiefEmail.Visible = false;

                lblNewChiefPhoneNo.Visible = false;
                txtNewChiefPhone.Visible = false;

                lblCurrentChiefName.Visible = false;
                lblCurrentChiefEmail.Visible = false;
                lblCurrentChiefPhoneNo.Visible = false;
            }
            else
            {
                //lblNewChiefName
                lblNewChiefName.Visible = true;
                txtNewChiefName.Visible = true;

                lblNewChiefEmail.Visible = true;
                txtNewChiefEmail.Visible = true;

                lblNewChiefPhoneNo.Visible = true;
                txtNewChiefPhone.Visible = true;

                lblCurrentChiefName.Visible = false;                
                lblCurrentChiefEmail.Visible = false;                
                lblCurrentChiefPhoneNo.Visible = false;
            }
        }

        protected void ChangedFiscalName(object sender, EventArgs e)
        {
            if (rbFiscalCorrectNo.Checked)
            {
                lblNewFiscalOfficerName.Visible = false;
                txtNewFiscalName.Visible = false;

                lblNewFiscalOfficerEmail.Visible = false;
                txtNewFiscalEmail.Visible = false;

                lblNewFiscalOfficerPhoneNo.Visible = false;
                txtNewFiscalPhone.Visible = false;


                lblCurrentFiscalOfficerName.Visible = false;
                lblCurrentFiscalOfficerEmail.Visible = false;
                lblCurrentFiscalOfficerPhoneNo.Visible = false;

            }
            else
            {
               
                lblNewFiscalOfficerName.Visible = true;
                txtNewFiscalName.Visible = true;

                lblNewFiscalOfficerEmail.Visible = true;
                txtNewFiscalEmail.Visible = true;

                lblNewFiscalOfficerPhoneNo.Visible = true;
                txtNewFiscalPhone.Visible = true;


                lblCurrentFiscalOfficerName.Visible = false;
                lblCurrentFiscalOfficerEmail.Visible = false;
                lblCurrentFiscalOfficerPhoneNo.Visible = false;
            }
        }

        protected void ChangedAuthorizedName(object sender, EventArgs e)
        {
            if (rbOfficialCorrectNo.Checked)
            {
                lblNewAuthosizedOfficalName.Visible = false;
                txtNewAuthorizedName_1.Visible = false;

                lblNewAuthosizedOfficalEmail.Visible = false;
                txtNewAuthorizedEmail_1.Visible = false;

                lblNewAuthosizedOfficalPhoneNo.Visible = false;
                txtNewAuthorizedPhone_1.Visible = false;


                lblCurrentAuthosizedOfficalName.Visible = false;
                lblCurrentAuthosizedOfficalEmail.Visible = false;
                lblCurrentAuthosizedOfficalPhoneNo.Visible = false;

            }
            else
            {
                //lblNewAuthosizedOfficalName
                lblNewAuthosizedOfficalName.Visible = true;
                txtNewAuthorizedName_1.Visible = true;

                lblNewAuthosizedOfficalEmail.Visible = true;
                txtNewAuthorizedEmail_1.Visible = true;

                lblNewAuthosizedOfficalPhoneNo.Visible = true;
                txtNewAuthorizedPhone_1.Visible = true;


                lblCurrentAuthosizedOfficalName.Visible = false;
                lblCurrentAuthosizedOfficalEmail.Visible = false;
                lblCurrentAuthosizedOfficalPhoneNo.Visible = false;
            }            
        }

        protected void ChangedAuthorizedName_2(object sender, EventArgs e)
        {
            if (rbOfficialCorrectNo_2.Checked)
            {
                lblNewAuthosizedOfficalName_2.Visible = false;
                txtNewAuthorizedName_2.Visible = false;

                lblNewAuthosizedOfficalEmail_2.Visible = false;
                txtNewAuthorizedEmail_2.Visible = false;

                lblNewAuthosizedOfficalPhoneNo_2.Visible = false;
                txtNewAuthorizedPhone_2.Visible = false;


                lblCurrentAuthosizedOfficalName_2.Visible = false;
                lblCurrentAuthosizedOfficalEmail_2.Visible = false;
                lblCurrentAuthosizedOfficalPhoneNo_2.Visible = false;

            }
            else
            {
                //lblNewAuthosizedOfficalName
                lblNewAuthosizedOfficalName_2.Visible = true;
                txtNewAuthorizedName_2.Visible = true;

                lblNewAuthosizedOfficalEmail_2.Visible = true;
                txtNewAuthorizedEmail_2.Visible = true;

                lblNewAuthosizedOfficalPhoneNo_2.Visible = true;
                txtNewAuthorizedPhone_2.Visible = true;


                lblCurrentAuthosizedOfficalName_2.Visible = false;
                lblCurrentAuthosizedOfficalEmail_2.Visible = false;
                lblCurrentAuthosizedOfficalPhoneNo_2.Visible = false;
            }
        }

        protected void ChangedBillingAddress(object sender, EventArgs e)
        {
            bool showFields = rbBilingAddressYes.Checked;

            //lblPhysicalStreet.Visible = showFields;
            //txtPhysicalStreet.Visible = showFields;

            //lblPhysicalCity.Visible = showFields;
            //txtPhysicalCity.Visible = showFields;

            //lblPhysicalState.Visible = showFields;
            //txtPhysicalState.Visible = showFields;

            //lblPhysicalZipCode.Visible = showFields;
            //txtPhysicalZip.Visible = showFields;

            //txtMailCity.Visible = showFields;
            //txtMailState.Visible = showFields;
            //bool showFields = rbBilingAddressYes.Checked;

            lblNewBillingStreetAdd.Visible = showFields;
            txtNewBillingStreetAdd.Visible = showFields;

            lblNewBillingCityAdd.Visible = showFields;
            txtNewBillingCityAdd.Visible = showFields;

            lblNewBillingStateAdd.Visible = showFields;
            txtNewBillingStateAdd.Visible = showFields;

            lblNewBillingZipCode.Visible = showFields;
            txtNewBillingZipCode.Visible = showFields;



        }
        protected void ChangedPhysicalLocationAddress(object sender, EventArgs e)
        {
            bool showFields = rbPhysicalAddressYes.Checked;

            lblNewPhysicalLocationStreetAdd.Visible = showFields;
            txtNewPhysicalLocationStreetAdd.Visible = showFields;

            lblNewPhysicalLocationCityAdd.Visible = showFields;
            txtNewPhysicalLocationCityAdd.Visible = showFields;

            lblNewPhysicalLocationStateAdd.Visible = showFields;
            txtNewPhysicalLocationStateAdd.Visible = showFields;

            lblNewPhysicalLocationZipCode.Visible = showFields;
            txtNewPhysicalLocationZipCode.Visible = showFields;

        }
        protected void ChangedMailingAddress(object sender, EventArgs e)
        {
            bool showFields = rbMailingAddressYes.Checked;

            lblNewMailingStreetAdd.Visible = showFields;
            txtNewMailingStreetAdd.Visible = showFields;

            lblNewMailingCityAdd.Visible = showFields;
            txtNewMailingCityAdd.Visible = showFields;

            lblNewMailingStateAdd.Visible = showFields;
            txtNewMailingStateAdd.Visible = showFields;

            lblNewMailingZipCode.Visible = showFields;
            txtNewMailingZipCode.Visible = showFields;

        }
        protected void ChangedInsurancePayToAddress(object sender, EventArgs e)
        {
            bool showFields = rbInsurancePayToAddressYes.Checked;

            lblNewInsurancePayToStreetAdd.Visible = showFields;
            txtNewInsurancePayToStreetAdd.Visible = showFields;

            lblInsurancePayToCityAdd.Visible = showFields;
            txtInsurancePayToCityAdd.Visible = showFields;

            lblInsurancePayToStateAdd.Visible = showFields;
            txtInsurancePayToStateAdd.Visible = showFields;

            lblNewInsuranceToPayZipCode.Visible = showFields;
            txtNewInsuranceToPayZipCode.Visible = showFields;

        }

        protected void CheckedReconcileComments(object sender, EventArgs e)
        {
            // If YES is selected, show the dropdown
            if (rbReconciliationYes.Checked)
            {
                lblReconcileComments.Visible = false;
                txtReconcileComments.Visible = false;

                ddlFrequency.Visible = true;
            }
            // If NO is selected, show the comment box
            else if (rbReconciliationNo.Checked)
            {
                ddlFrequency.Visible = false;

                lblReconcileComments.Visible = false;
                txtReconcileComments.Visible = false;
            }

            // Update the UpdatePanel manually if needed
            upReconcileComment.Update();
        }

        // Override to allow rendering form controls outside normal rendering pipeline
        public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
        {
            // Needed for RenderControl to work without error
        }

        private void BindClientNumbers()
        {
            // Example: get data from DB
            // string query = "SELECT ClientID, ClientNumber FROM Clients";
            string Companycode = "";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("[dbo].[spCRF_GetDetailsForClientReviewForm]", connection))
                {                   
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CompanyCode", Companycode);
                    SqlDataReader rdr = cmd.ExecuteReader();

                    ddlClientNumber.DataSource = rdr;
                    ddlClientNumber.DataTextField = "CompanyCode";  // what user sees
                    ddlClientNumber.DataValueField = "CompanyCode";     // actual value
                    ddlClientNumber.DataBind();
                }
            }

            // Add default option
            ddlClientNumber.Items.Insert(0, new ListItem("--Client Num--", ""));
        }

        public void ResetForm(System.Web.UI.Control parent)
        {
            foreach (System.Web.UI.Control control in parent.Controls)
            {
                // Recursively clear nested controls
                if (control.HasControls())
                {
                    ResetForm(control); // Call recursively
                }

                // Reset Web Forms controls
                if (control is TextBox)
                {
                    ((TextBox)control).Text = string.Empty;
                }

                if (control is DropDownList)
                {
                    ((DropDownList)control).SelectedIndex = -1;
                }

                if (control is CheckBox)
                {
                    ((CheckBox)control).Checked = false;
                }

                if (control is RadioButton)
                {
                    ((RadioButton)control).Checked = false;
                }

                if (control is RadioButtonList)
                {
                    ((RadioButtonList)control).SelectedIndex = -1;
                }

                if (control is CheckBoxList)
                {
                    foreach (ListItem item in ((CheckBoxList)control).Items)
                    {
                        item.Selected = false;
                    }
                }

                if (control is FileUpload)
                {
                    ((FileUpload)control).Attributes.Clear();
                }
            }
        }


        protected void ddlClientNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            string getClientIdWithName = ddlClientNumber.SelectedValue;
            ResetForm(this);
            ddlClientNumber.Text = getClientIdWithName;
            txtReportType.Text = "Date of Entry";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("[dbo].[sp_GetClientInfoWithClientID]", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClientId", getClientIdWithName);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read()) // Read first row
                        {
                            // Example: if SP returns ClientName column
                            txtClientName.Text = rdr["CompanyName"].ToString();
                            txtAccountExecutive.Text = rdr["AccountExecutive"].ToString();
                            txtEmail.Text = rdr["AccountExecutiveEmailID"].ToString();
                            txtPhone.Text = rdr["AccountExecutivePhone"].ToString();

                        }
                    }

                }
            }

            txtMailStreet.Text = txtMailCity.Text = txtMailState.Text = txtMailZip.Text = ""; // MAILING ADDRESS
            txtCurrentChiefName.Text = txtNewChiefName.Text = txtNewChiefEmail.Text = txtNewChiefPhone.Text = ""; // CHIEF
            txtCurrentFiscalOfficer.Text = txtNewFiscalName.Text = txtNewFiscalEmail.Text = txtNewFiscalPhone.Text = ""; // FISCAL OFFICER
            txtCurrentAuthorizedOfficial_1.Text = txtNewAuthorizedName_1.Text = txtNewAuthorizedEmail_1.Text = txtNewAuthorizedPhone_1.Text = ""; // AUTHORIZED OFFICIAL-1
            txtCurrentAuthorizedOfficial_2.Text = txtNewAuthorizedName_2.Text = txtNewAuthorizedEmail_2.Text = txtNewAuthorizedPhone_2.Text = ""; // AUTHORIZED OFFICIAL-2

            FrmClientZohoApiCredentials ZohoCred = new FrmClientZohoApiCredentials();

            ZohoCred.ClientId = ConfigurationManager.AppSettings["ZohoClientId"].ToString();
            ZohoCred.ClientSecret = ConfigurationManager.AppSettings["ZohoClientSecret"].ToString();
            ZohoCred.RefreshToken = ConfigurationManager.AppSettings["ZohoRefreshToken"].ToString();
            string accessToken = GetAccessTokenFromRefreshToken(ZohoCred);
            if (accessToken != "")
            {
                string url = $"https://www.zohoapis.com/crm/v8/Accounts/search?criteria=(Account_Number:equals:{getClientIdWithName})";
                string zohoData = MakeZohoApiRequest("GET", url, accessToken);

                if (!string.IsNullOrEmpty(zohoData))
                {
                    var jsonObj = JObject.Parse(zohoData);
                    var dataArray = jsonObj["data"]?.ToObject<List<JObject>>();

                    if (dataArray != null && dataArray.Count > 0)
                    {
                        var contact = dataArray[0];
                        Dictionary<String, List<string>> authorizedOfficialDict = new Dictionary<String, List<string>>();
                        List<string> ChiefList = new List<string>
                        {
                            "CHIEF",
                        };
                        List<string> FiscalOfficerList = new List<string>
                        {
                             "FISCAL OFFICER",
                        };

                        string ContactUrl = $"https://www.zohoapis.com/crm/v8/Contacts/search?criteria=(Account_Name:equals:{contact["id"]})";
                        string zohoContactData = MakeZohoApiRequest("GET", ContactUrl, accessToken);
                        var jsonContactObj = JObject.Parse(zohoContactData);
                        var ContactDataArray = jsonContactObj["data"]?.ToObject<List<JObject>>();
                        if (ContactDataArray != null && ContactDataArray.Count > 0)
                        {
                            int i = 1;
                            foreach (var ContactData in ContactDataArray)
                            {
                                // CHIEF
                                if (ChiefList.Select(chief => chief.Trim().ToUpper()).Contains(ContactData["Title"]?.ToString().Trim().ToUpper()))
                                {
                                    //txtCurrentChiefName.Text = ContactData["Title"]?.ToString();
                                    txtCurrentChiefName.Text = $"{ContactData["First_Name"]?.ToString()} {ContactData["Last_Name"]?.ToString()}";
                                    txtNewChiefName.Text = $"{ContactData["First_Name"]?.ToString()} {ContactData["Last_Name"]?.ToString()}";
                                    txtNewChiefEmail.Text = ContactData["Email"]?.ToString();
                                    txtNewChiefPhone.Text = ContactData["Phone"]?.ToString();


                                }

                                // FISCAL OFFICER
                                if (FiscalOfficerList.Select(fiscalOff => fiscalOff.Trim().ToUpper()).Contains(ContactData["Title"]?.ToString().Trim().ToUpper()))
                                {
                                    //txtCurrentFiscalOfficer.Text = ContactData["Title"]?.ToString();
                                    txtCurrentFiscalOfficer.Text = $"{ContactData["First_Name"]?.ToString()} {ContactData["Last_Name"]?.ToString()}";
                                    txtNewFiscalName.Text = $"{ContactData["First_Name"]?.ToString()} {ContactData["Last_Name"]?.ToString()}";
                                    txtNewFiscalEmail.Text = ContactData["Email"]?.ToString();
                                    txtNewFiscalPhone.Text = ContactData["Phone"]?.ToString();
                                }

                                if (ContactData["Medicare_Authorized_Official"].ToString().ToUpper() == "TRUE")
                                {
                                    authorizedOfficialDict[$"Authorized Official {i}"] = new List<string>
                                    {
                                        //ContactData["Title"]?.ToString(),
                                        $"{ContactData["First_Name"]?.ToString()} {ContactData["Last_Name"]?.ToString()}",
                                        $"{ContactData["First_Name"]?.ToString()} {ContactData["Last_Name"]?.ToString()}",
                                        ContactData["Email"]?.ToString(),
                                        ContactData["Phone"]?.ToString(),
                                    };
                                    i++;
                                }
                            }

                            if (authorizedOfficialDict.Count > 0)
                            {
                                // AUTHORIZED OFFICIAL-1
                                txtCurrentAuthorizedOfficial_1.Text = authorizedOfficialDict["Authorized Official 1"][0];
                                txtNewAuthorizedName_1.Text = authorizedOfficialDict["Authorized Official 1"][1];
                                txtNewAuthorizedEmail_1.Text = authorizedOfficialDict["Authorized Official 1"][2];
                                txtNewAuthorizedPhone_1.Text = authorizedOfficialDict["Authorized Official 1"][3];
                                if (authorizedOfficialDict.Count == 2)
                                {
                                    // AUTHORIZED OFFICIAL-2
                                    txtCurrentAuthorizedOfficial_2.Text = authorizedOfficialDict["Authorized Official 2"][0];
                                    txtNewAuthorizedName_2.Text = authorizedOfficialDict["Authorized Official 2"][1];
                                    txtNewAuthorizedEmail_2.Text = authorizedOfficialDict["Authorized Official 2"][2];
                                    txtNewAuthorizedPhone_2.Text = authorizedOfficialDict["Authorized Official 2"][3];
                                }
                            }

                        }
                        else
                        {
                            Console.WriteLine("No Contact Data");
                        }
                        txtMailStreet.Text = contact["Mailing_Street"]?.ToString();
                        txtMailCity.Text = contact["Mailing_City"]?.ToString();
                        txtMailState.Text = contact["Mailing_State"]?.ToString();
                        txtMailZip.Text = contact["Mailing_Zip"]?.ToString();
                    }
                    else
                    {
                        txtMailStreet.Text = txtMailCity.Text = txtMailState.Text = txtMailZip.Text = "N/A";
                    }

                }
            }
            else
            {
                Console.WriteLine("Zoho CRM Access Token Error");
            }

        }

        public static string CleanedVersionOfValues(object value, bool removeDecimal = true, string type = "AMOUNT")
        {
            
            if (type.ToUpper() != "AMOUNT")
            {
                decimal numericValue = 0;
                if (value is float || value is decimal || value is int)
                {
                    numericValue = Convert.ToDecimal(value) * 100;
                }


                if (removeDecimal)
                {
                    return $"{numericValue.ToString("F0")} %";
                }
                else
                {
                    return $"{numericValue.ToString("F2")} %";
                }


            }
            else
            {
                string strValue = value.ToString();
                
                if (decimal.TryParse(strValue, out decimal result))
                    {
                        // Format with thousand separators and 2 decimal places
                        strValue =  result.ToString("C2", CultureInfo.CreateSpecificCulture("en-US"));
                    }
                
                if (removeDecimal)
                {
                    if (strValue == "")
                        return "$0";

                    return strValue.Substring(0, strValue.IndexOf('.'));
                }
                else
                {
                    if (strValue == "")
                        return "$0";

                    return strValue;
                }
            }     
        }


        protected void txtClientRevenueNumberEndDate_TextChanged(object sender, EventArgs e)
        {
            string companyID = ddlClientNumber.SelectedValue;
            if (Session["StartDate"] != null)
                txtClientRevenueNumberStartDate.Text = DateTime.ParseExact(Session["StartDate"].ToString(), "yyyy-MM-dd", null).ToString("yyyy-MM-dd");
            string startDateFormatted = txtClientRevenueNumberStartDate.Text;
           
            if (Session["EndDate"] != null)
                txtClientRevenueNumberEndDate.Text = DateTime.ParseExact(Session["EndDate"].ToString(), "yyyy-MM-dd", null).ToString("yyyy-MM-dd");
            string endDateFormatted = txtClientRevenueNumberEndDate.Text;

            
            using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("[dbo].[sp_GetClientReviewFormDetails]", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                         cmd.CommandTimeout = 360;
                         cmd.Parameters.AddWithValue("@CompanyKey", companyID);
                        cmd.Parameters.AddWithValue("@Period1BeginDate", startDateFormatted);
                        cmd.Parameters.AddWithValue("@Period1EndDate", endDateFormatted);
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read()) 
                            {                            
                                txtTransports.Text = ((long)Convert.ToDouble(rdr["Runs_Prev"])).ToString("N0", new System.Globalization.CultureInfo("en-US"));
                                txtRevenuePerTransport.Text = CleanedVersionOfValues(rdr["RPT_Prev"]);
                                txtCharges.Text = CleanedVersionOfValues(rdr["Charges_Prev"]);
                                txtPayments.Text = CleanedVersionOfValues(rdr["Payments_Prev"]);
                                txtAdjustments.Text = CleanedVersionOfValues(rdr["Adjustments_Prev"]);
                                txtWriteOffs.Text = CleanedVersionOfValues(rdr["WriteOffs_Prev"]);
                                txtCollectionRate.Text = CleanedVersionOfValues(rdr["Collection_Rate_Prev"], removeDecimal:false, type: "PERCENTAGE");
                                if (rdr["StartDate"] != DBNull.Value)
                                txtClientRevenueNumberStartDate.Text = Convert.ToDateTime(rdr["StartDate"]).ToString("MM-dd-yyyy");

                                if (rdr["EndDate"] != DBNull.Value)
                                txtClientRevenueNumberEndDate.Text = Convert.ToDateTime(rdr["EndDate"]).ToString("MM-dd-yyyy");

                            }
                        }

                    }
                }
            GetChargeRate(companyID);
            
        }
        protected void GetChargeRate(string Companycode)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("[dbo].[spCRF_GetDetailsForClientReviewForm]", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 240;
                    cmd.Parameters.AddWithValue("@CompanyCode", Companycode);
                    
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            txtBls.Text = CleanedVersionOfValues(rdr["BLSE"]);
                            txtBlsNe.Text = CleanedVersionOfValues(rdr["BLSNE"]);
                            txtAls.Text = CleanedVersionOfValues(rdr["ALSE"]);
                            txtAlsNe.Text = CleanedVersionOfValues(rdr["ALSNE"]);
                            txtAls2.Text = CleanedVersionOfValues(rdr["ALS2"]);
                            txtMileage.Text = CleanedVersionOfValues(rdr["Ground_Mileage"]);

                            //Address information for Insurance 
                            txtInsuranceStreet.Text = rdr["InsPayToAddress"].ToString();
                            txtInsuranceCity.Text = rdr["InsPayToCity"].ToString();
                            txtInsuranceState.Text = rdr["InsPayToState"].ToString();
                            txtInsuranceZip.Text = rdr["InsPayToZip"].ToString();

                            // address information for Billing  address and physical 

                            txtBillingStreet.Text = rdr["PhysicalAddress"].ToString();
                            txtBillingCity.Text = rdr["PhysicalCity"].ToString();
                            txtBillingState.Text = rdr["PhysicalState"].ToString();
                            txtBillingZip.Text = rdr["PhysicalZip"].ToString();
                        }
                    }

                }
            }
        }

        private string GetAccessTokenFromRefreshToken(FrmClientZohoApiCredentials zohoCred)
        {
            try
            {
                string postData = $"refresh_token={zohoCred.RefreshToken}&client_id={zohoCred.ClientId}&client_secret={zohoCred.ClientSecret}&grant_type=refresh_token";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(zohoAuthUrl);
                byte[] data = Encoding.UTF8.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                using (WebResponse response = request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var responseText = reader.ReadToEnd();
                    JObject tokenObj = JObject.Parse(responseText);
                    return tokenObj["access_token"]?.ToString() ?? string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        private string MakeZohoApiRequest(string method, string url, string accessToken, string jsonPayload = null)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method;
                request.Headers.Add("Authorization", $"Zoho-oauthtoken {accessToken}");

                if (jsonPayload != null)
                {
                    byte[] byteArray = Encoding.UTF8.GetBytes(jsonPayload);
                    request.ContentType = "application/json";
                    request.ContentLength = byteArray.Length;
                    using (Stream dataStream = request.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);
                    }
                }

                using (WebResponse response = request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                // Handle only GET method error by returning null
                if (method == "GET")
                {
                    return null;
                }

                // For PUT or other methods, optionally rethrow or log the exception
                throw; // Or log and return error details if preferred
            }
        }

    }
}

public class FrmClientZohoApiCredentials
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string RefreshToken { get; set; }
}