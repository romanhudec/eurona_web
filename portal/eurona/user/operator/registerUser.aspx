<%@ Page Title="<%$ Resources:Strings, UserPage_PersonDetail%>" Language="C#"  MasterPageFile="~/user/operator/admin.master" AutoEventWireup="true" CodeBehind="registerUser.aspx.cs" Inherits="Eurona.User.Operator.RegisterUser" %>

<%@ Register assembly="cms" namespace="CMS.Controls.UserManagement" tagprefix="cmsUser" %>
<%@ Register assembly="cms" namespace="CMS.Controls" tagprefix="cms" %>
<%@ Register Assembly="eurona" Namespace="Eurona.Controls.UserManagement" TagPrefix="cmsOrg" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_Registration %>" />
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" runat="server">
	<cms:RoundPanel ID="RoundPanel2" runat="server" CssClass="roundPanel" Text="<%$ Resources:Strings, Navigation_Registration %>"  Width="700px">
		<cmsOrg:OrganizationControl Visible="false" ID="organizationControl" runat="server" IsEditing="true" CssRoundPanel="roundPanelNoBg" SaveButtonText="<%$ Resources:Strings, ContinueButton_Text %>" />
	</cms:RoundPanel>

</asp:Content>
