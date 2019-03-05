<%@ Page Title="<%$ Resources:Strings, LoginControl_ForgotPasswordButton %>" Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" Inherits="Eurona.ForgotPassword" Codebehind="forgotPassword.aspx.cs" %>

<%@ Register assembly="eurona" namespace="Eurona.Controls" tagprefix="cmsAcount" %>
<%@ Register assembly="cms" namespace="CMS.Controls" tagprefix="cms" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .ms-formvalidation span{ color:#EA008A!important;}
    </style>
<script type="text/javascript">
    $(document).ready(function () {
        var elm = document.getElementById('<%=forgotPassword.txtEmail.ClientID%>');
        elm.oninput = function () {
            checkEmail();
        }
    });

    function checkEmail() {
        var email = document.getElementById('<%=forgotPassword.txtEmail.ClientID%>').value;
        var labelElm = document.getElementById('<%=forgotPassword.lblValidatorText.ClientID%>');
        var btnVerifyElm = document.getElementById('<%=forgotPassword.btnSend.ClientID%>');
        labelElm.style.display = "none";
        btnVerifyElm.disabled = true;
        var isEmailValid = validateEmailPattern(email);
        if (isEmailValid == false) {
            labelElm.innerText = "<%=Resources.Strings.EmailVerifyControl_EmailValidation_NespravnyFormat %>";
            labelElm.style.display = "block";
            return;
        }
        btnVerifyElm.disabled = false;
    }
</script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">
    <cms:RoundPanel ID="RoundPanel1" runat="server" CssClass="roundPanel" Width="450px">
        <cmsAcount:ForgotPasswordControl runat="server" id="forgotPassword" Width="400px"></cmsAcount:ForgotPasswordControl>
    </cms:RoundPanel>
</asp:Content>

