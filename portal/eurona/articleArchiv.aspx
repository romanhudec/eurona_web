<%@ Page Title="<%$ Resources:Strings, Navigation_ArchivedArticles %>" Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" Inherits="Eurona.ArticleArchivPage" Codebehind="articleArchiv.aspx.cs" %>

<%@ Register assembly="cms" namespace="CMS.Controls.Article" tagprefix="cmsArticle" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1"  href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_ArchivedArticles %>" />
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">
	<cmsArticle:ArchivedArticlesControl ID="archivedArticlesControl" CssClass="articleArchiv" CommentsFormatUrl="~/articleComments.aspx?id={0}" DisplayUrlFormat="~/article.aspx?id={0}" 
	CssEditorToolBar="contentEditorToolbar" CssEditorContent="contentEditorContent" NewUrl="~/admin/article.aspx" 
	ManageUrl="~/admin/articles.aspx" runat="server">
	<%--<ArticleTemplate>
	    <div class="articleArchiv_title">
			<%# (Container.DataItem as CMS.Entities.Article).Title %>
	    </div>
	</ArticleTemplate>--%>
	</cmsArticle:ArchivedArticlesControl>
</asp:Content>

