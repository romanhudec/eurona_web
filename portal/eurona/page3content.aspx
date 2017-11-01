<%@ Page Language="C#" MasterPageFile="~/page3content.master" AutoEventWireup="true" Inherits="Eurona.GenericPage3Content" Codebehind="page3content.aspx.cs" %>

<%@ Register assembly="cms" namespace="CMS.Controls.Page" tagprefix="cmsPage" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="aHome" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal id="aThisPage" runat="server" />	
	</div>
</asp:Content>
<asp:Content ID="_content" ContentPlaceHolderID="content" runat="server">
<cmsPage:PageControl ID="genericPage" IsEditing="false" runat="server" CssEditorToolBar="contentEditorToolbar" CssEditorContent="contentEditorContent" NewUrl="~/admin/page.aspx" 
	ManageUrl="~/admin/pages.aspx" NotFoundUrlFormat="~/notFound.aspx?page={0}" PopUpEditorUrlFormat="~/admin/contentEditor.aspx?id={0}" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="sidebar_left" Runat="Server">
	<cmsPage:PageControl ID="genericPage1" IsEditing="false" runat="server" CssEditorToolBar="contentEditorToolbar" CssEditorContent="contentEditorContent" NewUrl="~/admin/page.aspx" 
	ManageUrl="~/admin/pages.aspx" NotFoundUrlFormat="~/notFound.aspx?page={0}" PopUpEditorUrlFormat="~/admin/contentEditor.aspx?id={0}" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="sidebar_center" Runat="Server">
	<cmsPage:PageControl ID="genericPage2" IsEditing="false" runat="server" CssEditorToolBar="contentEditorToolbar" CssEditorContent="contentEditorContent" NewUrl="~/admin/page.aspx" 
	ManageUrl="~/admin/pages.aspx" NotFoundUrlFormat="~/notFound.aspx?page={0}" PopUpEditorUrlFormat="~/admin/contentEditor.aspx?id={0}" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="sidebar_right" Runat="Server">
	<cmsPage:PageControl ID="genericPage3" IsEditing="false" runat="server" CssEditorToolBar="contentEditorToolbar" CssEditorContent="contentEditorContent" NewUrl="~/admin/page.aspx" 
	ManageUrl="~/admin/pages.aspx" NotFoundUrlFormat="~/notFound.aspx?page={0}" PopUpEditorUrlFormat="~/admin/contentEditor.aspx?id={0}" />
</asp:Content>

