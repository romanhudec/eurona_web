<%@ Page Title="<%$ Resources:Strings, Navigation_SearchResult %>" Language="C#" MasterPageFile="~/eshop/default.master" AutoEventWireup="true" Inherits="Eurona.EShop.SearchResultPage" Codebehind="search.aspx.cs" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>
<%@ Register Assembly="shp" Namespace="SHP.Controls.Category" TagPrefix="shpCategory" %>

<%@ Register src="ProductsControl.ascx" tagname="ProductsControl" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <shpCategory:CategoryControl runat="server" ID="categoryControl" />    
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <uc1:ProductsControl ID="productsControl" runat="server"/>
</asp:Content>

