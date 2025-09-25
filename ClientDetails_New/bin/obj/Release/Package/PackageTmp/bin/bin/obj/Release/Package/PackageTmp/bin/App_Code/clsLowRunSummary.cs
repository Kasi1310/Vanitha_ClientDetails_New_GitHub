using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ClientDetails.App_Code
{
    public class clsLowRunSummary
    {
        DataTable dt;

        SqlCommand objSqlCommand;
        clsConnection objclsConnection;

        public int ID { get; set; }
        public string Mode { get; set; }
        public string Comment { get; set; }

        public DataTable SelectLowSummaryReport()
        {

            DataSet ds = new DataSet();
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("USP_tblLowRunSummary_Select");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            objSqlCommand.Parameters.AddWithValue("@ID", ID);

            ds = objclsConnection.ExecuteDataSet(objSqlCommand);

            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }
            return ds.Tables[0];
        }

        public void UpdateLowRunSummaryReport()
        {
            objclsConnection = new clsConnection();
            objSqlCommand = new SqlCommand();

            objSqlCommand = new SqlCommand("USP_tblLowRunSummary_Update");
            objSqlCommand.CommandType = CommandType.StoredProcedure;
            objSqlCommand.CommandTimeout = 0;

            objSqlCommand.Parameters.AddWithValue("@Mode", Mode);
            objSqlCommand.Parameters.AddWithValue("@ID", ID);
            objSqlCommand.Parameters.AddWithValue("@Comment", Comment);

            objclsConnection.ExecuteNonQuery(objSqlCommand);
        }
    }
}