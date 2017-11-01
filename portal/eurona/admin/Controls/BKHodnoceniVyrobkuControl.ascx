<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BKHodnoceniVyrobkuControl.ascx.cs" Inherits="Eurona.admin.Controls.BKHodnoceniVyrobkuControl" %>
<table>
        <tr>
        <td colspan="7"><asp:CheckBox AutoPostBack="true" runat="server" ID="cbAktivni" Text="Aktivní" OnCheckedChanged="cbAktivni_CheckedChanged" /></td>
    </tr>
    <tr>
        <td style="white-space:nowrap;">Hodnocení Od :</td>
        <td>
            <asp:TextBox runat="server" ID="txtValueOd" Width="100"></asp:TextBox> %
            <asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtValueOd" MinimumValue="0" MaximumValue="99999" Type="Integer" ErrorMessage="*"></asp:RangeValidator>
        </td>
        <td style="white-space:nowrap;">Hodnocení Do :</td>
        <td>
            <asp:TextBox runat="server" ID="txtValueDo"  Width="100"></asp:TextBox> %
            <asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="txtValueDo" MinimumValue="0" MaximumValue="99999" Type="Integer" ErrorMessage="*"></asp:RangeValidator>
        </td>
        <td style="white-space:nowrap;">Kredit :</td>
        <td>
            <asp:TextBox runat="server" ID="txtKredit"></asp:TextBox>
            <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtKredit" MinimumValue="0" MaximumValue="99999" Type="Integer" ErrorMessage="*"></asp:RangeValidator>
        </td>
        <td><asp:Button runat="server" ID="btnSave" Text="Přidat" OnClick="OnAdd" /></td>
    </tr>
    <tr>
        <td colspan="7">
            <asp:GridView runat="server" ID="gridView" DataKeyNames="Id" OnRowCommand="OnRowCommand" AutoGenerateColumns="False" Width="100%" CellPadding="4" EnableModelValidation="True" ForeColor="#333333" GridLines="None">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField HeaderText="Hodnocení Od" DataField="HodnotaOd" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField HeaderText="Hodnocení Do" DataField="HodnotaDo" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField HeaderText="Kredit" DataField="Kredit" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                    <asp:TemplateField HeaderText="" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:ImageButton ID="lblDelete" ImageUrl="~/images/Trash.png" ForeColor="Red" runat="server" OnClientClick="return confirm('Opravdu si přejete vymazat tento záznam?');" CommandName="DELETE_ITEM" CommandArgument='<%#Eval("id")%>' />
                        </ItemTemplate>
                    </asp:TemplateField> 
                </Columns>
                <EditRowStyle BackColor="#2461BF" />
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#EFF3FB" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            </asp:GridView>
        </td>
    </tr>
</table>