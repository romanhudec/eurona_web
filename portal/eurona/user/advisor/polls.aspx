<%@ Page Title="<%$ Resources:Strings, Navigation_Polls %>" Language="C#" MasterPageFile="~/user/advisor/page.master" AutoEventWireup="true" Inherits="Eurona.User.Advisor.Polls" Codebehind="polls.aspx.cs" %>

<%@ Register assembly="cms" namespace="CMS.Controls.Poll" tagprefix="cmsPoll" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/user/advisor/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_Polls %>" />
	</div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="content_header" runat="server">
    <div class="content-header-container">
        <asp:Label CssClass="title" ID="lblTitle" runat="server" Text="<%$ Resources:Strings, Navigation_Polls %>" />
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">
	<cmsPoll:ArchivedPollsControl ID="archivedPollsControl" CssClass="archivedPolls" CssPollClass="poll" runat="server" />
</asp:Content>

