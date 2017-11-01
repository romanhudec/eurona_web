<%@ Page Title="<%$ Resources:Reports, PrehledProdukceSkupiny_Title %>" Language="C#" MasterPageFile="~/user/advisor/reports/report.master" AutoEventWireup="true" CodeBehind="prehledProdukceSkupiny.aspx.cs" Inherits="Eurona.User.Advisor.Reports.PrehledProdukceSkupinyReport" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content2" ContentPlaceHolderID="filter_content" runat="server">
<table width="750px">
    <tr>
        <td><asp:RadioButton runat="server" ID="rbPrvniLinie" Text="<%$ Resources:Reports, PrvniLinie %>" GroupName="group" /></td>
        <td><asp:RadioButton runat="server" ID="rbSkupina" Text="<%$ Resources:Reports, CeleSkupiny %>" GroupName="group" /></td>
        <td><asp:RadioButton runat="server" ID="rbBez21PodrizenychSkupin" Text="<%$ Resources:Reports, Bez21Skupin %>" GroupName="group" /></td>
		<td><asp:CheckBox runat="server" ID="cbBez21" Text="<%$ Resources:Reports, Bez21Poradcu %>" GroupName="group" /></td>        
        <td align="right"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Reports, Advisor %>"></asp:Literal> <i>(reg. číslo nebo jméno)</i></td>
        <td><asp:TextBox runat="server" ID="txtAdvisorCode"></asp:TextBox></td>
    </tr>
</table>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="server">
    <telerik:RadGrid AutoGenerateColumns="False" ID="gridView" AllowFilteringByColumn="True" AllowPaging="True" AllowSorting="True" runat="server">
        <PagerStyle Mode="NextPrevAndNumeric" />
        <GroupingSettings CaseSensitive="false" />
        <MasterTableView TableLayout="Fixed" PageSize = "50">
            <Columns>
                <telerik:GridHyperLinkColumn HeaderText="Kód" DataTextField="Kod_odberatele" DataType="System.String" DataNavigateUrlFields="Id_odberatele,RRRRMM" UniqueName="Kod_odberatele" HeaderStyle-Width="100px" DataNavigateUrlFormatString="~/user/advisor/reports/prehledProdukceSkupiny.aspx?id={0}&obdobi={1}"
                    SortExpression="Kod_odberatele" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                <telerik:GridHyperLinkColumn HeaderText="<%$ Resources:Reports, Jmeno_Column %>" DataTextField="Nazev_firmy" DataNavigateUrlFields="E_mail"  UniqueName="Nazev_firmy" HeaderStyle-Width="110px" DataNavigateUrlFormatString="mailto:{0}"
                    SortExpression="Nazev_firmy" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                <telerik:GridBoundColumn HeaderText="<%$ Resources:Reports, Telefon_Column %>" DataField="Telefon" UniqueName="Telefon" HeaderStyle-Width="150px"
                    SortExpression="Telefon" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                
                <telerik:GridBoundColumn HeaderText="Prov.hl." DataField="Hladina" UniqueName="Hladina"
                    SortExpression="Hladina" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F0}" />
                <telerik:GridBoundColumn HeaderText="<%$ Resources:Reports, RozdilHl_Column %>" DataField="Hladina_rozdil" UniqueName="Hladina_rozdil"
                    SortExpression="Hladina_rozdil" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F0}" />

                <telerik:GridHyperLinkColumn HeaderText="BO sk." DataTextField="Body_os" DataType="System.Decimal" DataNavigateUrlFields="Id_odberatele,RRRRMM" UniqueName="Body_os" HeaderStyle-Width="100px" DataNavigateUrlFormatString="~/user/advisor/reports/osobniPrehledPoradce.aspx?id={0}&obdobi={1}"
                    SortExpression="Body_os" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                <telerik:GridBoundColumn HeaderText="OO sk." DataField="Objem_os" UniqueName="Objem_os"
                    SortExpression="Objem_os" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F2}" />

                <telerik:GridBoundColumn HeaderText="Adresa" DataField="Adresa" UniqueName="Adresa" HeaderStyle-Width="300px"
                    SortExpression="Adresa" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                <telerik:GridBoundColumn HeaderText="PSČ" DataField="Psc" UniqueName="Psc" HeaderStyle-Width="50px"
                    SortExpression="Adresa" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>

</asp:Content>
