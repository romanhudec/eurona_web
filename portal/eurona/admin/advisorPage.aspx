<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="Eurona.Admin.AdvisorPage" Codebehind="advisorPage.aspx.cs" %>

<%@ Register Assembly="Eurona" Namespace="Eurona.Controls" TagPrefix="cmsPage" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A3" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<a id="A4" href="~/admin/default.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<a id="A5" href="~/admin/pages.aspx" runat="server"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_AdvisorPages %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Strings, Navigation_AdvisorPage %>" />
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
	<cmsPage:AdminAdvisorPageControl runat="server" ID="adminPage" Width="600px"/>
</asp:Content>
