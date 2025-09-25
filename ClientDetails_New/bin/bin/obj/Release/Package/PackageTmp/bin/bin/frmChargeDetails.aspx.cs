using ClientDetails.App_Code;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClientDetails
{
    public partial class frmChargeDetails : System.Web.UI.Page
    {
        clsChargeDetails objclsChargeDetails;
        clsSendMail objclsSendMail;
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadGrid();
            }
        }

        private void LoadGrid()
        {
            objclsChargeDetails = new clsChargeDetails();
            DataTable dt = new DataTable();
            dt = objclsChargeDetails.SelectChargeDetails();

            ViewState["dt"] = dt;
            gvChargeDetails.DataSource = dt;
            gvChargeDetails.DataBind();


        }

        protected void btnSendLink_Click(object sender, EventArgs e)
        {
            objclsChargeDetails = new clsChargeDetails();
            objclsSendMail = new clsSendMail();
             dt = new DataTable();

            if (ViewState["dt"] != null)
            {
                dt = (DataTable)ViewState["dt"];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["IsMailSend"].ToString().Trim().ToUpper() == "NO")
                    {
                        if (File.Exists(Server.MapPath("~/Images/" + dt.Rows[i]["FileName"].ToString().Trim())))
                        {
                            //objclsSendMail.SendMail(dt.Rows[i]["ChiefEmail"].ToString().Trim(), "", "arengasamy@medicount.com", "MEDICOUNT: YOUR CURRENT CHARGE RATE VERIFICATION AUDIT REQUEST", MailBody(dt.Rows[i]["ID"].ToString().Trim(), dt.Rows[i]["FileName"].ToString().Trim()));
                            //objclsSendMail.SendMail(dt.Rows[i]["ChiefEmail"].ToString().Trim(), "", "arengasamy@medicount.com", "MEDICOUNT: YOUR CURRENT CHARGE RATE VERIFICATION AUDIT REQUEST - 2nd Notice", MailBody(dt.Rows[i]["ID"].ToString().Trim(), dt.Rows[i]["FileName"].ToString().Trim()));
                            objclsSendMail.SendMail(dt.Rows[i]["ChiefEmail"].ToString().Trim(), "", "arengasamy@medicount.com", "MEDICOUNT: YOUR CURRENT CHARGE RATE VERIFICATION AUDIT REQUEST - Final Notice", MailBody(dt.Rows[i]["ID"].ToString().Trim(), dt.Rows[i]["FileName"].ToString().Trim()));
                            objclsChargeDetails.UpdateChargeDetails("MailStatus", int.Parse(dt.Rows[i]["ID"].ToString().Trim()));
                        }
                    }
                }
                LoadGrid();
            }
        }

        protected void btnExcelExport_Click(object sender, EventArgs e)
        {
            dt = new DataTable();
            if (ViewState["dt"] != null)
            {
                dt = (DataTable)ViewState["dt"];
                DataView dv = new System.Data.DataView(dt);

                dt = dv.ToTable("Selected", false, "CompanyCode", "CompanyName", "FileName", "ChiefName", "ChiefEmail", "AEName", "AEEmail", "IsMailSend", "Status");


                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "ChargeRate");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=ChargeRate_ClientStatus.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }

            }

        private string MailBody(string ID, string FileName)
        {
            clsCommon objclsCommon = new clsCommon();
            StringBuilder sb = new StringBuilder();

            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            string imagePath = "";
            int lastIndex = url.LastIndexOf("/");
            //imagePath = url.Substring(0, lastIndex) + "/Images/Logo.jpg";
            imagePath = url.Substring(0, lastIndex) + "/Images/";
            url = url.Substring(0, lastIndex) + "/frmChargeDetailsApproval.aspx?Code=" + objclsCommon.Encrypt(ID) + "&Status=";

            sb.AppendLine("<html><head></head><body>");
            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;margin:0;line-height:18.0pt;'>");
            sb.AppendLine("<b><span style = 'font-size:14pt;'> &nbsp;</span></b></p>");
            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;margin:0;line-height:18.0pt;'>");
            sb.AppendLine("<span style = 'font-size:14pt;'>One of Medicount’s SOC Audit requirements is reviewing our client’s transport charge rates regularly. Below are the transport charge rates currently on file for submitting your claims to insurance providers. If there are no charge amount number next to the “Charge Type” it is non-applicable.<br><br></span>");
            sb.AppendLine("<span style = 'font-size:14pt;'>Please verify the rates, and if correct, click on the approval button if not correct click on the rejected button. If you click on the rejected button your account executive will contact, you in the next few days.<br><br></span>");
            sb.AppendLine("<span style = 'font-size:14pt;'>If you want to change your rates, please contact your account executive directly. <br><br></span>");
            sb.AppendLine("<span style = 'font-size:14pt;'><b>THIS IS THE THIRD AND FINAL NOTICE YOU WILL RECEIVE. MEDICOUNT WILL ASSUME THAT YOUR CHARGE RATES ARE CORRECT, AS SHOWN BELOW.</b><br><br></span>");


            sb.AppendLine("<table border = '1' cellspacing = '0' cellpadding = '0' style = 'border-collapse: collapse; margin-left: 134.7pt; border-style: none; transform: scale(0.788091, 0.788091); transform-origin: left top;' min-scale = '0.7880910683012259'>");
            sb.AppendLine("<tbody><tr style = 'height:566.25pt;'>");
            sb.AppendLine("<td valign = 'top' style = 'width:543.3pt;height:566.25pt;padding:0 5.4pt;border:3pt solid #009094;'>");
            sb.AppendLine("<p align = 'center' style = 'font-size:11pt;font-family:Calibri,sans-serif;text-align:center;margin:0;'>");
            sb.AppendLine("<img data-imagetype = 'AttachmentByCid' originalsrc = 'cid:image002.png@611A0A90.001E98B3' data-custom = 'AAMkAGNmNzBjNDUyLTMxOTktNDlkNi05MWI3LTNkMmM2YjQ4OWQ1YgBGAAAAAAA%2B8aw%2BdMZ%2BSpckUHEBZ2e6BwCXJ8bZhveiSKRhV94WGI35AAAAAAEMAACXJ8bZhveiSKRhV94WGI35AACzf2H3AAABEgAQANNFAfYNzjhIpiuNpExuOu4%3D' naturalheight = '0' naturalwidth = '0' src ='" + imagePath + "Logo.png' width = '549' height = '140'");
            sb.AppendLine("id='x_Picture 3' crossorigin='use-credentials' style='cursor: pointer; '></p>");
            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;margin:0;'> &nbsp;</p>");
            sb.AppendLine("<p align='center' style='margin: 0in; text - align:center; background:#009094'><b><span style='font-size:36.0pt;color:white'>THIRD &amp; FINAL NOTICE</span></b><o:p></o:p></p>");

            sb.AppendLine("<table align = 'left' border = '0' cellspacing = '0' cellpadding = '0' style = 'width:98.72%;'>");
            sb.AppendLine("<tbody><tr style = 'height:49.75pt;'>");
            sb.AppendLine("<td valign = 'top' style = 'width:100%;height:49.75pt;padding:0;'>");

                 
            //sb.AppendLine("<b><span style = 'font-size:14pt;'><img naturalheight = '0' naturalwidth = '0' src ='" + imagePath + "2ndNotice.png' style = 'cursor: pointer;'width='1000' height='60'></span></b>");
            sb.AppendLine("<b><span style = 'font-size:14pt;'><img naturalheight = '0' naturalwidth = '0' src ='" + imagePath + FileName + "' style = 'cursor: pointer;'width='1000' height='100%'></span></b>");
            sb.AppendLine("<p align='center' style = 'font-size:11pt;font-family:Calibri,sans-serif;text-align:center;margin:0;line-height:15.0pt;'>");

            sb.AppendLine("<b><span style = 'font-size:10pt;'><a href='" + url + "approved' traget='_self'><img naturalheight = '0' naturalwidth = '0' src ='" + imagePath + "Approved.png' style = 'cursor: pointer;'  width='200' height='70'></a></span></b>");
            sb.AppendLine("<b><span style = 'font-size:10pt;'><a href='" + url + "rejected'><img naturalheight = '0' naturalwidth = '0' src ='" + imagePath + "Rejected.png' style = 'cursor: pointer;'  width='200' height='70'></a></span></b></p>");

            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</tbody></table>");
            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;margin:0;'></p>");
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</tbody></table>");


            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;margin:0;line-height:18.0pt;'>");
            sb.AppendLine("<b><span style = 'font-size:14pt;'> &nbsp;</span></b></p>");
            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;margin:0;line-height:18.0pt;'>");
            sb.AppendLine("<b><span style = 'font-size:14pt;'> Thank you. <br>");
            sb.AppendLine("</span></b><b><span style = 'color:#009094;font-size:16pt;'> Medicount Management, Inc.</span></b></p>");

            sb.AppendLine("</body></html>");

            sb.AppendLine("<br>");
            return sb.ToString();
        }
    }
}