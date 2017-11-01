<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="Eurona.Admin.CheckAccountsPolicy" Codebehind="checkAccountsPolicy.aspx.cs" %>

<%@ Register Assembly="Eurona" Namespace="Eurona.Controls" TagPrefix="cmsAccount" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

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
    <div>
        <h2>Kontrola password policy na účtech - chybné policy</h2>
    </div>
    <br /><br />
    <span style="float:right;font-weight:bold;"><asp:LinkButton ID="LinkButton1" runat="server" CssClass="exporttoexcel" ForeColor="#eb0a5b" Text="Export" OnClick="OnExport"></asp:LinkButton></span>
    <cmsAccount:AdminAccountsControl runat="server" ID="adminAccounts" CssClass="dataGrid" HideCredit="true" IdentificationUrlFromat="~/admin/user.aspx?id={0}" AddCreditUrlFormat="~/admin/addAccountBonusCredit.aspx?id={0}" EditUrlFormat="~/admin/account.aspx?id={0}" RolesUrlFormat="~/admin/accountRoles.aspx?id={0}" NewUrl="~/admin/account.aspx?" OnOnDataLoad="adminAccounts_OnDataLoad" />
    <div style="padding:20px;text-align:right; background-color:#FFFFFF">
        <asp:Button runat="server" ID="btnRepair" Text="Opravit password policy na všech chybných účtech" OnClick="btnRepair_Click" />
    </div>
</asp:Content>

