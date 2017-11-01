<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="Eurona.Admin.AdminNavigationMenuItem" Codebehind="navigationMenuItem.aspx.cs" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls.Menu" TagPrefix="cmsMenu" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>	
		<a id="A2" href="~/admin/default.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<a id="A3" href="~/admin/navigationMenus.aspx" runat="server"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_NavigationMenus %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Strings, Navigation_NavigationMenuItem %>" />
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
	<cmsMenu:AdminNavigationMenuItemControl runat="server" ID="navigationMenuItem" Width="600px" />
</asp:Content>
