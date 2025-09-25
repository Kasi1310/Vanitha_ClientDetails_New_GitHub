using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ClientDetails.App_Code;
using ClosedXML.Excel;

namespace ClientDetails
{
    public partial class frmAgency : System.Web.UI.Page
    {
        clsAgency objclsAgency = new clsAgency();
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadGrid();
        }

        protected void btnExcelExport_Click(object sender, EventArgs e)
        {
            objclsAgency = new clsAgency();

            
            DataTable dt = new DataTable();

            dt = objclsAgency.SelectAgency();
            
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Agency");

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Agency_Details.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }

        }

        private void LoadGrid()
        {
            objclsAgency = new clsAgency();
            gvAgency.DataSource = objclsAgency.SelectAgency();
            gvAgency.DataBind();
        }
    }
}