<%@ Page Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" Inherits="Eurona.User.Anonymous.RequestEmailVerifycation" CodeBehind="requestEmailVerifycation.aspx.cs" %>

<%@ Register Src="~/user/RequestEmailVerifyControl.ascx" TagPrefix="uc1" TagName="RequestEmailVerifyControl" %>

<asp:Content ID="Content5" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
    <link href='<%=ResolveUrl("~/styles/emailVerify.css") %>' type="text/css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" runat="Server">
    <script type="text/jscript">
        $(document).ready(function () {
            $.blockUI.defaults.overlayCSS.cursor = 'default';
            $.blockUI({
                message: $('#verfityForm'),
                overlayCSS: { backgroundColor: '#333' },
                css: {
                    top: '30%',
                    width: '30%',
                    left: '35%',
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

            <% if( this.Page.Request["status"] == "typeEmail"){%>
                startVerifyTypeEmail();
            <%} else {%>
                startVerifyAfterRegistration();
            <%}%>
        });

        function startVerifyTypeEmail() {
            var email = document.getElementById('<%=txtEmail.ClientID%>').value;
            var labelElm = document.getElementById('<%=lblValidatorText.ClientID%>');
            var btnVerifyElm = document.getElementById('<%=btnVerify.ClientID%>');

            var layoutTypeEmail = document.getElementById('layoutTypeEmail');
            var layoutSendinEmail = document.getElementById('layoutSendinEmail');
            var layoutSendinEmailError = document.getElementById('layoutSendinEmailError');
            var layoutEmailSend = document.getElementById('layoutEmailSend');
            layoutTypeEmail.style.display = 'table';
            layoutSendinEmail.style.display = 'none';
            layoutSendinEmailError.style.display = 'none';
            layoutEmailSend.style.display = 'none';
        }

        function onBtnVerifyClick() {
            var emailId = document.getElementById('<%=txtEmail.ClientID%>').value +'|'+ <%=Page.Request["Id"]%> + "";
            startVerifyAfterRegistration(emailId);
        }

        function startVerifyAfterRegistration(emailId) {
            var layoutTypeEmail = document.getElementById('layoutTypeEmail');
            var layoutSendinEmail = document.getElementById('layoutSendinEmail');
            var layoutSendinEmailError = document.getElementById('layoutSendinEmailError');
            var layoutEmailSend = document.getElementById('layoutEmailSend');
            layoutTypeEmail.style.display = 'none';
            layoutSendinEmail.style.display = 'table';
            layoutSendinEmailError.style.display = 'none';
            layoutEmailSend.style.display = 'none';

            var method = "sendEmail2EmailAnonymousVerify";
            if( emailId != null ){
                method = "sendEmail2EmailAnonymousVerifyEmailId";
            }
            $.ajax({
                url: "<%=Page.ResolveUrl("~/user/emailVerifycationService.ashx")%>?method=" + method,
                dataType: "json",
                data: emailId,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    if (data.Status == 0) {
                        layoutSendinEmail.style.display = 'none';
                        layoutSendinEmailError.style.display = 'none';
                        layoutEmailSend.style.display = 'table';
                    } else {
                        layoutSendinEmail.style.display = 'none';
                        layoutSendinEmailError.style.display = 'table';
                        layoutEmailSend.style.display = 'none';
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    layoutSendinEmail.style.display = 'none';
                    layoutSendinEmailError.style.display = 'table';
                    layoutEmailSend.style.display = 'none';
                }
            });

        }

        function checkEmail() {
            var email = document.getElementById('<%=txtEmail.ClientID%>').value;
            var labelElm = document.getElementById('<%=lblValidatorText.ClientID%>');
            var btnVerifyElm = document.getElementById('<%=btnVerify.ClientID%>');
            labelElm.style.display = "none";
            btnVerifyElm.disabled = true;
            var isEmailValid = validateEmailPattern(email);
            if (isEmailValid == false) {
                labelElm.innerText = "<%=Resources.Strings.EmailVerifyControl_EmailValidation_NespravnyFormat %>";
                labelElm.style.display = "block";
                return;
            }


            $.ajax({
                url: "<%=Page.ResolveUrl("~/user/emailVerifycationService.ashx")%>?method=checkEmail",
                data: email,
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    if (data.Status != 0) {
                        labelElm.innerText = data.ErrorMessage;
                        labelElm.style.display = "block";
                    } else {
                        btnVerifyElm.disabled = false;
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    labelElm.style.display = "block";
                }
            });

        }

        function onClose() {
            location.href = "<%=Page.ResolveUrl("~/")%>";
        }
    </script>
    <div id="verfityForm" style="display: none; cursor: default;">
        <table id="layoutSendinEmail" style="width: 100%; display: none;">
            <tr>
                <td class="message">
                    <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, EmailVerifyControl_SendingEmail_Message %>"></asp:Literal></td>
            </tr>
            <tr>
                <td style="text-align: center; color: #EA008A; font-size: 16px;">
                    <asp:Image ID="Image3" runat="server" ImageUrl="~/images/ajax-indicator.gif" /></td>
            </tr>
        </table>
        <table id="layoutTypeEmail" style="width:100%;display:table;">
            <tr>
                <td colspan="2">
                <div class="input-description-black" style="padding-top:0px;text-align:left;padding-bottom:10px;">
                    <asp:Label runat="server" ID="Label2" Text="<%$ Resources:Strings, EmailVerifyControl_EmailDescriptionTop %>" style="text-align:left;" ></asp:Label>
                </div>
                </td>    
            </tr>
            <tr>
                <td colspan="2">
                    <div class="input-label">
                        <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Strings, EmailVerifyControl_Email %>"></asp:Label>
                    </div>
                    <div>
                    <asp:TextBox runat="server" ID="txtEmail" Width="100%" type="email" CssClass="form-control" oninput="checkEmail()"></asp:TextBox>
                    </div>
                    <div class="validation-message">
                        <asp:Label runat="server" ID="lblValidatorText" style="display:none;"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align:left;padding-top:10px;">
                    <asp:Button ID="btnCancelAndLogout" runat="server" CssClass="button-blue" Text="<%$ Resources:Strings, EmailVerifyControl_ZrusitOvereni %>" OnClientClick="onContinueToCancel();" />    
                </td>
                <td style="text-align:right;padding-top:10px;">
                    <asp:Button runat="server" ID="btnVerify" CssClass="button" Text="<%$ Resources:Strings, EmailVerifyControl_OveritEmail %>" disabled="true" OnClientClick="onBtnVerifyClick();" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div class="input-description">
                        <asp:Label runat="server" ID="Label1" Text="<%$ Resources:Strings, EmailVerifyControl_DescriptionBottom %>" ></asp:Label>
                    </div>
                </td>
            </tr>
        </table>
        <table id="layoutEmailSend" style="width: 100%; display: none;">
            <tr>
                <td class="message">
                    <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, EmailVerifyControl_AnonymousEmailSended_Message %>"></asp:Literal></td>
            </tr>
            <tr>
                <td style="text-align: center; color: #EA008A; font-size: 16px;">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/success.png" /></td>
            </tr>
            <tr> 
                <td style="text-align:center;padding-top:10px;">
                    <asp:Button ID="btnOk" runat="server" CssClass="button" Text=" Ok " OnClientClick="onClose();" />    
                </td>   
            </tr>  
        </table>
        <table id="layoutSendinEmailError" style="width: 100%; display: none;">
            <tr>
                <td style="text-align: center; color: #EA008A; font-size: 16px;">
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/images/warning.png" /></td>
            </tr>
            <tr>
                <td class="message">
                    <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, EmailVerifyControl_SendingEmailError_Message %>"></asp:Literal></td>
            </tr>
            <tr>
                <td style="text-align: right; padding-top: 10px;">
                    <asp:Button ID="Button1" runat="server" CssClass="button" Text="<%$ Resources:Strings, EmailVerifyControl_OdeslatZnovu %>" OnClientClick="onBtnVerifyClick();" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

