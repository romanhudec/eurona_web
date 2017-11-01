<%@ Page Title="" Language="C#" MasterPageFile="~/eshop/admin/admin.master" AutoEventWireup="true" CodeBehind="OdeslaneDatoveListy.aspx.cs" Inherits="Eurona.EShop.Admin.Reporty.OdeslaneDatoveListy" %>

<%@ Register Assembly="Eurona" Namespace="Eurona.Controls.Product" TagPrefix="shpProduct" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<a id="A2" href="~/eshop/admin/default.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:EShopStrings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal2" runat="server" Text="Odeslané datové listy" />
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div>
        <table>
            <tr>
                <td>Datum od:</td>
                <td><cms:ASPxDatePicker runat="server" ID="dtpDatumOd" Width="80px" /></td>
                <td>Datum do:</td>
                <td><cms:ASPxDatePicker runat="server" ID="dtpDatumDo" Width="80px"/></td>
                <td><asp:Button runat="server" ID="btnFilter" Text="Načítat data" OnClick="btnFilter_Click" /></td>
            </tr>
        </table>
    </div>
    <telerik:RadGrid AutoGenerateColumns="False" ID="gridView" AllowFilteringByColumn="false" AllowPaging="True" AllowSorting="True" runat="server">
		<PagerStyle Mode="NextPrevAndNumeric" />
		<GroupingSettings CaseSensitive="false" />
		<MasterTableView TableLayout="Fixed" PageSize="50" DataKeyNames="Id">
			<Columns>
				<telerik:GridBoundColumn HeaderText="Email" DataField="Email" UniqueName="Email" SortExpression="Email"  AllowFiltering="true" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                <telerik:GridBoundColumn HeaderText="Kód produktu" DataField="Info" UniqueName="Info" SortExpression="Info"  AllowFiltering="true" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                <telerik:GridBoundColumn HeaderText="Datum" DataField="Timestamp" UniqueName="Timestamp" SortExpression="Timestamp" DataFormatString="{0:d}"  AllowFiltering="true" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="false" />
			</Columns>
		</MasterTableView>
	</telerik:RadGrid>
</asp:Content>
