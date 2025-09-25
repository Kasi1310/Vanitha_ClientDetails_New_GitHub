using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using iText.Commons.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Windows;

public partial class CcmsToZohoCrm : System.Web.UI.Page
{
    string connectionString = ConfigurationManager.ConnectionStrings["CcmsToZohoConnectionString"].ToString();
    string expectedApiKey = ConfigurationManager.AppSettings["POSTCcmsToZohoCrmApiKey"].ToString();
    string zohoAuthUrl = ConfigurationManager.AppSettings["ZohoAuthenticationUrl"].ToString();
    string zohoApiUrl = ConfigurationManager.AppSettings["ZohoApiUrl"].ToString();

    string sServer;
    int sPort;
    string sUser;
    string sPass;

    // 32 bytes for AES-256, 16 bytes for AES-128 (Do not use special characters)
    private static readonly byte[] Key = Encoding.ASCII.GetBytes("Medicount-32-byte-long-key!!1234"); // 32 chars = 256-bit
    private static readonly byte[] IV = Encoding.ASCII.GetBytes("Medicount-pass-b"); // 16 chars = 128-bit

    readonly Dictionary<string, (string ZohoKey, string CcmsKey)> fieldMappings = new Dictionary<string, (string ZohoKey, string CcmsKey)>
                                                                        {
                                                                            //{<"Title Name">, (<"Zoho Data">, <"CCMS Data">) }
                                                                            { "Year 1 Fee", ("zoho_Year_1_Fee1", "ccms_Year1") },
                                                                            { "Year 2 Fee", ("zoho_Year_2_Fee1", "ccms_Year2") },
                                                                            { "Year 3 Fee", ("zoho_Year_3_Fee1", "ccms_Year3") },
                                                                            { "Year 4 Fee", ("zoho_Year_4_Fee1", "ccms_Year4") },
                                                                            // Add any new mappings here
                                                                        };

    protected void Page_Load(object sender, EventArgs e)
    {
        sServer = ConfigurationManager.AppSettings["SmtpServer"];
        sUser = ConfigurationManager.AppSettings["SmtpUsername"];
        sPass = ConfigurationManager.AppSettings["SmtpPassword"];

        string portStr = ConfigurationManager.AppSettings["SmtpPort"];
        sPort = 587; // default
        if (!int.TryParse(portStr, out sPort))
        {
            sPort = 587;
        }

        if (Request.HttpMethod == "POST")
        {
            try
            {
                var receivedApiKey = Request.Headers["apikey"];

                if (string.IsNullOrEmpty(receivedApiKey) || receivedApiKey != expectedApiKey)
                {
                    Response.StatusCode = 401;
                    Response.Write("Unauthorized - Invalid API Key");
                    return;
                }


                ZohoApiCredentials ZohoCred = new ZohoApiCredentials();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("[dbo].[GetZohoApiCredentials]", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        // cmd.Parameters.AddWithValue("@ZohoCrmId", data.ContactId); // Uncomment if needed

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ZohoCred.ClientId = reader["ClientId"]?.ToString();
                                ZohoCred.ClientSecret = reader["ClientSecret"]?.ToString();
                                ZohoCred.RefreshToken = reader["RefreshToken"]?.ToString();
                            }
                        }
                    }
                }

                // Read JSON payload
                string jsonPayload;
                using (StreamReader reader = new StreamReader(Request.InputStream))
                {
                    jsonPayload = reader.ReadToEnd();
                }

                JObject payload = JObject.Parse(jsonPayload);

                string ZohoId = payload["ZohoCrmId"]?.ToString();
                string CompanyId = payload["CompanyId"]?.ToString();

                // Get valid access token
                string accessToken = GetAccessTokenFromRefreshToken(ZohoCred);
                if (accessToken != "")
                {
                    // Search existing record by Account_Name
                    string searchUrl = $"{zohoApiUrl}/Accounts/search?criteria=(Account_Number:equals:{Uri.EscapeDataString(CompanyId)})"; //using Account number (i.e Company Id in CCMS)
                                                                                                                                           //string searchUrl = $"{zohoApiUrl}/Accounts/search?criteria=(Account_Number:equals:12345678"; //using Account number ----------------------------------Invalid Testing-----------------------


                    var searchResponse = MakeZohoApiRequest("GET", searchUrl, accessToken);

                    //Compare and build update payload with changed fields
                    Dictionary<string, string> changedFields = new Dictionary<string, string>();
                    string RequestType = "";
                    List<string> ccMails = new List<string>();

                    if ((searchResponse != null) && (searchResponse != "")) // Old = Zoho Data & New = CCMS Changed Data
                    {
                        JObject responseJson = JObject.Parse(searchResponse);
                        JArray records = (JArray)responseJson["data"];
                        if (records != null && records.Count > 0)
                        {
                            // Record exists → Update it
                            string recordId = records[0]["id"].ToString();
                            string accountType = records[0]["Account_Type"].ToString().ToUpper();
                            ccMails = records[0]["Owner"]["email"]?
                                                     .ToString()
                                                     .Split(';')
                                                     .Where(email => !string.IsNullOrWhiteSpace(email))
                                                     .Select(email => email.Trim())
                                                     .ToList();
                            if (accountType == "FORMER CLIENT" || accountType == "MERGE WITH ANOTHER CLIENT")
                            {
                                Response.StatusCode = 200;
                                Response.Write($"Account Number:{CompanyId} is already in {accountType}.Email not required");
                            }
                            else if ((accountType == "CUSTOMER") || (accountType == "CUSTOMER MBI PURCHASE"))
                            {
                                foreach (var kvp in fieldMappings)
                                {
                                    // Extract keys (Remove "zoho_" and "ccms_" prefix for Zoho field name)
                                    string zohoKey = kvp.Value.ZohoKey.StartsWith("zoho_") ? kvp.Value.ZohoKey.Substring("zoho_".Length) : kvp.Value.ZohoKey;
                                    string ccmsKey = kvp.Value.CcmsKey.StartsWith("ccms_") ? kvp.Value.CcmsKey.Substring("ccms_".Length) : kvp.Value.CcmsKey;


                                    // Skip if payload doesn't contain the CCMS key
                                    if (!payload.ContainsKey(ccmsKey))
                                        continue;

                                    // Get values from payload and records
                                    string payloadValue = payload[ccmsKey]?.ToString()?.Trim();
                                    string recordValue = records[0][zohoKey]?.ToString()?.Trim();

                                    bool valuesAreDifferent = true;

                                    // Try numeric comparison if both values can be parsed
                                    bool isPayloadDecimal = decimal.TryParse(payloadValue, out decimal payloadDecimal);
                                    bool isRecordDecimal = decimal.TryParse(recordValue, out decimal recordDecimal);

                                    if (isPayloadDecimal && isRecordDecimal)
                                    {
                                        valuesAreDifferent = payloadDecimal != recordDecimal;
                                    }
                                    else
                                    {
                                        // Fall back to case-insensitive string comparison
                                        valuesAreDifferent = !string.Equals(payloadValue, recordValue, StringComparison.OrdinalIgnoreCase);
                                    }

                                    if (valuesAreDifferent)
                                    {
                                        changedFields["zoho_" + zohoKey] = recordValue ?? "";
                                        changedFields["ccms_" + ccmsKey] = payloadValue ?? "";
                                        changedFields["old_ccms_" + ccmsKey] = payload["Old_" + ccmsKey]?.ToString();
                                    }
                                }


                                if (changedFields.Count > 0)
                                {
                                    var updateData = new
                                    {
                                        data = new[]
                                        {
                                changedFields
                            }
                                    };

                                    CompulsoryFields comFields = new CompulsoryFields();
                                    comFields.Id = payload["Id"]?.Value<int>() ?? 0;  // Default to 0 if null
                                    comFields.CompanyId = int.TryParse(payload["CompanyId"]?.ToString(), out int cid) ? cid : 0;
                                    comFields.ZohoCrmId = recordId;
                                    comFields.AccountName = payload["CompanyName"]?.ToString();
                                    comFields.RequestedBy = payload["RequestedBy"]?.ToString();
                                    RequestType = "UPDATE";

                                    SendEmailApproval(changedFields, comFields, RequestType, ccMails);
                                    Response.StatusCode = 200;
                                    Response.Write("Email Sent Successfully.");
                                }
                                else //No changes in records
                                {
                                    Response.StatusCode = 200;
                                    Response.Write("No Changes in records. Email not required");
                                }
                            }
                            else
                            {
                                Response.StatusCode = 200;
                                Response.Write($"Account Type:{accountType} is not need to change. Email not required");
                            }
                        }
                        else
                        {
                            Response.StatusCode = 200;
                            Response.Write("Error while converting zoho CRM records to json object.");
                        }
                    }
                    else // Data Not Found in Zoho CRM for given AccountNumber(AccountId or CompanyId) and also check with Former Client [Old = CCMS Old Data & New = CCMS Changed Data]
                    {
                        searchUrl = $"{zohoApiUrl}/Accounts/search?criteria=((Account_Type:equals:{Uri.EscapeDataString("Former Client")})and(Account_Number:equals:{Uri.EscapeDataString(CompanyId)}))&fields=id,Account_Number,Account_Name";
                        var formerClient = MakeZohoApiRequest("GET", searchUrl, accessToken);
                        string anotherSearchUrl = $"{zohoApiUrl}/Accounts/search?criteria=((Account_Type:equals:{Uri.EscapeDataString("Merge with Another Client")})and(Account_Number:equals:{Uri.EscapeDataString(CompanyId)}))&fields=id,Account_Number,Account_Name";
                        var mergedClient = MakeZohoApiRequest("GET", anotherSearchUrl, accessToken);
                        if (formerClient == "" && mergedClient == "")
                        {
                            foreach (var kvp in fieldMappings)
                            {
                                // Extract keys (Remove "zoho_" and "ccms_" prefix for Zoho field name)
                                string zohoKey = kvp.Value.ZohoKey.StartsWith("zoho_") ? kvp.Value.ZohoKey.Substring("zoho_".Length) : kvp.Value.ZohoKey;
                                string oldCcmsKey = "Old_" + (kvp.Value.CcmsKey.StartsWith("ccms_") ? kvp.Value.CcmsKey.Substring("ccms_".Length) : kvp.Value.CcmsKey);
                                string ccmsKey = kvp.Value.CcmsKey.StartsWith("ccms_") ? kvp.Value.CcmsKey.Substring("ccms_".Length) : kvp.Value.CcmsKey;


                                // Skip if payload doesn't contain the CCMS key
                                if (!payload.ContainsKey(ccmsKey))
                                    continue;

                                // Get values from payload and records
                                string payloadValue = payload[ccmsKey]?.ToString()?.Trim();

                                changedFields["zoho_" + zohoKey] = payloadValue ?? "";
                                changedFields["ccms_" + ccmsKey] = payloadValue ?? "";
                                changedFields["old_ccms_" + ccmsKey] = payload["Old_" + ccmsKey]?.ToString();
                            }

                            var updateData = new
                            {
                                data = new[]
                                        {
                                changedFields
                            }
                            };

                            CompulsoryFields comFields = new CompulsoryFields();
                            comFields.Id = payload["Id"]?.Value<int>() ?? 0;  // Default to 0 if null
                            comFields.CompanyId = int.TryParse(payload["CompanyId"]?.ToString(), out int cid) ? cid : 0;
                            comFields.ZohoCrmId = "";
                            comFields.AccountName = payload["CompanyName"]?.ToString();
                            comFields.RequestedBy = payload["RequestedBy"]?.ToString();
                            RequestType = "NEW";

                            SendEmailApproval(changedFields, comFields, RequestType, ccMails);

                            Response.StatusCode = 200;
                            Response.Write("Email Sent Successfully.");
                        }
                        else
                        {
                            Response.StatusCode = 200;
                            Response.Write($"Account Number:{CompanyId} is already in Zoho with another account type (Former Client / Merge with Another Client).Email not required");
                        }

                    }
                }
                else
                {
                    Response.StatusCode = 500;
                    Response.Write("Zoho CRM Access Token Error");
                }
                
            }
            catch (WebException webEx)
            {
                string errorMsg = new StreamReader(webEx.Response.GetResponseStream()).ReadToEnd();
                Response.StatusCode = 500;
                Response.Write($"WebException: {errorMsg}");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                Response.Write($"Exception: {ex.Message}");
            }
        }
        else if (Request.HttpMethod == "GET")
        {
            try
            {
                string token = HttpUtility.UrlDecode(Decrypt(Request.QueryString["token"]));
                string ClientDetailsId = HttpUtility.UrlDecode(Decrypt(Request.QueryString["uid"]));
                string responder = HttpUtility.UrlDecode(Decrypt(Request.QueryString["responder"]));
                string decision = HttpUtility.UrlDecode(Decrypt(Request.QueryString["decision"]));
                string requestType = HttpUtility.UrlDecode(Decrypt(Request.QueryString["requesttype"]));
                string emailStatus = decision == "approve" ? "APPROVED" : "REJECTED";
                bool isFirstApproval = true;
                string preResponder = "", preEmailStatus = "", modifiedOn = "";

                if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(responder) && !string.IsNullOrEmpty(decision) && !string.IsNullOrEmpty(ClientDetailsId) && !string.IsNullOrEmpty(requestType))
                {
                    // Check if user confirmed
                    string confirmed = Request.QueryString["confirmed"];

                    if (string.IsNullOrEmpty(confirmed))
                    {
                        // Step 1: No confirmation yet — show SweetAlert, no logic run
                        string script = $@"
                        <script src='https://cdn.jsdelivr.net/npm/sweetalert2@11'></script>
                        <script>
                        Swal.fire({{
                            title: 'Please Confirm',
                            text: 'Do you want to {decision} this request?',
                            icon: 'warning',
                            showCancelButton: true,
                            confirmButtonText: 'Confirm',
                            cancelButtonText: 'Cancel'
                        }}).then(function(result) {{
                            if (result.isConfirmed) {{
                                var url = new URL(window.location.href);
                                url.searchParams.set('confirmed', 'true');
                                window.location.href = url.toString();
                            }} else {{
                                // Replace body content with cancellation message
                                document.body.style.backgroundColor = '#f4f4f4'; // light gray background
                                document.body.innerHTML = `
                                    <div style='max-width: 600px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 8px; background-color: #ffffff; font-family: Arial, sans-serif;'>
                                        <div style='text-align: center; margin-bottom: 20px;'>
                                            <img src='Images/Logo.jpg' alt='Company Logo' style='max-height: 60px;' />
                                        </div>
                                        <h2 style='color:#d9534f; text-align: center;'>This operation was cancelled.</h2>
                                    </div>
                                `;
                            }}
                        }});
                        </script>";


                        ClientScript.RegisterStartupScript(this.GetType(), "ShowConfirm", script);
                        return;
                    }
                    else if (confirmed == "true")
                    {
                        Dictionary<string, string> changedFields = new Dictionary<string, string>();
                        string CreatedBy = "", ModifiedBy = "", AccountName = "", AccountId = "", ZohoRecordId = "", recordId = "", ZohoStatus = "", AccountType="";

                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            conn.Open();

                            SqlCommand cmd = new SqlCommand("[dbo].[IsFirstApproval]", conn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ClientDetailsId", ClientDetailsId);
                            cmd.Parameters.AddWithValue("@Token", token);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {


                                if (reader.Read())
                                {
                                    preEmailStatus = reader["EmailStatus"]?.ToString();
                                    preResponder = reader["Responder"]?.ToString();
                                    modifiedOn = reader["ModifiedOn"]?.ToString();
                                    isFirstApproval = preEmailStatus == "PENDING" || preEmailStatus == "PARTIAL SUCCESS";

                                }
                            }
                        }

                        if (isFirstApproval)
                        {
                            using (SqlConnection conn = new SqlConnection(connectionString))
                            {
                                conn.Open();

                                SqlCommand cmd = new SqlCommand("[dbo].[GetApprovedZohoData]", conn);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@ClientDetailsId", ClientDetailsId);
                                cmd.Parameters.AddWithValue("@Token", token);
                                cmd.Parameters.AddWithValue("@Responder", responder);
                                cmd.Parameters.AddWithValue("@EmailStatus", emailStatus);

                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {


                                    if (reader.Read())
                                    {
                                        // Ensure all address objects are initialized
                                        string json = reader["ChangesJson"]?.ToString();

                                        if (!string.IsNullOrEmpty(json))
                                        {
                                            changedFields = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                                        }
                                        recordId = reader["Id"]?.ToString();
                                        CreatedBy = reader["CreatedBy"]?.ToString();
                                        ModifiedBy = reader["ModifiedBy"]?.ToString();
                                        AccountName = reader["AccountName"]?.ToString();
                                        AccountId = reader["AccountId"]?.ToString();
                                        ZohoRecordId = reader["ZohoCrmRecordId"]?.ToString();
                                        AccountType = reader["AccountType"]?.ToString();

                                    }
                                }
                            }

                            bool zohoUpdateSuccess = true;

                            if (decision == "approve")
                            {
                                ZohoApiCredentials ZohoCred = new ZohoApiCredentials();

                                using (SqlConnection connection = new SqlConnection(connectionString))
                                {
                                    connection.Open();

                                    using (SqlCommand cmd = new SqlCommand("[dbo].[GetZohoApiCredentials]", connection))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        // cmd.Parameters.AddWithValue("@ZohoCrmId", data.ContactId); // Uncomment if needed

                                        using (SqlDataReader reader = cmd.ExecuteReader())
                                        {
                                            if (reader.Read())
                                            {
                                                ZohoCred.ClientId = reader["ClientId"]?.ToString();
                                                ZohoCred.ClientSecret = reader["ClientSecret"]?.ToString();
                                                ZohoCred.RefreshToken = reader["RefreshToken"]?.ToString();
                                            }
                                        }
                                    }
                                }


                                // Get valid access token
                                string accessToken = GetAccessTokenFromRefreshToken(ZohoCred);

                                if (accessToken != "")
                                {
                                    Dictionary<string, string> updateFields = new Dictionary<string, string>();

                                    foreach (var field in fieldMappings.Values)
                                    {
                                        string zohoKey = field.ZohoKey;   // e.g., "zoho_Year_1_Fee1"
                                        string ccmsKey = field.CcmsKey;   // e.g., "ccms_Year1"

                                        if (changedFields.TryGetValue(ccmsKey, out string ccmsValue))
                                        {
                                            // Remove "zoho_" prefix for Zoho field name
                                            string cleanedZohoField = zohoKey.StartsWith("zoho_") ? zohoKey.Substring("zoho_".Length) : zohoKey;

                                            updateFields[cleanedZohoField] = ccmsValue;
                                        }
                                    }
                                    try
                                    {
                                        if (requestType == "update")
                                        {
                                            var updateData = new
                                            {
                                                data = new[]
                                                    {
                                                  updateFields
                                                }
                                            };

                                            string updateUrl = $"{zohoApiUrl}/Accounts/{ZohoRecordId}";
                                            string updateJson = JsonConvert.SerializeObject(updateData);

                                            MakeZohoApiRequest("PUT", updateUrl, accessToken, updateJson);
                                            CmsUpdateZohoStatusById(connectionString, recordId, "SUCCESS");
                                        }
                                        else
                                        {
                                            updateFields["Account_Name"] = AccountName;
                                            updateFields["Account_Number"] = AccountId;
                                            updateFields["Account_Type"] = AccountType;

                                            var updateData = new
                                            {
                                                data = new[]
                                                {
                                                updateFields
                                            },
                                                trigger = new[] { "workflow" }
                                            };
                                            string updateUrl = $"{zohoApiUrl}/Accounts";

                                            string updateJson = JsonConvert.SerializeObject(updateData);
                                            MakeZohoApiRequest("POST", updateUrl, accessToken, updateJson);
                                            CmsUpdateZohoStatusById(connectionString, recordId, "SUCCESS");
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        CmsUpdateZohoStatusById(connectionString, recordId, "FAILED");
                                        Console.WriteLine(ex);
                                    }
                                }
                                else
                                {
                                    CmsUpdateZohoStatusById(connectionString, recordId, "ZOHO CRM ACCESS TOKEN ERROR");
                                    Console.WriteLine("Zoho CRM Access Token Error");
                                }
                                
                            }
                            else
                            {
                                CmsUpdateZohoStatusById(connectionString, recordId, "REJECTED");
                                Console.WriteLine("Rejected");
                            }

                            ZohoStatus = CmsGetZohoStatusById(connectionString, ClientDetailsId, token);

                            // Determine Swal settings based on decision
                            string swalIcon = "";
                            string swalTitle = "";
                            string headerColor = "";
                            string headerText = "";

                            if (!string.IsNullOrEmpty(decision))
                            {
                                if (decision.Equals("Approve", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (ZohoStatus == "SUCCESS")
                                    {
                                        swalIcon = "success";
                                        swalTitle = "✅ Approved!";
                                        headerColor = "#2E8B57"; // green
                                        headerText = "The Zoho CRM has been successfully updated with your changes.";
                                    }
                                    else if (ZohoStatus == "FAILED")
                                    {
                                        swalIcon = "error";
                                        swalTitle = "❗Failed";
                                        headerColor = "#d9534f"; // red
                                        headerText = "Failed to apply your changes to the Zoho CRM.";
                                    }
                                    else
                                    {
                                        swalIcon = "error";
                                        swalTitle = "❗Error";
                                        headerColor = "#d9534f"; // red
                                        headerText = "Error while findng the Zoho result";
                                    }
                                }
                                else if (decision.Equals("Reject", StringComparison.OrdinalIgnoreCase))
                                {
                                    swalIcon = "error";
                                    swalTitle = "❌ Rejected!";
                                    headerColor = "#d9534f";  // red
                                    headerText = "❌ Approval Response Rejected";
                                }
                            }

                            Response.Write($@"
                                <!DOCTYPE html>
                                <html>
                                <head>
                                    <title>Approval Response</title>
                                    <script src=""https://cdn.jsdelivr.net/npm/sweetalert2@11""></script>
                                </head>
                                <body style=""font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 30px;"">
                                    <script>
                                        Swal.fire({{
                                            title: '{swalTitle}',
                                            icon: '{swalIcon}',
                                            draggable: true
                                        }});
                                    </script>

                                    <div style='max-width: 600px; margin: auto; padding: 20px; border: 1px solid #ddd; border-radius: 8px; background-color: #ffffff;'>
                                        <div style='text-align: center; margin-bottom: 20px;'>
                                            <img src='Images/Logo.jpg' alt='Company Logo' style='max-height: 60px;' />
                                        </div>

                                        <h2 style='color: {headerColor}; text-align: center;'>{headerText}</h2>
                                        <hr style='border: none; border-top: 1px solid #ccc; margin: 20px 0;' />

                                        <table style='width: 100%; border-collapse: collapse; font-family: Arial, sans-serif;'>
                                            <tr>
                                                <td style='padding: 8px; font-weight: bold; width: 35%;'>Responder</td>
                                                <td style='padding: 8px;'>{responder}</td>
                                            </tr>
                                            <tr>
                                                <td style='padding: 8px; font-weight: bold;'>Decision</td>
                                                <td style='padding: 8px;'>{(decision?.ToLower() == "approve" ? "Approved" : "Rejected")}</td>
                                            </tr>
                                        </table>
                                    </div>
                                </body>
                                </html>
                                ");
                        }
                        else
                        {
                            ZohoStatus = CmsGetZohoStatusById(connectionString, ClientDetailsId, token);
                            string zohoStatus = ZohoStatus == "SUCCESS" ? "SUCCESS" : "FAILED";
                            string color = ZohoStatus == "SUCCESS" ? "#2E8B57" : "#d9534f";
                            string decColor = preEmailStatus == "APPROVED" ? "#2E8B57" : "#d9534f";

                            Response.Write($@"
                                <!DOCTYPE html>
                                <html>
                                <head>
                                    <title>Approval Status</title>
                                    <script src='https://cdn.jsdelivr.net/npm/sweetalert2@11'></script>
                                    <style>
                                        body {{
                                            font-family: Arial, sans-serif;
                                            background-color: #f4f4f4;
                                            padding: 30px;
                                        }}
                                        .container {{
                                            max-width: 600px;
                                            margin: auto;
                                            background: #ffffff;
                                            border: 1px solid #e0e0e0;
                                            border-radius: 8px;
                                            padding: 20px;
                                            box-shadow: 0 2px 6px rgba(0,0,0,0.1);
                                        }}
                                        .logo {{
                                            text-align: center;
                                            margin-bottom: 20px;
                                        }}
                                        .logo img {{
                                            max-height: 60px;
                                        }}
                                        h2 {{
                                            color: #d9534f;
                                            text-align: center;
                                        }}
                                        table {{
                                            width: 100%;
                                            border-collapse: collapse;
                                            margin-top: 20px;
                                        }}
                                        td {{
                                            padding: 8px 12px;
                                            border: 1px solid #ccc;
                                        }}
                                        td.label {{
                                            font-weight: bold;
                                            background-color: #ffffff;
                                            width: 40%;
                                        }}
                                    </style>
                                </head>
                                <body>
                                    <div class='container' id='response-container'>
                                        <div class='logo'>
                                            <img src='Images/Logo.jpg' alt='Company Logo' />
                                        </div>
                                        <h2>❗Approval Already Submitted</h2>

                                        <table>
                                            <tr>
                                                <td class='label'>Responder</td>
                                                <td>{preResponder}</td>
                                            </tr>
                                            <tr>
                                                <td class='label'>Decision</td>
                                                <td style='color:{decColor}'>{preEmailStatus}</td>
                                            </tr>
                                            <tr>
                                                <td class='label'>Zoho Update Status</td>
                                                <td style='color:{color}'>{ZohoStatus}</td>
                                            </tr>
                                            <tr>
                                                <td class='label'>Modified On</td>
                                                <td>{modifiedOn}</td>
                                            </tr>
                                        </table>
                                    </div>

                                    <script>
                                        document.addEventListener('DOMContentLoaded', function () {{
                                            Swal.fire({{
                                                title: 'Approval Already Submitted',
                                                icon: 'info',
                                                text: 'A response has already been recorded.',
                                                confirmButtonText: 'OK'
                                            }});
                                        }});
                                    </script>
                                </body>
                                </html>
                            ");
                        }
                    }
                    else
                    {
                        Response.Write("❌ Invalid.");
                    }

                }
                else
                {
                    Response.Write("❌ Missing one or more required query parameters.");
                }
            }
            catch (Exception ex)
            {
                Response.Write("❌ Missing one or more required query parameters.");
            }

        }
        else
        {
            Response.StatusCode = 405;
            Response.Write("Only POST and GET requests are allowed.");
        }
    }

    private string GetAccessTokenFromRefreshToken(ZohoApiCredentials zohoCred)
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

    private void SendEmailApproval(Dictionary<string, string> changedFields, CompulsoryFields comFields, string RequestType, List<string> ccMails)
    {
        string smtpHost = "";
        int smtpPort = 0;
        string smtpEmail = "";
        string smtpPassword = ""; // App Password

        //SMTP details from web config
        smtpHost = sServer;
        smtpPort = sPort;
        smtpEmail = sUser;
        smtpPassword = sPass;


        ////SMTP details will get from database
        //using (SqlConnection conn = new SqlConnection(connectionString))
        //{
        //    conn.Open();
        //    {
        //        SqlCommand cmd = new SqlCommand("[dbo].[GetSMTPDetails]", conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        using (SqlDataReader reader = cmd.ExecuteReader())
        //        {
        //            if (reader.Read())
        //            {
        //                smtpHost = reader["server"] != DBNull.Value ? reader["server"].ToString() : "";
        //                smtpPort = reader["port"] != DBNull.Value ? Convert.ToInt32(reader["port"]) : 0;
        //                smtpEmail = reader["username"] != DBNull.Value ? reader["username"].ToString() : "";
        //                smtpPassword = reader["password"] != DBNull.Value ? reader["password"].ToString() : "";

        //            }
        //        }
        //    }
        //}

        string action = "CMS TO ZOHO";
        List<string> recipients = new List<string>();
        List<string> bccMails = new List<string>();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            {
                SqlCommand cmd = new SqlCommand("[dbo].[GetEmailApprover]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))  // assuming email is in the first column
                        {
                            string email = reader.GetString(0).Trim();
                            string recipientType = reader.GetString(1).Trim();
                            if (recipientType == "TO")
                            {
                                recipients.Add(email);
                            }
                            else if (recipientType == "CC")
                            {
                                ccMails.Add(email);
                            }
                            else if (recipientType == "BCC")
                            {
                                bccMails.Add(email);
                            }
                        }
                    }
                }
            }

        }
        string token = Guid.NewGuid().ToString(); // Unique per recipient
        //string webhookLink = "https://snapshots.medicount.com/CcmsToZohoCrm.aspx";
        string webhookLink = HttpContext.Current.Request.Url.AbsoluteUri;
        var changesTable = new StringBuilder();
        var finalChangesTable = "";
        string warningContent = "";
        List<string> failedRecipients = new List<string>();
        List<string> successfulRecipients = new List<string>();


        if (recipients.Count != 0)
        {
            
            //Dictionary<string, (string ZohoKey, string CcmsKey)> fieldMappings = new Dictionary<string, (string ZohoKey, string CcmsKey)>
            //{
            //    //{<"Title Name">, (<"Zoho Data">, <"CCMS Data">) }
            //    { "Year 1 Fee", ("zoho_Year_1_Fee1", "ccms_Year1") },
            //    { "Year 2 Fee", ("zoho_Year_2_Fee1", "ccms_Year2") },
            //    { "Year 3 Fee", ("zoho_Year_3_Fee1", "ccms_Year3") },
            //    { "Year 4 Fee", ("zoho_Year_4_Fee1", "ccms_Year4") },
            //    // Add any new mappings here
            //};

            // Filter only mappings where at least one value exists in changedFields
            var activeFields = fieldMappings
                .Where(kvp =>
                    changedFields.ContainsKey(kvp.Value.ZohoKey) || changedFields.ContainsKey(kvp.Value.CcmsKey))
                .ToList();

            int columnSpan = activeFields.Count;

            string TableHeadingNow = "";
            if (RequestType == "UPDATE")
            {
                TableHeadingNow = "ZOHO FEE DETAILS";
            }
            else
            {
                TableHeadingNow = "CMS FEE DETAILS";
                warningContent = @"
                <p style='font-weight: bold; margin: 10px; padding: 10px; color: #dc3545; background-color: #f8d7da; border: 1px solid #f5c2c7; border-radius: 4px;text-align:center'>
                    ⚠️ Warning: Account Number is not available in the Zoho CRM.
                </p>";
            }

            
                changesTable.Append($@"
            <table width=""100%"" border=""1"" cellspacing=""0"" cellpadding=""5"" style=""text-align: center; border-collapse: collapse; font-size: 13px;"">
                <tr style=""background-color: teal; color: white; text-align: center;"">
                  <td colspan=""{columnSpan * 2 + 1}""><strong>FEE DETAILS</strong></td>
                </tr>
                <tr style=""text-align: center;"">
                  <td style=""background-color: #b9e3a4;"" colspan=""{columnSpan}""><strong>{TableHeadingNow} (NOW)</strong></td>
                  <td rowspan=""3""></td>
                  <td style=""background-color: #00949033;"" colspan=""{columnSpan}""><strong>ZOHO FEE DETAILS (CHANGE TO)</strong></td>
                </tr>
                <tr style=""background-color: #f2f2f2;"">
            ");

            foreach (var field in activeFields)
            {
                changesTable.Append($@"<td><strong>{field.Key}</strong></td>");
            }

            foreach (var field in activeFields)
            {
                changesTable.Append($@"<td><strong>{field.Key}</strong></td>");
            }

            changesTable.Append("</tr><tr>");

            // Old values (Zoho)
            foreach (var field in activeFields)
            {
                string value = changedFields.ContainsKey(field.Value.ZohoKey) ? changedFields[field.Value.ZohoKey] : "";
                changesTable.Append($@"<td>{value}</td>");
            }

            // New values (CCMS)
            foreach (var field in activeFields)
            {
                string value = changedFields.ContainsKey(field.Value.CcmsKey) ? changedFields[field.Value.CcmsKey] : "";
                changesTable.Append($@"<td style=""color:red;font-weight:bold;"">{value}</td>");
            }

            changesTable.Append("</tr></table>");

            finalChangesTable = changesTable.ToString();

            bool torecipientOver = false;
            recipients.Add("TO RECIPIENTS OVER");// Just to send the cc and bcc emails, this name is added
            foreach (string toEmail in recipients)
            {
                torecipientOver = toEmail == "TO RECIPIENTS OVER" ? true : false;
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
                            APPROVAL REQUEST ({action})
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
                                <td style=""font-weight: bold;"">{comFields.AccountName.ToUpper()}</td>
                              </tr>
                              <tr>
                                <td align=""right"" style=""color: gray; font-weight: bold;"">ACCOUNT NUMBER</td>
                                <td style=""font-weight: bold;"">{comFields.CompanyId}</td>
                              </tr>
                              <tr>
                                <td align=""right"" style=""color: gray; font-weight: bold;"">REQUESTED BY</td>
                                <td style=""font-weight: bold;"">{comFields.RequestedBy.ToUpper()}</td>
                              </tr>
                            </table>
                          </td>
                        </tr>

                        <!-- Instruction -->
                        <tr>
                          <td style=""padding: 10px;"">
                            {warningContent}
                            <p style=""font-weight: bold; margin: 0 0 10px;"">PLEASE REVIEW THE FOLLOWING CUSTOMER ACTION:</p>
                            {finalChangesTable}
                            
                          </td>
                        </tr>";
                if (!torecipientOver)
                {
                    htmlBody += $@"
                        <tr>
                          <td style=""padding: 10px;"">
                            <p style=""font-weight: bold;"">Click one of the options below to respond:</p>
                            <table cellpadding=""0"" cellspacing=""0"" border=""0"">
                              <tr>
                                <td style=""padding-right: 10px;"">
                                  <a href=""{webhookLink}?token={HttpUtility.UrlEncode(Encrypt(token))}&uid={HttpUtility.UrlEncode(Encrypt(comFields.Id.ToString()))}&responder={HttpUtility.UrlEncode(Encrypt(toEmail))}&decision={HttpUtility.UrlEncode(Encrypt("approve"))}&requesttype={HttpUtility.UrlEncode(Encrypt(RequestType.ToLower()))}""
                                     style=""display: inline-block; padding: 12px 20px; background-color: #28a745; color: #ffffff; text-decoration: none; border-radius: 4px;"">
                                    ✅ Approve
                                  </a>
                                </td>
                                <td>
                                  <a href=""{webhookLink}?token={HttpUtility.UrlEncode(Encrypt(token))}&uid={HttpUtility.UrlEncode(Encrypt(comFields.Id.ToString()))}&responder={HttpUtility.UrlEncode(Encrypt(toEmail))}&decision={HttpUtility.UrlEncode(Encrypt("reject"))}&requesttype={HttpUtility.UrlEncode(Encrypt(RequestType.ToLower()))}""
                                     style=""display: inline-block; padding: 12px 20px; background-color: #dc3545; color: #ffffff; text-decoration: none; border-radius: 4px;"">
                                    ❌ Reject
                                  </a>
                                </td>
                              </tr>
                            </table>
                          </td>
                        </tr>";
                }

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
                        Subject = torecipientOver ? $"Changes from ({action}) for Account: {comFields.AccountName} - {formattedDate}" : $"Approval Needed ({action}) for Account: {comFields.AccountName} - {formattedDate}",
                        Body = htmlBody,
                        IsBodyHtml = true
                    };
                    if (!torecipientOver)
                    {
                        mail.To.Add(toEmail);
                    }
                    else
                    {
                        // Add all CC emails (if any)
                        if (ccMails != null && ccMails.Count > 0)
                        {
                            foreach (var ccEmail in ccMails)
                            {
                                mail.CC.Add(ccEmail);
                            }
                        }

                        // Add all BCC emails (if any)
                        if (bccMails != null && bccMails.Count > 0)
                        {
                            foreach (var bccEmail in bccMails)
                            {
                                mail.Bcc.Add(bccEmail);
                            }
                        }
                    }

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
            SaveToEmailApprovalTable(changedFields, comFields,  token, emailStatus, errorMsg, allRecipients);

        }
        else // No Recepient(No Email Address)
        {
            //Console.WriteLine("No Email Address");
        }
    }

    private void SaveToEmailApprovalTable(Dictionary<string, string> changedFields, CompulsoryFields comFields, string token, string emailStatus, string errorMsg, string toEmail)
    {

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string changesJson = JsonConvert.SerializeObject(changedFields);

            // Use stored procedure name instead of raw SQL
            string storedProcedureName = "";
            storedProcedureName = "InsertCCMSEmailApprovalForZohoCRM";

            using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
            {
                // Indicate that this is a stored procedure, not a regular SQL query
                command.CommandType = System.Data.CommandType.StoredProcedure;

                // Add parameters for the stored procedure

                command.Parameters.AddWithValue("@ChangesJson", changesJson);
                command.Parameters.AddWithValue("@CreatedBy", comFields.RequestedBy ?? "");
                command.Parameters.AddWithValue("@AccountName", comFields.AccountName ?? "");
                command.Parameters.AddWithValue("@AccountId", comFields.CompanyId);
                command.Parameters.AddWithValue("@ClientDetailsId", comFields.Id);
                command.Parameters.AddWithValue("@ZohoCrmRecordId", comFields.ZohoCrmId);

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

    public static void CmsUpdateZohoStatusById(string connectionString, string recordId, string ZohoStatus)
    {
        try
        {


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("[dbo].[UpdateZohoStatus]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", recordId);
                    cmd.Parameters.AddWithValue("@ZohoStatus", ZohoStatus);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception EX)
        {
            Console.WriteLine("Error");
        }
    }

    public static string CmsGetZohoStatusById(string connectionString, string ClientDetailsId, string token)
    {
        string result = "";
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("[dbo].[GetZohoStatus]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ClientDetailsId", ClientDetailsId);
                    cmd.Parameters.AddWithValue("@Token", token);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = reader["ZohoStatus"]?.ToString();
                        }
                    }
                }
            }

            return result;
        }
        catch (Exception EX)
        {
            return "ERROR";
            Console.WriteLine("Error");
        }
    }
}

public class ZohoApiCredentials
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string RefreshToken { get; set; }
}

public class CompulsoryFields
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string ZohoCrmId { get;set;}
    public string AccountName { get; set; }
    public string RequestedBy { get; set; }
}
