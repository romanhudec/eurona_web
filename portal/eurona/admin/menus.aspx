<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="Eurona.Admin.AdminMenus" Codebehind="menus.aspx.cs" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls.Menu" TagPrefix="cmsMenu" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>	
		<a id="A2" href="~/admin/default.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_Menus %>" />
	</div>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="content" Runat="server">
	<cmsMenu:AdminMenusControl runat="server" ID="adminMenus" CssClass="dataGrid" EditUrlFormat="~/admin/menu.aspx?id={0}" NewUrl="~/admin/menu.aspx?" />
</asp:Content>
