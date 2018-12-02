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
<asp:Content ID="Content4" ContentPlaceHolderID="filter_buttons_content" runat="server">       
    <div style="margin-left:20px;">
        <div id="Div1" style="padding:3px; background-image:url('../../../images/activity_report_color_magenta.png');background-repeat:no-repeat;" runat="server"><span>N – nezaúčtovaná produkce</span></div>
    </div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="server">
    <telerik:RadGrid AutoGenerateColumns="False" ID="gridView" AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True" runat="server" OnItemDataBound="OnRowDataBound">
        <PagerStyle Mode="NextPrevAndNumeric" />
        <FilterItemStyle  />
        <GroupingSettings CaseSensitive="false" />
        <MasterTableView TableLayout="Fixed" AllowFilteringByColumn="true" PageSize = "50">
            <Columns>                
                <telerik:GridHyperLinkColumn HeaderText="Kód" DataTextField="Kod_odberatele" DataType="System.String" UniqueName="Kod_odberatele" HeaderStyle-Width="100px"
                    SortExpression="Kod_odberatele" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="true" />

                <telerik:GridBoundColumn HeaderText="Jméno" DataField="Dor_nazev_firmy" UniqueName="Dor_nazev_firmy" HeaderStyle-Width="100px"
                    SortExpression="Dor_nazev_firmy" AutoPostBackOnFilter="true" AllowFiltering="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />

                <telerik:GridTemplateColumn HeaderText="Č. objednávky" HeaderStyle-Width="80px"> 
                    <ItemTemplate>
                        <a ID="btnlnk" runat="server" href='<%# GetObjednavkaUrl(Eval("id_web_objednavky"))%>' target="_blank"><%# Eval("id_web_objednavky") %></a>
                    </ItemTemplate>
                </telerik:GridTemplateColumn>

                <telerik:GridBoundColumn HeaderText="Kód sp." DataField="Kod_odberatele_sponzor" UniqueName="Kod_odberatele_sponzor" HeaderStyle-Width="100px"
                    SortExpression="Kod_odberatele_sponzor" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                <telerik:GridBoundColumn HeaderText="<%$ Resources:Reports, JmenoSp_Column %>" DataField="Nazev_firmy_sponzor" UniqueName="Nazev_firmy_sponzor" HeaderStyle-Width="90px"
                    SortExpression="Nazev_firmy_sponzor" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                
                <telerik:GridBoundColumn HeaderText="Datum objednávky" DataField="datum_vystaveni" UniqueName="datum_vystaveni" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:d}"
                    SortExpression="datum_vystaveni" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                
                <telerik:GridBoundColumn HeaderText="Body" DataField="celkem_body" UniqueName="celkem_body" HeaderStyle-Width="40px"
                    SortExpression="celkem_body" AutoPostBackOnFilter="true" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F0}" />
                
             <%--   <telerik:GridBoundColumn HeaderText="Objem pro marži" DataField="celkem_objem_pro_marzi" UniqueName="celkem_objem_pro_marzi" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                    SortExpression="celkem_objem_pro_marzi" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F2}" />
                --%>
                <telerik:GridBoundColumn HeaderText="BO pro marži" DataField="celkem_bo_pro_marzi" UniqueName="celkem_bo_pro_marzi" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                    SortExpression="celkem_bo_pro_marzi" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F2}" />

                <telerik:GridBoundColumn HeaderText="Katalogová cena" DataField="celkem_katalogova_cena" UniqueName="celkem_katalogova_cena" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                    SortExpression="celkem_katalogova_cena" AutoPostBackOnFilter="true" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F2}" />
                
                <telerik:GridBoundColumn HeaderText="Cena s DPH" DataField="celkem_k_uhrade" UniqueName="celkem_k_uhrade" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                    SortExpression="celkem_k_uhrade" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F2}" />

                <telerik:GridBoundColumn HeaderText="Objem obchodu" DataField="celkem_objem_obchodu" UniqueName="celkem_objem_obchodu" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                    SortExpression="celkem_objem_obchodu" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F2}" />

                <telerik:GridBoundColumn HeaderText="Stav objednávky" DataField="Stav_objednavky_nazev" UniqueName="Stav_objednavky_nazev" HeaderStyle-Width="100px"
                    SortExpression="Stav_objednavky_nazev" AutoPostBackOnFilter="true" AllowFiltering="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                                    
                <telerik:GridBoundColumn HeaderText="Město" DataField="dor_misto" UniqueName="dor_misto" HeaderStyle-Width="120px" AllowSorting="true"
                    SortExpression="dor_misto" AutoPostBackOnFilter="true" AllowFiltering="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
               
                <telerik:GridBoundColumn HeaderText="PSČ" DataField="dor_psc" UniqueName="dor_psc" HeaderStyle-Width="40px" AllowSorting="true"
                    SortExpression="dor_psc" AutoPostBackOnFilter="true" AllowFiltering="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                 
                <telerik:GridBoundColumn HeaderText="N" DataField="Zauctovana" UniqueName="Zauctovana" HeaderStyle-Width="40px" HeaderTooltip="N – nezaúčtovaná produkce"
                SortExpression="Zauctovana" AutoPostBackOnFilter="true" AllowFiltering="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />

                <telerik:GridBoundColumn HeaderText="Top manager" DataField="top_manager" UniqueName="top_manager" HeaderStyle-Width="80px"
                    SortExpression="top_manager" AutoPostBackOnFilter="true" AllowFiltering="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
   
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</asp:Content>
