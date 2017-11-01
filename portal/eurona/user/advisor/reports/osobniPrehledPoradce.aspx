<%@ Page Title="<%$ Resources:Reports, OsobniPrehledPoradce_Title %>" Language="C#" MasterPageFile="~/user/advisor/reports/report.master" AutoEventWireup="true" CodeBehind="osobniPrehledPoradce.aspx.cs" Inherits="Eurona.User.Advisor.Reports.OsobniPrehledPoradceReport" %>
<%@ Register src="osobniPrehledPoradce.ascx" tagname="osobniPrehledPoradce" tagprefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="filter_content" runat="server">
<table width="350px">
    <tr>
        <td align="right"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Reports, Advisor %>"></asp:Literal><i>(reg. číslo nebo jméno)</i></td>
        <td><asp:TextBox runat="server" ID="txtAdvisorCode"></asp:TextBox></td>
    </tr>
</table>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="server">
    <uc1:osobniPrehledPoradce ID="osobniPrehledPoradce" runat="server" />
</asp:Content>
