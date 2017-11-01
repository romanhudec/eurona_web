<%@ Page Title="<%$ Resources:Strings, Navigation_AccountProfile %>" Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" Inherits="Eurona.AccountProfilePage" Codebehind="accountProfile.aspx.cs" %>

<%@ Register Assembly="Eurona" Namespace="Eurona.Controls" TagPrefix="ctrls" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_AccountProfile %>" />
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">
	<cms:RoundPanel ID="rpProfile" runat="server" CssClass="roundPanel" Text="<%$ Resources:Strings, Navigation_AccountProfile%>">
     <ctrls:AccountProfileControl runat="server" ID="accountProfileControl" />
    </cms:RoundPanel>
</asp:Content>

