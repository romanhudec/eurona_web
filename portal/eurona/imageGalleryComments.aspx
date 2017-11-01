<%@ Page Title="<%$ Resources:Strings, Navigation_ImageGalleryComments %>" Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" Inherits="Eurona.ImageGalleryCommentsPage" Codebehind="imageGalleryComments.aspx.cs" %>

<%@ Register assembly="cms" namespace="CMS.Controls.ImageGallery" tagprefix="cmsImageGallery" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<a id="returnUrl" href="~/imageGalleries.aspx" runat="server"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_ImageGalleries %>" /></a>
        <span>&nbsp;-&nbsp;</span>		
		<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_ImageGalleryComments %>" />		
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">
    <div class="comments">
    <cmsImageGallery:ImageGalleryCommentsControl ID="imageGalleryCommentsControl" AccountProfileItemName="Nick Name" CssCarma="carma" DisplayUrlFormat="~/imageGalleries.aspx?id={0}" CssClass="comments" runat="server" >
      <%--  <ImageGalleryTemplate>
            <div CssClass="blog"><%#Container.DataItem.Title%></div>
        </ImageGalleryTemplate>--%>
    </cmsImageGallery:ImageGalleryCommentsControl>
    <cmsImageGallery:ImageGalleryCommentFormControl ID="imageGalleryCommentFormControl" CssClass="commentForm" HiddenFieldParenId="hfParentId"  runat="server" />
    </div>
</asp:Content>

