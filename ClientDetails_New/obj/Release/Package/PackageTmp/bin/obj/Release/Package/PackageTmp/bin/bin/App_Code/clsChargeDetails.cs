using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ClientDetails.App_Code
{
    public class clsChargeDetails
    {

        SqlCommand objSqlCommand;
        clsConnection objclsConnection;

        DataSet ds;
        public DataTable SelectChargeDetails()
        {
            ds = new DataSet();
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("USP_tblChargeDetails_Select");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            ds= objclsConnection.ExecuteDataSet(objSqlCommand);

            if(ds==null || ds.Tables.Count!=1)
            {
                return null;
            }
            return ds.Tables[0];
        }
        public DataTable UpdateChargeDetails(string Mode,int ID)
        {
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("USP_tblChargeDetails_Update");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            objSqlCommand.Parameters.AddWithValue("@Mode", Mode);
            objSqlCommand.Parameters.AddWithValue("@ID", ID);

            ds = objclsConnection.ExecuteDataSet(objSqlCommand);

            if (ds == null || ds.Tables.Count != 1)
            {
                return null;
            }
            return ds.Tables[0];
        }
    }
}