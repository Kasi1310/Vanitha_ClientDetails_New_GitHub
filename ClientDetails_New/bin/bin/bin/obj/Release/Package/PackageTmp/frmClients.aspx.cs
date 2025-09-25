using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClientDetails.App_Code;
using ClosedXML.Excel;

namespace ClientDetails
{
    public partial class frmClients : System.Web.UI.Page
    {
        clsClientDetails objclsClientDetails;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadClientGrid();
            }
        }

        private void LoadClientGrid()
        {
            objclsClientDetails = new clsClientDetails();
            ViewState["dtClients"] = objclsClientDetails.SelectClients();
            if (ViewState["dtClients"] != null)
            {
                gvClients.DataSource = ViewState["dtClients"];
                gvClients.DataBind();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            objclsClientDetails = new clsClientDetails();
            objclsClientDetails.ClientID = int.Parse(txtClientNo.Text.Trim());
            objclsClientDetails.ClientName = txtClientName.Text.Trim();
            objclsClientDetails.AccountExecutive = txtAccountExecutive.Text.Trim();
            objclsClientDetails.EmailID = txtEmailID.Text.Trim();

            dt = objclsClientDetails.InsertClients();

            if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "Already Exists")
            {
                ClientScript.RegisterStartupScript(GetType(), "myscript", "alert('Client# Already Exists');", true);
            }
            LoadClientGrid();
        }

        protected void gvClients_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmdSend")
            {
                int index = int.Parse(e.CommandArgument.ToString());
                Label gvlblClientNo = (Label)gvClients.Rows[index].FindControl("gvlblClientNo");
                Label gvlblEmailID = (Label)gvClients.Rows[index].FindControl("gvlblEmailID");
                HiddenField gvhdnAccountExecutiveEmailID = (HiddenField)gvClients.Rows[index].FindControl("gvhdnAccountExecutiveEmailID");

                SendMail(gvlblEmailID.Text.Trim(), gvhdnAccountExecutiveEmailID.Value.Trim(), gvlblClientNo.Text.Trim());
            }
        }

        protected void btnSendLink_Click(object sender, EventArgs e)
        {
            if (ViewState["dtClients"] != null)
            {
                DataTable dt = new DataTable();
                dt = (DataTable)ViewState["dtClients"];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SendMail(dt.Rows[i]["EmailID"].ToString().Trim(), dt.Rows[i]["AccountExecutiveEmailID"].ToString().Trim(), dt.Rows[i]["ClientID"].ToString().Trim());
                }
            }
        }

        private void SendMail(string EmailID, string ccEmailID, string ClientID)
        {
            clsSendMail objclsSendMail = new clsSendMail();

            objclsSendMail.SendMail(EmailID, ccEmailID,"", "MEDICOUNT: END OF MONTH INVOICES-STATEMENTS-SNAPSHOTS", MailBody1(ClientID));
            //objclsSendMail.SendMail(EmailID, "Client Details", "Hi,<br /><br /> Please click the below link and fill the details<br /><br /><a href='" + url + "'>" + url + "</a><br /><br />Thanks,<br />Medicount Team.");

            objclsClientDetails = new clsClientDetails();
            objclsClientDetails.ClientID = int.Parse(ClientID);
            objclsClientDetails.UpdateClients("Send");
        }

        protected void btnExcelExport_Click(object sender, EventArgs e)
        {
            objclsClientDetails = new clsClientDetails();

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            objclsClientDetails.ClientID = 0;

            ds = objclsClientDetails.SelectClientDetails();

            if (ds == null || ds.Tables.Count == 0)
            {
                return;
            }

            dt = ds.Tables[0];
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Clients");

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Client_Details.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }

        }

        private string MailBody1(string ClientID)
        {
            clsCommon objclsCommon = new clsCommon();
            StringBuilder sb = new StringBuilder();

            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            string imagePath = "";
            int lastIndex = url.LastIndexOf("/");
            //imagePath = url.Substring(0, lastIndex) + "/Images/Logo.jpg";
            imagePath = url.Substring(0, lastIndex) + "/Images/";
            url = url.Substring(0, lastIndex) + "/frmClientDetails.aspx?ClientID=" + objclsCommon.Encrypt(ClientID);

            sb.AppendLine("<html><head></head><body>");
            sb.AppendLine("<table border = '1' cellspacing = '0' cellpadding = '0' style = 'border-collapse: collapse; margin-left: 134.7pt; border-style: none; transform: scale(0.788091, 0.788091); transform-origin: left top;' min-scale = '0.7880910683012259'>"); sb.AppendLine("<tbody><tr style = 'height:566.25pt;'>");
            sb.AppendLine("<td valign = 'top' style = 'width:543.3pt;height:566.25pt;padding:0 5.4pt;border:3pt solid #009094;'>");
            sb.AppendLine("<p align = 'center' style = 'font-size:11pt;font-family:Calibri,sans-serif;text-align:center;margin:0;'>");
            sb.AppendLine("<img data-imagetype = 'AttachmentByCid' originalsrc = 'cid:image002.png@611A0A90.001E98B3' data-custom = 'AAMkAGNmNzBjNDUyLTMxOTktNDlkNi05MWI3LTNkMmM2YjQ4OWQ1YgBGAAAAAAA%2B8aw%2BdMZ%2BSpckUHEBZ2e6BwCXJ8bZhveiSKRhV94WGI35AAAAAAEMAACXJ8bZhveiSKRhV94WGI35AACzf2H3AAABEgAQANNFAfYNzjhIpiuNpExuOu4%3D' naturalheight = '0' naturalwidth = '0' src ='" + imagePath + "Logo.jpg' width = '549' height = '140'");
            sb.AppendLine("id='x_Picture 3' crossorigin='use-credentials' style='cursor: pointer; '></p>");
            sb.AppendLine("<p align = 'center' style = 'font-size:11pt;font-family:Calibri,sans-serif;text-align:center;background-color:#009094;margin:0;'>");
            sb.AppendLine("<b><span style = 'color:white;font-size:22pt;'>END OF MONTH: INVOICES-STATEMENTS-SNAPSHOTS</span></b></p>");
            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;margin:0;'> &nbsp;</p>");
            sb.AppendLine("<table align = 'left' border = '0' cellspacing = '0' cellpadding = '0' style = 'width:98.72%;'>");
            sb.AppendLine("<tbody><tr style = 'height:49.75pt;'>");
            sb.AppendLine("<td valign = 'top' style = 'width:100%;height:49.75pt;padding:0;'>");
            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;text-align:justify;margin:0 0 0 9pt;'>");
            sb.AppendLine("<span style = 'font-size:14pt;'> Medicount is going paperless by the end of 2022. There are two primary reasons. First, the United States Postal Service has become slower and less reliable for various reasons. Second, document security is an ongoing concern. Uploading the documents to the appropriate party resolves both issues. </ span></p>");
            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;text-align:justify;margin:0;'>");
            sb.AppendLine("<span style = 'font-size:14pt;'> &nbsp;</span></p>");
            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;text-align:justify;margin:0 0 0 9pt;'>");
            sb.AppendLine("<span style = 'font-size:14pt;'> Also, we would like to send out all payments by ACH, no more checks, again for the timeliness and security.</ span></p>");
            sb.AppendLine("<p align = 'center' style = 'font-size:11pt;font-family:Calibri,sans-serif;text-align:center;margin:0 0 8pt 9pt;line-height:107%;'>");
            sb.AppendLine("<b><span style = 'color:#009091;font-size:16pt;line-height:107%;'> WE NEED YOUR HELP TO MAKE THIS HAPPEN </span></b></p>");
            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;text-align:justify;text-indent:-18pt;margin:0 0 0 45pt;line-height:115%;'>");
            sb.AppendLine("<span style = 'font-size:14pt;font-family:Symbol;line-height:115%;'>" +
                "<img data-imagetype = 'AttachmentByCid' originalsrc = 'cid:image001.gif@611A0A90.001E98B3' data-custom = 'AAMkAGNmNzBjNDUyLTMxOTktNDlkNi05MWI3LTNkMmM2YjQ4OWQ1YgBGAAAAAAA%2B8aw%2BdMZ%2BSpckUHEBZ2e6BwCXJ8bZhveiSKRhV94WGI35AAAAAAEMAACXJ8bZhveiSKRhV94WGI35AACzf2H3AAABEgAQAJDhZNYrF1VPmzhcjDzHMW0%3D' naturalheight = '0' naturalwidth = '0' src ='" + imagePath + "Capture.jpg' width = '16' height = '16' alt = '*' crossorigin = 'use-credentials' style = 'cursor: pointer;'>" +
                "<span style = 'font-size:7pt;font-family:Times New Roman;'> &nbsp; &nbsp;");
            sb.AppendLine("</span></span><span style = 'font-size:14pt;line-height:115%;'> Please provide your bank account information to receive funds</span></p>");
            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;text-align:justify;text-indent:-18pt;margin:0 0 0 45pt;'>");
            sb.AppendLine("<span style = 'font-size:14pt;font-family:Symbol;'>" +
                "<img data-imagetype = 'AttachmentByCid' originalsrc = 'cid:image001.gif@611A0A90.001E98B3' data-custom = 'AAMkAGNmNzBjNDUyLTMxOTktNDlkNi05MWI3LTNkMmM2YjQ4OWQ1YgBGAAAAAAA%2B8aw%2BdMZ%2BSpckUHEBZ2e6BwCXJ8bZhveiSKRhV94WGI35AAAAAAEMAACXJ8bZhveiSKRhV94WGI35AACzf2H3AAABEgAQAJDhZNYrF1VPmzhcjDzHMW0%3D' naturalheight = '0' naturalwidth = '0' src = '" + imagePath + "Capture.jpg'  width = '16' height = '16' alt = '*' crossorigin = 'use-credentials' style = 'cursor: pointer;'>" +
                "<span style = 'font-size:7pt;font-family:Times New Roman;'> &nbsp;&nbsp;");
            sb.AppendLine("</span></span><span style = 'font-size:14pt;'> Please provide a contact person to receive the EOM statements, Snapshots and Supporting Documents</span></p>");
            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;text-align:justify;margin:0 0 0 9pt;'>");
            sb.AppendLine("<span style = 'font-size:14pt;'> &nbsp;</span></p>");
            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;text-align:justify;margin:0 0 0 9pt;'>");
            sb.AppendLine("<span style = 'font-size:14pt;'> We have made it as easy as possible to send us the above information, just click on the link below.This link allows you to fill out the data and send back to us securely.</span></p>");
            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;text-align:justify;margin:0 0 0 36pt;'>");
            sb.AppendLine("<span style = 'font-size:14pt;'> &nbsp;</span></p>");
            sb.AppendLine("<p align='center' style='font-size:11pt;font-family:Calibri,sans-serif;text-align:center;margin:0 0 0 9pt;'>");
            sb.AppendLine("<span style='font-size:14pt;'>If you have any questions, please contact Grace Brunsman, Payments Supervisor,</ span><br>");
            sb.AppendLine("<span style='font-size:14pt;'>Email: gbrunsman@medicount.com & Phone Number: 513-612-3148</ span>");
            sb.AppendLine("</p>");
            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;margin:0;line-height:18.0pt;'>");
            sb.AppendLine("<b><span style = 'font-size:14pt;'> &nbsp;</span></b></p>");
            sb.AppendLine("<p align='center' style = 'font-size:11pt;font-family:Calibri,sans-serif;text-align:center;margin:0;line-height:18.0pt;'>");
            sb.AppendLine("<b><span style = 'font-size:14pt;'><a href='" + url + "'><img data-imagetype = 'AttachmentByCid' originalsrc = 'cid:image003.png@611A0A90.001E98B3' data-custom = 'AAMkAGNmNzBjNDUyLTMxOTktNDlkNi05MWI3LTNkMmM2YjQ4OWQ1YgBGAAAAAAA%2B8aw%2BdMZ%2BSpckUHEBZ2e6BwCXJ8bZhveiSKRhV94WGI35AAAAAAEMAACXJ8bZhveiSKRhV94WGI35AACzf2H3AAABEgAQAE053cP0XRVEvc6N6uWj7NE%3D' naturalheight = '0' naturalwidth = '0' src ='" + imagePath + "Click.jpg'  width = '307' height = '88' id = 'x_Picture 1' crossorigin = 'use-credentials' style = 'cursor: pointer;'></a></span></b></p>");
            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;margin:0;line-height:18.0pt;'>");
            sb.AppendLine("<b><span style = 'font-size:14pt;'> &nbsp;</span></b></p>");
            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;margin:0;line-height:18.0pt;'>");
            sb.AppendLine("<b><span style = 'font-size:14pt;'> Thank you. <br>");
            sb.AppendLine("</span></b><b><span style = 'color:#009094;font-size:16pt;'> Medicount Management, Inc.</span></b></p>");
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</tbody></table>");
            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;margin:0;'></p>");
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</tbody></table>");
            sb.AppendLine("</body></html>");

            sb.AppendLine("<br>");
            return sb.ToString();
        }
        private string MailBody(string ClientID)
        {
            clsCommon objclsCommon = new clsCommon();
            StringBuilder sb = new StringBuilder();

            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            string imagePath = "";
            int lastIndex = url.LastIndexOf("/");
            //imagePath = url.Substring(0, lastIndex) + "/Images/Logo.jpg";
            imagePath = url.Substring(0, lastIndex) + "/Images/";
            url = url.Substring(0, lastIndex) + "/frmClientDetails.aspx?ClientID=" + objclsCommon.Encrypt(ClientID);

            sb.AppendLine("<html><head></head><body>");
            sb.AppendLine("<div style='border-collapse: collapse; margin-left: 134.7pt; border-style: none; transform: scale(0.788091, 0.788091); transform-origin: left top;'>");
            sb.AppendLine("<div style='width:543.3pt;height:566.25pt;padding:0 5.4pt;border:3pt solid #009094;'>");
            sb.AppendLine("<div align='center'>");
            sb.AppendLine("<img src='" + imagePath + "Logo.jpg' />");
            sb.AppendLine("</div>");
            sb.AppendLine("<div align='center' style='padding:2px 0px 2px 0px;  background-color: #009094;font-family: Calibri; font-size:22.0pt; color: white; font-weight:bolder;'>");
            sb.AppendLine("INVOICES-STATEMENT & END OF MONTH SNAPSHOTS");
            sb.AppendLine("</div>");
            sb.AppendLine("<div align='justify' style='padding:5px 20px 10px 20px;  font-family: Calibri;font-size:14.0pt;'>");
            sb.AppendLine("Medicount is attempting to go paperless by the end of the year. There are two primary reasons. First, the United States Postal Service has become slower and less reliable for many reasons. Second, document security is an ongoing concern. Uploading the documents to the appropriate party resolves both issues");
            sb.AppendLine("<br />");
            sb.AppendLine("<br />");
            sb.AppendLine("Simultaneously, we would like to send out all payments by ACH, again for the timeliness and security.");
            sb.AppendLine("</div>");
            sb.AppendLine("<div align='center' style='padding: 2px 0px 2px 0px; font-family: Calibri;font-size: 16.0pt; color:#009091;font-weight:bolder;'>");
            sb.AppendLine("WE NEED YOUR HELP TO MAKE THIS HAPPEN");
            sb.AppendLine("</div>");
            sb.AppendLine("<div align='justify' style='padding:5px 20px 10px 20px;  font-family: Calibri;font-size:14.0pt;'>");
            sb.AppendLine("         <img src='" + imagePath + "Capture.jpg' />  Please provide your bank account information to receive funds");
            sb.AppendLine("<br />");
            sb.AppendLine("         <img src='" + imagePath + "Capture.jpg' />  Please provide a contact person to receive the EOM statements, Snapshots and Supporting Documents");
            sb.AppendLine("<br />");
            sb.AppendLine("<br />");
            sb.AppendLine("We have made it as easy as possible to send us the above information, just click on the link below. This link allows you to fill out the data and send back to us securely.");
            sb.AppendLine("<br />");
            sb.AppendLine("<br />");
            sb.AppendLine("If you have any questions, please contact Grace Brunsman, Payments Supervisor.");
            sb.AppendLine("</div>");
            sb.AppendLine("<div align='center'>");
            sb.AppendLine("<a href='" + url + "'><img src='" + imagePath + "Click.jpg' /></a>");
            sb.AppendLine("</div>");
            sb.AppendLine("<div align='justify' style='padding:5px 20px 0px 20px;  font-family: Calibri;font-size:14.0pt;font-weight:bolder;'>");
            sb.AppendLine("Thank you.");
            sb.AppendLine("</div>");
            sb.AppendLine("<div align='justify' style='padding: 0px 20px 10px 20px; font-family: Calibri;font-size: 16.0pt; color:#009094; font-weight: bolder; '>");
            sb.AppendLine(" Medicount Management, Inc.");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</body></html>");

            return sb.ToString();
        }

       


    }
}