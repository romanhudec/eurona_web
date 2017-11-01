<%@ Page Title="<%$ Resources:Strings, Navigation_News %>" Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" Inherits="Eurona.News" Codebehind="news.aspx.cs" %>

<%@ Register assembly="cms" namespace="CMS.Controls.News" tagprefix="cmsNews" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<a id="returnUrl"  href="~/newsArchiv.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_ArchivedNews %>" /></a>		
		<span>&nbsp;-&nbsp;</span>				
		<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_News %>" />
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">
    <cmsNews:NewsControl ID="newsControl" CssClass="news" runat="server" ArchivUrl="~/newsArchiv.aspx" />
</asp:Content>

