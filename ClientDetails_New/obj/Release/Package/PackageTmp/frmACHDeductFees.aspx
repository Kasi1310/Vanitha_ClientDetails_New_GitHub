<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmACHDeductFees.aspx.cs" Inherits="ClientDetails.frmACHDeductFees" %>

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
            border: 3pt solid #009094;
        }
    </style>
    <style>
        body {
            font-family: Calibri;
            font-size: 16px;
        }
    </style>

    <style>
        .custom-checkbox input {
            display: none;
        }

        .custom-checkbox label span {
            border: 3px solid #7e8a94;
            float: right;
            height: 30px;
            width: 60px;
            border-radius: 5px;
            cursor: pointer;
            display: flex;
            justify-content: center;
            align-items: center;
            margin-right: 10px;
            margin-left: 10px;
        }

        .custom-checkbox:hover span,
        .custom-checkbox input:checked + label span {
            border: 3px solid #7e8a94;
        }

            .custom-checkbox input:checked + label span:before {
                content: "✔";
                /*position: absolute;*/
                font-size: 1.6em;
                line-height: 0.8;
                transition: all .2s;
            }
    </style>
</head>
<body>
    <div class="container h-100">
        <div class="row h-100 justify-content-center align-items-center">
            <form class="col-lg-12" runat="server">
                <div class="col-lg-12 form-group border-info border-10" style="padding-left: 0px; padding-right: 0px; padding-top: 10px;">
                    <div class="col-lg-12 justify-content-center align-items-center" style="text-align: center">
                        <img class="navbar-brand" src="Images/Logo.jpg" />
                    </div>
                    <div class="col-lg-12 form-group" style="font-family: Calibri; font-size: 24px; font-weight: bold; color: #009094; text-align: center;">
                        ACH-UPLOAD END OF MONTH INVOICES & SNAPSHOTS-DEDUCT FEES FORM
                    </div>
                    <div class="col-lg-12 form-group">
                        <asp:HiddenField ID="hdnID" runat="server" Value="0" />

                        <asp:BulletedList ID="BulletedList1" runat="server">
                            <asp:ListItem>Please enter the client’s name.</asp:ListItem>
                            <asp:ListItem>Center a checkmark on #1,#2, #3</asp:ListItem>
                            <asp:ListItem>Enter Ach Information</asp:ListItem>
                            <asp:ListItem>Enter name and email address to whom End of Month Invoices & Snapshots will be sent.</asp:ListItem>
                        </asp:BulletedList>

                    </div>
                    <div class="col-lg-12 form-group">
                        <div class="col-lg-3 d-inline-block" style="padding-right:0px !important;">
                            <label style="font-weight: bold;">Medicount’ s Client (name of client)</label>
                        </div>
                        <div class="col-lg-4 d-inline-block" style="padding-left:0px !important;padding-right:0px !important;">
                            <asp:TextBox ID="txtClientName" runat="server" CssClass="form-control" Text="" MaxLength="50"
                                autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="col-lg-4 d-inline-block" style="padding-left:0px !important;padding-right:0px !important;">
                            <label style="font-weight: bold;">,agrees to the following:</label>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <div class="col-lg-5 d-inline-block" style="vertical-align: top;">
                            <label style="font-weight: bold;">1.	Allow Medicount to send all due funds client by ACH. </label>
                        </div>
                        <div class="col-lg-4 d-inline-block">
                            <asp:RadioButtonList ID="rdolstFundsClientByACH" runat="server" RepeatDirection="Horizontal" CssClass="custom-checkbox" Style="font-weight: bold;">
                                <asp:ListItem Value="true">YES<span></span></asp:ListItem>
                                <asp:ListItem Value="false">NO<span></span></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>

                    <div class="col-lg-12 form-group">
                        <div class="col-lg-5 d-inline-block" style="vertical-align: top;">
                            <label style="font-weight: bold;">2.	Allow Medicount to upload all End of Month Invoices & Snapshots through Medicount’ s Customer Portal.  </label>
                        </div>
                        <div class="col-lg-4 d-inline-block">
                            <asp:RadioButtonList ID="rdolstEndofMonthInvoice" runat="server" RepeatDirection="Horizontal" CssClass="custom-checkbox" Style="font-weight: bold;">
                                <asp:ListItem Value="true">YES<span></span></asp:ListItem>
                                <asp:ListItem Value="false">NO<span></span></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>

                    <div class="col-lg-12 form-group">
                        <div class="col-lg-5 d-inline-block" style="vertical-align: top;">
                            <label style="font-weight: bold;">3.	Allow Medicount to deduct their monthly fees from client’s End of Month funds due them by check or ACH.   </label>
                        </div>
                        <div class="col-lg-4 d-inline-block">
                            <asp:RadioButtonList ID="rdolstEndofMonthFunds" runat="server" RepeatDirection="Horizontal" CssClass="custom-checkbox" Style="font-weight: bold;">
                                <asp:ListItem Value="true">YES<span></span></asp:ListItem>
                                <asp:ListItem Value="false">NO<span></span></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <div class="col-lg-12 form-group" style="font-family: Calibri; font-size: 20px; font-weight: bold; color: #009094;">
                            ACH Information:
                        </div>

                        <div class="col-lg-12 form-group" style="font-weight: bold;">
                            Exact Name of the entity on Bank Account
                        </div>
                        <div class="col-lg-12 form-group">
                            <table border="1" style="width: 80%;">
                                <tr>
                                    <td style="color: #009094; width: 20%; padding-left: 10px; font-weight: bold;">Name Of Bank:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNameOfBank" runat="server" CssClass="form-control" Text="" MaxLength="50"
                                            autocomplete="off"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="color: #009094; width: 30%; padding-left: 10px; font-weight: bold;">Routing Number:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRoutingNumber" runat="server" CssClass="form-control" Text="" MaxLength="50"
                                            autocomplete="off"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="color: #009094; width: 30%; padding-left: 10px; font-weight: bold;">Account Number:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAccountNumber" runat="server" CssClass="form-control" Text="" MaxLength="50"
                                            autocomplete="off"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="col-lg-12 form-group" style="font-weight: bold;">
                            Name and email address to upload End of Month Invoices & Snapshots
                        </div>
                        <div class="col-lg-12 form-group">
                            <table border="1" style="width: 80%;">
                                <tr>
                                    <td style="color: #009094; font-weight: bold; width: 4%; text-align: center;">S.No
                                    </td>
                                    <td style="color: #009094; font-weight: bold; width: 43%; text-align: center;">Name
                                    </td>
                                    <td style="color: #009094; font-weight: bold; width: 43%; text-align: center;">Email Address
                                    </td>
                                </tr>

                                <tr>
                                    <td style="text-align: center;">1.
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtName1" runat="server" CssClass="form-control" Text="" MaxLength="50"
                                            autocomplete="off"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmailAddress1" runat="server" CssClass="form-control" Text="" MaxLength="50"
                                            autocomplete="off"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center;">2.
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtName2" runat="server" CssClass="form-control" Text="" MaxLength="50"
                                            autocomplete="off"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmailAddress2" runat="server" CssClass="form-control" Text="" MaxLength="50"
                                            autocomplete="off"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center;">3.
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtName3" runat="server" CssClass="form-control" Text="" MaxLength="50"
                                            autocomplete="off"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmailAddress3" runat="server" CssClass="form-control" Text="" MaxLength="50"
                                            autocomplete="off"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center;">4.
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtName4" runat="server" CssClass="form-control" Text="" MaxLength="50"
                                            autocomplete="off"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmailAddress4" runat="server" CssClass="form-control" Text="" MaxLength="50"
                                            autocomplete="off"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: center;">5.
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtName5" runat="server" CssClass="form-control" Text="" MaxLength="50"
                                            autocomplete="off"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEmailAddress5" runat="server" CssClass="form-control" Text="" MaxLength="50"
                                            autocomplete="off"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div align="center" class="col-lg-12 form-group">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-info custom"
                            OnClick="btnSubmit_Click" OnClientClick="return Validation();" />
                    </div>
                </div>
            </form>
        </div>
    </div>
    <script type="text/javascript">
        function Validation() {
            var txtClientName = document.getElementById("<%=txtClientName.ClientID %>");
            var rdolstFundsClientByACH = document.getElementById("<%=rdolstFundsClientByACH.ClientID %>").getElementsByTagName("input");
            var rdolstEndofMonthInvoice = document.getElementById("<%=rdolstEndofMonthInvoice.ClientID %>").getElementsByTagName("input");
            var rdolstEndofMonthFunds = document.getElementById("<%=rdolstEndofMonthFunds.ClientID %>").getElementsByTagName("input");
            var txtNameOfBank = document.getElementById("<%=txtNameOfBank.ClientID %>");
            var txtRoutingNumber = document.getElementById("<%=txtRoutingNumber.ClientID %>");
            var txtAccountNumber = document.getElementById("<%=txtAccountNumber.ClientID %>");


            var txtName1 = document.getElementById("<%=txtName1.ClientID %>");
            var txtEmailAddress1 = document.getElementById("<%=txtEmailAddress1.ClientID %>");
            var txtEmailAddress2 = document.getElementById("<%=txtEmailAddress2.ClientID %>");
            var txtEmailAddress3 = document.getElementById("<%=txtEmailAddress3.ClientID %>");
            var txtEmailAddress4 = document.getElementById("<%=txtEmailAddress4.ClientID %>");
            var txtEmailAddress5 = document.getElementById("<%=txtEmailAddress5.ClientID %>");

            if (txtClientName.value.trim() == "") {
                alert("Enter client name");
                txtClientName.focus();
                return false;
            }

            if (!rdolstFundsClientByACH[0].checked && !rdolstFundsClientByACH[1].checked) {
                alert("Select Allow Medicount to send all due funds client by ACH.");
                return false;
            }

            if (!rdolstEndofMonthInvoice[0].checked && !rdolstEndofMonthInvoice[1].checked) {
                alert("Select Allow Medicount to upload all End of Month Invoices & Snapshots through Medicount’ s Customer Portal.");
                return false;
            }

            if (!rdolstEndofMonthFunds[0].checked && !rdolstEndofMonthFunds[1].checked) {
                alert("Allow Medicount to deduct their monthly fees from client’s End of Month funds due them by check or ACH.");
                return false;
            }

            if (txtNameOfBank.value.trim() == "") {
                alert("Enter name of bank");
                txtNameOfBank.focus();
                return false;
            }
            if (txtRoutingNumber.value.trim() == "") {
                alert("Enter routing number");
                txtRoutingNumber.focus();
                return false;
            }
            if (txtAccountNumber.value.trim() == "") {
                alert("Enter account number");
                txtAccountNumber.focus();
                return false;
            }

            if (txtName1.value.trim() == "") {
                alert("Enter name");
                txtName1.focus();
                return false;
            }

            if (txtEmailAddress1.value.trim() == "") {
                alert("Enter email address");
                txtEmailAddress1.focus();
                return false;
            }

            if (!ValidateEmail(txtEmailAddress1, "Invalid email address")) {
                return false;
            }

            if (txtEmailAddress2.value.trim() != "" && !ValidateEmail(txtEmailAddress2, "Invalid email address")) {
                return false;
            }
            if (txtEmailAddress3.value.trim() != "" && !ValidateEmail(txtEmailAddress3, "Invalid email address")) {
                return false;
            }
            if (txtEmailAddress4.value.trim() != "" && !ValidateEmail(txtEmailAddress4, "Invalid email address")) {
                return false;
            }
            if (txtEmailAddress5.value.trim() != "" && !ValidateEmail(txtEmailAddress5, "Invalid email address")) {
                return false;
            }






        }
    </script>
</body>
</html>
