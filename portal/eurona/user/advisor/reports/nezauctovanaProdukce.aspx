<%@ Page Title="<%$ Resources:Reports, NezauctovanaProdukcevReportech_Title %>" Language="C#" MasterPageFile="~/user/advisor/reports/report.master" AutoEventWireup="true" CodeBehind="nezauctovanaProdukce.aspx.cs" Inherits="Eurona.User.Advisor.Reports.NezauctovanaProdukceReport" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content2" ContentPlaceHolderID="filter_content" runat="server">
<table width="200px">
    <tr>
        <td><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Reports, Advisor %>"></asp:Literal></td>
        <td><asp:TextBox runat="server" ID="txtAdvisorCode"></asp:TextBox></td>
    </tr>
</table>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="server">
    <telerik:RadGrid AutoGenerateColumns="False" ID="gridView" AllowFilteringByColumn="False" AllowPaging="True" AllowSorting="True" runat="server" Width="60%">
        <PagerStyle Mode="NextPrevAndNumeric" />
        <GroupingSettings CaseSensitive="false" />
        <MasterTableView TableLayout="Fixed" PageSize = "50">
            <Columns>
                <telerik:GridBoundColumn HeaderText="U" DataField="Vnoreni" UniqueName="Vnoreni" HeaderStyle-Width="20px"
                    SortExpression="Vnoreni" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                <telerik:GridBoundColumn HeaderText="Kód" DataField="Kod_odberatele" UniqueName="Kod_odberatele" HeaderStyle-Width="100px"
                    SortExpression="Kod_odberatele" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                <telerik:GridBoundColumn HeaderText="<%$ Resources:Reports, Jmeno_Column %>" DataField="Nazev_firmy" UniqueName="Nazev_firmy"
                    SortExpression="Nazev_firmy" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
                 <telerik:GridBoundColumn HeaderText="BO" DataField="Body_vlastni" UniqueName="Body_vlastni" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                    SortExpression="Body_vlastni" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F0}" />
                <telerik:GridBoundColumn HeaderText="OO" DataField="Objem_vlastni" UniqueName="Objem_vlastni" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                    SortExpression="Objem_vlastni" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F2}" />
                <telerik:GridBoundColumn HeaderText="<%$ Resources:Reports, ObjemProMarzi_Column %>" DataField="objem_pro_marzi" UniqueName="objem_pro_marzi" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                    SortExpression="objem_pro_marzi" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" DataFormatString="{0:F2}" />
                <telerik:GridBoundColumn HeaderText="stav" DataField="Stav" UniqueName="Stav" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                    SortExpression="Stav" AutoPostBackOnFilter="false" CurrentFilterFunction="Contains" ShowFilterIcon="false" />
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</asp:Content>
