<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmMedicountClientSurveySendLink.aspx.cs" Inherits="ClientDetails.frmMedicountClientSurveySendLink" %>

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
                        <%--<div class="col-lg-10 d-inline-block">--%>
                        <asp:Button ID="btnSendLink" runat="server" Text="Send Link" CssClass="btn btn-info custom" OnClick="btnSendLink_Click" />
                        <%--</div>
                        <div class="col-lg-1 d-inline-block">--%>
                        <asp:Button ID="btnExcelExport" runat="server" Text="Export" CssClass="btn btn-info" OnClick="btnExcelExport_Click" />
                        <%--</div>--%>
                    </div>
                    <div class="col-lg-11 form-group">
                        <asp:GridView ID="gvMedicountClientSurvey" runat="server" AutoGenerateColumns="false"
                            CssClass="table table-striped table-bordered table-hover"
                            AllowPaging="false" ShowHeaderWhenEmpty="true">
                            <Columns>
                                <asp:TemplateField HeaderText="Account#">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="gvhdnID" runat="server" Value='<%# Eval("ID") %>' />
                                        <asp:Label ID="gvlblAccountNumber" runat="server" Text='<%# Eval("AccountNumber") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Account Name">
                                    <ItemTemplate>
                                        <asp:Label ID="gvlblAccountName" runat="server" Text='<%# Eval("AccountName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Account Owner">
                                    <ItemTemplate>
                                        <asp:Label ID="gvlblAccountOwner" runat="server" Text='<%# Eval("AccountOwner") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Full Name">
                                    <ItemTemplate>
                                        <asp:Label ID="gvlblFullName" runat="server" Text='<%# Eval("FullName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Email">
                                    <ItemTemplate>
                                        <asp:Label ID="gvlblEmail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mail Send To Client">
                                    <ItemTemplate>
                                        <asp:Label ID="gvlblMailSend" runat="server" Text='<%# Eval("MailSend") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Survey Filled By Client">
                                    <ItemTemplate>
                                        <asp:Label ID="gvlblSurveyFilled" runat="server" Text='<%# Eval("SurveyFilled") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvlnkSend" runat="server" Text="Send" CommandName="cmdSend"
                                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </form>
        </div>
    </div>
    
</body>
</html>
