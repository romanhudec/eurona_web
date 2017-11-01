<%@ Page Title="" Language="C#" MasterPageFile="~/eshop/admin/admin.master" AutoEventWireup="true" CodeBehind="orders.aspx.cs" Inherits="Eurona.EShop.Admin.OrdersPage" %>

<%@ Register Assembly="eurona" Namespace="Eurona.Controls.Order" TagPrefix="shpOrder" %>

<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<a id="A2" href="~/eshop/admin/default.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:EShopStrings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:EShopStrings, Navigation_Orders %>" />
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
     <shpOrder:AdminOrdersControl runat="server" ID="adminOrdersControl" CssClass="dataGrid" UserUrlFormat="~/user/detail.aspx?id={0}"  EditUrlFormat="~/eshop/admin/order.aspx?id={0}"/>
</asp:Content>
