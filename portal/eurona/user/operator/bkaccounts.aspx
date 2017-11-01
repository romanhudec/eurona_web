<%@ Page Title="" Language="C#" MasterPageFile="~/user/operator/admin.master" AutoEventWireup="true" Inherits="Eurona.Operator.BKAccounts" Codebehind="bkaccounts.aspx.cs" %>

<%@ Register Assembly="eurona" Namespace="Eurona.Controls" TagPrefix="cmsAccount" %>

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
	<h2>Bonusové kredity - uživatelé</h2>
	<cmsAccount:AdminBKAccountsControl runat="server" ID="adminAccounts" CssClass="dataGrid" HideCredit="true" IdentificationUrlFromat="~/admin/user.aspx?id={0}" AddCreditUrlFormat="~/admin/addAccountBonusCredit.aspx?id={0}" EditUrlFormat="~/admin/account.aspx?id={0}" RolesUrlFormat="~/admin/accountRoles.aspx?id={0}" NewUrl="~/admin/account.aspx?" />
</asp:Content>

