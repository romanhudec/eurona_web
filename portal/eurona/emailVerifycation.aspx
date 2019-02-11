<%@ Page  Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" Inherits="Eurona.EmailVerifycation" Codebehind="emailVerifycation.aspx.cs" %>
<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">
    <script type="text/jscript">
        $(document).ready(function () {
            $.blockUI.defaults.overlayCSS.cursor = 'default';
            $.blockUI({
                message: $('#verifyForm'),
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

            startVerifycationProcess();
        });

        function startVerifycationProcess() {
            var code="<%=Page.Request["code"]%>";
            var verifyForm = document.getElementById('verifyForm');
            var layoutVerifycationProcess = document.getElementById('layoutVerifycationProcess');
            var layoutVerifycationSuccess = document.getElementById('layoutVerifycationSuccess');
            var layoutVerifycationFailed = document.getElementById('layoutVerifycationFailed');

            verifyForm.style.display = 'block';
            layoutVerifycationProcess.style.display = 'block';
            layoutVerifycationSuccess.style.display = 'none';
            layoutVerifycationFailed.style.display = 'none';

            $.ajax({
                url: "<%=Page.ResolveUrl("~/emailVerifycationService.ashx")%>?method=verify",
                data: code,
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    if (data.Status == 0) {
                        verifyForm.style.display = 'block';
                        layoutVerifycationProcess.style.display = 'none';
                        layoutVerifycationSuccess.style.display = 'block';
                        layoutVerifycationFailed.style.display = 'none';
                    } else {
                        verifyForm.style.display = 'block';
                        layoutVerifycationProcess.style.display = 'none';
                        layoutVerifycationSuccess.style.display = 'none';
                        layoutVerifycationFailed.style.display = 'block';
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    verifyForm.style.display = 'block';
                    layoutVerifycationProcess.style.display = 'none';
                    layoutVerifycationSuccess.style.display = 'none';
                    layoutVerifycationFailed.style.display = 'block';
                }
            });
        }

        function onContinueToOffice() {
            location.href = "<%=Page.ResolveUrl("~/user/advisor/default.aspx")%>";
            //$.unblockUI();
        }
    </script>
    <div id="verifyForm" style="display:none;">
        <div id="layoutVerifycationProcess" style="display:none;">
            <table style="text-align:center;margin: 0 auto;">
                <tr>
                    <td><h2><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, EmailVerifyControl_Title %>"></asp:Literal></h2></td>
                </tr>
                <tr>
                    <td style="text-align:center;color:#EA008A;font-size: 16px;padding-bottom:10px;"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, EmailVerifyControl_VerifyingEmail_Message %>"></asp:Literal></td>
                </tr>
            </table>
        </div>
        <div id="layoutVerifycationSuccess" style="display:none;">
            <table style="text-align:center;margin: 0 auto;">
                <tr>
                    <td><h2><asp:Literal ID="Label1" runat="server" Text="<%$ Resources:Strings, EmailVerifyControl_Title %>"></asp:Literal></h2></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label runat="server" ID="lblVerificationMessage" Text="<%$ Resources:Strings, EmailVerifyControl_EmailVerified_Message %>" style="text-align:center;color:#EA008A;font-size: 20px;"></asp:Label>       
                    </td>
                </tr>
                <tr>
                    <td style="text-align:center;padding:20px;"><asp:Image ID="Image1" runat="server" ImageUrl="~/images/success.png" /></td>
                </tr>  
                <tr>
                    <td>
                        <asp:Button ID="btnContinue" runat="server" CssClass="button" Text="<%$ Resources:Strings, EmailVerifyControl_Pokracovat %>" OnClientClick="onContinueToOffice();" />    
                    </td>
                </tr>
            </table>
        </div>
        <div id="layoutVerifycationFailed" style="display:none;">
            <table>
                <tr>
                    <td><h2><asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Strings, EmailVerifyControl_Title %>"></asp:Literal></h2></td>
                </tr>
                <tr>
                    <td style="text-align:center;color:#EA008A;font-size: 16px;"><asp:Image ID="Image2" runat="server" ImageUrl="~/images/warning.png" /></td>
                </tr> 
                <tr>
                    <td style="text-align:center;color:#EA008A;font-size: 16px;"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, EmailVerifyControl_EmailVerifiedFailed_Message %>"></asp:Literal></td>
                </tr> 
                <tr>
                    <td style="text-align:right;padding-top:10px;">
                        <asp:Button ID="Button1" runat="server" CssClass="button" Text="<%$ Resources:Strings, EmailVerifyControl_Opakovat %>" OnClientClick="startVerifycationProcess();" />
                    </td>
                </tr>                              
            </table>
        </div>
    </div>
</asp:Content>

