using ClientDetails.App_Code;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClientDetails
{
    public partial class frmClientProposal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            clsClientProposal objclsClientProposal = new clsClientProposal();

            List<string> lstAttachement = new List<string>();


            string FolderPath = ConfigurationManager.AppSettings["ClientProposalFolderPath"];
            string FileName = "";
            string FilePath = "";

            objclsClientProposal.ClientName = txtClient.Text.Trim();
            objclsClientProposal.AEName = txtAEName.Text.Trim();
            objclsClientProposal.NameOfEntity = txtNameofEntity.Text.Trim();
            objclsClientProposal.ReferralFrom = txtReferralFrom.Text.Trim();
            objclsClientProposal.IsChangeHealthCareClient = rdolstIsChangeHealthCareClient.SelectedValue.Trim();
            objclsClientProposal.IsALSDispatchProtocol = rdolstIsALSDispatchProtocol.SelectedValue.Trim();

            if (fuImageOfPotentialClientsAmbulance.HasFile)
            {
                FileName = Path.GetFileName(fuImageOfPotentialClientsAmbulance.PostedFile.FileName); //+ "_" + DateTime.Now.ToString("MMddyyyyHHmmmss") + Path.GetExtension(fuImageOfPotentialClientsAmbulance.PostedFile.FileName);

                FilePath = FolderPath + FileName;
                fuImageOfPotentialClientsAmbulance.SaveAs(FilePath);

                objclsClientProposal.ImageOfPotentialClientsAmbulance = FileName;

                lstAttachement.Add(FilePath);
            }
            else
            {
                objclsClientProposal.ImageOfPotentialClientsAmbulance = "";
            }
            if (fuImageOfPotentialClientsEmblem.HasFile)
            {
                FileName = Path.GetFileName(fuImageOfPotentialClientsEmblem.PostedFile.FileName); //+ "_" + DateTime.Now.ToString("MMddyyyyHHmmmss") + Path.GetExtension(fuImageOfPotentialClientsAmbulance.PostedFile.FileName);
                FilePath = FolderPath + FileName;
                fuImageOfPotentialClientsEmblem.SaveAs(FilePath);

                objclsClientProposal.ImageOfPotentialClientsEmblem = FileName;
                lstAttachement.Add(FilePath);
            }
            else
            {
                objclsClientProposal.ImageOfPotentialClientsEmblem = "";
            }
            objclsClientProposal.NamesAndTitleOfIndividuals = txtNamesAndTitleOfIndividuals.Text.Trim();
            objclsClientProposal.AddressOfEntity = txtAddressOfEntity.Text.Trim();
            objclsClientProposal.CountyName = txtCountyName.Text.Trim();
            objclsClientProposal.PhoneNumber = txtPhoneNumber.Text.Trim();
            objclsClientProposal.EmailAddress = txtEmailAddress.Text.Trim();
            objclsClientProposal.NumberOfProposalsRequested = txtNumberOfProposalsRequested.Text.Trim();
            objclsClientProposal.ApproximateDateOfProposal = txtApproximateDateOfProposal.Text.Trim();
            objclsClientProposal.ProposalsMailedDirectly = txtProposalsMailedDirectly.Text.Trim();
            objclsClientProposal.FeeRecommended = txtFeeRecommended.Text.Trim();
            objclsClientProposal.AdditionalEquipment = txtAdditionalEquipment.Text.Trim();
            objclsClientProposal.LengthOfClientServicesAgreement = txtLengthOfClientServicesAgreement.Text.Trim();
            objclsClientProposal.ChangeHealthCareClient = "";// txtChangeHealthCareClient.Text.Trim();
            objclsClientProposal.ALSProtocalRequiredCSAProtocol = "";// txtALSProtocalRequiredCSAProtocol.Text.Trim();
            objclsClientProposal.IsEPCROrEquipmentCharges = rdolstIsEPCROrEquipmentCharges.SelectedValue.Trim();
            if (fuCopyOfTheCurrentBillersContract.HasFile)
            {
                FileName = Path.GetFileName(fuCopyOfTheCurrentBillersContract.PostedFile.FileName); //+ "_" + DateTime.Now.ToString("MMddyyyyHHmmmss") + Path.GetExtension(fuImageOfPotentialClientsAmbulance.PostedFile.FileName);
                FilePath = FolderPath + FileName;
                fuCopyOfTheCurrentBillersContract.SaveAs(FilePath);

                objclsClientProposal.CopyOfTheCurrentBillersContract = FileName;
                lstAttachement.Add(FilePath);
            }
            else
            {
                objclsClientProposal.CopyOfTheCurrentBillersContract = "";
            }

            if (fuAttachedTransportAndRevenue.HasFile)
            {
                FileName = Path.GetFileName(fuAttachedTransportAndRevenue.PostedFile.FileName); //+ "_" + DateTime.Now.ToString("MMddyyyyHHmmmss") + Path.GetExtension(fuImageOfPotentialClientsAmbulance.PostedFile.FileName);
                FilePath = FolderPath + FileName;
                fuAttachedTransportAndRevenue.SaveAs(FilePath);

                objclsClientProposal.AttachedTransportAndRevenue = FileName;
                lstAttachement.Add(FilePath);
            }
            else
            {
                objclsClientProposal.AttachedTransportAndRevenue = "";
            }

            objclsClientProposal.AnyPassThroughCharges = txtAnyPassThroughCharges.Text.Trim();
            objclsClientProposal.OtherComments = txtOtherComments.Text.Trim();
            objclsClientProposal.LastUpdatedBy = "";

            int ID = objclsClientProposal.InsertClientProposal();

            if (fuReferenceCoverLetter.HasFile)
            {
                foreach (HttpPostedFile uploadedFile in fuReferenceCoverLetter.PostedFiles)
                {
                    FileName = Path.GetFileName(uploadedFile.FileName); //+ "_" + DateTime.Now.ToString("MMddyyyyHHmmmss") + Path.GetExtension(fuImageOfPotentialClientsAmbulance.PostedFile.FileName);
                    FilePath = FolderPath + FileName;
                    fuReferenceCoverLetter.SaveAs(FilePath);

                    if (hdnReferenceCoverLetter.Value == "")
                    {
                        hdnReferenceCoverLetter.Value = FileName;
                    }
                    else
                    {
                        hdnReferenceCoverLetter.Value = hdnReferenceCoverLetter.Value + "<br />" + FileName;
                    }

                    lstAttachement.Add(FilePath);

                    objclsClientProposal.InsertClientProposalAttachment(ID, "Reference Cover Letter", FileName);
                }
            }

            if (fuReferenceProposal.HasFile)
            {
                foreach (HttpPostedFile uploadedFile in fuReferenceProposal.PostedFiles)
                {
                    FileName = Path.GetFileName(uploadedFile.FileName); //+ "_" + DateTime.Now.ToString("MMddyyyyHHmmmss") + Path.GetExtension(fuImageOfPotentialClientsAmbulance.PostedFile.FileName);
                    FilePath = FolderPath + FileName;
                    fuReferenceProposal.SaveAs(FilePath);

                    if (hdnReferenceProposal.Value == "")
                    {
                        hdnReferenceProposal.Value = FileName;
                    }
                    else
                    {
                        hdnReferenceProposal.Value = hdnReferenceProposal.Value + "<br />" + FileName;
                    }

                    lstAttachement.Add(FilePath);

                    objclsClientProposal.InsertClientProposalAttachment(ID, "Reference Cover Proposal", FileName);
                }
            }



            clsSendMail objclsSendMail = new clsSendMail();

            string To = ConfigurationManager.AppSettings["ClientProposal.To.email"].ToString().Trim();
            string CC = ConfigurationManager.AppSettings["ClientProposal.CC.email"].ToString().Trim();
            string BCC = ConfigurationManager.AppSettings["ClientProposal.BCC.email"].ToString().Trim();

            if (objclsSendMail.SendMail(To, CC, BCC, "DETAILED PROPOSAL FORM - " + txtClient.Text.Trim(), MailBody(), lstAttachement.ToArray()))
            {
                divContent.Style.Add("display", "none");
                lblMessage.Style.Add("display", "block");
            }
            else
            {
                divContent.Style.Add("display", "none");
                lblError.Style.Add("display", "block");
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.AbsoluteUri);
        }


        private string MailBody()
        {
            StringBuilder sb = new StringBuilder();

            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            string imagePath = "";
            int lastIndex = url.LastIndexOf("/");
            //imagePath = url.Substring(0, lastIndex) + "/Images/Logo.jpg";
            imagePath = url.Substring(0, lastIndex) + "/Images/";


            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("<style type='text/css'>");
            sb.AppendLine(".lblText {font-family: Calibri;font-weight: bold;font-size: 20px;height: 30px;padding-left:10px;}");
            sb.AppendLine(".lblValue {font-family: Calibri;font-size: 18px;height: 30px;padding-left: 10px;}");
            sb.AppendLine("</style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body lang=EN-US style='word-wrap:break-word;'>");
            sb.AppendLine("<div>");
            sb.AppendLine("<center>");
            sb.AppendLine("<table border=1 cellspacing=0 cellpadding=0 style='width:1000px;'>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td style='width:600px;'>");
            sb.AppendLine("<img src='" + imagePath + "Logo.jpg' />");
            sb.AppendLine("</td>");
            sb.AppendLine("<td style='width: 400px; background-color: #ed7d31;padding:0 20px 0 20px;' align='center'>");
            sb.AppendLine("<span style='font-family: Calibri; font-weight: bold; font-size: 34px; color: white;'>DETAILED PROPOSAL FORM FOR</span>");
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td style='height: 100px; background-color: #ed7d31;' align='center'>");
            sb.AppendLine("<span style='font-family: Calibri; font-weight: bold; font-size: 34px; color: white;'>NAME OF CLIENT:</span>");
            sb.AppendLine("</td>");
            sb.AppendLine("<td style='font-family: Calibri; font-weight: bold; font-size: 34px;' align='center'>");
            sb.AppendLine(txtClient.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td colspan='2' align='center' style='background-color: #ffc000;'>");
            sb.AppendLine("<span style='font-family: Calibri; font-weight: bold; font-size: 22px; height: 30px;'>Please provide the following information for the proposal</span>");
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("Account Executive name(s):");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(txtAEName.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("Name of Entity - Confirm:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(txtNameofEntity.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("Referral From Who/Where:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(txtReferralFrom.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("Change Healthcare Client:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(rdolstIsChangeHealthCareClient.SelectedValue.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("If Yes, do they use the ALS Dispatch Protocol:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(rdolstIsALSDispatchProtocol.SelectedValue.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("Image of the potential Client's ambulance for the cover page:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(Path.GetFileName(fuImageOfPotentialClientsAmbulance.PostedFile.FileName));
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("Image of the potential Client’s emblem/badge for the cover page:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(Path.GetFileName(fuImageOfPotentialClientsEmblem.PostedFile.FileName));
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("Name(s) and title of individuals receiving the proposal for cover letter and cover page:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(txtNamesAndTitleOfIndividuals.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("Address of Entity for Cover Letter:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(txtAddressOfEntity.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("County Name:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(txtCountyName.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("Phone Number:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(txtPhoneNumber.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("Email address of your Contact (for UPS services):");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(txtEmailAddress.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("The number of proposals requested:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(txtNumberOfProposalsRequested.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("Approximate Date of when the proposal is needed:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(txtApproximateDateOfProposal.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("Proposals: Mailed directly to the potential Client or the account executive (or picked up)");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(txtProposalsMailedDirectly.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("Fee % - Recommended:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(txtFeeRecommended.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("Additional equipment or software applications that need to be included in the fee and or Client Services Agreement:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(txtAdditionalEquipment.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("Length of term for Client Services Agreement:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(txtLengthOfClientServicesAgreement.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            //sb.AppendLine("<tr>");
            //sb.AppendLine("<td class='lblText'>");
            //sb.AppendLine("Change Healthcare Client:");
            //sb.AppendLine("</td>");
            //sb.AppendLine("<td class='lblValue'>");
            //sb.AppendLine(txtChangeHealthCareClient.Text.Trim());
            //sb.AppendLine("</td>");
            //sb.AppendLine("</tr>");
            //sb.AppendLine("<tr>");
            //sb.AppendLine("<td class='lblText'>");
            //sb.AppendLine("ALS Protocol required in CSA: Protocol");
            //sb.AppendLine("</td>");
            //sb.AppendLine("<td class='lblValue'>");
            //sb.AppendLine(txtALSProtocalRequiredCSAProtocol.Text.Trim());
            //sb.AppendLine("</td>");
            //sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("Reference on the Cover letter:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(hdnReferenceCoverLetter.Value);
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("References in the body of the proposal:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(hdnReferenceProposal.Value);
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("Are any ePCR or equipment Charges included in the previous biller contract?");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(rdolstIsEPCROrEquipmentCharges.SelectedValue.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("Please attach a copy of the current billing contract.");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(Path.GetFileName(fuCopyOfTheCurrentBillersContract.PostedFile.FileName));
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("Attached a report from a client in the last 12 months, Transport & Revenue.");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(Path.GetFileName(fuAttachedTransportAndRevenue.PostedFile.FileName));
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("Any Pass Through Charges?");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(txtAnyPassThroughCharges.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("<tr>");
            sb.AppendLine("<td class='lblText'>");
            sb.AppendLine("Other Comments:");
            sb.AppendLine("</td>");
            sb.AppendLine("<td class='lblValue'>");
            sb.AppendLine(txtOtherComments.Text.Trim());
            sb.AppendLine("</td>");
            sb.AppendLine("</tr>");
            sb.AppendLine("</table>");
            sb.AppendLine("</center>");
            sb.AppendLine("</div>");

            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;margin:0;line-height:18.0pt;'>");
            sb.AppendLine("<b><span style = 'font-size:14pt;'> &nbsp;</span></b></p>");
            sb.AppendLine("<p style = 'font-size:11pt;font-family:Calibri,sans-serif;margin:0;line-height:18.0pt;'>");
            sb.AppendLine("<b><span style = 'font-size:14pt;'> Thank you. <br>");
            sb.AppendLine("</span></b><b><span style = 'color:#009094;font-size:16pt;'> Medicount Management, Inc.</span></b></p>");

            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            sb.AppendLine("<br />");

            return sb.ToString();
        }
    }
}