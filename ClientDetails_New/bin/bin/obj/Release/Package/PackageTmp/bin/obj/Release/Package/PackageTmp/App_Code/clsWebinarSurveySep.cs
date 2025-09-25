using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace ClientDetails.App_Code
{
    public class clsWebinarSurveySep
    {
        SqlCommand objSqlCommand;
        clsConnection objclsConnection;
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string Question1 { get; set; }
        public string Question2 { get; set; }
        public string Question3 { get; set; }
        public string Question4 { get; set; }

        public void InsertWebinarSurveySep()
        {
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("USP_tblWebinarSurveySep_Insert");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            objSqlCommand.Parameters.AddWithValue("@Name", Name);
            objSqlCommand.Parameters.AddWithValue("@Email", Email);
            objSqlCommand.Parameters.AddWithValue("@CompanyName", CompanyName);
            objSqlCommand.Parameters.AddWithValue("@Question1", Question1);
            objSqlCommand.Parameters.AddWithValue("@Question2", Question2);
            objSqlCommand.Parameters.AddWithValue("@Question3", Question3);
            objSqlCommand.Parameters.AddWithValue("@Question4", Question4);

            objclsConnection.ExecuteNonQuery(objSqlCommand);
        }
        public DataTable SelectWebinarSurveySep()
        {
            DataSet ds = new DataSet();
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("USP_tblWebinarSurveySep_Select");
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