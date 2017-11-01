<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeBehind="singleUserCookieReports2.aspx.cs" Inherits="Eurona.Admin.Reports.SingleUserCookieReports2Page" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
    <div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<a id="A2" href="~/admin/default.aspx" runat="server"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<a id="A3" href="~/admin/singleUserCookieReports.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="Výstupy odkazů poradců" /></a>
        <span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal4" runat="server" Text="Za vybrané období počet dokončených registrací / uživatel" />
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <h2>Report - a vybrané období počet dokončených registrací / uživatel</h2>
    <table>
        <tr>
            <td  style="white-space:nowrap;"><asp:Literal runat="server" ID="lblObdobi" Text="<%$ Resources:Reports, Obdobi %>"></asp:Literal><asp:TextBox runat="server" ID="txtObdobi" Width="60px"></asp:TextBox></td>
            <td style="width:100%;"><asp:Button runat="server" ID="btnGenerateReport" Text="<%$ Resources:Reports, Generovat %>" OnClick="OnGenearte"></asp:Button></td>
        </tr>
    </table>
    <telerik:RadGrid AutoGenerateColumns="False" ID="gridView" AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True" runat="server">
        <PagerStyle Mode="NextPrevAndNumeric" />
        <GroupingSettings CaseSensitive="false" />
        <MasterTableView TableLayout="Fixed" PageSize = "50">
            <Columns>
               <telerik:GridBoundColumn HeaderText="Období" DataField="Obdobi" UniqueName="Obdobi"
                    SortExpression="Obdobi" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="true" />
                <telerik:GridBoundColumn HeaderText="Registrační číslo" DataField="RegistrationCode" UniqueName="RegistrationCode"
                    SortExpression="RegistrationCode" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="true" />
                <telerik:GridBoundColumn HeaderText="Jméno a příjmení" DataField="Name" UniqueName="Name"
                    SortExpression="Name" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="true" />
                <telerik:GridBoundColumn HeaderText="Dokončených registrací" DataField="Pocet" UniqueName="Pocet"
                   SortExpression="Pocet" AutoPostBackOnFilter="true" CurrentFilterFunction="EqualTo" ShowFilterIcon="true" />
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</asp:Content>
