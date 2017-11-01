<%@ Page Language="C#" Title="<%$ Resources:Strings, UserPage_PersonDetail%>" MasterPageFile="~/user/host/page.master" AutoEventWireup="true" CodeBehind="detail.aspx.cs" Inherits="Eurona.User.Host.Detail" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls.UserManagement" TagPrefix="cmsUm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="banner" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" runat="server">
	<cms:RoundPanel ID="rpPerson" runat="server" CssClass="roundPanel" Text="<%$ Resources:Strings, UserPage_PersonDetail%>">
		<cmsUm:PersonControl ID="personControl" runat="server" Required="true" CssRoundPanel="roundPanel"
			IsEditing="false" Width="100%" />
	</cms:RoundPanel>
	<cms:RoundPanel ID="rpOrganization" runat="server" CssClass="roundPanel" Text="<%$ Resources:Strings, UserPage_OrganizationDetail%>">
		<cmsUm:OrganizationControl ID="organizationControl" runat="server" CssRoundPanel="roundPanel"
			IsEditing="false" Width="100%" />
	</cms:RoundPanel>			
    <div style="text-align:right;">
        <asp:HyperLink runat="server" ID="hlBack" Text="<%$Resources:Strings, BackLink%>"></asp:HyperLink>
    </div>			
</asp:Content>
