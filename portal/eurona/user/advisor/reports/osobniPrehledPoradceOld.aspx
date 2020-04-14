<%@ Page Title="<%$ Resources:Reports, OsobniPrehledPoradce_Title %>" Language="C#" MasterPageFile="~/user/advisor/reports/report.master" AutoEventWireup="true" CodeBehind="osobniPrehledPoradceOld.aspx.cs" Inherits="Eurona.User.Advisor.Reports.OsobniPrehledPoradceReportOld" %>
<%@ Register src="osobniPrehledPoradceOld.ascx" tagname="osobniPrehledPoradceOld" tagprefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="filter_content" runat="server">
<table width="350px">
    <tr>
        <td align="right"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Reports, Advisor %>"></asp:Literal><i>(reg. číslo nebo jméno)</i></td>
        <td><asp:TextBox runat="server" ID="txtAdvisorCode"></asp:TextBox></td>
    </tr>
</table>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="server">
    <uc1:osobniPrehledPoradceOld ID="osobniPrehledPoradceOld" runat="server" />
</asp:Content>
