<%@ Page Title="<%$ Resources:EShopStrings, Navigation_Orders %>" Language="C#" MasterPageFile="~/user/host/page.Master" AutoEventWireup="true" CodeBehind="orders.aspx.cs" Inherits="Eurona.User.Host.OrdersPage" %>

<%@ Register Assembly="shp" Namespace="SHP.Controls.Order" TagPrefix="shpOrder" %>

<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
	<div class="navigation-links">
		<asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" />
		<span>&nbsp;-&nbsp;</span>		
		<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:EShopStrings, Navigation_Orders %>" />
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
     <link href='<%=ResolveUrl("~/styles/eshop.css") %>' type="text/css" rel="stylesheet" />
     <shpOrder:AdminOrdersControl runat="server" ID="adminOrdersControl" CssClass="dataGrid" EditUrlFormat="~/user/host/order.aspx?id={0}"/>
</asp:Content>
