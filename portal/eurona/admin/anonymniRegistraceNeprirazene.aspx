<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="Eurona.Admin.AnonymniRegistraceNeprirazenePage" Codebehind="anonymniRegistraceNeprirazene.aspx.cs" %>

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
	<telerik:RadGrid AutoGenerateColumns="False" ID="gridView" AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True" runat="server">
        <PagerStyle Mode="NextPrevAndNumeric" />
        <GroupingSettings CaseSensitive="false" />
        <MasterTableView TableLayout="Fixed" PageSize = "50">
            <Columns>
                <telerik:GridHyperLinkColumn HeaderText="Reg. číslo" DataTextField="Code" DataNavigateUrlFields="AccountId"  UniqueName="Code" ItemStyle-Wrap="false" DataNavigateUrlFormatString="~/user/operator/user.aspx?id={0}&ReturnUrl=/user/operator/anonymousAccounts.aspx"
                    SortExpression="Code" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="true" />
                <telerik:GridBoundColumn HeaderText="Jméno" DataField="Name" UniqueName="Name"
                    SortExpression="Name" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
				<telerik:GridHyperLinkColumn HeaderText="Email" DataTextField="ContactEmail" UniqueName="ContactEmail"  DataNavigateUrlFields="ContactEmail" DataNavigateUrlFormatString="mailto:{0}"
                    SortExpression="ContactEmail" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="true" />
				<telerik:GridBoundColumn HeaderText="Datum registrace" DataField="AnonymousCreatedAt" UniqueName="AnonymousCreatedAt"
                    SortExpression="AnonymousCreatedAt" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="true" />
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</asp:Content>

