<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CountDownControl.ascx.cs" Inherits="Eurona.User.Anonymous.CountDownControl" %>
<div runat="server" id="divContainer">
    <asp:Label runat="server" ID="lblDnes" CssClass="dnes-info">.</asp:Label>
    <asp:Label runat="server" ID="lblUzavierkaInfo" CssClass="uzavierka-info"></asp:Label>
    <asp:Label runat="server" ID="lblZbyvaInfo" CssClass="zbyva-info"></asp:Label>
    <a class="cas-info" id="cnt_container"></a>
</div>