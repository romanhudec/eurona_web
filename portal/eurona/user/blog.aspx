<%@ Page Title="<%$ Resources:Strings, Navigation_Blog %>" Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" Inherits="Eurona.User.Blog" Codebehind="blog.aspx.cs" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls.Blog" TagPrefix="cmsBlog" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" />
		<span>&nbsp;-&nbsp;</span>
		<a id="A5" href="~/user/blogs.aspx" runat="server"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_Blogs %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Strings, Navigation_Blog %>" />
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <cmsBlog:AdminBlogControl ID="userBlogControl" runat="server" UrlAliasPrefixId="2" DisplayUrlFormat="~/blog.aspx?id={0}" CommentsFormatUrl="~/blogComments.aspx?id={0}"/>
</asp:Content>

