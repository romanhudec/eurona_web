<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="Eurona.Admin.SingleUserCookieReportsPage" Codebehind="singleUserCookieReports.aspx.cs" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>
<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A2" href="~/default.aspx" runat="server">
			<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span> <a id="A3" href="~/admin/default.aspx" runat="server">
			<asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span> <a id="A4" href="~/admin/singleUserCookieReports.aspx" runat="server">
			<asp:Literal ID="Literal4" runat="server" Text="Výstupy odkazů poradců" /></a>
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <h2>Výstupy odkazů poradců</h2>
	<cms:RoundPanel ID="RoundPanel1" runat="server" CssClass="roundPanel">
		<div style="margin: 10px">
            <a id="A1" href="~/admin/reports/singleUserCookieReports1.aspx" runat="server">
				<asp:Literal ID="Literal1" runat="server" Text="Za vybrané období počet odeslaných cookie / uživatel" />
			</a>
			&nbsp;&nbsp;
            <a id="A5" href="~/admin/reports/singleUserCookieReports2.aspx" runat="server">
				<asp:Literal ID="Literal5" runat="server" Text="Za vybrané období počet dokončených registrací / uživatel" />
			</a>	            						
		</div>
	</cms:RoundPanel>
</asp:Content>
