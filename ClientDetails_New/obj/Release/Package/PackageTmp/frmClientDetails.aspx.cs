using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClientDetails.App_Code;

namespace ClientDetails
{
    public partial class frmClientDetails : System.Web.UI.Page
    {
        clsClientDetails objclsClientDetails;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                clsCommon objclsCommon = new clsCommon();
                string queryString = Request.QueryString["ClientID"].ToString();

                hdnClientID.Value = objclsCommon.Decrypt(queryString);


                if (!IsPostBack)
                {
                    LoadClientDetails();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(GetType(), "myscript", "alert('Invalid Link');", true);
            }
        }

        private void LoadClientDetails()
        {
            objclsClientDetails = new clsClientDetails();
            DataSet ds = new DataSet();

            objclsClientDetails.ClientID = int.Parse(hdnClientID.Value.Trim());
            ds = objclsClientDetails.SelectClientDetails();
            if (ds == null || ds.Tables.Count != 3)
            {
                ClientScript.RegisterStartupScript(GetType(), "myscript", "alert('Invalid Link');", true);
            }
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                txtClientNo.Text = ds.Tables[0].Rows[0]["ClientID"].ToString();
                txtClientName.Text = ds.Tables[0].Rows[0]["ClientName"].ToString();
                txtAccountExecutive.Text = ds.Tables[0].Rows[0]["AccountExecutive"].ToString();

                txtClientNo.Attributes.Add("ReadOnly", "ReadOnly");
                txtClientName.Attributes.Add("ReadOnly", "ReadOnly");
                txtAccountExecutive.Attributes.Add("ReadOnly", "ReadOnly");

                if (bool.Parse(ds.Tables[0].Rows[0]["IsUpdated"].ToString()))
                {
                    btnSubmit.Style.Add("display", "none");
                    btnCancel.Style.Add("display", "none");
                    lblMessage.Style.Add("display", "block");
                }
            }
            if (ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
            {
                hdnBankDetails.Value = ds.Tables[1].Rows[0]["ID"].ToString();
                txtBankName.Text = ds.Tables[1].Rows[0]["BankName"].ToString();
                txtRoutingNumber.Text = ds.Tables[1].Rows[0]["RoutingNumber"].ToString();
                txtAccountNumber.Text = ds.Tables[1].Rows[0]["AccountNumber"].ToString();
                txtAccountNumber1.Text = ds.Tables[1].Rows[0]["AccountNumber"].ToString();
            }
            if (ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        hdnContactPerson1.Value = ds.Tables[2].Rows[i]["ID"].ToString();
                        txtContactPerson1.Text = ds.Tables[2].Rows[i]["ContactPerson"].ToString();
                        txtTitle1.Text = ds.Tables[2].Rows[i]["Title"].ToString();
                        txtEmailID1.Text = ds.Tables[2].Rows[i]["EmailID"].ToString();
                        txtPhone1.Text = ds.Tables[2].Rows[i]["Phone"].ToString();
                    }
                    if (i == 1)
                    {
                        hdnContactPerson2.Value = ds.Tables[2].Rows[i]["ID"].ToString();
                        txtContactPerson2.Text = ds.Tables[2].Rows[i]["ContactPerson"].ToString();
                        txtTitle2.Text = ds.Tables[2].Rows[i]["Title"].ToString();
                        txtEmailID2.Text = ds.Tables[2].Rows[i]["EmailID"].ToString();
                        txtPhone2.Text = ds.Tables[2].Rows[i]["Phone"].ToString();
                    }
                    if (i == 2)
                    {
                        hdnContactPerson3.Value = ds.Tables[3].Rows[i]["ID"].ToString();
                        txtContactPerson3.Text = ds.Tables[2].Rows[i]["ContactPerson"].ToString();
                        txtTitle3.Text = ds.Tables[2].Rows[i]["Title"].ToString();
                        txtEmailID3.Text = ds.Tables[2].Rows[i]["EmailID"].ToString();
                        txtPhone3.Text = ds.Tables[2].Rows[i]["Phone"].ToString();
                    }
                    if (i == 3)
                    {
                        hdnContactPerson4.Value = ds.Tables[2].Rows[i]["ID"].ToString();
                        txtContactPerson4.Text = ds.Tables[2].Rows[i]["ContactPerson"].ToString();
                        txtTitle4.Text = ds.Tables[2].Rows[i]["Title"].ToString();
                        txtEmailID4.Text = ds.Tables[2].Rows[i]["EmailID"].ToString();
                        txtPhone4.Text = ds.Tables[2].Rows[i]["Phone"].ToString();
                    }
                    if (i == 4)
                    {
                        hdnContactPerson5.Value = ds.Tables[2].Rows[i]["ID"].ToString();
                        txtContactPerson5.Text = ds.Tables[2].Rows[i]["ContactPerson"].ToString();
                        txtTitle5.Text = ds.Tables[2].Rows[i]["Title"].ToString();
                        txtEmailID5.Text = ds.Tables[2].Rows[i]["EmailID"].ToString();
                        txtPhone5.Text = ds.Tables[2].Rows[i]["Phone"].ToString();
                    }
                }
            }
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //Insert Bank Details
            objclsClientDetails = new clsClientDetails();

            objclsClientDetails.ClientID = int.Parse(hdnClientID.Value.Trim());

            objclsClientDetails.ID = int.Parse(hdnBankDetails.Value.Trim());
            objclsClientDetails.BankName = txtBankName.Text.Trim();
            objclsClientDetails.RoutingNumber = txtRoutingNumber.Text.Trim();
            objclsClientDetails.AccountNumber = txtAccountNumber.Text.Trim();
            objclsClientDetails.InsertBankDetails();

            InsertOtherDetails(hdnContactPerson1.Value.Trim(), txtContactPerson1.Text.Trim(), txtTitle1.Text.Trim(), txtEmailID1.Text.Trim(), txtPhone1.Text.Trim());

            if (txtContactPerson2.Text.Trim() != "" && txtTitle2.Text.Trim() != ""
                && txtEmailID2.Text.Trim() != "" && txtPhone2.Text.Trim() != "")
            {
                InsertOtherDetails(hdnContactPerson2.Value.Trim(), txtContactPerson2.Text.Trim(), txtTitle2.Text.Trim(), txtEmailID2.Text.Trim(), txtPhone2.Text.Trim());
            }

            if (txtContactPerson3.Text.Trim() != "" && txtTitle3.Text.Trim() != ""
               && txtEmailID3.Text.Trim() != "" && txtPhone3.Text.Trim() != "")
            {
                InsertOtherDetails(hdnContactPerson3.Value.Trim(), txtContactPerson3.Text.Trim(), txtTitle3.Text.Trim(), txtEmailID3.Text.Trim(), txtPhone3.Text.Trim());
            }

            if (txtContactPerson4.Text.Trim() != "" && txtTitle4.Text.Trim() != ""
               && txtEmailID4.Text.Trim() != "" && txtPhone4.Text.Trim() != "")
            {
                InsertOtherDetails(hdnContactPerson4.Value.Trim(), txtContactPerson4.Text.Trim(), txtTitle4.Text.Trim(), txtEmailID4.Text.Trim(), txtPhone4.Text.Trim());
            }

            if (txtContactPerson5.Text.Trim() != "" && txtTitle5.Text.Trim() != ""
               && txtEmailID5.Text.Trim() != "" && txtPhone5.Text.Trim() != "")
            {
                InsertOtherDetails(hdnContactPerson5.Value.Trim(), txtContactPerson5.Text.Trim(), txtTitle5.Text.Trim(), txtEmailID5.Text.Trim(), txtPhone5.Text.Trim());
            }

            objclsClientDetails.ClientID = int.Parse(hdnClientID.Value.Trim());
            objclsClientDetails.UpdateClients("Update");

            Response.Redirect(Request.Url.AbsoluteUri);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.AbsoluteUri);
        }



        private void InsertOtherDetails(string ID, string ContactPerson, string Title, string EmailID, string Phone)
        {
            objclsClientDetails = new clsClientDetails();
            objclsClientDetails.ID = int.Parse(ID);
            objclsClientDetails.ClientID = int.Parse(hdnClientID.Value.Trim());
            objclsClientDetails.ContactPerson = ContactPerson;
            objclsClientDetails.Title = Title;
            objclsClientDetails.EmailID = EmailID;
            objclsClientDetails.Phone = Phone;
            objclsClientDetails.InsertOtherDetails();
        }



    }
}