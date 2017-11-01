<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BKFacebookControl.ascx.cs" Inherits="Eurona.admin.Controls.BKFacebookControl" %>
<table>
    <tr>
        <td colspan="2">
            <asp:CheckBox AutoPostBack="true" runat="server" ID="cbAktivni" Text="Aktivní" OnCheckedChanged="cbAktivni_CheckedChanged" /></td>
    </tr>
    <tr>
        <td>Počet kreditů :</td>
        <td>
            <asp:TextBox runat="server" ID="txtKredit"></asp:TextBox>
            <asp:RangeValidator runat="server" ControlToValidate="txtKredit" MinimumValue="0" MaximumValue="99999" Type="Integer" ErrorMessage="*"></asp:RangeValidator>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtKredit" ErrorMessage="*"></asp:RequiredFieldValidator>
        </td>
        <td>
            <asp:Button runat="server" ID="btnSave" Text="Uložit" OnClick="OnSave" /></td>
    </tr>
</table>
