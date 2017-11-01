<%@ Page Title="" Language="C#" MasterPageFile="~/user/operator/admin.master" AutoEventWireup="true" CodeBehind="orders.aspx.cs" Inherits="Eurona.Operator.OrdersPage" %>

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
	 <h2>Objednávky</h2>
     <table width="100%" border="0">
         <tr>
             <td>
                 <asp:CheckBox ID="cbSelecUnselectAll" runat="server" Text="Označit/Odznačit všechny" OnCheckedChanged="OnSelectUnselectAll" AutoPostBack="true" />
             </td>
         </tr>
     </table>
     <shpOrder:AdminOrdersControl runat="server" ID="adminOrdersControl" CssClass="dataGrid" NotOrderStatusCode="-3" OnlyLastMonths="2" EditUrlFormat="~/user/operator/order.aspx?id={0}"/>
    <asp:Button runat="server" ID="btnDeleteSelectedOrders" Text="Smazat vybrané objednávky" OnClick="btnDeleteSelectedOrders_Click" OnClientClick="return confirm('Opravdu si přejete vymazat označené objednávky?');" />
</asp:Content>
