<%@ Page Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" Inherits="Eurona.AdvisorGenericPage" Codebehind="advisorPage.aspx.cs" %>

<%@ Register Assembly="Eurona" Namespace="Eurona.Controls" TagPrefix="cmsPage" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="aHome" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal id="aThisPage" runat="server" />	
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">
	<cmsPage:AdvisorPageControl ID="genericPage" IsEditing="false" runat="server" CssEditorToolBar="contentEditorToolbar" CssEditorContent="contentEditorContent" NewUrl="" 
	ManageUrl="" NotFoundUrlFormat="~/notFound.aspx?page={0}" PopUpEditorUrlFormat="~/admin/advisorContentEditor.aspx?id={0}" />
</asp:Content>

