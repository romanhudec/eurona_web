<%@ Page Title="<%$ Resources:EShopStrings, Navigation_Orders %>" Language="C#" MasterPageFile="~/user/advisor/page.Master" AutoEventWireup="true" CodeBehind="orderstoassociate.aspx.cs" Inherits="Eurona.User.Advisor.OrdersToAssociatePage" %>

<%@ Register Assembly="eurona" Namespace="Eurona.Controls.Order" TagPrefix="shpOrder" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
	<div class="navigation-links">
		<asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" />
		<span>&nbsp;-&nbsp;</span>		
		<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:EShopStrings, Navigation_OrdersToAssociate %>" />
	</div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content_header" runat="server">
    <div class="content-header-container">
        <asp:Label CssClass="title" ID="lblTitle" runat="server" Text="<%$ Resources:EShopStrings, Navigation_OrdersToAssociate %>" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
     <shpOrder:OrdersToAssociateControl runat="server" ID="ordersToAssociateControl" CssClass="dataGrid" EditUrlFormat="~/user/advisor/order.aspx?id={0}"/>
</asp:Content>
