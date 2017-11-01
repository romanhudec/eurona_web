<%@ Page Title="" Language="C#" MasterPageFile="~/user/anonymous/page.master" AutoEventWireup="true" CodeBehind="finish.aspx.cs" Inherits="Eurona.User.Anonymous.FinishPage" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls.Page" TagPrefix="cmsPage" %>
<%@ Register Assembly="Eurona" Namespace="Eurona.Controls" TagPrefix="ctrls" %>

<%@ Register Assembly="eurona" Namespace="Eurona.Controls" TagPrefix="shpOrder" %>
<%@ Register Assembly="cms" Namespace="CMS.Utilities" TagPrefix="cmsUtilities" %>

<%@ Register src="OrderControl.ascx" tagname="OrderControl" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    <style type="text/css">
        #rn_order{background-image:url(../../images/anonymous-register-navigation-item-last-selected.png);}
        #rn_order a{color:#36AFE2;}        
        .content_content{height:100%;min-height:400px;};
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content_header" runat="server">
	<asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_PotvrzeniObjednavky %>"></asp:Literal>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <span style="color:#e2008b;">
    </span>
    <div style="margin:20px;">
        <div style="margin-bottom:10px;">
            <cmsPage:PageControl ID="PageControl2" IsEditing="true" runat="server" CssEditorToolBar="contentEditorToolbar" CssEditorContent="contentEditorContent" NewUrl="" 
	        ManageUrl="" NotFoundUrlFormat="~/notFound.aspx?page={0}" PageName="anonymous-order-finish-banner1-content" PopUpEditorUrlFormat="~/admin/contentEditor.aspx?id={0}" />
        </div>
        <h2  style="color:#e2008b;"><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_OrderFinish_Dekujeme %>"></asp:Literal></h2>
        <asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_OrderFinish_VasiObjednavkuSiMuzeteProhlednout %>"></asp:Literal>&nbsp;<a runat="server" id="linkOrder"><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_OrderFinish_Zde %>"></asp:Literal></a>.<br />
        <asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_OrderFinish_VNejblizsichDnechVasKontaktujeme %>"></asp:Literal><br />
        <br />
        <div runat="server" id="divPoradce">
            <b>
            <asp:Label runat="server" ID="lblName"></asp:Label>, <asp:Label runat="server" ID="lblEmail"></asp:Label>, <asp:Label runat="server" ID="lblMobil"></asp:Label>
            </b>
            <br />
        </div>
        <br /><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_OrderFinish_VPripadeDalsichDotazu %>"></asp:Literal><br />
        <br />
        <span style="color:#e2008b;"><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_OrderFinish_OperatorkaCentrum %>"></asp:Literal></span>
        <br />
        <%if( locale == "sk" ) {%>
        - PO-PIA 8:00 – 16:00 hod<br />
        - e-mail: prodej@eurona.cz<br />
        <br /><br />
        - telefon CZ: +420 491 477 361<br />
        <%} else if(locale == "pl" ){ %>
        - PO- PIĄ 8:00 – 16:00 godzin<br />
        - e-mail: prodej@eurona.cz<br />
        <br /><br />
        - telefon CZ: +420 491 477 361<br />
        <%} else{ %>
        - PO-PÁ 8:00 - 16:00 hod<br />
        - e-mail: prodej@eurona.cz<br />
        <br /><br />
        - telefon CZ: +420 491 477 361<br />
        <%} %>
        <div style="margin-top:10px;">
            <cmsPage:PageControl ID="PageControl1" IsEditing="true" runat="server" CssEditorToolBar="contentEditorToolbar" CssEditorContent="contentEditorContent" NewUrl="" 
	        ManageUrl="" NotFoundUrlFormat="~/notFound.aspx?page={0}" PageName="anonymous-order-finish-banner2-content" PopUpEditorUrlFormat="~/admin/contentEditor.aspx?id={0}" />
        </div>
    </div>
    <%--UTILITIES--%>
    <cmsUtilities:AliasUtilities ID="aliasUtilities" runat="server" ></cmsUtilities:AliasUtilities>  

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
