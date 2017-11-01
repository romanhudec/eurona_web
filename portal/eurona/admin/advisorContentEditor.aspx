<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="advisorContentEditor.aspx.cs" Inherits="Eurona.Admin.AdvisorContentEditorPage" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="cms" Namespace="CMS.Controls.RadEditor" TagPrefix="cmsRadEditor" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/styles/default.css" type="text/css" rel="Stylesheet" />
    <link href="~/styles/cms.css" type="text/css" rel="Stylesheet" />
    <style type="text/css">.body{ overflow:hidden;}</style>
    <script type="text/javascript">       
        function getRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        function closeRadWindow(eventArgs) {
            var oWnd = getRadWindow();
            oWnd.close(eventArgs);
        }

        function resizeEditor(editor) {
            var width = document.body.offsetWidth;
            var height = document.body.offsetHeight;
            if (width && width != 0) width = width - 10;
            if (height && height != 0) height = height - 40;
            editor.setSize(width, height);
        }

        function editorOnClientLoad(editor) {
            window.onresize = function() { resizeEditor(editor); };
            resizeEditor(editor);
            var elm = document.getElementById('editorContainer');
            elm.style.visibility = 'visible';
            resizeEditor(editor);
        }

        function onPopUpActivate(sender) {
            alert(activate);
        }    
    </script>       
</head>
<body class="body">
    <form id="form1" runat="server">
    <%//-- Definicia ScripManagera AJAX, TELERIK %>
    <asp:ScriptManager ID="ScriptManager" runat="server" />

    <table width="100%" style="height:100%;">
    <tr>
        <td style="height:auto;" valign="top">    
            <div runat="server" id="editorContainer" style="height:100%;visibility:hidden;"></div>
        </td>
    </tr>
    <tr>
        <td style="height:30px;">
            <div>
                <asp:Button ID="save" runat="server" OnClick="OnSave" Text="Uložiť" />
                <asp:Button ID="cancel" runat="server" OnClientClick="closeRadWindow(null); return false;" Text="Zrušiť" />
            </div>        
        </td>
    </tr>    
    </table>
    </form>
</body>
</html>
