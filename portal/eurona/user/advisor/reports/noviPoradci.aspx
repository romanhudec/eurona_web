<%@ Page Title="<%$ Resources:Reports, NoviPoradci_Title %>" Language="C#" MasterPageFile="~/user/advisor/reports/report.master" AutoEventWireup="true" CodeBehind="noviPoradci.aspx.cs" Inherits="Eurona.User.Advisor.Reports.NoviPoradciReport" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content2" ContentPlaceHolderID="filter_content" runat="server">
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
                <telerik:GridBoundColumn HeaderText="<%$ Resources:Reports, Datum1Obj_Column %>" DataField="Datum_zahajeni" UniqueName="Datum_zahajeni"  HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Right"
                    SortExpression="Datum_zahajeni" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:d}" />

                <telerik:GridBoundColumn HeaderText="BO sk." DataField="Body_os" UniqueName="Body_os" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="30px"
                    SortExpression="Body_os" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F0}" />
                <telerik:GridBoundColumn HeaderText="OO sk." DataField="Objem_os" UniqueName="Objem_os" ItemStyle-HorizontalAlign="Right"  HeaderStyle-Width="60px"
                    SortExpression="Objem_os" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F2}" />

                <telerik:GridBoundColumn HeaderText="BO 1" DataField="Body_1" UniqueName="Body_1" ItemStyle-HorizontalAlign="Right"
                    SortExpression="Body_1" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                <telerik:GridBoundColumn HeaderText="BO 2" DataField="Body_2" UniqueName="Body_2" ItemStyle-HorizontalAlign="Right"
                    SortExpression="Body_2" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                <telerik:GridBoundColumn HeaderText="BO 3" DataField="Body_3" UniqueName="Body_3" ItemStyle-HorizontalAlign="Right"
                    SortExpression="Body_3" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</asp:Content>
