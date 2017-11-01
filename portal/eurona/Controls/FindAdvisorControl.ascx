<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FindAdvisorControl.ascx.cs" Inherits="Eurona.Controls.FindAdvisorControl" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls.Page" TagPrefix="cmsPage" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<style type="text/css">
    .rcbInputCell .rcbEmptyMessage{color:#DADADA!important;}
</style>
<div>
    <table border="0" style="margin: auto;">
        <tr>
            <td>
                <telerik:RadComboBox runat="server" ID="ddlRegion" EmptyMessage="<%$Resources:Strings, HostLogin_PleaseEneterRegionCode %>" Width="300px"></telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadTextBox runat="server" ID="txtCity" EmptyMessageStyle-ForeColor="#DADADA"
                    EmptyMessageStyle-Font-Italic="true" EmptyMessage="<%$Resources:Strings, HostLogin_PleaseEneterCity %>"
                    Width="300px">
                </telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadTextBox runat="server" ID="txtAdvisorName" EmptyMessageStyle-ForeColor="#DADADA"
                    EmptyMessageStyle-Font-Italic="true" EmptyMessage="<%$Resources:Strings, HostLogin_PleaseEneterAdvisor %>"
                    Width="300px">
                </telerik:RadTextBox>
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Button ID="btnFindAdvisor" runat="server" Text="<%$Resources:Strings, HostLogin_FindAdvisor_ButtonText %>"
                    OnClick="OnFindAdvisor" />
            </td>
        </tr>
    </table>
    <div runat="server" id="gridContainer" style="width:1005; margin: auto;">
    </div>
</div>
