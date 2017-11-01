<%@ Page Title="<%$ Resources:Strings, Navigation_ImageGalleries %>" Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" Inherits="Eurona.ImageGalleriesPage" Codebehind="imageGalleries.aspx.cs" %>

<%@ Register assembly="cms" namespace="CMS.Controls.ImageGallery" tagprefix="cmsImageGallery" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_ImageGalleries %>" />
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">
	<cmsImageGallery:ImageGalleriesControl ID="imageGalleriesControl" 
	CssClass="imageGalleriesControl" CssEditorToolBar="contentEditorToolbar" CssEditorContent="contentEditorContent" 
	CommentsFormatUrl="~/imageGalleryComments.aspx?id={0}" 
	DisplayUrlFormat="~/imageGallery.aspx?id={0}" 
	NewUrl="~/admin/imageGallery.aspx" 
	ManageUrl="~/admin/imageGalleries.aspx" runat="server">
	<%--<ImageGalleryLayoutTemplate>
	    <table>
			<%# (Container.DataItem as CMS.Entities.ImageGallery).Name %>
	    </table>
	</ImageGalleryLayoutTemplate>
	<ImageGalleryGroupTemplate>
	    <tr>
	    </tr>
	</ImageGalleryGroupTemplate>
	<ImageGalleryItemTemplate>
	    <td>
	        <%# (Container.DataItem as CMS.Entities.ImageGallery).ImageUrl %>
	    </td>
	</ImageGalleryItemTemplate>--%>	
	</cmsImageGallery:ImageGalleriesControl>
</asp:Content>

