<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="Eurona.Admin.ImageGallery" Codebehind="imageGallery.aspx.cs" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls.ImageGallery" TagPrefix="cmsImageGallery" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A3" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<a id="A4" href="~/admin/default.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<a id="A5" href="~/admin/imageGalleries.aspx" runat="server"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_ImageGalleries %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Strings, Navigation_ImageGallery %>" />
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <cmsImageGallery:AdminImageGalleryControl ID="adminImageGalleryControl" UrlAliasPrefixId="3" DisplayUrlFormat="~/imageGallery.aspx?id={0}" CommentsFormatUrl="~/imageGalleryComments.aspx?id={0}" runat="server"/>
</asp:Content>

