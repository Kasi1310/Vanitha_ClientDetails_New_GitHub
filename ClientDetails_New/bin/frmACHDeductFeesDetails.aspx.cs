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
    public partial class frmACHDeductFeesDetails : System.Web.UI.Page
    {
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
            clsACHDeductFees objclsACHDeductFees = new clsACHDeductFees();
            objclsACHDeductFees.ID = 0;
            dt = new DataTable();
            dt = objclsACHDeductFees.SelectACHDeductFees();

            ViewState["dt"] = dt;

            gvACHDeductFees.DataSource = dt;
            gvACHDeductFees.DataBind();
        }

        protected void btnExcelExport_Click(object sender, EventArgs e)
        {
            dt = new DataTable();
            if (ViewState["dt"] != null)
            {
                dt = (DataTable)ViewState["dt"];

                foreach (DataRow dr in dt.Rows)
                {
                    dr["NameOfBank"]= SSTCryptographer.Decrypt(dr["NameOfBank"].ToString(), "Medicount");
                    dr["RoutingNumber"] = SSTCryptographer.Decrypt(dr["RoutingNumber"].ToString(), "Medicount");
                    dr["AccountNumber"] = SSTCryptographer.Decrypt(dr["AccountNumber"].ToString(), "Medicount");
                }

                DataView dv = new System.Data.DataView(dt);

                dt = dv.ToTable("Selected", false, "ClientName", "FundsClientByACH", "EndofMonthInvoice", "EndofMonthFunds", "NameOfBank", "RoutingNumber", "AccountNumber", "Name1", "EmailAddress1", "Name2", "EmailAddress2", "Name3", "EmailAddress3", "Name4", "EmailAddress4", "Name5", "EmailAddress5", "Last Activity Date");

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "Clients ACH Invoices");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=ClientsACHInvoices_Details.xlsx");
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
}