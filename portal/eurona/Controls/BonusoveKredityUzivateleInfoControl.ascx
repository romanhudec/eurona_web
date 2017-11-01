<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BonusoveKredityUzivateleInfoControl.ascx.cs" Inherits="Eurona.Controls.BonusoveKredityUzivateleInfoControl" %>
<style type="text/css">
    .bk-table td{color:#818287;}
    .bk-table .bk-td, .bk-table .bk-td span{color:#818287;}
    
    .bksumar-table td{color:#818287;}
    .bksumar-table .bksumar-td, .bksumar-table .bksumar-td span{color:#818287;}    
</style>
<div><span style="color:#23408E; margin-top:30px;margin-left:30px; font-size:30px; font-weight:bold; position:absolute;" >Bonusové kredity (BK)</span></div>
<img runat="server" src="~/images/BK_uzivatele_info.jpg" style="width:100%"/>
<%--<asp:Repeater ID="rpBKUzivatele" runat="server" >
    <HeaderTemplate>
        <table class="bk-table" border="0" cellpadding="8" cellspacing="0" width="100%">
            <tr>
                <td></td>
                <td align="right" style="color:#23408E!important; width:70px;">Počet BK</td>
                <td align="right" style="color:#23408E!important; width:70px;">KS</td>
                <td align="right" style="color:#23408E!important; width:70px;">Celkem</td>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
            <tr>
                <td align="left"><%#Eval("Nazev") %></td>
                <td align="right"><%#Eval("Pocet") %></td>
                <td align="right"><%#Eval("Ks") %></td>
                <td align="right"><%#Eval("Celkem") %></td>
            </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>--%>
<hr />
<br />
<%if(Eurona.Security.IsInRole( Eurona.Common.DAL.Entities.Role.ADVISOR)){ %>
<span style="color:#23408E; font-size:16px;font-weight:bold;" >PŘEHLED MÝCH BONUSOVÝCH KREDITŮ (BK)</span>
<%}%>
<table class="bksumar-table">
    <tr>
        <td style="color:#23408E!important;">Tento měsíc bylo dosaženo :</td>
        <td>
            <asp:Label runat="server" ID="lblDosazenoTentoMesic" ForeColor="#818287"></asp:Label> BK 
            <span style="font-style:italic;font-size:12px;">(platné od příštího měsíce.)</span>
        </td>
    </tr>
    <tr>
        <td style="color:#23408E!important;">Platných na tento měsíc :</td>
        <td>
            <asp:Label runat="server" ID="lblPlatnychTentoMesic" ForeColor="#818287"></asp:Label> BK 
            <span style="font-style:italic; font-size:12px;">(Nelze převádět body do dalšího měsíce. BK je nutné vyčerpat v rámci aktuálního měsíce.)</span>
        </td>
    </tr>
   <tr>
        <td style="color:#23408E!important;">Tento měsíc bylo čerpáno :</td>
        <td><asp:Label runat="server" ID="lblCerpanoTentoMesicCelkem" ForeColor="#818287"></asp:Label> BK</td>
    </tr>
    <tr>
        <td style="color:#23408E!important;">K dispozici na tento měsíc zbývá :</td>
        <td><asp:Label runat="server" ID="lblZbyvaCelkemTentoMesic" ForeColor="#818287"></asp:Label> BK</td>
    </tr>
</table>
