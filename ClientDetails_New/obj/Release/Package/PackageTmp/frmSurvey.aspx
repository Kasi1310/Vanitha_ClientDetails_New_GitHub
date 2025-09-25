<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmSurvey.aspx.cs" Inherits="ClientDetails.frmSurvey" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-3.0.0.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <link href="Content/bootstrap.css" rel="stylesheet" />

    <style>
        body, html {
            height: 100%;
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

    <div class="container h-100" style="width: 900px !important;">
        <div class="row h-100 justify-content-center align-items-center">
            <form class="col-lg-12" runat="server">
                <asp:HiddenField ID="hdnAttendeesID" runat="server" Value="" />
                <div class="col-lg-12" style="text-align: center;">
                    <asp:Label ID="lblMessage" runat="server" Style="font-weight: bold; font-size: 20px;"></asp:Label>
                </div>
                <div class="col-lg-12" id="divControls" runat="server">
                    <div class="col-lg-12 justify-content-center align-items-center">
                        <center>
                            <img class="navbar-brand" src="Images/Logo.jpg" />
                        </center>
                    </div>
                    <div class="col-lg-12 form-group" style="padding-left: 0px; padding-right: 0px; padding-top: 10px; font-weight: bold !important;">
                        <div class="col-lg-12 form-group" style="font-size: 26px; font-weight: bold;">
                            Did your account executive explain your billing activity to your satisfaction?
                        </div>

                        <div class="col-lg-12 form-group">
                            <asp:RadioButtonList ID="rdoBillingActivity" runat="server" RepeatDirection="Vertical">
                                <asp:ListItem Value="Yes, They did!"><span style="font-size: 18pt; line-height: 18pt; font-weight: bold;color:#2ecc71;padding: 13px;">Yes, They did!</span></asp:ListItem>
                                <asp:ListItem Value="No, I am still a little confused"><span style="font-size: 18pt; line-height: 18pt; font-weight: bold;color:red;padding: 13px;">No, I am still a little confused</span></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>

                    <div class="col-lg-12 form-group" style="padding-left: 0px; padding-right: 0px; padding-top: 10px; font-weight: bold !important;">
                        <div class="col-lg-12 form-group" style="font-size: 26px; font-weight: bold;">
                            Did your account executive answer all your questions?
                        </div>

                        <div class="col-lg-12 form-group" style="font-size: 18pt; line-height: 18pt; font-weight: bold;">
                            <asp:RadioButtonList ID="rdoAnswerAllQuestion" runat="server" RepeatDirection="Vertical">
                                <asp:ListItem Value="Yes, They did!"><span style="font-size: 18pt; line-height: 18pt; font-weight: bold;color:#2ecc71;padding: 13px;">Yes, They did!</span></asp:ListItem>
                                <asp:ListItem Value="No, I have follow up questions"><span style="font-size: 18pt; line-height: 18pt; font-weight: bold;color:red;padding: 13px;">No, I have follow up questions</span></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group" style="padding-left: 0px; padding-right: 0px; padding-top: 10px; font-weight: bold !important;">
                        <div class="col-lg-12 form-group" style="font-size: 26px; font-weight: bold;">
                            Is your agency/department meeting your revenue expectations?
                        </div>

                        <div class="col-lg-12 form-group" style="font-size: 18pt; font-weight: bold;">
                            <asp:RadioButtonList ID="rdoMeetingRevenueExpectation" runat="server" RepeatDirection="Vertical">
                                <asp:ListItem Value="Yes"><span style="font-size: 18pt; line-height: 18pt; font-weight: bold;color:#2ecc71;padding: 13px;">Yes</span></asp:ListItem>
                                <asp:ListItem Value="There is room to improve"><span style="font-size: 18pt; line-height: 18pt; font-weight: bold;color:#2980b9;padding: 13px;">There is room to improve</span></asp:ListItem>
                                <asp:ListItem Value="No"><span style="font-size: 18pt; line-height: 18pt; font-weight: bold;color:red;padding: 13px;">No</span></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                    </div>
                    
                    <div class="col-lg-12 form-group" style="padding-left: 0px; padding-right: 0px; padding-top: 10px; font-weight: bold !important;">
                        <div class="col-lg-12 form-group" style="font-size: 26px; font-weight: bold;">
                            Comment
                        </div>

                        <div class="col-lg-12 form-group" style="font-size: 12pt; font-weight: bold;">
                            <asp:TextBox ID="txtComment" runat="server" TextMode="MultiLine" Rows="3" style="resize:none;" class="col-lg-12"></asp:TextBox>
                        </div>
                    </div>
                    <div align="center" class="col-lg-12 form-group">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-info custom"
                            OnClick="btnSubmit_Click" OnClientClick="return Validation();" />
                    </div>

                </div>
            </form>
        </div>
    </div>

    <script type="text/javascript">
        function Validation() {
            var rdoBillingActivity = document.getElementById("<%=rdoBillingActivity.ClientID %>").getElementsByTagName("input");
            var rdoAnswerAllQuestion = document.getElementById("<%=rdoAnswerAllQuestion.ClientID %>").getElementsByTagName("input");
            var rdoMeetingRevenueExpectation = document.getElementById("<%=rdoMeetingRevenueExpectation.ClientID %>").getElementsByTagName("input");

            if (!rdoBillingActivity[0].checked && !rdoBillingActivity[1].checked) {
                alert("Did your account executive explain your billing activity to your satisfaction?");
                return false;
            }

            if (!rdoAnswerAllQuestion[0].checked && !rdoAnswerAllQuestion[1].checked) {
                alert("Did your account executive answer all your questions?");
                return false;
            }
            if (!rdoMeetingRevenueExpectation[0].checked && !rdoMeetingRevenueExpectation[1].checked && !rdoMeetingRevenueExpectation[2].checked) {
                alert("Is your agency/department meeting your revenue expectations?");
                return false;
            }
        }

    </script>
</body>
</html>
