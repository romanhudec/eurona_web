<%@ Page Title="<%$ Resources:Strings, Navigation_Registration %>" Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" CodeBehind="register.aspx.cs" Inherits="Eurona.User.Operator.RegisterPage" %>
<%@ Register src="../../Controls/Register.ascx" tagname="Register" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="banner" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_Registration %>" />
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" runat="server">
    <div class="register_user">
        <uc1:Register ID="registerControl" runat="server" />
    </div>
</asp:Content>
