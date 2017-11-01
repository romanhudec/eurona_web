<%@ Page Title="<%$ Resources:EShopStrings, Navigation_Order %>" Language="C#" MasterPageFile="~/user/advisor/page.Master" AutoEventWireup="true" CodeBehind="pay.aspx.cs" Inherits="Eurona.EShop.User.PayPage" %>

<%@ Register Assembly="eurona" Namespace="Eurona.Controls" TagPrefix="shpOrder" %>
<%@ Register src="../../eshop/PayOrderControl.ascx" tagname="PayOrderControl" tagprefix="uc1" %>

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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
	 <div runat="server" id="divEPay" visible="false" style=" background-image:url('../../images/roundPanelEx/bg-1px.png');">
		<table width="100%">
			<tr>
				<td><h1><asp:Literal ID="Literal5" runat="server" Text="Elektronická platba"></asp:Literal></h1></td>
			</tr>
			<tr>
				<td>
					<table border="0" style="margin-left:auto; margin-right:auto;">
						<tr>
							<td align="center" style="width:150px;"><a id="A1" runat="server"><img runat="server" ID="Img1" border="0" src="~/images/pay/visa.png" width="103" height="65" /></a></td>
							<td align="center" style="width:150px;"><a id="A2" runat="server"><img runat="server" ID="Img2" border="0" src="~/images/pay/visaelektron.png" width="103" height="65" /></a></td>
							<td align="center" style="width:150px;"><a id="A4" runat="server"><img runat="server" ID="Img3" border="0" src="~/images/pay/mastercard.png" width="103" height="65" /></a></td>
							<td align="center" style="width:150px;"><a id="A5" runat="server"><img runat="server" ID="Img4" border="0" src="~/images/pay/maestrol.png " width="103" height="65" /></a></td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td>
					<uc1:PayOrderControl ID="payOrderControl" runat="server" />
					<hr />
				</td>
			</tr>       
		</table>
	 </div>
     <shpOrder:AdminOrderControl runat="server" ID="adminOrderControl" IsEditing="true" CssClass="adminOrderControl" CssGridView="dataGrid" FinishUrlFormat="~/user/advisor/orderFinish.aspx?id={0}" />
</asp:Content>
