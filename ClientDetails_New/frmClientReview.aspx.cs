using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
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
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.UI.WebControls;
using System.Xml;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace ClientDetails
{
    public partial class frmClientReview : System.Web.UI.Page
    {
        // 32 bytes for AES-256, 16 bytes for AES-128 (Do not use special characters)
        private static readonly byte[] Key = Encoding.ASCII.GetBytes("Medicount-32-byte-long-key!!1234"); // 32 chars = 256-bit
        private static readonly byte[] IV = Encoding.ASCII.GetBytes("Medicount-pass-b"); // 16 chars = 128-bit

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string token, reviewId, responder, requestType;

                switch (TryGetQueryParams(out token, out reviewId, out responder, out requestType))
                {
                    case QueryParamStatus.None:
                        LoadClientDropdown();
                        formType.Value = "NEW-0";
                        ClientScript.RegisterStartupScript(this.GetType(), "SetFormTypeNew", "<script>var formType='NEW';</script>");
                        break;

                    case QueryParamStatus.MissingParams:
                        ShowErrorMessage("❌ Some required parameters are missing in the link.");
                        break;

                    case QueryParamStatus.InvalidValues:
                        ShowErrorMessage("❌ The provided parameters are invalid or corrupted.");
                        break;

                    case QueryParamStatus.AllValid:
                        if (Request.QueryString["confirmed"] != "true")
                        {
                            ShowConfirmationPopup(); // Ask for confirmation before loading
                        }
                        else
                        {
                            string xmlData = LoadReviewXml(reviewId);

                            Dictionary<string, string> reviewData = new Dictionary<string, string>();

                            if (!string.IsNullOrEmpty(xmlData))
                            {
                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(xmlData);

                                XmlNode root = doc.DocumentElement;
                                foreach (XmlNode node in root.ChildNodes)
                                {
                                    if (node.NodeType == XmlNodeType.Element)
                                    {
                                        reviewData[node.Name] = node.InnerText;
                                    }
                                }
                                //reviewData["clientName"] = doc.SelectSingleNode("//clientName")?.InnerText ?? "";
                            }

                            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                            string jsonReviewData = jsSerializer.Serialize(reviewData);

                            string script = $@"
                        <script>
                            var reviewData = {jsonReviewData};
                            var formType = 'UPDATE-{reviewId}';
                        </script>";
                            formType.Value = $"UPDATE-{reviewId}";

                            string clientNumberValue = reviewData.ContainsKey("clientNumber") ? reviewData["clientNumber"] : "";
                            clientNumber.Items.Clear();
                            clientNumber.Items.Add(new ListItem(clientNumberValue, clientNumberValue));
                            clientNumber.SelectedValue = clientNumberValue;

                            ClientScript.RegisterStartupScript(this.GetType(), "LoadReviewData", script);
                        }
                        break;
                }

  
            }
        }

        private void ShowErrorMessage(string message)
        {
            Response.Write($@"
                <div style='padding:20px; max-width:600px; margin:50px auto; background:#fff3f3; border:1px solid #d9534f; border-radius:5px; color:#a94442; font-family:Arial;'>
                    <h3>Error</h3>
                    <p>{HttpUtility.HtmlEncode(message)}</p>
                </div>
            ");
        }

        private enum QueryParamStatus
        {
            None,
            AllValid,
            MissingParams,
            InvalidValues
        }

        private QueryParamStatus TryGetQueryParams(out string token, out string reviewId, out string responder, out string requestType)
        {
            token = reviewId = responder = requestType = null;

            string tokenEnc = Request.QueryString["token"];
            string reviewIdEnc = Request.QueryString["uid"];
            string responderEnc = Request.QueryString["responder"];
            string requestTypeEnc = Request.QueryString["requesttype"];

            bool anyProvided = !string.IsNullOrEmpty(tokenEnc) || !string.IsNullOrEmpty(reviewIdEnc) ||
                               !string.IsNullOrEmpty(responderEnc) || !string.IsNullOrEmpty(requestTypeEnc);

            bool allProvided = !string.IsNullOrEmpty(tokenEnc) && !string.IsNullOrEmpty(reviewIdEnc) &&
                               !string.IsNullOrEmpty(responderEnc) && !string.IsNullOrEmpty(requestTypeEnc);

            if (!anyProvided)
                return QueryParamStatus.None;

            if (!allProvided)
                return QueryParamStatus.MissingParams;

            try
            {
                token = HttpUtility.UrlDecode(Decrypt(tokenEnc));
                reviewId = HttpUtility.UrlDecode(Decrypt(reviewIdEnc));
                responder = HttpUtility.UrlDecode(Decrypt(responderEnc));
                requestType = HttpUtility.UrlDecode(Decrypt(requestTypeEnc));

                // Add any value-level validation here if needed
                if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(reviewId))
                    return QueryParamStatus.InvalidValues;

                return QueryParamStatus.AllValid;
            }
            catch
            {
                return QueryParamStatus.InvalidValues;
            }
        }


        private void ShowConfirmationPopup()
        {
            string script = @"
            <script src='https://cdn.jsdelivr.net/npm/sweetalert2@11'></script>
            <script>
                Swal.fire({
                    title: 'Please Confirm',
                    text: 'Do you want to proceed with this request?',
                    icon: 'warning',
                    showCancelButton: true,
                    confirmButtonText: 'Confirm',
                    cancelButtonText: 'Cancel'
                }).then(function(result) {
                    if (result.isConfirmed) {
                        var url = new URL(window.location.href);
                        url.searchParams.set('confirmed', 'true');
                        window.location.href = url.toString();
                    } else {
                        document.body.style.backgroundColor = '#f4f4f4';
                        document.body.innerHTML = `
                            <div style='max-width: 600px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 8px; background-color: #ffffff; font-family: Arial, sans-serif;'>
                                <div style='text-align: center; margin-bottom: 20px;'>
                                    <img src='Images/Logo.jpg' alt='Company Logo' style='max-height: 60px;' />
                                </div>
                                <h2 style='color:#d9534f; text-align: center;'>This operation was cancelled.</h2>
                            </div>
                        `;
                    }
                });
            </script>";

            ClientScript.RegisterStartupScript(this.GetType(), "ShowConfirm", script);
        }


        public class ClientItem
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }

        [WebMethod]
        public static List<ClientItem> GetClientNumbers()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["EsoToZohoConnectionString"].ToString();
            string companyCode = ""; // You can change this as needed

            List<ClientItem> clients = new List<ClientItem>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("[dbo].[spCRF_GetDetailsForClientReviewForm]", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CompanyCode", companyCode);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            string code = rdr["CompanyCode"].ToString();
                            clients.Add(new ClientItem { Text = code, Value = code });
                        }
                    }
                }
            }

            return clients;
        }

        private void LoadClientDropdown()
        {
            var clients = GetClientNumbers();

            clientNumber.DataSource = clients;
            clientNumber.DataTextField = "Text";
            clientNumber.DataValueField = "Value";
            clientNumber.DataBind();

            clientNumber.Items.Insert(0, new ListItem("SELECT", ""));
        }


        [WebMethod]
        public static object OnClientNumberChanged(string clientId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["EsoToZohoConnectionString"].ToString();
            var result = new Dictionary<string, string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("[dbo].[sp_GetClientInfoWithClientID]", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClientId", clientId);
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            result["ClientName"] = rdr["CompanyName"]?.ToString() ?? "";
                            result["AccountExecutive"] = rdr["AccountExecutive"]?.ToString() ?? "";
                            result["Email"] = rdr["AccountExecutiveEmailID"]?.ToString() ?? "";
                            result["Phone"] = rdr["AccountExecutivePhone"]?.ToString() ?? "";
                            result["RenewalDate"] = rdr["RenewalDate"] != DBNull.Value ? Convert.ToDateTime(rdr["RenewalDate"]).ToString("MM/dd/yyyy") : "";
                            result["FeeRate"] = rdr["FeeRate"] != DBNull.Value ? $"{Convert.ToDecimal(rdr["FeeRate"]):F2} %" : "0.00 %";
                        }
                    }
                }
            }

            // --- Zoho API integration ---
            FrmClientZohoApiCredentials ZohoCred = new FrmClientZohoApiCredentials();

            ZohoCred.ClientId = ConfigurationManager.AppSettings["ZohoClientId"].ToString();
            ZohoCred.ClientSecret = ConfigurationManager.AppSettings["ZohoClientSecret"].ToString();
            ZohoCred.RefreshToken = ConfigurationManager.AppSettings["ZohoRefreshToken"].ToString();
            string accessToken = GetAccessTokenFromRefreshToken(ZohoCred);

            if (!string.IsNullOrEmpty(accessToken))
            {
                string url = $"https://www.zohoapis.com/crm/v8/Accounts/search?criteria=(Account_Number:equals:{clientId})";
                string zohoData = MakeZohoApiRequest("GET", url, accessToken);

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
                                    result["currentChiefName"] = $"{ContactData["First_Name"]?.ToString()} {ContactData["Last_Name"]?.ToString()}";
                                    result["newChiefName"] = $"{ContactData["First_Name"]?.ToString()} {ContactData["Last_Name"]?.ToString()}";
                                    result["newChiefEmail"] = ContactData["Email"]?.ToString();
                                    result["newChiefPhone"] = ContactData["Phone"]?.ToString();
                                }

                                // FISCAL OFFICER
                                if (FiscalOfficerList.Select(fiscalOff => fiscalOff.Trim().ToUpper()).Contains(ContactData["Title"]?.ToString().Trim().ToUpper()))
                                {
                                    result["currentFiscalOfficer"] = $"{ContactData["First_Name"]?.ToString()} {ContactData["Last_Name"]?.ToString()}";
                                    result["newFiscalName"] = $"{ContactData["First_Name"]?.ToString()} {ContactData["Last_Name"]?.ToString()}";
                                    result["newFiscalEmail"] = ContactData["Email"]?.ToString();
                                    result["newFiscalPhone"] = ContactData["Phone"]?.ToString();
                                }

                                if (ContactData["Medicare_Authorized_Official"].ToString().ToUpper() == "TRUE")
                                {
                                    authorizedOfficialDict[$"Authorized Official {i}"] = new List<string>
                                    {
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
                                result["currentAuthorizedOfficial_1"] = authorizedOfficialDict["Authorized Official 1"][0];
                                result["newAuthorizedName_1"] = authorizedOfficialDict["Authorized Official 1"][1];
                                result["newAuthorizedEmail_1"] = authorizedOfficialDict["Authorized Official 1"][2];
                                result["newAuthorizedPhone_1"] = authorizedOfficialDict["Authorized Official 1"][3];
                                if (authorizedOfficialDict.Count == 2)
                                {
                                    // AUTHORIZED OFFICIAL-2
                                    result["currentAuthorizedOfficial_2"] = authorizedOfficialDict["Authorized Official 2"][0];
                                    result["newAuthorizedName_2"] = authorizedOfficialDict["Authorized Official 2"][1];
                                    result["newAuthorizedEmail_2"] = authorizedOfficialDict["Authorized Official 2"][2];
                                    result["newAuthorizedPhone_2"] = authorizedOfficialDict["Authorized Official 2"][3];
                                }
                            }

                        }
                        else
                        {
                            Console.WriteLine("No Contact Data");
                        }


                    result["MailStreet"] = contact["Mailing_Street"]?.ToString() ?? "";
                    result["MailCity"] = contact["Mailing_City"]?.ToString() ?? "";
                    result["MailState"] = contact["Mailing_State"]?.ToString() ?? "";
                    result["MailZip"] = contact["Mailing_Zip"]?.ToString() ?? "";

                    // You can continue adding Chief, Fiscal, and Authorized Officials as needed
                }
            }

            return result;
        }


        

        [WebMethod]
        public static Dictionary<string, string> GetClientReviewData(string companyID, string startDate, string endDate)
        {
            var result = new Dictionary<string, string>();

            string startDateFormatted = startDate.Replace("/", "-");
            string endDateFormatted = endDate.Replace("/", "-");

            string connectionString = ConfigurationManager.ConnectionStrings["EsoToZohoConnectionString"].ToString();

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
                            result["Transports"] = ((long)Convert.ToDouble(rdr["Runs_Prev"])).ToString("N0", new System.Globalization.CultureInfo("en-US"));
                            result["RevenuePerTransport"] = CleanedVersionOfValues(rdr["RPT_Prev"]);
                            result["Charges"] = CleanedVersionOfValues(rdr["Charges_Prev"]);
                            result["Payments"] = CleanedVersionOfValues(rdr["Payments_Prev"]);
                            result["Adjustments"] = CleanedVersionOfValues(rdr["Adjustments_Prev"]);
                            result["WriteOffs"] = CleanedVersionOfValues(rdr["WriteOffs_Prev"]);
                            //result["CollectionRate"] = rdr["Collection_Rate_Prev"] != DBNull.Value ? $"{Convert.ToDecimal(rdr["Collection_Rate_Prev"]):F2} %" : "0.00 %";
                            result["CollectionRate"] = CleanedVersionOfValues(rdr["Collection_Rate_Prev"], removeDecimal:false, type:"PERCENTAGE");

                            //result["StartDate"] = rdr["StartDate"] != DBNull.Value ? Convert.ToDateTime(rdr["StartDate"]).ToString("MM/dd/yyyy") : "";
                            //result["EndDate"] = rdr["EndDate"] != DBNull.Value ? Convert.ToDateTime(rdr["EndDate"]).ToString("MM/dd/yyyy") : "";

                            result["RunsReviewed"] = rdr["TotalRuns"].ToString();
                            result["RunsNotMet"] = rdr["RunsNotMet"].ToString();
                        }
                    }
                }
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("[dbo].[spCRF_GetDetailsForClientReviewForm]", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 240;
                    cmd.Parameters.AddWithValue("@CompanyCode", companyID);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            result["Bls"] = CleanedVersionOfValues(rdr["BLSE"]);
                            result["BlsNe"] = CleanedVersionOfValues(rdr["BLSNE"]);
                            result["Als"] = CleanedVersionOfValues(rdr["ALSE"]);
                            result["AlsNe"] = CleanedVersionOfValues(rdr["ALSNE"]);
                            result["Als2"] = CleanedVersionOfValues(rdr["ALS2"]);
                            result["Mileage"] = CleanedVersionOfValues(rdr["Ground_Mileage"]);

                            if (rdr["LastRateChange"] != DBNull.Value)
                            {
                                try
                                {
                                    DateTime lastRateChangeDate = (DateTime)rdr["LastRateChange"];
                                    result["DateOfLastRateChange"] = lastRateChangeDate.ToString("MM/dd/yyyy");
                                }
                                catch
                                {
                                    result["DateOfLastRateChange"] = "";
                                }
                            }
                            else
                            {
                                result["DateOfLastRateChange"] = "";
                            }

                            result["InsuranceStreet"] = rdr["InsPayToAddress"].ToString();
                            result["InsuranceCity"] = rdr["InsPayToCity"].ToString();
                            result["InsuranceState"] = rdr["InsPayToState"].ToString();
                            result["InsuranceZip"] = rdr["InsPayToZip"].ToString();

                            result["BillingStreet"] = rdr["PhysicalAddress"].ToString();
                            result["BillingCity"] = rdr["PhysicalCity"].ToString();
                            result["BillingState"] = rdr["PhysicalState"].ToString();
                            result["BillingZip"] = rdr["PhysicalZip"].ToString();
                        }
                    }
                }
            }

            return result;
        }

        [WebMethod]
        public static object SubmitReview(string xmlData)
        {
            try
            {
                int reviewId = SaveToDatabase(xmlData);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlData);

                string clientName = doc.SelectSingleNode("//clientName")?.InnerText;
                string clientNumber = doc.SelectSingleNode("//clientNumber")?.InnerText;
                string toRecipients = doc.SelectSingleNode("//aeEmail")?.InnerText;
                toRecipients = "sageit4@medicount.com";
                frmClientReview instance = new frmClientReview();
                instance.SendEmailApproval(reviewId, clientName, clientNumber, toRecipients, "NEW");
                return new { success = true, reviewId = reviewId };
            }
            catch (Exception ex)
            {
                return new { success = false, message = ex.Message };
            }
        }


        [WebMethod]
        public static string GeneratePdf(string formHtml)
        {
            string htmlPath = null;
            string pdfPath = null;

            try
            {
                htmlPath = SaveHtmlToTempFile(formHtml);
                byte[] pdfBytes = ConvertHtmlToPdf(htmlPath);
                pdfPath = Path.ChangeExtension(htmlPath, ".pdf");

                return Convert.ToBase64String(pdfBytes);
            }
            finally
            {
                if (!string.IsNullOrEmpty(htmlPath) && File.Exists(htmlPath))
                    File.Delete(htmlPath);

                if (!string.IsNullOrEmpty(pdfPath) && File.Exists(pdfPath))
                    File.Delete(pdfPath);
            }
        }


        private static int SaveToDatabase(string xmlData)
        {
            int reviewId = 0;
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp_InsertClientReview", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    SqlParameter xmlParam = new SqlParameter("@XmlData", SqlDbType.Xml)
                    {
                        Value = xmlData
                    };
                    command.Parameters.Add(xmlParam);

                    SqlParameter idParam = new SqlParameter("@ReviewID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(idParam);

                    connection.Open();
                    command.ExecuteNonQuery();
                    reviewId = Convert.ToInt32(idParam.Value);
                }
            }

            return reviewId;
        }

        private static string BuildHtmlFromXml(string xmlData)
        {
            // Very simple placeholder; you should construct real HTML form layout
            var sb = new StringBuilder();
            sb.Append("<html><body><h1>Client Review</h1>");
            sb.Append("<pre>").Append(HttpUtility.HtmlEncode(xmlData)).Append("</pre>");
            sb.Append("</body></html>");
            return sb.ToString();
        }

        private static string SaveHtmlToTempFile(string html)
        {
            string folder = HttpContext.Current.Server.MapPath("~/Temp/");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            string fileName = "ClientReview_" + Guid.NewGuid() + ".html";
            string path = Path.Combine(folder, fileName);
            File.WriteAllText(path, html, Encoding.UTF8);
            return path;
        }

        private static byte[] ConvertHtmlToPdf(string htmlFilePath)
        {
            string wkhtmlPath = @"C:\Program Files\wkhtmltopdf\bin\wkhtmltopdf.exe"; // Adjust path if needed

            string inputFile = "file:///" + htmlFilePath.Replace("\\", "/");
            string outputPdf = Path.ChangeExtension(htmlFilePath, ".pdf");

            var startInfo = new ProcessStartInfo
            {
                FileName = wkhtmlPath,
                Arguments = $"--enable-local-file-access --viewport-size 1280x1024 --zoom 1.0 --page-size A4 --print-media-type \"{inputFile}\" \"{outputPdf}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (var proc = Process.Start(startInfo))
            {
                string err = proc.StandardError.ReadToEnd();
                proc.WaitForExit();

                if (proc.ExitCode != 0)
                    throw new Exception("wkhtmltopdf failed: " + err);
            }

            return File.ReadAllBytes(outputPdf);
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
                    strValue = result.ToString("C2", CultureInfo.CreateSpecificCulture("en-US"));
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

        private static string GetAccessTokenFromRefreshToken(FrmClientZohoApiCredentials zohoCred)
        {
            try
            {
                string zohoAuthUrl = ConfigurationManager.AppSettings["ZohoAuthenticationUrl"].ToString();
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

        private static string MakeZohoApiRequest(string method, string url, string accessToken, string jsonPayload = null)
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

        private string LoadReviewXml(string reviewId)
        {
            // Replace this with actual DB fetch logic based on your table schema
            string xml = "";
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT XmlData FROM ClientReviews WHERE ReviewId = @ReviewId", conn))
                {
                    cmd.Parameters.AddWithValue("@ReviewId", reviewId);
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                        xml = result.ToString();
                }
            }

            return xml;
        }


        private void SendEmailApproval(int reviewId, string accountName, string accountNumber, string toRecipients, string RequestType)
        {
            string smtpHost = ConfigurationManager.AppSettings["SmtpServer"];
            int smtpPort = 0;
            string portStr = ConfigurationManager.AppSettings["SmtpPort"];
            if (!int.TryParse(portStr, out smtpPort))
            {
                smtpPort = 587;
            }
            string smtpEmail = ConfigurationManager.AppSettings["SmtpUsername"];
            string smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];

            string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;

            
            List<string> recipients = new List<string>();
            List<string> bccMails = new List<string>();
            List<string> ccMails = new List<string>();
            recipients.Add(toRecipients);
            //using (SqlConnection conn = new SqlConnection(connectionString))
            //{
            //    conn.Open();
            //    {
            //        SqlCommand cmd = new SqlCommand("[dbo].[spZoho_GetEmailApprover]", conn);
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.AddWithValue("@Category", Category.ToUpper());

            //        using (SqlDataReader reader = cmd.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                if (!reader.IsDBNull(0))  // assuming email is in the first column
            //                {
            //                    string email = reader.GetString(0).Trim();
            //                    string recipientType = reader.GetString(1).Trim();
            //                    string category = reader.GetString(2).Trim();
            //                    if (recipientType == "TO")
            //                    {
            //                        recipients.Add(email);
            //                    }
            //                    else if (recipientType == "CC")
            //                    {
            //                        ccMails.Add(email);
            //                    }
            //                    else if (recipientType == "BCC")
            //                    {
            //                        bccMails.Add(email);
            //                    }
            //                }
            //            }
            //        }
            //    }

            //}

            string token = Guid.NewGuid().ToString(); // Unique per recipient
            Uri currentUri = HttpContext.Current.Request.Url;

            string[] segments = currentUri.Segments;
            int aspxIndex = Array.FindLastIndex(segments, s => s.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase));
            string basePath = string.Join("", segments.Take(aspxIndex + 1));
            string baseUrl = $"{currentUri.Scheme}://{currentUri.Authority}{basePath}?";


            List<string> failedRecipients = new List<string>();
            List<string> successfulRecipients = new List<string>();


            if (recipients.Count != 0)
            {
                foreach (string toEmail in recipients)
                {
                    // Create HTML content with personalized approval/reject links
                    string htmlBody = $@"
            <html>
              <body style=""margin: 0; padding: 0; font-family: Arial, sans-serif; font-size: 14px;"">
                <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background-color: #ffffff;"">
                  <tr>
                    <td align=""center"">
                      <!-- Wrapper Table with Max Width -->
                      <table width=""800"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""width: 980px; border: solid 1px teal; border-radius: 5px; max-width: auto; margin: 0 auto;"">

                        <!-- Header Section -->
                        <tr>
                          <td align=""center"" style=""padding: 10px;"">
                            <img src=""https://snapshots.medicount.com/Images/medicount_mail_logo_new.jpg"" alt=""Medicount Logo"" height=""50"" style=""display: block;"">
                          </td>
                        </tr>
                        <tr>
                          <td align=""center"" style=""font-size: 18px; font-weight: bold; padding-bottom: 10px;"">
                           HEADING TEST
                          </td>
                        </tr>
                        <tr>
                          <td align=""center"" style=""padding-bottom: 10px;"">
                            <span style=""background-color: #f5f5f5; padding: 5px 10px; border-radius: 4px; font-size: 14px; color: #333333;font-weight:bold"">
                              {DateTime.Now.ToString("MMM-dd-yyyy | hh:mm tt")}
                            </span>
                          </td>
                        </tr>

                        <!-- Account Details -->
                        <tr>
                          <td align=""center"">
                            <table cellpadding=""5"" cellspacing=""0"" border=""0"" style=""margin: 0 auto; text-align: left;"">
                              <tr>
                                <td align=""right"" style=""color: gray; font-weight: bold;"">ACCOUNT NAME</td>
                                <td style=""font-weight: bold;"">{accountName.ToUpper()}</td>
                              </tr>
                              <tr>
                                <td align=""right"" style=""color: gray; font-weight: bold;"">ACCOUNT NUMBER</td>
                                <td style=""font-weight: bold;"">{accountNumber}</td>
                              </tr>
                            </table>
                          </td>
                        </tr>

                        <!-- Instruction -->
                        <tr>
                          <td style=""padding: 10px;"">
                            <p style=""font-weight: bold; margin: 0 0 10px;"">Please click on the link below:</p>
                            <a href=""{baseUrl}&token={HttpUtility.UrlEncode(Encrypt(token))}&uid={HttpUtility.UrlEncode(Encrypt(reviewId.ToString()))}&responder={HttpUtility.UrlEncode(Encrypt(toEmail))}&requesttype={HttpUtility.UrlEncode(Encrypt(RequestType.ToLower()))}""
                                     style=""display: inline-block; padding: 12px 20px; background-color: #28a745; color: #ffffff; text-decoration: none; border-radius: 4px;"">
                                CLICK HERE                            
                            </a>
                          </td>
                        </tr>";



                    htmlBody += $@"       
                        <!-- Footer -->
                        <tr>
                            <td style=""padding: 10px;"">
                            <p style=""margin: 0 0 10px;"">If you did not request this action, you can safely ignore this email.</p>
                            <table border=""0"" cellspacing=""0"" cellpadding=""0"">
                                <tbody>
                                    <tr>
                                        <td>Thanks,</td>
                                    </tr>
                                    <tr>
                                        <td style=""font-weight:bold"">Medicount Management, Inc.</td>
                                    </tr>
                                </tbody>
                            </table>
                            </td>
                        </tr>

                      </table>
                    </td>
                  </tr>
                </table>
              </body>
            </html>
            ";

                    try
                    {
                        string formattedDate = DateTime.Now.ToString("MM/dd/yyyy (hh:mm:ss tt)", CultureInfo.InvariantCulture);
                        MailMessage mail = new MailMessage
                        {
                            From = new MailAddress(smtpEmail),
                            Subject = $"Client Review Form for Account: {accountName} - {formattedDate}",
                            Body = htmlBody,
                            IsBodyHtml = true
                        };

                        mail.To.Add(toEmail);

                        //// Add all CC emails (if any)
                        //if (ccMails != null && ccMails.Count > 0)
                        //{
                        //    foreach (var ccEmail in ccMails)
                        //    {
                        //        mail.CC.Add(ccEmail);
                        //    }
                        //}

                        //// Add all BCC emails (if any)
                        //if (bccMails != null && bccMails.Count > 0)
                        //{
                        //    foreach (var bccEmail in bccMails)
                        //    {
                        //        mail.Bcc.Add(bccEmail);
                        //    }
                        //}


                        using (SmtpClient smtp = new SmtpClient(smtpHost, smtpPort))
                        {
                            smtp.Credentials = new NetworkCredential(smtpEmail, smtpPassword);
                            smtp.EnableSsl = true;
                            smtp.Send(mail);
                        }

                        Console.WriteLine($"✅ Email sent to {toEmail}");
                        successfulRecipients.Add(toEmail);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Failed to send email to {toEmail}: {ex.Message}");
                        failedRecipients.Add($"{toEmail} - {ex.Message}");
                    }
                }

                // Save summary once after all emails
                string emailStatus = failedRecipients.Count > 0
                    ? (successfulRecipients.Count > 0 ? "PARTIAL SUCCESS" : "ERROR")
                    : "PENDING";

                string errorMsg = string.Join("; ", failedRecipients);
                string allRecipients = string.Join(",", recipients);
                if (recipients.Count > 0) allRecipients += ",";

                allRecipients += string.Join(",", ccMails);
                if (ccMails.Count > 0) allRecipients += ",";

                allRecipients += string.Join(",", bccMails);
                allRecipients = allRecipients.Replace("TO RECIPIENTS OVER", "").Trim(',').Trim();
                // Save approval request record per recipient
                SaveToEmailApprovalTable(reviewId,token, emailStatus, errorMsg, allRecipients);

            }
            else // No Recepient(No Email Address)
            {
                //Console.WriteLine("No Email Address");
            }
        }

        private void SaveToEmailApprovalTable(int reviewId, string token, string emailStatus, string errorMsg, string toEmail)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Use stored procedure name instead of raw SQL
                string storedProcedureName = "";
                storedProcedureName = "spCRF_UpdateEmailStatus";

                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    // Indicate that this is a stored procedure, not a regular SQL query
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Add parameters for the stored procedure
                    command.Parameters.AddWithValue("@ReviewId", reviewId);
                    command.Parameters.AddWithValue("@Token", token);
                    command.Parameters.AddWithValue("@EmailStatus", emailStatus);
                    command.Parameters.AddWithValue("@ErrorMsg", errorMsg);
                    command.Parameters.AddWithValue("@EmailReceiver", toEmail);

                    // Execute the stored procedure
                    command.ExecuteNonQuery();
                }

            }
        }


        public static string Encrypt(string plainText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (StreamWriter sw = new StreamWriter(cs))
                {
                    sw.Write(plainText);
                    sw.Close();
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public static string Decrypt(string cipherText)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = Key;
                aes.IV = IV;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                byte[] buffer = Convert.FromBase64String(cipherText);

                using (MemoryStream ms = new MemoryStream(buffer))
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (StreamReader sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
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