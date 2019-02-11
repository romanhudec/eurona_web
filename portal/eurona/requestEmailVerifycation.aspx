<%@ Page Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" Inherits="Eurona.RequestEmailVerifycation" Codebehind="requestEmailVerifycation.aspx.cs" %>

<%@ Register Src="~/Controls/RequestEmailVerifyControl.ascx" TagPrefix="uc1" TagName="RequestEmailVerifyControl" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">
    <script type="text/jscript">
        /*
        $(document).ready(function() {
            $.blockUI({ message: $('#verfityForm') });
        });
        */

        $(document).ready(function () {
            $.blockUI.defaults.overlayCSS.cursor = 'default';
            $.blockUI({
                message: $('#verfityForm'),
                overlayCSS: { backgroundColor: '#333' },
                css: {
                    width: '25%',
                    left: '37%',
                    cursor: 'default',
                    border: 'none',
                    padding: '15px',
                    backgroundColor: '#fff',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    opacity: 1,
                    color: '#EA008A'
                }
            });
        });
    </script>
    <div id="verfityForm" style="display:none;cursor: default;">
        <uc1:RequestEmailVerifyControl runat="server" ID="RequestEmailVerifyControl" />
    </div>
</asp:Content>

