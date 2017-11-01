<%@ Page Title="<%$ Resources:Strings, UserPage_PersonDetail%>" Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" CodeBehind="registerPerson.aspx.cs" Inherits="Eurona.User.Host.RegisterPerson" %>

<%@ Register assembly="cms" namespace="CMS.Controls.UserManagement" tagprefix="cmsUser" %>
<%@ Register assembly="cms" namespace="CMS.Controls" tagprefix="cms" %>

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
	<cms:RoundPanel ID="RoundPanel2" runat="server" CssClass="roundPanel" >
		<cmsUser:PersonControl Visible="false" ID="personControl" runat="server" IsEditing="true" CssRoundPanel="roundPanel" SaveButtonText="<%$ Resources:Strings, ContinueButton_Text %>" />
	</cms:RoundPanel>

</asp:Content>
