<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmASPPDocumentUploadRpt.aspx.cs" Inherits="ClientDetails.frmASPPDocumentUploadRpt" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ASPP Document Upload - Report</title>
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
                        <asp:Button ID="btnExcelExport" runat="server" Text="Export"
                            CssClass="btn btn-info" OnClick="btnExcelExport_Click" />
                    </div>
                    <div class="col-lg-11 form-group">
                        <asp:GridView ID="gvASPPDocument" runat="server" AutoGenerateColumns="false"
                            CssClass="table table-striped table-bordered table-hover"
                            AllowPaging="false" ShowHeaderWhenEmpty="true"
                            OnRowCommand="gvASPPDocument_RowCommand" >
                            <Columns>
                                <asp:TemplateField HeaderText="List of Documents">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvlnkbtnDocumentName" runat="server"
                                            Text='<%# Eval("OriginalFileName") %>'
                                            CommandName="cmdOpenFile"
                                            CommandArgument='<%# Eval("FileName")+","+ Eval("OriginalFileName")+","+ Eval("ClientName") %>'
                                            Style="text-wrap: inherit;"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Client Name">
                                    <ItemTemplate>
                                        <asp:Label ID="gvlblClientName" runat="server" Text='<%# Eval("ClientName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Email Address">
                                    <ItemTemplate>
                                        <asp:Label ID="gvlblEmailId" runat="server" Text='<%# Eval("EmailId") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Uploaded Date">
                                    <ItemTemplate>
                                        <asp:Label ID="gvlblUploadedDate" runat="server" Text='<%# Eval("UploadedDate") %>'></asp:Label>
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
