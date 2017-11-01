<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="kariera.aspx.cs" Inherits="Eurona.eshop.kariera" %>
<%@ Register assembly="cms" namespace="CMS.Controls.Page" tagprefix="cmsPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <%//-- Definicia ScripManagera AJAX, TELERIK %>
        <asp:ScriptManager id="ScriptManager" runat="server"/> 
        <div>
    	    <cmsPage:PageControl ID="genericPage" PageName="kariera-s-euronou" IsEditing="false" runat="server" CssEditorToolBar="contentEditorToolbar" CssEditorContent="contentEditorContent" NewUrl="~/admin/page.aspx" 
	        ManageUrl="~/admin/pages.aspx" NotFoundUrlFormat="~/notFound.aspx?page={0}" PopUpEditorUrlFormat="~/admin/contentEditor.aspx?id={0}" />
        </div>
    </form>
</body>
</html>
