<%@ Page Language="C#" MasterPageFile="~/user/advisor/page.Master" AutoEventWireup="true" Inherits="Eurona.User.Advisor.GenericPage" Codebehind="page.aspx.cs" %>

<%@ Register assembly="cms" namespace="CMS.Controls.Page" tagprefix="cmsPage" %>
<asp:Content ID="head" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript" language="javascript">
    function onLoad() {
        var elm = document.getElementById('elmPodporaProdejeContainer');
        elm.style.display = 'block';
    }
    window.onload = onLoad;
</script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="aHome" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal id="aThisPage" runat="server" />	
	</div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="content_header" runat="server">
    <div class="content-header-container">
        <asp:Label CssClass="title" ID="lblTitle" runat="server" />
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">
	<cmsPage:PageControl ID="genericPage" IsEditing="false" runat="server" CssEditorToolBar="contentEditorToolbar" CssEditorContent="contentEditorContent" NewUrl="~/admin/page.aspx" 
	ManageUrl="~/admin/pages.aspx" NotFoundUrlFormat="~/notFound.aspx?page={0}" PopUpEditorUrlFormat="~/admin/contentEditor.aspx?id={0}" />
</asp:Content>

