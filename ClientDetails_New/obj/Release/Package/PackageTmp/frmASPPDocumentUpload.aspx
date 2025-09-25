<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmASPPDocumentUpload.aspx.cs" Inherits="ClientDetails.frmASPPDocumentUpload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ASPP Document Upload</title>
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
            font-size: 20px !important;
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
    <style>
        .custom {
            height: 50px !important;
            width: 150px !important;
            font-weight: bold;
            font-family: Calibri;
            font-size: 20px !important;
        }
    </style>

    <style>
        /* body {
            font-family: Arial, Helvetica, sans-serif;
        }*/

        /* The Modal (background) */
        .modal {
            display: none; /* Hidden by default */
            position: fixed; /* Stay in place */
            z-index: 1; /* Sit on top */
            padding-top: 100px; /* Location of the box */
            left: 0;
            top: 0;
            width: 100%; /* Full width */
            height: 100%; /* Full height */
            overflow: auto; /* Enable scroll if needed */
            background-color: rgb(0,0,0); /* Fallback color */
            background-color: rgba(0,0,0,0.4); /* Black w/ opacity */
        }

        /* Modal Content */
        .modal-content {
            background-color: #fefefe;
            margin: auto;
            padding: 20px;
            border: 1px solid #888;
            width: 75%;
        }

        /* The Close Button */
        .close {
            color: red !important;
            text-decoration: none !important;
            float: right;
            font-size: 20px;
            font-weight: bold;
            opacity: 1 !important;
        }

            .close:hover,
            .close:focus {
                color: #000;
                text-decoration: none;
                cursor: pointer;
            }
    </style>


</head>
<body>
    <div class="container h-100">
        <div class="row h-100 justify-content-center align-items-center">
            <form class="col-lg-12" runat="server">
                <asp:HiddenField ID="hdnID" runat="server" Value="" />
                <asp:HiddenField ID="hdnDeletedFileName" runat="server" Value="" />
                <div class="col-lg-12 form-group border-info border-10" style="padding-left: 0px; padding-right: 0px; padding-top: 10px;">
                    <div class="col-lg-12 justify-content-center align-items-center" style="text-align: center">
                        <img class="navbar-brand" src="Images/Logo.jpg" />
                    </div>
                    <div class="col-lg-12 form-group" style="padding-top: 20px;">
                        <div class="col-lg-2 d-inline-block"></div>
                        <div class="col-lg-3 d-inline-block" style="text-align:right;">
                            <label>Client Name</label>
                        </div>
                        <div class="col-lg-4 d-inline-block">
                            <asp:TextBox ID="txtClientName" runat="server" Text="" CssClass="form-control" MaxLength="100"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <div class="col-lg-2 d-inline-block"></div>
                        <div class="col-lg-3 d-inline-block" style="text-align:right;">
                            <label>Email Address</label>
                        </div>
                        <div class="col-lg-4 d-inline-block">
                            <asp:TextBox ID="txtEmailId" runat="server" Text="" CssClass="form-control" MaxLength="50"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <div class="col-lg-2 d-inline-block"></div>
                        <div class="col-lg-3 d-inline-block" style="text-align:right;">
                            <label>Upload Documents</label>
                        </div>
                        <div class="col-lg-4 d-inline-block">
                            <asp:FileUpload ID="fuFiles" runat="server" AllowMultiple="true" accept=".xls,.xlsx,.csv,.pdf,.doc,.docx,.png,.jpeg" />
                        </div>
                    </div>
                    <div class="col-lg-12 form-group">
                        <%--<div class="col-lg-4 d-inline-block"></div>
                        <div class="col-lg-3 d-inline-block">--%>
                        <center>
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-info custom"
                                OnClick="btnSubmit_Click" OnClientClick="return Validation();" />
                        </center>
                        <%--</div>--%>
                    </div>
                    <div class="col-lg-12 form-group" style="padding-top: 10px; padding-bottom: 10px;">
                        <center>
                            <asp:Label ID="lblMessage" runat="server" CssClass="text-danger"
                                Style="font-weight: bold; font-size: 24px;display:none"
                                Text="Thank you for submitting your documents"></asp:Label>
                        </center>
                    </div>
                    <div class="col-lg-12 form-group">
                        <asp:GridView ID="gvDocuments" runat="server" AutoGenerateColumns="false"
                            CssClass="table table-striped table-bordered table-hover"
                            AllowPaging="false" ShowHeaderWhenEmpty="true"
                            OnRowCommand="gvDocuments_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="List of Documents">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="gvhdnID" runat="server" Value='<%# Eval("ID") %>' />
                                        <asp:LinkButton ID="gvlnkbtnDocumentName" runat="server"
                                            Text='<%# Eval("OriginalFileName") %>'
                                            CommandName="cmdOpenFile"
                                            CommandArgument='<%# Eval("FileName")+","+ Eval("OriginalFileName") %>'
                                            Style="text-wrap: inherit;"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvlnkDelete" runat="server"
                                            ToolTip="Delete" CommandName="cmdDelete" Text="Delete"
                                            OnClientClick="return confirm('Are you sure, want to delete this document?');"
                                            CommandArgument='<%# Eval("ID")+","+ Eval("FileName") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                    </div>
                    <div class="col-lg-12 form-group" style="padding-top: 20px;">
                    </div>
                </div>

                <%-- <div id="myDeleteModal" class="modal">
                    <!-- Modal content -->
                    <div class="modal-content !important" style="width: 40% !important;">
                        <div class="col-lg-12 container rounded border-info border-5" style="padding-left: 0px; padding-right: 0px;">
                            <div class="text-lg-left bg-info form-group text-white" style="margin-top: -1px; margin-left: -1px;">
                                <b>Confirmation Message</b>
                            </div>
                            <div class="col-lg-12 form-group">
                                Are you sure that you want to delete the document?
                            </div>
                            <div class="col-lg-12 form-group text-lg-right">
                                <asp:Button ID="btnOk" runat="server" Text="Yes"
                                    CssClass="btn btn-info custom" OnClick="btnOk_Click" />
                                <input type="button" id="btnCancel" value="No" class="btn btn-danger custom" />
                            </div>
                            <input type="button" id="btnConfirmDummy" value="Dummy" class="btn btn-danger custom" style="display: none;" />
                        </div>
                    </div>
                </div>


                <div id="myMessageModal" class="modal">
                    <!-- Modal content -->
                    <div class="modal-content !important" style="width: 40% !important;">
                        <div class="col-lg-12 container rounded border-info border-5" style="padding-left: 0px; padding-right: 0px;">
                            <div class="text-lg-left bg-info form-group text-white" style="margin-top: -1px; margin-left: -1px;">
                                <b>Message</b>
                            </div>
                            <div class="col-lg-12 form-group">
                                <asp:Label ID="lblPopUpMessage" runat="server">Thank you for submitting your documents</asp:Label>
                            </div>
                            <div class="col-lg-12 form-group text-lg-right">
                                <input type="button" id="btnMessageOk" value="Ok" class="btn btn-info custom" />
                            </div>
                            <input type="button" id="btnMessageDummy" value="Dummy" 
                                class="btn btn-danger custom" style="display: none;" />
                        </div>
                    </div>
                </div>--%>
            </form>
        </div>
    </div>

    <script type="text/javascript">
        function Validation() {
            var fuOutputFile = document.getElementById("<%=fuFiles.ClientID %>");
            var txtClientName = document.getElementById("<%=txtClientName.ClientID %>");
            var txtEmailId = document.getElementById("<%=txtEmailId.ClientID %>");


            if (txtClientName.value.trim() == "") {
                alert("Enter Client Name");
                return false;
            }

            if (txtEmailId.value.trim() == "") {
                alert("Enter Email Id");
                return false;
            }
            if (fuOutputFile.value == "") {
                alert("Select file");
                return false;
            }

            if (!ValidateEmail(txtEmailId, "Invalid Email Id")) {
                //alert("Invalid Email Id");
                return false;
            }
            return true;
        }

    </script>

    <%--  <script>
        // Get the modal
        var myDeleteModal = document.getElementById("myDeleteModal");

        // Get the button that opens the modal
        var btnDeleteDummy = document.getElementById("btnDeleteDummy");

        // Get the <span> element that closes the modal
        var btnDeleteCancel = document.getElementById("btnDeleteCancel");

        // When the user clicks the button, open the modal 
        btnDeleteDummy.onclick = function () {
            myDeleteModal.style.display = "block";
        }

        // When the user clicks on <span> (x), close the modal
        //btnConfirmClose.onclick = function () {
        //    modal.style.display = "none";
        //}

        btnDeleteCancel.onclick = function () {
            myDeleteModal.style.display = "none";
        }

        // When the user clicks anywhere outside of the modal, close it
        window.onclick = function (event) {
            if (event.target == myDeleteModal) {
                myDeleteModal.style.display = "none";
            }
        }

        function OpenDeletePopup() {
            //alert("1");
            document.getElementById("btnDeleteDummy").click();
            //modal.style.display = "block";
        }
    </script>


    <script type="text/javascript">
        function OpenMessagePopup() {
            alert("1")

            document.getElementById("btnMessageDummy").click();
            //modal.style.display = "block";
        }

        // Get the modal
        var myMessageModal = document.getElementById("myMessageModal");

        // Get the button that opens the modal
        var btnMessageDummy = document.getElementById("btnMessageDummy");

        // Get the <span> element that closes the modal
        var btnMessageOk = document.getElementById("btnMessageOk");

        btnMessageOk.onclick = function () {
            myMessageModal.style.display = "none";
        }

        // When the user clicks the button, open the modal 
        btnMessageDummy.onclick = function () {
            myMessageModal.style.display = "block";
        }

        // When the user clicks anywhere outside of the modal, close it
        window.onclick = function (event) {
            if (event.target == modal) {
                myMessageModal.style.display = "none";
            }
        }
    </script>--%>
</body>
</html>

