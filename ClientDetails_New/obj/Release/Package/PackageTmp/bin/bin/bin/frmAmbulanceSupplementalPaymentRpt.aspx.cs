using ClientDetails.App_Code;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClientDetails
{
    public partial class frmAmbulanceSupplementalPaymentRpt : System.Web.UI.Page
    {
        clsAmbulanceSupplementalPayment objclsAmbulanceSupplementalPayment;
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
            objclsAmbulanceSupplementalPayment = new clsAmbulanceSupplementalPayment();
            dt = new DataTable();
            dt = objclsAmbulanceSupplementalPayment.SelectAmbulanceSupplementalPayment();

            ViewState["dt"] = dt;

            gvAmbSupp.DataSource = dt;
            gvAmbSupp.DataBind();
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
            dtExcel.Columns.Add("Name");
            dtExcel.Columns.Add("Email Address");
            dtExcel.Columns.Add("Phone Number");
            dtExcel.Columns.Add("General Information");
            dtExcel.Columns.Add("Call Back");
            dtExcel.Columns.Add("Specific Question");
            dtExcel.Columns.Add("Filled Date");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dtExcel.NewRow();
                dr["Client Name"] = dt.Rows[i]["ClientName"].ToString().Trim();
                dr["Name"] = dt.Rows[i]["Name"].ToString().Trim();
                dr["Email Address"] = dt.Rows[i]["EmailAddress"].ToString().Trim();
                dr["Phone Number"] = dt.Rows[i]["PhoneNumber"].ToString().Trim();
                dr["General Information"] = dt.Rows[i]["GeneralInformation"].ToString().Trim();
                dr["Call Back"] = dt.Rows[i]["CallBack"].ToString().Trim();
                dr["Specific Question"] = dt.Rows[i]["SpecificQuestion"].ToString().Trim();
                dr["Filled Date"] = dt.Rows[i]["FilledDate"].ToString().Trim();

                dtExcel.Rows.Add(dr);
            }


            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dtExcel, "Ambulance Supplemental Payment");

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=OHIO_ASPP_Report.xlsx");
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