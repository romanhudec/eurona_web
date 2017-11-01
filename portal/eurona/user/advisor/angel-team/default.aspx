<%@ Page Title="Podmínky - V.I.P Angel team Professional" Language="C#" MasterPageFile="~/user/advisor/angel-team/page.master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Eurona.User.Advisor.AngelTeam.DefaultPage" %>
<%@ Register assembly="cms" namespace="CMS.Controls.Page" tagprefix="cmsPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #atmi_podminky{background-image: url(../../../images/angel-menuitem-selected-bg.png);}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div>
    	<cmsPage:PageControl ID="genericPage" PageName="angel-team-profesional-podminky-content" IsEditing="false" runat="server" CssEditorToolBar="contentEditorToolbar" CssEditorContent="contentEditorContent" NewUrl="~/admin/page.aspx" 
	    ManageUrl="~/admin/pages.aspx" NotFoundUrlFormat="~/notFound.aspx?page={0}" PopUpEditorUrlFormat="~/admin/contentEditor.aspx?id={0}" />
    </div>
</asp:Content>
