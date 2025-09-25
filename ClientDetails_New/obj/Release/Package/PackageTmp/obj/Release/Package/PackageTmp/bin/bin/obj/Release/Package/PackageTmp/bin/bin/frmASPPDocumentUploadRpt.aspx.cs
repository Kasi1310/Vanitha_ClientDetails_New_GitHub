using ClientDetails.App_Code;
using ClosedXML.Excel;
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
    public partial class frmASPPDocumentUploadRpt : System.Web.UI.Page
    {
        clsASPPDocumentUpload objclsASPPDocumentUpload;
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
            objclsASPPDocumentUpload = new clsASPPDocumentUpload();
            dt = new DataTable();
            dt = objclsASPPDocumentUpload.SelectASPPDocumentUpload();

            ViewState["dt"] = dt;

            gvASPPDocument.DataSource = dt;
            gvASPPDocument.DataBind();
        }

        protected void btnExcelExport_Click(object sender, EventArgs e)
        {
            if (ViewState["dt"] == null)
            {
                LoadGrid();
            }
            dt = new DataTable();
            dt = (DataTable)ViewState["dt"];

            DataTable dtExcel = new DataTable();
            dtExcel.Columns.Add("Client Name");
            dtExcel.Columns.Add("Email Address");
            dtExcel.Columns.Add("List of Documents");
            dtExcel.Columns.Add("Uploaded Date");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dtExcel.NewRow();
                dr["Client Name"] = dt.Rows[i]["ClientName"].ToString().Trim();
                dr["Email Address"] = dt.Rows[i]["EmailId"].ToString().Trim();
                dr["List of Documents"] = dt.Rows[i]["OriginalFileName"].ToString().Trim();
                dr["Uploaded Date"] = dt.Rows[i]["UploadedDate"].ToString().Trim();

                dtExcel.Rows.Add(dr);
            }



            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dtExcel, "ASPP Document");

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=ASPP_Document.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }

        protected void gvASPPDocument_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "cmdOpenFile")
            {

                string[] CommandArgument = e.CommandArgument.ToString().Split(',');

                string designationFileName = CommandArgument[0];
                string OriginalFileName = CommandArgument[1];
                string ClientName = CommandArgument[2];

                string designationFilePath = "";
                string ASPPFolderPath = ConfigurationManager.AppSettings["ASPPFolderPath"];

                designationFilePath = Path.Combine(ASPPFolderPath, ClientName, designationFileName);

                Response.ContentType = "application/pdf";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(OriginalFileName));
                Response.TransmitFile(designationFilePath);
                Response.End();
                //System.Diagnostics.Process.Start(destinationPDFFilePath);
            }
        }
    }
}