using DocumentFormat.OpenXml;
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
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Web.Services;

namespace ClientDetails
{
    public partial class frmClientReview : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Nothing needed here for initial load
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



    }
}

public class FrmClientZohoApiCredentials
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string RefreshToken { get; set; }
}