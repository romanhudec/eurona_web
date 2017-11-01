<%@ Control Language="C#" AutoEventWireup="true" Inherits="Eurona.Controls.Login" Codebehind="Login.ascx.cs" %>
<%@ Register Assembly="cms" Namespace="CMS.Utilities" TagPrefix="cmsUtilities" %>
<script language="javascript" type="text/javascript">
	/*
    var firstTimeLogin = true;
	function loginFocused() {
		if (!firstTimeLogin) return;
		var login = document.getElementById("<%=login.ClientID%>");
		firstTimeLogin = false;
		login.value = "";
		login.style.color = "black";
	}
	var firstTimePassword = true;
	function passwordFocused(obj) {
		if (!firstTimePassword) return;
		var newO = document.createElement('input');
		newO.setAttribute('type', 'password');
		newO.setAttribute('name', obj.getAttribute('name'));
		newO.setAttribute('id', obj.getAttribute('id'));
		newO.setAttribute('class', obj.getAttribute('class'));
		newO.style.color = "black";
		newO.style.width = "99%";
		newO.onkeydown = doLoginUser;
		obj.parentNode.replaceChild(newO, obj);
		window.setTimeout("document.getElementById('" + obj.getAttribute('id') + "').focus()", 200);
		//newO.focus();
    }

    function doLoginUser(e) {

        if (!e) var e = window.event;
        if(e.which || e.keyCode){
            if ((e.which == 13) || (e.keyCode == 13)){
                var button = document.getElementById("<%=doLogin.ClientID %>");
                //button.click();
                __doPostBack("<%=doLogin.UniqueID  %>", '');
                return false;
            }
        } else  return true
    }
    */
    function loginFocused() {
    }
    function passwordFocused(obj) {
    }
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
<%--<div class="login-form-header"><span><asp:Literal runat="server" Text="<%$ Resources:Strings, LoginControl_LoginHeaderLabel %>"></asp:Literal></span></div>--%>
<table style="margin:auto;padding-top:10px;padding-bottom:10px;" class="login-form" border="0" cellspacing="10" id="loginForm" runat="server" visible="false">
	<tr>
		<td align="right" style="white-space:nowrap;">
			<asp:Label runat="server" Text="<%$ Resources:Strings, LoginControl_LoginLabel %>"></asp:Label> :
		</td>
		<td  style="width:100%;">
			<input onfocus="loginFocused()" onkeydown="doLoginUser(event)" class="login" type="text" runat="server" id="login" />
		</td>
	</tr>
	<tr>
		<td align="right" style="white-space:nowrap;">
			<asp:Label runat="server" Text="<%$ Resources:Strings, LoginControl_PasswordLabel %>"></asp:Label> :
		</td>
		<td style="width:100%;">
			<input onfocus="passwordFocused(this)" onkeydown="doLoginUser(event)" class="password" type="password" runat="server" id="password" />
		</td>
	</tr>	
	<tr>
		<td align="center" colspan="2" style="padding-top:10px;>
		    <b><asp:LinkButton runat="server" class="button" id="doLogin" Text="<%$ Resources:Strings, LoginControl_LoginButton %>" OnClick="OnLoginClick" causesvalidation="false"/></b>
		    &nbsp;|&nbsp;<asp:HyperLink runat="server" class="button" id="doForgotPassword" Text="<%$ Resources:Strings, LoginControl_ForgotPasswordButton %>" NavigateUrl="../forgotPassword.aspx?ReturnUrl=default.aspx" />
            &nbsp;|&nbsp;<asp:HyperLink runat="server" class="button" id="doRegister" Text="<%$ Resources:Strings, Navigation_Registration %>" />
		</td>        	
	</tr>
<%--	<tr>
		<td>
		    <a class="navigation-link"  href='<%=aliasUtilities.Resolve("~/user/advisor/register.aspx")%>' >
		        <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Strings, Navigation_Registration %>" />
		    </a>
		</td>    	
	</tr>--%>		
</table>
<table border="0" class="login-form" id="loginInfo" runat="server" visible="false">
	<tr>
		<td colspan="3" align="right">
			<asp:Literal runat="server" Text="<%$ Resources:Strings, LoginControl_Welcome %>"></asp:Literal>&nbsp;<asp:HyperLink id="info" runat="server"></asp:HyperLink>
		</td>
	</tr>
	<tr>
	    <td colspan="3" align="right">
	        <b><asp:LinkButton runat="server" Text='<%$ Resources:Strings, LoginControl_LogoutButton %>' OnClick="OnLogoutClick" causesvalidation="false" ></asp:LinkButton></b>
	    </td>
	</tr>
</table>
<%--UTILITIES--%>
<cmsUtilities:AliasUtilities ID="aliasUtilities" runat="server" ></cmsUtilities:AliasUtilities>   


