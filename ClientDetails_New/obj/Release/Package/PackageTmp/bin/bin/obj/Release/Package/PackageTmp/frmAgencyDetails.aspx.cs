using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClientDetails.App_Code;

namespace ClientDetails
{
    public partial class frmAgencyDetails : System.Web.UI.Page
    {
        clsAgency objclsAgency = new clsAgency();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            objclsAgency = new clsAgency();
            objclsAgency.Name = txtNameOfAgency.Text.Trim();
            objclsAgency.PersonInterested = txtPersonInterested.Text.Trim();
            objclsAgency.EmailAddress = txtEmailAddress.Text.Trim();
            objclsAgency.InsertAgency();

            ClientScript.RegisterStartupScript(GetType(), "myscript", "alert('Details inserted successfully');", true);
        }
    }
}