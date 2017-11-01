<%@ Page Title="<%$ Resources:EShopStrings, OrderControl_Order %>" Language="C#" MasterPageFile="~/user/advisor/page.master" AutoEventWireup="true" CodeBehind="orderFinish.aspx.cs" Inherits="Eurona.User.Advisor.OrderFinishPage" %>

<%@ Register src="../../eshop/PayOrderControl.ascx" tagname="PayOrderControl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="content_header" runat="server">
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
    <div class="content-header-container">
        <asp:image runat="server" ID="imgLogo" Height="32px" style="padding:20px;" ImageUrl="~/images/order_finish_header.png"></asp:image>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <table border="0" width="100%">
        <tr>
            <td valign="top">
                <table>
                    <tr>
                        <td>
                            <h1><asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:EShopStrings, OrderFinish_DekujemeZaVasiObjednavku %>"></asp:Literal></h1>
                        </td>
                    </tr>
                    <tr>
                    <td valign="top"><asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:EShopStrings, OrderFinish_Text %> "></asp:Literal>
                        <asp:Hyperlink runat="server" ID="hlOrders" Text="<%$ Resources:EShopStrings, OrderControl_MyOrders %>">
                        </asp:Hyperlink>
                     </td>
                    </tr>
                </table>                
            </td>
        </tr>
        
    </table>
    <!-- Měřicí kód Sklik.cz -->
    <iframe width="119" height="22" frameborder="0" scrolling="no" src="//c.imedia.cz/checkConversion?c=100029016&color=ffffff&v=<%=order.Price %>"></iframe>

     <!-- Google Code for Konverze Conversion Page -->
    <script type="text/javascript">
        /* <![CDATA[ */
        var google_conversion_id = 855739542;
        var google_conversion_language = "en";
        var google_conversion_format = "3";
        var google_conversion_color = "ffffff";
        var google_conversion_label = "rJQgCPzRqnAQlpmGmAM";
        var google_conversion_value = 1.00;
        var google_conversion_currency = "CZK";
        var google_remarketing_only = false;
        /* ]]> */
    </script>
    <script type="text/javascript" src="//www.googleadservices.com/pagead/conversion.js">
    </script>
    <noscript>
    <div style="display:inline;">
    <img height="1" width="1" style="border-style:none;" alt="" src="//www.googleadservices.com/pagead/conversion/855739542/?value=1.00&amp;currency_code=CZK&amp;label=rJQgCPzRqnAQlpmGmAM&amp;guid=ON&amp;script=0"/>
    </div>
    </noscript>

</asp:Content>
