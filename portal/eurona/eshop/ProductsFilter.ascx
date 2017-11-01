<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductsFilter.ascx.cs" Inherits="Eurona.EShop.ProductsFilter" %>
<table width="100%">
    <tr>
        <td>Výraz</td>
        <td>Výrobce</td>
        <td>Cena od</td>
        <td>Cena do</td>
    </tr>
    <tr>
        <td><asp:TextBox runat="server" ID="txtExpression"></asp:TextBox></td>
        <td><asp:TextBox runat="server" ID="txtManufacturer"></asp:TextBox></td>
        <td><asp:TextBox runat="server" ID="txtPriceFrom"></asp:TextBox></td>
        <td><asp:TextBox runat="server" ID="txtPriceTo"></asp:TextBox></td>
    </tr>
    <tr>
        <td colspan="3">Seřadit podle : <asp:DropDownList runat="server" ID="ddlSortBy" Width="200px"></asp:DropDownList></td>
        <td align="right">
            <asp:Button ID="btnFilter" runat="server" Text="Filtruj" onclick="btnFilter_Click" />
        </td>
    </tr>
    <tr>
        <td></td>
        <td></td>
        <td></td>
        <td align="right">
            <asp:Button ID="btnCancelFilter" runat="server" Text="Zrušit" onclick="btnCancelFilter_Click" />
        </td>
    </tr>        
</table>