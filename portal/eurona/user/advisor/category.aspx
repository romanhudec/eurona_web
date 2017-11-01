<%@ Page Language="C#" MasterPageFile="~/user/advisor/page.master" AutoEventWireup="true" CodeBehind="category.aspx.cs" Inherits="Eurona.User.Advisor.CategoryPage" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>
<%@ Register Assembly="shp" Namespace="SHP.Controls.Category" TagPrefix="shpCategory" %>

<%@ Register src="../../eshop/ProductsControl.ascx" tagname="ProductsControl" tagprefix="uc1" %>
<%@ Register src="CategoryNavigation.ascx" tagname="CategoryNavigation" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <shpCategory:CategoryControl runat="server" ID="categoryControl" />
    <style type="text/css">
        .content_header{background-image:none!important; border: 0px none #fff!important;}
        .content_content{vertical-align:middle!important; background-color:#fff;}
    </style>      
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content_header" runat="server">
    <div class="category-sitemenu-container">
        <uc1:CategoryNavigation ID="categoryNavigation" runat="server" CssClass="category_sitemenu" MenuItemSeparatorImageUrl="~/images/category-menu-item-separator.png" />
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <uc1:ProductsControl ID="productsControl" runat="server" OnOnLoadProducts="OnLoadProducts" OnOnProductAddedToChart="OnProductAddedToChart"/>
</asp:Content>
