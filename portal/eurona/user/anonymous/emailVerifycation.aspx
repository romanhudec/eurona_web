<%@ Page  Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" Inherits="Eurona.User.Anonymous.EmailVerifycation" Codebehind="emailVerifycation.aspx.cs" %>
<asp:Content ID="Content5" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script> 
    <link href='<%=ResolveUrl("~/styles/emailVerify.css") %>' type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">
    <script type="text/jscript">
        var verifyForm = document.getElementById('verifyForm');
        var layoutVerifycationProcess;
        var layoutVerifycationSuccessFinish;
        var layoutVerifycationFailed;
        $(document).ready(function () {
            verifyForm = document.getElementById('verifyForm');
            layoutVerifycationProcess = document.getElementById('layoutVerifycationProcess');
            layoutVerifycationSuccessFinish = document.getElementById('layoutVerifycationSuccessFinish');
            layoutVerifycationFailed = document.getElementById('layoutVerifycationFailed');
            layoutVerifycationMessage = document.getElementById('layoutVerifycationMessage');

            $.blockUI.defaults.overlayCSS.cursor = 'default';
            $.blockUI({
                message: $('#verifyForm'),
                overlayCSS: { backgroundColor: '#333' },
                css: {
                    top: '30%',
                    width: 'aouto',
                    left: '32%',
                    right: '32%',
                    cursor: 'default',
                    border: 'none',
                    padding: '15px',
                    backgroundColor: '#fff',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    'border-radius': '10px',
                    opacity: 1,
                    color: '#EA008A'
                }
            });

            startVerifycationProcess();
        });

        function startVerifycationProcess() {
            showLayoutVerifycationProcess();
            $.ajax({
                url: "<%=Page.ResolveUrl("~/user/emailVerifycationService.ashx")%>?method=verify",
                data: "<%=Page.Request["code"]%>",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    if (data.Status == 0) {
                        continueVerificationToFinish();
                    } else if(data.Status == 2){
                        showLayoutVerifycationMessage(data.Message);
                    } else {
                        showLayoutVerifycationFailed(data.ErrorMessage);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    showLayoutVerifycationFailed(null);
                }
            });
        }

        function continueVerificationToFinish() {
            showLayoutVerifycationProcess();
            $.ajax({
                url: "<%=Page.ResolveUrl("~/user/emailVerifycationService.ashx")%>?method=verifyAnonymousFinish",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    if (data.Status == 0) {
                        showLayoutVerifycationSuccessFinish(data.Message);
                    } else {
                        showLayoutVerifycationFailed(data.ErrorMessage);
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    showLayoutVerifycationFailed(null);
                }
            });
        }


        function showLayoutVerifycationProcess() {
            verifyForm.style.display = 'block';
            layoutVerifycationProcess.style.display = 'block';
            layoutVerifycationSuccessFinish.style.display = 'none';
            layoutVerifycationFailed.style.display = 'none';
        }

        function showLayoutVerifycationFailed(message) {
            verifyForm.style.display = 'block';
            layoutVerifycationProcess.style.display = 'none';
            layoutVerifycationSuccessFinish.style.display = 'none';
            layoutVerifycationFailed.style.display = 'block';
            var lblErrorMesage = document.getElementById('<%=lblErrorMesage.ClientID%>');
            if (message != null && message.length != 0) {
                lblErrorMesage.innerHTML = message;
            }
        }

        function showLayoutVerifycationMessage(message) {
            verifyForm.style.display = 'block';
            layoutVerifycationProcess.style.display = 'none';
            layoutVerifycationSuccessFinish.style.display = 'none';
            layoutVerifycationFailed.style.display = 'none';
            layoutVerifycationMessage.style.display = 'block';
            var lblMesage = document.getElementById('<%=lblMesage.ClientID%>');
            if (message != null && message.length != 0) {
                lblMesage.innerHTML = message;
            }
        }

        function showLayoutVerifycationSuccessFinish(message) {
            var lblFinishMessage = document.getElementById('<%=lblFinishMessage.ClientID%>');
            lblFinishMessage.innerHTML = message;
            verifyForm.style.display = 'block';
            layoutVerifycationProcess.style.display = 'none';
            layoutVerifycationSuccessFinish.style.display = 'block';
            layoutVerifycationFailed.style.display = 'none';
        }

        function onContinueToCancel() {
            $.ajax({
                url: "<%=Page.ResolveUrl("~/user/emailVerifycationService.ashx")%>?method=verifyCancel",
                data: "",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    location.href = "<%=Page.ResolveUrl("~/")%>";
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    location.href = "<%=Page.ResolveUrl("~/")%>";
                }
            });
        }

        function onContinueToOffice() {
            $.ajax({
                url: "<%=Page.ResolveUrl("~/user/emailVerifycationService.ashx")%>?method=getRedirectUrlAfterVerify",
                data: "<%=Page.Request["code"]%>",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    location.href = "<%=Page.ResolveUrl("~/user/advisor/default.aspx?verified")%>";
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    location.href = "<%=Page.ResolveUrl("~/user/advisor/default.aspx?verified")%>";
                }
            });
        }

    </script>
    <div id="verifyForm" style="display:none;">
        <div id="layoutVerifycationProcess" style="display:none;">
            <table style="text-align:center;margin: 0 auto;">
                <tr><td><h4><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, EmailVerifyControl_Title %>"></asp:Literal></h4></td></tr>
                <tr><td class="message"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, EmailVerifyControl_VerifyingEmail_Message %>"></asp:Literal></td></tr>
                <tr><td style="text-align:center;"><asp:Image ID="Image3" runat="server" ImageUrl="~/images/ajax-indicator.gif" /></td></tr>
            </table>
        </div>
        <div id="layoutVerifycationSuccessFinish" style="display:none;">
            <table style="text-align:center;margin: 0 auto;">
                <tr><td><h4><asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:Strings, EmailVerifyControl_Title %> "></asp:Literal></h4></td></tr>
                <tr><td><asp:Label runat="server" ID="lblFinishTitle" Text="<%$ Resources:Strings, EmailVerifyControl_VerifycationSuccessFinish_Title %>" style="text-align:center;color:#EA008A;font-size:20px;"></asp:Label></td></tr>
                <tr>
                    <td>
                        <div class="message" style="padding-top:10px;">
                            <asp:Label runat="server" ID="lblFinishMessage" Text="<%$ Resources:Strings, EmailVerifyControl_VerifycationSuccessFinish_Message %>" ></asp:Label>
                        </div>
                    </td>
                </tr>
                <tr><td style="text-align:center;padding-bottom:10px;"><asp:Image ID="Image4" runat="server" ImageUrl="~/images/success.png" /></td></tr>  
                <tr>
                    <td>
                        <asp:Button ID="btnFinish" runat="server" CssClass="button" Text="<%$ Resources:Strings, EmailVerifyControl_Pokracovat %>" OnClientClick="onContinueToOffice();" />    
                    </td>
                </tr>
            </table>
        </div>
        <div id="layoutVerifycationFailed" style="display:none;">
            <table style="text-align:center;margin: 0 auto;width:100%;">
                <tr><td><h4><asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Strings, EmailVerifyControl_Title %>"></asp:Literal></h4></td></tr>
                <tr><td style="text-align:center;"><asp:Image ID="Image2" runat="server" ImageUrl="~/images/warning.png" /></td></tr> 
                <tr><td class="message"><asp:Label ID="lblErrorMesage" runat="server" Text="<%$ Resources:Strings, EmailVerifyControl_EmailVerifiedFailed_Message %>"></asp:Label></td></tr> 
                <tr>
                    <td style="text-align:right;padding-top:10px;">
                        <asp:Button ID="Button1" runat="server" CssClass="button" Text="<%$ Resources:Strings, EmailVerifyControl_Opakovat %>" OnClientClick="startVerifycationProcess();" />
                    </td>
                </tr>                              
            </table>
        </div>
        <div id="layoutVerifycationMessage" style="display:none;">
            <table style="text-align:center;margin: 0 auto;width:100%;">
                <tr><td><h4><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, EmailVerifyControl_Title %>"></asp:Literal></h4></td></tr>
                <tr><td style="text-align:center;"><asp:Image ID="Image1" runat="server" ImageUrl="~/images/success.png" /></td></tr> 
                <tr><td class="message"><asp:Label ID="lblMesage" runat="server"></asp:Label></td></tr> 
                <tr>
                    <td style="text-align:right;padding-top:10px;">
                        <asp:Button ID="Button2" runat="server" CssClass="button" Text="<%$ Resources:Strings, EmailVerifyControl_Pokracovat %>" OnClientClick="onContinueToOffice();" />
                    </td>
                </tr>                              
            </table>
        </div>
    </div>
</asp:Content>

