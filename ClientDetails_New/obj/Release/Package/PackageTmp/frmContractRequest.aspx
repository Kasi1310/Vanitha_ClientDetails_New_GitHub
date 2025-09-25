<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmContractRequest.aspx.cs" Inherits="ClientDetails.frmContractRequest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link href="Content/bootstrap.css" rel="stylesheet" />
    <script src="Scripts/Custom/jquery.min.js"></script>
    <script src="Scripts/Custom/bootstrap.min.js"></script>
    <link href="Scripts/Custom/bootstrap.min.css" rel="stylesheet" />
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

                <div class="col-lg-12 justify-content-center align-items-center">
                    <img class="navbar-brand" src="Images/Logo.jpg" style="width: 300px; height: 100px;" />
                </div>
                <div class="col-lg-12 form-group border-info border-10" style="padding-left: 0px; padding-right: 0px; padding-top: 0px;">

                    <div align="center" class="col-lg-12 bg-info form-group text-white" style="font-weight: bold;">
                        <span style="font-size: 24px; font-weight: bold;">CONTRACT REQUEST FORM</span>
                    </div>
                    <div class="col-lg-12 form-group">
                        <asp:Label ID="lblMessage" runat="server" CssClass="text-success font-weight-bold"
                            Text="Details send successfully."
                            Style="display: none;"></asp:Label>
                        <asp:Label ID="lblError" runat="server" CssClass="text-danger font-weight-bold"
                            Text="Details not send, please contact Medicount team"
                            Style="display: none;"></asp:Label>
                    </div>
                    <div runat="server" class="col-lg-12" id="divContent">
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>Name of Client:</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:TextBox ID="txtClient" runat="server" CssClass="form-control" Text="" MaxLength="100" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">

                            <div class="col-lg-6 d-inline-block">
                                <label>Account Executive name(s):</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:TextBox ID="txtAEName" runat="server" CssClass="form-control" Text="" MaxLength="500" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">

                            <div class="col-lg-6 d-inline-block">
                                <label>Name of the chief:</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:TextBox ID="txtChiefName" runat="server" CssClass="form-control" Text="" MaxLength="500" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>Name of Client - Confirm:</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:TextBox ID="txtNameofClient" runat="server" CssClass="form-control" Text="" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>Client Services Agreement:</label>
                            </div>
                            <div class="col-lg-4 d-inline-block">
                                <asp:RadioButtonList ID="rdolstClientServicesAgreement" runat="server" RepeatDirection="Horizontal" CssClass="custom-checkbox" Style="font-weight: bold;">
                                    <asp:ListItem Value="New">New<span></span></asp:ListItem>
                                    <asp:ListItem Value="Renewal">Renewal<span></span></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>Fee % - Recommended:</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:TextBox ID="txtFeeRecommended" runat="server" CssClass="form-control" Text="" MaxLength="500" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>Length of Client Services Agreement: </label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:TextBox ID="txtLengthOfClientServicesAgreement" runat="server" CssClass="form-control" Text="" MaxLength="500" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>Additional equipment or software applications that need to be included in the fee and or Client Services Agreement:</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:TextBox ID="txtAdditionalEquipment" runat="server" CssClass="form-control" Text="" MaxLength="500" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>ALS Protocol required in CSA:</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:TextBox ID="txtALSProtocalRequiredCSAProtocol" runat="server" CssClass="form-control" Text="" MaxLength="500" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <%--<label>Address of Entity for CSA Cover Letter while return:</label>--%>
                                <label>Address of entity for cover letter for return of the CSA:</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:TextBox ID="txtAddressOfEntity" runat="server" CssClass="form-control" Text="" MaxLength="500" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>County Name:</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:TextBox ID="txtCountyName" runat="server" CssClass="form-control" Text="" MaxLength="50" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>State:</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:TextBox ID="txtState" runat="server" CssClass="form-control" Text="" MaxLength="50" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>Email address to send Docusign CSA:</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="form-control" Text="" MaxLength="500" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>Change Healthcare Client:</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:TextBox ID="txtChangeHealthCareClient" runat="server" CssClass="form-control" Text="" MaxLength="500" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>Other Comments:</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:TextBox ID="txtOtherComments" runat="server" CssClass="form-control" Text="" MaxLength="500" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div align="center" class="col-lg-12 form-group">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-info custom"
                                OnClick="btnSubmit_Click" OnClientClick="return Validation();" />

                            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-danger custom"
                                OnClick="btnClear_Click" />
                        </div>
                    </div>
                </div>
            </form>
        </div>
    </div>

    <script type="text/javascript">
        function Validation() {
            var txtClient = document.getElementById("<%=txtClient.ClientID %>");
            if (txtClient.value.trim() == "") {
                alert("Enter Name Of Client");
                return false;
            }
        }
    </script>
</body>
</html>
