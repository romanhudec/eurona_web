<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="Eurona.Admin.WebErrorsPage" Codebehind="webErrors.aspx.cs" %>

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
	<h2>Web Chyby</h2>
	<telerik:RadGrid AutoGenerateColumns="False" ID="gridView" AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True" runat="server">
        <PagerStyle Mode="NextPrevAndNumeric" />
        <GroupingSettings CaseSensitive="false" />
        <MasterTableView TableLayout="Fixed" PageSize = "50">
            <Columns>
				<telerik:GridBoundColumn HeaderText="Id" Visible="false" DataField="Id" UniqueName="Id"
                    SortExpression="Id" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="true" />
				<telerik:GridBoundColumn HeaderText="Datum" DataField="Stamp" UniqueName="Stamp"
                    SortExpression="Stamp" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="true" />
                <telerik:GridBoundColumn HeaderText="Reg.číslo" DataField="Code" UniqueName="Code" ItemStyle-Wrap="false"
                    SortExpression="Code" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="true" />
                <telerik:GridBoundColumn HeaderText="Jméno" DataField="Name" UniqueName="Name"
                    SortExpression="Name" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="true" />
				<telerik:GridBoundColumn HeaderText="Chyba" DataField="Exception" UniqueName="Exception"
                    SortExpression="Exception" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="true" />
				<telerik:GridBoundColumn HeaderText="Stránka" DataField="Location" UniqueName="Location"
                    SortExpression="Location" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="true" />
                <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-HorizontalAlign="Right">
					<ItemTemplate>
						<a href='webErrorDetail.aspx?id=<%#Eval("Id") %>' >Detail chyby</a>
					</ItemTemplate>
				</telerik:GridTemplateColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</asp:Content>

