<%@ Page Title="<%$ Resources:EShopStrings, Navigation_Order %>" EnableEventValidation="false" Language="C#" MasterPageFile="~/user/advisor/page.Master" AutoEventWireup="true" CodeBehind="order.aspx.cs" Inherits="Eurona.EShop.User.OrderPage" %>

<%@ Register Assembly="eurona" Namespace="Eurona.Controls" TagPrefix="shpOrder" %>

<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
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
 <script type="text/javascript">
     setTimeout("setFocusToCartCodeEdit()", 2000);
     function setFocusToCartCodeEdit() {
         if (document.getElementById)
             document.getElementById("ctl00_content_adminOrderControl_txtKod").focus();
            else if (document.all)
                document.all("ctl00_content_adminOrderControl_txtKod").focus();
          return false;
      }
    </script>
    <%--<script type="text/javascript">
        $(function () {
            $('.has-tooltip').tooltip();
        });
    </script>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
     <shpOrder:AdminOrderControl runat="server" ID="adminOrderControl" IsEditing="true" CssClass="adminOrderControl" CssGridView="dataGrid" FinishUrlFormat="~/user/advisor/orderFinishPayments.aspx?id={0}" />
</asp:Content>
