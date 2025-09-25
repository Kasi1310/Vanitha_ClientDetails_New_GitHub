<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmNoticeAnnouncement.aspx.cs" Inherits="ClientDetails.frmNoticeAnnouncement" %>

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
                    <img class="navbar-brand" src="Images/Logo.jpg" />
                </div>
                <div class="col-lg-12 form-group border-info border-10" style="padding-left: 0px; padding-right: 0px; padding-top: 10px;">
                    <div class="col-lg-12 form-group">
                        <div class="col-lg-4 d-inline-block" style="vertical-align: top;">
                            Name
                        </div>
                        <div class="col-lg-5 d-inline-block">
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <div class="col-lg-4 d-inline-block" style="vertical-align: top;">
                            Email ID
                        </div>
                        <div class="col-lg-5 d-inline-block">
                            <asp:TextBox ID="txtEmailID" runat="server" CssClass="form-control" MaxLength="50"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <div class="col-lg-4 d-inline-block" style="vertical-align: top;">
                            Do you want the below notice to go out? 
                        </div>
                        <div class="col-lg-5 d-inline-block">
                            <asp:RadioButtonList ID="rdolstNotice" CssClass="custom-checkbox" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1"><span></span>Yes</asp:ListItem>
                                <asp:ListItem Value="0"><span></span>No</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div class="col-lg-4  d-inline-block" style="vertical-align: top;">
                            If no, to whom?           Client-Fire Chief, Only
                        </div>
                        <div class="col-lg-5  d-inline-block">
                            <asp:RadioButtonList ID="rdolstClientFireChief" CssClass="custom-checkbox" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1"><span></span>Yes</asp:ListItem>
                                <asp:ListItem Value="0"><span></span>No</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                         <div class="col-lg-4  d-inline-block" style="vertical-align: top;">
                            To all Client contacts
                        </div>
                        <div class="col-lg-5  d-inline-block">
                            <asp:RadioButtonList ID="rdolstAllClientContacts" CssClass="custom-checkbox" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1"><span></span>Yes</asp:ListItem>
                                <asp:ListItem Value="0"><span></span>No</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div class="col-lg-4 d-inline-block" style="vertical-align: top;">
                            Client-Entire Fire Dept  
                        </div>
                        <div class="col-lg-5 d-inline-block">
                            <asp:RadioButtonList ID="rdolstClientEntireFire" CssClass="custom-checkbox" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1"><span></span>Yes</asp:ListItem>
                                <asp:ListItem Value="0"><span></span>No</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div class="col-lg-4 d-inline-block" style="vertical-align: top;">
                            Client-Fiscal Officer 
                        </div>
                        <div class="col-lg-5 d-inline-block">
                            <asp:RadioButtonList ID="rdolstClientFiscalOfficer" CssClass="custom-checkbox" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1"><span></span>Yes</asp:ListItem>
                                <asp:ListItem Value="0"><span></span>No</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <%--<div class="col-lg-4  d-inline-block" style="vertical-align: top;">
                            All Client contacts in ZOHO
                        </div>
                        <div class="col-lg-5  d-inline-block">
                            <asp:RadioButtonList ID="rdolstAllClientContactsZOHO" CssClass="custom-checkbox" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1"><span></span>Yes</asp:ListItem>
                                <asp:ListItem Value="0"><span></span>No</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>--%>

                        <div class="col-lg-4 d-inline-block" style="vertical-align: top;">
                            State Specific Client Contacts in ZOHO
                        </div>
                        <div class="col-lg-5 d-inline-block">
                            <asp:RadioButtonList ID="rdolstSpecificClientContactsZOHO" CssClass="custom-checkbox" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1"><span></span>Yes</asp:ListItem>
                                <asp:ListItem Value="0"><span></span>No</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>

                        <%--<div class="col-lg-4 d-inline-block" style="vertical-align: top;">
                            ALL Potential Clients
                        </div>
                        <div class="col-lg-5 d-inline-block">
                            <asp:RadioButtonList ID="rdolstALLPotentialClients" CssClass="custom-checkbox" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="1"><span></span>Yes</asp:ListItem>
                                <asp:ListItem Value="0"><span></span>No</asp:ListItem>
                            </asp:RadioButtonList>
                        </div>--%>
                        <div class="col-lg-4 d-inline-block" style="vertical-align: top;">
                            Comments
                        </div>
                        <div class="col-lg-5 d-inline-block">
                            <asp:TextBox ID="txtComments" runat="server" CssClass="form-control" TextMode="MultiLine" Style="resize: none;"></asp:TextBox>
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
</body>
</html>
