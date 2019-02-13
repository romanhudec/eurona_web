<%@ Page Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" Inherits="Eurona.User.RequestEmailVerifycation" Codebehind="requestEmailVerifycation.aspx.cs" %>

<%@ Register Src="~/user/RequestEmailVerifyControl.ascx" TagPrefix="uc1" TagName="RequestEmailVerifyControl" %>

<asp:Content ID="Content5" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script> 
    <link href='<%=ResolveUrl("~/styles/emailVerify.css") %>' type="text/css" rel="stylesheet" />
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
                    top: '30%',
                    width: '30%',
                    left: '37%',
                    cursor: 'default',
                    border: 'none',
                    padding: '15px',
                    backgroundColor: '#fff',
                    '-webkit-border-radius': '10px',
                    '-moz-border-radius': '10px',
                    'border-radius' : '10px',
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

