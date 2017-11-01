<%@ Page Language="C#" MasterPageFile="~/eshop/default.master" AutoEventWireup="true" CodeBehind="category.aspx.cs" Inherits="Eurona.EShop.CategoryPage" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>
<%@ Register Assembly="shp" Namespace="SHP.Controls.Category" TagPrefix="shpCategory" %>

<%@ Register src="ProductsControl.ascx" tagname="ProductsControl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        /* <![CDATA[ */
        var seznam_retargeting_id = 38225;
        /* ]]> */
    </script>
    <script type="text/javascript" src="//c.imedia.cz/js/retargeting.js"></script>
    <shpCategory:CategoryControl runat="server" ID="categoryControl" />    
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <uc1:ProductsControl ID="productsControl" runat="server" OnOnLoadProducts="OnLoadProducts"/>
</asp:Content>
