<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmClientDetails.aspx.cs" Inherits="ClientDetails.frmClientDetails" %>

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
                <asp:HiddenField ID="hdnClientID" runat="server" Value="" />
                <asp:HiddenField ID="hdnBankDetails" runat="server" Value="0" />
                <asp:HiddenField ID="hdnContactPerson1" runat="server" Value="0" />
                <asp:HiddenField ID="hdnContactPerson2" runat="server" Value="0" />
                <asp:HiddenField ID="hdnContactPerson3" runat="server" Value="0" />
                <asp:HiddenField ID="hdnContactPerson4" runat="server" Value="0" />
                <asp:HiddenField ID="hdnContactPerson5" runat="server" Value="0" />
                <div class="col-lg-12 justify-content-center align-items-center">
                    <img class="navbar-brand" src="Images/Logo.jpg" />
                </div>
                <div class="col-lg-12 form-group border-info border-10" style="padding-left: 0px; padding-right: 0px; padding-top: 10px;">
                    <div class="col-lg-12 form-group">
                        <asp:Label ID="lblMessage" runat="server" CssClass="text-success font-weight-bold"
                            Text="Details saved successfully. If you wants to modify please contact medicount team."
                            Style="display: none;"></asp:Label>
                    </div>
                    <div class="col-lg-12 form-group">
                        <div class="col-lg-1 d-inline-block">
                            <label>Client#:</label>
                        </div>
                        <div class="col-lg-1 d-inline-block">
                            <asp:TextBox ID="txtClientNo" runat="server" CssClass="form-control" Text="" MaxLength="50" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="col-lg-1 d-inline-block">
                            <label>Client Name:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtClientName" runat="server" CssClass="form-control" Text="" MaxLength="50" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="col-lg-2 d-inline-block">
                            <label>Account Executive:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtAccountExecutive" runat="server" CssClass="form-control" Text="" MaxLength="50" autocomplete="off"></asp:TextBox>
                        </div>
                    </div>

                    <div align="center" class="col-lg-12 bg-info form-group text-white">
                        <h4>BANK ACCOUNT INFORMATION TO RECEIVE ACH PAYMENTS FROM MEDICOUNT</h4>
                    </div>
                    <div class="col-lg-12 form-group">
                        <div class="col-lg-2 d-inline-block">
                            <label>Bank Name:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtBankName" runat="server" CssClass="form-control" Text="" MaxLength="50" autocomplete="off"></asp:TextBox>
                        </div>

                        <div class="col-lg-2 d-inline-block">
                            <label>Routing Number:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtRoutingNumber" runat="server" CssClass="form-control" Text=""
                                onkeypress="return isNumberKey(event);"
                                MaxLength="50" autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <div class="col-lg-2 d-inline-block">
                            <label>Account Number:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtAccountNumber" runat="server" CssClass="form-control" Text=""
                                onkeypress="return isNumberKey(event);"
                                MaxLength="50" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="col-lg-2 d-inline-block">
                            <label>Re-Enter Account Number:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtAccountNumber1" runat="server" CssClass="form-control" Text=""
                                onkeypress="return isNumberKey(event);"
                                MaxLength="50" autocomplete="off"></asp:TextBox>
                        </div>

                    </div>

                    <div align="center" class="col-lg-12 bg-info form-group text-white">
                        <h4>END OF MONTH: INVOICES-STATEMENTS-SNAPSHOTS</h4>
                    </div>
                    <div class="col-lg-12 form-group" style="padding-top: 30px;">
                        <div class="col-lg-2 d-inline-block">
                            <label>Contact Person:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtContactPerson1" runat="server" CssClass="form-control" Text="" MaxLength="50" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="col-lg-2 d-inline-block">
                            <label>Title:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtTitle1" runat="server" CssClass="form-control" Text="" MaxLength="50" autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <div class="col-lg-2 d-inline-block">
                            <label>Email ID:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtEmailID1" runat="server" CssClass="form-control" Text=""
                                MaxLength="50" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="col-lg-2 d-inline-block">
                            <label>Phone#:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtPhone1" runat="server" CssClass="form-control" Text=""
                                onkeypress="return isNumberKey(event);"
                                onfocus="mngPhoneFaxNumber(this);"
                                MaxLength="10" autocomplete="off"></asp:TextBox>
                        </div>

                    </div>
                    <div class="col-lg-12 form-group" style="padding-top: 30px;">
                        <div class="col-lg-2 d-inline-block">
                            <label>Contact Person:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtContactPerson2" runat="server" CssClass="form-control" Text="" MaxLength="50" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="col-lg-2 d-inline-block">
                            <label>Title:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtTitle2" runat="server" CssClass="form-control" Text="" MaxLength="50" autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <div class="col-lg-2 d-inline-block">
                            <label>Email ID:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtEmailID2" runat="server" CssClass="form-control" Text=""
                                MaxLength="50" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="col-lg-2 d-inline-block">
                            <label>Phone#:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtPhone2" runat="server" CssClass="form-control" Text=""
                                onkeypress="return isNumberKey(event);"
                                onfocus="mngPhoneFaxNumber(this);"
                                MaxLength="10" autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group" style="padding-top: 30px;">
                        <div class="col-lg-2 d-inline-block">
                            <label>Contact Person:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtContactPerson3" runat="server" CssClass="form-control" Text="" MaxLength="50" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="col-lg-2 d-inline-block">
                            <label>Title:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtTitle3" runat="server" CssClass="form-control" Text="" MaxLength="50" autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <div class="col-lg-2 d-inline-block">
                            <label>Email ID:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtEmailID3" runat="server" CssClass="form-control" Text=""
                                MaxLength="50" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="col-lg-2 d-inline-block">
                            <label>Phone#:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtPhone3" runat="server" CssClass="form-control" Text=""
                                onkeypress="return isNumberKey(event);"
                                onfocus="mngPhoneFaxNumber(this);"
                                MaxLength="10" autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group" style="padding-top: 30px;">
                        <div class="col-lg-2 d-inline-block">
                            <label>Contact Person:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtContactPerson4" runat="server" CssClass="form-control" Text="" MaxLength="50" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="col-lg-2 d-inline-block">
                            <label>Title:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtTitle4" runat="server" CssClass="form-control" Text="" MaxLength="50" autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <div class="col-lg-2 d-inline-block">
                            <label>Email ID:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtEmailID4" runat="server" CssClass="form-control" Text=""
                                MaxLength="50" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="col-lg-2 d-inline-block">
                            <label>Phone#:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtPhone4" runat="server" CssClass="form-control" Text=""
                                onkeypress="return isNumberKey(event);"
                                onfocus="mngPhoneFaxNumber(this);"
                                MaxLength="10" autocomplete="off"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-lg-12 form-group" style="padding-top: 30px;">
                        <div class="col-lg-2 d-inline-block">
                            <label>Contact Person:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtContactPerson5" runat="server" CssClass="form-control" Text="" MaxLength="50" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="col-lg-2 d-inline-block">
                            <label>Title:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtTitle5" runat="server" CssClass="form-control" Text="" MaxLength="50" autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <div class="col-lg-2 d-inline-block">
                            <label>Email ID:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtEmailID5" runat="server" CssClass="form-control" Text=""
                                MaxLength="50" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="col-lg-2 d-inline-block">
                            <label>Phone#:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtPhone5" runat="server" CssClass="form-control" Text=""
                                onkeypress="return isNumberKey(event);"
                                onfocus="mngPhoneFaxNumber(this);"
                                MaxLength="10" autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div align="center" class="col-lg-12 form-group">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-info custom"
                            OnClick="btnSubmit_Click" OnClientClick="return Validation();" />

                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-danger custom"
                            OnClick="btnCancel_Click" />
                    </div>
                </div>
            </form>
        </div>
    </div>

    <script type="text/javascript">
        function Validation() {
            var txtBankName = document.getElementById("<%=txtBankName.ClientID %>");
            var txtRoutingNumber = document.getElementById("<%=txtRoutingNumber.ClientID %>");
            var txtAccountNumber = document.getElementById("<%=txtAccountNumber.ClientID %>");
            var txtAccountNumber1 = document.getElementById("<%=txtAccountNumber1.ClientID %>");

            if (txtBankName.value.trim() == "") {
                alert("Enter bank name");
                txtBankName.focus();
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

            if (txtAccountNumber1.value.trim() == "") {
                alert("Re-Enter account number");
                txtAccountNumber1.focus();
                return false;
            }

            if (txtAccountNumber.value.trim() != txtAccountNumber1.value.trim()) {
                alert("Account Number Mismatch");
                txtAccountNumber.focus();
                return false;
            }

            var txtContactPerson1 = document.getElementById("<%=txtContactPerson1.ClientID %>");
            var txtTitle1 = document.getElementById("<%=txtTitle1.ClientID %>");
            var txtEmailID1 = document.getElementById("<%=txtEmailID1.ClientID %>");
            var txtPhone1 = document.getElementById("<%=txtPhone1.ClientID %>");

            if (txtContactPerson1.value.trim() == "") {
                alert("Enter contact person");
                txtContactPerson1.focus();
                return false;
            }

            if (txtTitle1.value.trim() == "") {
                alert("Enter Title");
                txtTitle1.focus();
                return false;
            }

            if (txtEmailID1.value.trim() == "") {
                alert("Enter Email ID");
                txtEmailID1.focus();
                return false;
            }

            if (txtPhone1.value.trim() == "") {
                alert("Enter Phone#");
                txtPhone1.focus();
                return false;
            }
            if (!ValidateEmail(txtEmailID1, "Invalid Email ID")) {
                return false;
            }
            if (!ValidatePhoneFaxNumber(txtPhone1, "Invalid  Phone#")) {
                return false;
            }
            var txtContactPerson2 = document.getElementById("<%=txtContactPerson2.ClientID %>");
            var txtTitle2 = document.getElementById("<%=txtTitle2.ClientID %>");
            var txtEmailID2 = document.getElementById("<%=txtEmailID2.ClientID %>");
            var txtPhone2 = document.getElementById("<%=txtPhone2.ClientID %>");

            if (txtContactPerson2.value.trim() != "" || txtTitle2.value.trim() != ""
                || txtEmailID2.value.trim() != "" || txtPhone2.value.trim() != "") {

                if (txtContactPerson2.value.trim() == "") {
                    alert("Enter contact person");
                    txtContactPerson2.focus();
                    return false;
                }

                if (txtTitle2.value.trim() == "") {
                    alert("Enter Title");
                    txtTitle2.focus();
                    return false;
                }

                if (txtEmailID2.value.trim() == "") {
                    alert("Enter Email ID");
                    txtEmailID2.focus();
                    return false;
                }

                if (txtPhone2.value.trim() == "") {
                    alert("Enter Phone#");
                    txtPhone2.focus();
                    return false;
                }
            }

            if (!ValidateEmail(txtEmailID2, "Invalid Email ID")) {
                return false;
            }
            if (!ValidatePhoneFaxNumber(txtPhone2, "Invalid  Phone#")) {
                return false;
            }
            var txtContactPerson3 = document.getElementById("<%=txtContactPerson3.ClientID %>");
            var txtTitle3 = document.getElementById("<%=txtTitle3.ClientID %>");
            var txtEmailID3 = document.getElementById("<%=txtEmailID3.ClientID %>");
            var txtPhone3 = document.getElementById("<%=txtPhone3.ClientID %>");

            if (txtContactPerson3.value.trim() != "" || txtTitle3.value.trim() != ""
                || txtEmailID3.value.trim() != "" || txtPhone3.value.trim() != "") {

                if (txtContactPerson3.value.trim() == "") {
                    alert("Enter contact person");
                    txtContactPerson3.focus();
                    return false;
                }

                if (txtTitle3.value.trim() == "") {
                    alert("Enter Title");
                    txtTitle3.focus();
                    return false;
                }

                if (txtEmailID3.value.trim() == "") {
                    alert("Enter Email ID");
                    txtEmailID3.focus();
                    return false;
                }

                if (txtPhone3.value.trim() == "") {
                    alert("Enter Phone#");
                    txtPhone3.focus();
                    return false;
                }
            }

            if (!ValidateEmail(txtEmailID3, "Invalid Email ID")) {
                return false;
            }
            if (!ValidatePhoneFaxNumber(txtPhone3, "Invalid  Phone#")) {
                return false;
            }
            var txtContactPerson4 = document.getElementById("<%=txtContactPerson4.ClientID %>");
            var txtTitle4 = document.getElementById("<%=txtTitle4.ClientID %>");
            var txtEmailID4 = document.getElementById("<%=txtEmailID4.ClientID %>");
            var txtPhone4 = document.getElementById("<%=txtPhone4.ClientID %>");

            if (txtContactPerson4.value.trim() != "" || txtTitle4.value.trim() != ""
                || txtEmailID4.value.trim() != "" || txtPhone4.value.trim() != "") {

                if (txtContactPerson4.value.trim() == "") {
                    alert("Enter contact person");
                    txtContactPerson4.focus();
                    return false;
                }

                if (txtTitle4.value.trim() == "") {
                    alert("Enter Title");
                    txtTitle4.focus();
                    return false;
                }

                if (txtEmailID4.value.trim() == "") {
                    alert("Enter Email ID");
                    txtEmailID4.focus();
                    return false;
                }

                if (txtPhone4.value.trim() == "") {
                    alert("Enter Phone#");
                    txtPhone4.focus();
                    return false;
                }
            }

            if (!ValidateEmail(txtEmailID4, "Invalid Email ID")) {
                return false;
            }
            if (!ValidatePhoneFaxNumber(txtPhone4, "Invalid  Phone#")) {
                return false;
            }

            var txtContactPerson5 = document.getElementById("<%=txtContactPerson5.ClientID %>");
            var txtTitle5 = document.getElementById("<%=txtTitle5.ClientID %>");
            var txtEmailID5 = document.getElementById("<%=txtEmailID5.ClientID %>");
            var txtPhone5 = document.getElementById("<%=txtPhone5.ClientID %>");

            if (txtContactPerson5.value.trim() != "" || txtTitle5.value.trim() != ""
                || txtEmailID5.value.trim() != "" || txtPhone5.value.trim() != "") {
                if (txtContactPerson5.value.trim() == "") {
                    alert("Enter contact person");
                    txtContactPerson5.focus();
                    return false;
                }

                if (txtTitle5.value.trim() == "") {
                    alert("Enter Title");
                    txtTitle5.focus();
                    return false;
                }

                if (txtEmailID5.value.trim() == "") {
                    alert("Enter Email ID");
                    txtEmailID5.focus();
                    return false;
                }

                if (txtPhone5.value.trim() == "") {
                    alert("Enter Phone#");
                    txtPhone5.focus();
                    return false;
                }
            }

            if (!ValidateEmail(txtEmailID5, "Invalid Email ID")) {
                return false;
            }
            if (!ValidatePhoneFaxNumber(txtPhone5, "Invalid  Phone#")) {
                return false;
            }

            return confirm('Are you sure to Submit?')
        }

    </script>
</body>
</html>
