<%@ Page Title="" Language="C#" MasterPageFile="~/user/anonymous/page.master" AutoEventWireup="true" CodeBehind="order.aspx.cs" Inherits="Eurona.User.Anonymous.OrderPage" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls.Page" TagPrefix="cmsPage" %>
<%@ Register Assembly="Eurona" Namespace="Eurona.Controls" TagPrefix="ctrls" %>

<%@ Register Assembly="eurona" Namespace="Eurona.Controls" TagPrefix="shpOrder" %>
<%@ Register Assembly="cms" Namespace="CMS.Utilities" TagPrefix="cmsUtilities" %>

<%@ Register src="OrderControl.ascx" tagname="OrderControl" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #rn_cartAfter{background-image:url(../../images/anonymous-register-navigation-item-selected.png);}
        #rn_cartAfter a{color:#36AFE2;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content_header" runat="server">
	<asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_MujKosikPoPrepoctu %>"></asp:Literal>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <span style="color:#e2008b;">
    </span>
    <uc1:OrderControl ID="adminOrderControl" runat="server" IsEditing="true" CssClass="adminOrderControl" CssGridView="dataGrid" />

    <div>
        <cmsPage:PageControl ID="PageControl2" IsEditing="true" runat="server" CssEditorToolBar="contentEditorToolbar" CssEditorContent="contentEditorContent" NewUrl="" 
	    ManageUrl="" NotFoundUrlFormat="~/notFound.aspx?page={0}" PageName="anonymous-cart-after-banner1-content" PopUpEditorUrlFormat="~/admin/contentEditor.aspx?id={0}" />
    </div>
    <div style="margin-top:10px;margin-bottom:10px;">
        <span>
            <a class="add-button-150" href='<%=aliasUtilities.Resolve("~/eshop/default.aspx") %>'><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_PridatDalsiVyrobky %>"></asp:Literal></a>
            <asp:Button runat="server" id="btnContinue" class="button" Text="<%$ Resources:EShopStrings, Anonymous_Objednat %>" OnClick="OnContinue" CausesValidation="true"></asp:Button>
        </span>
    </div>
    <div>
        <cmsPage:PageControl ID="PageControl1" IsEditing="true" runat="server" CssEditorToolBar="contentEditorToolbar" CssEditorContent="contentEditorContent" NewUrl="" 
	    ManageUrl="" NotFoundUrlFormat="~/notFound.aspx?page={0}" PageName="anonymous-cart-after-banner2-content" PopUpEditorUrlFormat="~/admin/contentEditor.aspx?id={0}" />
    </div>
    <%--UTILITIES--%>
    <cmsUtilities:AliasUtilities ID="aliasUtilities" runat="server" ></cmsUtilities:AliasUtilities>  
</asp:Content>
