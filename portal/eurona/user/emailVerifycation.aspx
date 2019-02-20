<%@ Page  Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" Inherits="Eurona.User.EmailVerifycation" Codebehind="emailVerifycation.aspx.cs" %>
<asp:Content ID="Content5" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script> 
    <link href='<%=ResolveUrl("~/styles/emailVerify.css") %>' type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">
    <script type="text/jscript">
        var verifyForm = document.getElementById('verifyForm');
        var layoutVerifycationProcess;
        var layoutVerifycationSuccessStep1;
        var layoutVerifycationSuccessFinish;
        var layoutVerifycationFailed;
        $(document).ready(function () {
            verifyForm = document.getElementById('verifyForm');
            layoutVerifycationProcess = document.getElementById('layoutVerifycationProcess');
            layoutVerifycationSuccessStep1 = document.getElementById('layoutVerifycationSuccessStep1');
            layoutVerifycationSuccessFinish = document.getElementById('layoutVerifycationSuccessFinish');
            layoutVerifycationFailed = document.getElementById('layoutVerifycationFailed');

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
                        showLayoutVerifycationSuccessStep1();
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
            layoutVerifycationSuccessStep1.style.display = 'none';
            layoutVerifycationSuccessFinish.style.display = 'none';
            layoutVerifycationFailed.style.display = 'none';
        }

        function showLayoutVerifycationFailed(message) {
            verifyForm.style.display = 'block';
            layoutVerifycationProcess.style.display = 'none';
            layoutVerifycationSuccessStep1.style.display = 'none';
            layoutVerifycationSuccessFinish.style.display = 'none';
            layoutVerifycationFailed.style.display = 'block';
            var lblErrorMesage = document.getElementById('<%=lblErrorMesage.ClientID%>');
            if (message != null) {
                lblErrorMesage.innerHTML = message;
            }
        }

        function showLayoutVerifycationSuccessStep1() {
            verifyForm.style.display = 'block';
            layoutVerifycationProcess.style.display = 'none';
            layoutVerifycationSuccessStep1.style.display = 'block';
            layoutVerifycationSuccessFinish.style.display = 'none';
            layoutVerifycationFailed.style.display = 'none';
            var btnContinueToFinish = document.getElementById('<%=btnContinueToFinish.ClientID%>');
            btnContinueToFinish.disabled = true;
            clearPassword();
        }

        function showLayoutVerifycationSuccessFinish(message) {
            var lblFinishMessage = document.getElementById('<%=lblFinishMessage.ClientID%>');
            lblFinishMessage.innerHTML = message;
            verifyForm.style.display = 'block';
            layoutVerifycationProcess.style.display = 'none';
            layoutVerifycationSuccessStep1.style.display = 'none';
            layoutVerifycationSuccessFinish.style.display = 'block';
            layoutVerifycationFailed.style.display = 'none';
        }

        function onContinueToFinish() {
            showLayoutVerifycationProcess();
            $.ajax({
                url: "<%=Page.ResolveUrl("~/user/emailVerifycationService.ashx")%>?method=verifyFinish",
                data: document.getElementById('<%=txtPwd.ClientID%>').value,
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

        function clearPassword() {
            document.getElementById('<%=txtPwd.ClientID%>').value = '';
            document.getElementById('<%=txtPwdRepeat.ClientID%>').value = '';
        }

        function validatePwd() {
            var btnContinueToFinish = document.getElementById('<%=btnContinueToFinish.ClientID%>');
            var elmErrorMessage = document.getElementById('<%=lblValidatorTextPwd.ClientID%>');           
            var elmErrorMessageRepeat = document.getElementById('<%=lblValidatorTextPwdRepeat.ClientID%>');
            var elm = document.getElementById('<%=txtPwd.ClientID%>');
            var elmRepeat = document.getElementById('<%=txtPwdRepeat.ClientID%>');

            var result = validatePasswordAndRepeatPassword(elm, elmRepeat, elmErrorMessage, elmErrorMessageRepeat);
            if (result == false) {
                btnContinueToFinish.disabled = true;
            } else {
                btnContinueToFinish.disabled = false;
            }
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
        <div id="layoutVerifycationSuccessStep1" style="display:none;">
            <table style="text-align:center;margin: 0 auto;width:100%;">
                <tr><td colspan="2"><h4><asp:Literal ID="Label1" runat="server" Text="<%$ Resources:Strings, EmailVerifyControl_Title %>"></asp:Literal></h4></td></tr>
                <tr><td colspan="2" style="padding-bottom:10px;"><asp:Label runat="server" Text="<%$ Resources:Strings, EmailVerifyControl_VerifycationSuccessStep1_Title %>" style="text-align:center;color:#EA008A;font-size: 20px;"></asp:Label></td></tr>

                <tr>
                    <td colspan="2">
                        <div class="input-label">
                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Strings, EmailVerifyControl_Password %>"></asp:Label>
                        </div>
                        <asp:TextBox runat="server" ID="txtPwd" Width="100%" TextMode="Password" CssClass="form-control" oninput="validatePwd();"></asp:TextBox>
                        <div class="validation-message">
                            <asp:Label runat="server" ID="lblValidatorTextPwd" Text="<%$ Resources:Strings, EmailVerifyControl_PasswordErrorMessage %>" ForeColor="#EA008A" style="display:none;"></asp:Label>
                        </div>
                        <div class="input-label">
                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Strings, EmailVerifyControl_PasswordRepeat %>"></asp:Label>
                        </div>
                        <asp:TextBox runat="server" ID="txtPwdRepeat" Width="100%" TextMode="Password" CssClass="form-control" oninput="validatePwd();"></asp:TextBox>
                        <div class="validation-message">
                            <asp:Label runat="server" ID="lblValidatorTextPwdRepeat" Text="<%$ Resources:Strings, EmailVerifyControl_PasswordRepeatErrorMessage %>" ForeColor="#EA008A" style="display:none;"></asp:Label>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="padding-top:20px;text-align:left;">
                        <asp:Button ID="btnCancelAndLogout" runat="server" CssClass="button" Text="<%$ Resources:Strings, EmailVerifyControl_ZrusitOvereni %>" OnClientClick="onContinueToCancel();" />    
                    </td>
                    <td style="padding-top:20px;text-align:right;">
                        <asp:Button ID="btnContinueToFinish" runat="server" CssClass="button-positive" disabled="true"  Text="<%$ Resources:Strings, EmailVerifyControl_DokoncitOvereni %>" OnClientClick="onContinueToFinish();" />    
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div class="input-description">
                            <asp:Label runat="server" ID="Label4" Text="<%$ Resources:Strings, EmailVerifyControl_PasswordDescriptionBottom %>" ></asp:Label>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="layoutVerifycationSuccessFinish" style="display:none;">
            <table style="text-align:center;margin: 0 auto;">
                <tr><td><h4><asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:Strings, EmailVerifyControl_Title %> "></asp:Literal></h4></td></tr>
                <tr><td><asp:Label runat="server" ID="Label3" Text="<%$ Resources:Strings, EmailVerifyControl_VerifycationSuccessFinish_Title %>" style="text-align:center;color:#EA008A;font-size:20px;"></asp:Label></td></tr>
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
    </div>
</asp:Content>

