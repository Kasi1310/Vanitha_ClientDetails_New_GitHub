using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ClientDetails.App_Code
{
    public class clsNoticeAnnouncement
    {
        SqlCommand objSqlCommand;
        clsConnection objclsConnection;
        public int ID { get; set; }
        public string Name { get; set; }
        public string EmailID { get; set; }
        public bool IsNotice { get; set; }
        public bool IsClientFireChief { get; set; }
        public bool IsAllClientContacts { get; set; }
        public bool IsClientEntireFire { get; set; }
        public bool IsClientFiscalOfficer { get; set; }
        public bool IsSpecificClientContactsZOHO { get; set; }
        //public bool IsALLPotentialClients { get; set; }
        public string Comments { get; set; }

        public void InsertNoticeAnnouncement()
        {
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("USP_tblNoticeAnnouncement_Insert");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            objSqlCommand.Parameters.AddWithValue("@Name", Name);
            objSqlCommand.Parameters.AddWithValue("@EmailID", EmailID);
            objSqlCommand.Parameters.AddWithValue("@IsNotice", IsNotice);
            objSqlCommand.Parameters.AddWithValue("@IsClientFireChief", IsClientFireChief);
            objSqlCommand.Parameters.AddWithValue("@IsAllClientContacts", IsAllClientContacts);
            objSqlCommand.Parameters.AddWithValue("@IsClientEntireFire", IsClientEntireFire);
            objSqlCommand.Parameters.AddWithValue("@IsClientFiscalOfficer", IsClientFiscalOfficer);
            objSqlCommand.Parameters.AddWithValue("@IsSpecificClientContactsZOHO", IsSpecificClientContactsZOHO);
            //objSqlCommand.Parameters.AddWithValue("@IsALLPotentialClients", IsALLPotentialClients);
            objSqlCommand.Parameters.AddWithValue("@Comments", Comments);

            objclsConnection.ExecuteNonQuery(objSqlCommand);
        }
    }
}