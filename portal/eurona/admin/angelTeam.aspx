<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" Inherits="Eurona.Admin.AngelTeamPage" Codebehind="angelTeam.aspx.cs" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>
<asp:Content ID="content" ContentPlaceHolderID="content" Runat="Server">
    
    <cms:RoundPanel ID="rp" runat="server" CssClass="roundPanel" Width="100%">
        <table>
            <tr>
                <td>
                    <div style="margin-bottom:10px;">
                        <h2><asp:Literal ID="Literal1" runat="server" Text="Nastavení Angel Team Profesional" /></h2>
                    </div>                
                </td>
            </tr>
            <tr>
                <td>
                    <h3>
						<asp:Literal ID="Literal2" runat="server" Text="Vypnout ATP" /><asp:CheckBox runat="server" ID="cbDisableATP" />
						<asp:Label runat="server" ForeColor="Red" ID="lblDisableATPMessage" Text="(ATP je pro uživatele nedosupné!)"></asp:Label>
					</h3>
                </td>
            </tr>
            <tr>
                <td>
					<hr />  
                    <h3>Maximální počet zobrazení stránky ATP check-in za poslední 1 minutu</h3>
					<asp:TextBox runat="server" ID="txtMaxViewPerMinute" Width="50px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
					<hr />  
                    <h3>Zablokování přístupu (hodin) ATP check-in po překročeni max. počtu zobrazení za 1 minutu</h3>
					<asp:TextBox runat="server" ID="txtBlockATPHours" Width="50px"></asp:TextBox><span>&nbsp;hodin</span>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
						<hr />  						
                        <h3>Počet Eurona Star nutných pro vstup do V.I.P. Angel teamu Professional </h3>
                        <asp:TextBox runat="server" ID="txtPocetProVstup"></asp:TextBox>
                    </div>                
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <hr />        
                        <h3>Počet Eurona Star nutných pro udržení ve V.I.P. Angel teamu Professional</h3> 
                        <asp:TextBox runat="server" ID="txtPocetProUdrzeni"></asp:TextBox>
                    </div>                
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <asp:Button runat="server" ID="btnSave" Text="Uložit" OnClick="OnSave" />
                    <asp:Button runat="server" ID="btnCancel" Text="Zrušit" OnClick="OnCancel" CausesValidation="false" />
                </td>
            </tr>
        </table>
    </cms:RoundPanel>
</asp:Content>

