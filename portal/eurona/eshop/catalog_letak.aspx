<%@ Page Title="<%$ Resources:Strings, Navigation_ProductsCatalog %>" Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" CodeBehind="catalog_letak.aspx.cs" Inherits="Eurona.EShop.CatalogLetak" %>
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
        <embed src="/userfiles/virtual-catalog-letak/book_online.swf" base="/userfiles/virtual-catalog-letak/"
            name="katalog" width="1024" height="700" align="middle" pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" menu="false" allowFullScreen="false" bgcolor="#ffffff" allowScriptAccess="sameDomain" quality="high" wmode="transparent"/>
        </div>
    </div>	
    <div class="virtual-catalog" runat="server" id="vc_pl" visible="false">
        <div style="margin:auto; width:100%;">
        <embed src="/userfiles/virtual-catalog-letak-pl/book_online.swf" base="/userfiles/virtual-catalog-letak-pl/"
            name="katalog" width="1024" height="700" align="middle" pluginspage="http://www.macromedia.com/go/getflashplayer" type="application/x-shockwave-flash" menu="false" allowFullScreen="false" bgcolor="#ffffff" allowScriptAccess="sameDomain" quality="high" wmode="transparent"/>
        </div>
    </div>
</asp:Content>
