<%@ Control Language="C#" AutoEventWireup="true" Inherits="Eurona.Controls.LoginControl" Codebehind="LoginControl.ascx.cs" %>
<%@ Register Assembly="cms" Namespace="CMS.Utilities" TagPrefix="cmsUtilities" %>
<script language="javascript" type="text/javascript">

    function doLoginUser(e) {
        if (!e) var e = window.event;
        if (e.which || e.keyCode) {
            if ((e.which == 13) || (e.keyCode == 13)) {
                var button = document.getElementById("<%=doLogin.ClientID %>");
                //button.click();
                __doPostBack("<%=doLogin.UniqueID  %>", '');
                return false;
            }
        } else return true
    }              
</script>
<table style="display:inline-block;" class="login-control" border="0"  id="loginForm" runat="server">
	<tr>
		<td align="right" style="white-space:nowrap;">
			<asp:Label runat="server" Text="<%$ Resources:Strings, LoginControl_LoginLabel %>"></asp:Label>
		</td>
		<td>
			<input onkeydown="doLoginUser(event)" class="login" type="text" runat="server" id="login" />
		</td>
        <td><div style="width:1px;height:25px;background-color:#fff;margin-right:10px;"></div></td>
		<td align="right" style="white-space:nowrap;">
			<asp:Label runat="server" Text="<%$ Resources:Strings, LoginControl_PasswordLabel %>"></asp:Label>
		</td>
		<td>
			<input onkeydown="doLoginUser(event)" class="password" type="password" runat="server" id="password" />
		</td>
        <td><div style="width:1px;height:25px;background-color:#fff;margin-right:10px;"></div></td>
		<td align="center">
		    <b><asp:LinkButton runat="server" class="button" id="doLogin" Text="<%$ Resources:Strings, LoginControl_LoginButton %>" OnClick="OnLoginClick" causesvalidation="false"/></b>
		</td>
		<td>
			&nbsp;&nbsp;&nbsp;&nbsp;
			<asp:HyperLink runat="server" class="button" id="doForgotPassword" Text="<%$ Resources:Strings, LoginControl_ForgotPasswordButton %>" NavigateUrl="../forgotPassword.aspx?ReturnUrl=default.aspx" />
		</td>      	
	</tr>	
</table>
<%--UTILITIES--%>

