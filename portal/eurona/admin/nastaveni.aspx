<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeBehind="nastaveni.aspx.cs" Inherits="Eurona.Admin.EuronaNastaveniPage" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="server">
    <cms:RoundPanel ID="rp" runat="server" CssClass="roundPanel" Width="100%">
        <h2>Všeobecná nastavení</h2>
        <table style="padding:10px;" width="100%">
            <tr>
                <td colspan="2">
                    <h3>Nastavení emailu</h3>
                </td>
            </tr>
            <tr>
                <td valign="middle"><span style="white-space:nowrap;">Při vložení komentáře produktu : </span></td>
                <td>
                    <div>
                        <span style="font-style: italic;">(Emaily oddelte ';' např. jan@eurona.cz;ivan@eurona.cz)</span>
                        <asp:TextBox runat="server" ID="txtEmaliKomentar" Width="500px"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td valign="middle"><span style="white-space:nowrap;">Při vložení přispěvku diskuse : </span></td>
                <td>
                    <div>
                        <span style="font-style: italic;">(Emaily oddelte ';' např. jan@eurona.cz;ivan@eurona.cz)</span>
                        <asp:TextBox runat="server" ID="txtEmailPrispevek" Width="500px"></asp:TextBox>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <h3>Automatické vysypání všech košíků </h3>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div>
                        <asp:CheckBox runat="server" ID="cbECartPovelena" AutoPostBack="true" OnCheckedChanged="OnECartPovolenaChecked" Text="Povoleno" />
                        &nbsp;
                        <span style="white-space:nowrap;">Čas vysypání košíků : </span>
                        <asp:TextBox runat="server" ID="txtECartCas" Width="80px"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtECartCas" ErrorMessage="Čas musí být ve fomátu 00:00!" ValidationExpression="^(([0-1]?[0-9])|([2][0-3])):([0-5]?[0-9])?$">
                        </asp:RegularExpressionValidator>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <h3>Platba kartou </h3>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div>
                        <asp:CheckBox runat="server" ID="cbEPlatbaKartouPovelena" Text="Povoleno" Checked="true" />
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <h3>Limit pro platbu kartou </h3>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div>
                        <asp:TextBox runat="server" ID="txtCardPaymentLimit" Width="80px"></asp:TextBox> minut
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtCardPaymentLimit" ErrorMessage="Čas musí být ve fomátu čísla!" ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <h3>Združené obejdnávky</h3>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div>
                        <asp:CheckBox runat="server" ID="cbZdruzeneObjednavkyPovelena" Text="Povoleno" Checked="true" />
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <h3>Platba kartou združených obejdnávek</h3>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div>
                        <asp:CheckBox runat="server" ID="cbEPlatbaKartouZOPovelena" Text="Povoleno" Checked="true" />
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <h3>Limit platnosti cookie odkazu poradce </h3>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div>
                        <asp:CheckBox runat="server" ID="cbAccountsLinkCookieEnabled" Text="Povoleno" Checked="true" OnCheckedChanged="cbAccountsLinkCookieEnabled_CheckedChanged" AutoPostBack="true" />
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div>
                        <asp:TextBox runat="server" ID="txtAccountLinkCookiesLimit" Width="80px"></asp:TextBox> dnů
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtAccountLinkCookiesLimit" ErrorMessage="Počet dnů musí být ve fomátu čísla!" ValidationExpression="^[0-9]+$"></asp:RegularExpressionValidator>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button runat="server" ID="btnSave" Text="Uložit" OnClick="OnSave" />
                    <asp:Button runat="server" ID="btnCancel" Text="Zrušit" OnClick="OnCancel" CausesValidation="false" />
                </td>
            </tr>            
        </table>
    </cms:RoundPanel>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
</asp:Content>
