using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ClientDetails.App_Code
{
    public class clsContactAE
    {
        SqlCommand objSqlCommand;
        clsConnection objclsConnection;
        public int ID { get; set; }
        public string Name { get; set; }
        public string MailID { get; set; }
        public string ClientName { get; set; }
        public string Phone { get; set; }
        public string Comments { get; set; }


        public int InsertContactAE()
        {
            DataSet ds = new DataSet();
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("USP_tblContactAE_Insert");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            objSqlCommand.Parameters.AddWithValue("@ID", ID);
            objSqlCommand.Parameters.AddWithValue("@Name", Name);
            objSqlCommand.Parameters.AddWithValue("@MailID", MailID);
            objSqlCommand.Parameters.AddWithValue("@ClientName", ClientName);
            objSqlCommand.Parameters.AddWithValue("@Phone", Phone);
            objSqlCommand.Parameters.AddWithValue("@Comments", Comments);

            ds = objclsConnection.ExecuteDataSet(objSqlCommand);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0)
            {
                return 0;
            }
            return Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
        }

        public DataTable SelectContactAE()
        {
            DataSet ds = new DataSet();
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("USP_tblContactAE_Select");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            objSqlCommand.Parameters.AddWithValue("@MailID", MailID);

            ds = objclsConnection.ExecuteDataSet(objSqlCommand);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            return ds.Tables[0];
        }
    }
}