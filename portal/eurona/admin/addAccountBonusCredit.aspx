<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="Eurona.Admin.AddAccountBonusCredit" Codebehind="addAccountBonusCredit.aspx.cs" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls.Account" TagPrefix="cmsAccount" %>

<%@ Register src="Controls/AccountBKAdminControl.ascx" tagname="AccountBKAdminControl" tagprefix="uc1" %>

<%@ Register src="../Controls/BonusoveKredityUzivateleInfoControl.ascx" tagname="BonusoveKredityUzivateleInfoControl" tagprefix="uc2" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A3" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<a id="A4" href="~/admin/default.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<a id="A5" href="~/admin/accounts.aspx" runat="server"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_Users %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Strings, Navigation_AddAccountCredit %>" />
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <h2>Ruční vložení bonusových kreditů na uživatelský účet</h2>
    <h3>Souhrn kreditů uživatele</h3>
    <uc2:BonusoveKredityUzivateleInfoControl ID="bonusoveKredityUzivateleInfoControl" runat="server" />
    <h3>Vložit kredity na uživatelský účet</h3>
    <uc1:AccountBKAdminControl ID="accountBKAdminControl" runat="server" />
</asp:Content>

