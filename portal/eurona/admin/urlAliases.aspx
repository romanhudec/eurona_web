<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="Eurona.Admin.UrlAliasesPage" Codebehind="urlAliases.aspx.cs" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls.UrlAlias" TagPrefix="cmsUrlAlias" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<a id="A2" href="~/admin/default.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_UrlAliases %>" />
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
	<cmsUrlAlias:AdminUrlAliasesControl runat="server" ID="adminUrlAliasesControl" CssClass="dataGrid" EditUrlFormat="~/admin/urlAlias.aspx?id={0}" NewUrl="~/admin/urlAlias.aspx?" />
</asp:Content>

