<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="Eurona.Admin.AdvisorPages" Codebehind="advisorPages.aspx.cs" %>

<%@ Register Assembly="Eurona" Namespace="Eurona.Controls" TagPrefix="cmsPage" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<a id="A2" href="~/admin/default.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_AdvisorPages %>" />
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
	<cmsPage:AdminAdvisorPagesControl runat="server" ID="adminAdvisorPagesControl" CssClass="dataGrid"
		EditUrlFormat="~/admin/advisorPage.aspx?id={0}" EditContentUrlFormat="~/advisorPage.aspx?id={0}&edit=1"
		NewUrl="~/admin/advisorPage.aspx?" />
</asp:Content>
