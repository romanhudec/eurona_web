<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CartInfoControl.ascx.cs" Inherits="Eurona.User.Host.CartInfoControl" %>
<div class="cartInfo">
    <div class="title"><asp:HyperLink runat="server" ID="hlCart" Text="<%$ Resources:EShopStrings, CartControl_Cart %>"></asp:HyperLink></div>
    <div>
        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:EShopStrings, CartControl_ItemsCount %>"></asp:Literal>
        <asp:Label CssClass="items" ID="txtCount" runat="server" Text=""></asp:Label>
    </div>
    <div>
        <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:EShopStrings, CartControl_Price %>"></asp:Literal>
        <asp:Label CssClass="price" ID="txtPrice" runat="server"></asp:Label>
    </div>
    <a runat="server" ID="hlCartImage">
        <div class="image"></div>
    </a>
</div>