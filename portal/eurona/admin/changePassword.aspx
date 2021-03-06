﻿<%@ Page Title="<%$ Resources:Strings, Navigation_ChangePassword %>" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true"
	CodeBehind="changePassword.aspx.cs" Inherits="Eurona.Admin.ChangePasswordPage" %>

<%@ Register Assembly="eurona" Namespace="Eurona.Controls" TagPrefix="cmsAcount" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server">
			<asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a> <span>&nbsp;-&nbsp;</span>
			<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_ChangePassword %>" />
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" runat="Server">
<cms:RoundPanel ID="rpChangePassword" runat="server" CssClass="roundPanel" Width="480px">
	<cmsAcount:ChangePasswordControl ID="changePassword" runat="server" Width="450px" />	
</cms:RoundPanel>
</asp:Content>
