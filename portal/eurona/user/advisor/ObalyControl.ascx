<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ObalyControl.ascx.cs" Inherits="Eurona.user.advisor.ObalyControl" %>
<div runat="server" id="divContainer">
    <script type="text/javascript">
    </script>
    <asp:Repeater id="rpObaly" runat="server" OnItemCommand="rpObaly_ItemCommand">
        <HeaderTemplate>
            <table>
        </HeaderTemplate>
        <AlternatingItemTemplate>
            <tr valign="center">
                <%--<td style="padding-left:10px"><a href='<%=Page.ResolveUrl("~/eshop/product.aspx?id=") %><%#Eval("Id")%>' ><%#Eval("Name")%></a></td>--%>
                <td style="padding-left:10px"><asp:LinkButton runat="server" CausesValidation="false" NavigateUrl='<%=Page.ResolveUrl("~/eshop/product.aspx?id=") %><%#Eval("Id")%>' Text='<%#Eval("Name")%>' CommandArgument='<%#Eval("Id") %>'></asp:LinkButton></td>
			    <td><asp:TextBox Enabled="true" ID="txtPocetKs" runat="server" Text=""  Width="25px" CommandArgument='<%#Eval("Id") %>' /></td>
                <td><asp:ImageButton runat="server" ID="btnPridat" CommandArgument='<%#Eval("Id") %>' ImageUrl="~/images/Refresh.png" CausesValidation="false"/></td>
        </AlternatingItemTemplate>
        <ItemTemplate>
                <%--<td style="padding-left:10px"><a href='<%=Page.ResolveUrl("~/eshop/product.aspx?id=") %><%#Eval("Id")%>' ><%#Eval("Name")%></a></td>--%>
            <td style="padding-left:10px"><asp:LinkButton CausesValidation="false" runat="server" NavigateUrl='<%=Page.ResolveUrl("~/eshop/product.aspx?id=") %><%#Eval("Id")%>' Text='<%#Eval("Name")%>' CommandArgument='<%#Eval("Id") %>'></asp:LinkButton></td>
			    <td><asp:TextBox Enabled="true" ID="txtPocetKs" runat="server" Text="" Width="25px" CommandArgument='<%#Eval("Id") %>' AutoPostBack="true" /></td>
                <td><asp:ImageButton runat="server" ID="btnPridat" CommandArgument='<%#Eval("Id") %>' ImageUrl="~/images/Refresh.png" CausesValidation="false"/></td>
            </tr>
        </ItemTemplate>
        <FooterTemplate></table></FooterTemplate>
    </asp:Repeater>  
</div>