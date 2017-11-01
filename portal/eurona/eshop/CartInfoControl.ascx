<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CartInfoControl.ascx.cs" Inherits="Eurona.Eshop.CartInfoControl" %>
<%@ Register Assembly="Eurona" Namespace="Eurona.Controls" TagPrefix="euronaCtrls" %>
<div class="cartInfo">
	<table border="0" cellpadding="0" cellspacing="0" width="100%">
		<tr>
			<td>
				<div class="title"><asp:HyperLink runat="server" ID="hlCart" Text="<%$ Resources:EShopStrings, CartControl_Cart %>"></asp:HyperLink></div>
				<div>
					<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:EShopStrings, CartControl_ItemsCount %>"></asp:Literal>
					<asp:Label CssClass="items" ID="txtCount" runat="server" Text=""></asp:Label>
				</div>
				<div>
					<asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:EShopStrings, CartControl_Price %>"></asp:Literal>
					<asp:Label CssClass="price" ID="txtPrice" runat="server"></asp:Label>
					
				</div>
			</td>
			<td align="center" valign="top">
				<asp:HyperLink runat="server" ID="hlImage" >
					<div class="image"></div>
				</asp:HyperLink>
				<div>
					<a runat="server" id="hlkPokladne" class="kpokladne"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:EShopStrings, CartControl_KPokladne %>"></asp:Literal></a>
				</div>
			</td>
			<td>
				<euronaCtrls:LocaleSwitchControl runat="server" ID="localeSwitchControl" CssClass="localeSwitch" />
			</td>
		</tr>
	</table>
</div>