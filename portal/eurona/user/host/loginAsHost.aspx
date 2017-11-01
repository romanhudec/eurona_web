<%@ Page Title="" Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" Inherits="Eurona.User.Host.LoginAsHost" Codebehind="loginAsHost.aspx.cs" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>
<%@ Register assembly="cms" namespace="CMS.Controls.Page" tagprefix="cmsPage" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">

     <cms:RoundPanel ID="rpAdvisorHost" runat="server" CssClass="roundPanel" Text="" Width="500px">
        <table>
            <tr>
                <td align="right">
                    <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Strings, HostLogin_Name %>" />
                </td>
                <td colspan="2">
                    <telerik:RadTextBox runat="server" ID="txtName" Width="200px" ></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td><asp:Literal runat="server" Text="<%$ Resources:Strings, HostLogin_Password %>"></asp:Literal></td>
                <td><telerik:RadTextBox runat="server" ID="txtAdvisorCode" Width="200px" ></telerik:RadTextBox></td>
                <td colspan="3" align="right">
                    <asp:Button runat="server" ID="btnLoginAdvisorHost" Text="<%$ Resources:Strings, HostLogin_Login %>" OnClick="OnLogin" />
                </td>
            </tr>
        </table>
     </cms:RoundPanel>
</asp:Content>

