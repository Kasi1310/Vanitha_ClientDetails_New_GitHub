using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using iText.Commons.Utils;
using iText.Html2pdf.Attach;
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
using System.util.zlib;
using System.Web;
using System.Web.UI;
using System.Windows;

public partial class EsoToZohoCrm : System.Web.UI.Page
{
    string connectionString = ConfigurationManager.ConnectionStrings["EsoToZohoConnectionString"].ToString();
    string expectedApiKey = ConfigurationManager.AppSettings["POSTEsoToZohoCrmApiKey"].ToString();
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
                                                                            //{<"Title Name">, (<"Zoho Data">, <"ESO Data">) }
                                                                            { "BLS Rate", ("zoho_BLS_Rate", "eso_BlsRate") },
                                                                            { "ALS 1 Rate", ("zoho_ALS_1_Rate", "eso_AlsRate1") },
                                                                            { "ALS 2 Rate", ("zoho_ALS_2_Rate", "eso_AlsRate2") },
                                                                            { "Mileage Rate", ("zoho_Mileage_Rate", "eso_Mileage") },
                                                                            { "Non ER BLS Rate", ("zoho_Non_ER_BLS_Rate", "eso_BlsNE") },
                                                                            { "Non ER ALS Rate", ("zoho_Non_ER_ALS_Rate", "eso_AlsNE") },
                                                                            { "Non ER Mileage", ("zoho_Non_ER_Mileage", "eso_MileageNE") },
                                                                            { "SCT", ("zoho_SCT", "eso_Sct") },
                                                                            { "Insu Pay to Street", ("zoho_Insu_Pay_to_Street", "eso_InsPayToAddress") },
                                                                            { "Insu Pay to City", ("zoho_Insu_Pay_to_City", "eso_InsPayToCity") },
                                                                            { "Insu Pay to State", ("zoho_Insu_Pay_to_State", "eso_InsPayToState") },
                                                                            { "Insu Pay to Zip", ("zoho_Insu_Pay_to_Zip", "eso_InsPayToZip") },
                                                                            { "Physical Address", ("zoho_Physical_Address", "eso_PhysicalAddress") },
                                                                            { "Physical Location City", ("zoho_Physical_Location_City", "eso_PhysicalCity") },
                                                                            { "Physical Location State", ("zoho_Physical_Location_State", "eso_PhysicalState") },
                                                                            { "Physical Location Zip", ("zoho_Physical_Location_Zip", "eso_PhysicalZip") },
                                                                            { "Billing Street", ("zoho_Billing_Street", "eso_BillingStreet") },
                                                                            { "Billing City", ("zoho_Billing_City", "eso_BillingCity") },
                                                                            { "Billing State", ("zoho_Billing_State", "eso_BillingState") },
                                                                            { "Billing Zip", ("zoho_Billing_Code", "eso_BillingZip") },
                                                                            
                                                                            // Add any new mappings here
                                                                        };

    public static Dictionary<string, List<(string zohoKey, string esoKey)>> addressFieldsMapping = new Dictionary<string, List<(string zohoKey, string esoKey)>>
    {
        { "Insurance Pay to Address", new List<(string zohoKey, string esoKey)>
            {
                ("zoho_Insu_Pay_to_Street", "eso_InsPayToAddress"),
                ("zoho_Insu_Pay_to_City", "eso_InsPayToCity"),
                ("zoho_Insu_Pay_to_State", "eso_InsPayToState"),
                ("zoho_Insu_Pay_to_Zip", "eso_InsPayToZip")
            }
        },

        { "Physical Address", new List<(string zohoKey, string esoKey)>
            {
                ("zoho_Physical_Address", "eso_PhysicalAddress"),
                ("zoho_Physical_Location_City", "eso_PhysicalCity"),
                ("zoho_Physical_Location_State", "eso_PhysicalState"),
                ("zoho_Physical_Location_Zip", "eso_PhysicalZip")
            }
        },

        { "Billing Address", new List<(string zohoKey, string esoKey)>
            {
                ("zoho_Billing_Street", "eso_BillingStreet"),
                ("zoho_Billing_City", "eso_BillingCity"),
                ("zoho_Billing_State", "eso_BillingState"),
                ("zoho_Billing_Code", "eso_BillingZip")
            }
        }
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

                string encodedMethod = Request.QueryString["method"];
                string decodedMethod;

                if (string.IsNullOrEmpty(encodedMethod))
                {
                    decodedMethod = "POST";  
                }
                else
                {
                    try
                    {
                        byte[] base64Bytes = Convert.FromBase64String(encodedMethod);
                        decodedMethod = Encoding.UTF8.GetString(base64Bytes);
                    }
                    catch (FormatException)
                    {
                        Response.StatusCode = 400;
                        Response.Write("Invalid Base64 in 'method' Query parameter");
                        return;
                    }
                }


                // Optional: Check the decoded method
                if (decodedMethod == "PUT")
                {
                    Response.StatusCode = 200;
                    Response.Write($"OK");
                    return;
                }
                else // method = POST
                {
                    EsoZohoApiCredentials ZohoCred = new EsoZohoApiCredentials();

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();

                        using (SqlCommand cmd = new SqlCommand("[dbo].[spZoho_GetZohoApiCredentials]", connection))
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

                    string jsonPayload;
                    using (StreamReader reader = new StreamReader(Request.InputStream))
                    {
                        jsonPayload = reader.ReadToEnd();
                    }

                    JObject payload = JObject.Parse(jsonPayload);
                    string RequestType = "";

                    EsoCompulsoryFields comFields = new EsoCompulsoryFields();
                    comFields.Id = payload["CompanyId"]?.Value<int>() ?? 0;  // Default to 0 if null
                    comFields.CompanyId = int.TryParse(payload["CompanyId"]?.ToString(), out int cid) ? cid : 0;
                    comFields.ZohoCrmId = payload["ZohoCrmId"]?.ToString(); ;
                    comFields.AccountName = payload["CompanyName"]?.ToString();
                    comFields.RequestedBy = payload["RequestedBy"]?.ToString();
                    RequestType = payload["RequestType"]?.ToString();

                    // 4. Define keys to exclude from email
                    var excludedKeys = new HashSet<string> { "CompanyId", "CompanyName", "RequestedBy", "ZohoCrmId", "RequestType" };

                    // 5. Create a dictionary<string, string> from payload excluding above keys
                    var changedFields = payload
                        .Properties()
                        .Where(p => !excludedKeys.Contains(p.Name))
                        .ToDictionary(p => p.Name, p => p.Value?.ToString());

                    //********************************** TO DELETE BELOW LINE FOR LIVE************************
                    //DirectUpdateToZohoCRM(ZohoCred, changedFields, recordId);
                    SendEmailApproval(changedFields, comFields, RequestType); // Uncomment this line in live(**Mail approval function**)
                    Response.StatusCode = 200;
                    Response.Write($"Success: Email Sent Successfully");

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

                            SqlCommand cmd = new SqlCommand("[dbo].[spZoho_IsFirstApproval]", conn);
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

                                SqlCommand cmd = new SqlCommand("[dbo].[spZoho_GetApprovedZohoData]", conn);
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


                            if (decision == "approve")
                            {
                                EsoZohoApiCredentials ZohoCred = new EsoZohoApiCredentials();

                                using (SqlConnection connection = new SqlConnection(connectionString))
                                {
                                    connection.Open();

                                    using (SqlCommand cmd = new SqlCommand("[dbo].[spZoho_GetZohoApiCredentials]", connection))
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


                                Dictionary<string, string> updateFields = new Dictionary<string, string>();

                                foreach (var field in fieldMappings.Values)
                                {
                                    string zohoKey = field.ZohoKey;   // e.g., "zoho_Year_1_Fee1"
                                    string ccmsKey = field.CcmsKey;   // e.g., "eso_Year1"

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
                                        UpdateZohoStatusById(connectionString, recordId, "SUCCESS");
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
                                        UpdateZohoStatusById(connectionString, recordId, "SUCCESS");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    UpdateZohoStatusById(connectionString, recordId, "FAILED");
                                    Console.WriteLine(ex);
                                }

                            }
                            else
                            {
                                UpdateZohoStatusById(connectionString, recordId, "REJECTED");
                                Console.WriteLine("Rejected");
                            }

                            ZohoStatus = GetZohoStatusById(connectionString, ClientDetailsId, token);


                            // Determine Swal settings based on decision
                            string swalIcon = "";
                            string swalTitle = "";
                            string headerColor = "";
                            string headerText =  "";
                            

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
                            ZohoStatus = GetZohoStatusById(connectionString, ClientDetailsId, token);
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
            Response.Write("POST, PUT, GET requests are allowed.");
        }
    }

    private string GetAccessTokenFromRefreshToken(EsoZohoApiCredentials zohoCred)
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

    private void SendEmailApproval(Dictionary<string, string> changedFields, EsoCompulsoryFields comFields, string RequestType)
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

        ////SMTP details from database
        //using (SqlConnection conn = new SqlConnection(connectionString))
        //{
        //    conn.Open();
        //    {
        //        SqlCommand cmd = new SqlCommand("[dbo].[spZoho_GetSMTPDetails]", conn);
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

        string action = "ESO TO ZOHO";
        List<string> recipients = new List<string>();
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            {
                SqlCommand cmd = new SqlCommand("[dbo].[spZoho_GetEmailApprover]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))  // assuming email is in the first column
                        {
                            string email = reader.GetString(0).Trim();
                            recipients.Add(email);
                        }
                    }
                }
            }

        }
        string token = Guid.NewGuid().ToString(); // Unique per recipient
        //string webhookLink = "https://snapshots.medicount.com/CcmsToZohoCrm.aspx";
        string webhookLink = HttpContext.Current.Request.Url.AbsoluteUri;
        var changesTable = new StringBuilder();
        var addressTable = new StringBuilder();
        var addressRows = new StringBuilder();
        var finalChangesTable = "";
        var finalAddressTable = "";
        string warningContent = "";
        bool hasAnyAddressChange = false;
        List<string> failedRecipients = new List<string>();
        List<string> successfulRecipients = new List<string>();


        if (recipients.Count != 0)
        {

            // Extract all address-related fields dynamically from addressFieldsMapping
            var addressFields = addressFieldsMapping
                .SelectMany(mapping => mapping.Value)  // Flatten the list of address key pairs
                .Select(pair => pair.zohoKey)          // Get the Zoho keys for address fields
                .Concat(addressFieldsMapping
                    .SelectMany(mapping => mapping.Value)
                    .Select(pair => pair.esoKey))      // Get the ESO keys for address fields
                .ToList();                            // Collect them into a list

            // Separate active non-address fields
            var activeFields = fieldMappings
                .Where(kvp =>
                    // Filter out address fields and check if they are in changedFields
                    !addressFields.Contains(kvp.Value.ZohoKey) &&
                    !addressFields.Contains(kvp.Value.CcmsKey) &&
                    (changedFields.ContainsKey(kvp.Value.ZohoKey) || changedFields.ContainsKey(kvp.Value.CcmsKey)))
                .ToList();

            int columnSpan = activeFields.Count;

            if (RequestType == "UPDATE")
            {
                if (activeFields.Count > 0)  // Render CHARGE RATES table only if changes exist
                {
                    changesTable.Append($@"
<table width='100%' border='1' cellspacing='0' cellpadding='5' style='text-align: center; border-collapse: collapse; font-size: 13px;'>
    <tr style='background-color: teal; color: white; text-align: center;'>
        <td colspan='{columnSpan * 2 + 1}'><strong>CHARGE RATES</strong></td>
    </tr>
    <tr style='text-align: center;'>
        <td style='background-color: #b9e3a4;' colspan='{columnSpan}'><strong>ZOHO (NOW)</strong></td>
        <td rowspan='3'></td>
        <td style='background-color: #00949033;' colspan='{columnSpan}'><strong>ZOHO (CHANGE TO)</strong></td>
    </tr>
    <tr style='background-color: #f2f2f2;'>");

                    foreach (var field in activeFields)
                        changesTable.Append($@"<td><strong>{field.Key}</strong></td>");
                    foreach (var field in activeFields)
                        changesTable.Append($@"<td><strong>{field.Key}</strong></td>");

                    changesTable.Append("</tr><tr>");

                    // Old values
                    foreach (var field in activeFields)
                    {
                        string value = changedFields.ContainsKey(field.Value.ZohoKey)
                            ? changedFields[field.Value.ZohoKey]?.ToUpper()
                            : "";
                        changesTable.Append($@"<td>{value}</td>");
                    }

                    // New values
                    foreach (var field in activeFields)
                    {
                        string value = changedFields.ContainsKey(field.Value.CcmsKey)
                            ? changedFields[field.Value.CcmsKey]?.ToUpper()
                            : "";
                        changesTable.Append($@"<td style='color:red;font-weight:bold;'>{value}</td>");
                    }

                    changesTable.Append("</tr></table><br>");
                    finalChangesTable = changesTable.ToString();
                }

                // ==== ADDRESS CHANGES ====

                foreach (var kvp in addressFieldsMapping)
                {
                    string addressType = kvp.Key;
                    var keys = kvp.Value;

                    string zStreet = "", zCity = "", zState = "", zZip = "";
                    string eStreet = "", eCity = "", eState = "", eZip = "";

                    foreach (var field in keys)
                    {
                        string zValue = changedFields.ContainsKey(field.zohoKey) ? changedFields[field.zohoKey]?.ToUpper() : "";
                        string eValue = changedFields.ContainsKey(field.esoKey) ? changedFields[field.esoKey]?.ToUpper() : "";

                        if (field.zohoKey.Contains("Street") || field.zohoKey.Contains("Address"))
                        {
                            zStreet = zValue;
                            eStreet = eValue;
                        }
                        else if (field.zohoKey.Contains("City"))
                        {
                            zCity = zValue;
                            eCity = eValue;
                        }
                        else if (field.zohoKey.Contains("State"))
                        {
                            zState = zValue;
                            eState = eValue;
                        }
                        else if (field.zohoKey.Contains("Zip") || field.zohoKey.Contains("Code"))
                        {
                            zZip = zValue;
                            eZip = eValue;
                        }
                    }

                    // Check if any value has changed
                    bool hasChange = zStreet != eStreet || zCity != eCity || zState != eState || zZip != eZip;

                    if (hasChange)
                    {
                        addressRows.Append($@"
<tr>
    <td><strong>{addressType}</strong></td>
    <td>{zStreet}</td>
    <td>{zCity}</td>
    <td>{zState}</td>
    <td>{zZip}</td>
    <td><strong>{addressType}</strong></td>
    <td style='{(zStreet != eStreet ? "color:red;font-weight:bold;" : "")}'>{eStreet}</td>
    <td style='{(zCity != eCity ? "color:red;font-weight:bold;" : "")}'>{eCity}</td>
    <td style='{(zState != eState ? "color:red;font-weight:bold;" : "")}'>{eState}</td>
    <td style='{(zZip != eZip ? "color:red;font-weight:bold;" : "")}'>{eZip}</td>
</tr>");
                        hasAnyAddressChange = true;
                    }
                }


                // Render Address Table if changes exist
                if (hasAnyAddressChange)
                {
                    finalAddressTable = $@"
<table width='100%' border='1' cellspacing='0' cellpadding='5' style='text-align: center; border-collapse: collapse; font-size: 13px;'>
    <tr style='background-color: teal; color: white; text-align: center;'>
        <td colspan='11'><strong>ADDRESS DETAILS</strong></td>
    </tr>
    <tr style='text-align: center;'>
        <td style='background-color: #b9e3a4;' colspan='5'><strong>ZOHO (NOW)</strong></td>
        <td rowspan='6'></td>
        <td style='background-color: #00949033;' colspan='5'><strong>ZOHO (CHANGE TO)</strong></td>
    </tr>
    <tr style='background-color: #f2f2f2;'>
        <td><strong>ADDRESS TYPE</strong></td>
        <td><strong>STREET</strong></td>
        <td><strong>CITY</strong></td>
        <td><strong>STATE</strong></td>
        <td><strong>ZIP</strong></td>
        <td><strong>ADDRESS TYPE</strong></td>
        <td><strong>STREET</strong></td>
        <td><strong>CITY</strong></td>
        <td><strong>STATE</strong></td>
        <td><strong>ZIP</strong></td>
    </tr>
    {addressRows}
</table>";
                }
                else
                {
                    finalAddressTable = string.Empty;
                }
            }
            else
            {
                warningContent = @"
                                <p style='font-weight: bold; margin: 10px; padding: 10px; color: #dc3545; background-color: #f8d7da; border: 1px solid #f5c2c7; border-radius: 4px;text-align:center'>
                                    ⚠️ Warning: Account Number is not available in the Zoho CRM.
                                </p>";

                if (activeFields.Count > 0)  // Only render the table if there are any active charge rate changes
                {
                    // Initialize StringBuilder for changes table
                    changesTable.Append($@"
                <table width=""100%"" border=""1"" cellspacing=""0"" cellpadding=""5"" style=""text-align: center; border-collapse: collapse; font-size: 13px;"">
                    <tr style=""background-color: teal; color: white; text-align: center;"">
                        <td colspan=""{columnSpan}""><strong>CHARGE RATES</strong></td> 
                    </tr>
                    <tr style=""background-color: #f2f2f2;"">
                ");

                    // Loop through each active field and display the field names (now with only CCMS values)
                    foreach (var field in activeFields)
                    {
                        changesTable.Append($@"<td><strong>{field.Key}</strong></td>");
                    }

                    changesTable.Append("</tr><tr>");

                    // Loop through each active field and only display the CCMS (new) values
                    foreach (var field in activeFields)
                    {
                        string value = changedFields.ContainsKey(field.Value.CcmsKey) ? changedFields[field.Value.CcmsKey] : "";
                        changesTable.Append($@"<td>{value}</td>");
                    }

                    changesTable.Append("</tr></table><br>");
                    finalChangesTable = changesTable.ToString();
                }
                // ==== ADDRESS CHANGES ====

                foreach (var kvp in addressFieldsMapping)
                {
                    string addressType = kvp.Key;
                    var keys = kvp.Value;

                    string eStreet = "", eCity = "", eState = "", eZip = "";

                    foreach (var field in keys)
                    {
                        string eValue = changedFields.ContainsKey(field.esoKey) ? changedFields[field.esoKey]?.ToUpper() : "";

                        if (field.zohoKey.Contains("Street") || field.zohoKey.Contains("Address"))
                        {
                            eStreet = eValue;
                        }
                        else if (field.zohoKey.Contains("City"))
                        {
                            eCity = eValue;
                        }
                        else if (field.zohoKey.Contains("State"))
                        {
                            eState = eValue;
                        }
                        else if (field.zohoKey.Contains("Zip"))
                        {
                            eZip = eValue;
                        }
                    }


                    // Check if any value has changed
                    bool hasChange = eStreet != "" || eCity != "" || eState != "" || eZip != "";

                    if (hasChange)
                    {
                        addressRows.Append($@"
                        <tr>
                            <td><strong>{addressType}</strong></td>
                            <td>{eStreet}</td>
                            <td>{eCity}</td>
                            <td>{eState}</td>
                            <td>{eZip}</td>
                        </tr>");

                    hasAnyAddressChange = true;
                }
            }


                // Render Address Table if changes exist
                if (hasAnyAddressChange)
                {
                    finalAddressTable = $@"
<table width='100%' border='1' cellspacing='0' cellpadding='5' style='text-align: center; border-collapse: collapse; font-size: 13px;'>
    <tr style='background-color: teal; color: white; text-align: center;'>
        <td colspan='11'><strong>ADDRESS DETAILS</strong></td>
    </tr>
    
    <tr style='background-color: #f2f2f2;'>
        <td><strong>ADDRESS TYPE</strong></td>
        <td><strong>STREET</strong></td>
        <td><strong>CITY</strong></td>
        <td><strong>STATE</strong></td>
        <td><strong>ZIP</strong></td>
    </tr>
    {addressRows}
</table>";
                }
                else
                {
                    finalAddressTable = string.Empty;
                }


            }


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
                            {finalAddressTable}
                          </td>
                        </tr>

                        <!-- Buttons -->
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
                        </tr>

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
                        Subject = $"Approval Needed ({action}) for Account: {comFields.AccountName} - {formattedDate}",
                        Body = htmlBody,
                        IsBodyHtml = true
                    };
                    mail.To.Add(toEmail);

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

            // Save approval request record per recipient
            SaveToEmailApprovalTable(changedFields, comFields, token, emailStatus, errorMsg, allRecipients);

        }
        else // No Recepient(No Email Address)
        {
            //Console.WriteLine("No Email Address");
        }
    }

    private void SaveToEmailApprovalTable(Dictionary<string, string> changedFields, EsoCompulsoryFields comFields, string token, string emailStatus, string errorMsg, string toEmail)
    {

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string changesJson = JsonConvert.SerializeObject(changedFields);

            // Use stored procedure name instead of raw SQL
            string storedProcedureName = "";
            storedProcedureName = "spZoho_InsertCCMSEmailApprovalForZohoCRM";

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


    //Without approval (Only for testing and firstime intetaction) 
    public void DirectUpdateToZohoCRM(EsoZohoApiCredentials ZohoCred, Dictionary<string, string> changedFields, string ZohoRecordId)
    {
        // Get valid access token
        string accessToken = GetAccessTokenFromRefreshToken(ZohoCred);


        Dictionary<string, string> updateFields = new Dictionary<string, string>();

        foreach (var field in fieldMappings.Values)
        {
            string zohoKey = field.ZohoKey;   // e.g., "zoho_Year_1_Fee1"
            string ccmsKey = field.CcmsKey;   // e.g., "eso_Year1"

            if (changedFields.TryGetValue(ccmsKey, out string ccmsValue))
            {
                // Remove "zoho_" prefix for Zoho field name
                string cleanedZohoField = zohoKey.StartsWith("zoho_") ? zohoKey.Substring("zoho_".Length) : zohoKey;

                updateFields[cleanedZohoField] = ccmsValue;
            }
        }
        var updateData = new
        {
            data = new[]
            {
                updateFields
            }
        };

        string updateUrl = $"{zohoApiUrl}/Accounts/{ZohoRecordId}";
        string updateJson = JsonConvert.SerializeObject(updateData);
        try
        {
            MakeZohoApiRequest("PUT", updateUrl, accessToken, updateJson);
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    public static void UpdateZohoStatusById(string connectionString, string recordId, string ZohoStatus)
    {
        try
        {


            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("[dbo].[spZoho_UpdateZohoStatus]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", recordId);
                    cmd.Parameters.AddWithValue("@ZohoStatus", ZohoStatus);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch(Exception EX)
        {
            Console.WriteLine("Error");
        }
    }

    public static string GetZohoStatusById(string connectionString, string ClientDetailsId, string token)
    {
        string result = "";
        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("[dbo].[spZoho_GetZohoStatus]", conn))
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

public class EsoZohoApiCredentials
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string RefreshToken { get; set; }
}

public class EsoCompulsoryFields
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string ZohoCrmId { get; set; }
    public string AccountName { get; set; }
    public string RequestedBy { get; set; }
}
