<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RequestEmailVerifyControl.ascx.cs" Inherits="Eurona.Controls.RequestEmailVerifyControl" %>
<script type="text/javascript">
    function checkEmail() {
        var email = document.getElementById('<%=txtEmail.ClientID%>').value;
        var labelElm = document.getElementById('<%=lblValidatorText.ClientID%>');
        var btnVerifyElm = document.getElementById('<%=btnVerify.ClientID%>');
        labelElm.style.display = "none";
        btnVerifyElm.disabled = true;
        var isEmailValid = validateEmailPattern(email);
        if (isEmailValid == false) return;
        

        $.ajax({
            url: "<%=Page.ResolveUrl("~/emailVerifycationService.ashx")%>?method=checkEmail",
            data: email,
            dataType: "json",
            type: "POST",
            contentType: "application/json; charset=utf-8",
            dataFilter: function (data) { return data; },
            success: function (data) {
                if (data.Status != 0) {
                    labelElm.innerText = data.Message;
                    labelElm.style.display = "block";
                } else {
                    btnVerifyElm.disabled = false;
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                labelElm.style.display = "block";
            }
        });
    }
    function validateEmailPattern(email) {
        var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(String(email).toLowerCase());
    }

    function onBtnVerifyClick() {
        var layoutTypeEmail = document.getElementById('layoutTypeEmail');
        var layoutSendinEmail = document.getElementById('layoutSendinEmail');
        var layoutSendinEmailError = document.getElementById('layoutSendinEmailError');
        var layoutEmailSend = document.getElementById('layoutEmailSend');
        layoutTypeEmail.style.display = 'none';
        layoutSendinEmail.style.display = 'table';
        layoutSendinEmailError.style.display = 'none';
        layoutEmailSend.style.display = 'none';

        var email = document.getElementById('<%=txtEmail.ClientID%>').value;

        var dataValue = "{ email: '"+email+"'}";
        $.ajax({
            url: "<%=Page.ResolveUrl("~/emailVerifycationService.ashx")%>?method=sendEmail2EmailVerify",
              data: dataValue,
              dataType: "json",
              type: "POST",
              contentType: "application/json; charset=utf-8",
              dataFilter: function (data) { return data; },
              success: function (data) {
                  if (data.Status == 0) {
                      layoutTypeEmail.style.display = 'none';
                      layoutSendinEmail.style.display = 'none';
                      layoutSendinEmailError.style.display = 'none';
                      layoutEmailSend.style.display = 'table';
                  } else {
                      layoutTypeEmail.style.display = 'none';
                      layoutSendinEmail.style.display = 'none';
                      layoutSendinEmailError.style.display = 'table';
                      layoutEmailSend.style.display = 'none';
                  }
              },
              error: function (XMLHttpRequest, textStatus, errorThrown) {
                  layoutTypeEmail.style.display = 'none';
                  layoutSendinEmail.style.display = 'none';
                  layoutSendinEmailError.style.display = 'table';
                  layoutEmailSend.style.display = 'none';
              }
          });
    }
</script>
<table style="width:100%;">
    <tr>
        <td style="text-align:center;"><h2><asp:Literal ID="Label1" runat="server" Text="<%$ Resources:Strings, EmailVerifyControl_Title %>"></asp:Literal></h2></td>
    </tr>
    <tr>
        <td>
            <div style="width:100%;display:block;">
                <table id="layoutTypeEmail" style="width:100%;display:table;">
                    <tr>
                        <td style="text-align:left;"><asp:Label runat="server" Text="<%$ Resources:Strings, EmailVerifyControl_Email %>"></asp:Label></td>
                    </tr>
                    <tr>
                        <td><asp:TextBox runat="server" ID="txtEmail" Width="100%" oninput="checkEmail()"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td><asp:Label runat="server" ID="lblValidatorText" ForeColor="#EA008A" style="display:none;font-size: 16px;padding-top:10px;"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="text-align:right;padding-top:10px;">
                            <asp:Button runat="server" ID="btnVerify" CssClass="button" Text="<%$ Resources:Strings, EmailVerifyControl_OveritEmail %>" disabled="true" OnClientClick="onBtnVerifyClick();" />
                        </td>
                    </tr>
                </table>
                <table id="layoutSendinEmail" style="width:100%;display:none;">
                    <tr>
                        <td style="text-align:center;color:#EA008A;font-size: 16px;"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, EmailVerifyControl_SendingEmail_Message %>"></asp:Literal></td>
                    </tr>
                </table>
                <table id="layoutEmailSend" style="width:100%;display:none;">
                    <tr>
                         <td style="text-align:center;color:#EA008A;font-size: 16px;"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, EmailVerifyControl_EmailSended_Message %>"></asp:Literal></td>
                    </tr>  
                    <tr>
                         <td style="text-align:center;color:#EA008A;font-size: 16px;"><asp:Image ID="Image1" runat="server" ImageUrl="~/images/success.png" /></td>
                    </tr>                              
                </table>
                <table id="layoutSendinEmailError" style="width:100%;display:none;">
                    <tr>
                         <td style="text-align:center;color:#EA008A;font-size: 16px;"><asp:Image ID="Image2" runat="server" ImageUrl="~/images/warning.png" /></td>
                    </tr> 
                    <tr>
                         <td style="text-align:center;color:#EA008A;font-size: 16px;"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, EmailVerifyControl_SendingEmailError_Message %>"></asp:Literal></td>
                    </tr> 
                    <tr>
                        <td style="text-align:right;padding-top:10px;">
                            <asp:Button runat="server" CssClass="button" Text="<%$ Resources:Strings, EmailVerifyControl_OdeslatZnovu %>" OnClientClick="onBtnVerifyClick();" />
                        </td>
                    </tr>                              
                </table>
            </div>
        </td>
    </tr>
</table>
