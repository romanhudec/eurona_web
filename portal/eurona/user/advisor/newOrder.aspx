<%@ Page Title="<%$ Resources:EShopStrings, Navigation_Order %>" Language="C#" MasterPageFile="~/user/advisor/page.Master" AutoEventWireup="true" CodeBehind="newOrder.aspx.cs" Inherits="Eurona.EShop.User.NewOrderPage" %>

<%@ Register Assembly="eurona" Namespace="Eurona.Controls" TagPrefix="shpOrder" %>
<%@ Register src="../../eshop/PayOrderControl.ascx" tagname="PayOrderControl" tagprefix="uc1" %>

<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
    <style>
        ._roundPanel_main{width:100%;}
        .adminOrderControl_orderNumber{font-size:24px;}
        .payOrderControl {width:100%;}
    </style>
   
	<div class="navigation-links">
		<asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" />
		<span>&nbsp;-&nbsp;</span>	
		<a id="A3" href="~/user/advisor/orders.aspx" runat="server"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:EShopStrings, Navigation_Orders %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:EShopStrings, Navigation_Order %>" />
	</div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content_header" runat="server">
    <div class="content-header-container">
        <asp:Label CssClass="title" ID="Literal1" runat="server" Text="<%$ Resources:EShopStrings, Navigation_Order %>" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
     <shpOrder:RepayOrderControl Width="700px" runat="server" ID="repayOrderControl" IsEditing="false" CssClass="adminOrderControl" CssGridView="dataGrid" FinishUrlFormat="~/user/advisor/orderFinishPayments.aspx?id={0}" />
    <div style="text-align: center;">
        <br /><br />
        <uc1:PayOrderControl ID="payOrderControl" runat="server" />
    </div>
</asp:Content>
