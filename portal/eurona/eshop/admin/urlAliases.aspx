<%@ Page Title="" Language="C#" MasterPageFile="~/eshop/admin/admin.master" AutoEventWireup="true" Inherits="Eurona.EShop.Admin.UrlAliasesPage" Codebehind="urlAliases.aspx.cs" %>

<%@ Register Assembly="shp" Namespace="SHP.Controls.UrlAlias" TagPrefix="shpUrlAlias" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<a id="A2" href="~/eshop/admin/default.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:EShopStrings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<a id="A3" href="~/eshop/admin/urlAliases.aspx" runat="server"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:EShopStrings, Navigation_UrlAliases %>" /></a>
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
	<shpUrlAlias:AdminUrlAliasesControl runat="server" ID="adminUrlAliasesControl" CssClass="dataGrid" EditUrlFormat="~/eshop/admin/urlAlias.aspx?id={0}" NewUrl="~/eshop/admin/urlAlias.aspx?" />
</asp:Content>

