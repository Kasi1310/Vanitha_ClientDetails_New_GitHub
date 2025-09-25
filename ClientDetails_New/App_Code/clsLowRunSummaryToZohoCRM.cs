using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.UI;

namespace ClientDetails.App_Code
{
    public class clsLowRunSummaryToZohoCRM
    {
        string zohoAuthUrl = ConfigurationManager.AppSettings["ZohoAuthenticationUrl"].ToString();
        string ClientId = ConfigurationManager.AppSettings["ZohoClientId"].ToString();
        string ClientSecret = ConfigurationManager.AppSettings["ZohoClientSecret"].ToString();
        string RefreshToken = ConfigurationManager.AppSettings["ZohoRefreshToken"].ToString();

        public string GetAccessTokenFromRefreshToken()
        {
            try
            {
                //string url = "https://accounts.zoho.com/oauth/v2/token";
                string postData = $"refresh_token={RefreshToken}&client_id={ClientId}&client_secret={ClientSecret}&grant_type=refresh_token";

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

        public string MakeZohoApiRequest(string method, string url, string accessToken, string jsonPayload = null)
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