<%@ Page Title="" Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" Inherits="Eurona.Login" Codebehind="login.aspx.cs" %>

<%@ Register src="Controls/Login.ascx" tagname="Login" tagprefix="uc1" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>
<%@ Register assembly="cms" namespace="CMS.Controls.Page" tagprefix="cmsPage" %>

<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">
<style>
    .pagemenu-container{display:none;}
    .onpage-akcni-nabidky{display:none;}
</style>
<table style="margin:auto;">
    <tr>
        <td>
            <cms:RoundPanel ID="RoundPanel1" runat="server" CssClass="roundPanel" Text="<%$ Resources:Strings, LoginControl_LoginHeaderLabel %>" Width="350px" Height="100%" Visible="False">
                <uc1:Login ID="login" runat="server" />
             </cms:RoundPanel>
             <cmsPage:PageControl ID="genericPage" IsEditing="false" PageName="user-login" runat="server" CssEditorToolBar="contentEditorToolbar" CssEditorContent="contentEditorContent" NewUrl="~/admin/page.aspx" 
	            ManageUrl="~/admin/pages.aspx" NotFoundUrlFormat="~/notFound.aspx?page={0}" PopUpEditorUrlFormat="~/admin/contentEditor.aspx?id={0}" />
        </td>
    </tr>
</table>     
</asp:Content>

