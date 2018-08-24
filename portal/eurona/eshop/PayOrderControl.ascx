<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PayOrderControl.ascx.cs" Inherits="Eurona.EShop.PayOrderControl" %>
<script type="text/javascript">
    function onCountDownEndFunction() {
        var button = document.getElementById('<%=this.btnPay.ClientID%>');
        button.disabled = true;
        button.className = 'button-uhrada-kartou-disabled';
        button.value = '';
    };
</script>
<table class="payOrderControl" runat="server" id="payOrderInput">
    <tr>
        <td colspan="2" align="center">
            <span style="color:#eb0a5b;font-size:18px;">
                <asp:Literal ID="lblHeader" runat="server" Text="<%$ Resources:EShopStrings, PayOrderControl_OrderHeader%>"></asp:Literal>
            </span>
        </td>
    </tr>
    <tr>
        <%--<td class="label"><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, PayOrderControl_OrderIsNotPayed%>"></asp:Literal></td>--%>
        <td align="center" valign="top">
			<asp:Button CssClass="button-uhrada-dobirkou" runat="server" ID="Button1" Text="<%$ Resources:EShopStrings, PayOrderControl_UhradaDobirkou%>" OnClick="OnUhradaDobirkou" />
		</td>
        <td align="center" valign="top">
        <%if (!Eurona.Common.DAL.Entities.Settings.IsPlatbaKartouPovolena()) { %>
			<asp:Button CssClass="button-uhrada-kartou-disabled" runat="server" ID="btnPayDisabled" Text="" Enabled="false" />
        <%}else{ %>
            <div>
                <asp:Button CssClass="button-uhrada-kartou" runat="server" ID="btnPay" Text="<%$ Resources:EShopStrings, PayOrderControl_UhradaKartou%>" OnClick="OnPayNow" />
                <div id="payLimit" runat="server">
                     <asp:Literal ID="Literal3" runat="server" Text="Úhrada katou bude dostupná ješte :">
                        <a class="cas-info" style="font-size:18px;" id="cnt_container"></a>
                     </asp:Literal>
                </div>
            </div>
        <%}%>
       <%--     <div style="color:#868686" ><asp:Literal Visible="true" ID="lblPlatbaKarout4ZdruzeneObjednavkyMessage" runat="server" Text="<%$ Resources:EShopStrings, OrderFinish_NelzeVyuzitProSdruzeneObjednavky %>"></asp:Literal></div>
            <div style="color:#868686" ><asp:Literal Visible="false" ID="lblCelkovaCenaPridruzeni" runat="server"></asp:Literal></div>--%>
		</td>
    </tr>
</table>
<div runat="server" id="payOrderResult" style="color:#f00;font-weight:bold;">
</div>