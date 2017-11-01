<%@ Page Title="<%$ Resources:Strings, Navigation_ImageGallery %>" Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" Inherits="Eurona.ImageGalleryPage" Codebehind="imageGallery.aspx.cs" %>

<%@ Register assembly="cms" namespace="CMS.Controls.ImageGallery" tagprefix="cmsImageGallery" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<a id="returnUrl" href="~/imageGalleries.aspx" runat="server"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_ImageGalleries %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_ImageGallery %>" />
	</div> 
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">
	<cmsImageGallery:ImageGalleryItemsControl ID="imageGalleryItemsControl" CssClass="imageGalleryItemsControl" CssRating="rating" CommentsFormatUrl="~/imageGalleryItemComments.aspx?id={0}" runat="server">
	<%--<ImageGalleryItemTemplate>
	    <div>
			<%# (Container.DataItem as CMS.Entities.ImageGalleryItem).Description %>
	    </div>
	</ImageGalleryItemTemplate>--%>
	</cmsImageGallery:ImageGalleryItemsControl>
</asp:Content>

