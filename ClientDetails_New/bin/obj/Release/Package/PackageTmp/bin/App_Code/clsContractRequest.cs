using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ClientDetails.App_Code
{
    public class clsContractRequest
    {
        SqlCommand objSqlCommand;
        clsConnection objclsConnection;
        public int ID { get; set; }
        public string ClientName { get; set; }
        public string AEName { get; set; }
        public string NameOfEntity { get; set; }
        public string ClientServicesAgreement { get; set; }
        public string FeeRecommended { get; set; }
        public string LengthOfClientServicesAgreement { get; set; }
        public string AdditionalEquipment { get; set; }
        public string ALSProtocalRequiredCSAProtocol { get; set; }
        public string AddressOfEntity { get; set; }
        public string CountyName { get; set; }
        public string EmailAddress { get; set; }
        public string ChangeHealthcareClient { get; set; }
        public string OtherComments { get; set; }
        public string LastUpdatedBy { get; set; }
        public string ChiefName { get; set; }
        public string State { get; set; }


        public int InsertClientContractRequest()
        {
            DataSet ds = new DataSet();
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("USP_tblClientContractRequest_Insert");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            objSqlCommand.Parameters.AddWithValue("@ID", ID);
            objSqlCommand.Parameters.AddWithValue("@ClientName", ClientName);
            objSqlCommand.Parameters.AddWithValue("@AEName", AEName);
            objSqlCommand.Parameters.AddWithValue("@ChiefName", ChiefName);
            objSqlCommand.Parameters.AddWithValue("@NameOfEntity", NameOfEntity);
            objSqlCommand.Parameters.AddWithValue("@ClientServicesAgreement", ClientServicesAgreement);
            objSqlCommand.Parameters.AddWithValue("@FeeRecommended", FeeRecommended);
            objSqlCommand.Parameters.AddWithValue("@LengthOfClientServicesAgreement", LengthOfClientServicesAgreement);
            objSqlCommand.Parameters.AddWithValue("@AdditionalEquipment", AdditionalEquipment);
            objSqlCommand.Parameters.AddWithValue("@ALSProtocalRequiredCSAProtocol", ALSProtocalRequiredCSAProtocol);
            objSqlCommand.Parameters.AddWithValue("@AddressOfEntity", AddressOfEntity);
            objSqlCommand.Parameters.AddWithValue("@CountyName", CountyName);
            objSqlCommand.Parameters.AddWithValue("@State", State);
            objSqlCommand.Parameters.AddWithValue("@EmailAddress", EmailAddress);
            objSqlCommand.Parameters.AddWithValue("@ChangeHealthcareClient", ChangeHealthcareClient);
            objSqlCommand.Parameters.AddWithValue("@OtherComments", OtherComments);
            objSqlCommand.Parameters.AddWithValue("@LastUpdatedBy", LastUpdatedBy);

            ds = new DataSet();
            ds = objclsConnection.ExecuteDataSet(objSqlCommand);

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count != 0)
            {
                return int.Parse(ds.Tables[0].Rows[0][0].ToString().Trim());
            }

            return 0;
        }
        public DataSet SelectClientContractRequest()
        {
            DataSet ds = new DataSet();
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("USP_tblClientContractRequest_Select");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            objSqlCommand.Parameters.AddWithValue("@ID", ID);

            return objclsConnection.ExecuteDataSet(objSqlCommand);
        }
    }
}