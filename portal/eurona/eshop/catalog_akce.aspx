<%@ Page Title="<%$ Resources:Strings, Navigation_ProductsCatalog %>" Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" CodeBehind="catalog_akce.aspx.cs" Inherits="Eurona.EShop.CatalogAkce" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="banner" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="navigation" runat="server">
	<div class="navigation-links">
		<a id="aHome" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal id="aThisPage" runat="server" />	
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" runat="server">
    <div class="virtual-catalog" runat="server" id="vc_cs" visible="false">
        <div style="margin:auto; width:100%;">
        <embed src="/userfiles/virtual-catalog-akce/book_online.swf" base="/userfiles/virtual-catalog-akce/"
            name="katalog" width="1024" height="700" align="middle" pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" menu="false" allowFullScreen="false" bgcolor="#ffffff" allowScriptAccess="sameDomain" quality="high" wmode="transparent"/>
        </div>
    </div>	
    <div class="virtual-catalog" runat="server" id="vc_pl" visible="false">
        <div style="margin:auto; width:100%;">
        <embed src="/userfiles/virtual-catalog-akce-pl/book_online.swf" base="/userfiles/virtual-catalog-akce-pl/"
            name="katalog" width="1024" height="700" align="middle" pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" menu="false" allowFullScreen="false" bgcolor="#ffffff" allowScriptAccess="sameDomain" quality="high" wmode="transparent"/>
        </div>
    </div>
</asp:Content>
