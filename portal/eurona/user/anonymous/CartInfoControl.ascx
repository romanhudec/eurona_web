<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CartInfoControl.ascx.cs" Inherits="Eurona.User.Anonymous.CartInfoControl" %>
<div class="cartInfo">
    <div class="title"><asp:HyperLink runat="server" ID="hlCart" Text="NÁKUPNÍ KOŠÍK"></asp:HyperLink></div>
    <div>
        <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:EShopStrings, CartControl_ItemsCount %>"></asp:Literal>
        <asp:Label CssClass="items" ID="txtCount" runat="server" Text=""></asp:Label>
    </div>
    <div>
        <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:EShopStrings, CartControl_Price %>"></asp:Literal>
        <asp:Label CssClass="price" ID="txtPrice" runat="server"></asp:Label>
    </div>
    <asp:HyperLink runat="server" ID="hlImage" CssClass="image" >
<%--        <div class="image"></div>--%>
    </asp:HyperLink>
</div>