<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="Eurona.Admin.WebErrorDetailPage" Codebehind="webErrorDetail.aspx.cs" %>

<%@ Register Assembly="Eurona" Namespace="Eurona.Controls" TagPrefix="cmsAccount" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<a id="A2" href="~/admin/default.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<a id="A3" href="~/admin/webErrors.aspx" runat="server"><asp:Literal ID="Literal4" runat="server" Text="WEB Chyby" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal2" runat="server" Text="WEB Chyba" />
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
	<h2>Web Chyba Detail</h2>
	Uživatel:&nbsp;<b><asp:Literal runat="server" ID="lblName"></asp:Literal></b>
	<hr />
	Datum:&nbsp;<b><asp:Literal runat="server" ID="lblDate"></asp:Literal></b>
	<hr />	
	Stránka:&nbsp;<b><asp:Literal runat="server" ID="lblLocation"></asp:Literal></b>
	<hr />	
	<asp:Literal runat="server" ID="lblExeption"></asp:Literal>
	<hr />
	<asp:Literal runat="server" ID="lblStackTrace"></asp:Literal>
	<br /><br />
	<asp:Button runat="server" ID="btnBack" OnClick="OnBack" Text="Zpět" />
</asp:Content>

