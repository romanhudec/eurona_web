<%@ Page Title="Diskuze" Language="C#" MasterPageFile="~/user/advisor/angel-team/page.master" AutoEventWireup="true" CodeBehind="diskuze.aspx.cs" Inherits="Eurona.User.Advisor.AngelTeam.DiskuzePage" %>
<%@ Register assembly="cms" namespace="CMS.Controls.Page" tagprefix="cmsPage" %>
<%@ Register assembly="cms" namespace="CMS.Controls.Forum" tagprefix="cmsForum" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #atmi_podminky{background-image: url(../../../images/angel-menuitem-selected-bg.png);}
        .atmi-table{display:none;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div style="padding:5px;background-color:#0092f3;">
	    <cmsForum:ForumsControl ID="forumsControl" CssClass="forums" ShowForumHeader="true" NewUrl="~/user/advisor/forumDetail.aspx?threadId={0}" ForumContentUrl="~/user/advisor/forumThreads.aspx" RSSFormatUrl="" PagerSize="10" runat="server" />
    </div>
</asp:Content>
