using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ClientDetails.App_Code
{
    public class clsAgency
    {
        SqlCommand objSqlCommand;
        clsConnection objclsConnection;

        public int ID { get; set; }
        public string Name { get; set; }
        public string PersonInterested { get; set; }
        public string EmailAddress { get; set; }


        public DataTable SelectAgency()
        {
            DataSet ds = new DataSet();
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("USP_tblAgency_Select");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            ds = objclsConnection.ExecuteDataSet(objSqlCommand);

            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }
            return ds.Tables[0];
        }
        public DataTable InsertAgency()
        {
            DataSet ds = new DataSet();
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("USP_tblAgency_Insert");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            objSqlCommand.Parameters.AddWithValue("@Name", Name);
            objSqlCommand.Parameters.AddWithValue("@PersonInterested", PersonInterested);
            objSqlCommand.Parameters.AddWithValue("@EmailAddress", EmailAddress);

            ds = objclsConnection.ExecuteDataSet(objSqlCommand);

            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }
            return ds.Tables[0];
        }


        //public void UpdateClients(string Mode)
        //{
        //    objSqlCommand = new SqlCommand();
        //    objclsConnection = new clsConnection();

        //    objSqlCommand = new SqlCommand("USP_tblClients_Update");
        //    objSqlCommand.CommandType = CommandType.StoredProcedure;

        //    objSqlCommand.Parameters.AddWithValue("@Mode", Mode);
        //    objSqlCommand.Parameters.AddWithValue("@ClientID", ClientID);

        //    objclsConnection.ExecuteNonQuery(objSqlCommand);
        //}

    }
}