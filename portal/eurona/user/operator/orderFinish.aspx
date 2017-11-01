<%@ Page Title="<%$ Resources:EShopStrings, OrderControl_Order %>" Language="C#" MasterPageFile="~/user/operator/admin.master" AutoEventWireup="true" CodeBehind="orderFinish.aspx.cs" Inherits="Eurona.User.Operator.OrderFinishPage" %>

<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <table>
        <tr>
            <td><h1><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, OrderFinish_Title %>"></asp:Literal></h1></td>
        </tr>
        <tr>
            <td><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, OrderFinish_Text %> "></asp:Literal>
                <asp:Hyperlink runat="server" ID="hlOrders" Text="<%$ Resources:EShopStrings, OrderControl_MyOrders %>">
                </asp:Hyperlink>
             </td>
        </tr>        
    </table>
</asp:Content>
