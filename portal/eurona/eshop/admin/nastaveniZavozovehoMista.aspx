<%@ Page Title="" Language="C#" MasterPageFile="~/eshop/admin/admin.master" AutoEventWireup="true" Inherits="Eurona.Admin.NastaveniZavozovehoMista" Codebehind="nastaveniZavozovehoMista.aspx.cs" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="content" ContentPlaceHolderID="content" Runat="Server">
    
    <style type="text/css">
        .item{margin:5px;}
    </style>
    <cms:RoundPanel ID="rp" runat="server" CssClass="roundPanel" Width="100%">
        <div style="margin-bottom:10px;">
            <h2><asp:Literal ID="Literal1" runat="server" Text="Nastavení závozových míst" /></h2>
        </div>
        <div class="item">
            <table width="100%">
                <tr>
                    <td style="white-space:nowrap;">Město :</td>
                    <td width="100%">
                        <asp:TextBox runat="server" ID="txtMesto" Width="100%"></asp:TextBox>
                    </td>
                    <td style="white-space:nowrap;">Stát :</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddlStat" Width="100px">
                            <asp:ListItem Text="CZ" Value="CZ"></asp:ListItem>
                            <asp:ListItem Text="SK" Value="SK"></asp:ListItem>
                            <asp:ListItem Text="PL" Value="PL"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="white-space:nowrap;">PSČ :</td>
                    <td>
                        <asp:TextBox runat="server" ID="txtPsc"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td style="white-space:nowrap;">Datum :</td>
                    <td><cms:ASPxDatePicker runat="server" ID="dtpDatum" Width="80px" /></td>
                    <td>čas :</td>
                    <td><asp:TextBox runat="server" ID="txtCas" Width="40px"></asp:TextBox></td>
                    <td style="white-space:nowrap;">Datum skryti :</td>
                    <td><cms:ASPxDatePicker runat="server" ID="dtpDatumSkryti" Width="80px" /></td>
                    <td>čas :</td>
                    <td><asp:TextBox runat="server" ID="txtCasSkryti" Width="40px"></asp:TextBox></td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td style="white-space:nowrap;">Popis :</td>
                    <td width="100%">
                        <asp:TextBox runat="server" ID="txtPopis" Width="100%"></asp:TextBox>
                    </td>
                </tr>
                <tr align="right">
                    <td colspan="2"><asp:Button runat="server" ID="btnSave" Text="Přidat" OnClick="OnAdd" CausesValidation="true" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                     <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCas" ErrorMessage="Čas musí být ve fomátu 00:00!" ValidationExpression="^(([0-1]?[0-9])|([2][0-3])):([0-5]?[0-9])?$">
                     </asp:RegularExpressionValidator>
                     <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtCasSkryti" ErrorMessage="Čas skryti musí být ve fomátu 00:00!" ValidationExpression="^(([0-1]?[0-9])|([2][0-3])):([0-5]?[0-9])?$">
                     </asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                    </td>
                </tr>
            </table>
            <asp:GridView runat="server" ID="gridView" DataKeyNames="Id" OnRowCommand="OnRowCommand" AutoGenerateColumns="False" Width="100%" CellPadding="4" EnableModelValidation="True" ForeColor="#333333" GridLines="None">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField HeaderText="Stát" DataField="Stat" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField HeaderText="Město" DataField="Mesto" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField HeaderText="PSČ" DataField="Psc" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField HeaderText="Popis" DataField="Popis" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                    <asp:BoundField HeaderText="Datum a čas" DataField="DatumACas" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                    <asp:BoundField HeaderText="Datum a čas skryti" DataField="DatumACas_Skryti" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                    <asp:TemplateField HeaderText="" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:ImageButton ID="lblEdit" ImageUrl="~/images/Pen.png" ForeColor="Red" runat="server" CommandName="EDIT_ITEM" CommandArgument='<%#Eval("id")%>' />
                        </ItemTemplate>
                    </asp:TemplateField> 
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
        <div style="margin-bottom:10px;">
            <h3><asp:Literal ID="Literal2" runat="server" Text="Osobní odběr v sídle společnosti" /></h3>
        </div>
        <div class="item">
            <asp:CheckBox runat="server" ID="cbPovoliOsobniOdber" Text="Osobní odběr povolen" AutoPostBack="true" OnCheckedChanged="cbPovoliOsobniOdber_CheckedChanged"/>
            <asp:Panel runat="server" ID="panelOsobniOdber">
                <div>
		            Interval od&nbsp;
                    <asp:DropDownList runat="server" ID="ddlLimitFrom"></asp:DropDownList>&nbsp;
                    Hodin : <asp:TextBox runat="server" ID="txtLimitFromHodin" Width="40px"></asp:TextBox>
                    Minut : <asp:TextBox runat="server" ID="txtLimitFromMinut" Width="40px"></asp:TextBox>
                    &nbsp;&nbsp;do&nbsp;
                    <asp:DropDownList runat="server" ID="ddlLimitTo"></asp:DropDownList>
                    Hodin : <asp:TextBox runat="server" ID="txtLimitToHodin" Width="40px"></asp:TextBox>
                    Minut : <asp:TextBox runat="server" ID="txtLimitToMinut" Width="40px"></asp:TextBox>
                    <asp:Button runat="server" Text="Přidat" ID="btnAddLimit" OnClick="OnAddLimit" />
                    <asp:GridView runat="server" ID="gridViewLimit" DataKeyNames="DisplayString" OnRowCommand="OnLimitRowCommand" AutoGenerateColumns="False" Width="100%" CellPadding="4" EnableModelValidation="True" ForeColor="#333333" GridLines="None">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:BoundField HeaderText="Povolené časy" DataField="DisplayString" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
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
            </asp:Panel>
        </div>
      </cms:RoundPanel>
</asp:Content>

