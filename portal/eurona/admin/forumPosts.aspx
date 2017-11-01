<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="Eurona.Admin.ForumPosts" Codebehind="forumPosts.aspx.cs" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls.Forum" TagPrefix="cmsForum" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		&nbsp;&raquo;&nbsp;		
		<a id="A2" href="~/admin/default.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_Administration %>" /></a>
		&nbsp;&raquo;&nbsp;
		<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_ForumPosts %>" />
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <cmsForum:AdminForumPostsControl ID="adminForumPostsControl" CssClass="dataGrid" runat="server" NewUrl="~/admin/forumPost.aspx?forumId={0}" EditUrlFormat="~/admin/forumPost.aspx?Id={0}" />
</asp:Content>

