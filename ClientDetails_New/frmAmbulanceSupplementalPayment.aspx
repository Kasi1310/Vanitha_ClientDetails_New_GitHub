<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAmbulanceSupplementalPayment.aspx.cs" Inherits="ClientDetails.frmAmbulanceSupplementalPayment" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>OHIO Ambulance Supplemental Payment</title>
    <script src="Scripts/jquery-3.0.0.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <script src="Scripts/Custom/Custom.js"></script>
    <style>
        body, html {
            height: 100%;
        }

        .border-10 {
            border: 1px solid rgb(218,220,224) !important;
            border-width: 1px;
            border-radius: 8px;
        }
    </style>
    <style>
        body {
            font-family: Calibri;
            font-size: 16px;
        }
    </style>
    <style>
        /* The custom-checkbox */
        .custom-checkbox {
            display: block;
            position: relative;
            padding-left: 35px;
            margin-bottom: 12px;
            cursor: pointer;
            font-size: 16px;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
        }

            /* Hide the browser's default checkbox */
            .custom-checkbox input {
                position: absolute;
                opacity: 0;
                cursor: pointer;
                height: 0;
                width: 0;
            }

        /* Create a custom checkbox */
        .checkmark {
            position: absolute;
            top: 0;
            left: 0;
            height: 25px;
            width: 25px;
            border: 1px solid #000000 !important;
            background-color: #fff;
        }

        /* On mouse-over, add a grey background color */
        .custom-checkbox:hover input ~ .checkmark {
            background-color: #fff;
        }

        /* When the checkbox is checked, add a blue background */
        .custom-checkbox input:checked ~ .checkmark {
            background-color: #2196F3;
        }

        /* Create the checkmark/indicator (hidden when not checked) */
        .checkmark:after {
            content: "";
            position: absolute;
            display: none;
        }

        /* Show the checkmark when checked */
        .custom-checkbox input:checked ~ .checkmark:after {
            display: block;
        }

        /* Style the checkmark/indicator */
        .custom-checkbox .checkmark:after {
            left: 9px;
            top: 5px;
            width: 5px;
            height: 10px;
            border: solid white;
            border-width: 0 3px 3px 0;
            -webkit-transform: rotate(45deg);
            -ms-transform: rotate(45deg);
            transform: rotate(45deg);
        }
    </style>
</head>
<body style="background-color: rgb(217, 239, 237);">
    <div class="container h-100" style="width: 700px !important;">
        <div class="row h-100 justify-content-center align-items-center">
            <form id="form1" runat="server">
                <div id="divcontainer" runat="server">
                    <div class="col-lg-12 form-group border-info border-10"
                        style="background-color: #ffffff;">
                        <div class="col-lg-12 form-group text-center" style="margin-top: 15px; font-size: 24pt; color: #00968F !important; line-height: 1.2">
                            <b>OHIO AMBULANCE SUPPLEMENTAL PAYMENT PROGRAM FOR EMERGENCY MEDICAL SERVICES INFORMATION REQUEST</b>
                        </div>
                        <div class="col-lg-12 form-group text-center" style="line-height:2">

                            <b>FOR INFORMATION REGARDING THE PROGRAM PLEASE FILL OUT THE FOLLOWING:</b>
                            <br />

                            <b>YOU WILL RECEIVE INFORMATION ON THE PROGRAM AS ITS UPDATED.</b>

                        </div>
                        <div class="col-lg-12 form-group"
                            style="border-top: 1px solid rgb(218,220,224) !important; margin-top: 12px;">
                        </div>
                        <div class="col-lg-12 form-group text-danger"
                            style="margin-top: 15px; font-size: 14px !important; padding-left: 0px!important">
                            * Indicates required question
                        </div>
                    </div>

                    <div class="col-lg-12 form-group border-info border-10"
                        style="padding-left: 0px; padding-right: 0px; padding-top: 20px; padding-bottom: 20px; background-color: #ffffff;">
                        <div class="col-lg-12" style="padding-left: 10px; font-size: 12pt !important;">
                            Medicount Client Name:<span class="text-danger">*</span>
                        </div>
                        <div class="col-lg-6" style="padding-left: 10px; padding-top: 20px;">
                            <asp:TextBox ID="txtClientName" runat="server" autocomplete="off"
                                placeholder="Your answer" Text=""
                                CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvClientName" runat="server"
                                ControlToValidate="txtClientName" ErrorMessage="This is a required question"
                                ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group border-info border-10"
                        style="padding-left: 0px; padding-right: 0px; padding-top: 20px; padding-bottom: 20px; background-color: #ffffff;">
                        <div class="col-lg-12" style="padding-left: 10px; font-size: 12pt !important;">
                            Your Name:<span class="text-danger">*</span>
                        </div>
                        <div class="col-lg-6" style="padding-left: 10px; padding-top: 20px;">
                            <asp:TextBox ID="txtName" runat="server" autocomplete="off"
                                placeholder="Your answer" Text=""
                                CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvName" runat="server"
                                ControlToValidate="txtName" ErrorMessage="This is a required question"
                                ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group border-info border-10"
                        style="padding-left: 0px; padding-right: 0px; padding-top: 20px; padding-bottom: 20px; background-color: #ffffff;">
                        <div class="col-lg-12" style="padding-left: 10px; font-size: 12pt !important;">
                            Your Email address:<span class="text-danger">*</span>
                        </div>
                        <div class="col-lg-6" style="padding-left: 10px; padding-top: 20px;">
                            <asp:TextBox ID="txtEmailAddress" runat="server" autocomplete="off"
                                placeholder="Your answer" Text=""
                                CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEmailAddress" runat="server"
                                ControlToValidate="txtEmailAddress" ErrorMessage="This is a required question"
                                ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group border-info border-10"
                        style="padding-left: 0px; padding-right: 0px; padding-top: 20px; padding-bottom: 20px; background-color: #ffffff;">
                        <div class="col-lg-12" style="padding-left: 10px; font-size: 12pt !important;">
                            Your Phone Number:<span class="text-danger">*</span>
                        </div>
                        <div class="col-lg-6" style="padding-left: 10px; padding-top: 20px;">
                            <asp:TextBox ID="txtPhoneNumber" runat="server" autocomplete="off"
                                placeholder="Your answer" Text=""
                                CssClass="form-control"></asp:TextBox>


                            <asp:RequiredFieldValidator ID="rfvPhoneNumber" runat="server"
                                ControlToValidate="txtPhoneNumber" ErrorMessage="This is a required question"
                                ForeColor="Red"></asp:RequiredFieldValidator>
                            <br />
                            <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                            ErrorMessage="Enter valid Phone number" ControlToValidate="txtPhoneNumber" 
                            ValidationExpression="(\+1\s?)?\(?\d{3}\)?[-\s]?\d{3}[-\s]?\d{4}" 
                            ForeColor="Red">
                        </asp:RegularExpressionValidator>--%>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group border-info border-10"
                        style="padding-left: 0px; padding-right: 0px; padding-top: 20px; padding-bottom: 20px; background-color: #ffffff;">
                        <div class="col-lg-12" style="padding-left: 10px; font-size: 12pt !important;">
                            For more information on the program:
                        </div>

                        <div class="col-lg-12" style="padding-top: 10px !important;">
                            <label class="custom-checkbox">
                                General information: Public Consulting Group PowerPoint Presentation of the Ohio Ambulance Supplemental Payment Program
                            <asp:CheckBox ID="chkGeneralInformation" runat="server" />
                                <span class="checkmark"></span>
                            </label>
                        </div>
                        <div class="col-lg-6">
                            <label class="custom-checkbox">
                                A Call Back
                        <asp:CheckBox ID="chkCallBack" runat="server" />
                                <span class="checkmark"></span>
                            </label>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group border-info border-10"
                        style="padding-left: 0px; padding-right: 0px; padding-top: 20px; padding-bottom: 20px; background-color: #ffffff;">
                        <div class="col-lg-12" style="padding-left: 10px; font-size: 12pt !important;">
                            A specific question:
                        </div>
                        <div class="col-lg-6" style="padding-left: 10px; padding-top: 20px;">
                            <asp:TextBox ID="txtSpecificQuestion" runat="server" autocomplete="off"
                                placeholder="Your answer" Text=""
                                CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group"
                        style="padding-left: 0px; padding-right: 0px; padding-top: 5px; padding-bottom: 50px;">
                        <div class="col-lg-2" style="padding-left: 0px; font-size: 12pt !important; float: left;">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-info"
                                Style="font-weight: bold; width: 100px!important; height: 36px!important;"
                                OnClick="btnSubmit_Click" />
                        </div>
                        <div class="col-lg-9" style="padding-left: 10px; font-size: 12pt !important;">
                        </div>
                        <div class="col-lg-2" style="padding-left: 10px; font-size: 12pt !important; float: right;">
                            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-danger"
                                Style="font-weight: bold; width: 100px!important; height: 36px!important;"
                                OnClick="btnClear_Click" />
                        </div>
                    </div>
                </div>
                <div id="divMessage" runat="server" class="col-lg-12 form-group border-info border-10 text-center"
                    style="padding: 20px; font-size: 20pt; background-color: #ffffff; display: none;">
                    <span class="text-success" style="font-weight: bold;">Sent Successfully!</span>
                </div>
                <%--                <div id="divError" runat="server" class="col-lg-12 form-group border-info border-10 text-center"
                        style="padding:20px; font-size: 20pt; background-color: #ffffff;">
                    <span class="text-danger" style="font-weight:bold;">Saved Successfully</span>
                </div>--%>
            </form>
        </div>
    </div>

</body>
</html>
