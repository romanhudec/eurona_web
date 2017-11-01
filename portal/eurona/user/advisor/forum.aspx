<%@ Page Title="<%$ Resources:Strings, Navigation_Forum %>" Language="C#" MasterPageFile="~/user/advisor/page.master" AutoEventWireup="true" Inherits="Eurona.ForumPage" Codebehind="forum.aspx.cs" %>

<%@ Register assembly="cms" namespace="CMS.Controls.Forum" tagprefix="cmsForum" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="banner" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content_header" Runat="Server">
    <div class="content-header-container">
        <asp:Label CssClass="title" ID="Literal1" runat="server" Text="<%$Resources:Strings, Navigation_Forum %>" />
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">
    <div style="padding:5px;background-color:#0092f3;">
	    <%--<cmsForum:ForumPostsControl ID="forumPostsControl" CssClass="forum" ShowForumHeader="true" ForumContentUrl="~/forumThreads.aspx" PagerSize="10" runat="server" />--%>
        <cmsForum:ForumPostsControl ID="forumPostsControl" CssClass="forumPosts" CssCarma="carma" ShowForumHeader="true" ColapsibleAttachment="true" ForumContentUrl="~/user/advisor/forumThreads.aspx" PagerSize="10" runat="server" >
          <%--  <ArticleTemplate>
                <div CssClass="article"><%#Container.DataItem.Title%></div>
            </ArticleTemplate>--%>
        </cmsForum:ForumPostsControl>
        <cmsForum:ForumPostFormControl ID="forumPostFormControl" CssClass="forumPostForm" HiddenFieldParenId="hfParentId" MaxfilesToUpload="5"  runat="server" />
    </div>
</asp:Content>

