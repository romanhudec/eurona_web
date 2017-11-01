﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BKMaximalniPocetZaMesicControl.ascx.cs" Inherits="Eurona.admin.Controls.BKMaximalniPocetZaMesicControl" %>
<table>
    <tr>
        <td colspan="2"><asp:CheckBox AutoPostBack="true" runat="server" ID="cbAktivni" Text="Aktivní" OnCheckedChanged="cbAktivni_CheckedChanged" /></td>
    </tr>
    <tr>
        <td>Maximální počet kreditů :</td>
        <td>
            <asp:TextBox runat="server" ID="txtKredit"></asp:TextBox>
            <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtKredit" MinimumValue="0" MaximumValue="99999" Type="Integer" ErrorMessage="*"></asp:RangeValidator>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtKredit" ErrorMessage="*"></asp:RequiredFieldValidator>
        </td>
        <td><asp:Button runat="server" ID="btnSave" Text="Uložit" OnClick="OnSave" /></td>
    </tr>
</table>