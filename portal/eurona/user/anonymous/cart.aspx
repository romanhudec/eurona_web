<%@ Page Title="" Language="C#" MasterPageFile="~/user/anonymous/page.master" AutoEventWireup="true" CodeBehind="cart.aspx.cs" Inherits="Eurona.User.Anonymous.CartPage" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls.Page" TagPrefix="cmsPage" %>
<%@ Register Assembly="Eurona" Namespace="Eurona.Controls" TagPrefix="ctrls" %>

<%@ Register src="CartControl.ascx" tagname="CartControl" tagprefix="uc1" %>
<%@ Register Assembly="cms" Namespace="CMS.Utilities" TagPrefix="cmsUtilities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #rn_cartBefore{background-image:url(../../images/anonymous-register-navigation-item-selected.png);}
        #rn_cartBefore a{color:#36AFE2;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content_header" runat="server">
	<asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_MujKosikPredPrepoctem %>"></asp:Literal>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <span style="color:#e2008b;">
    </span>
    <div>
        <uc1:CartControl ID="cartControl" runat="server" CssClass="dataGrid" />
		<div style="margin-top:10px; margin-bottom:20px;">
			<span style="padding:5px;color:#00AF00; font-size:16px; font-weight:bold;margin-bottom:20px;"><asp:Literal runat="server" ID="lblPostovneInfo"></asp:Literal></span>
		</div>
        <div style="margin-top:10px;">
            <h2 style="color:#e2008b;"><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_VyberDopravy %>"></asp:Literal></h2>
            <asp:RadioButtonList runat="server" ID="rblPreprava"> </asp:RadioButtonList>
        </div>
        <div>
            <cmsPage:PageControl ID="PageControl2" IsEditing="true" runat="server" CssEditorToolBar="contentEditorToolbar" CssEditorContent="contentEditorContent" NewUrl="" 
	        ManageUrl="" NotFoundUrlFormat="~/notFound.aspx?page={0}" PageName="anonymous-cart-before-banner1-content" PopUpEditorUrlFormat="~/admin/contentEditor.aspx?id={0}" />
        </div>
        <div style="margin-top:10px;margin-bottom:10px;">
            <span>
                <a class="add-button-150" href='<%=aliasUtilities.Resolve("~/eshop/default.aspx") %>'><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_PridatDalsiVyrobky %>"></asp:Literal></a>
                <asp:Button runat="server" id="btnContinue" class="button" Text="<%$ Resources:Strings, ContinueButton_Text %>" OnClick="OnContinue"></asp:Button>
            </span>
        </div>
        <div>
            <cmsPage:PageControl ID="PageControl1" IsEditing="true" runat="server" CssEditorToolBar="contentEditorToolbar" CssEditorContent="contentEditorContent" NewUrl="" 
	        ManageUrl="" NotFoundUrlFormat="~/notFound.aspx?page={0}" PageName="anonymous-cart-before-banner2-content" PopUpEditorUrlFormat="~/admin/contentEditor.aspx?id={0}" />
        </div>
    </div>

    <%--UTILITIES--%>
    <cmsUtilities:AliasUtilities ID="aliasUtilities" runat="server" ></cmsUtilities:AliasUtilities>  
</asp:Content>
