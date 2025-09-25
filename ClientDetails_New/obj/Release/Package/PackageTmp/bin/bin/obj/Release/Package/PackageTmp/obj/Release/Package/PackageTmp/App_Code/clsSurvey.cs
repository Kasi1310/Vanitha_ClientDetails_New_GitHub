using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ClientDetails.App_Code
{
    public class clsSurvey
    {
       // string connString = ConfigurationManager.ConnectionStrings["MyConnectionStringMeetingAgenda"].ToString();

        DataSet ds;

        SqlCommand objSqlCommand;
        clsConnection objclsConnection;

        public int ID { get; set; }
        public int AttendeesID { get; set; }
        public string BillingActivity { get; set; }
        public string AnswerAllQuestion { get; set; }
        public string MeetingRevenueExpectation { get; set; }
        public string Comments { get; set; }


        public DataTable InsertSurvey()
        {
            ds = new DataSet();
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("USP_tblSurvey_InsertUpdate");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            objSqlCommand.Parameters.AddWithValue("@AttendeesID", AttendeesID);
            objSqlCommand.Parameters.AddWithValue("@BillingActivity", BillingActivity);
            objSqlCommand.Parameters.AddWithValue("@AnswerAllQuestion", AnswerAllQuestion);
            objSqlCommand.Parameters.AddWithValue("@MeetingRevenueExpectation", MeetingRevenueExpectation);
            objSqlCommand.Parameters.AddWithValue("@Comments", Comments);

            ds = objclsConnection.ExecuteDataSet(objSqlCommand);

            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }
            return ds.Tables[0];
        }

        public DataTable CheckSurveyFilled()
        {
           
            ds = new DataSet();
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("USP_tblSurvey_CheckSurveyFilled");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            objSqlCommand.Parameters.AddWithValue("@AttendeesID", AttendeesID);

            ds = objclsConnection.ExecuteDataSet(objSqlCommand);

            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }
            return ds.Tables[0];
        }

        public DataTable InsertSurveyTest()
        {
            ds = new DataSet();
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("USP_tblSurvey_InsertUpdate_Test");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            objSqlCommand.Parameters.AddWithValue("@AttendeesID", AttendeesID);
            objSqlCommand.Parameters.AddWithValue("@BillingActivity", BillingActivity);
            objSqlCommand.Parameters.AddWithValue("@AnswerAllQuestion", AnswerAllQuestion);
            objSqlCommand.Parameters.AddWithValue("@MeetingRevenueExpectation", MeetingRevenueExpectation);
            objSqlCommand.Parameters.AddWithValue("@Comments", Comments);

            ds = objclsConnection.ExecuteDataSet(objSqlCommand);

            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }
            return ds.Tables[0];
        }

        public DataTable CheckSurveyFilledTest()
        {

            ds = new DataSet();
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("USP_tblSurvey_CheckSurveyFilled_Test");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            objSqlCommand.Parameters.AddWithValue("@AttendeesID", AttendeesID);

            ds = objclsConnection.ExecuteDataSet(objSqlCommand);

            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }
            return ds.Tables[0];
        }
    }
}