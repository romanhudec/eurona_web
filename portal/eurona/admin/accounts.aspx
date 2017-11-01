<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="Eurona.Admin.Accounts" Codebehind="accounts.aspx.cs" %>

<%@ Register Assembly="Eurona" Namespace="Eurona.Controls" TagPrefix="cmsAccount" %>

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
    <div style="padding:3px;">
        <a runat="server" href="~/admin/checkAccountsPolicy.aspx">Skontrolovat password policy na účtech</a>&nbsp;&nbsp
        <a runat="server" href="~/admin/checkAccountsEmails.aspx">Skontrolovat duplicity emalů na účtech</a>
    </div>
    <br /><br />
	<%--<cmsAccount:AdminAccountsControl runat="server" ID="adminAccounts" CssClass="dataGrid" HideCredit="true" IdentificationUrlFromat="~/admin/user.aspx?id={0}" AddCreditUrlFormat="~/admin/addAccountBonusCredit.aspx?id={0}" EditUrlFormat="~/admin/account.aspx?id={0}" RolesUrlFormat="~/admin/accountRoles.aspx?id={0}" NewUrl="~/admin/account.aspx?" />--%>
    <cmsAccount:AdminAccountsControl runat="server" ID="adminAccounts" CssClass="dataGrid" HideCredit="true" IdentificationUrlFromat="~/admin/user.aspx?id={0}" EditUrlFormat="~/admin/account.aspx?id={0}" RolesUrlFormat="~/admin/accountRoles.aspx?id={0}" NewUrl="~/admin/account.aspx?" />
</asp:Content>

