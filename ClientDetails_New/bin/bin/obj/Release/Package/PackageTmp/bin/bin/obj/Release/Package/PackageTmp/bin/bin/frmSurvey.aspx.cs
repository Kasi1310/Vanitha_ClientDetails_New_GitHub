using ClientDetails.App_Code;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClientDetails
{
    public partial class frmSurvey : System.Web.UI.Page
    {
        clsSurvey objclsSurvey;
        DataTable dt;
        bool IsSurveyFilled;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //try
                //{
                IsSurveyFilled = false;

                if (Request.QueryString["ID"] != null)
                {
                    hdnAttendeesID.Value = Request.QueryString["ID"].ToString();
                    //CGCipher.Decrypt(Request.QueryString["ID"].ToString(), "");

                    objclsSurvey = new clsSurvey();
                    dt = new DataTable();
                    objclsSurvey.AttendeesID = int.Parse(hdnAttendeesID.Value);
                    //if (Request.QueryString["Mode"] != null) //For testing
                    //{
                    //    dt = objclsSurvey.CheckSurveyFilledTest();
                    //}
                    //else
                    //{
                        dt = objclsSurvey.CheckSurveyFilled();
                    //}
                    if (dt == null)
                    {
                        lblMessage.Style.Add("color", "red");
                        lblMessage.Text = "Invalid Link";
                        divControls.Visible = false;
                        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "alert('Invalid Link');window.close();", true);
                    }
                    else if (dt.Rows.Count > 0 && Convert.ToBoolean(dt.Rows[0]["IsSurveyFilled"].ToString()))
                    {
                        IsSurveyFilled = true;

                        lblMessage.Style.Add("color", "red");
                        lblMessage.Text = "Survey already filled";
                        divControls.Visible = false;
                        this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "alert('Survey already filled');window.close();", true);
                    }

                }
                else
                {
                    lblMessage.Style.Add("color", "red");
                    lblMessage.Text = "Invalid Link";
                    divControls.Visible = false;
                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "alert('Invalid Link');window.close();", true);
                }
                //}
                //catch (Exception ex)
                //{
                //    throw new Exception(ex.Message);
                //}
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!IsSurveyFilled)
            {
                string SurveyCCMailID = "";
                string SurveyBCCMailID = "";

                DataTable dt = new DataTable();
                objclsSurvey = new clsSurvey();
                objclsSurvey.AttendeesID = int.Parse(hdnAttendeesID.Value.Trim());
                objclsSurvey.BillingActivity = rdoBillingActivity.SelectedValue.Trim();
                objclsSurvey.AnswerAllQuestion = rdoAnswerAllQuestion.SelectedValue.Trim();
                objclsSurvey.MeetingRevenueExpectation = rdoMeetingRevenueExpectation.SelectedValue.Trim();
                objclsSurvey.Comments = txtComment.Text.Trim();


                //if (Request.QueryString["Mode"] != null)
                //{
                //    dt = objclsSurvey.InsertSurveyTest();
                //}
                //else
                //{
                    dt = objclsSurvey.InsertSurvey();

                    SurveyCCMailID = ConfigurationManager.AppSettings["survey.cc.mail"].ToString();
                    SurveyBCCMailID = ConfigurationManager.AppSettings["survey.bcc.mail"].ToString();
                //}
                
                


                clsSendMail objclsSendMail = new clsSendMail();
                objclsSendMail.SendMail(dt.Rows[0]["AEMailID"].ToString().Trim(), SurveyCCMailID, SurveyBCCMailID, "Client Review Meeting Response", MailBody(dt.Rows[0]["AEName"].ToString().Trim(), dt.Rows[0]["AttendeeName"].ToString().Trim(), dt.Rows[0]["ClientName"].ToString().Trim(), dt.Rows[0]["ClientNo"].ToString().Trim()));

                lblMessage.Style.Add("color", "green");
                lblMessage.Text = "Thank You!";
                divControls.Visible = false;

                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Close", "window.close();", true);

                //this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "window.close();", true);
            }
            //Page.ClientScript.RegisterOnSubmitStatement(typeof(Page), "closePage", "window.onunload = CloseWindow();");


            //ClientScript.RegisterStartupScript(typeof(Page), "closePage", "<script type='text/JavaScript'>window.close();</script>");

            //ClientScript.RegisterStartupScript(typeof(Page), "closePage", "window.close();", true);

            // Response.Write("<script>javascript:window.close();</script>");

            //this.ClientScript.RegisterClientScriptBlock(this.GetType(), "Close", "window.close()", true);

            //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "CloseWindow();", true);//this will dispaly the alert box


            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Close_Window", "CloseWindow();", true);//this will close the page on button click




        }



        private string MailBody(string AEName, string AttendeeName, string ClientName, string ClientNo)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("<meta charset='utf-8' />");
            sb.AppendLine("<title></title>");
            sb.AppendLine("<style>.paraDesign {margin: 0in;font-size: 11.0pt;font-family: Calibri;}</style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("<table border='0' cellpadding='0' width='741' style='width: 556.1pt; transform: scale(0.977887, 0.977887); transform-origin: left top;' min-scale='0.9778869778869779'>");
            sb.AppendLine("<tbody>");
            sb.AppendLine("<tr style='height:8.15pt'>");
            sb.AppendLine("<td style='padding:.75pt .75pt .75pt .75pt; height:8.15pt'>");
            sb.AppendLine("<p class='paraDesign'><span style='font-size:14.0pt'>Hi " + AEName + ", </span></p>");
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr style='height:8.15pt'>");
            sb.AppendLine("<td style='padding:7.5pt 0in 7.5pt 0in; height:8.15pt'>");
            sb.AppendLine("<p class='paraDesign'>");
            sb.AppendLine("<span style='font-size:14.0pt'>Please see the below response from the client.</span>");
            sb.AppendLine("</p><p class='paraDesign' aria-hidden='true'>&nbsp;</p>");
            sb.AppendLine("<table class='x_MsoTable15Grid1LightAccent1' border='1' cellspacing='0' cellpadding='0' style='border-collapse:collapse; border:none'>");
            sb.AppendLine("<tbody>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td width='177' valign='top' style='width:133.1pt; border:solid #B4C6E7 1.0pt; border-bottom:solid #8EAADB 1.5pt; padding:0in 5.4pt 0in 5.4pt'>");
            sb.AppendLine("<p class='paraDesign'><b><span style='font-size:14.0pt'>Name</span></b></p>");
            sb.AppendLine("</td>");
            sb.AppendLine("<td width='246' valign='top' style='width:184.5pt; border-top:solid #B4C6E7 1.0pt; border-left:none; border-bottom:solid #8EAADB 1.5pt; border-right:solid #B4C6E7 1.0pt; padding:0in 5.4pt 0in 5.4pt'>");
            sb.AppendLine("<p class='paraDesign'><b><span style='font-size:14.0pt'>" + AttendeeName + "</span></b></p>");
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr style='height:4.35pt'>");
            sb.AppendLine("<td width='177' valign='top' style='width:133.1pt; border:solid #B4C6E7 1.0pt; border-top:none; padding:0in 5.4pt 0in 5.4pt; height:4.35pt'>");
            sb.AppendLine("<p class='paraDesign'><b><span style='font-size:14.0pt'>Name of the Client</span></b></p>");
            sb.AppendLine("</td>");
            sb.AppendLine("<td width='246' valign='top' style='width:184.5pt; border-top:none; border-left:none; border-bottom:solid #B4C6E7 1.0pt; border-right:solid #B4C6E7 1.0pt; padding:0in 5.4pt 0in 5.4pt; height:4.35pt'>");
            sb.AppendLine("<p class='paraDesign'><span style='font-size:14.0pt'>" + ClientName + "</span></p>");
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td width='177' valign='top' style='width:133.1pt; border:solid #B4C6E7 1.0pt; border-top:none; padding:0in 5.4pt 0in 5.4pt'>");
            sb.AppendLine("<p class='paraDesign'><b><span style='font-size:14.0pt'>Client #</span></b></p>");
            sb.AppendLine("</td>");
            sb.AppendLine("<td width='246' valign='top' style='width:184.5pt; border-top:none; border-left:none; border-bottom:solid #B4C6E7 1.0pt; border-right:solid #B4C6E7 1.0pt; padding:0in 5.4pt 0in 5.4pt'>");
            sb.AppendLine("<p class='paraDesign'><span style='font-size:14.0pt'>" + ClientNo + "</span></p>");
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</tbody>");
            sb.AppendLine("</table><p class='paraDesign' aria-hidden='true'>&nbsp;</p>");
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr style='height:.7in'>");
            sb.AppendLine("<td style='padding:.75pt .75pt .75pt .75pt; height:.7in'>");
            sb.AppendLine("<table class='x_MsoTable15Grid4Accent1' border='1' cellspacing='0' cellpadding='0' width='809' style='width:606.45pt; margin-left:.15pt; border-collapse:collapse; border:none'>");
            sb.AppendLine("<tbody>");
            sb.AppendLine("<tr style='height:16.2pt'>");
            sb.AppendLine("<td width='572' style='width: 429.15pt; border-top: 1pt solid rgb(68, 114, 196); border-bottom: 1pt solid rgb(68, 114, 196); border-left: 1pt solid rgb(68, 114, 196); border-image: initial; border-right: none; background-image: initial; background-position: initial; background-size: initial; background-repeat: initial; background-attachment: initial; background-origin: initial; background-clip: initial; background-color: rgb(68, 114, 196) !important; padding: 0in 5.4pt; height: 16.2pt;'>");
            sb.AppendLine("<p class='paraDesign' align='center' style='text-align:center'><b><span style='font-size: 14pt; color: white !important;'>Survey Questions</span></b></p>");
            sb.AppendLine("</td>");
            sb.AppendLine("<td width='236' style='width: 177.3pt; border-top: 1pt solid rgb(68, 114, 196); border-right: 1pt solid rgb(68, 114, 196); border-bottom: 1pt solid rgb(68, 114, 196); border-image: initial; border-left: none; background-image: initial; background-position: initial; background-size: initial; background-repeat: initial; background-attachment: initial; background-origin: initial; background-clip: initial; background-color: rgb(68, 114, 196) !important; padding: 0in 5.4pt; height: 16.2pt;'>");
            sb.AppendLine("<p class='paraDesign' align='center' style='text-align:center'><b><span style='font-size: 14pt; color: white !important;'>Response</span></b></p>");
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr style='height:31.75pt'>");
            sb.AppendLine("<td width='572' style='width: 429.15pt; border-right: 1pt solid rgb(142, 170, 219); border-bottom: 1pt solid rgb(142, 170, 219); border-left: 1pt solid rgb(142, 170, 219); border-image: initial; border-top: none; background-image: initial; background-position: initial; background-size: initial; background-repeat: initial; background-attachment: initial; background-origin: initial; background-clip: initial; background-color: rgb(217, 226, 243) !important; padding: 0in 5.4pt; height: 31.75pt;'>");
            sb.AppendLine("<p class='paraDesign'><b><span style='font-size: 14pt; color: black !important;'>Did your account executive explain your billing activity to your satisfaction? </span></b><b><span style='font-size:14.0pt'></span></b></p>");
            sb.AppendLine("</td>");
            sb.AppendLine("<td width='236' style='width: 177.3pt; border-top: none; border-left: none; border-bottom: 1pt solid rgb(142, 170, 219); border-right: 1pt solid rgb(142, 170, 219); background-image: initial; background-position: initial; background-size: initial; background-repeat: initial; background-attachment: initial; background-origin: initial; background-clip: initial; background-color: rgb(217, 226, 243) !important; padding: 0in 5.4pt; height: 31.75pt;'>");
            sb.AppendLine("<p class='paraDesign' align='center' style='text-align:center'><span style='font-size: 14pt; color: black !important;'>" + rdoBillingActivity.SelectedValue.Trim() + "</span><span style='font-size:14.0pt'></span></p>");
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");

            sb.AppendLine("<tr style='height:32.55pt'>");
            sb.AppendLine("<td width='572' style='width:429.15pt; border:solid #8EAADB 1.0pt; border-top:none; padding:0in 5.4pt 0in 5.4pt; height:32.55pt'>");
            sb.AppendLine("<p class='paraDesign'><b><span style='font-size:14.0pt'>Did your account executive answer all your questions? </span></b></p>");
            sb.AppendLine("</td>");
            sb.AppendLine("<td width='236' style='width:177.3pt; border-top:none; border-left:none; border-bottom:solid #8EAADB 1.0pt; border-right:solid #8EAADB 1.0pt; padding:0in 5.4pt 0in 5.4pt; height:32.55pt'>");
            sb.AppendLine("<p class='paraDesign' align='center' style='text-align:center'><span style='font-size:14.0pt'>" + rdoAnswerAllQuestion.SelectedValue.Trim() + "</span></p>");
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");

            sb.AppendLine("<tr style='height:36.85pt'>");
            sb.AppendLine("<td width='572' style='width: 429.15pt; border-right: 1pt solid rgb(142, 170, 219); border-bottom: 1pt solid rgb(142, 170, 219); border-left: 1pt solid rgb(142, 170, 219); border-image: initial; border-top: none; background-image: initial; background-position: initial; background-size: initial; background-repeat: initial; background-attachment: initial; background-origin: initial; background-clip: initial; background-color: rgb(217, 226, 243) !important; padding: 0in 5.4pt; height: 36.85pt;'>");
            sb.AppendLine("<p class='paraDesign'><b><span style='font-size: 14pt; color: black !important;'>Is your agency/department meeting your revenue expectations? </span></b><b><span style='font-size:14.0pt'></span></b></p>");
            sb.AppendLine("</td>");
            sb.AppendLine("<td width='236' style='width: 177.3pt; border-top: none; border-left: none; border-bottom: 1pt solid rgb(142, 170, 219); border-right: 1pt solid rgb(142, 170, 219); background-image: initial; background-position: initial; background-size: initial; background-repeat: initial; background-attachment: initial; background-origin: initial; background-clip: initial; background-color: rgb(217, 226, 243) !important; padding: 0in 5.4pt; height: 36.85pt;'>");
            sb.AppendLine("<p class='paraDesign' align='center' style='text-align:center'><span style='font-size: 14pt; color: black !important;'>" + rdoMeetingRevenueExpectation.SelectedValue.Trim() + "</span><span style='font-size:14.0pt'></span></p>");
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");


            sb.AppendLine("<tr style='height:36.85pt'>");
            sb.AppendLine("<td colspan='2' width='572' style='width:429.15pt; border:solid #8EAADB 1.0pt; border-top:none; padding:0in 5.4pt 0in 5.4pt; height:32.55pt'>");
            sb.AppendLine("<p class='paraDesign'><b><span style='font-size: 14pt; color: black !important;'>Comments </span></b><b><span style='font-size:14.0pt'></span></b></p>");
            sb.AppendLine("<br />");
            sb.AppendLine("<p class='paraDesign'><span style='font-size: 14pt; color: black !important;'>" + txtComment.Text.Trim() + "</span><span style='font-size:14.0pt'></span></p>");
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");

            sb.AppendLine("</tbody>");
            sb.AppendLine("</table>");
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr style='height:10.3pt'>");
            sb.AppendLine("<td style='padding:.75pt .75pt .75pt .75pt; height:10.3pt'>");
            sb.AppendLine("<p class='paraDesign'><span style='font-size:14.0pt'>&nbsp;</span></p><p class='paraDesign' aria-hidden='true'>&nbsp;</p><p class='paraDesign'><span style='font-size:14.0pt'>Thank you</span></p><p class='paraDesign'><b><span style='font-size: 14pt; color: rgb(0, 144, 148) !important;'>Medicount Management, Inc.</span></b><b></b></p>");
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</tbody>");
            sb.AppendLine("</table>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }
    }
}