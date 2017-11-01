<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="Eurona.Admin.ImageGalleries" Codebehind="imageGalleries.aspx.cs" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls.ImageGallery" TagPrefix="cmsImageGallery" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<a id="A2" href="~/admin/default.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_ImageGalleries %>" />
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <cmsImageGallery:AdminImageGalleriesControl ID="adminImageGalleriesControl" CssClass="dataGrid" runat="server"
		EditUrlFormat="~/admin/imageGallery.aspx?Id={0}" NewUrl="~/admin/imageGallery.aspx" ItemsUrlFormat="~/admin/imageGalleryItems.aspx?Id={0}" />
</asp:Content>

