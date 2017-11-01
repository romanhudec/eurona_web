<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdvisorCookieLinkControl.ascx.cs" Inherits="Eurona.user.advisor.AdvisorCookieLinkControl" %>
<div runat="server" id="divContainer">
    <table>
        <tr>
            <td>
                <asp:Label runat="server" ID="lblOdkaz" Font-Bold="true" ForeColor="#00B8EC" Text="<%$ Resources:EShopStrings, AdvisorCookieLinkControl_label %>"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="word-wrap: normal; word-break: break-all;"  >
                <asp:Label runat="server" ID="lblAdvisorCookieLink"></asp:Label>
            </td>
        </tr>
    </table>
</div>