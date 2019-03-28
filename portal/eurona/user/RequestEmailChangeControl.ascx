<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequestEmailChangeControl.ascx.cs" Inherits="Eurona.User.RequestEmailChangeControl" %>
<script type="text/javascript">
    document.getElementById('<%=txtEmail.ClientID%>').addEventListener('keypress', function (event) {
        if (event.keyCode == 13) {
            document.getElementById('<%=btnVerify.ClientID%>').click();
        }
    });
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

    function onBtnVerifyClick() {
        var layoutTypeEmail = document.getElementById('layoutTypeEmail');
        var layoutSendinEmail = document.getElementById('layoutSendinEmail');
        var layoutSendinEmailError = document.getElementById('layoutSendinEmailError');
        var layoutEmailSend = document.getElementById('layoutEmailSend');
        layoutTypeEmail.style.display = 'none';
        layoutSendinEmail.style.display = 'table';
        layoutSendinEmailError.style.display = 'none';
        layoutEmailSend.style.display = 'none';

        var email = document.getElementById('<%=txtEmail.ClientID%>').value;
        $.ajax({
            url: "<%=Page.ResolveUrl("~/user/emailVerifycationService.ashx")%>?method=sendEmail2ChangeEmail",
            data: email,
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) { return data; },
            success: function (data) {
                if (data.Status == 0) {
                    layoutTypeEmail.style.display = 'none';
                    layoutSendinEmail.style.display = 'none';
                    layoutSendinEmailError.style.display = 'none';
                    layoutEmailSend.style.display = 'table';
                } else {
                    layoutTypeEmail.style.display = 'none';
                    layoutSendinEmail.style.display = 'none';
                    layoutSendinEmailError.style.display = 'table';
                    layoutEmailSend.style.display = 'none';
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                layoutTypeEmail.style.display = 'none';
                layoutSendinEmail.style.display = 'none';
                layoutSendinEmailError.style.display = 'table';
                layoutEmailSend.style.display = 'none';
            }
        });
    }

    function onContinueToCancel() {
        $.ajax({
            url: "<%=Page.ResolveUrl("~/user/emailVerifycationService.ashx")%>?method=changeEmailCancel",
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
</script>
<table style="width:100%;">
    <tr>
        <td class="title" style="text-align:center;"><h4><asp:Literal ID="Label1" runat="server" Text="<%$ Resources:Strings, ChangeEmailControl_Title %>"></asp:Literal></h4></td>
    </tr>
    <tr>
        <td>
            <div style="width:100%;display:block;">
                <table id="layoutTypeEmail" style="width:100%;display:table;">
                    <tr>
                        <td colspan="2">
                        <div class="input-description-black" style="padding-top:0px;text-align:left;padding-bottom:10px;">
                            <asp:Label runat="server" ID="Label2" Text="<%$ Resources:Strings, ChangeEmailControl_EmailDescriptionTop %>" style="text-align:left;" ></asp:Label>
                        </div>
                        </td>    
                    </tr>
                    <tr>
                        <td colspan="2">
                            <div class="input-label">
                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Strings, ChangeEmailControl_Email %>"></asp:Label>
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
                            <asp:Button ID="btnCancelAndLogout" runat="server" CssClass="button" Text="<%$ Resources:Strings, ChangeEmailControl_ZrusitZmenu %>" OnClientClick="onContinueToCancel();" />    
                        </td>
                        <td style="text-align:right;padding-top:10px;">
                            <asp:Button runat="server" ID="btnVerify" CssClass="button" Text="<%$ Resources:Strings, ChangeEmailControl_OveritEmail %>" disabled="true" OnClientClick="onBtnVerifyClick();" />
                        </td>
                    </tr>
                </table>
                <table id="layoutSendinEmail" style="width:100%;display:none;">
                    <tr>
                        <td class="message"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, ChangeEmailControl_SendingEmail_Message %>"></asp:Literal></td>
                    </tr>
                    <tr>
                        <td style="text-align:center;color:#EA008A;font-size: 16px;"><asp:Image ID="Image3" runat="server" ImageUrl="~/images/ajax-indicator.gif" /></td>
                    </tr>
                </table>
                <table id="layoutEmailSend" style="width:100%;display:none;">
                    <tr>
                         <td class="message"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, ChangeEmailControl_EmailSended_Message %>"></asp:Literal></td>
                    </tr>  
                    <tr>
                         <td style="text-align:center;color:#EA008A;font-size: 16px;"><asp:Image ID="Image1" runat="server" ImageUrl="~/images/success.png" /></td>
                    </tr>                              
                </table>
                <table id="layoutSendinEmailError" style="width:100%;display:none;">
                    <tr>
                         <td style="text-align:center;color:#EA008A;font-size: 16px;"><asp:Image ID="Image2" runat="server" ImageUrl="~/images/warning.png" /></td>
                    </tr> 
                    <tr>
                         <td class="message"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, ChangeEmailControl_SendingEmailError_Message %>"></asp:Literal></td>
                    </tr> 
                    <tr>
                        <td style="text-align:right;padding-top:10px;">
                            <asp:Button runat="server" CssClass="button" Text="<%$ Resources:Strings, ChangeEmailControl_OdeslatZnovu %>" OnClientClick="onBtnVerifyClick();" />
                        </td>
                    </tr>                              
                </table>
            </div>
        </td>
    </tr>
</table>
