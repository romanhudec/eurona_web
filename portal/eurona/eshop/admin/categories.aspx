<%@ Page Title="" Language="C#" MasterPageFile="~/eshop/admin/admin.master" AutoEventWireup="true" CodeBehind="categories.aspx.cs" Inherits="Eurona.EShop.Admin.Categories" %>

<%@ Register Assembly="eurona.common" Namespace="Eurona.Common.Controls.Category" TagPrefix="eshopCategory" %>

<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<a id="A2" href="~/eshop/admin/default.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:EShopStrings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:EShopStrings, Navigation_Categories %>" />
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <eshopCategory:AdminCategoriesControl runat="server" ID="adminCategories" CssClass="dataGrid" NewUrl="~/eshop/admin/category.aspx?" EditUrlFormat="~/eshop/admin/category.aspx?id={0}" CategoriesUrlFormat="~/eshop/admin/categories.aspx?id={0}" AttributesUrlFormat="~/eshop/admin/attributes.aspx?id={0}" />
</asp:Content>
