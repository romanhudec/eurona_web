<%@ Page Title="" Language="C#" MasterPageFile="~/eshop/admin/admin.master" AutoEventWireup="true" Inherits="Eurona.EShop.Admin.UrlAliasPage" Codebehind="urlAlias.aspx.cs" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls.UrlAlias" TagPrefix="cmsUrlAlias" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<a id="A2" href="~/eshop/admin/default.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:EShopStrings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<a id="A5" href="~/eshop/admin/urlAliases.aspx" runat="server"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:EShopStrings, Navigation_UrlAliases %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:EShopStrings, Navigation_UrlAlias %>" />
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
	<cmsUrlAlias:AdminUrlAliasControl runat="server" ID="adminUrlAlias" Width="600px" />
</asp:Content>

