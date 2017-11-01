<%@ Page Title="<%$ Resources:Strings, Navigation_ChangePassword %>" Language="C#" MasterPageFile="~/user/operator/admin.master" AutoEventWireup="true" CodeBehind="generatePassword.aspx.cs" Inherits="Eurona.User.Operator.GeneratePasswordPage" %>

<%@ Register Assembly="eurona" Namespace="Eurona.Controls" TagPrefix="cmsAcount" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" runat="Server">
    <div class="navigation-links">
        <a id="A1" href="~/default.aspx" runat="server">
            <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
        &nbsp;&raquo;&nbsp;
        <asp:Literal ID="Literal1" runat="server" Text="Generování nového hesla uživatele" />
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" runat="Server">
    <cms:RoundPanel ID="rpChangePassword" runat="server" CssClass="roundPanel" Text="Generování nového hesla uživatele" Width="350px">
        <table border="1" width="100%">
            <tr>
                <td class="form_label">Účet:</td>
                <td class="form_control"><%=AccountEntity.Login%></td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <telerik:RadCaptcha ID="capcha" runat="server">
                    </telerik:RadCaptcha>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                <asp:Button runat="server" ID="btnGenerovat" Text="Generovat" OnClick="OnGenerate" />
                <asp:Button runat="server" ID="btnCancel" Text="Zrušit" OnClick="OnCancel" />
                </td>
            </tr>
        </table>
    </cms:RoundPanel>
</asp:Content>
