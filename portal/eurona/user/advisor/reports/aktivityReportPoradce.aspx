<%@ Page Title="<%$ Resources:Reports, AktivityReportPoradce_Title %>" Language="C#" MasterPageFile="~/user/advisor/reports/report.master" AutoEventWireup="true" CodeBehind="aktivityReportPoradce.aspx.cs" Inherits="Eurona.User.Advisor.Reports.AktivityReportPoradce" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content2" ContentPlaceHolderID="filter_content" runat="server">
    <style>
        .page-container {background-image:none!important;}
	    .page-container-overlay{width:auto!important;}
	    .page{width:auto!important;margin:0px 50px 0px 50px;}
        .page-container{width:auto!important;}
        .RadGrid .rgHeader{padding-left:5px!important;padding-right:0px!important;}
    </style>
    <table border="0" width="550px">
        <tr>
            <td><asp:RadioButton runat="server" ID="rbPrvniLinie" Text="<%$ Resources:Reports, PrvniLinie %>" GroupName="group" /></td>
            <td><asp:RadioButton runat="server" ID="rbSkupina" Text="<%$ Resources:Reports, CeleSkupiny %>" GroupName="group" /></td>
            <td><asp:CheckBox runat="server" ID="cbOsobniSkupiny" Text="<%$ Resources:Reports, OsobniSkupiny %>" /></td>
            <td align="right"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Reports, Advisor %>"></asp:Literal> <i>(reg. číslo nebo jméno)</i></td>
            <td><asp:TextBox runat="server" ID="txtAdvisorCode"></asp:TextBox></td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="filter_buttons_content" runat="server">       
    <div style="margin-left:20px;">
        <div style="padding:3px; background-image:url('../../../images/activity_report_color_green.png');background-repeat:no-repeat;" runat="server"><span>Nově registrovaný zákazník/poradce v daném období</span></div>
        <div style="padding:3px; background-image:url('../../../images/activity_report_color_yellow.png');background-repeat:no-repeat;" runat="server"><span>Historicky první postup na vyšši provizní hladinu</span></div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="filter_footer_content" runat="server">
    <div style="padding:0px;text-align:right;cursor:pointer">
        <asp:CheckBox ID="cbFilterPrvniHladinovePostupy" runat="server" Text="filtrovat historicky první hladinové postupy v daném období" AutoPostBack="true" OnCheckedChanged="cbFilterPrvniHladinovePostupy_CheckedChanged"></asp:CheckBox>
    </div> 
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="server">
    <telerik:RadGrid AutoGenerateColumns="False" ID="gridView" AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True" runat="server" OnItemDataBound="OnRowDataBound" >
        <PagerStyle Mode="NextPrevAndNumeric" />
        <FilterItemStyle  />
        <GroupingSettings CaseSensitive="false" />
        <MasterTableView TableLayout="Fixed" AllowFilteringByColumn="true" PageSize = "50">
            <Columns>
                <telerik:GridBoundColumn HeaderText="U" DataField="Vnoreni" UniqueName="Vnoreni" HeaderStyle-Width="20px"
                    SortExpression="Vnoreni" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" AllowFiltering="false" />
                
                <telerik:GridHyperLinkColumn HeaderText="Kód" DataTextField="Kod_odberatele" DataType="System.String" DataNavigateUrlFields="Id_odberatele,RRRRMM" UniqueName="Kod_odberatele" HeaderStyle-Width="100px" DataNavigateUrlFormatString="~/user/advisor/reports/aktivityReportPoradce.aspx?id={0}&obdobi={1}"
                    SortExpression="Kod_odberatele" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                <telerik:GridBoundColumn HeaderText="<%$ Resources:Reports, Jmeno_Column %>" DataField="Nazev_firmy" UniqueName="Nazev_firmy" HeaderStyle-Width="110px" ItemStyle-Wrap="true"
                    SortExpression="Nazev_firmy" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                <telerik:GridHyperLinkColumn HeaderText="Email" DataTextField="E_mail" DataNavigateUrlFields="E_mail"  UniqueName="E_mail" HeaderStyle-Width="110px" DataNavigateUrlFormatString="mailto:{0}"
                    SortExpression="E_mail" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                <telerik:GridBoundColumn HeaderText="<%$ Resources:Reports, Telefon_Column %>" DataField="Telefon" UniqueName="Telefon" HeaderStyle-Width="90px"
                    SortExpression="Telefon" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                <telerik:GridBoundColumn HeaderText="Kód sp." DataField="Kod_odberatele_sponzor" UniqueName="Kod_odberatele_sponzor" HeaderStyle-Width="100px"
                    SortExpression="Kod_odberatele_sponzor" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                <telerik:GridBoundColumn HeaderText="<%$ Resources:Reports, JmenoSp_Column %>" DataField="Nazev_firmy_sponzor" UniqueName="Nazev_firmy_sponzor" HeaderStyle-Width="90px"
                    SortExpression="Nazev_firmy_sponzor" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                
                <telerik:GridBoundColumn HeaderText="<%$ Resources:Reports, DatumReg_Column %>" DataField="Datum_zahajeni" UniqueName="Datum_zahajeni" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="70px"
                    SortExpression="Datum_zahajeni" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" AllowFiltering="false" ShowFilterIcon="false" DataFormatString="{0:d}"  />

                <telerik:GridBoundColumn HeaderText="Prov.hl." HeaderTooltip="Provizní hladina" DataField="Hladina" UniqueName="Hladina" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="40px"
                    SortExpression="Hladina" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" AllowFiltering="false" ShowFilterIcon="false" DataFormatString="{0:F0}" />

                <telerik:GridBoundColumn HeaderText="BO" HeaderTooltip="Bodový obrat" DataField="Body_vlastni" UniqueName="Body_vlastni" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Right"
                    SortExpression="Body_vlastni" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F0}" />
                <%--<telerik:GridBoundColumn HeaderText="OO" DataField="Objem_vlastni" UniqueName="Objem_vlastni" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Right"
                    SortExpression="Objem_vlastni" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F2}" />--%>
                <telerik:GridHyperLinkColumn HeaderText="BO sk." HeaderTooltip="Bodový obrat skupiny" DataTextField="Body_os" UniqueName="Body_os" DataNavigateUrlFields="Id_odberatele,RRRRMM"  HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Right"
                    SortExpression="Body_os" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataTextFormatString="{0:F0}" DataNavigateUrlFormatString="~/user/advisor/reports/osobniPrehledPoradce.aspx?id={0}&obdobi={1}" />

                <telerik:GridBoundColumn HeaderText="M" HeaderTooltip="Marže" DataField="Marze_platna" UniqueName="Marze_platna" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Right"
                    SortExpression="Marze_platna" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F2}" />
                <telerik:GridBoundColumn HeaderText="$$$" HeaderTooltip="Měna" DataField="Marze_kod_meny" UniqueName="Marze_kod_meny" HeaderStyle-Width="30px"
                    SortExpression="Marze_kod_meny" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                <telerik:GridBoundColumn HeaderText="Mn" HeaderTooltip="Předpokládaná marže následující měsíc" DataField="Marze_nasledujici" UniqueName="Marze_nasledujici" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Right"
                    SortExpression="Marze_nasledujici" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F2}" />
                <telerik:GridBoundColumn HeaderText="N" HeaderTooltip="zobrazuje 'A', pokud jde o nově reg. zákazníka" DataField="Novy" UniqueName="Novy" HeaderStyle-Width="30px"
                    SortExpression="Novy" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                <telerik:GridBoundColumn HeaderText="PN" HeaderTooltip="Počet nováčků" DataField="Pocet_novych" UniqueName="Pocet_novych" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Right"
                    SortExpression="Pocet_novych" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F0}" />
                <telerik:GridBoundColumn HeaderText="PO" HeaderTooltip="Počet obj. nově registrovaných zákazníků" DataField="Pocet_novych_s_objednavkou" UniqueName="Pocet_novych_s_objednavkou" HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Right"
                    SortExpression="Pocet_novych_s_objednavkou" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F0}" />
                <telerik:GridBoundColumn HeaderText="Mě" HeaderTooltip=" Počet měsíců bez objednání" DataField="Mesicu_bez_objednavky" UniqueName="Mesicu_bez_objednavky" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Right"
                    SortExpression="Mesicu_bez_objednavky" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F0}" />
                <telerik:GridBoundColumn HeaderText="Město" DataField="Misto" UniqueName="Misto" HeaderStyle-Width="100px" AllowSorting="true"
                    SortExpression="Misto" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                <telerik:GridBoundColumn HeaderText="PSČ" DataField="Psc" UniqueName="Psc" HeaderStyle-Width="60px" AllowSorting="true"
                    SortExpression="Psc" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" /> 
                 <telerik:GridBoundColumn HeaderText="US" DataField="UspesnyStart" UniqueName="UspesnyStart" HeaderStyle-Width="20px"
                    SortExpression="UspesnyStart" HeaderTooltip="Úspěšný start" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                 <telerik:GridBoundColumn HeaderText="Top manager" DataField="top_manager" UniqueName="top_manager" HeaderStyle-Width="80px"
                    SortExpression="top_manager" AutoPostBackOnFilter="false" AllowFiltering="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
    <div><hr /></div>
    <div style="background-color:#ffffff; padding:10px;">
        <asp:Label ID="lblHeader" ForeColor="#eb0a5b" runat="server" Text="<%$ Resources:Reports, GenerovatMujMesicniReportPDF %>"></asp:Label>
        <br /> <br />
        <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="False" 
            CellPadding="4" EnableModelValidation="True" ForeColor="#333333" 
            GridLines="None">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField HeaderText="<%$ Resources:Reports, Rok_Column %>" DataField="rok" />
                <asp:BoundField HeaderText="<%$ Resources:Reports, Mesic_Column %>" DataField="mesic" />
                <asp:TemplateField>
                <ItemTemplate>
                        <asp:HyperLink ID="hplReport" runat="server"  Text="<%$ Resources:Reports, Generovat %>" NavigateUrl='<%# String.Concat("~/user/advisor/reports/AktivityReportPDF.ashx?obdobi=",DataBinder.Eval(Container.DataItem, "RRRRMM"),"&id=",DataBinder.Eval(Container.DataItem, "Id_odberatele")) %>'></asp:HyperLink></b>
                        </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        </asp:GridView>
    </div>
</asp:Content>
