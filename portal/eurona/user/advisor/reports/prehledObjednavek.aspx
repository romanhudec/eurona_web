<%@ Page Title="<%$ Resources:Reports, PrehledObjednavek_Title %>" Language="C#" MasterPageFile="~/user/advisor/reports/report.master" AutoEventWireup="true" CodeBehind="prehledObjednavek.aspx.cs" Inherits="Eurona.User.Advisor.Reports.PrehledObjednavek" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>

<asp:Content ID="Content2" ContentPlaceHolderID="filter_content" runat="server">
    <style>
        .page-container {background-image:none!important;}
	    .page-container-overlay{width:auto!important;}
	    .page{width:auto!important;margin:0px 50px 0px 50px;}
        .page-container{width:auto!important;}
        .RadGrid .rgHeader{padding-left:5px!important;padding-right:0px!important;}
        .pickerDateInputClass {width:100px!important;}
    </style>
    <table style="width:350px;">
    <tr>
        <td align="right" style="white-space:nowrap"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Reports, Advisor %>"></asp:Literal><i>(reg. číslo nebo jméno)</i></td>
        <td><asp:TextBox runat="server" ID="txtAdvisorCode"></asp:TextBox></td>
        <td style="white-space:nowrap">Datum od:</td>
        <td>
            <telerik:RadDatePicker runat="server" CssClass="rad-date-picker" ID="dtpDatumOd" >
                <DateInput ID="DateInput1" runat="server" CssClass="pickerDateInputClass"/>
            </telerik:RadDatePicker>
        </td>
        <td style="white-space:nowrap">Datum do:</td>
        <td>
            <telerik:RadDatePicker runat="server" CssClass="rad-date-picker" ID="dtpDatumDo" >
                <DateInput ID="DateInput2" runat="server" CssClass="pickerDateInputClass"/>
            </telerik:RadDatePicker>
        </td>
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
                <telerik:GridHyperLinkColumn HeaderText="Kód" DataTextField="Kod_odberatele" DataType="System.String" UniqueName="Kod_odberatele" HeaderStyle-Width="100px"
                    SortExpression="Kod_odberatele" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />

                <telerik:GridBoundColumn HeaderText="Jméno" DataField="Dor_nazev_firmy" UniqueName="Dor_nazev_firmy" HeaderStyle-Width="100px"
                    SortExpression="Dor_nazev_firmy" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />

                <telerik:GridHyperLinkColumn HeaderText="Č. objednávky" DataTextField="id_web_objednavky" DataType="System.String" DataNavigateUrlFields="id_prepoctu" UniqueName="id_web_objednavky" HeaderStyle-Width="80px" DataNavigateUrlFormatString="~/user/advisor/reports/Objednavka.aspx?id={0}" Target="_blank"
                    SortExpression="id_web_objednavky" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                
                <telerik:GridBoundColumn HeaderText="Datum objednávky" DataField="datum_vystaveni" UniqueName="datum_vystaveni" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:d}"
                    SortExpression="datum_vystaveni" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                
                <telerik:GridBoundColumn HeaderText="Body" DataField="celkem_body" UniqueName="celkem_body" HeaderStyle-Width="40px"
                    SortExpression="celkem_body" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F0}" />
                
                <telerik:GridBoundColumn HeaderText="Objem pro marži" DataField="celkem_objem_pro_marzi" UniqueName="celkem_objem_pro_marzi" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                    SortExpression="celkem_objem_pro_marzi" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F2}" />
                
                <telerik:GridBoundColumn HeaderText="Katalogová cena" DataField="celkem_katalogova_cena" UniqueName="celkem_katalogova_cena" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                    SortExpression="celkem_katalogova_cena" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F2}" />
                
                <telerik:GridBoundColumn HeaderText="Cena s DPH" DataField="celkem_k_uhrade" UniqueName="celkem_k_uhrade" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                    SortExpression="celkem_k_uhrade" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F2}" />

                <telerik:GridBoundColumn HeaderText="Objem obchodu" DataField="celkem_objem_obchodu" UniqueName="celkem_objem_obchodu" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                    SortExpression="celkem_objem_obchodu" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F2}" />

                <telerik:GridBoundColumn HeaderText="Stav objednávky" DataField="Stav_objednavky_nazev" UniqueName="Stav_objednavky_nazev" HeaderStyle-Width="100px"
                    SortExpression="Stav_objednavky_nazev" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                                    
                <telerik:GridBoundColumn HeaderText="Město" DataField="dor_misto" UniqueName="dor_misto" HeaderStyle-Width="80px"
                    SortExpression="dor_misto" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
               
                <telerik:GridBoundColumn HeaderText="PSČ" DataField="dor_psc" UniqueName="dor_psc" HeaderStyle-Width="40px"
                    SortExpression="dor_psc" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                 
                <telerik:GridBoundColumn HeaderText="Top manager" DataField="top_manager" UniqueName="top_manager" HeaderStyle-Width="80px"
                    SortExpression="top_manager" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />

             <%--   <telerik:GridBoundColumn HeaderText="Cena bez DPH" DataField="celkem_bez_dph" UniqueName="celkem_bez_dph" HeaderStyle-Width="90px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                    SortExpression="celkem_bez_dph" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F2}" />
                --%>
             <%--   <telerik:GridBoundColumn HeaderText="DPH" DataField="dph_zs" UniqueName="dph_zs" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                    SortExpression="dph_zs" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" AllowFiltering="false" ShowFilterIcon="false" DataFormatString="{0:F2}" />

                <telerik:GridBoundColumn HeaderText="Cena s DPH" DataField="celkem_k_uhrade" UniqueName="celkem_k_uhrade" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                    SortExpression="celkem_k_uhrade" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F2}" />--%>

            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</asp:Content>
