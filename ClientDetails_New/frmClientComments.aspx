<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmClientComments.aspx.cs" Inherits="ClientDetails.frmClientComments" %>

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
            border: 3pt solid #00968F !important;
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
    <div class="container h-100" style="width: 700px !important;">
        <div class="row h-100 justify-content-center align-items-center">
            <form class="col-lg-12" runat="server">
                <div class="col-lg-12 form-group border-info border-10" style="padding-left: 0px; padding-right: 0px; padding-top: 10px;">
                    <div class="col-lg-12 justify-content-center align-items-center" style="text-align: center">
                        <img class="navbar-brand" src="Images/Logo.jpg" />
                    </div>
                    <div class="col-lg-12 form-group">
                        <p align='center' style='margin: 0in; text-align: center; background: #009094'><b><span style='font-size: 30.0px; color: white'>Questions</span></b></p>
                    </div>
                    <div class="col-lg-12 form-group">
                        <div class="col-lg-4 d-inline-block">
                            <span class="text-danger">*</span>
                            <label>Name:</label>
                        </div>
                        <div class="col-lg-7 d-inline-block">
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" Text="" MaxLength="50"
                                autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <div class="col-lg-4 d-inline-block">
                            <span class="text-danger">*</span>
                            <label>Email Address:</label>
                        </div>
                        <div class="col-lg-7 d-inline-block">
                            <asp:TextBox ID="txtMailID" runat="server" CssClass="form-control" Text="" MaxLength="50"
                                autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <div class="col-lg-4 d-inline-block" style="vertical-align: top;">
                            <span class="text-danger">*</span>
                            <label>Submit your questions:</label>
                        </div>
                        <div class="col-lg-7 d-inline-block">
                            <asp:TextBox ID="txtComments" runat="server" CssClass="form-control" Text="" TextMode="MultiLine"
                                Rows="5" autocomplete="off" Style="resize: none;"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <div class="col-lg-11 d-inline-block">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-info custom"
                                OnClick="btnSubmit_Click" OnClientClick="return Validation();" Style="float: right; font-weight: bold;" />
                        </div>
                    </div>
                </div>
            </form>
        </div>
    </div>
    <script type="text/javascript">
        function Validation() {
            var txtName = document.getElementById("<%=txtName.ClientID %>");
            var txtMailID = document.getElementById("<%=txtMailID.ClientID %>");
            var txtComments = document.getElementById("<%=txtComments.ClientID %>");

            if (txtName.value.trim() == "") {
                alert("Enter Name");
                return false;
            }

            if (txtMailID.value.trim() == "") {
                alert("Enter Email Address");
                return false;
            }

            if (txtComments.value.trim() == "") {
                alert("Enter your questions");
                return false;
            }
            return true;
        }
    </script>
</body>
</html>
