<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CountDownControl.ascx.cs" Inherits="Eurona.user.advisor.CountDownControl" %>
<div runat="server" id="divContainer">
    <table>
        <tr><td><asp:Label runat="server" ID="lblDnes" CssClass="dnes-info"></asp:Label></td></tr>
        <tr><td><asp:Label runat="server" ID="lblUzavierkaInfo" CssClass="uzavierka-info"></asp:Label></td></tr>
        <tr><td></td></tr>
        <tr><td><asp:Label runat="server" ID="lblZbyvaInfo" CssClass="zbyva-info"></asp:Label></td></tr>
        <tr><td><a class="cas-info" id="cnt_container"></a></td></tr>
    </table>
</div>