<%@ Page Title="" Language="C#" MasterPageFile="~/user/operator/admin.master" AutoEventWireup="true" Inherits="Eurona.Operator.LoggedAccountsPage" Codebehind="loggedAccounts.aspx.cs" %>

<%@ Register Assembly="eurona" Namespace="Eurona.Controls" TagPrefix="cmsAccount" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<a id="A2" href="~/admin/default.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_Users %>" />
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
	<h2>Právě přihlášení poradci</h2>
	<telerik:RadGrid AutoGenerateColumns="False" ID="gridView" AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True" runat="server">
        <PagerStyle Mode="NextPrevAndNumeric" />
        <GroupingSettings CaseSensitive="false" />
        <MasterTableView TableLayout="Fixed" PageSize = "50">
            <Columns>
                <telerik:GridHyperLinkColumn HeaderText="Reg. čislo" DataTextField="Code" DataNavigateUrlFields="Id"  UniqueName="Code" ItemStyle-Wrap="false" DataNavigateUrlFormatString="~/user/operator/user.aspx?id={0}&ReturnUrl=/user/operator/loggedAccounts.aspx"
                    SortExpression="Code" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="true" />
                <telerik:GridBoundColumn HeaderText="Jméno" DataField="Name" UniqueName="Name"
                    SortExpression="Name" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="true" />
				<telerik:GridHyperLinkColumn HeaderText="Email" DataTextField="Email" UniqueName="Email"  DataNavigateUrlFields="Email" DataNavigateUrlFormatString="mailto:{0}"
                    SortExpression="Email" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="true" />
				<telerik:GridBoundColumn HeaderText="Přihlášen" DataField="LoggedMinutes" UniqueName="LoggedMinutes" ItemStyle-HorizontalAlign="Center" DataFormatString="{0} minut"
                    SortExpression="LoggedMinutes" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="true" />
				<telerik:GridCheckBoxColumn HeaderText="ATP člen" DataField="AngelTeamClen" UniqueName="AngelTeamClen"
                    SortExpression="AngelTeamClen" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="true" />
				<telerik:GridCheckBoxColumn HeaderText="ATP manager" DataField="AngelTeamManager" UniqueName="AngelTeamManager"
                    SortExpression="AngelTeamManager" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="true" />
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</asp:Content>

