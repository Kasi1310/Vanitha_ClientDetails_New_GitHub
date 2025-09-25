<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmMonthlyRunVolume.aspx.cs" Inherits="ClientDetails.frmMonthlyRunVolume" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MONTHLY RUN VOLUME</title>
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
</head>
<body>
    <div class="container h-100">
        <div class="row h-100 justify-content-center align-items-center">
            <form class="col-lg-12" runat="server">
                <div class="col-lg-12 form-group border-info border-10"
                    style="padding-left: 0px; padding-right: 0px; padding-top: 10px;">
                    <div class="col-lg-12 justify-content-center align-items-center" style="text-align: center">
                        <img class="navbar-brand" src="Images/Logo.jpg" />
                    </div>
                    <div class="col-lg-12 form-group"
                        style="font-family: Calibri; font-size: 24px; font-weight: bold; color: #009094; text-align: center;">
                        Monthly Run Volume Alert
                    </div>
                    <div class="col-lg-12 form-group">
                        <asp:HiddenField ID="hdnID" runat="server" Value="0" />
                        <asp:HiddenField ID="hdnType" runat="server" Value="" />
                        <div class="col-lg-2 d-inline-block" style="color: #009094; font-weight: bold;">
                            <label>Client Name:</label>
                        </div>
                        <div class="col-lg-9 d-inline-block">
                            <asp:Label ID="lblClientName" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <div class="col-lg-2 d-inline-block" style="color: #009094; font-weight: bold;vertical-align:top;">
                            
                            <asp:Label ID="lblComment" runat="server" Text="Comments/Issues:"></asp:Label>
                            <asp:Label ID="lblReasonForLowRuns" runat="server" Text="Question(s) for AE:"></asp:Label>
                        </div>
                        <div class="col-lg-9 d-inline-block">
                            <asp:TextBox ID="txtComment" runat="server" CssClass="form-control" Rows="5" 
                                Text="" TextMode="MultiLine" style="resize:none;" autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div align="center" class="col-lg-12 form-group">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-info custom"
                            style="font-weight:bold;" OnClick="btnSubmit_Click" OnClientClick="return Validation();" />
                    </div>
                </div>
            </form>
        </div>
    </div>
</body>
</html>
