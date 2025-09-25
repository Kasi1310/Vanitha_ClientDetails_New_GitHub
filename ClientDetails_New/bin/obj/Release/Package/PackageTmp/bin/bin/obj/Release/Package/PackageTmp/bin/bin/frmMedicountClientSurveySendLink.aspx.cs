using ClientDetails.App_Code;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClientDetails
{
    public partial class frmMedicountClientSurveySendLink : System.Web.UI.Page
    {
        clsMedicountClientSurvey objclsMedicountClientSurvey;
        DataTable dt;

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                LoadGrid();
            }
        }

        private void LoadGrid()
        {
            objclsMedicountClientSurvey = new clsMedicountClientSurvey();
            dt = new DataTable();
            dt = objclsMedicountClientSurvey.SelectClientDetails();

            ViewState["dt"] = dt;

            gvMedicountClientSurvey.DataSource = dt;
            gvMedicountClientSurvey.DataBind();
        }

        protected void btnSendLink_Click(object sender, EventArgs e)
        {
            clsSendMail objclsSendMail = new clsSendMail();

            dt = new DataTable();
            dt = (DataTable)ViewState["dt"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if(dt.Rows[i]["MailSend"].ToString().Trim().ToUpper()=="NO" && dt.Rows[i]["SurveyFilled"].ToString().Trim().ToUpper() == "NO")
                {
                    if(objclsSendMail.SendMail(dt.Rows[i]["Email"].ToString().Trim(), "", "arengasamy@medicount.com", "MEDICOUNT MANAGEMENT, INC. Your Feedback Shapes Our Future: Annual Client Survey Inside", MailBody(dt.Rows[i]["ID"].ToString().Trim())));
                    {
                        objclsMedicountClientSurvey = new clsMedicountClientSurvey();
                        objclsMedicountClientSurvey.ID = int.Parse(dt.Rows[i]["ID"].ToString().Trim());
                        objclsMedicountClientSurvey.UpdateMailSendStatus();
                    }
                }
            }

            LoadGrid();
        }

        protected void btnExcelExport_Click(object sender, EventArgs e)
        {
            objclsMedicountClientSurvey = new clsMedicountClientSurvey();

            DataTable dt = new DataTable();

            dt = objclsMedicountClientSurvey.SelectMedicountClientSurvey();

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Medicount_Client_Survey");

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

        private string MailBody(string ID)
        {
            StringBuilder sb = new StringBuilder();

            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            string imagePath = "";
            int lastIndex = url.LastIndexOf("/");
            //imagePath = url.Substring(0, lastIndex) + "/Images/Logo.jpg";

            url = url.Substring(0, lastIndex);
            lastIndex = url.LastIndexOf("/");

            url = "https://snapshots.medicount.com/frmMedicountClientSurvey.aspx?ID=" + System.Web.HttpUtility.UrlEncode(ID.ToString()); //CGCipher.Encrypt(ID.ToString(), "");


            imagePath = "https://snapshots.medicount.com/Images/";

            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("<meta http-equiv=Content-Type content='text / html; charset = windows - 1252'>");
            sb.AppendLine("<meta name=Generator content='Microsoft Word 15(filtered)'>");
            sb.AppendLine("<style>");
            sb.AppendLine("@font-face {");
            sb.AppendLine("font-family: 'Cambria Math';");
            sb.AppendLine("panose-1: 2 4 5 3 5 4 6 3 2 4;");
            sb.AppendLine("}");
            sb.AppendLine("");
            sb.AppendLine("@font-face {");
            sb.AppendLine("font-family: Calibri;");
            sb.AppendLine("panose-1: 2 15 5 2 2 2 4 3 2 4;");
            sb.AppendLine("}");
            sb.AppendLine("p.MsoNormal, li.MsoNormal, div.MsoNormal {");
            sb.AppendLine("margin-top: 0in;");
            sb.AppendLine("margin-right: 0in;");
            sb.AppendLine("margin-bottom: 0in;");
            sb.AppendLine("margin-left: 0in;");
            sb.AppendLine("line-height: 107%;");
            sb.AppendLine("font-size: 11.0pt;");
            sb.AppendLine("font-family: 'Calibri',sans-serif;");
            sb.AppendLine("}");
            sb.AppendLine("");
            sb.AppendLine(".MsoChpDefault {");
            sb.AppendLine("font-family: 'Calibri',sans-serif;");
            sb.AppendLine("}");
            sb.AppendLine("");
            sb.AppendLine(".MsoPapDefault {");
            sb.AppendLine("margin-bottom: 0in;");
            sb.AppendLine("line-height: 107%;");
            sb.AppendLine("}");
            sb.AppendLine("");
            sb.AppendLine("@page WordSection1 {");
            sb.AppendLine("size: 8.5in 11.0in;");
            sb.AppendLine("margin: 0in 0in 0in 0in;");
            sb.AppendLine("}");
            sb.AppendLine("");
            sb.AppendLine("div.WordSection1 {");
            sb.AppendLine("page: WordSection1;");
            sb.AppendLine("}");
            sb.AppendLine("</style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body lang=EN-US style='word-wrap:break-word;'>");
            sb.AppendLine("<div class=WordSection1>");
            sb.AppendLine("<center>");
            sb.AppendLine("<table class=MsoTableGrid border=1 cellspacing=0 cellpadding=0 style='border-collapse:collapse;border:none;width:860px;'>");
            sb.AppendLine("<tr style='height:390pt'>");
            sb.AppendLine("<td width=628 valign=top style='width:471.25pt;border:solid #009094 3.0pt;padding:0in 5.4pt 0in 5.4pt;height:390pt'>");
            sb.AppendLine("<p class=MsoNormal align=center style='margin-bottom:0in;text-align:center;line-height:normal'>");
            sb.AppendLine("<img width=548 height=147 id='Picture 1' src='" + imagePath + "Medicount_Logo_Quick.png'>");
            sb.AppendLine("</p>");

            sb.AppendLine("<p class=MsoNormal style='margin-bottom: 0in; text-align: center; line-height: normal;font-weight:bold; background-color: #009094; color:white;'>");
            sb.AppendLine("<span style='font-size:24.0pt'>");
            sb.AppendLine("MEDICOUNT CLIENT SATISFACTION SURVEY");
            sb.AppendLine("</span>");
            sb.AppendLine("</p>");
            sb.AppendLine("<p class=MsoNormal align=center style='margin-bottom:0in;text-align:center; line-height:normal'><span style='font-size:10.0pt'>&nbsp;</span></p>");
            sb.AppendLine("<p class=MsoNormal style='margin-bottom:0in;text-align:justify; line-height:normal'>");
            sb.AppendLine("<span style='font-size:16.0pt;'>");
            sb.AppendLine("Thank you for your ongoing partnership with <span style='color: #009094;font-weight:bold;'>Medicount Management.</span> We prioritize the well-being of our clients and their citizens and consistently strive to offer exemplary customer service to all.");
            sb.AppendLine("</span>");
            sb.AppendLine("</p>");
            sb.AppendLine("<p class=MsoNormal align=center style='margin-bottom:0in;text-align:center; line-height:normal'><span style='font-size:10.0pt'>&nbsp;</span></p>");
            sb.AppendLine("<p class=MsoNormal style='margin-bottom:0in;text-align:justify; line-height:normal'>");
            sb.AppendLine("<span style='font-size:16.0pt;'>");
            sb.AppendLine("We kindly ask you to participate in our annual <span style='color: #009094;font-weight:bold;'>Client Satisfaction Survey.</span> Your insights are invaluable to us, helping us recognize our strengths and areas for improvement. Completing the survey will only take about 3 minutes.");
            sb.AppendLine("</span>");
            sb.AppendLine("</p>");
            sb.AppendLine("<p class=MsoNormal align=center style='margin-bottom:0in;text-align:center; line-height:normal'><span style='font-size:10.0pt'>&nbsp;</span></p>");
            sb.AppendLine("<p class=MsoNormal style='margin-bottom:0in;text-align:justify; line-height:normal'>");
            sb.AppendLine("<span style='font-size:16.0pt;'>");
            sb.AppendLine("In gratitude for your time and feedback, you will be entered into a drawing for a chance to <span style='color: #009094;font-weight:bold;'>win a $250.00</span> gift card. If you prefer not to accept the gift card, we will gladly donate in your name to a charity of your choice.");
            sb.AppendLine("</span>");
            sb.AppendLine("</p>");
            sb.AppendLine("<p class=MsoNormal align=center style='margin-bottom:0in;text-align:center; line-height:normal'><span style='font-size:10.0pt'>&nbsp;</span></p>");
            sb.AppendLine("<p class=MsoNormal style='margin-bottom:0in;text-align:justify; line-height:normal'>");
            sb.AppendLine("<span style='font-size:16.0pt;'>");
            sb.AppendLine("You can access and complete the survey by <span style='color: #009094;font-weight:bold;'>October 20, 2023.</span>");
            sb.AppendLine("</span>");
            sb.AppendLine("</p>");
            sb.AppendLine("<p class=MsoNormal align=center style='margin-bottom:0in;text-align:center; line-height:normal'><span style='font-size:10.0pt'>&nbsp;</span></p>");
            sb.AppendLine("<p class=MsoNormal align=center style='margin-bottom:0in;text-align:center; line-height:normal'>");
            sb.AppendLine("<a target='_blank' href='" + url + "'");
            sb.AppendLine("<span style='font-size:16.0pt;color:windowtext;text-decoration:none'>");
            sb.AppendLine("<img border=0 width=266 height=44 id='Picture 2' src='" + imagePath + "QuickSurvey.png'>");
            sb.AppendLine("</span>");
            sb.AppendLine("</a>");
            sb.AppendLine("</p>");
            sb.AppendLine("<p class=MsoNormal align=center style='margin-bottom:0in;text-align:center; line-height:normal'><span style='font-size:10.0pt'>&nbsp;</span></p>");
            sb.AppendLine("<p class=MsoNormal style='margin-bottom:0in;text-align:justify; line-height:normal'>");
            sb.AppendLine("<span style='font-size:16.0pt;'>");
            sb.AppendLine("We appreciate you taking the time to share your feedback. Always remember that Medicount is your trusted partner in EMS Billing.");
            sb.AppendLine("</span>");
            sb.AppendLine("</p>");
            sb.AppendLine("<p class=MsoNormal align=center style='margin-bottom:0in;text-align:center; line-height:normal'><span style='font-size:10.0pt'>&nbsp;</span></p>");
            sb.AppendLine("<p class=MsoNormal style='margin-bottom:0in;text-align:justify; line-height:normal'>");
            sb.AppendLine("<span style='font-size:16.0pt;'>");
            sb.AppendLine("If you have questions, don't hesitate to contact your account executive.");
            sb.AppendLine("</span>");
            sb.AppendLine("</p>");
            sb.AppendLine("");
            sb.AppendLine("<p class=MsoNormal align=center style='margin-bottom:0in;text-align:center; line-height:normal'><span style='font-size:10.0pt'>&nbsp;</span></p>");
            sb.AppendLine("<p class=MsoNormal style='margin-bottom: 0in; text-align: center; line-height: normal;'>");
            sb.AppendLine("");
            sb.AppendLine("<table align='center' border='1' cellspacing='0' style='font-size:14.0pt;width:700px;'>");
            sb.AppendLine("<tr style='font-weight: bold; background-color: #009094; color: white; padding: 0in 0in 0in 0in;'>");
            sb.AppendLine("<td align='center'>AE Name</td>");
            sb.AppendLine("<td align='center'>Phone Number</td>");
            sb.AppendLine("<td align='center'>Email Address</td>");
            sb.AppendLine("</tr>");

            


            sb.AppendLine("<tr><td align='center'>Debby Pifer</td><td align='center'>513-801-1183</td><td align='center'><a href='#' style='text-decoration: none; color:#000000' name='myname'>dpifer@medicount.com</a></td></tr>");
            sb.AppendLine("<tr><td align='center'>Heath Smedley</td><td align='center'>859-307-0629</td><td align='center'><a href='#' style='text-decoration: none; color:#000000' name='myname'>hsmedley@medicount.com</a></td></tr>");
            sb.AppendLine("<tr><td align='center'>Joshua Russell</td><td align='center'>317-313-1090</td><td align='center'><a href='#' style='text-decoration: none; color:#000000' name='myname'>jrussell@medicount.com</a></td></tr>");
            sb.AppendLine("<tr><td align='center'>Michelle Davis</td><td align='center'>440-463-9853</td><td align='center'><a href='#' style='text-decoration: none; color:#000000' name='myname'>mdavis@medicount.com</a></td></tr>");
            sb.AppendLine("<tr><td align='center'>Ted Jennings</td><td align='center'>513-592-1584</td><td align='center'><a href='#' style='text-decoration: none; color:#000000' name='myname'>tjennings@medicount.com</a></td></tr>");
            sb.AppendLine("<tr><td align='center'>Tim Newcomb</td><td align='center'>513-225-5223</td><td align='center'><a href='#' style='text-decoration: none; color:#000000' name='myname'>tnewcomb@medicount.com</a></td></tr>");
            sb.AppendLine("</table>");
            sb.AppendLine("</p>");
            sb.AppendLine("<p class=MsoNormal align=center style='margin-bottom:0in;text-align:center; line-height:normal'><span style='font-size:10.0pt'>&nbsp;</span></p>");
            sb.AppendLine("<p class=MsoNormal align=center style='margin-bottom:0in;text-align:left; line-height:normal'>");
            sb.AppendLine("<b>");
            sb.AppendLine("<span style='font-size:14.0pt;'>");
            sb.AppendLine("Thank you.");
            sb.AppendLine("</span>");
            sb.AppendLine("</b>");
            sb.AppendLine("</p>");
            sb.AppendLine("<p class='MsoNormal' style='line-height:normal'><b><span style='font-size:14.0pt;color: #009094'>Medicount Management, Inc.</span></b></p>");

            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</table>");
            sb.AppendLine("</center>");
            //sb.AppendLine("<p class='MsoNormal'>&nbsp;</p>");
            sb.AppendLine("</div>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            string mailContent= sb.ToString();

            const string pattern = "HYPERLINK \"([^\"]+)\"";

            return Regex.Replace(mailContent, pattern, "");
        }
        private string MailBody1(string ID)
        {
            StringBuilder sb = new StringBuilder();

            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            string imagePath = "";
            int lastIndex = url.LastIndexOf("/");
            //imagePath = url.Substring(0, lastIndex) + "/Images/Logo.jpg";

            url = url.Substring(0, lastIndex);
            lastIndex = url.LastIndexOf("/");

            url = "https://snapshots.medicount.com/frmMedicountClientSurvey.aspx?ID=" + System.Web.HttpUtility.UrlEncode(ID.ToString()); //CGCipher.Encrypt(ID.ToString(), "");


            imagePath = "https://snapshots.medicount.com/Images/";

            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("<meta http-equiv=Content-Type content='text / html; charset = windows - 1252'>");
            sb.AppendLine("<meta name=Generator content='Microsoft Word 15(filtered)'>");
            sb.AppendLine("<style>");
            sb.AppendLine("@font-face {");
            sb.AppendLine("font-family: 'Cambria Math';");
            sb.AppendLine("panose-1: 2 4 5 3 5 4 6 3 2 4;");
            sb.AppendLine("}");
            sb.AppendLine("");
            sb.AppendLine("@font-face {");
            sb.AppendLine("font-family: Calibri;");
            sb.AppendLine("panose-1: 2 15 5 2 2 2 4 3 2 4;");
            sb.AppendLine("}");
            sb.AppendLine("p.MsoNormal, li.MsoNormal, div.MsoNormal {");
            sb.AppendLine("margin-top: 0in;");
            sb.AppendLine("margin-right: 0in;");
            sb.AppendLine("margin-bottom: 0in;");
            sb.AppendLine("margin-left: 0in;");
            sb.AppendLine("line-height: 107%;");
            sb.AppendLine("font-size: 11.0pt;");
            sb.AppendLine("font-family: 'Calibri',sans-serif;");
            sb.AppendLine("}");
            sb.AppendLine("");
            sb.AppendLine(".MsoChpDefault {");
            sb.AppendLine("font-family: 'Calibri',sans-serif;");
            sb.AppendLine("}");
            sb.AppendLine("");
            sb.AppendLine(".MsoPapDefault {");
            sb.AppendLine("margin-bottom: 0in;");
            sb.AppendLine("line-height: 107%;");
            sb.AppendLine("}");
            sb.AppendLine("");
            sb.AppendLine("@page WordSection1 {");
            sb.AppendLine("size: 8.5in 11.0in;");
            sb.AppendLine("margin: 0in 0in 0in 0in;");
            sb.AppendLine("}");
            sb.AppendLine("");
            sb.AppendLine("div.WordSection1 {");
            sb.AppendLine("page: WordSection1;");
            sb.AppendLine("}");
            sb.AppendLine("</style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body lang=EN-US style='word-wrap:break-word;'>");
            sb.AppendLine("<div class=WordSection1>");
            sb.AppendLine("<center>");
            sb.AppendLine("<table class=MsoTableGrid border=1 cellspacing=0 cellpadding=0 style='border-collapse:collapse;border:none;'>");
            sb.AppendLine("<tr style='height:390pt'>");
            sb.AppendLine("<td width=628 valign=top style='width:471.25pt;border:solid #009094 3.0pt;padding:0in 5.4pt 0in 5.4pt;height:390pt'>");
            sb.AppendLine("<p class=MsoNormal align=center style='margin-bottom:0in;text-align:center;line-height:normal'>");
            sb.AppendLine("<img width=548 height=147 id='Picture 1' src='" + imagePath + "Medicount_Logo_Quick.png'>");
            sb.AppendLine("</p>");

            sb.AppendLine("<p class=MsoNormal style='margin-bottom:0in;text-align:left; line-height:normal'>");
            sb.AppendLine("<span style='font-size:14.0pt'>");
            sb.AppendLine("Dear Valued Client,");
            sb.AppendLine("</span>");
            sb.AppendLine("</p>");

            sb.AppendLine("<p class=MsoNormal align=center style='margin-bottom:0in;text-align:center; line-height:normal'><span style='font-size:10.0pt'>&nbsp;</span></p>");

            sb.AppendLine("<p class=MsoNormal style='margin-bottom:0in;text-align:justify; line-height:normal'>");
            sb.AppendLine("<span style='font-size:14.0pt;'>");
            sb.AppendLine("At Medicount, our commitment to excellence is driven by the invaluable insights we receive from our clients. As we strive to enhance our EMS Billing services, your feedback plays a pivotal role. By taking a few moments to complete our annual survey, you help us understand our strengths and areas of improvement. This collaborative effort ensures we meet your expectations and continually adapt to your needs. We genuinely appreciate your partnership and look forward to hearing your thoughts.");
            sb.AppendLine("</span>");
            sb.AppendLine("</p>");


            sb.AppendLine("<p class=MsoNormal align=center style='margin-bottom:0in;text-align:center; line-height:normal'><span style='font-size:14.0pt'>&nbsp;</span></p>");
            sb.AppendLine("<p class=MsoNormal align=center style='margin-bottom:0in;text-align:center; line-height:normal'>");
            sb.AppendLine("<a target='_blank' href='" + url + "'");
            sb.AppendLine("<span style='font-size:36.0pt;color:windowtext;text-decoration:none'>");
            sb.AppendLine("<img border=0 width=266 height=44 id='Picture 2' src='" + imagePath + "QuickSurvey.png'>");
            sb.AppendLine("</span>");
            sb.AppendLine("</a>");
            sb.AppendLine("</p>");
            sb.AppendLine("<p class=MsoNormal align=center style='margin-bottom:0in;text-align:center; line-height:normal'><span style='font-size:14.0pt'>&nbsp;</span></p>");

            sb.AppendLine("<p class=MsoNormal align=center style='margin-bottom:0in;text-align:left; line-height:normal'>");
            sb.AppendLine("<b>");
            sb.AppendLine("<span style='font-size:14.0pt;'>");
            sb.AppendLine("Thank you.");
            sb.AppendLine("</span>");
            sb.AppendLine("</b>");
            sb.AppendLine("</p>");
            sb.AppendLine("<p class='MsoNormal' style='line-height:normal'><b><span style='font-size:14.0pt;color: #009094'>Medicount Management, Inc.</span></b></p>");
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</table>");
            sb.AppendLine("</center>");
            //sb.AppendLine("<p class='MsoNormal'>&nbsp;</p>");
            sb.AppendLine("</div>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }
    }
}