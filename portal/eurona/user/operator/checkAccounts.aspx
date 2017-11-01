<%@ Page Title="" Language="C#" MasterPageFile="~/user/operator/admin.master" AutoEventWireup="true" Inherits="Eurona.Operator.CheckAccounts" Codebehind="checkAccounts.aspx.cs" %>

<%@ Register Assembly="Eurona" Namespace="Eurona.User.Operator" TagPrefix="operatorCtrl" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<a id="A2" href="~/admin/default.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_Users %>" />
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
	<operatorCtrl:AdminCheckAdvisorAccountsControl runat="server" ID="adminAccounts" CssClass="dataGrid" HideCredit="true" IdentificationUrlFromat="~/user/operator/user.aspx?id={0}" AddCreditUrlFormat="~/admin/addAccountCredit.aspx?id={0}" EditUrlFormat="~/user/operator/account.aspx?id={0}" RolesUrlFormat="~/admin/accountRoles.aspx?id={0}" NewUrl="~/user/operator/account.aspx?" />
</asp:Content>

