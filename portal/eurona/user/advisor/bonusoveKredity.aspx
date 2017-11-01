<%@ Page Title="" Language="C#" MasterPageFile="~/user/advisor/pageBK.master" AutoEventWireup="true" CodeBehind="bonusoveKredity.aspx.cs" Inherits="Eurona.user.advisor.bonusoveKredity" %>
<%@ Register src="../../Controls/BonusoveKredityUzivateleInfoControl.ascx" tagname="BonusoveKredityUzivateleInfoControl" tagprefix="uc1" %>
<%@ Register src="../../Controls/BKProductsControl.ascx" tagname="BKProductsControl" tagprefix="uc2" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls.Page" TagPrefix="cmsPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .content_header{height:0px;}
        .content_content{border:0px none #fff!important;margin:10px 0px 10px 10px; background-image:url(../../images/roundPanelEx/bg1-1px.png);}
        .tr_content{background-image:url(../../images/BK_background.jpg);}
        .advisor-pagemenu{border:0px none #fff!important;margin:10px 10px 10px 0px;background-image:url(../../images/roundPanelEx/bg1-1px.png);}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="banner" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content_header" runat="server">
    <div class="content-header-container">
	    <cmsPage:PageControl ID="genericPage" IsEditing="false" runat="server" CssEditorToolBar="contentEditorToolbar" CssEditorContent="contentEditorContent" NewUrl="~/admin/page.aspx" 
	        ManageUrl="~/admin/pages.aspx" NotFoundUrlFormat="~/notFound.aspx?page={0}" PageName="advisor-bk-banner-content" PopUpEditorUrlFormat="~/admin/contentEditor.aspx?id={0}" />
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <div class="content-overlay">
        <%--<div style="margin-bottom:15px;"><span style="color:#23408E; font-size:30px; font-weight:bold;" >Bonusové kredity (BK)</span></div>--%>
      <%--  <div runat="server" id="divMaxPocetBKDosazen" style="margin-bottom:5px;"><span style="color:#FF4023; font-size:18px;" >Gratulujeme! Právě jste dosáhl/a maximální počet BK pro tento měsíc</span></div>
        <div style="margin-bottom:5px;"><span style="color:#23408E; font-size:16px;" >Připravili jsme pro Vás tuto novou příležitost, jak získat skvělé dárky. <b style="color:#23408E; font-size:16px;font-weight:bold;">SBÍREJTE</b> bonusové kredity za čas strávený u Vašeho počítače s <b style="color:#23408E; font-size:16px;font-weight:bold;">EURONOU</b> a <b style="color:#23408E; font-size:16px;font-weight:bold;">ZÍSKEJTE</b> vybrané výrobky ZDARMA!</span></div>--%>
        <uc1:BonusoveKredityUzivateleInfoControl ID="bonusoveKredityUzivateleInfoControl" runat="server" width="100%" />
        <br />
        <div runat="server" id="divOrderErrorMessage">
            <span style="color:Red;font-size:16px;">
                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, BKErrorMessage %>" />
            </span>
        </div>
        <div runat="server" id="divKreditErrorMessage">
            <span style="color:Red;font-size:16px;">
            <%--Na nákup za bonusové kredity nemáte dostatečný kredit!--%>
                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, BKErrorMessage %>" />
            </span>
        </div>
        <br />
        <div>
            <div style="margin-bottom:5px;"><span style="color:#23408E; font-size:16px; font-weight:bold;" >SBÍREJTE BK a ZÍSKEJTE dárky z naší aktuální nabídky!</span></div>
        </div>
        <div runat="server" id="divCart">
            <uc2:BKProductsControl ID="bkProductsControl" runat="server" />
        </div>
        <div><span style="color:#23408E; font-size:10px;" >EURONA si vyhrazuje právo podmínky pro bonusové kredity a odměny za bonusové kredity upravovat.</span></div>
    </div>
</asp:Content>
