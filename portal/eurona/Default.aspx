<%@ Page Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" Inherits="Eurona.DefaultPage" CodeBehind="default.aspx.cs" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls.Page" TagPrefix="cmsPage" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls.Translations" TagPrefix="cmsVocabulary" %>

<asp:Content ID="head" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="navigation" ContentPlaceHolderID="navigation" runat="Server">
    <div class="navigation-links">
        <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" />
    </div>
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="content" runat="Server">
    <style type="text/css">
        .lotos {
            position: relative;
            z-index: 1!important;
        }
    </style>
   
    <table border="0" style="width: 100%; height: 100%;" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <cmsPage:PageControl ID="genericPage" IsEditing="false" runat="server" CssEditorToolBar="contentEditorToolbar" CssEditorContent="contentEditorContent" NewUrl="~/admin/page.aspx"
                    ManageUrl="~/admin/pages.aspx" NotFoundUrlFormat="~/notFound.aspx?page={0}" PageName="home-content" PopUpEditorUrlFormat="~/admin/contentEditor.aspx?id={0}" />
            </td>
        </tr>
        <tr>
            <td valign="middle" align="center" style="height: 250px;">
                <cmsPage:PageControl ID="genericPage2" IsEditing="false" runat="server" CssEditorToolBar="contentEditorToolbar" CssEditorContent="contentEditorContent" NewUrl="~/admin/page.aspx"
                    ManageUrl="~/admin/pages.aspx" NotFoundUrlFormat="~/notFound.aspx?page={0}" PageName="home-banner" PopUpEditorUrlFormat="~/admin/contentEditor.aspx?id={0}" />
            </td>
        </tr>
    </table>
    <cmsVocabulary:Vocabulary ID="vocMasterPage" CssClass="vocabulary" runat="server" Name="MasterPage" />

    <script type="text/javascript">
        var leady_track_key = "J9APK9zccYu5TPq2";
        var leady_track_server = document.location.protocol + "//t.leady.cz/";
        (function () {
            var l = document.createElement("script"); l.type = "text/javascript"; l.async = true;
            l.src = leady_track_server + leady_track_key + "/L.js";
            var s = document.getElementsByTagName("script")[0]; s.parentNode.insertBefore(l, s);
        })();
    </script>

</asp:Content>

