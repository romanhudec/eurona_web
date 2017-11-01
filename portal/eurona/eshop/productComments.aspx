<%@ Page Title="<%$ Resources:EShopStrings, Navigation_ProductComments %>" Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" Inherits="Eurona.EShop.ProductCommentsPage" Codebehind="productComments.aspx.cs" %>

<%@ Register Assembly="eurona.common" Namespace="Eurona.Common.Controls.Product" TagPrefix="commoProduct" %>
<%@ Register Assembly="shp" Namespace="SHP.Controls.Product" TagPrefix="shpProduct" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<a id="returnUrl"  href="~/default.aspx" runat="server"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:EShopStrings, Navigation_Product %>" /></a>
        <span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:EShopStrings, Navigation_ProductComments %>" />		
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">
    <div class="comments">
    <commoProduct:ProductCommentsControl ID="productCommentsControl" CssCarma="carma" DisplayUrlFormat="~/product.aspx?id={0}" CssClass="comments" runat="server" >
      <%--  <ArticleTemplate>
            <div CssClass="article"><%#Container.DataItem.Title%></div>
        </ArticleTemplate>--%>
    </commoProduct:ProductCommentsControl>
    <shpProduct:ProductCommentFormControl ID="productCommentFormControl" CssClass="commentForm" HiddenFieldParenId="hfParentId"  runat="server" />
    </div>
</asp:Content>

