<%@ Page Title="<%$ Resources:EShopStrings, Navigation_Order %>" Language="C#" MasterPageFile="~/user/host/page.Master" AutoEventWireup="true" CodeBehind="order.aspx.cs" Inherits=" Eurona.User.Host.OrderPage" %>

<%@ Register Assembly="shp" Namespace="SHP.Controls.Order" TagPrefix="shpOrder" %>

<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
	<div class="navigation-links">
		<asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" />
		<span>&nbsp;-&nbsp;</span>	
		<a id="A3" href="~/eshop/user/orders.aspx" runat="server"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:EShopStrings, Navigation_Orders %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:EShopStrings, Navigation_Order %>" />
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
     <link href='<%=ResolveUrl("~/styles/eshop.css") %>' type="text/css" rel="stylesheet" />
     <shpOrder:AdminOrderControl runat="server" ID="adminOrderControl" IsEditing="true" CssClass="adminOrderControl" CssGridView="dataGrid" />
</asp:Content>
