<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="Eurona.Admin.EmailsPage" Codebehind="emails.aspx.cs" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<a id="A2" href="~/admin/default.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal2" runat="server" Text="WEB Chyby" />
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
	<h2>Odeslané emaily</h2>
	<telerik:RadGrid AutoGenerateColumns="False" ID="gridView" AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True" runat="server">
        <PagerStyle Mode="NextPrevAndNumeric" />
        <GroupingSettings CaseSensitive="false" />
        <MasterTableView TableLayout="Fixed" PageSize = "50">
            <Columns>
				<telerik:GridBoundColumn HeaderText="Id" Visible="false" DataField="Id" UniqueName="Id"
                    SortExpression="Id" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="true" />
				<telerik:GridBoundColumn HeaderText="Email" DataField="Email" UniqueName="Email"
                    SortExpression="Email" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="true" />
                <telerik:GridBoundColumn HeaderText="Předmět" DataField="Subject" UniqueName="Subject" ItemStyle-Wrap="false"
                    SortExpression="Subject" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="true" />
<%--                <telerik:GridBoundColumn HeaderText="Správa" DataField="Message" UniqueName="Message" 
                    SortExpression="Message" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="true"  />--%>
				<telerik:GridBoundColumn HeaderText="Status" DataField="Status" UniqueName="Status"
                    SortExpression="Status" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="true" />
				<telerik:GridBoundColumn HeaderText="Chyba" DataField="Error" UniqueName="Error"
                    SortExpression="Error" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="true" />
				<telerik:GridBoundColumn HeaderText="Datum" DataField="Timestamp" UniqueName="Timestamp"
                    SortExpression="Timestamp" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="true" />
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</asp:Content>

