using ClientDetails.App_Code;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ClientDetails
{
    public partial class frmVoicentAutoDialer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            WriteLog("Start - " + DateTime.Now.ToString("MMddyyyyhhmmss"));

            if (fuOutputFile.HasFile)
            {
                DataTable dtInput = new DataTable();
                DataTable dt = new DataTable();

                string FileName = Path.GetFileNameWithoutExtension(fuOutputFile.PostedFile.FileName) + "_" + DateTime.Now.ToString("MMddyyyyHHmmmss") + Path.GetExtension(fuOutputFile.PostedFile.FileName);
                string FolderPath = ConfigurationManager.AppSettings["FolderPath"];

                string FilePath = FolderPath + FileName;
                fuOutputFile.SaveAs(FilePath);

                dtInput = ImportFiles(FilePath);

                dtInput.Rows.RemoveAt(0);

                dt = new DataTable();

                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Type", typeof(string));
                dt.Columns.Add("Address", typeof(double));
                dt.Columns.Add("Status", typeof(string));
                dt.Columns.Add("Start Time", typeof(DateTime));
                dt.Columns.Add("Duration", typeof(string));
                dt.Columns.Add("Notes", typeof(string));
                dt.Columns.Add("Agent Recording", typeof(string));
                dt.Columns.Add("Customer", typeof(double));

                for (int i = 0; i < dtInput.Rows.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = dtInput.Rows[i]["Column1"].ToString().Trim();
                    dr[1] = dtInput.Rows[i]["Column2"].ToString().Trim();
                    dr[2] = dtInput.Rows[i]["Column3"];
                    dr[3] = dtInput.Rows[i]["Column4"].ToString().Trim();


                    double date = double.Parse(dtInput.Rows[i]["Column5"].ToString().Trim());

                    var dateTime = DateTime.FromOADate(date).ToString("MM/dd/yyyy hh:mm:ss tt");


                    dr[4] = dateTime;
                    dr[5] = dtInput.Rows[i]["Column6"].ToString().Trim();
                    dr[6] = dtInput.Rows[i]["Column7"].ToString().Trim();
                    dr[7] = dtInput.Rows[i]["Column8"].ToString().Trim();
                    dr[8] = dtInput.Rows[i]["Column9"];

                    dt.Rows.Add(dr);
                }

                clsConnection objclsConnection = new clsConnection();
                SqlCommand objSqlCommand = new SqlCommand();
                DataSet dsCheckReportDate = new DataSet();


                ////Check the today report generated
                //objSqlCommand = new SqlCommand("USP_Voicent_CheckReportGenerated");
                //objSqlCommand.CommandType = CommandType.StoredProcedure;
                //dsCheckReportDate = objclsConnection.ExecuteDataSet(objSqlCommand);

                //if (dsCheckReportDate != null && dsCheckReportDate.Tables.Count != 0 && dsCheckReportDate.Tables[0].Rows.Count == 0)
                //{

                WriteLog("Import yesterday report - " + DateTime.Now.ToString("MMddyyyyhhmmss"));

                //import yesterday report
                objclsConnection.SQLBulkCopy(dt, "Voicent_AutoDialer_CallReport");

                WriteLog("USP_VoicentMaster_Select(Yesterday) - " + DateTime.Now.ToString("MMddyyyyhhmmss"));

                //truncate and insert the master and input datas
                objSqlCommand = new SqlCommand();
                objclsConnection = new clsConnection();
                objSqlCommand = new SqlCommand("USP_VoicentMaster_Select");
                objSqlCommand.CommandType = CommandType.StoredProcedure;
                objSqlCommand.CommandTimeout = 600;
                objSqlCommand.Parameters.AddWithValue("@Mode", "Yesterday");
                objclsConnection.ExecuteNonQuery(objSqlCommand);

                WriteLog("USP_VoicentYesterdayInput_Select - " + DateTime.Now.ToString("MMddyyyyhhmmss"));

                objSqlCommand = new SqlCommand();
                objclsConnection = new clsConnection();
                objSqlCommand = new SqlCommand("USP_VoicentYesterdayInput_Select");
                objSqlCommand.CommandType = CommandType.StoredProcedure;
                objSqlCommand.CommandTimeout = 600;
                objclsConnection.ExecuteNonQuery(objSqlCommand);

                WriteLog("USP_VoicentMaster_Select(Today) - " + DateTime.Now.ToString("MMddyyyyhhmmss"));

                objSqlCommand = new SqlCommand();
                objclsConnection = new clsConnection();
                objSqlCommand = new SqlCommand("USP_VoicentMaster_Select");
                objSqlCommand.CommandType = CommandType.StoredProcedure;
                objSqlCommand.CommandTimeout = 600;
                objSqlCommand.Parameters.AddWithValue("@Mode", "Today");
                objclsConnection.ExecuteNonQuery(objSqlCommand);

                WriteLog("USP_VoicentTodayInput_Select - " + DateTime.Now.ToString("MMddyyyyhhmmss"));

                objSqlCommand = new SqlCommand();
                objclsConnection = new clsConnection();
                objSqlCommand = new SqlCommand("USP_VoicentTodayInput_Select");
                objSqlCommand.CommandTimeout = 600;
                objSqlCommand.CommandType = CommandType.StoredProcedure;
                objclsConnection.ExecuteNonQuery(objSqlCommand);

                //}

                WriteLog("USP_Voicent_Output - " + DateTime.Now.ToString("MMddyyyyhhmmss"));

                //select the output tables(4 tables)
                DataSet ds = new DataSet();
                objSqlCommand = new SqlCommand();
                objclsConnection = new clsConnection();

                objSqlCommand = new SqlCommand("USP_Voicent_Output");
                objSqlCommand.CommandType = CommandType.StoredProcedure;
                objSqlCommand.CommandTimeout = 600;

                ds = objclsConnection.ExecuteDataSet(objSqlCommand);

                WriteLog("Excel Start - " + DateTime.Now.ToString("MMddyyyyhhmmss"));

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(ds.Tables[0], "Yestday_Master");
                    wb.Worksheets.Add(ds.Tables[1], "Yesterday_Input");
                    wb.Worksheets.Add(ds.Tables[2], "Today_Master");
                    wb.Worksheets.Add(ds.Tables[3], "Today_Input");

                    //wb.Worksheet(5).Row(1).Delete();
                    //wb.Worksheet(5).FirstRow().Delete();
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=" + DateTime.Now.ToString("MM.dd.yyyy") + " - CallReport.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        HttpContext.Current.Response.Flush();
                        //HttpContext.Current.Response.End();
                        HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                        HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
                    }
                }

                WriteLog("Excel End - " + DateTime.Now.ToString("MMddyyyyhhmmss"));
            }
        }

        private DataTable ImportFiles(string FilePath)
        {
            DataTable dtInputFile = new DataTable();

            FileStream strem = File.Open(FilePath, FileMode.Open, FileAccess.Read);
            Excel.IExcelDataReader excelReader;

            excelReader = Excel.ExcelReaderFactory.CreateBinaryReader(strem);
            dtInputFile = excelReader.AsDataSet().Tables[0];

            return dtInputFile;
        }

        private void WriteLog(string Message)
        {
            string strFilePath = ConfigurationManager.AppSettings["FolderPath"]+"\\Logs";
            string strLogFileName = "";
            if (ViewState["LogFileName"] == null)
            {
                ViewState["LogFileName"] = "Log_" + DateTime.Now.ToString("MMddyyyyhhmmss") + ".txt";
            }

            strLogFileName = ViewState["LogFileName"].ToString().Trim();


            if (!Directory.Exists(strFilePath))
            {
                Directory.CreateDirectory(strFilePath);
            }

            FileStream fs = new FileStream(strFilePath + "\\" + strLogFileName, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);
            DateTime dt = new DateTime();
            dt = DateTime.Now;
            sw.WriteLine(dt.ToShortDateString() + " " + dt.ToLongTimeString() + "   " + Message);
            sw.Close();
            sw = null;
        }
    }
}