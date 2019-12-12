<%@ Page Title="" Language="C#" MasterPageFile="~/eshop/admin/admin.master" AutoEventWireup="true" Inherits="Eurona.Admin.ZavozoveMistoEdit" Codebehind="zavozoveMistoEdit.aspx.cs" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>

<asp:Content ID="content" ContentPlaceHolderID="content" Runat="Server">
    
    <style type="text/css">
        .bkItem{margin:5px;}
    </style>
    <cms:RoundPanel ID="rp" runat="server" CssClass="roundPanel" Width="100%">
        <div style="margin-bottom:10px;">
            <h2><asp:Literal ID="Literal1" runat="server" Text="Nastavení závozových míst - detail" /></h2>
        </div>
        <div class="bkItem">
            <table border="0">
                <tr>
                    <td style="white-space:nowrap;">Stát :</td>
                    <td colspan="3">
                        <asp:DropDownList runat="server" ID="ddlStat" Width="100px">
                            <asp:ListItem Text="CZ" Value="CZ"></asp:ListItem>
                            <asp:ListItem Text="SK" Value="SK"></asp:ListItem>
                            <asp:ListItem Text="PL" Value="PL"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="white-space:nowrap;">Město :</td>
                    <td colspan="3">
                        <asp:TextBox runat="server" ID="txtMesto" Width="500px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="white-space:nowrap;">PSČ :</td>
                    <td colspan="3">
                        <asp:TextBox runat="server" ID="txtPsc" Width="100px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="white-space:nowrap;">Popis :</td>
                    <td colspan="3">
                        <asp:TextBox runat="server" ID="txtPopis" Width="500px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="white-space:nowrap;">Datum :</td>
                    <td><cms:ASPxDatePicker runat="server" ID="dtpDatum" Width="80px" /> čas : <asp:TextBox runat="server" ID="txtCas" Width="40px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td style="white-space:nowrap;">Datum skryti :</td>
                    <td><cms:ASPxDatePicker runat="server" ID="dtpDatumSkryti" Width="80px" /> čas : <asp:TextBox runat="server" ID="txtCasSkryti" Width="40px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="4"><asp:Button runat="server" ID="btnSave" Text="Uložit" OnClick="OnSave" CausesValidation="true" /></td>
                </tr>
                <tr>
                    <td  colspan="4">
                     <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtCas" ErrorMessage="Čas musí být ve fomátu 00:00!" ValidationExpression="^(([0-1]?[0-9])|([2][0-3])):([0-5]?[0-9])?$">
                     </asp:RegularExpressionValidator>
                     <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtCasSkryti" ErrorMessage="Čas skryti musí být ve fomátu 00:00!" ValidationExpression="^(([0-1]?[0-9])|([2][0-3])):([0-5]?[0-9])?$">
                     </asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td  colspan="4">
                    </td>
                </tr>
            </table>
        </div>
      </cms:RoundPanel>
</asp:Content>

