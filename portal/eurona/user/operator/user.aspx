<%@ Page Title="" Language="C#" MasterPageFile="~/user/operator/admin.master" AutoEventWireup="true" CodeBehind="user.aspx.cs" Inherits="Eurona.User.Operator.UserPage" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls.UserManagement" TagPrefix="cmsUm" %>
<%@ Register Assembly="eurona" Namespace="Eurona.Controls.UserManagement" TagPrefix="cmsOrg" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server">
			<asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a> &nbsp;&raquo;&nbsp;
			<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_User %>" />
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" runat="Server">
	<cms:RoundPanel ID="rpPerson" runat="server" CssClass="roundPanel">
		<cmsUm:PersonControl ID="personControl" runat="server" Required="true" CssRoundPanel="roundPanelNoBg" IsEditing="true" Width="100%" />
	</cms:RoundPanel>
	<cms:RoundPanel ID="rpOrganization" runat="server" CssClass="roundPanel" Width="700px">
        <div><asp:Hyperlink ID="hlOsobnyReportPoradcu" runat="server" NavigateUrl="~/user/operator/osobniPrehledPoradce.aspx?id={0}" Target="_blank" Text="<%$ Resources:Reports, OsobniPrehledPoradce %>"></asp:Hyperlink></div>
		<cmsOrg:OrganizationControl ID="organizationControl" runat="server" CssRoundPanel="roundPanelNoBg" IsEditing="true" Width="100%" />
	</cms:RoundPanel>
</asp:Content>
