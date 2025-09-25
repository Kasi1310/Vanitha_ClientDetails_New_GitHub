using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ClientDetails.App_Code
{
    public class clsMedicountClientSurvey
    {
        SqlCommand objSqlCommand;
        clsConnection objclsConnection;
        public int ID { get; set; }

        public int ClientID { get; set; }
        public string Question1 { get; set; }
        public string Question2 { get; set; }
        public string Question3 { get; set; }
        public string Question4 { get; set; }
        public string Question5 { get; set; }
        public string Question6 { get; set; }
        public string Question7 { get; set; }
        public string Question8a { get; set; }
        public string Question8b { get; set; }
        public string Question8c { get; set; }
        public string Question8d { get; set; }
        public string Question9 { get; set; }
        public string Question10Name { get; set; }
        public string Question10AgencyName { get; set; }
        public string Question10Email { get; set; }
        public string Question10Phone { get; set; }

        public void InsertMedicountClientSurvey()
        {
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("USP_tblMedicountClientSurvey_Insert");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            objSqlCommand.Parameters.AddWithValue("@ClientID", ClientID);
            objSqlCommand.Parameters.AddWithValue("@Question1", Question1);
            objSqlCommand.Parameters.AddWithValue("@Question2", Question2);
            objSqlCommand.Parameters.AddWithValue("@Question3", Question3);
            objSqlCommand.Parameters.AddWithValue("@Question4", Question4);
            objSqlCommand.Parameters.AddWithValue("@Question5", Question5);
            objSqlCommand.Parameters.AddWithValue("@Question6", Question6);
            objSqlCommand.Parameters.AddWithValue("@Question7", Question7);
            objSqlCommand.Parameters.AddWithValue("@Question8a", Question8a);
            objSqlCommand.Parameters.AddWithValue("@Question8b", Question8b);
            objSqlCommand.Parameters.AddWithValue("@Question8c", Question8c);
            objSqlCommand.Parameters.AddWithValue("@Question8d", Question8d);
            objSqlCommand.Parameters.AddWithValue("@Question9", Question9);
            objSqlCommand.Parameters.AddWithValue("@Question10Name", Question10Name);
            objSqlCommand.Parameters.AddWithValue("@Question10AgencyName", Question10AgencyName);
            objSqlCommand.Parameters.AddWithValue("@Question10Email", Question10Email);
            objSqlCommand.Parameters.AddWithValue("@Question10Phone", Question10Phone);

            objclsConnection.ExecuteNonQuery(objSqlCommand);
        }

        public DataTable SelectClientDetails()
        {
            DataSet ds = new DataSet();
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("USP_tblMedicountClientSurveyEmailList_Select");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            ds = objclsConnection.ExecuteDataSet(objSqlCommand);

            if(ds==null || ds.Tables.Count==0)
            {
                return null;
            }
            return ds.Tables[0];
        }

        public DataTable CheckSurveyFilled()
        {
            DataSet ds = new DataSet();
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("USP_tblMedicountClientSurvey_CheckSurveyFilled");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            objSqlCommand.Parameters.AddWithValue("@ClientID", ClientID);

            ds = objclsConnection.ExecuteDataSet(objSqlCommand);

            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }
            return ds.Tables[0];
        }
        public DataTable SelectMedicountClientSurvey()
        {
            DataSet ds = new DataSet();
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("USP_tblMedicountClientSurvey_Select");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            ds = objclsConnection.ExecuteDataSet(objSqlCommand);

            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }
            return ds.Tables[0];
        }

        public void UpdateMailSendStatus()
        {
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("USP_tblMedicountClientSurveyEmailList_UpdateMailStatus");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            objSqlCommand.Parameters.AddWithValue("@ID", ID);

            objclsConnection.ExecuteNonQuery(objSqlCommand);
        }
    }
}