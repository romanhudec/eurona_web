<%@ Page Title="<%$ Resources:Reports, DenniPrehledObjednavek_Title %>" Language="C#" MasterPageFile="~/user/advisor/reports/report.master" AutoEventWireup="true" CodeBehind="denniPrehledObjednavek.aspx.cs" Inherits="Eurona.User.Advisor.Reports.DenniPrehledObjednavek" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content2" ContentPlaceHolderID="filter_content" runat="server">
<style>
    .page-container {background-image:none!important;}
	.page-container-overlay{width:auto!important;}
	.page{width:auto!important;margin:0px 50px 0px 50px;}
    .page-container{width:auto!important;}
    .RadGrid .rgHeader{padding-left:5px!important;padding-right:0px!important;}
</style>
<table width="350px">
    <tr>
        <td align="left"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Reports, Advisor %>"></asp:Literal><i>(reg. číslo nebo jméno)</i></td>
        <td valign="top"><asp:TextBox runat="server" ID="txtAdvisorCode"></asp:TextBox></td>
    </tr>
    <tr>
        <td colspan="2"><asp:Label runat="server" ID="lblObdobi"></asp:Label></td>
    </tr>
</table>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="server">
    <telerik:RadGrid AutoGenerateColumns="False" ID="gridView" AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True" runat="server">
        <PagerStyle Mode="NextPrevAndNumeric" />
        <FilterItemStyle  />
        <GroupingSettings CaseSensitive="false" />
        <MasterTableView TableLayout="Fixed" AllowFilteringByColumn="false" PageSize = "50">
            <Columns>                
                <telerik:GridBoundColumn HeaderText="Kód" DataField="Kod_odberatele" UniqueName="Kod_odberatele" HeaderStyle-Width="100px"
                    SortExpression="Kod_odberatele" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />

                <telerik:GridBoundColumn HeaderText="Jméno" DataField="Nazev_firmy" UniqueName="Nazev_firmy" HeaderStyle-Width="80px"
                    SortExpression="Nazev_firmy" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />

                <telerik:GridHyperLinkColumn HeaderText="Č. objednávky" DataTextField="cislo_objednavky" DataType="System.String" DataNavigateUrlFields="id_prepoctu" UniqueName="cislo_objednavky" HeaderStyle-Width="80px" DataNavigateUrlFormatString="~/user/advisor/reports/Objednavka.aspx?id={0}" Target="_blank"
                    SortExpression="cislo_objednavky" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                
                <telerik:GridBoundColumn HeaderText="Body" DataField="celkem_body" UniqueName="celkem_body" HeaderStyle-Width="40px"
                    SortExpression="celkem_body" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F0}" />

                <telerik:GridBoundColumn HeaderText="Katalogová cena" DataField="celkem_katalogova_cena" UniqueName="celkem_katalogova_cena" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                    SortExpression="celkem_katalogova_cena" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F2}" />

                <telerik:GridBoundColumn HeaderText="Cena bez DPH" DataField="celkem_bez_dph" UniqueName="celkem_bez_dph" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                    SortExpression="celkem_bez_dph" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F2}" />
                
                <telerik:GridBoundColumn HeaderText="DPH" DataField="dph_zs" UniqueName="dph_zs" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                    SortExpression="dph_zs" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" AllowFiltering="false" ShowFilterIcon="false" DataFormatString="{0:F2}" />

                <telerik:GridBoundColumn HeaderText="Cena s DPH" DataField="celkem_k_uhrade" UniqueName="celkem_k_uhrade" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                    SortExpression="celkem_k_uhrade" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F2}" />

                <telerik:GridBoundColumn HeaderText="Objem obchodu" DataField="celkem_objem_obchodu" UniqueName="celkem_objem_obchodu" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                    SortExpression="celkem_objem_obchodu" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F2}" />

                <telerik:GridBoundColumn HeaderText="Objem pro marži" DataField="celkem_objem_pro_marzi" UniqueName="celkem_objem_pro_marzi" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                    SortExpression="celkem_objem_pro_marzi" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F2}" />
                
                <telerik:GridBoundColumn HeaderText="Datum" DataField="datum_vystaveni" UniqueName="datum_vystaveni" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:d}"
                    SortExpression="datum_vystaveni" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />

                <telerik:GridBoundColumn HeaderText="Stav objednávky" DataField="Stav_objednavky_nazev" UniqueName="Stav_objednavky_nazev" HeaderStyle-Width="110px"
                    SortExpression="Stav_objednavky_nazev" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                                    
                <telerik:GridBoundColumn HeaderText="Adresa" DataField="Adresa" UniqueName="Adresa" HeaderStyle-Width="220px"
                    SortExpression="Adresa" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
               
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</asp:Content>
