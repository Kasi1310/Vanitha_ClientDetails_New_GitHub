using ClientDetails;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Runtime.Remoting.Lifetime;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;

public partial class ZohoCrmWebhookReceiver : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.HttpMethod == "POST")
        {
            try
            {
                // Step 1: Read the incoming JSON payload
                string jsonPayload;
                using (StreamReader reader = new StreamReader(Request.InputStream))
                {
                    jsonPayload = reader.ReadToEnd();
                }

                // Step 2: Deserialize JSON to an object
                var webhookData = JsonConvert.DeserializeObject<ZohoCrmWebhookData>(jsonPayload);
                
                //var approvers = new List<string>{
                //                                    "sageit4@medicount.com",
                //                                    "arengasamy@medicount.com"
                //                                };
                // Step3: Email Approval
                SendEmailApproval(webhookData);


                // Step 4: Respond to the webhook sender
                Response.ContentType = "application/json";
                Response.StatusCode = 200; // OK
                Response.Write(JsonConvert.SerializeObject(new
                {
                    Message = "Data received and stored successfully.",
                    Data = webhookData
                }));
            }
            catch (Exception ex)
            {
                // Handle errors
                Response.StatusCode = 500; // Internal Server Error
                Response.Write($"Error: {ex.Message}");
            }
        }
        else if (Request.HttpMethod == "GET")
        {
            string token = Request.QueryString["token"];
            string zcrmId = Request.QueryString["zcrmid"];
            string responder = HttpUtility.UrlDecode(Request.QueryString["responder"]);
            string decision = Request.QueryString["decision"];
            string module = Request.QueryString["module"];
            string emailStatus = decision == "approve" ? "APPROVED" : "REJECTED";
            bool isFirstApproval = true;
            string preResponder = "", preEmailStatus = "", modifiedOn = "";

            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(responder) && !string.IsNullOrEmpty(decision))
            {
                ZohoCrmWebhookData contact = new ZohoCrmWebhookData
                {
                    ContactsMailingAddress = new AddressInfo(),
                    BillingAddress = new AddressInfo(),
                    MailingAddress = new AddressInfo(),
                    PhysicalLocationAddress = new AddressInfo(),
                    InsurancePayToAddress = new AddressInfo()
                };


                string connectionString = ConfigurationManager.ConnectionStrings["ZohoCrmConnectionString"].ToString();

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand("[dbo].[IsFirstApproval]", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ZcrmId", zcrmId);
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

                        SqlCommand cmd = new SqlCommand("[dbo].[GetApprovedPEContacts]", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ZcrmId", zcrmId);
                        cmd.Parameters.AddWithValue("@Token", token);
                        cmd.Parameters.AddWithValue("@Responder", responder);
                        cmd.Parameters.AddWithValue("@EmailStatus", emailStatus);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {


                            if (reader.Read())
                            {
                                // Ensure all address objects are initialized

                                contact.Type = reader["Type"]?.ToString();
                                contact.Module = reader["Module"]?.ToString();
                                contact.Title = reader["Title"]?.ToString();
                                contact.FirstName = reader["FirstName"]?.ToString();
                                contact.LastName = reader["LastName"]?.ToString();

                                contact.ContactsMailingAddress.Street = reader["Street"]?.ToString();
                                contact.ContactsMailingAddress.City = reader["City"]?.ToString();
                                contact.ContactsMailingAddress.State = reader["State"]?.ToString();
                                contact.ContactsMailingAddress.Zip = reader["ZipCode"]?.ToString();

                                contact.Phone = reader["Phone"]?.ToString();
                                contact.Mobile = reader["Mobile"]?.ToString();
                                contact.Fax = reader["Fax"]?.ToString();
                                contact.Email = reader["Email"]?.ToString();
                                contact.CreatedBy = reader["CreatedBy"]?.ToString();
                                contact.ModifiedBy = reader["ModifiedBy"]?.ToString();
                                contact.AccountName = reader["AccountName"]?.ToString();
                                contact.AccountId = reader["AccountId"]?.ToString();
                                contact.ContactId = reader["ContactId"].ToString();

                                contact.BillingAddress.Street = reader["BillingStreet"]?.ToString();
                                contact.BillingAddress.City = reader["BillingCity"]?.ToString();
                                contact.BillingAddress.State = reader["BillingState"]?.ToString();
                                contact.BillingAddress.Zip = reader["BillingZip"]?.ToString();

                                contact.MailingAddress.Street = reader["MailingStreet"]?.ToString();
                                contact.MailingAddress.City = reader["MailingCity"]?.ToString();
                                contact.MailingAddress.State = reader["MailingState"]?.ToString();
                                contact.MailingAddress.Zip = reader["MailingZip"]?.ToString();

                                contact.PhysicalLocationAddress.Street = reader["PhysicalLocationStreet"]?.ToString();
                                contact.PhysicalLocationAddress.City = reader["PhysicalLocationCity"]?.ToString();
                                contact.PhysicalLocationAddress.State = reader["PhysicalLocationState"]?.ToString();
                                contact.PhysicalLocationAddress.Zip = reader["PhysicalLocationZip"]?.ToString();

                                contact.InsurancePayToAddress.Street = reader["InsurancePayToStreet"]?.ToString();
                                contact.InsurancePayToAddress.City = reader["InsurancePayToCity"]?.ToString();
                                contact.InsurancePayToAddress.State = reader["InsurancePayToState"]?.ToString();
                                contact.InsurancePayToAddress.Zip = reader["InsurancePayToZip"]?.ToString();
                            }
                        }
                    }
                    if (decision == "approve")
                    {
                        SaveToDatabase(contact);
                    }
                    else
                    {
                        Console.WriteLine("Rejected");
                    }
                    // Determine Swal settings based on decision
                    string swalIcon = "info";
                    string swalTitle = "Response Received";
                    string headerColor = "#2E8B57"; // default greenish color
                    string headerText = "Your changes were successfully applied to the CCMS and PED application.";

                    if (!string.IsNullOrEmpty(decision))
                    {
                        if (decision.Equals("Approve", StringComparison.OrdinalIgnoreCase))
                        {
                            swalIcon = "success";
                            swalTitle = "✅ Approved!";
                            headerColor = "#2E8B57";  // green
                            headerText = "Your changes were successfully applied to the CCMS and PED application.";
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
                                        <td>{preEmailStatus}</td>
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
                Response.Write("❌ Missing one or more required query parameters.");
            }
           
        }else
        {
            // Reject non-POST requests
            Response.StatusCode = 405; // Method Not Allowed
            Response.Write("Only POST and GET requests are allowed.");
        }
    }



    private void SendEmailApproval(ZohoCrmWebhookData data) // New Changes
    {
        string smtpHost = "";
        int smtpPort = 0;
        string smtpEmail = "";
        string smtpPassword = ""; // App Password

        string emailConnectionString = ConfigurationManager.ConnectionStrings["ZohoCrmConnectionString"].ToString();
        using (SqlConnection conn = new SqlConnection(emailConnectionString))
        {
            conn.Open();
            {
                SqlCommand cmd = new SqlCommand("[dbo].[GetSMTPDetails]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        smtpHost = reader["server"] != DBNull.Value ? reader["server"].ToString() : "";
                        smtpPort = reader["port"] != DBNull.Value ? Convert.ToInt32(reader["port"]) : 0;
                        smtpEmail = reader["server"] != DBNull.Value ? reader["username"].ToString() : "";
                        smtpPassword = reader["server"] != DBNull.Value ? reader["password"].ToString() : "";

                    }
                }
            }
        }
            

        string action = data.Type;
        string module = data.Module;

        var addressTable = new StringBuilder();
        var changesTable = new StringBuilder();
        var finalChangesTable = "";
        var finalAddressTable = "";
        ZohoCrmWebhookData zohoData = new ZohoCrmWebhookData();
        bool hasAnyAddressChange = false;
        bool hasAnyContactChange = false;
        List<string> recipients = new List<string>();

         using (SqlConnection conn = new SqlConnection(emailConnectionString))
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
                            recipients.Add(email);
                        }
                    }
                }
            }

            }

        if (recipients.Count != 0)
        {

            if (action == "EDIT")
            {
                // Initialize old values to empty to avoid null issues
                string oldCompanyName = "", oldTitle = "", oldFirstName = "", oldLastName = "", oldAddress = "", oldZipCode = "";
                string oldPhone = "", oldFax = "", oldEmail = "", oldCreatedBy = "", oldStreet = "", oldCity = "", oldState = "";

                string connectionString = ConfigurationManager.ConnectionStrings["ZohoCrmConnectionString"].ToString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    if (module == "CONTACTS") // CONTACTS
                    {
                        SqlCommand cmd = new SqlCommand("[dbo].[GetOldPEContactValues]", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ZohoCrmId", data.ContactId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                oldCompanyName = reader["companyName"] != DBNull.Value ? reader["companyName"].ToString() : "";
                                oldTitle = reader["Title"] != DBNull.Value ? reader["Title"].ToString() : "";
                                oldFirstName = reader["FirstName"] != DBNull.Value ? reader["FirstName"].ToString() : "";
                                oldLastName = reader["LastName"] != DBNull.Value ? reader["LastName"].ToString() : "";
                                oldAddress = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : "";

                                // Trim and split the address by comma
                                string[] addressParts = oldAddress.Split(',', (char)StringSplitOptions.RemoveEmptyEntries);

                                // Safely assign values
                                oldStreet = addressParts.Length > 0 ? addressParts[0].Trim() : "";
                                oldCity = addressParts.Length > 1 ? addressParts[1].Trim() : "";
                                oldState = addressParts.Length > 2 ? addressParts[2].Trim() : "";

                                oldZipCode = reader["ZipCode"] != DBNull.Value ? reader["ZipCode"].ToString() : "";
                                oldPhone = reader["Phone"] != DBNull.Value ? reader["Phone"].ToString() : "";
                                oldFax = reader["Fax"] != DBNull.Value ? reader["Fax"].ToString() : "";
                                oldEmail = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : "";
                                oldCreatedBy = reader["CreatedBy"] != DBNull.Value ? reader["CreatedBy"].ToString() : "";
                            }
                        }
                    }
                    else // ACCOUNTS
                    {
                        SqlCommand cmd = new SqlCommand("[dbo].[GetOldAccountAddresses]", conn)
                        {
                            CommandType = CommandType.StoredProcedure
                        };

                        cmd.Parameters.AddWithValue("@AccountId", data.AccountId);
                        zohoData.PhysicalLocationAddress = new AddressInfo { Street = "", City = "", State = "", Zip = "" }; // Default to empty
                        zohoData.MailingAddress = new AddressInfo { Street = "", City = "", State = "", Zip = "" }; // Default to empty
                        zohoData.BillingAddress = new AddressInfo { Street = "", City = "", State = "", Zip = "" }; // Default to empty
                        zohoData.InsurancePayToAddress = new AddressInfo { Street = "", City = "", State = "", Zip = "" }; // Default to empty

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Map the first address (Physical Location Address)
                                if (reader["AddressType"].ToString() == "PHYSICAL LOCATION ADDRESS")
                                {
                                    zohoData.PhysicalLocationAddress = new AddressInfo
                                    {
                                        Street = reader["Street"] != DBNull.Value ? reader["Street"].ToString() : "",
                                        City = reader["City"] != DBNull.Value ? reader["City"].ToString() : "",
                                        State = reader["State"] != DBNull.Value ? reader["State"].ToString() : "",
                                        Zip = reader["Zip"] != DBNull.Value ? reader["Zip"].ToString() : ""
                                    };
                                }

                                // Map the second address (Mailing Address)
                                else if (reader["AddressType"].ToString() == "MAILING ADDRESS")
                                {
                                    zohoData.MailingAddress = new AddressInfo
                                    {
                                        Street = reader["Street"] != DBNull.Value ? reader["Street"].ToString() : "",
                                        City = reader["City"] != DBNull.Value ? reader["City"].ToString() : "",
                                        State = reader["State"] != DBNull.Value ? reader["State"].ToString() : "",
                                        Zip = reader["Zip"] != DBNull.Value ? reader["Zip"].ToString() : ""
                                    };
                                }

                                // Map the third address (Billing Address)
                                else if (reader["AddressType"].ToString() == "BILLING ADDRESS")
                                {
                                    zohoData.BillingAddress = new AddressInfo
                                    {
                                        Street = reader["Street"] != DBNull.Value ? reader["Street"].ToString() : "",
                                        City = reader["City"] != DBNull.Value ? reader["City"].ToString() : "",
                                        State = reader["State"] != DBNull.Value ? reader["State"].ToString() : "",
                                        Zip = reader["Zip"] != DBNull.Value ? reader["Zip"].ToString() : ""
                                    };
                                }

                                // Map the fourth address (Insurance Pay To Address)
                                else if (reader["AddressType"].ToString() == "INSURANCE PAY TO ADDRESS")
                                {
                                    zohoData.InsurancePayToAddress = new AddressInfo
                                    {
                                        Street = reader["Street"] != DBNull.Value ? reader["Street"].ToString() : "",
                                        City = reader["City"] != DBNull.Value ? reader["City"].ToString() : "",
                                        State = reader["State"] != DBNull.Value ? reader["State"].ToString() : "",
                                        Zip = reader["Zip"] != DBNull.Value ? reader["Zip"].ToString() : ""
                                    };
                                }
                            }
                        }
                    }


                }

                var addressRows = new StringBuilder();

                if (module == "CONTACTS")
                {
                    // Extract values
                    string zTitle = data?.Title?.ToUpper() ?? "";
                    string dTitle = oldTitle?.ToUpper() ?? "";
                    string zFirstName = data?.FirstName?.ToUpper() ?? "";
                    string dFirstName = oldFirstName?.ToUpper() ?? "";
                    string zLastName = data?.LastName?.ToUpper() ?? "";
                    string dLastName = oldLastName?.ToUpper() ?? "";
                    string zEmail = data?.Email?.ToUpper() ?? "";
                    string dEmail = oldEmail?.ToUpper() ?? "";
                    string zPhone = data?.Phone?.ToUpper() ?? "";
                    string dPhone = oldPhone?.ToUpper() ?? "";
                    string zFax = data?.Fax?.ToUpper() ?? "";
                    string dFax = oldFax?.ToUpper() ?? "";

                    // Check if anything is different
                    bool isContactDifferent =
                            !string.Equals(zTitle, dTitle, StringComparison.OrdinalIgnoreCase) ||
                            !string.Equals(zFirstName, dFirstName, StringComparison.OrdinalIgnoreCase) ||
                            !string.Equals(zLastName, dLastName, StringComparison.OrdinalIgnoreCase) ||
                            !string.Equals(zEmail, dEmail, StringComparison.OrdinalIgnoreCase) ||
                            !string.Equals(zPhone, dPhone, StringComparison.OrdinalIgnoreCase) ||
                            !string.Equals(zFax, dFax, StringComparison.OrdinalIgnoreCase);

                    if (isContactDifferent)
                    {
                        hasAnyContactChange = true;


                        changesTable.Append($@"
                    <table width=""100%"" border=""1"" cellspacing=""0"" cellpadding=""5"" style=""text-align: center; border-collapse: collapse; font-size: 13px;"">
                        <tr style=""background-color: teal; color: white; text-align: center;"">
                          <td colspan=""13""><strong>CONTACT PERSON DETAILS</strong></td>
                        </tr>
                        <tr style=""text-align: center;"">
                          <td style=""background-color: #b9e3a4;"" colspan=""6""><strong>NOW</strong></td>
                          <td rowspan=""3""></td>
                          <td style=""background-color: #00949033;"" colspan=""6""><strong>CHANGE TO</strong></td>
                        </tr>
                        <tr style=""background-color: #f2f2f2;"">
                          <td><strong>TITLE</strong></td>
                          <td><strong>FIRST NAME</strong></td>
                          <td><strong>LAST NAME</strong></td>
                          <td><strong>EMAIL</strong></td>
                          <td><strong>PHONE</strong></td>
                          <td><strong>FAX</strong></td>
                          <td><strong>TITLE</strong></td>
                          <td><strong>FIRST NAME</strong></td>
                          <td><strong>LAST NAME</strong></td>
                          <td><strong>EMAIL</strong></td>
                          <td><strong>PHONE</strong></td>
                          <td><strong>FAX</strong></td>
                        </tr>
                        <tr>
                          <td>{dTitle}</td>
                          <td>{dFirstName}</td>
                          <td>{dLastName}</td>
                          <td>{oldEmail}</td>
                          <td>{dPhone}</td>
                          <td>{dFax}</td>
                          <td style='{(zTitle != dTitle ? "color:red;font-weight:bold;" : "")}'>{zTitle}</td>
                          <td style='{(zFirstName != dFirstName ? "color:red;font-weight:bold;" : "")}'>{zFirstName}</td>
                          <td style='{(zLastName != dLastName ? "color:red;font-weight:bold;" : "")}'>{zLastName}</td>
                          <td style='{(zEmail != dEmail ? "color:red;font-weight:bold;" : "")}'>{data.Email}</td>
                          <td style='{(zPhone != dPhone ? "color:red;font-weight:bold;" : "")}'>{zPhone}</td>
                          <td style='{(zFax != dFax ? "color:red;font-weight:bold;" : "")}'>{zFax}</td>
                        </tr>
                      </table>

                      <br>
                ");
                    }

                    finalChangesTable = changesTable.ToString();

                    // Extract values
                    string zStreet = data?.ContactsMailingAddress?.Street?.ToUpper() ?? "";
                    string dStreet = oldStreet?.ToUpper() ?? "";
                    string zCity = data?.ContactsMailingAddress?.City?.ToUpper() ?? "";
                    string dCity = oldCity?.ToUpper() ?? "";
                    string zState = data?.ContactsMailingAddress?.State?.ToUpper() ?? "";
                    string dState = oldState?.ToUpper() ?? "";
                    string zZip = data?.ContactsMailingAddress?.Zip?.ToUpper() ?? "";
                    string dZip = oldZipCode?.ToUpper() ?? "";

                    // Check if anything is different
                    bool isDifferent =
                            !string.Equals(zStreet, dStreet, StringComparison.OrdinalIgnoreCase) ||
                            !string.Equals(zCity, dCity, StringComparison.OrdinalIgnoreCase) ||
                            !string.Equals(zState, dState, StringComparison.OrdinalIgnoreCase) ||
                            !string.Equals(zZip, dZip, StringComparison.OrdinalIgnoreCase);

                    if (isDifferent)
                    {
                        hasAnyAddressChange = true;

                        addressRows.Append($@"
                        <tr>
                            <td><strong>MAILING ADDRESS</strong></td>
                            <td>{dStreet}</td>
                            <td>{dCity}</td>
                            <td>{dState}</td>
                            <td>{dZip}</td>
                            <td><strong>MAILING ADDRESS</strong></td>
                            <td style='{(zStreet != dStreet ? "color:red;font-weight:bold;" : "")}'>{zStreet}</td>
                            <td style='{(zCity != dCity ? "color:red;font-weight:bold;" : "")}'>{zCity}</td>
                            <td style='{(zState != dState ? "color:red;font-weight:bold;" : "")}'>{zState}</td>
                            <td style='{(zZip != dZip ? "color:red;font-weight:bold;" : "")}'>{zZip}</td>
                        </tr>");
                    }

                    // Build full table only if needed
                    if (hasAnyAddressChange)
                    {
                        addressTable.Append($@"
                        <!-- ADDRESS DETAILS -->        
                        <table width='100%' border='1' cellspacing='0' cellpadding='5' style='text-align: center; border-collapse: collapse; font-size: 13px;'>
                            <tr style='background-color: teal; color: white; text-align: center;'>
                                <td colspan='11'><strong>ADDRESS DETAILS</strong></td>
                             </tr>
                             <tr style='text-align: center;'>
                                 <td style='background-color: #b9e3a4;' colspan='5'><strong>NOW</strong></td>
                                 <td rowspan='6'></td>
                                 <td style='background-color: #00949033;' colspan='5'><strong>CHANGE TO</strong></td>
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
                        </table>");
                    }
                }
                else // ACCOUNTS ADDRESSES
                {
                    changesTable.Append($@"");

                    finalChangesTable = changesTable.ToString();

                    void AppendRowIfDifferent(string addressType, dynamic zoho, dynamic newData)
                    {
                        // Extract values
                        string zStreet = zoho?.Street?.ToUpper() ?? "";
                        string dStreet = newData?.Street?.ToUpper() ?? "";
                        string zCity = zoho?.City?.ToUpper() ?? "";
                        string dCity = newData?.City?.ToUpper() ?? "";
                        string zState = zoho?.State?.ToUpper() ?? "";
                        string dState = newData?.State?.ToUpper() ?? "";
                        string zZip = zoho?.Zip?.ToUpper() ?? "";
                        string dZip = newData?.Zip?.ToUpper() ?? "";

                        // Check if anything is different
                        bool isDifferent =
                            !string.Equals(zStreet, dStreet, StringComparison.OrdinalIgnoreCase) ||
                            !string.Equals(zCity, dCity, StringComparison.OrdinalIgnoreCase) ||
                            !string.Equals(zState, dState, StringComparison.OrdinalIgnoreCase) ||
                            !string.Equals(zZip, dZip, StringComparison.OrdinalIgnoreCase);

                        if (isDifferent)
                        {
                            hasAnyAddressChange = true;

                            addressRows.Append($@"
                        <tr>
                            <td><strong>{addressType}</strong></td>
                            <td>{zStreet}</td>
                            <td>{zCity}</td>
                            <td>{zState}</td>
                            <td>{zZip}</td>
                            <td><strong>{addressType}</strong></td>
                            <td style='{(zStreet != dStreet ? "color:red;font-weight:bold;" : "")}'>{dStreet}</td>
                            <td style='{(zCity != dCity ? "color:red;font-weight:bold;" : "")}'>{dCity}</td>
                            <td style='{(zState != dState ? "color:red;font-weight:bold;" : "")}'>{dState}</td>
                            <td style='{(zZip != dZip ? "color:red;font-weight:bold;" : "")}'>{dZip}</td>
                        </tr>");
                        }
                    }

                    // Compare and build only differing rows
                    AppendRowIfDifferent("BILLING ADDRESS", zohoData?.BillingAddress, data?.BillingAddress);
                    AppendRowIfDifferent("MAILING ADDRESS", zohoData?.MailingAddress, data?.MailingAddress);
                    AppendRowIfDifferent("PHYSICAL LOCATION ADDRESS", zohoData?.PhysicalLocationAddress, data?.PhysicalLocationAddress);
                    AppendRowIfDifferent("INSURANCE PAY TO ADDRESS", zohoData?.InsurancePayToAddress, data?.InsurancePayToAddress);

                    // Build full table only if needed
                    if (hasAnyAddressChange)
                    {
                        addressTable.Append($@"
                        <!-- ADDRESS DETAILS -->        
                        <table width='100%' border='1' cellspacing='0' cellpadding='5' style='text-align: center; border-collapse: collapse; font-size: 13px;'>
                            <tr style='background-color: teal; color: white; text-align: center;'>
                                <td colspan='11'><strong>ADDRESS DETAILS</strong></td>
                             </tr>
                             <tr style='text-align: center;'>
                                 <td style='background-color: #b9e3a4;' colspan='5'><strong>NOW</strong></td>
                                 <td rowspan='6'></td>
                                 <td style='background-color: #00949033;' colspan='5'><strong>CHANGE TO</strong></td>
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
                        </table>");
                    }
                }

                finalAddressTable = addressTable.ToString();
            }

            else // ADD or DELETE
            {
                addressTable.Append($@"
            <!-- ADDRESS DETAILS -->
            <table width=""100%"" border=""1"" cellspacing=""0"" cellpadding=""5"" style=""text-align: center;border-collapse: collapse; font-size: 13px;"">
                <tr style=""background-color: teal; color: white; text-align: center;"">
                  <td colspan=""12""><strong>ADDRESS DETAILS</strong></td>
                </tr>
                <tr style=""background-color: #f2f2f2; text-align: center;"">
                  <td><strong>ADDRESS TYPE</strong></td>
                  <td><strong>STREET</strong></td>
                  <td><strong>CITY</strong></td>
                  <td><strong>STATE</strong></td>
                  <td><strong>ZIP</strong></td>
                </tr>");
                if (module == "CONTACTS")
                {
                    changesTable.Append($@"
                            <table width=""100%"" border=""1"" cellspacing=""0"" cellpadding=""5"" style=""text-align: center;border-collapse: collapse; font-size: 13px;"">

                                <tr style=""background-color: teal; color: white; text-align: center;"">
                                  <td colspan=""12""><strong>CONTACT PERSON DETAILS</strong></td>
                                </tr>
    
                                <tr style=""background-color: #f2f2f2;"">
                                  <td><strong>TITLE</strong></td>
                                  <td><strong>FIRST NAME</strong></td>
                                  <td><strong>LAST NAME</strong></td>
                                  <td><strong>EMAIL</strong></td>
                                  <td><strong>PHONE</strong></td>
                                  <td><strong>FAX</strong></td>
                                </tr>
                                <tr>
                                  <td>{data.Title.ToUpper()}</td>
                                  <td>{data.FirstName.ToUpper()}</td>
                                  <td>{data.LastName.ToUpper()}</td>
                                  <td>{data.Email}</td>
                                  <td>{data.Phone.ToUpper()}</td>
                                  <td>{data.Fax.ToUpper()}</td>
                                </tr>
                              </table>
                           <br>
                ");

                    finalChangesTable = changesTable.ToString();

                    addressTable.Append($@"
                    <tr>
                      <td><strong>MAILING ADDRESS</strong></td>
                      <td>{data.ContactsMailingAddress.Street.ToUpper()}</td>
                      <td>{data.ContactsMailingAddress.City.ToUpper()}</td>
                      <td>{data.ContactsMailingAddress.State.ToUpper()}</td>
                      <td>{data.ContactsMailingAddress.Zip.ToUpper()}</td>
                    </tr>
                </table>");
                }
                else // ACCOUNTS
                {
                    changesTable.Append($@"");

                    finalChangesTable = changesTable.ToString();

                    addressTable.Append($@"
                    <tr>
                      <td><strong>BILLING ADDRESS</strong></td>
                      <td>{data.BillingAddress.Street.ToUpper()}</td>
                      <td>{data.BillingAddress.City.ToUpper()}</td>
                      <td>{data.BillingAddress.State.ToUpper()}</td>
                      <td>{data.BillingAddress.Zip.ToUpper()}</td>
                    </tr>
                    <tr>
                      <td><strong>MAILING ADDRESS</strong></td>
                      <td>{data.MailingAddress.Street.ToUpper()}</td>
                      <td>{data.MailingAddress.City.ToUpper()}</td>
                      <td>{data.MailingAddress.State.ToUpper()}</td>
                      <td>{data.MailingAddress.Zip.ToUpper()}</td>
                    </tr>
                    <tr>
                      <td><strong>PHYSICAL LOCATION ADDRESS</strong></td>
                      <td>{data.PhysicalLocationAddress.Street.ToUpper()}</td>
                      <td>{data.PhysicalLocationAddress.City.ToUpper()}</td>
                      <td>{data.PhysicalLocationAddress.State.ToUpper()}</td>
                      <td>{data.PhysicalLocationAddress.Zip.ToUpper()}</td>
                    </tr>
                    <tr>
                      <td><strong>INSURANCE PAY TO ADDRESS</strong></td>
                      <td>{data.InsurancePayToAddress.Street.ToUpper()}</td>
                      <td>{data.InsurancePayToAddress.City.ToUpper()}</td>
                      <td>{data.InsurancePayToAddress.State.ToUpper()}</td>
                      <td>{data.InsurancePayToAddress.Zip.ToUpper()}</td>
                    </tr>
                </table>");
                }

                finalAddressTable = addressTable.ToString();
            }

            if ((data.Type == "EDIT" && (hasAnyContactChange || hasAnyAddressChange)) || data.Type == "ADD" || data.Type == "DELETE")
            {
                string token = Guid.NewGuid().ToString(); // Unique per recipient
                //string webhookLink = "https://webhook.site/7bed42f2-7403-4cc5-ae2b-6b3e15a5d0c0";
                //string webhookLink = "http://localhost:52911/ZohoCrmWebhookReceiver.aspx";
                string webhookLink = "https://snapshots.medicount.com/ZohoCrmWebhookReceiver.aspx";

                List<string> failedRecipients = new List<string>();
                List<string> successfulRecipients = new List<string>();

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
                                <td style=""font-weight: bold;"">{data.AccountName.ToUpper()}</td>
                              </tr>
                              <tr>
                                <td align=""right"" style=""color: gray; font-weight: bold;"">ACCOUNT NUMBER</td>
                                <td style=""font-weight: bold;"">{data.AccountId}</td>
                              </tr>
                              <tr>
                                <td align=""right"" style=""color: gray; font-weight: bold;"">REQUESTED BY</td>
                                <td style=""font-weight: bold;"">{data.ModifiedBy.ToUpper()}</td>
                              </tr>
                            </table>
                          </td>
                        </tr>

                        <!-- Instruction -->
                        <tr>
                          <td style=""padding: 10px;"">
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
                                  <a href=""{webhookLink}?token={token}&zcrmid={data.ContactId}&responder={Uri.EscapeDataString(toEmail)}&decision=approve&module={data.Module}""
                                     style=""display: inline-block; padding: 12px 20px; background-color: #28a745; color: #ffffff; text-decoration: none; border-radius: 4px;"">
                                    ✅ Approve
                                  </a>
                                </td>
                                <td>
                                  <a href=""{webhookLink}?token={token}&zcrmid={data.ContactId}&responder={Uri.EscapeDataString(toEmail)}&decision=reject&module={data.Module}""
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
                        MailMessage mail = new MailMessage
                        {
                            From = new MailAddress(smtpEmail),
                            Subject = $"Approval Needed for Account: {data.AccountName} - {DateTime.Now}",
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
                SaveToEmailApprovalTable(data, zohoData, token, emailStatus, errorMsg, allRecipients);
            }
        }
        else // No Recepient(No Email Address)
        {
            //Console.WriteLine("No Email Address");
        }
    }

    
    private void SaveToEmailApprovalTable(ZohoCrmWebhookData data, ZohoCrmWebhookData ZohoData, string token, string emailStatus, string errorMsg, string toEmail)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ZohoCrmConnectionString"].ToString();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            // Use stored procedure name instead of raw SQL
            string storedProcedureName = "";

            if (data.Module == "CONTACTS") // CONTACTS
            {
                storedProcedureName = "InsertPEDContactsEmailApprovalFromZohoCRM";

                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    // Indicate that this is a stored procedure, not a regular SQL query
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Add parameters for the stored procedure
                    command.Parameters.AddWithValue("@Type", data.Type ?? "");
                    command.Parameters.AddWithValue("@Module", data.Module ?? "");
                    command.Parameters.AddWithValue("@Title", data.Title ?? "");
                    command.Parameters.AddWithValue("@FirstName", data.FirstName ?? "");
                    command.Parameters.AddWithValue("@LastName", data.LastName ?? "");
                    command.Parameters.AddWithValue("@Street", data.ContactsMailingAddress.Street ?? "");
                    command.Parameters.AddWithValue("@City", data.ContactsMailingAddress.City ?? "");
                    command.Parameters.AddWithValue("@State", data.ContactsMailingAddress.State ?? "");
                    command.Parameters.AddWithValue("@Zipcode", data.ContactsMailingAddress.Zip ?? "");
                    command.Parameters.AddWithValue("@Phone", data.Phone ?? "");
                    command.Parameters.AddWithValue("@Mobile", data.Mobile ?? "");
                    command.Parameters.AddWithValue("@Fax", data.Fax ?? "");
                    command.Parameters.AddWithValue("@Email", data.Email ?? "");
                    command.Parameters.AddWithValue("@CreatedBy", data.CreatedBy ?? "");
                    command.Parameters.AddWithValue("@ModifiedBy", data.ModifiedBy ?? "");
                    command.Parameters.AddWithValue("@AccountName", data.AccountName ?? "");
                    command.Parameters.AddWithValue("@AccountId", data.AccountId ?? "");
                    command.Parameters.AddWithValue("@ContactId", data.ContactId ?? "");
                    command.Parameters.AddWithValue("@Token", token);
                    command.Parameters.AddWithValue("@EmailStatus", emailStatus);
                    command.Parameters.AddWithValue("@ErrorMsg", errorMsg);
                    command.Parameters.AddWithValue("@EmailReceiver", toEmail);


                    // Execute the stored procedure
                    command.ExecuteNonQuery();
                }
            }
            else // ACCOUNTS
            {
                storedProcedureName = "InsertPEDAccountsEmailApprovalFromZohoCRM";

                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    // Indicate that this is a stored procedure, not a regular SQL query
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Add parameters for the stored procedure
                    command.Parameters.AddWithValue("@Type", data.Type ?? "");
                    command.Parameters.AddWithValue("@Module", data.Module ?? "");

                    command.Parameters.AddWithValue("@BillingStreet", data.BillingAddress.Street ?? "");
                    command.Parameters.AddWithValue("@BillingCity", data.BillingAddress.City ?? "");
                    command.Parameters.AddWithValue("@BillingState", data.BillingAddress.State ?? "");
                    command.Parameters.AddWithValue("@BillingZip", data.BillingAddress.Zip ?? "");
                    command.Parameters.AddWithValue("@MailingStreet", data.MailingAddress.Street ?? "");
                    command.Parameters.AddWithValue("@MailingCity", data.MailingAddress.City ?? "");
                    command.Parameters.AddWithValue("@MailingState", data.MailingAddress.State ?? "");
                    command.Parameters.AddWithValue("@MailingZip", data.MailingAddress.Zip ?? "");
                    command.Parameters.AddWithValue("@PhysicalLocationStreet", data.PhysicalLocationAddress.Street ?? "");
                    command.Parameters.AddWithValue("@PhysicalLocationCity", data.PhysicalLocationAddress.City ?? "");
                    command.Parameters.AddWithValue("@PhysicalLocationState", data.PhysicalLocationAddress.State ?? "");
                    command.Parameters.AddWithValue("@PhysicalLocationZip", data.PhysicalLocationAddress.Zip ?? "");
                    command.Parameters.AddWithValue("@InsurancePayToStreet", data.InsurancePayToAddress.Street ?? "");
                    command.Parameters.AddWithValue("@InsurancePayToCity", data.InsurancePayToAddress.City ?? "");
                    command.Parameters.AddWithValue("@InsurancePayToState", data.InsurancePayToAddress.State ?? "");
                    command.Parameters.AddWithValue("@InsurancePayToZip", data.InsurancePayToAddress.Zip ?? "");

                    command.Parameters.AddWithValue("@CreatedBy", data.CreatedBy ?? "");
                    command.Parameters.AddWithValue("@ModifiedBy", data.ModifiedBy ?? "");
                    command.Parameters.AddWithValue("@AccountName", data.AccountName ?? "");
                    command.Parameters.AddWithValue("@AccountId", data.AccountId ?? "");
                    command.Parameters.AddWithValue("@Token", token);
                    command.Parameters.AddWithValue("@EmailStatus", emailStatus);
                    command.Parameters.AddWithValue("@ErrorMsg", errorMsg);
                    command.Parameters.AddWithValue("@EmailReceiver", toEmail);


                    // Execute the stored procedure
                    command.ExecuteNonQuery();
                }
            }
            
        }
    }

    private void SaveToDatabase(ZohoCrmWebhookData data)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ZohoCrmConnectionString"].ToString();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            // Use stored procedure name instead of raw SQL
            string storedProcedureName = "";
            if (data.Module == "CONTACTS")
            {
                storedProcedureName = "UpdatePEDContactsFromZohoCRM";

                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    // Indicate that this is a stored procedure, not a regular SQL query
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Add parameters for the stored procedure
                    command.Parameters.AddWithValue("@Type", data.Type ?? "");
                    command.Parameters.AddWithValue("@Title", data.Title ?? "");
                    command.Parameters.AddWithValue("@FirstName", data.FirstName ?? "");
                    command.Parameters.AddWithValue("@LastName", data.LastName ?? "");
                    command.Parameters.AddWithValue("@Street", data.ContactsMailingAddress.Street ?? "");
                    command.Parameters.AddWithValue("@City", data.ContactsMailingAddress.City ?? "");
                    command.Parameters.AddWithValue("@State", data.ContactsMailingAddress.State ?? "");
                    command.Parameters.AddWithValue("@Zipcode", data.ContactsMailingAddress.Zip ?? "");
                    command.Parameters.AddWithValue("@Phone", data.Phone ?? "");
                    command.Parameters.AddWithValue("@Mobile", data.Mobile ?? "");
                    command.Parameters.AddWithValue("@Fax", data.Fax ?? "");
                    command.Parameters.AddWithValue("@Email", data.Email ?? "");
                    command.Parameters.AddWithValue("@CreatedBy", data.CreatedBy ?? "");
                    command.Parameters.AddWithValue("@ModifiedBy", data.ModifiedBy ?? "");
                    command.Parameters.AddWithValue("@AccountName", data.AccountName ?? "");
                    command.Parameters.AddWithValue("@AccountId", data.AccountId ?? "");
                    command.Parameters.AddWithValue("@ContactId", data.ContactId ?? "");

                    // Execute the stored procedure
                    command.ExecuteNonQuery();
                }
            }
            else
            {
                storedProcedureName = "UpdatePEDAccountsFromZohoCRM";

                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    // Indicate that this is a stored procedure, not a regular SQL query
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    // Add parameters for the stored procedure
                    command.Parameters.AddWithValue("@Type", data.Type ?? "");

                    command.Parameters.AddWithValue("@BillingStreet", data.BillingAddress.Street ?? "");
                    command.Parameters.AddWithValue("@BillingCity", data.BillingAddress.City ?? "");
                    command.Parameters.AddWithValue("@BillingState", data.BillingAddress.State ?? "");
                    command.Parameters.AddWithValue("@BillingZip", data.BillingAddress.Zip ?? "");
                    command.Parameters.AddWithValue("@MailingStreet", data.MailingAddress.Street ?? "");
                    command.Parameters.AddWithValue("@MailingCity", data.MailingAddress.City ?? "");
                    command.Parameters.AddWithValue("@MailingState", data.MailingAddress.State ?? "");
                    command.Parameters.AddWithValue("@MailingZip", data.MailingAddress.Zip ?? "");
                    command.Parameters.AddWithValue("@PhysicalLocationStreet", data.PhysicalLocationAddress.Street ?? "");
                    command.Parameters.AddWithValue("@PhysicalLocationCity", data.PhysicalLocationAddress.City ?? "");
                    command.Parameters.AddWithValue("@PhysicalLocationState", data.PhysicalLocationAddress.State ?? "");
                    command.Parameters.AddWithValue("@PhysicalLocationZip", data.PhysicalLocationAddress.Zip ?? "");
                    command.Parameters.AddWithValue("@InsurancePayToStreet", data.InsurancePayToAddress.Street ?? "");
                    command.Parameters.AddWithValue("@InsurancePayToCity", data.InsurancePayToAddress.City ?? "");
                    command.Parameters.AddWithValue("@InsurancePayToState", data.InsurancePayToAddress.State ?? "");
                    command.Parameters.AddWithValue("@InsurancePayToZip", data.InsurancePayToAddress.Zip ?? "");
                    
                    command.Parameters.AddWithValue("@CreatedBy", data.CreatedBy ?? "");
                    command.Parameters.AddWithValue("@ModifiedBy", data.ModifiedBy ?? "");
                    command.Parameters.AddWithValue("@AccountName", data.AccountName ?? "");
                    command.Parameters.AddWithValue("@AccountId", data.AccountId ?? "");

                    // Execute the stored procedure
                    command.ExecuteNonQuery();
                }
            }
            
        }
    }
}

// Define the data structure for the webhook payload
public class ZohoCrmWebhookData
{
    public string Type { get; set; }
    public string Module { get; set; }
    public string Title { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Mobile { get; set; }
    public string Fax { get; set; }
    public string Email { get; set; }
    public string CreatedBy { get; set; }
    public string ModifiedBy { get; set; }
    public string AccountName { get; set; }
    public string AccountId { get; set; }
    public string ContactId { get; set; }
    public AddressInfo ContactsMailingAddress { get; set; }
    public AddressInfo BillingAddress { get; set; }  
    public AddressInfo MailingAddress { get; set; }
    public AddressInfo PhysicalLocationAddress { get; set; }
    public AddressInfo InsurancePayToAddress { get; set; }
}

public class AddressInfo
{
    public string State { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string Zip { get; set; }

}