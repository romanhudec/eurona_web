<%@ Page Language="C#" MasterPageFile="~/user/host/page.master" AutoEventWireup="true" Inherits="Eurona.HostAccessPage" Codebehind="HostAccess.aspx.cs" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls.Page" TagPrefix="cmsPage" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="aHome" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="thisPage" runat="server" />
	</div>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="content" Runat="Server">
<cmsPage:PageControl ID="genericPage" IsEditing="false" PageName="host-access" runat="server" CssEditorToolBar="contentEditorToolbar" CssEditorContent="contentEditorContent" NewUrl="~/admin/page.aspx" 
    ManageUrl="~/admin/pages.aspx" NotFoundUrlFormat="~/notFound.aspx?page={0}" PopUpEditorUrlFormat="~/admin/contentEditor.aspx?id={0}" />
</asp:Content>

