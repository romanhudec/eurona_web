<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="Eurona.Admin.ForumPost" Codebehind="forumPost.aspx.cs" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls.Forum" TagPrefix="cmsForum" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A3" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		&nbsp;&raquo;&nbsp;		
		<a id="A4" href="~/admin/default.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_Administration %>" /></a>
		&nbsp;&raquo;&nbsp;
		<a id="A5" href="~/admin/forumPosts.aspx" runat="server"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_ForumPosts %>" /></a>
		&nbsp;&raquo;&nbsp;
		<asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Strings, Navigation_ForumPost %>" />
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <cmsForum:AdminForumPostControl ID="adminForumPostControl" runat="server"/>
</asp:Content>

