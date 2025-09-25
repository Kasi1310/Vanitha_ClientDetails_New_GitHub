<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmWebinarSurveySep.aspx.cs" Inherits="ClientDetails.frmWebinarSurveySep" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Medicount Webinar Survey</title>
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
            font-family: 'Times New Roman' !important;
            font-size: 20px;
        }
    </style>


    <style>
        input[type="range"] {
            -webkit-appearance: none;
            width: 100%;
            height: 20px;
            background: linear-gradient(to right, red, yellow, green);
        }

            input[type="range"]::-webkit-slider-thumb {
                -webkit-appearance: none;
                appearance: none;
                width: 20px;
                height: 20px;
                background-color: gray;
                border-radius: 50%;
                cursor: pointer;
            }

            input[type="range"]::-moz-range-thumb {
                width: 20px;
                height: 20px;
                background-color: white;
                border: none;
                border-radius: 50%;
                cursor: pointer;
            }

        datalist {
            display: flex;
            flex-direction: column;
            justify-content: space-between;
            writing-mode: vertical-lr;
            width: 100%;
        }

        option {
            transform: rotate(270deg); /* Rotate the values 90 degrees clockwise to appear vertically aligned */
        }


        /* .rating-scale {
            display: flex;
            justify-content: space-between;
            margin-top: 10px;
        }

            .rating-scale span {
                flex: 1;
                text-align: left;
            }*/
    </style>
    <style>
        .Custom-RadioButton input {
            display: none;
        }

        .Custom-RadioButton {
            font-size: 50px;
            cursor: pointer;
        }


        .selected {
            background-color: #17a2b8 !important;
        }
    </style>

    <script>
        $(document).ready(function () {
            $('.emojiQuestion1').click(function () {
                $('.emojiQuestion1').removeClass('selected');
                $(this).addClass('selected');
            });
            $('.emojiQuestion2').click(function () {
                $('.emojiQuestion2').removeClass('selected');
                $(this).addClass('selected');
            });
            $('.emojiQuestion3').click(function () {
                $('.emojiQuestion3').removeClass('selected');
                $(this).addClass('selected');
            });
        });
    </script>

</head>
<body>
    <div class="col-lg-12" style="text-align: center;">
        <asp:Label ID="lblMessage" runat="server" Style="font-weight: bold; font-size: 20px; color: green;"></asp:Label>
    </div>
    <div id="divControls" runat="server" class="container h-100" style="width:950px !important;">
        <div class="row h-100 justify-content-center align-items-center">
            <form class="col-lg-12" runat="server">
                <asp:HiddenField ID="hdnClientID" runat="server" Value="" />
                <asp:HiddenField ID="hdnQuestion9" runat="server" Value="" />
                <div class="col-lg-12 form-group border-info border-10" style="padding-left: 0px; padding-right: 0px; padding-top: 10px;">
                    <div class="col-lg-12 justify-content-center align-items-center">
                        <center>
                            <img class="navbar-brand" src="Images/Logo.jpg" />
                        </center>
                    </div>
                    <div class="col-lg-12 justify-content-center align-items-center text-info">
                        <center>
                            <h2><b>MEDICOUNT WEBINAR SURVEY</b></h2>
                        </center>
                    </div>
                    <div class="col-lg-12 form-group">
                        <div class="col-lg-3 d-inline-block"  style="width:175px !important;">
                            
                            <label><span class="text-danger">*</span>Name</label>
                        </div>
                        <div class="col-lg-4 d-inline-block">
                            <asp:TextBox ID="txtName" runat="server" Text="" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <div class="col-lg-3 d-inline-block"  style="width:175px !important;">
                            
                            <label><span class="text-danger">*</span>Email</label>
                        </div>
                        <div class="col-lg-4 d-inline-block">
                            <asp:TextBox ID="txtEmail" runat="server" Text="" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <div class="col-lg-3 d-inline-block" style="width:175px !important;">
                            
                            <label><span class="text-danger">*</span>Company Name</label>
                        </div>
                        <div class="col-lg-4 d-inline-block">
                            <asp:TextBox ID="txtCompanyName" runat="server" Text="" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <label>1.	How would you rate the relevance and usefulness of the content presented in the Coffee with Kathy webinar?</label><br>
                        <asp:RadioButtonList ID="rdolstQuestion1" runat="server" RepeatDirection="Horizontal"
                            CssClass="Custom-RadioButton" style="width:100% !important;">
                            <asp:ListItem class="emojiQuestion1" Value="Very Dissatisfied">&#x1F61E;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion1" Value="Dissatisfied">&#x1F615;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion1" Value="Neither Satisfied nor Dissatisfied">&#x1F610;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion1" Value="Satisfied">&#x1F642;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion1" Value="Very Satisfied">&#x1F600;</asp:ListItem>
                        </asp:RadioButtonList>

                    </div>
                    <div class="col-lg-12 form-group">
                        <label>2.	How would you rate the presenter's clarity, knowledge, and engagement level?</label><br>

                        <asp:RadioButtonList ID="rdolstQuestion2" runat="server" RepeatDirection="Horizontal"
                            CssClass="Custom-RadioButton"  style="width:100% !important;">
                            <asp:ListItem class="emojiQuestion2" Value="Very Dissatisfied">&#x1F61E;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion2" Value="Dissatisfied">&#x1F615;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion2" Value="Neither Satisfied nor Dissatisfied">&#x1F610;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion2" Value="Satisfied">&#x1F642;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion2" Value="Very Satisfied">&#x1F600;</asp:ListItem>
                        </asp:RadioButtonList>

                    </div>
                    <div class="col-lg-12 form-group">
                        <label>3.	How would you rate the PowerPoint slides?</label><br>
                        <asp:RadioButtonList ID="rdolstQuestion3" runat="server" RepeatDirection="Horizontal" 
                            CssClass="Custom-RadioButton"  style="width:100% !important;">
                            <asp:ListItem class="emojiQuestion3" Value="Very Dissatisfied">&#x1F61E;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion3" Value="Dissatisfied">&#x1F615;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion3" Value="Neither Satisfied nor Dissatisfied">&#x1F610;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion3" Value="Satisfied">&#x1F642;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion3" Value="Very Satisfied">&#x1F600;</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div class="col-lg-10 form-group">
                        <label>4.   What did you like most about the presentation? What could be improved?</label><br>
                        <asp:TextBox ID="txtQuestion4" runat="server" Text="" CssClass="form-control"
                            TextMode="MultiLine" Rows="3" Style="resize: none;"></asp:TextBox>
                    </div>

                    <div class="col-lg-12 form-group" style="padding-bottom: 30px;">
                        <div class="col-lg-6 d-inline-block" style="float: left;">
                            <label id="lblErrorMessage" style="font-weight: bold; font-size: 15px; color: red;"></label>
                        </div>
                        <div class="col-lg-1 d-inline-block">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-info"
                                Width="100px" Height="50px" Style="font-size: 20px !important; font-weight: bold !important;"
                                OnClientClick="return Validation();" OnClick="btnSubmit_Click" />
                        </div>
                    </div>
                </div>
            </form>
        </div>
    </div>

    <script type="text/javascript">
        function Validation() {
            var lblErrorMessage = document.getElementById("lblErrorMessage");

            var txtName = document.getElementById("<%=txtName.ClientID %>");
            var txtEmail = document.getElementById("<%=txtEmail.ClientID %>");
            var txtCompanyName = document.getElementById("<%=txtCompanyName.ClientID %>");

            lblErrorMessage.innerHTML = "";

            if (txtName.value.trim() == "") {
                lblErrorMessage.innerHTML = "Enter Name"
                txtName.focus();
                return false;
            }
            if (txtEmail.value.trim() == "") {
                lblErrorMessage.innerHTML = "Enter Email"
                txtEmail.focus();
                return false;
            }
            if (txtCompanyName.value.trim() == "") {
                lblErrorMessage.innerHTML = "Enter Company Name"
                txtCompanyName.focus();
                return false;
            }

            if (txtEmail.value.trim() != "") {
                var mailformat = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
                if (!txtEmail.value.match(mailformat)) {
                    lblErrorMessage.innerHTML = "Invalid MailID"
                    txtEmail.focus();
                    return false;
                }
            }

            return true;
        }
    </script>
</body>
</html>
