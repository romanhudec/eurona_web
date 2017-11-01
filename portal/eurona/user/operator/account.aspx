<%@ Page Title="" Language="C#" MasterPageFile="~/user/operator/admin.master" AutoEventWireup="true" Inherits="Eurona.Operator.Account" Codebehind="account.aspx.cs" %>

<%@ Register Assembly="Eurona" Namespace="Eurona.Controls" TagPrefix="cmsAccount" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A3" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<a id="A4" href="~/admin/default.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<a id="A5" href="~/admin/accounts.aspx" runat="server"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_Users %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Strings, Navigation_User %>" />
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <div style="margin:auto;">
	<cms:RoundPanel ID="rpAccount" runat="server" CssClass="roundPanel" Width="400px">
	<cmsAccount:AdminAccountControl runat="server" ID="adminAccount" UseCapcha="false" UserDetailUrlFormat="~/user/operator/user.aspx?id={0}" RegisterUserUrlFormat="~/user/operator/registerUser.aspx?id={0}"  ChangePasswordUrlFormat="~/user/operator/changePassword.aspx?id={0}" GeneratePasswordUrlFormat="~/user/operator/generatePassword.aspx?id={0}" Width="400px" />
    </cms:RoundPanel>
    </div>
</asp:Content>

