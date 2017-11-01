<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="Eurona.Admin.ImageGalleryItems" Codebehind="imageGalleryItems.aspx.cs" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls.ImageGallery" TagPrefix="cmsImageGallery" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<a id="A2" href="~/admin/default.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_ImageGallery %>" />
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <cmsImageGallery:AdminImageGalleryItemsControl ID="adminImageGalleryItemsControl" CssClass="adminImageGalleryItemsControl" CommentsFormatUrl="~/imageGalleryItemComments.aspx?id={0}" runat="server" />
</asp:Content>

