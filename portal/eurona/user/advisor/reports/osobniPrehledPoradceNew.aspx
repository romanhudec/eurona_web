<%@ Page Title="<%$ Resources:Reports, OsobniPrehledPoradce_Title %>" Language="C#" MasterPageFile="~/user/advisor/reports/report.master" AutoEventWireup="true" CodeBehind="osobniPrehledPoradceNew.aspx.cs" Inherits="Eurona.User.Advisor.Reports.OsobniPrehledPoradceReportNew" %>
<%@ Register src="osobniPrehledPoradceNew.ascx" tagname="osobniPrehledPoradceNew" tagprefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="filter_content" runat="server">
<table width="350px">
    <tr>
        <td align="right"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Reports, Advisor %>"></asp:Literal><i>(reg. číslo nebo jméno)</i></td>
        <td><asp:TextBox runat="server" ID="txtAdvisorCode"></asp:TextBox></td>
    </tr>
</table>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="server">
    <uc1:osobniPrehledPoradceNew ID="osobniPrehledPoradceNew" runat="server" />
</asp:Content>
