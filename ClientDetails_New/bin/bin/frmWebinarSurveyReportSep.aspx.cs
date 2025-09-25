using ClientDetails.App_Code;
using ClosedXML.Excel;
using System;
using System.Data;
using System.IO;

namespace ClientDetails
{
    public partial class frmWebinarSurveyReportSep : System.Web.UI.Page
    {
        clsWebinarSurveySep objclsWebinarSurveySep;
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
            objclsWebinarSurveySep = new clsWebinarSurveySep();
            dt = new DataTable();
            dt = objclsWebinarSurveySep.SelectWebinarSurveySep();

            ViewState["dt"] = dt;

            gvWebinarSurvey.DataSource = dt;
            gvWebinarSurvey.DataBind();
        }

        protected void btnExcelExport_Click(object sender, EventArgs e)
        {
            if (ViewState["dt"] == null)
            {
                LoadGrid();
            }
            dt = new DataTable();
            dt = (DataTable)ViewState["dt"];
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Webinar_Survey");

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Webinar_Survey.xlsx");
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
}