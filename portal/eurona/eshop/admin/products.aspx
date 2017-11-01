<%@ Page Title="" Language="C#" MasterPageFile="~/eshop/admin/admin.master" AutoEventWireup="true" CodeBehind="products.aspx.cs" Inherits="Eurona.EShop.Admin.Products" %>

<%@ Register Assembly="Eurona" Namespace="Eurona.Controls.Product" TagPrefix="shpProduct" %>

<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<a id="A2" href="~/eshop/admin/default.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:EShopStrings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:EShopStrings, Navigation_Products %>" />
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <shpProduct:AdminProductsControl runat="server" ID="adminProducts" CssClass="dataGrid" NewUrl="~/eshop/admin/product.aspx?" EditUrlFormat="~/eshop/admin/product.aspx?id={0}"/>
</asp:Content>
