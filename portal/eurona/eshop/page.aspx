<%@ Page Language="C#" MasterPageFile="~/eshop/default.master" AutoEventWireup="true" Inherits="Eurona.EShop.GenericPage" Codebehind="page.aspx.cs" %>

<%@ Register assembly="cms" namespace="CMS.Controls.Page" tagprefix="cmsPage" %>
<asp:Content ID="Content5" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">
    .products-content{background-color:transparent!important;}
</style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">
	<cmsPage:PageControl ID="genericPage" IsEditing="false" runat="server" CssEditorToolBar="contentEditorToolbar" CssEditorContent="contentEditorContent" NewUrl="~/admin/page.aspx" 
	ManageUrl="~/admin/pages.aspx" NotFoundUrlFormat="~/notFound.aspx?page={0}" PopUpEditorUrlFormat="~/admin/contentEditor.aspx?id={0}" />
</asp:Content>

