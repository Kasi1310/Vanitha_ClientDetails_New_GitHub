<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmClientProposal.aspx.cs" Inherits="ClientDetails.frmClientProposal" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link href="Content/bootstrap.css" rel="stylesheet" />
    <script src="Scripts/Custom/jquery.min.js"></script>
    <script src="Scripts/Custom/bootstrap.min.js"></script>
    <link href="Scripts/Custom/bootstrap.min.css" rel="stylesheet" />
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
    <style>
        .custom-checkbox input {
            display: none;
        }

        .custom-checkbox label span {
            border: 3px solid #7e8a94;
            float: right;
            height: 30px;
            width: 60px;
            border-radius: 5px;
            cursor: pointer;
            display: flex;
            justify-content: center;
            align-items: center;
            margin-right: 10px;
            margin-left: 10px;
        }

        .custom-checkbox:hover span,
        .custom-checkbox input:checked + label span {
            border: 3px solid #7e8a94;
        }

            .custom-checkbox input:checked + label span:before {
                content: "✔";
                /*position: absolute;*/
                font-size: 1.6em;
                line-height: 0.8;
                transition: all .2s;
            }
    </style>
</head>
<body>
    <div class="container h-100">
        <div class="row h-100 justify-content-center align-items-center">
            <form class="col-lg-12" runat="server">
                <asp:HiddenField ID="hdnReferenceCoverLetter" runat="server" Value="" />
                <asp:HiddenField ID="hdnReferenceProposal" runat="server" Value="" />
                <asp:HiddenField ID="hdnAttachement" runat="server" Value="" />
                <div class="col-lg-12 justify-content-center align-items-center">
                    <img class="navbar-brand" src="Images/Logo.jpg" style="width: 300px; height: 100px;" />
                </div>
                <div class="col-lg-12 form-group border-info border-10"
                    style="padding-left: 0px; padding-right: 0px; padding-top: 0px;">
                    <div align="center" class="col-lg-12 bg-info form-group text-white" style="font-weight: bold;">
                        <span style="font-size: 24px; font-weight: bold;">DETAILED PROPOSAL FORM</span>
                    </div>
                    <div class="col-lg-12 form-group">
                        <asp:Label ID="lblMessage" runat="server" CssClass="text-success font-weight-bold"
                            Text="Details send successfully."
                            Style="display: none;"></asp:Label>
                        <asp:Label ID="lblError" runat="server" CssClass="text-danger font-weight-bold"
                            Text="Details not send, please contact Medicount team"
                            Style="display: none;"></asp:Label>
                    </div>
                    <div runat="server" class="col-lg-12" id="divContent">
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>Name of Client:</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:TextBox ID="txtClient" runat="server" CssClass="form-control" Text="" MaxLength="100" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">

                            <div class="col-lg-6 d-inline-block">
                                <label>Account Executive name(s):</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:TextBox ID="txtAEName" runat="server" CssClass="form-control" Text="" MaxLength="500" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>Name of Entity - Confirm:</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:TextBox ID="txtNameofEntity" runat="server" CssClass="form-control" Text="" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>Referral From Who/Where:</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:TextBox ID="txtReferralFrom" runat="server" CssClass="form-control" Text="" MaxLength="500" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>Change Healthcare Client:</label>
                            </div>
                            <div class="col-lg-4 d-inline-block">
                                <asp:RadioButtonList ID="rdolstIsChangeHealthCareClient" runat="server" RepeatDirection="Horizontal" CssClass="custom-checkbox" Style="font-weight: bold;">
                                    <asp:ListItem Value="Yes">YES<span></span></asp:ListItem>
                                    <asp:ListItem Value="No">NO<span></span></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>If Yes, do they use the ALS Dispatch Protocol:</label>
                            </div>
                            <div class="col-lg-4 d-inline-block">
                                <asp:RadioButtonList ID="rdolstIsALSDispatchProtocol" runat="server" RepeatDirection="Horizontal" CssClass="custom-checkbox" Style="font-weight: bold;">
                                    <asp:ListItem Value="Yes">YES<span></span></asp:ListItem>
                                    <asp:ListItem Value="No">NO<span></span></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>Image of the potential Client's ambulance for the cover page:</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:FileUpload ID="fuImageOfPotentialClientsAmbulance" runat="server" />
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>Image of the potential Client’s emblem/badge for the cover page:</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:FileUpload ID="fuImageOfPotentialClientsEmblem" runat="server" />
                            </div>

                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>Name(s) and title of individuals receiving the proposal for cover letter and cover page:</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:TextBox ID="txtNamesAndTitleOfIndividuals" runat="server" CssClass="form-control" Text="" MaxLength="500" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>Address of Entity for Cover Letter:</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:TextBox ID="txtAddressOfEntity" runat="server" CssClass="form-control" Text="" MaxLength="500" autocomplete="off"></asp:TextBox>
                            </div>

                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>County Name:</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:TextBox ID="txtCountyName" runat="server" CssClass="form-control" Text="" MaxLength="500" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>Phone Number:</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:TextBox ID="txtPhoneNumber" runat="server" CssClass="form-control" Text="" MaxLength="500" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>Email address of your Contact (for UPS services):</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="form-control" Text="" MaxLength="500" autocomplete="off"></asp:TextBox>
                            </div>

                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>The number of proposals requested:</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:TextBox ID="txtNumberOfProposalsRequested" runat="server" CssClass="form-control" Text="" MaxLength="500" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>Approximate Date of when the proposal is needed:</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:TextBox ID="txtApproximateDateOfProposal" runat="server" CssClass="form-control" Text="" MaxLength="500" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>Proposals: Mailed directly to the potential Client or the account executive (or picked up)</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:TextBox ID="txtProposalsMailedDirectly" runat="server" CssClass="form-control" Text="" MaxLength="500" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>Fee % - Recommended:</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:TextBox ID="txtFeeRecommended" runat="server" CssClass="form-control" Text="" MaxLength="500" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>Additional equipment or software applications that need to be included in the fee and or Client Services Agreement:</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:TextBox ID="txtAdditionalEquipment" runat="server" CssClass="form-control" Text="" MaxLength="500" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>Length of term for Client Services Agreement: </label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:TextBox ID="txtLengthOfClientServicesAgreement" runat="server" CssClass="form-control" Text="" MaxLength="500" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <%--<div class="col-lg-12 form-group">
                        <div class="col-lg-6 d-inline-block">
                            <label>Change Healthcare Client:</label>
                        </div>
                        <div class="col-lg-6 d-inline-block">
                            <asp:TextBox ID="txtChangeHealthCareClient" runat="server" CssClass="form-control" Text="" MaxLength="500" autocomplete="off"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <div class="col-lg-6 d-inline-block">
                            <label>ALS Protocol required in CSA: Protocol</label>
                        </div>
                        <div class="col-lg-6 d-inline-block">
                            <asp:TextBox ID="txtALSProtocalRequiredCSAProtocol" runat="server" CssClass="form-control" Text="" MaxLength="500" autocomplete="off"></asp:TextBox>
                        </div>
                    </div>--%>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>Reference on the Cover letter:</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:FileUpload ID="fuReferenceCoverLetter" runat="server" AllowMultiple="true" />
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>References in the body of the proposal:</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:FileUpload ID="fuReferenceProposal" runat="server" AllowMultiple="true" />
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>Are any ePCR or equipment Charges included in the previous biller contract?</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:RadioButtonList ID="rdolstIsEPCROrEquipmentCharges" runat="server" RepeatDirection="Horizontal" CssClass="custom-checkbox" Style="font-weight: bold;">
                                    <asp:ListItem Value="Yes">YES<span></span></asp:ListItem>
                                    <asp:ListItem Value="No">NO<span></span></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>Please attach a copy of the current billing contract.</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:FileUpload ID="fuCopyOfTheCurrentBillersContract" runat="server" />
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>Attached a report from a client in the last 12 months, Transport & Revenue.</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:FileUpload ID="fuAttachedTransportAndRevenue" runat="server" />
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>Any Pass Through Charges?</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:TextBox ID="txtAnyPassThroughCharges" runat="server" CssClass="form-control" Text="" MaxLength="500" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-lg-12 form-group">
                            <div class="col-lg-6 d-inline-block">
                                <label>Other Comments:</label>
                            </div>
                            <div class="col-lg-6 d-inline-block">
                                <asp:TextBox ID="txtOtherComments" runat="server" CssClass="form-control" Text="" MaxLength="500" autocomplete="off"></asp:TextBox>
                            </div>
                        </div>
                        <div align="center" class="col-lg-12 form-group">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-info custom"
                                OnClick="btnSubmit_Click" OnClientClick="return Validation();" />

                            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-danger custom"
                                OnClick="btnClear_Click" />
                        </div>
                    </div>
                </div>
            </form>
        </div>
    </div>

    <script type="text/javascript">
        function Validation() {
            var txtClient = document.getElementById("<%=txtClient.ClientID %>");
            if (txtClient.value.trim() == "") {
                alert("Enter Name Of Client");
                return false;
            }
        }
    </script>
</body>
</html>
