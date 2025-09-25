<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmACHDeductFeesDetails.aspx.cs" Inherits="ClientDetails.frmACHDeductFeesDetails" %>

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
                            
                            <asp:Button ID="btnExcelExport" runat="server" Text="Export" CssClass="btn btn-info" OnClick="btnExcelExport_Click" />
                            <%--</div>--%>
                        </div>
                        <div class="col-lg-11 form-group">
                            <asp:GridView ID="gvACHDeductFees" runat="server" AutoGenerateColumns="false"
                                CssClass="table table-striped table-bordered table-hover"
                                AllowPaging="false" ShowHeaderWhenEmpty="true">
                                <Columns>
                                    <asp:TemplateField HeaderText="Client Name">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="gvhdnID" runat="server" Value='<%# Eval("ID") %>' />
                                            <asp:Label ID="gvlblClientName" runat="server" Text='<%# Eval("ClientName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Send all due funds client by ACH">
                                        <ItemTemplate>
                                            <asp:Label ID="gvlblFundsClientByACH" runat="server" Text='<%# Eval("FundsClientByACH") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Upload all End of Month Invoices & Snapshots">
                                        <ItemTemplate>
                                            <asp:Label ID="gvlblEndofMonthInvoice" runat="server" Text='<%# Eval("EndofMonthInvoice") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Deduct their monthly fees">
                                        <ItemTemplate>
                                            <asp:Label ID="gvlblEndofMonthFunds" runat="server" Text='<%# Eval("EndofMonthFunds") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </form>
        </div>
    </div>

</body>
</html>
