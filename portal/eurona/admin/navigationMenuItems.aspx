﻿<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="Eurona.Admin.AdminNavigationMenuItems" Codebehind="navigationMenuItems.aspx.cs" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls.Menu" TagPrefix="cmsMenu" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>	
		<a id="A2" href="~/admin/default.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<a id="A3" href="~/admin/navigationMenus.aspx" runat="server"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_NavigationMenus %>" /></a>
        <span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Strings, Navigation_NavigationMenuItems %>" />
	</div>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="content" Runat="server">
	<span>
	    <br />
	    <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:Strings, Navigation_NavigationMenu %>" />: <b><asp:Label runat="server" ID="lblNavigationMenu"></asp:Label></b>
	    <br />
	</span>
	<div>
	    <cmsMenu:AdminNavigationMenuItemsControl runat="server" ID="navigationMenuItems" CssClass="dataGrid" EditUrlFormat="~/admin/navigationMenuItem.aspx?id={0}" NewUrl="~/admin/navigationMenuItem.aspx?menuId={0}" />
	</div>
</asp:Content>
