<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmMedicountClientSurvey.aspx.cs" Inherits="ClientDetails.frmMedicountClientSurvey" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Medicount Survey</title>
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
            $('.emojiQuestion4').click(function () {
                $('.emojiQuestion4').removeClass('selected');
                $(this).addClass('selected');
            });
            $('.emojiQuestion5').click(function () {
                $('.emojiQuestion5').removeClass('selected');
                $(this).addClass('selected');
            });
            $('.emojiQuestion6').click(function () {
                $('.emojiQuestion6').removeClass('selected');
                $(this).addClass('selected');
            });
            $('.emojiQuestion7').click(function () {
                $('.emojiQuestion7').removeClass('selected');
                $(this).addClass('selected');
            });
            $('.emojiQuestion8a').click(function () {
                $('.emojiQuestion8a').removeClass('selected');
                $(this).addClass('selected');
            });
            $('.emojiQuestion8b').click(function () {
                $('.emojiQuestion8b').removeClass('selected');
                $(this).addClass('selected');
            });
            $('.emojiQuestion8c').click(function () {
                $('.emojiQuestion8c').removeClass('selected');
                $(this).addClass('selected');
            });
            $('.emojiQuestion8d').click(function () {
                $('.emojiQuestion8d').removeClass('selected');
                $(this).addClass('selected');
            });
        });
    </script>

</head>
<body>
    <div class="col-lg-12" style="text-align: center;">
        <asp:Label ID="lblMessage" runat="server" Style="font-weight: bold; font-size: 20px; color: green;"></asp:Label>
    </div>
    <div id="divControls" runat="server" class="container h-100">
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
                            <h2><b>MEDICOUNT CLIENT SURVEY</b></h2>
                        </center>
                    </div>
                    <div class="col-lg-12 form-group text-justify">
                        Medicount kindly requests your feedback regarding the following aspects of Medicount's services. Your responses will enable us to gain insights into areas for improvement and enhance our ability to serve you effectively.
                    </div>
                    <div class="col-lg-12 form-group">

                        <label>1.	How would you rate your overall satisfaction with <b>Medicount's EMS Billing Services?</b></label><br>

                        <asp:RadioButtonList ID="rdolstQuestion1" runat="server" RepeatDirection="Horizontal" CssClass="Custom-RadioButton"
                            Style="width: 100% !important;">
                            <asp:ListItem class="emojiQuestion1" Value="Very Dissatisfied">&#x1F61E;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion1" Value="Dissatisfied">&#x1F615;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion1" Value="Neither Satisfied nor Dissatisfied">&#x1F610;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion1" Value="Satisfied">&#x1F642;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion1" Value="Very Satisfied">&#x1F600;</asp:ListItem>
                        </asp:RadioButtonList>

                    </div>
                    <div class="col-lg-12 form-group">
                        <label>2.	How would you rate the service(s) provided by your <b>Medicount Account Executive?</b></label><br>

                        <asp:RadioButtonList ID="rdolstQuestion2" runat="server" RepeatDirection="Horizontal" CssClass="Custom-RadioButton"
                            Style="width: 100% !important;">
                            <asp:ListItem class="emojiQuestion2" Value="Very Dissatisfied">&#x1F61E;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion2" Value="Dissatisfied">&#x1F615;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion2" Value="Neither Satisfied nor Dissatisfied">&#x1F610;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion2" Value="Satisfied">&#x1F642;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion2" Value="Very Satisfied">&#x1F600;</asp:ListItem>
                        </asp:RadioButtonList>

                    </div>
                    <div class="col-lg-12 form-group">
                        <label>3.	How satisfied are you with the <b>Medicount Customer Portal?</b></label><br>

                        <asp:RadioButtonList ID="rdolstQuestion3" runat="server" RepeatDirection="Horizontal" CssClass="Custom-RadioButton"
                            Style="width: 100% !important;">
                            <asp:ListItem class="emojiQuestion3" Value="Very Dissatisfied">&#x1F61E;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion3" Value="Dissatisfied">&#x1F615;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion3" Value="Neither Satisfied nor Dissatisfied">&#x1F610;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion3" Value="Satisfied">&#x1F642;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion3" Value="Very Satisfied">&#x1F600;</asp:ListItem>
                        </asp:RadioButtonList>

                    </div>
                    <div class="col-lg-12 form-group">
                        <label>4.	How usefull is Medicount's <b>Communication</b> of news, events, and updates?</label><br>

                        <asp:RadioButtonList ID="rdolstQuestion4" runat="server" RepeatDirection="Horizontal" CssClass="Custom-RadioButton"
                            Style="width: 100% !important;">
                            <asp:ListItem class="emojiQuestion4" Value="Very Dissatisfied">&#x1F61E;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion4" Value="Dissatisfied">&#x1F615;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion4" Value="Neither Satisfied nor Dissatisfied">&#x1F610;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion4" Value="Satisfied">&#x1F642;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion4" Value="Very Satisfied">&#x1F600;</asp:ListItem>
                        </asp:RadioButtonList>

                    </div>
                    <div class="col-lg-12 form-group">
                        <label>5.	How satisfied are you with the <b>Payments Team</b> in answering your questions about reconciliation, missing payments, audits, or statements, etc.?</label><br>

                        <asp:RadioButtonList ID="rdolstQuestion5" runat="server" RepeatDirection="Horizontal" CssClass="Custom-RadioButton"
                            Style="width: 100% !important;">
                            <asp:ListItem class="emojiQuestion5" Value="Very Dissatisfied">&#x1F61E;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion5" Value="Dissatisfied">&#x1F615;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion5" Value="Neither Satisfied nor Dissatisfied">&#x1F610;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion5" Value="Satisfied">&#x1F642;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion5" Value="Very Satisfied">&#x1F600;</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div class="col-lg-12 form-group">
                        <label>6.	How satisfied are you with Medicount's <b>Patient Customer Service Department?</b></label><br>
                        <asp:RadioButtonList ID="rdolstQuestion6" runat="server" RepeatDirection="Horizontal" CssClass="Custom-RadioButton"
                            Style="width: 100% !important;">
                            <asp:ListItem class="emojiQuestion6" Value="Very Dissatisfied">&#x1F61E;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion6" Value="Dissatisfied">&#x1F615;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion6" Value="Neither Satisfied nor Dissatisfied">&#x1F610;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion6" Value="Satisfied">&#x1F642;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion6" Value="Very Satisfied">&#x1F600;</asp:ListItem>
                        </asp:RadioButtonList>

                    </div>
                    <div class="col-lg-12 form-group">
                        <label>7.	How satisfied are you with Medicount’s Documentation & Signature Training?</label><br>

                        <asp:RadioButtonList ID="rdolstQuestion7" runat="server" RepeatDirection="Horizontal" CssClass="Custom-RadioButton"
                            Style="width: 100% !important;">
                            <asp:ListItem class="emojiQuestion7" Value="Very Dissatisfied">&#x1F61E;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion7" Value="Dissatisfied">&#x1F615;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion7" Value="Neither Satisfied nor Dissatisfied">&#x1F610;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion7" Value="Satisfied">&#x1F642;</asp:ListItem>
                            <asp:ListItem class="emojiQuestion7" Value="Very Satisfied">&#x1F600;</asp:ListItem>
                        </asp:RadioButtonList>

                    </div>
                    <div class="col-lg-12 form-group">
                        <label>8.	How satisfied are you with Medicount’s:</label><br>
                        <ul>
                            <li>Billing Statement
                                <asp:RadioButtonList ID="rdolstQuestion8a" runat="server" RepeatDirection="Horizontal" CssClass="Custom-RadioButton"
                                    Style="width: 50% !important;">
                                    <asp:ListItem class="emojiQuestion8a" Value="Very Dissatisfied">&#x1F61E;</asp:ListItem>
                                    <asp:ListItem class="emojiQuestion8a" Value="Very Satisfied">&#x1F600;</asp:ListItem>
                                </asp:RadioButtonList>
                            </li>
                            <li>Semi & Annual Report
                                <asp:RadioButtonList ID="rdolstQuestion8b" runat="server" RepeatDirection="Horizontal" CssClass="Custom-RadioButton"
                                    Style="width: 50% !important;">
                                    <asp:ListItem class="emojiQuestion8b" Value="Very Dissatisfied">&#x1F61E;</asp:ListItem>
                                    <asp:ListItem class="emojiQuestion8b" Value="Very Satisfied">&#x1F600;</asp:ListItem>
                                </asp:RadioButtonList>
                            </li>
                            <li>Overall Report Capability
                                <asp:RadioButtonList ID="rdolstQuestion8c" runat="server" RepeatDirection="Horizontal" CssClass="Custom-RadioButton"
                                    Style="width: 50% !important;">
                                    <asp:ListItem class="emojiQuestion8c" Value="Very Dissatisfied">&#x1F61E;</asp:ListItem>
                                    <asp:ListItem class="emojiQuestion8c" Value="Very Satisfied">&#x1F600;</asp:ListItem>
                                </asp:RadioButtonList>
                            </li>
                            <li>Contacting the person you want to speak to at Medicount
                                <asp:RadioButtonList ID="rdolstQuestion8d" runat="server" RepeatDirection="Horizontal" CssClass="Custom-RadioButton"
                                    Style="width: 50% !important;">
                                    <asp:ListItem class="emojiQuestion8d" Value="Very Dissatisfied">&#x1F61E;</asp:ListItem>
                                    <asp:ListItem class="emojiQuestion8d" Value="Very Satisfied">&#x1F600;</asp:ListItem>
                                </asp:RadioButtonList>
                            </li>
                        </ul>


                    </div>

                    <div class="col-lg-12 form-group">
                        <label>9.	How likely is it that you would recommend Medicount EMS Billing Services to a colleague or neighboring department?</label><br>

                        <input type="range" id="ratingQuestion9" name="rating" min="1" max="10" step="1" value="5" list="values">

                        <datalist id="values">
                            <option value="1" label="1"></option>
                            <option value="2" label="2"></option>
                            <option value="3" label="3"></option>
                            <option value="4" label="4"></option>
                            <option value="5" label="5"></option>
                            <option value="6" label="6"></option>
                            <option value="7" label="7"></option>
                            <option value="8" label="8"></option>
                            <option value="9" label="9"></option>
                            <option value="10" label="10"></option>
                        </datalist>
                    </div>
                    <div class="col-lg-12 form-group">
                        <label>10.	Do you wish to be contacted by Medicount Management, Inc. regarding the survey? If so, Please fill out the following information:</label><br>
                    </div>
                    <div class="col-lg-12 form-group">
                        <div class="col-lg-2 d-inline-block">
                            <label>Name</label>
                        </div>
                        <div class="col-lg-4 d-inline-block">
                            <asp:TextBox ID="txtName" runat="server" Text="" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <div class="col-lg-2 d-inline-block">
                            <label>Agency Name</label>
                        </div>
                        <div class="col-lg-4 d-inline-block">
                            <asp:TextBox ID="txtAgencyName" runat="server" Text="" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <div class="col-lg-2 d-inline-block">
                            <label>Email</label>
                        </div>
                        <div class="col-lg-4 d-inline-block">
                            <asp:TextBox ID="txtEmail" runat="server" Text="" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <div class="col-lg-2 d-inline-block">
                            <label>Phone</label>
                        </div>
                        <div class="col-lg-4 d-inline-block">
                            <asp:TextBox ID="txtPhone" runat="server" Text="" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group" style="padding-bottom: 30px;">
                        <div class="col-lg-11 d-inline-block" style="float: left;">
                            <label id="lblErrorMessage" style="font-weight: bold; font-size: 15px; color: red;"></label>
                        </div>
                        <div class="col-lg-1 d-inline-block" style="float: right;">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-info custom"
                                OnClientClick="return Validation();" OnClick="btnSubmit_Click" />
                        </div>
                    </div>
                </div>
            </form>
        </div>
    </div>

    <script type="text/javascript">
        function Validation() {
            var ratingQuestion9 = document.getElementById("ratingQuestion9");
            var hdnQuestion9 = document.getElementById("<%=hdnQuestion9.ClientID %>");
            var lblErrorMessage = document.getElementById("lblErrorMessage");

            hdnQuestion9.value = ratingQuestion9.value;

            var txtEmail = document.getElementById("<%=txtEmail.ClientID %>");
            var txtPhone = document.getElementById("<%=txtPhone.ClientID %>");

            lblErrorMessage.innerHTML = "";

            if (txtEmail.value.trim() != "") {
                var mailformat = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$/;
                if (!txtEmail.value.match(mailformat)) {
                    lblErrorMessage.innerHTML = "Invalid MailID"
                    txtEmail.focus();
                    return false;
                }
            }

            if (txtPhone.value.trim() != "") {
                var strPhoneNumber = txtPhone.value.trim();
                // For button click event start
                if (strPhoneNumber.length == 8) {
                    strPhoneNumber = strPhoneNumber.substring(0, 3) + strPhoneNumber.substring(4, 8);
                }
                else if (strPhoneNumber.length == 14) {
                    strPhoneNumber = strPhoneNumber.substring(1, 4) + strPhoneNumber.substring(6, 9) + strPhoneNumber.substring(10, 14);
                }
                //End

                if (strPhoneNumber != "") {
                    if (!strPhoneNumber.match(/^\d+$/)) {
                        lblErrorMessage.innerHTML = "Invalid Phone";
                        txtPhone.focus();
                        return false;
                    }
                    else {
                        if (strPhoneNumber.length == 7) {
                            strPhoneNumber = strPhoneNumber.substring(0, 3) + "-" + strPhoneNumber.substring(3, 7);
                            txtPhone.value = strPhoneNumber;
                            return true;
                        }
                        else if (strPhoneNumber.length == 10) {
                            strPhoneNumber = "(" + strPhoneNumber.substring(0, 3) + ") " + strPhoneNumber.substring(3, 6) + "-" + strPhoneNumber.substring(6, 10);
                            txtPhone.value = strPhoneNumber;
                            return true;
                        }
                        else {
                            lblErrorMessage.innerHTML = "Invalid Phone";
                            txtPhone.focus();
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    </script>
</body>
</html>
