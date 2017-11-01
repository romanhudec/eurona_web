<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UzavierkaControl.ascx.cs" Inherits="Eurona.Controls.UzavierkaControl" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>
<fieldset>
<legend>Nastavení uzávierky pro poradce</legend>
    <table style="padding:10px;">
        <tr>
            <td colspan="8"><asp:CheckBox runat="server" ID="cbPovelena" AutoPostBack="true" OnCheckedChanged="OnPovolenaChecked" Text="Uzávěrka povolena" /></td>
        </tr>
        <tr>
            <td colspan="8"><b>Předchozí uzávierka</b></td>
        </tr>
        <tr>
            <td>Datum od:</td>
            <td colspan="2"><asp:Label runat="server" ID="lblBeforeDatumOd"></asp:Label></td>
            <td>Datum do:</td>
            <td colspan="2"><asp:Label runat="server" ID="lblBeforeDatumDo"></asp:Label></td>
        </tr>
        <tr>
            <td colspan="8"><b>Aktuální uzávierka</b></td>
        </tr>
        <tr>
            <td>Datum od:</td>
            <td><cms:ASPxDatePicker runat="server" ID="dtpDatumOd" Width="80px" /></td>
            <td>čas :</td>
            <td><asp:TextBox runat="server" ID="txtCasOd" Width="40px"></asp:TextBox></td>
            <td>Datum do:</td>
            <td><cms:ASPxDatePicker runat="server" ID="dtpDatumDo" Width="80px"/></td>
            <td>čas :</td>
            <td><asp:TextBox runat="server" ID="txtCasDo" Width="40px"></asp:TextBox></td>
        </tr>
        <tr>
            <td colspan="4" align="right">
                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtCasOd" ErrorMessage="Čas musí být ve fomátu 00:00!" ValidationExpression="^(([0-1]?[0-9])|([2][0-3])):([0-5]?[0-9])?$">
                </asp:RegularExpressionValidator>
            </td>
            <td colspan="4" align="right">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCasDo" ErrorMessage="Čas musí být ve fomátu 00:00!" ValidationExpression="^(([0-1]?[0-9])|([2][0-3])):([0-5]?[0-9])?$">
                </asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td colspan="8">
                <asp:Button runat="server" ID="btnAdd" Text="Pridat uzávierku" OnClick="OnAddAndSave" />
                <asp:Button runat="server" ID="btnSave" Text="Uložit změny" OnClick="OnSave" Visible ="false" />
                <asp:Button runat="server" ID="btnCancel" Text="Zrušit" OnClick="OnCancel" CausesValidation="false" />
            </td>
        </tr>
    </table>
</fieldset>