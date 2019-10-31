<%@ Page Title="" Language="C#" MasterPageFile="~/eshop/admin/admin.master" AutoEventWireup="true" Inherits="Eurona.Admin.NastaveniZavozovehoMista" Codebehind="nastaveniZavozovehoMista.aspx.cs" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>

<asp:Content ID="content" ContentPlaceHolderID="content" Runat="Server">
    
    <style type="text/css">
        .bkItem{margin:5px;}
    </style>
    <cms:RoundPanel ID="rp" runat="server" CssClass="roundPanel" Width="100%">
        <div style="margin-bottom:10px;">
            <h2><asp:Literal ID="Literal1" runat="server" Text="Nastavení závozových míst" /></h2>
        </div>
        <div class="bkItem">
            <table>
                <tr>
                    <td style="white-space:nowrap;">Město :</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtMesto" Width="100"></asp:TextBox>
                    </td>
                    <td style="white-space:nowrap;">Datum :</td>
                    <td><cms:ASPxDatePicker runat="server" ID="dtpDatum" Width="80px" /></td>
                    <td>čas :</td>
                    <td><asp:TextBox runat="server" ID="txtCas" Width="40px"></asp:TextBox></td>
                    <td><asp:Button runat="server" ID="btnSave" Text="Přidat" OnClick="OnAdd" CausesValidation="true" /></td>
                </tr>
                <tr>
                    <td colspan="7">
                     <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCas" ErrorMessage="Čas musí být ve fomátu 00:00!" ValidationExpression="^(([0-1]?[0-9])|([2][0-3])):([0-5]?[0-9])?$">
                     </asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                    </td>
                </tr>
            </table>
            <asp:GridView runat="server" ID="gridView" DataKeyNames="Id" OnRowCommand="OnRowCommand" AutoGenerateColumns="False" Width="100%" CellPadding="4" EnableModelValidation="True" ForeColor="#333333" GridLines="None">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField HeaderText="Město" DataField="Mesto" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField HeaderText="Datum a čas" DataField="DatumACas" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
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
        </div>
      </cms:RoundPanel>
</asp:Content>

