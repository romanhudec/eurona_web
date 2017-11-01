<%@ Page Title="<%$ Resources:Strings, Navigation_Forums %>" Language="C#" MasterPageFile="~/user/advisor/page.master" AutoEventWireup="true" Inherits="Eurona.ForumThreadPage" Codebehind="forumThread.aspx.cs" %>

<%@ Register assembly="cms" namespace="CMS.Controls.Forum" tagprefix="cmsForum" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="banner" Runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content_header" Runat="Server">
    <div class="content-header-container">
        <asp:Label CssClass="title" ID="Literal1" runat="server" Text="<%$Resources:Strings, Navigation_Forums %>" />
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">
    <div style="padding:5px;background-color:#0092f3;">
	    <cmsForum:ForumsControl ID="forumsControl" CssClass="forums" ShowForumHeader="true" NewUrl="~/user/advisor/forumDetail.aspx?threadId={0}" ForumContentUrl="~/user/advisor/forumThreads.aspx" RSSFormatUrl="" PagerSize="10" runat="server" />
    </div>
</asp:Content>

