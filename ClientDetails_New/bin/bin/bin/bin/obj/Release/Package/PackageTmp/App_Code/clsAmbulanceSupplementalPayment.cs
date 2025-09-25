using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
namespace ClientDetails.App_Code
{
    public class clsAmbulanceSupplementalPayment
    {
        SqlCommand objSqlCommand;
        clsConnection objclsConnection;
        public int ID { get; set; }
        public string ClientName { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsGeneralInformation { get; set; }
        public bool IsCallBack { get; set; }
        public string SpecificQuestion { get; set; }


        public int InsertAmbulanceSupplementalPayment()
        {
            DataSet ds = new DataSet();
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("usp_tblAmbu_Supple_Payment_Insert");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            objSqlCommand.Parameters.AddWithValue("@ClientName", ClientName);
            objSqlCommand.Parameters.AddWithValue("@Name", Name);
            objSqlCommand.Parameters.AddWithValue("@EmailAddress", EmailAddress);
            objSqlCommand.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
            objSqlCommand.Parameters.AddWithValue("@IsGeneralInformation", IsGeneralInformation);
            objSqlCommand.Parameters.AddWithValue("@IsCallBack", IsCallBack);
            objSqlCommand.Parameters.AddWithValue("@SpecificQuestion", SpecificQuestion);

            ds = objclsConnection.ExecuteDataSet(objSqlCommand);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0] == null || ds.Tables[0].Rows.Count == 0)
            {
                return 0;
            }
            return Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
        }

        public DataTable SelectAmbulanceSupplementalPayment()
        {
            DataSet ds = new DataSet();
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("usp_tblAmbu_Supple_Payment_Select");
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