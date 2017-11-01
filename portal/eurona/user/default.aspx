<%@ Page Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true"
	CodeBehind="default.aspx.cs" Inherits="Eurona.User.DefaultPage" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls.UserManagement" TagPrefix="cmsUm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="banner" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server">
			<asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a> <span>&nbsp;-&nbsp;</span>
			<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_User %>" />
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" runat="Server">
	<cms:RoundPanel ID="rpPerson" runat="server" CssClass="roundPanel" Width="500px">
		<cmsUm:PersonControl ID="personControl" runat="server" Required="true" CssRoundPanel="roundPanel"
			IsEditing="true" Width="100%" />
	</cms:RoundPanel>
	<cms:RoundPanel ID="rpOrganization" runat="server" CssClass="roundPanel" Width="500px">
		<cmsUm:OrganizationControl ID="organizationControl" runat="server" CssRoundPanel="roundPanel"
			IsEditing="true" Width="100%" />
	</cms:RoundPanel>
</asp:Content>
