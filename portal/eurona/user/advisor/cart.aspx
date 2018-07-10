<%@ Page Title="<%$ Resources:EShopStrings, CartControl_Cart %>" Language="C#" MasterPageFile="~/user/advisor/page.master" AutoEventWireup="true" CodeBehind="cart.aspx.cs" Inherits="Eurona.User.Advisor.CartPage" %>

<%@ Register Assembly="Eurona" Namespace="Eurona.Controls" TagPrefix="ctrls" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        setTimeout("setFocusToCartCodeEdit()", 1200);
        function setFocusToCartCodeEdit() {
            // W3C approved DOM code that will work in all modern browsers      
            if (document.getElementById)
                document.getElementById("<%= txtKod.ClientID %>").focus();
                  // To support older versions of IE:
              else if (document.all)
                  document.all("<%= txtKod.ClientID %>").focus();
            return false;
        }        
    </script>
    <style type="text/css">
        .minus{position:absolute;margin:0px -23px 0px 0px;display:none;background-image:url(../../images/rotator-minus.png);width:45px;height:45px;cursor:pointer;z-index:3;}
        .plus{position:absolute;margin:0px -23px 0px 0px;display:block;background-image:url(../../images/rotator-plus.png);width:45px;height:45px;cursor:pointer;z-index:3;}
        .orderButton{font-size:14px; background-color:transparent!important; border:0px none #fff!important; display:block; width:210px; height:75px!important; background-image:url(../../images/zelene-tlacitko.png);}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content_header" runat="server">
    <div class="content-header-container">
        <asp:Label CssClass="title" ID="Literal1" runat="server" Text="<%$ Resources:EShopStrings, CartControl_Cart %>" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content_sub_header" runat="server" Visible="true">
    <div style="padding:10px;" runat="server" id="divAutomaticEmptyCartMessage" visible="false">
        <asp:Label runat="server" ID="lblAutomaticEmptyCartMessage" ForeColor="#eb0a5b" Font-Size="12"></asp:Label>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <table width="100%" style="margin-bottom:-30px;">
        <tr>
            <td style="width:100%;" align="right"><a runat="server" href="~/user/advisor/bonusoveKredity.aspx?ReturnUrl=~/user/advisor/cart.aspx" style="text-decoration:none;">Zde si můžete vybrat prémie za získané bonusové kredity</a></td>
            <td><a runat="server" href="~/user/advisor/bonusoveKredity.aspx?ReturnUrl==~/user/advisor/cart.aspx" style="text-decoration:none;"><img  style="border:0px none #fff;"" width="50px" src="../../images/BK_ikona.jpg" /></a></td>
        </tr>
    </table>
    <table onload="setTimeout('setFocus()', 1200)">
        <tr>
            <td><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, CartControl_ProductCode %>"></asp:Literal></td>
            <td><asp:TextBox ID="txtKod" runat="server" Width="50px"></asp:TextBox></td>
            <td><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, CartControl_ProductQuantity %>"></asp:Literal></td>
            <td><asp:TextBox ID="txtMnozstvi" runat="server" Width="30px"></asp:TextBox></td>
            <td><asp:Button ID="btnAdd" runat="server" Text="<%$ Resources:EShopStrings, CartControl_Add %>" OnClick="OnAddCart"></asp:Button></td>
        </tr>
    </table>
    <ctrls:CartControl runat="server" ID="cartControl" CssClass="dataGrid" FinishUrlFormat="~/user/advisor/order.aspx?id={0}&ReturnUrl=/user/advisor/orders.aspx?type=ac" />
	<div style="margin-top:10px; margin-bottom:20px;">
		<span style="padding:5px;color:#00AF00; font-size:16px; font-weight:bold;margin-bottom:20px;"><asp:Literal runat="server" ID="lblPostovneInfo"></asp:Literal></span>
	</div>
</asp:Content>
