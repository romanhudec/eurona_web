﻿<%@ Page Title="<%$ Resources:EShopStrings, OrderControl_Order %>" Language="C#" MasterPageFile="~/user/anonymous/page.master" AutoEventWireup="true" CodeBehind="orderFinishPayments.aspx.cs" Inherits="Eurona.User.Anonymous.OrderFinishPaymentPage" %>

<%@ Register src="PayOrderControl.ascx" tagname="PayOrderControl" tagprefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #rn_order{background-image:url(../../images/anonymous-register-navigation-item-last-selected.png);}
        #rn_order a{color:#36AFE2;}        
        .content_content{height:100%;min-height:400px;}
    </style>
    <script language="javascript" type="text/javascript">
        ga('require', 'ecommerce');         // Aktivuje ecommerce plugin.
        ga('ecommerce:addTransaction', {    // Údaje o objednávce
            'id': '<%=order.OrderNumber%>'  // Číslo objednávky. Povinné.
            'affiliation': 'EuronabyCerny'  // Název obchodu.
        'revenue': '<%=order.PriceWVAT%>'// Celková částka za objednávku.
            'shipping': '<%=order.CartEntity.DopravneEurosap%>'// Doprava.
        'tax': ''// Daň.
        });
        <%foreach(Eurona.Common.DAL.Entities.CartProduct cp in order.CartEntity.CartProducts){%>
        ga('ecommerce:addItem', {         // údaje o produktu
            'id': '<%=order.OrderNumber%>'// Číslo objednávky. Povinné.
            'name':'<%=cp.ProductName%>'  // Název produktu. Povinné.
        'sku': '<%=cp.ProductCode%>'  // kód produktu.
        'category': ''        // Kategorie.
        'price': '<%=cp.PriceWVAT%>'  // Cena za jednotku.
            'quantity': '<%=cp.Quantity%>'// Množství.
        });
        <%}%>
        ga('ecommerce:send');               // Odešle údaje do Google Analytics
     </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="content_header" runat="server">
    <asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:EShopStrings, Anonymous_UhradaObjednavky %>"></asp:Literal>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <table border="0" width="100%">
        <tr>
            <td valign="middle">
                <table width="400px;">
		            <tr>
			            <td align="center" colspan="2">
				            <uc1:PayOrderControl ID="payOrderControl" runat="server" />
			            </td>
		            </tr>   
		            <tr>
			            <td align="center" colspan="2">
				            <table border="0">
                                <tr>
                                    <td align="center" colspan="4" style="color:#868686"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:EShopStrings, OrderFinish_AkceptujemeKarty %> "></asp:Literal></td>
                                </tr>
					            <tr>
						            <td align="center" style="width:80px;"><a runat="server" id="A1"><img runat="server" ID="Img1" border="0" src="~/images/pay/visa.png" width="71" height="42" /></a></td>
						            <td align="center" style="width:80px;"><a runat="server" id="A2"><img runat="server" ID="Img2" border="0" src="~/images/pay/visaelektron.png" width="71" height="42" /></a></td>
						            <td align="center" style="width:80px;"><a runat="server" id="A3"><img runat="server" ID="Img3" border="0" src="~/images/pay/mastercard.png" width="71" height="42" /></a></td>
						            <td align="center" style="width:80px;"><a runat="server" id="A4"><img runat="server" ID="Img4" border="0" src="~/images/pay/maestrol.png " width="71" height="42" /></a></td>
					            </tr>
				            </table>
			            </td>
		            </tr>  
                </table>
            </td>  
            <td align="center" valign="middle">
                <div class="order-finish-helpdesk">
                <table >
                    <tr>
                        <td align="center"><span style="font-weight:bold; font-size:16px;"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:EShopStrings, OrderFinish_PomocSNakupem %> " /></span></td>
                    </tr>
                    <tr>
                        <td align="center"><span style="color:#00b8ec;font-style:italic;"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:EShopStrings, OrderFinish_NeviteSiRady %> " /></span></td>
                    </tr>
                    <tr><td>&nbsp;</td></tr>
                    <tr>
                        <td align="center"><span  style="font-weight:bold; font-size:14px;"><asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:EShopStrings, OrderFinish_Volejte %> " /></span></td>
                    </tr>
                    <tr>
                        <td align="center"><span style="color:#00b8ec;font-size:16px;">+420 491 451 516</span></td>
                    </tr>
                    <tr><td>&nbsp;</td></tr>
                    <tr>
                        <td align="center"><span  style="font-weight:bold; font-size:14px;"><asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:EShopStrings, OrderFinish_PtejteSeEmailem %> " /></span></td>
                    </tr>
                    <tr>
                        <td align="center"><span style="color:#00b8ec;font-size:16px;">prodej@eurona.cz</span></td>
                    </tr>
                </table>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
