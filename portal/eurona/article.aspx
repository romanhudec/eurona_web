<%@ Page Title="<%$ Resources:Strings, Navigation_Article %>" Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" Inherits="Eurona.ArticlePage" Codebehind="article.aspx.cs" %>

<%@ Register assembly="cms" namespace="CMS.Controls.Article" tagprefix="cmsArticle" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
        <span>&nbsp;-&nbsp;</span>		
		<a id="returnUrl"  href="~/articleArchiv.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_ArchivedArticles %>" /></a>		
		<span>&nbsp;-&nbsp;</span>		
		<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_Article %>" />
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">
    <cmsArticle:ArticleControl ID="articleControl" CssClass="article" CssRating="rating" runat="server" CommentsFormatUrl="~/articleComments.aspx?id={0}" ArchivUrl="~/articleArchiv.aspx" >
      <%--  <ArticleTemplate>
            <div CssClass="article"><%#Container.DataItem.Title%></div>
        </ArticleTemplate>--%>
    </cmsArticle:ArticleControl>
</asp:Content>

