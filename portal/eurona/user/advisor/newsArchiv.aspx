<%@ Page Title="<%$ Resources:Strings, Navigation_ArchivedNews %>" Language="C#" MasterPageFile="~/user/advisor/page.master" AutoEventWireup="true" Inherits="Eurona.User.Advisor.NewsArchiv" Codebehind="newsArchiv.aspx.cs" %>

<%@ Register assembly="cms" namespace="CMS.Controls.News" tagprefix="cmsNews" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/user/advisor/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>			
		<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_ArchivedNews %>" />
	</div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="content_header" runat="server">
    <div class="content-header-container">
        <asp:Label CssClass="title" ID="lblTitle" runat="server" Text="<%$ Resources:Strings, Navigation_ArchivedNews %>" />
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">
	<cmsNews:ArchivedNewsControl ID="archivedNewsControl" CssClass="newsArchiv" DisplayUrlFormat="~/user/advisor/news.aspx?id={0}" 
	CssEditorToolBar="contentEditorToolbar" CssEditorContent="contentEditorContent" NewUrl="~/admin/news.aspx" 
	ManageUrl="~/admin/newsList.aspx" runat="server">
	<%--<NewsTemplate>
	    <div class="newsArchiv_head">
			<%# (Container.DataItem as CMS.Entities.News).Head %>
	    </div>
	</NewsTemplate>	--%>
	</cmsNews:ArchivedNewsControl>
</asp:Content>

