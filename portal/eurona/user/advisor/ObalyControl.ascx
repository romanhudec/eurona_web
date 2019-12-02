<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ObalyControl.ascx.cs" Inherits="Eurona.user.advisor.ObalyControl" %>
<div runat="server" id="divContainer">
    <asp:Repeater id="rpObaly" runat="server" OnItemCommand="rpObaly_ItemCommand">
        <HeaderTemplate>
            <table>
        </HeaderTemplate>
        <AlternatingItemTemplate>
            <tr valign="center">
                <td style="padding-left:10px"><span><%#Eval("Name")%></span></td>
			    <td><asp:TextBox Enabled="true" ID="txtPocetKs" runat="server" Text=""  Width="25px"/></td>
                <td><asp:Button runat="server" ID="btnPridat" CommandArgument='<%#Eval("Id") %>' Text="+" CausesValidation="false"/></td>
        </AlternatingItemTemplate>
        <ItemTemplate>
                <td style="padding-left:10px"><span><%#Eval("Name")%></span></tdstyle="padding-left:10px>
			    <td><asp:TextBox Enabled="true" ID="txtPocetKs" runat="server" Text="" Width="25px"/></td>
                <td><asp:Button runat="server" ID="btnPridat" CommandArgument='<%#Eval("Id") %>' Text="+" CausesValidation="false"/></td>
            </tr>
        </ItemTemplate>
        <FooterTemplate></table></FooterTemplate>
    </asp:Repeater>  
</div>