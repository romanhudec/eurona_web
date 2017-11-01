<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LogedHostControl.ascx.cs" Inherits="Eurona.User.Host.LogedHostControl" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls.Translations" TagPrefix="cmsVocabulary" %>
<style type="text/css">
        .hostButtonYes{font-size:24px; background-color:transparent!important; border:0px none #fff!important; display:block; width:210px; height:75px!important; background-image:url(../../images/zelene-tlacitko.png);}
        .hostButtonNo{ font-size:24px;background-color:transparent!important;border:0px none #fff!important;display:block; width:210px; height:75px!important; background-image:url(../../images/cervene-tlacitko.png);}
</style>
<table width="100%" runat="server" id="logedUserTable">
    <tr>
        <td align="right"><a runat="server" style="color:#eb0a5b; font-size:16px;" href="~/user/advisor/register.aspx"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, LogedHostControl_ContinueInRegistration %>"></asp:Literal></a></td>
    </tr>
    <tr><td></td></tr>
    <tr>
        <td align="right"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, LogedHostControl_Welcome %>" /></td>
    </tr>
    <tr>
        <td align="right" style="font-weight:bold"><a><%=Session[Eurona.User.Host.HostSecurity.HostNameSessionName].ToString() %></a></td>
    </tr>
    <tr><td align="right" style="font-weight:bold"><asp:LinkButton runat="server" ID="logout" Text="<%$ Resources:Strings, LoginControl_LogoutButton %>" OnClick="OnLogoutClick"></asp:LinkButton></td></tr>
    <tr><td><br /></td></tr>
    <tr>
        <td align="right"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, LogedHostControl_YouAreLogedAsHost %>" /></td>
    </tr>
    <tr>
        <td align="right" style="font-weight:bold"><a>p.<%=Advisor.Name%></a></td>
    </tr>
  <%--  <tr>
        <td align="right"><%=Advisor.RegisteredAddress.Street%></td>
    </tr>
    <tr>
        <td align="right"><%=Advisor.RegisteredAddress.City%> | <%=Advisor.RegisteredAddress.Zip%></td>
    </tr>--%>
    <tr>
        <td align="right"><%=Advisor.ContactMobile%></td>
    </tr>
    <tr>
        <td align="right"><%=Advisor.ContactEmail%></td>
    </tr>
</table>
<table width="100%" runat="server" id="notLogedUserTable">
    <tr>
        <td align="center">
            <span style="font-weight:bold;color:#0056A3;"><%=vocHostAccessPage["DefaultQuestion"]%></span>
        </td>
    </tr>
    <tr>
        <td align="center" style="padding-top:15px;" ><asp:Button ID="btnYes" runat="server" Text="Ano" OnClick="OnYes" CssClass="hostButtonYes" /></td>
    </tr>
    <tr>
        <td align="center"><asp:Button ID="btnNo" runat="server" Text="Ne" OnClick="OnNo" CssClass="hostButtonNo" /></td>
    </tr>
</table>
<cmsVocabulary:Vocabulary ID="vocHostAccessPage" CssClass="vocabulary" runat="server" Name="HostAccessPage" />  