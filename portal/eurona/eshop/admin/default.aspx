<%@ Page Title="" Language="C#" MasterPageFile="~/eshop/admin/admin.master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Eurona.EShop.Admin.Default" %>
<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <b><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:EShopStrings, AdministrationMode_HeaderAdministration %>" /></b>
</asp:Content>
