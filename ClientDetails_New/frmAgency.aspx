<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAgency.aspx.cs" Inherits="ClientDetails.frmAgency" %>

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
                        <asp:Button ID="btnExcelExport" runat="server" Text="Export" CssClass="btn btn-info" OnClick="btnExcelExport_Click" />
                    </div>
                    <div class="col-lg-12 form-group">
                        <asp:GridView ID="gvAgency" runat="server" AutoGenerateColumns="false"
                            CssClass="table table-striped table-bordered table-hover"
                            AllowPaging="false" ShowHeaderWhenEmpty="true">
                            <Columns>
                                <asp:TemplateField HeaderText="Name of Agency">
                                    <ItemTemplate>
                                        <asp:Label ID="gvlblNameOfAgency" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Person Interested">
                                    <ItemTemplate>
                                        <asp:Label ID="gvlblPersonInterested" runat="server" Text='<%# Eval("PersonInterested") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Email Address">
                                    <ItemTemplate>
                                        <asp:Label ID="gvlblEmailAddress" runat="server" Text='<%# Eval("EmailAddress") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </form>
        </div>
    </div>
</body>
</html>
