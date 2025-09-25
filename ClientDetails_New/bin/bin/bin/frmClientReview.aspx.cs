using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Office2013.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using iText.Html2pdf;
using iText.Html2pdf.Attach.Impl.Tags;
using iText.Html2pdf.Resolver.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout.Font;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // rbNonTransportYes.Attributes["onclick"] = "toggleNonTransportComment();";
                // rbNonTransportNo.Attributes["onclick"] = "toggleNonTransportComment();";
                // Set initial value in mm/dd/yyyy format
                // txtMeetingDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                // Initialize form fields if needed
                //txtMeetingDate.Text = DateTime.Today.ToString("yyyy-MM-dd");
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

                // Meeting Details
                writer.WriteElementString("ClientNumber", txtClientNumber.Text);
                writer.WriteElementString("ClientName", txtClientName.Text);
                writer.WriteElementString("MeetingDate", txtMeetingDate.Text);
                writer.WriteElementString("AccountExecutive", txtAccountExecutive.Text);
                writer.WriteElementString("Email", txtEmail.Text);
                writer.WriteElementString("Phone", txtPhone.Text);
                writer.WriteElementString("AttendeesName", txtName.Text);
                writer.WriteElementString("AttendeesTitle", txtTitle.Text);
                writer.WriteElementString("AttendeesEmail", txtAIEmail.Text);
                writer.WriteElementString("RevenueStartDate", txtRevenueStartDate.Text);
                writer.WriteElementString("RevenueEndDate", txtRevenueEndDate.Text);
                writer.WriteElementString("DateOfService", ddlDates.Text);
                // writer.WriteElementString("DateOfService", txtDateofService.Text);

                // RPT and Collection Rates
                writer.WriteElementString("YtdTransports", txtYtdTransports.Text);
                writer.WriteElementString("YtdRevenue", txtYtdRevenue.Text);
                writer.WriteElementString("RevenuePerTransport", txtRevenuePerTransport.Text);
                writer.WriteElementString("ClientComments", txtClientComments.Text);
                writer.WriteElementString("Charges", txtCharges.Text);
                writer.WriteElementString("Payments", txtPayments.Text);
                writer.WriteElementString("Adjustments", txtAdjustments.Text);
                writer.WriteElementString("AEComments", txtAEComments.Text);
                writer.WriteElementString("WriteOffs", txtWriteOffs.Text);
                writer.WriteElementString("CollectionRate", txtCollectionRate.Text);

                // Billing Rate Review
                writer.WriteElementString("LastRateChange", txtLastRateChange.Text);
                writer.WriteElementString("Bls", txtBls.Text);
                writer.WriteElementString("BlsNe", txtBlsNe.Text);
                writer.WriteElementString("BillingClientComments", txtBillingClientComments.Text);
                writer.WriteElementString("Als", txtAls.Text);
                writer.WriteElementString("AlsNe", txtAlsNe.Text);
                writer.WriteElementString("Als2", txtAls2.Text);
                writer.WriteElementString("BillingAEComments", txtBillingAEComments.Text);
                writer.WriteElementString("RateChanges", rbRateChangesYes.Checked ? "Yes" : "No");
                writer.WriteElementString("Mileage", txtMileage.Text);
                writer.WriteElementString("NonTransport", rbNonTransportYes.Checked ? "Yes" : "No");

                // Contract Status
                writer.WriteElementString("RenewalDate", txtRenewalDate.Text);
                writer.WriteElementString("CurrentRate", txtCurrentRate.Text);

                // Personnel Changes
                writer.WriteElementString("Chief", txtChief.Text);
                writer.WriteElementString("ChiefCorrect", rbChiefCorrectYes.Checked ? "Yes" : "No");
                //writer.WriteElementString("ChiefIfNot", txtChiefIfNot.Text);
                writer.WriteElementString("ChiefName", txtChiefName.Text);
                writer.WriteElementString("ChiefEmail", txtChiefEmail.Text);
                writer.WriteElementString("ChiefPhone", txtChiefPhone.Text);

                writer.WriteElementString("FiscalOfficer", txtFiscalOfficer.Text);
                writer.WriteElementString("FiscalCorrect", rbFiscalCorrectYes.Checked ? "Yes" : "No");
                // writer.WriteElementString("FiscalIfNot", txtFiscalIfNot.Text);
                writer.WriteElementString("FiscalName", txtFiscalName.Text);
                writer.WriteElementString("FiscalEmail", txtFiscalEmail.Text);
                writer.WriteElementString("FiscalPhone", txtFiscalPhone.Text);

                writer.WriteElementString("AuthorizedOfficial", txtAuthorizedOfficial.Text);
                writer.WriteElementString("OfficialCorrect", rbOfficialCorrectYes.Checked ? "Yes" : "No");
                // writer.WriteElementString("OfficialIfNot", txtOfficialIfNot.Text);
                writer.WriteElementString("AuthorizedName", txtAuthorizedName.Text);
                writer.WriteElementString("AuthorizedEmail", txtAuthorizedEmail.Text);
                writer.WriteElementString("AuthorizedPhone", txtAuthorizedPhone.Text);

                // Address Information
                //writer.WriteElementString("BillingStreet", txtBillingStreet.Text);
                //writer.WriteElementString("BillingCity", txtBillingCity.Text);
                //writer.WriteElementString("BillingState", txtBillingState.Text);
                //writer.WriteElementString("BillingZip", txtBillingZip.Text);

                //writer.WriteElementString("PhysicalStreet", txtPhysicalStreet.Text);
                //writer.WriteElementString("PhysicalCity", txtPhysicalCity.Text);
                //writer.WriteElementString("PhysicalState", txtPhysicalState.Text);
                //writer.WriteElementString("PhysicalZip", txtPhysicalZip.Text);

                //writer.WriteElementString("MailStreet", txtMailStreet.Text);
                //writer.WriteElementString("MailCity", txtMailCity.Text);
                //writer.WriteElementString("MailState", txtMailState.Text);
                //writer.WriteElementString("MailZip", txtMailZip.Text);

                //writer.WriteElementString("InsuranceStreet", txtInsuranceStreet.Text);
                //writer.WriteElementString("InsuranceCity", txtInsuranceCity.Text);
                //writer.WriteElementString("InsuranceState", txtInsuranceState.Text);
                //writer.WriteElementString("InsuranceZip", txtInsuranceZip.Text);

                // Demographic Changes
                writer.WriteElementString("BusinessClosed", rbBusinessClosedYes.Checked ? "Yes" : "No");
                writer.WriteElementString("NursingHome", rbNursingHomeYes.Checked ? "Yes" : "No");

                // Information from Medicount
                writer.WriteElementString("Portal", rbPortalYes.Checked ? "Yes" : "No");
                writer.WriteElementString("Alert", rbAlertYes.Checked ? "Yes" : "No");
                writer.WriteElementString("Training", rbTrainingYes.Checked ? "Yes" : "No");
                writer.WriteElementString("RunsReviewed", txtRunsReviewed.Text);
                writer.WriteElementString("RunsNotMet", txtRunsNotMet.Text);

                // Client Review Intervals
                string reviewInterval = "";
                if (rbIntervalQuarterly.Checked) reviewInterval = "Quarterly";
                else if (rbIntervalSemiAnnual.Checked) reviewInterval = "Semi-Annual";
                else if (rbIntervalYearly.Checked) reviewInterval = "Yearly";
                writer.WriteElementString("ReviewInterval", reviewInterval);
                writer.WriteElementString("NextReviewDate", txtNextReviewDate.Text);

                // ePCR
                //writer.WriteElementString("EpcrName", txtEpcrName.Text);
                writer.WriteElementString("Reconciliation", rbReconciliationYes.Checked ? "Yes" : "No");
                // writer.WriteElementString("ReconciliationDate", txtReconciliationDate.Text);

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
            txtNonTransportComment.Visible = show;

            // Optional to force update panel refresh
            upNonTransportComment.Update();
        }



        protected void ChangedChiefName(object sender, EventArgs e)
        {
            if (rbChiefCorrectNo.Checked)
            {

                //lblNewChiefName
                lblNewChiefName.Visible = false;
                txtChiefName.Visible = false;

                lblNewChiefEmail.Visible = false;
                txtChiefEmail.Visible = false;

                lblNewChiefPhoneNo.Visible = false;
                txtChiefPhone.Visible = false;

                lblCurrentChiefName.Visible = false;
                lblCurrentChiefEmail.Visible = false;
                lblCurrentChiefPhoneNo.Visible = false;
            }
            else
            {
                //lblNewChiefName
                lblNewChiefName.Visible = true;
                txtChiefName.Visible = true;

                lblNewChiefEmail.Visible = true;
                txtChiefEmail.Visible = true;

                lblNewChiefPhoneNo.Visible = true;
                txtChiefPhone.Visible = true;

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
                txtFiscalName.Visible = false;

                lblNewFiscalOfficerEmail.Visible = false;
                txtFiscalEmail.Visible = false;

                lblNewFiscalOfficerPhoneNo.Visible = false;
                txtFiscalPhone.Visible = false;


                lblCurrentFiscalOfficerName.Visible = false;
                lblCurrentFiscalOfficerEmail.Visible = false;
                lblCurrentFiscalOfficerPhoneNo.Visible = false;

            }
            else
            {
               
                lblNewFiscalOfficerName.Visible = true;
                txtFiscalName.Visible = true;

                lblNewFiscalOfficerEmail.Visible = true;
                txtFiscalEmail.Visible = true;

                lblNewFiscalOfficerPhoneNo.Visible = true;
                txtFiscalPhone.Visible = true;


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
                txtAuthorizedName.Visible = false;

                lblNewAuthosizedOfficalEmail.Visible = false;
                txtAuthorizedEmail.Visible = false;

                lblNewAuthosizedOfficalPhoneNo.Visible = false;
                txtAuthorizedPhone.Visible = false;


                lblCurrentAuthosizedOfficalName.Visible = false;
                lblCurrentAuthosizedOfficalEmail.Visible = false;
                lblCurrentAuthosizedOfficalPhoneNo.Visible = false;

            }
            else
            {
                //lblNewAuthosizedOfficalName
                lblNewAuthosizedOfficalName.Visible = true;
                txtAuthorizedName.Visible = true;

                lblNewAuthosizedOfficalEmail.Visible = true;
                txtAuthorizedEmail.Visible = true;

                lblNewAuthosizedOfficalPhoneNo.Visible = true;
                txtAuthorizedPhone.Visible = true;


                lblCurrentAuthosizedOfficalName.Visible = false;
                lblCurrentAuthosizedOfficalEmail.Visible = false;
                lblCurrentAuthosizedOfficalPhoneNo.Visible = false;
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
    }


}



