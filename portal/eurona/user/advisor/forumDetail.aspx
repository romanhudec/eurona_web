<%@ Page Title="<%$ Resources:Strings, Navigation_Forum %>" Language="C#" MasterPageFile="~/user/advisor/page.master" AutoEventWireup="true" Inherits="Eurona.ForumDetailPage" Codebehind="forumDetail.aspx.cs" %>

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
    <div style="padding:5px;">
	    <cmsForum:ForumControl ID="forumDetailControl" UrlAliasPrefixId="5" DisplayUrlFormat="~/user/advisor/forum.aspx?id={0}" runat="server" />
    </div>
</asp:Content>

