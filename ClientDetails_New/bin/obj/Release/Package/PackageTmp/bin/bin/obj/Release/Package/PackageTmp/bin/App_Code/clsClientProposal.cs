using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ClientDetails.App_Code
{
    public class clsClientProposal
    {
        SqlCommand objSqlCommand;
        clsConnection objclsConnection;
        public int ID { get; set; }
        public string ClientName { get; set; }
        public string AEName { get; set; }
        public string NameOfEntity { get; set; }
        public string ReferralFrom { get; set; }
        public string IsChangeHealthCareClient { get; set; }
        public string IsALSDispatchProtocol { get; set; }
        public string ImageOfPotentialClientsAmbulance { get; set; }
        public string ImageOfPotentialClientsEmblem { get; set; }
        public string NamesAndTitleOfIndividuals { get; set; }
        public string AddressOfEntity { get; set; }
        public string CountyName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string NumberOfProposalsRequested { get; set; }
        public string ApproximateDateOfProposal { get; set; }
        public string ProposalsMailedDirectly { get; set; }
        public string FeeRecommended { get; set; }
        public string AdditionalEquipment { get; set; }
        public string LengthOfClientServicesAgreement { get; set; }
        public string ChangeHealthCareClient { get; set; }
        public string ALSProtocalRequiredCSAProtocol { get; set; }
        public string IsEPCROrEquipmentCharges { get; set; }
        public string CopyOfTheCurrentBillersContract { get; set; }
        public string AttachedTransportAndRevenue { get; set; }
        public string AnyPassThroughCharges { get; set; }
        public string OtherComments { get; set; }
        public string LastUpdatedBy { get; set; }

        public int InsertClientProposal()
        {
            DataSet ds = new DataSet();
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("USP_tblClientProposal_Insert");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            objSqlCommand.Parameters.AddWithValue("@ID", ID);
            objSqlCommand.Parameters.AddWithValue("@ClientName", ClientName);
            objSqlCommand.Parameters.AddWithValue("@AEName", AEName);
            objSqlCommand.Parameters.AddWithValue("@NameOfEntity", NameOfEntity);
            objSqlCommand.Parameters.AddWithValue("@ReferralFrom", ReferralFrom);
            objSqlCommand.Parameters.AddWithValue("@IsChangeHealthCareClient", IsChangeHealthCareClient);
            objSqlCommand.Parameters.AddWithValue("@IsALSDispatchProtocol", IsALSDispatchProtocol);
            objSqlCommand.Parameters.AddWithValue("@ImageOfPotentialClientsAmbulance", ImageOfPotentialClientsAmbulance);
            objSqlCommand.Parameters.AddWithValue("@ImageOfPotentialClientsEmblem", ImageOfPotentialClientsEmblem);
            objSqlCommand.Parameters.AddWithValue("@NamesAndTitleOfIndividuals", NamesAndTitleOfIndividuals);
            objSqlCommand.Parameters.AddWithValue("@AddressOfEntity", AddressOfEntity);
            objSqlCommand.Parameters.AddWithValue("@CountyName", CountyName);
            objSqlCommand.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
            objSqlCommand.Parameters.AddWithValue("@EmailAddress", EmailAddress);
            objSqlCommand.Parameters.AddWithValue("@NumberOfProposalsRequested", NumberOfProposalsRequested);
            objSqlCommand.Parameters.AddWithValue("@ApproximateDateOfProposal", ApproximateDateOfProposal);
            objSqlCommand.Parameters.AddWithValue("@ProposalsMailedDirectly", ProposalsMailedDirectly);
            objSqlCommand.Parameters.AddWithValue("@FeeRecommended", FeeRecommended);
            objSqlCommand.Parameters.AddWithValue("@AdditionalEquipment", AdditionalEquipment);
            objSqlCommand.Parameters.AddWithValue("@LengthOfClientServicesAgreement", LengthOfClientServicesAgreement);
            objSqlCommand.Parameters.AddWithValue("@ChangeHealthCareClient", ChangeHealthCareClient);
            objSqlCommand.Parameters.AddWithValue("@ALSProtocalRequiredCSAProtocol", ALSProtocalRequiredCSAProtocol);
            objSqlCommand.Parameters.AddWithValue("@IsEPCROrEquipmentCharges", IsEPCROrEquipmentCharges);
            objSqlCommand.Parameters.AddWithValue("@CopyOfTheCurrentBillersContract", CopyOfTheCurrentBillersContract);
            objSqlCommand.Parameters.AddWithValue("@AttachedTransportAndRevenue", AttachedTransportAndRevenue);
            objSqlCommand.Parameters.AddWithValue("@AnyPassThroughCharges", AnyPassThroughCharges);
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

        public void InsertClientProposalAttachment(int ClientProposalID, string AttachmentFor, string AttachmentFileName)
        {
            DataSet ds = new DataSet();
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("USP_tblClientProposalAttachment_Insert");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            objSqlCommand.Parameters.AddWithValue("@ClientProposalID", ClientProposalID);
            objSqlCommand.Parameters.AddWithValue("@AttachmentFor", AttachmentFor);
            objSqlCommand.Parameters.AddWithValue("@AttachmentFileName", AttachmentFileName);

            objclsConnection.ExecuteNonQuery(objSqlCommand);
        }

        public DataSet SelectClientProposal()
        {
            DataSet ds = new DataSet();
            objSqlCommand = new SqlCommand();
            objclsConnection = new clsConnection();

            objSqlCommand = new SqlCommand("USP_tblClientProposal_Select");
            objSqlCommand.CommandType = CommandType.StoredProcedure;

            objSqlCommand.Parameters.AddWithValue("@ID", ID);

            return objclsConnection.ExecuteDataSet(objSqlCommand);
        }
    }
}