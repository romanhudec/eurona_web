<%@ Page Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeBehind="user.aspx.cs" Inherits="Eurona.Admin.UserPage" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls.UserManagement" TagPrefix="cmsUm" %>
<%@ Register Assembly="eurona" Namespace="Eurona.Controls.UserManagement" TagPrefix="cmsOrg" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A3" href="~/default.aspx" runat="server"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<a id="A4" href="~/admin/default.aspx" runat="server"><asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Strings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<a id="A5" href="~/admin/accounts.aspx" runat="server"><asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:Strings, Navigation_Users %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:Strings, Navigation_User %>" />
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" runat="Server">
	<cms:RoundPanel ID="rpPerson" runat="server" CssClass="roundPanel">
		<cmsUm:PersonControl ID="personControl" runat="server" Required="true" CssRoundPanel="roundPanelNoBg" IsEditing="true" Width="100%" />
	</cms:RoundPanel>
	<cms:RoundPanel ID="rpOrganization" runat="server" CssClass="roundPanel" Width="700px">
		<cmsOrg:OrganizationControl ID="organizationControl" runat="server" CssRoundPanel="roundPanelNoBg" IsEditing="true" Width="100%" />
	</cms:RoundPanel>
</asp:Content>
