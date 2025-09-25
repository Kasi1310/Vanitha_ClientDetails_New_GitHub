<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAgencyDetails.aspx.cs" Inherits="ClientDetails.frmAgencyDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-3.0.0.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <script src="Scripts/Custom/Custom.js"></script>
    <style>
        body, html {
            height: 100%;
        }

        .border-10 {
            border-style: solid;
            border-width: 2px;
        }
    </style>
    <style>
        body {
            font-family: Calibri;
            font-size: 16px;
        }
    </style>
</head>
<body>
    <div class="container h-100">
        <div class="row h-100 justify-content-center align-items-center">
            <form class="col-lg-12" runat="server">
                <div class="col-lg-12 justify-content-center align-items-center">
                    <img class="navbar-brand" src="Images/Logo.jpg" />
                </div>
                <div class="col-lg-12 form-group border-info border-10" style="padding-left: 0px; padding-right: 0px; padding-top: 10px;">
                    <div class="col-lg-12 form-group">
                        <div class="col-lg-2 d-inline-block">
                            <label>Name of  Agency:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtNameOfAgency" runat="server" CssClass="form-control" Text="" MaxLength="50"
                                autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="col-lg-2 d-inline-block">
                            <label>Person Interested:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtPersonInterested" runat="server" CssClass="form-control" Text="" MaxLength="50" autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <div class="col-lg-2 d-inline-block">
                            <label>Email Address:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="form-control" Text=""
                                MaxLength="50" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="col-lg-1 d-inline-block">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-info custom"
                                OnClick="btnSubmit_Click" OnClientClick="return Validation();" />
                        </div>
                    </div>
                </div>
            </form>
        </div>
    </div>
    <script type="text/javascript">
        function Validation() {
            var txtNameOfAgency = document.getElementById("<%=txtNameOfAgency.ClientID %>");
            var txtPersonInterested = document.getElementById("<%=txtPersonInterested.ClientID %>");
            var txtEmailAddress = document.getElementById("<%=txtEmailAddress.ClientID %>");

            if (txtNameOfAgency.value.trim() == "") {
                alert("Enter Name Of Agency");
                txtNameOfAgency.focus();
                return false;
            }

            if (txtPersonInterested.value.trim() == "") {
                alert("Enter Person Interested");
                txtPersonInterested.focus();
                return false;
            }

            if (txtEmailAddress.value.trim() == "") {
                alert("Enter Email Address");
                txtEmailAddress.focus();
                return false;
            }

            if (!ValidateEmail(txtEmailAddress, "Invalid Email Address")) {
                return false;
            }
        }
    </script>
</body>
</html>
