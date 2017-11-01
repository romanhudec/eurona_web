<%@ Page Title="<%$ Resources:Reports, ZruseniPoradci_Title %>" Language="C#" MasterPageFile="~/user/advisor/reports/report.master" AutoEventWireup="true" CodeBehind="zruseniPoradci.aspx.cs" Inherits="Eurona.User.Advisor.Reports.ZruseniPoradciReport" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content2" ContentPlaceHolderID="filter_content" runat="server">
<table width="250px">
    <tr>
        <td  style="white-space:nowrap;"><asp:Literal runat="server" ID="lblObdobi" Text="<%$ Resources:Reports, Obdobi %>"></asp:Literal><asp:TextBox runat="server" ID="txtObdobi" Width="60px"></asp:TextBox></td>
        <td><asp:CheckBox runat="server" ID="cbSkupina" Text="<%$ Resources:Reports, JenMojeSkupina %>" /></td>
    </tr>
</table>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="server">
    <telerik:RadGrid AutoGenerateColumns="False" ID="gridView" AllowFilteringByColumn="False" AllowPaging="True" AllowSorting="True" runat="server">
        <PagerStyle Mode="NextPrevAndNumeric" />
        <GroupingSettings CaseSensitive="false" />
        <MasterTableView TableLayout="Fixed" PageSize = "50">
            <Columns>
                <telerik:GridBoundColumn HeaderText="Kód" DataField="Kod_odberatele" UniqueName="Kod_odberatele" HeaderStyle-Width="100px"
                    SortExpression="Kod_odberatele" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                <telerik:GridBoundColumn HeaderText="<%$ Resources:Reports, Jmeno_Column %>" DataField="Nazev_firmy" UniqueName="Nazev_firmy" HeaderStyle-Width="120px"
                    SortExpression="Nazev_firmy" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                <telerik:GridBoundColumn HeaderText="<%$ Resources:Reports, Telefon_Column %>" DataField="Telefon" UniqueName="Telefon"  HeaderStyle-Width="90px"
                    SortExpression="Telefon" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                <telerik:GridHyperLinkColumn HeaderText="Email" DataTextField="E_mail" UniqueName="E_mail"  HeaderStyle-Width="90px" DataNavigateUrlFields="E_mail" DataNavigateUrlFormatString="mailto:{0}"
                    SortExpression="E_mail" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />                
                <telerik:GridBoundColumn HeaderText="Kód sp." DataField="Kod_odberatele_sponzor" UniqueName="Kod_odberatele_sponzor" HeaderStyle-Width="100px"
                    SortExpression="Kod_odberatele_sponzor" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                <telerik:GridBoundColumn HeaderText="<%$ Resources:Reports, JmenoSp_Column %>" DataField="Nazev_firmy_sponzor" UniqueName="Nazev_firmy_sponzor" HeaderStyle-Width="120px"
                    SortExpression="Nazev_firmy_sponzor" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                <telerik:GridBoundColumn HeaderText="<%$ Resources:Reports, TelefonSp_Column %>" DataField="Telefon_sponzor" UniqueName="Telefon_sponzor"  HeaderStyle-Width="90px"
                    SortExpression="Telefon_sponzor" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />

                <telerik:GridBoundColumn HeaderText="<%$ Resources:Reports, DatumReg_Column %>" DataField="Datum_zahajeni" UniqueName="Datum_zahajeni"  HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Right"
                    SortExpression="Datum_zahajeni" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:d}" />

                <telerik:GridBoundColumn HeaderText="<%$ Resources:Reports, DatumVymazu_Column %>" DataField="Datum_vymazu" UniqueName="Datum_vymazu"  HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Right"
                    SortExpression="Datum_vymazu" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:d}" />

            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</asp:Content>
