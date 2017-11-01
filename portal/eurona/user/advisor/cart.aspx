﻿<%@ Page Title="<%$ Resources:EShopStrings, CartControl_Cart %>" Language="C#" MasterPageFile="~/user/advisor/page.master" AutoEventWireup="true" CodeBehind="cart.aspx.cs" Inherits="Eurona.User.Advisor.CartPage" %>

<%@ Register Assembly="Eurona" Namespace="Eurona.Controls" TagPrefix="ctrls" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!--[if IE]>
    <script type="text/javascript">ie = 1;</script>
    <![endif]--> 
    <script type="text/javascript">
        setTimeout("setFocusToCartCodeEdit()", 1200);
        function setFocusToCartCodeEdit() {
            // W3C approved DOM code that will work in all modern browsers      
            if (document.getElementById)
                document.getElementById("<%= txtKod.ClientID %>").focus();
                  // To support older versions of IE:
              else if (document.all)
                  document.all("<%= txtKod.ClientID %>").focus();
            return false;
        }
        
        <%-- 
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
        function showControl(elmId) {
            var elm = document.getElementById(elmId);
            elm.style.display = 'block';
            document.getElementById('divPlus').style.display = 'none';
            document.getElementById('divMinus').style.display = 'block';
        }
        function hideControl(elmId) {
            var elm = document.getElementById(elmId);
            elm.style.display = 'none';
            document.getElementById('divPlus').style.display = 'block';
            document.getElementById('divMinus').style.display = 'none';
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
        --%> 
    </script>
    <style type="text/css">
        .minus{position:absolute;margin:0px -23px 0px 0px;display:none;background-image:url(../../images/rotator-minus.png);width:45px;height:45px;cursor:pointer;z-index:3;}
        .plus{position:absolute;margin:0px -23px 0px 0px;display:block;background-image:url(../../images/rotator-plus.png);width:45px;height:45px;cursor:pointer;z-index:3;}
        .orderButton{font-size:14px; background-color:transparent!important; border:0px none #fff!important; display:block; width:210px; height:75px!important; background-image:url(../../images/zelene-tlacitko.png);}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content_header" runat="server">
    <div class="content-header-container">
        <asp:Label CssClass="title" ID="Literal1" runat="server" Text="<%$ Resources:EShopStrings, CartControl_Cart %>" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content_sub_header" runat="server" Visible="true">
    <div style="padding:10px;" runat="server" id="divAutomaticEmptyCartMessage" visible="false">
        <asp:Label runat="server" ID="lblAutomaticEmptyCartMessage" ForeColor="#eb0a5b" Font-Size="12"></asp:Label>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <table width="100%" style="margin-bottom:-30px;">
        <tr>
            <td style="width:100%;" align="right"><a runat="server" href="~/user/advisor/bonusoveKredity.aspx?ReturnUrl=~/user/advisor/cart.aspx" style="text-decoration:none;">Zde si můžete vybrat prémie za získané bonusové kredity</a></td>
            <td><a runat="server" href="~/user/advisor/bonusoveKredity.aspx?ReturnUrl==~/user/advisor/cart.aspx" style="text-decoration:none;"><img  style="border:0px none #fff;"" width="50px" src="../../images/BK_ikona.jpg" /></a></td>
        </tr>
    </table>
    <table onload="setTimeout('setFocus()', 1200)">
        <tr>
            <td><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, CartControl_ProductCode %>"></asp:Literal></td>
            <td><asp:TextBox ID="txtKod" runat="server" Width="50px"></asp:TextBox></td>
            <td><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, CartControl_ProductQuantity %>"></asp:Literal></td>
            <td><asp:TextBox ID="txtMnozstvi" runat="server" Width="30px"></asp:TextBox></td>
            <td><asp:Button ID="btnAdd" runat="server" Text="<%$ Resources:EShopStrings, CartControl_Add %>" OnClick="OnAddCart"></asp:Button></td>
        </tr>
    </table>
    <ctrls:CartControl runat="server" ID="cartControl" CssClass="dataGrid" FinishUrlFormat="~/user/advisor/order.aspx?id={0}&ReturnUrl=/user/advisor/orders.aspx?type=ac" />
	<div style="margin-top:10px; margin-bottom:20px;">
		<span style="padding:5px;color:#00AF00; font-size:16px; font-weight:bold;margin-bottom:20px;"><asp:Literal runat="server" ID="lblPostovneInfo"></asp:Literal></span>
	</div>
    
   <%-- <div runat="server" id="divOrderForUser">
        <div class="plus" id="divPlus" onclick="showControl('tblOrdersForUser')"></div>
        <div class="minus" id="divMinus" onclick="hideControl('tblOrdersForUser')"></div>
        <fieldset id="fdsOrdersAssociation" style="width:95%;padding:10px;">
            <legend style="font-size:16px;color:#EA008A;font-weight:bold;padding-left:30px;"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:EShopStrings, CartControl_CreateOrderAsAnotherAdvisor %>"></asp:Literal></legend>
            <table id="tblOrdersForUser" border="0" width="100%" style="margin-top:10px;display:none;">
                <tr>
                    <td align="center" style="font-size:16px;color:#00B8EC;font-weight:bold;"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:EShopStrings, CartControl_1VybertePoradce %>"></asp:Literal></td>
                    <td align="center" style="font-size:16px;color:#00B8EC;font-weight:bold;"><asp:Literal ID="Literal32" runat="server" Text="<%$ Resources:EShopStrings, CartControl_2VytvortePoradciObjednavku %>"></asp:Literal></td>
                </tr>
                <tr>
                    <td>
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
                                    <td>
                                       <telerik:RadTextBox runat="server" ID="txtAdvisorCode" onkeydown="onEnterFindAdvisor(event)" EmptyMessage="<%$Resources:Strings, HostLogin_PleaseEneterAdvisorCode %>" EmptyMessageStyle-Font-Italic="true" EmptyMessageStyle-ForeColor="#DADADA" Width="200px"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                       <telerik:RadTextBox runat="server" ID="txtAdvisorName" onkeydown="onEnterFindAdvisor(event)" EmptyMessage="<%$Resources:Strings, HostLogin_PleaseEneterAdvisor %>" EmptyMessageStyle-Font-Italic="true" EmptyMessageStyle-ForeColor="#DADADA" Width="200px"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadComboBox runat="server" ID="ddlRegion" onkeydown="onEnterFindAdvisor(event)" EmptyMessage="<%$Resources:Strings, HostLogin_PleaseEneterRegionCode %>" Width="200px"></telerik:RadComboBox>
                                    </td>
                                    <td align="right">
                                        <asp:Button ID="btnFindAdvisor" runat="server" Text="<%$Resources:Strings, HostLogin_FindAdvisor_ButtonText %>" OnClick="OnFindAdvisor"  />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                       <telerik:RadTextBox runat="server" ID="txtCity" onkeydown="onEnterFindAdvisor(event)" EmptyMessage="<%$Resources:Strings, HostLogin_PleaseEneterCity %>" EmptyMessageStyle-Font-Italic="true" EmptyMessageStyle-ForeColor="#DADADA" Width="200px"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
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
                            </table> 
                        </ContentTemplate>
                        </asp:UpdatePanel>    
                    </td>
                    <td align="center"><asp:Button ID="btnCreateOrder" runat="server" Text="<%$Resources:EShopStrings, CartControl_CreateOrderButton_Text %>" CssClass="orderButton" OnClick="OnCreateOrderForUser" /></td>
                </tr>
            </table>
        </fieldset>
     </div>--%>
</asp:Content>
