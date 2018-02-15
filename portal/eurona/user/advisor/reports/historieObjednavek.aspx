<%@ Page Title="<%$ Resources:Reports, HistorieObjednavek_Title %>" Language="C#" MasterPageFile="~/user/advisor/reports/report.master" AutoEventWireup="true" CodeBehind="historieObjednavek.aspx.cs" Inherits="Eurona.User.Advisor.Reports.HistorieObjednavek" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content2" ContentPlaceHolderID="filter_content" runat="server">
<table width="350px">
    <tr>
        <td align="right"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Reports, Advisor %>"></asp:Literal><i>(reg. číslo nebo jméno)</i></td>
        <td><asp:TextBox runat="server" ID="txtAdvisorCode"></asp:TextBox></td>
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
                <telerik:GridHyperLinkColumn HeaderText="Č. objednávky" DataTextField="id_web_objednavky" DataType="System.String" DataNavigateUrlFields="id_prepoctu" UniqueName="id_web_objednavky" HeaderStyle-Width="80px" DataNavigateUrlFormatString="~/user/advisor/reports/Objednavka.aspx?id={0}" Target="_blank"
                    SortExpression="id_web_objednavky" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                
                <telerik:GridBoundColumn HeaderText="Body" DataField="celkem_body" UniqueName="celkem_body" HeaderStyle-Width="40px"
                    SortExpression="celkem_body" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F0}" />

                <telerik:GridBoundColumn HeaderText="Katalogová cena" DataField="celkem_katalogova_cena" UniqueName="celkem_katalogova_cena" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                    SortExpression="celkem_katalogova_cena" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F2}" />

                <%--   
                <telerik:GridBoundColumn HeaderText="Cena bez DPH" DataField="celkem_bez_dph" UniqueName="celkem_bez_dph" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                    SortExpression="celkem_bez_dph" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F2}" />
                
                <telerik:GridBoundColumn HeaderText="DPH" DataField="dph_zs" UniqueName="dph_zs" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                    SortExpression="dph_zs" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" AllowFiltering="false" ShowFilterIcon="false" DataFormatString="{0:F2}" />
            --%>
                <telerik:GridBoundColumn HeaderText="Cena s DPH" DataField="celkem_k_uhrade" UniqueName="celkem_k_uhrade" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                    SortExpression="celkem_k_uhrade" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F2}" />

                <telerik:GridBoundColumn HeaderText="Objem obchodu" DataField="celkem_objem_obchodu" UniqueName="celkem_objem_obchodu" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                    SortExpression="celkem_objem_obchodu" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F2}" />

                <telerik:GridBoundColumn HeaderText="Objem pro marži" DataField="celkem_objem_pro_marzi" UniqueName="celkem_objem_pro_marzi" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                    SortExpression="celkem_objem_pro_marzi" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F2}" />
                
                <telerik:GridBoundColumn HeaderText="Datum" DataField="datum_vystaveni_objednavky" UniqueName="datum_vystaveni_objednavky" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:d}"
                    SortExpression="datum_vystaveni_objednavky" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />

                <telerik:GridBoundColumn HeaderText="Stav objednávky" DataField="Stav_objednavky_nazev" UniqueName="Stav_objednavky_nazev" HeaderStyle-Width="110px"
                    SortExpression="Stav_objednavky_nazev" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                                    
                <telerik:GridBoundColumn HeaderText="Město" DataField="dor_misto" UniqueName="dor_misto" HeaderStyle-Width="120px" AllowSorting="true"
                    SortExpression="dor_misto" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
               
                <telerik:GridBoundColumn HeaderText="PSČ" DataField="dor_psc" UniqueName="dor_psc" HeaderStyle-Width="80px" AllowSorting="true"
                    SortExpression="dor_psc" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />

            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</asp:Content>
