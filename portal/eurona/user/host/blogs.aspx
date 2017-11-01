<%@ Page Title="<%$ Resources:Strings, Navigation_Blogs %>" Language="C#" MasterPageFile="~/user/host/page.master" AutoEventWireup="true" Inherits="Eurona.User.Host.Blogs" Codebehind="blogs.aspx.cs" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls.Blog" TagPrefix="cmsBlog" %>
<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_Blogs %>" />
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <cmsBlog:AdminBlogsControl ID="userBlogsControl" CssClass="dataGrid" runat="server"
	    NewUrl="~/user/blog.aspx" EditUrlFormat="~/user/blog.aspx?Id={0}" />
</asp:Content>

