<%@ Page Title="" Language="C#" MasterPageFile="~/eshop/admin/admin.master" AutoEventWireup="true" CodeBehind="uzavierka.aspx.cs" Inherits="Eurona.EShop.Admin.EuronaUzavierka" %>
<%@ Register src="../../Controls/UzavierkaControl.ascx" tagname="UzavierkaControl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="server">
    <uc1:UzavierkaControl ID="uzavierkaControl" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
</asp:Content>
