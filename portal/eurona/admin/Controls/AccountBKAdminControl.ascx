<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccountBKAdminControl.ascx.cs" Inherits="Eurona.admin.Controls.AccountBKAdminControl" %>
<style>
    table td{white-space:nowrap;}
</style>
<table width="100%">
    <tr>
        <td style="white-space:nowrap;">Kredit :</td>
        <td>
            <asp:TextBox runat="server" ID="txtKredit"></asp:TextBox>
            <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtKredit" MinimumValue="0" MaximumValue="99999" Type="Integer" ErrorMessage="*"></asp:RangeValidator>
        </td>
        <td>Důvod :</td>
        <td>
            <asp:TextBox runat="server" ID="txtPoznamka" Width="200px"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPoznamka"></asp:RequiredFieldValidator>
        </td>
        <td>
            <asp:RadioButton runat="server"  ID="rbAktualniMesic" Text="Aktuální měsíc" GroupName="obdobi"/>          
            &nbsp;
            <asp:RadioButton runat="server"  ID="rbPristiMesic" Text="Příští měsíc" GroupName="obdobi"/>
        </td>
        <td><asp:Button runat="server" ID="btnSave" Text="Přidat" OnClick="OnAdd" /></td>
    </tr>
    <tr>
        <td colspan="6">
            <asp:GridView runat="server" ID="gridView" DataKeyNames="Id" OnRowCommand="OnRowCommand" AutoGenerateColumns="False" Width="100%" CellPadding="4" EnableModelValidation="True" ForeColor="#333333" GridLines="None">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField HeaderText="Datum" DataField="Datum" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField HeaderText="Důvod" DataField="Poznamka" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField HeaderText="Přidaný kredit" DataField="Hodnota" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField HeaderText="Platnost od" DataField="PlatnostOd" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField HeaderText="Platnost do" DataField="PlatnostDo" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
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