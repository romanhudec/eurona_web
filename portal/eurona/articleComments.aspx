﻿<%@ Page Title="<%$ Resources:Strings, Navigation_ArticleComments %>" Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" Inherits="Eurona.ArticleCommentsPage" Codebehind="articleComments.aspx.cs" %>

<%@ Register assembly="cms" namespace="CMS.Controls.Article" tagprefix="cmsArticle" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<a id="returnUrl"  href="~/articleArchiv.aspx" runat="server"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_ArchivedArticles %>" /></a>
        <span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_ArticleComments %>" />		
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">
    <div class="comments">
    <cmsArticle:ArticleCommentsControl ID="articleCommentsControl" CssCarma="carma" DisplayUrlFormat="~/article.aspx?id={0}" CssClass="comments" runat="server" >
      <%--  <ArticleTemplate>
            <div CssClass="article"><%#Container.DataItem.Title%></div>
        </ArticleTemplate>--%>
    </cmsArticle:ArticleCommentsControl>
    <cmsArticle:ArticleCommentFormControl ID="articleCommentFormControl" CssClass="commentForm" HiddenFieldParenId="hfParentId"  runat="server" />
    </div>
</asp:Content>

