<%@ Page Title="<%$ Resources:Strings, Navigation_DatovyList %>" Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" Inherits="Eurona.DatovyListPage" Codebehind="datovyList.aspx.cs" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">
    <table style="margin:auto;padding-top:20px" cellpadding="3" cellspacing="2" border="0">
        <tr>
            <td colspan="2">
                <div style="width:450px;white-space:normal;color:#0077b6;"><asp:Literal runat="server" Text="<%$ Resources:Strings, DatovyListDescription %>"></asp:Literal></div>
            </td>
            <td rowspan="4">
                <asp:Image style="margin-left:100px;" runat="server" src="/images/datovy_list_text.png" width="220px" />
            </td>
        </tr>
        <tr>
            <td style="width:100px;padding-top:8px;" align="left" valign="top"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, DatovyList_KodProduktu %>"></asp:Literal></td>
            <td align="left">
                <asp:TextBox runat="server" ID="txtKodProduktu" Width="250px"></asp:TextBox><br />
                <span><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, DatovyList_KodProduktuHint %>"></asp:Literal></span>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtKodProduktu" ErrorMessage="*"></asp:RequiredFieldValidator>

            </td>
        </tr>
        <tr>
            <td style="width:100px;padding-top:8px;" align="left" valign="top">Email:</td>
            <td align="left">
                <asp:TextBox runat="server" ID="txtEmail" Width="250px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmail" ErrorMessage="*"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="right" style="padding-right:99px;">
                <asp:Button runat="server" ID="btnOdeslat" Text="<%$ Resources:Strings, DatovyList_Odeslat %>" OnClick="btnOdeslat_Click"/>
            </td>
        </tr>
    </table>
</asp:Content>

