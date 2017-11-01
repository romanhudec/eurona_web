<%@ Page Title="" Language="C#" MasterPageFile="~/eshop/admin/admin.master" AutoEventWireup="true" CodeBehind="product.aspx.cs" Inherits="Eurona.EShop.Admin.Product" %>

<%@ Register Assembly="Eurona" Namespace="Eurona.Controls.Product" TagPrefix="shpProduct" %>

<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<a href="~/eshop/admin/default.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:EShopStrings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<a href="~/eshop/admin/products.aspx" runat="server"><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Navigation_Products %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:EShopStrings, Navigation_Product %>" />		
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
<%--   <style type="text/css">
        option.article{line-height:25px; white-space:normal;}
    </style>--%>
    <shpProduct:AdminProductControl runat="server" ID="adminProduct" CssImageGalery="imageGallery" CssTreeView="treeView" MaxImagesToUpload="10" UrlAliasPrefixId="1001" DisplayUrlFormat="~/eshop/product.aspx?id={0}" CommentsFormatUrl="~/eshop/productComments.aspx?id={0}"/>
</asp:Content>