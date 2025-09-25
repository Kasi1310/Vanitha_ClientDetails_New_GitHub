using ClientDetails.App_Code;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClientDetails
{
    public partial class frmASPPDocumentUpload : System.Web.UI.Page
    {

        clsASPPDocumentUpload objclsASPPDocumentUpload;
        DataSet ds;
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Request.QueryString["Id"] == null || Request.QueryString["Id"].ToString() == "")
            //{
            //    divControl.Style.Add("display", "none");
            //    divMessage.Style.Add("display", "block");
            //}
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string designationFilePath = "";
            string ASPPFolderPath = ConfigurationManager.AppSettings["ASPPFolderPath"];

            designationFilePath = Path.Combine(ASPPFolderPath, txtClientName.Text.Trim());

            if (!Directory.Exists(designationFilePath))
            {
                Directory.CreateDirectory(designationFilePath);
            }

            if (fuFiles.HasFile)
            {
                string FileName = "";
                string OriginalFileName = "";
                ds = new DataSet();
                foreach (HttpPostedFile uploadedFile in fuFiles.PostedFiles)
                {
                    FileName = Path.GetFileNameWithoutExtension(uploadedFile.FileName)
                            + "_" + DateTime.Now.ToString("MMddyyyyHHmmmss")
                            + Path.GetExtension(uploadedFile.FileName);
                    OriginalFileName = Path.GetFileName(uploadedFile.FileName);

                    uploadedFile.SaveAs(designationFilePath + "\\" + FileName);

                    objclsASPPDocumentUpload = new clsASPPDocumentUpload();
                    objclsASPPDocumentUpload.ClientName = txtClientName.Text.Trim();
                    objclsASPPDocumentUpload.EmailId = txtEmailId.Text.Trim();
                    objclsASPPDocumentUpload.OriginalFileName = OriginalFileName;
                    objclsASPPDocumentUpload.FileName = FileName;
                    ds = objclsASPPDocumentUpload.InsertASPPDocumentUpload();
                }

                gvDocuments.DataSource = ds;
                gvDocuments.DataBind();

                lblMessage.Style.Remove("display");
                lblMessage.Style.Add("display", "block");

                //ClientScript.RegisterStartupScript(GetType(), "myscript", "OpenMessagePopup();", true);
            }

        }

        protected void gvDocuments_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmdOpenFile")
            {

                string[] CommandArgument = e.CommandArgument.ToString().Split(',');

                string designationFileName = CommandArgument[0];
                string OriginalFileName = CommandArgument[1];

                string designationFilePath = "";
                string ASPPFolderPath = ConfigurationManager.AppSettings["ASPPFolderPath"];

                designationFilePath = Path.Combine(ASPPFolderPath, txtClientName.Text.Trim(), designationFileName);

                Response.ContentType = "application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(OriginalFileName));
                Response.TransmitFile(designationFilePath);
                Response.End();
                //System.Diagnostics.Process.Start(destinationPDFFilePath);
            }
            else if (e.CommandName == "cmdDelete")
            {
                string[] CommandArgument = e.CommandArgument.ToString().Split(',');

                hdnID.Value = CommandArgument[0];
                hdnDeletedFileName.Value = CommandArgument[1];


                objclsASPPDocumentUpload = new clsASPPDocumentUpload();
                objclsASPPDocumentUpload.ID = int.Parse(hdnID.Value.Trim());
                ds = objclsASPPDocumentUpload.DeleteASPPDocumentUpload();

                gvDocuments.DataSource = ds;
                gvDocuments.DataBind();

                string designationFilePath = "";
                string ASPPFolderPath = ConfigurationManager.AppSettings["ASPPFolderPath"];

                designationFilePath = Path.Combine(ASPPFolderPath, txtClientName.Text.Trim(), hdnDeletedFileName.Value);

                if (File.Exists(designationFilePath))
                {
                    File.Delete(designationFilePath);
                }


                lblMessage.Style.Remove("display");
                lblMessage.Style.Add("display", "none");

                //ClientScript.RegisterStartupScript(GetType(), "myscript", "OpenDeletePopup();", true);
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {

        }
    }
}