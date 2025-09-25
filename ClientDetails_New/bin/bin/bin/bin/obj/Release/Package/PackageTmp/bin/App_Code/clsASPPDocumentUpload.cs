using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ClientDetails.App_Code
{
    public class clsASPPDocumentUpload
    {
        SqlCommand objSqlCommand;
        clsConnection objclsConnection;
        public int ID { get; set; }
        public string ClientName { get; set; }
        public string EmailId { get; set; }
        public string OriginalFileName { get; set; }
        public string FileName { get; set; }


        public DataSet InsertASPPDocumentUpload()
        {
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("usp_tblASPPDocumentDetails_Insert");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            objSqlCommand.Parameters.AddWithValue("@ClientName", ClientName);
            objSqlCommand.Parameters.AddWithValue("@EmailId", EmailId);
            objSqlCommand.Parameters.AddWithValue("@OriginalFileName", OriginalFileName);
            objSqlCommand.Parameters.AddWithValue("@FileName", FileName);

            return objclsConnection.ExecuteDataSet(objSqlCommand);

        }

        public DataSet DeleteASPPDocumentUpload()
        {
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("USP_tblASPPDocumentDetails_Delete");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            objSqlCommand.Parameters.AddWithValue("@ID", ID);

           return objclsConnection.ExecuteDataSet(objSqlCommand);

        }


        public DataTable SelectASPPDocumentUpload()
        {
            DataSet ds = new DataSet();
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("usp_tblASPPDocumentDetails_Select");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            ds = objclsConnection.ExecuteDataSet(objSqlCommand);

            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }
            return ds.Tables[0];
        }
    }
}