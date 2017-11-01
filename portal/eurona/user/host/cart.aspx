<%@ Page Title="<%$ Resources:EShopStrings, CartControl_Cart %>" Language="C#" MasterPageFile="~/user/Host/page.master" AutoEventWireup="true" CodeBehind="cart.aspx.cs" Inherits="Eurona.User.Host.CartPage" %>

<%@ Register Assembly="Eurona" Namespace="Eurona.Controls" TagPrefix="ctrls" %>
<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <ctrls:HostCartControl runat="server" ID="cartControl" CssClass="dataGrid" FinishUrlFormat="~/user/advisor/register.aspx" />
</asp:Content>
