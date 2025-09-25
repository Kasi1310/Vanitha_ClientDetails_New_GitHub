using System;
using System.Data;
using System.Data.SqlClient;

namespace ClientDetails.App_Code
{
    public class clsClientDetails
    {
        SqlCommand objSqlCommand;
        clsConnection objclsConnection;

        public int ID { get; set; }
        public int ClientID { get; set; }
        public string ClientName { get; set; }
        public string AccountExecutive { get; set; }

        public string BankName { get; set; }
        public string RoutingNumber { get; set; }
        public string AccountNumber { get; set; }

        public string ContactPerson { get; set; }
        public string Title { get; set; }
        public string EmailID { get; set; }
        public string Phone { get; set; }

        public DataTable SelectClients()
        {
            DataSet ds = new DataSet();
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("USP_tblClients_Select");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            objSqlCommand.Parameters.AddWithValue("@ClientID", ClientID);

            ds= objclsConnection.ExecuteDataSet(objSqlCommand);

            if(ds==null || ds.Tables.Count==0)
            {
                return null;
            }
            return ds.Tables[0];
        }
        public DataTable InsertClients()
        {
            DataSet ds = new DataSet();
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("USP_tblClients_Insert");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            objSqlCommand.Parameters.AddWithValue("@ClientID", ClientID);
            objSqlCommand.Parameters.AddWithValue("@ClientName", ClientName);
            objSqlCommand.Parameters.AddWithValue("@AccountExecutive", AccountExecutive);
            objSqlCommand.Parameters.AddWithValue("@EmailID", EmailID);

            ds = objclsConnection.ExecuteDataSet(objSqlCommand);

            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }
            return ds.Tables[0];
        }


        public void UpdateClients(string Mode)
        {
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("USP_tblClients_Update");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            objSqlCommand.Parameters.AddWithValue("@Mode", Mode);
            objSqlCommand.Parameters.AddWithValue("@ClientID", ClientID);

            objclsConnection.ExecuteNonQuery(objSqlCommand);
        }

        public void InsertBankDetails()
        {
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("USP_tblBankDetails_Insert");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            objSqlCommand.Parameters.AddWithValue("@ID", ID);
            objSqlCommand.Parameters.AddWithValue("@ClientID", ClientID);
            objSqlCommand.Parameters.AddWithValue("@BankName", BankName);
            objSqlCommand.Parameters.AddWithValue("@RoutingNumber", RoutingNumber);
            objSqlCommand.Parameters.AddWithValue("@AccountNumber", AccountNumber);

            objclsConnection.ExecuteNonQuery(objSqlCommand);
        }

        public void InsertOtherDetails()
        {
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("USP_tblOtherDetails_INSERT");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            objSqlCommand.Parameters.AddWithValue("@ID", ID);
            objSqlCommand.Parameters.AddWithValue("@ClientID", ClientID);
            objSqlCommand.Parameters.AddWithValue("@ContactPerson", ContactPerson);
            objSqlCommand.Parameters.AddWithValue("@Title", Title);
            objSqlCommand.Parameters.AddWithValue("@EmailID", EmailID);
            objSqlCommand.Parameters.AddWithValue("@Phone", Phone);

            objclsConnection.ExecuteNonQuery(objSqlCommand);
        }

        public DataSet SelectClientDetails()
        {
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("USP_tblClientsDetails_Select");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            objSqlCommand.Parameters.AddWithValue("@ClientID", ClientID);

            return objclsConnection.ExecuteDataSet(objSqlCommand);
        }
    }
}