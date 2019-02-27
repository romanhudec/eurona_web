<%@ Page Title="<%$ Resources:Strings, LoginControl_ForgotPasswordButton %>" Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" Inherits="Eurona.ForgotPassword" Codebehind="forgotPassword.aspx.cs" %>

<%@ Register assembly="eurona" namespace="Eurona.Controls" tagprefix="cmsAcount" %>
<%@ Register assembly="cms" namespace="CMS.Controls" tagprefix="cms" %>

<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">
    <cms:RoundPanel ID="RoundPanel1" runat="server" CssClass="roundPanel" Text="">
        <cmsAcount:ForgotPasswordControl runat="server" id="forgotPassword" Width="400px"></cmsAcount:ForgotPasswordControl>
    </cms:RoundPanel>
</asp:Content>

