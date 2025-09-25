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
using System.IO;
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
        
        //protected void btnSubmit_Click(object sender, EventArgs e)
        //{
        //    if (Page.IsValid)
        //    {
        //        try
        //        {
        //            // Create XML from form data
        //            XElement clientReviewXml = new XElement("ClientReview",
        //                // Meeting Details
        //                new XElement("ClientNumber", txtClientNumber.Text),
        //                new XElement("ClientName", txtClientName.Text),
        //                new XElement("MeetingDate", string.IsNullOrEmpty(txtMeetingDate.Text) ? null : txtMeetingDate.Text),
        //                new XElement("AccountExecutive", txtAccountExecutive.Text),
        //                new XElement("Email", txtEmail.Text),
        //                new XElement("Phone", txtPhone.Text),

        //                // Attendees Invited
        //                new XElement("AttendeeName", txtName.Text),
        //                new XElement("AttendeeTitle", txtTitle.Text),
        //                new XElement("AttendeeEmail", txtAIEmail.Text),
        //                new XElement("RevenueStartDate", string.IsNullOrEmpty(txtRevenueStartDate.Text) ? null : txtRevenueStartDate.Text),
        //                new XElement("RevenueEndDate", string.IsNullOrEmpty(txtRevenueEndDate.Text) ? null : txtRevenueEndDate.Text),

        //                // RPT and Collection Rates
        //                new XElement("YtdRevenue", string.IsNullOrEmpty(txtYtdRevenue.Text) ? null : txtYtdRevenue.Text),
        //                new XElement("YtdTransports", string.IsNullOrEmpty(txtYtdTransports.Text) ? null : txtYtdTransports.Text),
        //                new XElement("RevenuePerTransport", string.IsNullOrEmpty(txtRevenuePerTransport.Text) ? null : txtRevenuePerTransport.Text),
        //                new XElement("Charges", string.IsNullOrEmpty(txtCharges.Text) ? null : txtCharges.Text),
        //                new XElement("Payments", string.IsNullOrEmpty(txtPayments.Text) ? null : txtPayments.Text),
        //                new XElement("Adjustments", string.IsNullOrEmpty(txtAdjustments.Text) ? null : txtAdjustments.Text),
        //                new XElement("WriteOffs", string.IsNullOrEmpty(txtWriteOffs.Text) ? null : txtWriteOffs.Text),
        //                new XElement("CollectionRate", string.IsNullOrEmpty(txtCollectionRate.Text) ? null : txtCollectionRate.Text),

        //                // Billing Rate Review
        //                new XElement("LastRateChange", txtLastRateChange.Text),
        //                new XElement("BlsRate", string.IsNullOrEmpty(txtBls.Text) ? null : txtBls.Text),
        //                new XElement("BlsNeRate", string.IsNullOrEmpty(txtBlsNe.Text) ? null : txtBlsNe.Text),
        //                new XElement("AlsRate", string.IsNullOrEmpty(txtAls.Text) ? null : txtAls.Text),
        //                new XElement("AlsNeRate", string.IsNullOrEmpty(txtAlsNe.Text) ? null : txtAlsNe.Text),
        //                new XElement("Als2Rate", string.IsNullOrEmpty(txtAls2.Text) ? null : txtAls2.Text),
        //                new XElement("NonTransport", rbNonTransportYes.Checked),
        //                new XElement("MileageRate", string.IsNullOrEmpty(txtMileage.Text) ? null : txtMileage.Text),
        //                new XElement("RateChanges", rbRateChangesYes.Checked),

        //                // Contract Status
        //                new XElement("RenewalDate", string.IsNullOrEmpty(txtRenewalDate.Text) ? null : txtRenewalDate.Text),
        //                new XElement("CurrentRate", string.IsNullOrEmpty(txtCurrentRate.Text) ? null : txtCurrentRate.Text),

        //                // Personnel Changes
        //                new XElement("Chief", txtChief.Text),
        //                new XElement("ChiefCorrect", rbChiefCorrectYes.Checked),
        //                new XElement("ChiefIfNot", txtChiefIfNot.Text),
        //                new XElement("FiscalOfficer", txtFiscalOfficer.Text),
        //                new XElement("FiscalCorrect", rbFiscalCorrectYes.Checked),
        //                new XElement("FiscalIfNot", txtFiscalIfNot.Text),
        //                new XElement("AuthorizedOfficial", txtAuthorizedOfficial.Text),
        //                new XElement("OfficialCorrect", rbOfficialCorrectYes.Checked),
        //                new XElement("OfficialIfNot", txtOfficialIfNot.Text),

        //                // Demographic Changes
        //                new XElement("MajorBusinessClosed", rbBusinessClosedYes.Checked),
        //                new XElement("NursingHomeChanges", rbNursingHomeYes.Checked),

        //                // New Business
        //                new XElement("UseClientPortal", rbPortalYes.Checked),
        //                new XElement("ReceivingAlerts", rbAlertYes.Checked),
        //                new XElement("AdditionalTraining", rbTrainingYes.Checked),
        //                new XElement("RunsReviewed", string.IsNullOrEmpty(txtRunsReviewed.Text) ? null : txtRunsReviewed.Text),
        //                new XElement("RunsNotMet", string.IsNullOrEmpty(txtRunsNotMet.Text) ? null : txtRunsNotMet.Text),

        //                // Client Review Intervals
        //                new XElement("ReviewInterval", rbIntervalSemiAnnual.Checked ? "Semi-Annual" : "Yearly"),
        //                new XElement("NextReviewDate", string.IsNullOrEmpty(txtNextReviewDate.Text) ? null : txtNextReviewDate.Text),

        //                // ePCR
        //                new XElement("EpcrName", txtEpcrName.Text),

        //                // Reconciliation
        //                new XElement("ReconciliationStatus", rbReconciliationYes.Checked),
        //                new XElement("ReconciliationDate", rbReconciliationYes.Checked && !string.IsNullOrEmpty(txtReconciliationDate.Text) ?
        //                    txtReconciliationDate.Text : null),

        //                // Address Information
        //                new XElement("BillingStreet", txtBillingStreet.Text),
        //                new XElement("BillingCity", txtBillingCity.Text),
        //                new XElement("BillingState", txtBillingState.Text),
        //                new XElement("BillingZip", txtBillingZip.Text),
        //                new XElement("PhysicalState", txtPhysicalState.Text),
        //                new XElement("PhysicalZip", txtPhysicalZip.Text)
        //            );

        //            string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

        //            using (SqlConnection connection = new SqlConnection(connectionString))
        //            {
        //                using (SqlCommand command = new SqlCommand("sp_InsertClientReview", connection))
        //                {
        //                    command.CommandType = CommandType.StoredProcedure;

        //                    // Add XML parameter
        //                    SqlParameter xmlParam = command.Parameters.Add("@ClientReviewXML", SqlDbType.Xml);
        //                    xmlParam.Value = clientReviewXml.ToString();

        //                    // Add created by parameter
        //                    //command.Parameters.AddWithValue("@CreatedBy", User.Identity.Name ?? "System");

        //                    connection.Open();
        //                    var newId = command.ExecuteScalar();

        //                    if (newId != null)
        //                    {
        //                        ScriptManager.RegisterStartupScript(this, this.GetType(), "success",
        //                            "alert('Client review submitted successfully! ID: " + newId.ToString() + "');", true);
        //                    }
        //                    else
        //                    {
        //                        ScriptManager.RegisterStartupScript(this, this.GetType(), "error",
        //                            "alert('Error submitting the form. Please try again.');", true);
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            // Log the error
        //            // LogError(ex);

        //            // Show user-friendly error message
        //            ScriptManager.RegisterStartupScript(this, this.GetType(), "error",
        //                $"alert('An error occurred: {ex.Message}');", true);
        //        }
        //    }
        //}


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


        //private int SaveToDatabase(string xmlData)
        //{
        //    int reviewId = 0;
        //    string connectionString = "Your_Connection_String_Here";

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        using (SqlCommand command = new SqlCommand("sp_InsertClientReview", connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;

        //            // Add parameters
        //            SqlParameter xmlParam = new SqlParameter("@XmlData", SqlDbType.Xml);
        //            xmlParam.Value = xmlData;
        //            command.Parameters.Add(xmlParam);

        //            SqlParameter idParam = new SqlParameter("@ReviewID", SqlDbType.Int);
        //            idParam.Direction = ParameterDirection.Output;
        //            command.Parameters.Add(idParam);

        //            connection.Open();
        //            command.ExecuteNonQuery();

        //            reviewId = Convert.ToInt32(idParam.Value);
        //        }
        //    }

        //    return reviewId;
        //}


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
                    ddlClientNumber.DataTextField = "CompanyName";  // what user sees
                    ddlClientNumber.DataValueField = "CompanyCode";     // actual value
                    ddlClientNumber.DataBind();
                }
            }

            // Add default option
            ddlClientNumber.Items.Insert(0, new ListItem("-- Select Client Number --", ""));
        }     

        protected void ddlClientNumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            string getClientIdWithName = ddlClientNumber.SelectedValue;
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

            FrmClientZohoApiCredentials ZohoCred = new FrmClientZohoApiCredentials();

            ZohoCred.ClientId = ConfigurationManager.AppSettings["ZohoClientId"].ToString();
            ZohoCred.ClientSecret = ConfigurationManager.AppSettings["ZohoClientSecret"].ToString();
            ZohoCred.RefreshToken = ConfigurationManager.AppSettings["ZohoRefreshToken"].ToString();
            string accessToken = GetAccessTokenFromRefreshToken(ZohoCred);
            string url = $"https://www.zohoapis.com/crm/v8/Accounts/search?criteria=(Account_Number:equals:{getClientIdWithName})";
            string zohoData = MakeZohoApiRequest("GET", url, accessToken);

            if (!string.IsNullOrEmpty(zohoData))
            {
                var jsonObj = JObject.Parse(zohoData);
                var dataArray = jsonObj["data"]?.ToObject<List<JObject>>();

                if (dataArray != null && dataArray.Count > 0)
                {
                    var contact = dataArray[0];

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
                                txtTransports.Text = rdr["Payments_Prev"].ToString();
                                txtRevenuePerTransport.Text = rdr["RPT_Prev"].ToString();
                                txtCharges.Text = rdr["Charges_Prev"].ToString();
                                txtPayments.Text = rdr["Payments_Prev"].ToString();
                                txtAdjustments.Text = rdr["Adjustments_Prev"].ToString();
                                txtWriteOffs.Text = rdr["WriteOffs_Prev"].ToString();
                                txtCollectionRate.Text = rdr["Collection_Rate_Prev"].ToString();
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
                            txtBls.Text = rdr["BLSE"].ToString();
                            txtBlsNe.Text = rdr["BLSNE"].ToString();
                            txtAls.Text = rdr["ALSE"].ToString();
                            txtAlsNe.Text = rdr["ALSNE"].ToString();
                            txtAls2.Text = rdr["ALS2"].ToString();
                            txtMileage.Text = rdr["Ground_Mileage"].ToString();

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
            //string url = "https://accounts.zoho.com/oauth/v2/token";
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
                return tokenObj["access_token"].ToString();
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



