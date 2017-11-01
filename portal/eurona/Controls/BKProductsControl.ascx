<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BKProductsControl.ascx.cs" Inherits="Eurona.Controls.BKProductsControl" %>
<asp:Repeater ID="rpBKProducts" runat="server" >
    <HeaderTemplate>
        <table width="100%">
    </HeaderTemplate>

    <ItemTemplate>
        <tr>
            <td><img style="max-width:216px;max-height:120px;"src='<%# GetImageSrc( Eval( "Id" ) )%>' alt='produkt' /></td>
            <td style="color:#23408e;"><%#Eval("Name") %></td>
            <td style="white-space:nowrap;color:#23408e;padding-left:5px;padding-right:5px;"><%#((decimal)Eval("BonusovyKredit")).ToString("F0") %> BK</td>
            <td style="white-space:nowrap;color:#23408e;" >
                <asp:HiddenField runat="server" Value='<%#Eval( "Id" ) %>' /><asp:TextBox runat="server" Width="50px"></asp:TextBox> Ks
            </td>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
<div style="text-align:right;width:100%;">
    <asp:Button runat="server" ID="addToCart" Text="Přidat k objednávce" OnClick="OnAddCart" />
</div>