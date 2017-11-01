<%@ Page Title="<%$ Resources:Strings, Navigation_MyAccount %>" Language="C#" MasterPageFile="~/user/host/page.master" AutoEventWireup="true" CodeBehind="account.aspx.cs" Inherits="Eurona.User.Host.AccountDetailPage" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>
<%@ Register Assembly="Eurona" Namespace="Eurona.Controls" TagPrefix="ctrls" %>

<%@ Register src="../../Controls/AccountControl.ascx" tagname="Account" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="banner" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
    <div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server">
			<asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span> <a id="A2" href="~/user/default.aspx" runat="server"></a>
		<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_MyAccount %>" />
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" runat="server">
	<cms:RoundPanel ID="rpAccount" runat="server" CssClass="roundPanel">
	    <uc1:Account ID="accountControl" runat="server" Width="400px"/>
	</cms:RoundPanel>
</asp:Content>
