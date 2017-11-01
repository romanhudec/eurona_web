<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="Eurona.Admin.AdminNavigationMenus" Codebehind="navigationMenus.aspx.cs" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls.Menu" TagPrefix="cmsMenu" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>	
		<a id="A2" href="~/admin/default.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_NavigationMenus %>" />
	</div>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="content" Runat="server">
	<cmsMenu:AdminNavigationMenusControl runat="server" ID="navigationMenus" CssClass="dataGrid" EditUrlFormat="~/admin/navigationMenu.aspx?id={0}" NewUrl="~/admin/navigationMenu.aspx?" SubMenuUrlFormat="~/admin/navigationMenuItems.aspx?id={0}" />
</asp:Content>
