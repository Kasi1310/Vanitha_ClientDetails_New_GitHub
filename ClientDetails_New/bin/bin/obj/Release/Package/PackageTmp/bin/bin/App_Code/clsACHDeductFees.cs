using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ClientDetails.App_Code
{
    public class clsACHDeductFees
    {
        SqlCommand objSqlCommand;
        clsConnection objclsConnection;
        public int ID { get; set; }
        public string ClientName { get; set; }
        public bool IsFundsClientByACH { get; set; }
        public bool IsEndofMonthInvoice { get; set; }
        public bool IsEndofMonthFunds { get; set; }
        public string NameOfBank { get; set; }
        public string RoutingNumber { get; set; }
        public string AccountNumber { get; set; }
        public string Name1 { get; set; }
        public string EmailAddress1 { get; set; }
        public string Name2 { get; set; }
        public string EmailAddress2 { get; set; }
        public string Name3 { get; set; }
        public string EmailAddress3 { get; set; }
        public string Name4 { get; set; }
        public string EmailAddress4 { get; set; }
        public string Name5 { get; set; }
        public string EmailAddress5 { get; set; }

        public int InsertACHDeductFees()
        {
            DataSet ds = new DataSet();
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("usp_tblACHDeductFees_Insert");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            objSqlCommand.Parameters.AddWithValue("@ID", ID);
            objSqlCommand.Parameters.AddWithValue("@ClientName", ClientName);
            objSqlCommand.Parameters.AddWithValue("@IsFundsClientByACH", IsFundsClientByACH);
            objSqlCommand.Parameters.AddWithValue("@IsEndofMonthInvoice", IsEndofMonthInvoice);
            objSqlCommand.Parameters.AddWithValue("@IsEndofMonthFunds", IsEndofMonthFunds);
            objSqlCommand.Parameters.AddWithValue("@NameOfBank", NameOfBank);
            objSqlCommand.Parameters.AddWithValue("@RoutingNumber", RoutingNumber);
            objSqlCommand.Parameters.AddWithValue("@AccountNumber", AccountNumber);
            objSqlCommand.Parameters.AddWithValue("@Name1", Name1);
            objSqlCommand.Parameters.AddWithValue("@EmailAddress1", EmailAddress1);
            objSqlCommand.Parameters.AddWithValue("@Name2", Name2);
            objSqlCommand.Parameters.AddWithValue("@EmailAddress2", EmailAddress2);
            objSqlCommand.Parameters.AddWithValue("@Name3", Name3);
            objSqlCommand.Parameters.AddWithValue("@EmailAddress3", EmailAddress3);
            objSqlCommand.Parameters.AddWithValue("@Name4", Name4);
            objSqlCommand.Parameters.AddWithValue("@EmailAddress4", EmailAddress4);
            objSqlCommand.Parameters.AddWithValue("@Name5", Name5);
            objSqlCommand.Parameters.AddWithValue("@EmailAddress5", EmailAddress5);

            ds = objclsConnection.ExecuteDataSet(objSqlCommand);

            if (ds == null || ds.Tables.Count == 0 || ds.Tables[0]==null || ds.Tables[0].Rows.Count==0)
            {
                return 0;
            }
            return Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString());
        }

        public DataTable SelectACHDeductFees()
        {
            DataSet ds = new DataSet();
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("usp_tblACHDeductFees_Select");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            objSqlCommand.Parameters.AddWithValue("@ID", ID);

            ds = objclsConnection.ExecuteDataSet(objSqlCommand);

            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }
            return ds.Tables[0];
        }
    }
}