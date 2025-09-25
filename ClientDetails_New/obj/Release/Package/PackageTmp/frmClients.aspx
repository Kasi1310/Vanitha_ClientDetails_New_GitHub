<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmClients.aspx.cs" Inherits="ClientDetails.frmClients" %>

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
                        <div class="col-lg-1 d-inline-block">
                            <label>Client#:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtClientNo" runat="server" CssClass="form-control" Text="" MaxLength="50"
                                autocomplete="off" onkeypress="return isNumberKey(event);"></asp:TextBox>
                        </div>
                        <div class="col-lg-2 d-inline-block">
                            <label>Client Name:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtClientName" runat="server" CssClass="form-control" Text="" MaxLength="50" autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <div class="col-lg-1 d-inline-block">
                            <label>Email ID:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtEmailID" runat="server" CssClass="form-control" Text=""
                                MaxLength="50" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="col-lg-2 d-inline-block">
                            <label>Account Executive:</label>
                        </div>
                        <div class="col-lg-3 d-inline-block">
                            <asp:TextBox ID="txtAccountExecutive" runat="server" CssClass="form-control" Text="" MaxLength="50" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="col-lg-1 d-inline-block">
                            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-info custom"
                                OnClick="btnAdd_Click" OnClientClick="return Validation();" />
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <%--<div class="col-lg-10 d-inline-block">--%>
                            <asp:Button ID="btnSendLink" runat="server" Text="Send Link" CssClass="btn btn-info custom" OnClick="btnSendLink_Click" />
                        <%--</div>
                        <div class="col-lg-1 d-inline-block">--%>
                            <asp:Button ID="btnExcelExport" runat="server" Text="Export" CssClass="btn btn-info" OnClick="btnExcelExport_Click" />
                        <%--</div>--%>
                    </div>
                    <div class="col-lg-11 form-group">
                        <asp:GridView ID="gvClients" runat="server" AutoGenerateColumns="false"
                            CssClass="table table-striped table-bordered table-hover"
                            AllowPaging="false" ShowHeaderWhenEmpty="true" OnRowCommand="gvClients_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Client#">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="gvhdnAccountExecutiveEmailID" runat="server" Value='<%# Eval("AccountExecutiveEmailID") %>' />
                                        <asp:Label ID="gvlblClientNo" runat="server" Text='<%# Eval("ClientID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Client Name">
                                    <ItemTemplate>
                                        <asp:Label ID="gvlblClientName" runat="server" Text='<%# Eval("ClientName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Account Executive">
                                    <ItemTemplate>
                                        <asp:Label ID="gvlblAccountExecutive" runat="server" Text='<%# Eval("AccountExecutive") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               <%-- <asp:TemplateField HeaderText="Account Executive Email ID">
                                    <ItemTemplate>
                                        <asp:Label ID="gvlblAccountExecutiveEmailID" runat="server" Text='<%# Eval("AccountExecutiveEmailID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Email ID">
                                    <ItemTemplate>
                                        <asp:Label ID="gvlblEmailID" runat="server" Text='<%# Eval("EmailID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mail Send To Client">
                                    <ItemTemplate>
                                        <asp:Label ID="gvlblMailSend" runat="server" Text='<%# Eval("IsMailSend") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Details Updated By Client">
                                    <ItemTemplate>
                                        <asp:Label ID="gvlblUpdated" runat="server" Text='<%# Eval("IsUpdated") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <%--<asp:LinkButton ID="gvlnkEdit" runat="server" Text="Edit" CommandName="cmdEdit"
                                            CommandArgument='<%# Eval("ClientID") %>' />
                                        <asp:LinkButton ID="gvlnkDelete" runat="server" Text="Delete" CommandName="cmdDelete"
                                            CommandArgument='<%# Eval("ClientID") %>' />--%>
                                        <asp:LinkButton ID="gvlnkSend" runat="server" Text="Send" CommandName="cmdSend"
                                            CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </form>
        </div>
    </div>
    <script type="text/javascript">
        function Validation() {
            var txtClientNo = document.getElementById("<%=txtClientNo.ClientID %>");
            var txtClientName = document.getElementById("<%=txtClientName.ClientID %>");
            var txtEmailID = document.getElementById("<%=txtEmailID.ClientID %>");
            var txtAccountExecutive = document.getElementById("<%=txtAccountExecutive.ClientID %>");

            if (txtClientNo.value.trim() == "") {
                alert("Enter Client#");
                txtClientNo.focus();
                return false;
            }

            if (txtClientName.value.trim() == "") {
                alert("Enter Client Name");
                txtClientName.focus();
                return false;
            }

            if (txtEmailID.value.trim() == "") {
                alert("Enter EmailID");
                txtEmailID.focus();
                return false;
            }

            if (txtAccountExecutive.value.trim() == "") {
                alert("Enter Account Executive");
                txtAccountExecutive.focus();
                return false;
            }

            if (!ValidateEmail(txtEmailID, "Invalid Email ID")) {
                return false;
            }
        }
    </script>
</body>
</html>
