<%@ Page Title="" Language="C#" MasterPageFile="~/eshop/admin/admin.master" AutoEventWireup="true" CodeBehind="nastaveniObjednavky.aspx.cs" Inherits="Eurona.EShop.Admin.NastaveniObjednavky" %>
<%@ Register src="../../Controls/UzavierkaControl.ascx" tagname="UzavierkaControl" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="server">
<fieldset>
    <legend>Nastavení poštovného pro objednávky</legend>
    <table style="padding:10px;">
        <tr>
            <td colspan="2"><asp:CheckBox runat="server" ID="cbPovelena" AutoPostBack="true" OnCheckedChanged="OnPovolenaChecked" Text="Povolena" /></td>
        </tr>
        <tr>
            <td><br /></td>
        </tr>
        <tr>
            <td colspan="2"><b>Na objednávce nebude účtováno poštovné po dosažení katalogové ceny:</b></td>
        </tr>
        <tr>
            <td>CZ</td>
            <td>
                <div>
                    <asp:TextBox runat="server" ID="txtValueCS" Width="80px"></asp:TextBox> Kč
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtValueCS" ErrorMessage="Čas musí být ve fomátu čísla!" ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator>
                </div>
            </td>
        </tr>
        <tr>
            <td>SK</td>
            <td>
                <div>
                    <asp:TextBox runat="server" ID="txtValueSK" Width="80px"></asp:TextBox> Eur
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtValueSK" ErrorMessage="Čas musí být ve fomátu čísla!" ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator>
                </div>
            </td>
        </tr>
        <tr>
            <td>PL</td>
            <td>
                <div>
                    <asp:TextBox runat="server" ID="txtValuePL" Width="80px"></asp:TextBox> Zl
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtValuePL" ErrorMessage="Čas musí být ve fomátu čísla!" ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button runat="server" ID="btnSave" Text="Uložit změny" OnClick="OnSavePostovne" />
                <asp:Button runat="server" ID="btnCancel" Text="Zrušit" OnClick="OnCancelPostovne" CausesValidation="false" />
            </td>
        </tr>
        <tr>
            <td><br /></td>
        </tr>
    </table>
</fieldset>
<fieldset>
    <legend>Nastavení dopravce pro objednávky</legend>
    <table style="padding:10px;">
        <tr>
            <td colspan="2">
                <asp:Label runat="server" Text="Předvolený dopravce na objednávce :"></asp:Label>
                <asp:DropDownList runat="server" ID="ddlShipment" />
            </td>
        </tr>     
        <tr>
            <td colspan="2">
                <asp:Button runat="server" ID="Button1" Text="Uložit změny" OnClick="OnSaveDopravce" />
                <asp:Button runat="server" ID="Button2" Text="Zrušit" OnClick="OnCancelDopravce" CausesValidation="false" />
            </td>
        </tr>
        <tr>
            <td><br /></td>
        </tr>
    </table>
</fieldset>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
</asp:Content>
