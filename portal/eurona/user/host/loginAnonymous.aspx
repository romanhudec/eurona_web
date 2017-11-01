<%@ Page Title="" Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" Inherits="Eurona.User.Host.LoginAnonymous" Codebehind="loginAnonymous.aspx.cs" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>
<%@ Register assembly="cms" namespace="CMS.Controls.Page" tagprefix="cmsPage" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .multipleRowsColumns .rcbItem, 
        .multipleRowsColumns .rcbHovered{}
        
        .col1{width:150px; font-style:italic;}
        .col2{width:180px; font-weight:bold;}
        .rcbInputCell .rcbEmptyMessage{color:#DADADA!important;}
          
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">
<script type="text/javascript">var ie = 0;</script>
<!--[if IE]>
<script type="text/javascript">ie = 1;</script>
<![endif]--> 
<script type="text/javascript">
    function OnLoad() {
        var combo = $find("<%= ddlAdvisor.ClientID %>");
        if (combo.get_items().get_count() > 0) {
            // Pre-select the first country   
            combo.set_text(combo.get_items().getItem(0).get_text());
            combo.get_items().getItem(0).highlight();
            combo.showDropDown();
        }   
        return true;
    }
    function onEnterFindAdvisor(e) {
        if (!e) var e = window.event;
        if (e.which || e.keyCode) {
            if ((e.which == 13) || (e.keyCode == 13)) {
                var button = document.getElementById("<%=btnFindAdvisor.ClientID %>");
                e.returnValue = false;
                e.cancel = true;
                if (ie == 1) __doPostBack("<%=btnFindAdvisor.UniqueID  %>", '');
                else button.focus();
                return false;
            }
        } else return true;
    }
</script>
    <cms:RoundPanel ID="rpAnonymous" runat="server" CssClass="roundPanel" Text="" Width="500px">
    <asp:UpdatePanel ID="up" runat="server" UpdateMode="Conditional" RenderMode="Block">
    <ContentTemplate>
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="up" DisplayAfter="10" DynamicLayout="true">
            <ProgressTemplate>
                <div style="position:absolute;display:table; vertical-align: middle; width:480px;height:200px;">
                    <div style="margin: 0px auto; width:16px;display:table-cell; vertical-align:middle; text-align:center;">
                        <img border="0" src="../../images/ajax-indicator.gif" alt="Loading ..." />
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <table border="0" style="margin:auto;padding-top:20px;">
            <tr>
                <td align="right">
                    <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Strings, HostLogin_Name %>" />
                </td>
                <td colspan="2">
                    <telerik:RadTextBox runat="server" ID="txtName" Width="330px" ></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td colspan="3"></td>
            </tr>
            <tr>
                <td rowspan="4" align="right">
                    <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:Strings, HostLogin_Advisor %>" />
                </td>
                <td>
                    <telerik:RadComboBox runat="server" ID="ddlRegion" onkeydown="onEnterFindAdvisor(event)" EmptyMessage="<%$Resources:Strings, HostLogin_PleaseEneterRegionCode %>" Width="200px"></telerik:RadComboBox>
                </td>
                <td align="right">
                    <%--OnClick="OnFindAdvisor"--%>
                    <asp:Button ID="btnFindAdvisor" runat="server" Text="<%$Resources:Strings, HostLogin_FindAdvisor_ButtonText %>" OnClick="OnFindAdvisor"  />
                    <%--<input type="button" ID="btnFindAdvisor" runat="server" value="<%$Resources:Strings, HostLogin_FindAdvisor_ButtonText %>" onclick="OnFind();"  />--%>
                </td>
            </tr>
            <tr>
                <td>
                   <telerik:RadTextBox runat="server" ID="txtCity" onkeydown="onEnterFindAdvisor(event)" EmptyMessage="<%$Resources:Strings, HostLogin_PleaseEneterCity %>" EmptyMessageStyle-Font-Italic="true" EmptyMessageStyle-ForeColor="#DADADA" Width="200px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                   <telerik:RadTextBox runat="server" ID="txtAdvisorName" onkeydown="onEnterFindAdvisor(event)" EmptyMessage="<%$Resources:Strings, HostLogin_PleaseEneterAdvisor %>" EmptyMessageStyle-Font-Italic="true" EmptyMessageStyle-ForeColor="#DADADA" Width="200px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <%--OnDataBound="OnDdlAdvisor_DataBound" OnItemsRequested="OnDdlAdvisor_ItemsRequested"--%>
                    <telerik:RadComboBox runat="server" ID="ddlAdvisor" Skin="Default" Height="190px" Width="330px" 
                        MarkFirstMatch="false" EnableLoadOnDemand="false" HighlightTemplatedItems="true"
                        OnItemDataBound="OnDdlAdvisor_ItemDataBound"
                        OnItemsRequested="OnDdlAdvisor_ItemsRequested"
                        OnDataBound="OnDdlAdvisor_DataBound"
                        OnClientLoad="OnLoad"
                        DropDownCssClass="multipleRowsColumns" >
                        <ItemTemplate>
                         <table style="border-bottom: 1px dotted #EFEFEF; margin-bottom: 10px; font-size: 11px;" width="98%">
                            <tr>
                              <td class="col1"><%# DataBinder.Eval( Container.DataItem, "RegisteredAddressString" )%></td>
                              <td class="col2"><%# DataBinder.Eval( Container.DataItem, "Name" )%></td>
                            </tr>
                          </table>
                         </ItemTemplate>
                        <FooterTemplate>
                        A total of <asp:Literal runat="server" ID="RadComboItemsCount" /> items
                        </FooterTemplate>
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td colspan="3" align="right">
                    <asp:Button runat="server" ID="btnLogin" Text="<%$ Resources:Strings, HostLogin_Login %>" OnClick="OnLogin" />
                </td>
            </tr>
        </table> 
    </ContentTemplate>
    </asp:UpdatePanel>    
    </cms:RoundPanel>
</asp:Content>

