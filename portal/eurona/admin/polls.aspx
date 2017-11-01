<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="Eurona.Admin.Polls" Codebehind="polls.aspx.cs" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls.Poll" TagPrefix="cmsPoll" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<a id="A2" href="~/admin/default.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_Polls %>" />
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="Server">
	<cmsPoll:AdminPollsControl ID="AdminPollsControl" CssClass="dataGrid" runat="server"
		NewUrl="poll.aspx" DisplayUrlFormat="pollResult.aspx?Id={0}" EditUrlFormat="poll.aspx?Id={0}" OptionsUrlFormat="pollOptions.aspx?pollId={0}" />
</asp:Content>
