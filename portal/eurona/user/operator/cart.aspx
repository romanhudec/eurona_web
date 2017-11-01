<%@ Page Title="<%$ Resources:EShopStrings, CartControl_Cart %>" Language="C#" MasterPageFile="~/user/operator/admin.master" AutoEventWireup="true" CodeBehind="cart.aspx.cs" Inherits="Eurona.User.Operator.CartPage" %>

<%@ Register Assembly="CMS" Namespace="CMS.Controls" TagPrefix="cmsControls" %>
<%@ Register Assembly="Eurona" Namespace="Eurona.Controls" TagPrefix="ctrls" %>
<%@ Register Assembly="SHP" Namespace="SHP.Controls" TagPrefix="shpControls" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<a id="A2" href="~/eshop/admin/default.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:EShopStrings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:EShopStrings, CartControl_Cart %>" />
	</div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <script type="text/javascript"> var ie = 0;</script>
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
        function showControl(elmId, elmPlusId, elmMinusId) {
            var elm = document.getElementById(elmId);
            elm.style.display = 'block';
            document.getElementById(elmPlusId).style.display = 'none';
            document.getElementById(elmMinusId).style.display = 'block';
        }
        function hideControl(elmId, elmPlusId, elmMinusId) {
            var elm = document.getElementById(elmId);
            elm.style.display = 'none';
            document.getElementById(elmPlusId).style.display = 'block';
            document.getElementById(elmMinusId).style.display = 'none';
        }

        function SetFocus() {
            var elm = document.getElementById("<%=txtKod.ClientID %>");
            if (elm != null) elm.focus();
        }

        function onEnterAddCart(e) {
           
            if (!e) var e = window.event;
            if (e.which || e.keyCode) {
                if ((e.which == 13) || (e.keyCode == 13)) {
                    e.returnValue = false;
                    e.cancel = true;
                    var button = document.getElementById("<%=btnAdd.ClientID %>");
                    if (ie == 1) __doPostBack("<%=btnAdd.UniqueID  %>", '');
                    else button.focus(); 
                    return false;
                }
            } else return true;
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
        window.onload = SetFocus;
    </script>
    <style type="text/css">
        .minus{position:absolute;margin:0px -23px 0px 0px;display:none;background-image:url(../../images/rotator-minus.png);width:45px;height:45px;cursor:pointer;z-index:3;}
        .plus{position:absolute;margin:0px -23px 0px 0px;display:block;background-image:url(../../images/rotator-plus.png);width:45px;height:45px;cursor:pointer;z-index:3;}
        .orderButton{font-size:14px; background-color:transparent!important; border:0px none #fff!important; display:block; width:210px; height:75px!important; background-image:url(../../images/zelene-tlacitko.png);}
    </style>
    <table>
        <tr>
            <td>Kód zboží:</td>
            <td><asp:TextBox ID="txtKod" runat="server" Width="50px" onkeydown="onEnterAddCart(event)"></asp:TextBox></td>
            <td>Množství:</td>
            <td><asp:TextBox ID="txtMnozstvi" runat="server"  onkeydown="onEnterAddCart(event)" Width="30px"></asp:TextBox></td>
            <td><asp:Button ID="btnAdd" runat="server" Text="Přidat" OnClick="OnAddCart" CausesValidation="false"></asp:Button></td>
        </tr>
    </table>
    
    <div>
        <div class="plus" id="divPlusHz" onclick="showControl('tblHromadneZadavani', 'divPlusHz', 'divMinusHz')"></div>
        <div class="minus" id="divMinusHz" onclick="hideControl('tblHromadneZadavani', 'divPlusHz', 'divMinusHz')"></div>
        <fieldset id="fdsHromadneZadavanie" style="width:95%;padding:10px;">
            <legend style="font-size:16px;color:#EA008A;font-weight:bold;padding-left:30px;"><asp:Literal ID="Literal5" runat="server" Text="Hromadné zadávaní produktů"></asp:Literal></legend>
            <table id="tblHromadneZadavani" cellpadding="0" cellspacing="5" style="padding-top:10px;display:none;">
                <tr>
                    <td>Kód zboží:</td>
                    <td><asp:TextBox ID="txtKod1" runat="server" Width="50px"></asp:TextBox></td>
                    <td>Množství:</td>
                    <td><asp:TextBox ID="txtMnozstvi1" runat="server" Width="30px"></asp:TextBox></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Kód zboží:</td>
                    <td><asp:TextBox ID="txtKod2" runat="server" Width="50px"></asp:TextBox></td>
                    <td>Množství:</td>
                    <td><asp:TextBox ID="txtMnozstvi2" runat="server" Width="30px"></asp:TextBox></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Kód zboží:</td>
                    <td><asp:TextBox ID="txtKod3" runat="server" Width="50px"></asp:TextBox></td>
                    <td>Množství:</td>
                    <td><asp:TextBox ID="txtMnozstvi3" runat="server" Width="30px"></asp:TextBox></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Kód zboží:</td>
                    <td><asp:TextBox ID="txtKod4" runat="server" Width="50px"></asp:TextBox></td>
                    <td>Množství:</td>
                    <td><asp:TextBox ID="txtMnozstvi4" runat="server" Width="30px"></asp:TextBox></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Kód zboží:</td>
                    <td><asp:TextBox ID="txtKod5" runat="server" Width="50px"></asp:TextBox></td>
                    <td>Množství:</td>
                    <td><asp:TextBox ID="txtMnozstvi5" runat="server" Width="30px"></asp:TextBox></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Kód zboží:</td>
                    <td><asp:TextBox ID="txtKod6" runat="server" Width="50px"></asp:TextBox></td>
                    <td>Množství:</td>
                    <td><asp:TextBox ID="txtMnozstvi6" runat="server" Width="30px"></asp:TextBox></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Kód zboží:</td>
                    <td><asp:TextBox ID="txtKod7" runat="server" Width="50px"></asp:TextBox></td>
                    <td>Množství:</td>
                    <td><asp:TextBox ID="txtMnozstvi7" runat="server" Width="30px"></asp:TextBox></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Kód zboží:</td>
                    <td><asp:TextBox ID="txtKod8" runat="server" Width="50px"></asp:TextBox></td>
                    <td>Množství:</td>
                    <td><asp:TextBox ID="txtMnozstvi8" runat="server" Width="30px"></asp:TextBox></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Kód zboží:</td>
                    <td><asp:TextBox ID="txtKod9" runat="server" Width="50px"></asp:TextBox></td>
                    <td>Množství:</td>
                    <td><asp:TextBox ID="txtMnozstvi9" runat="server" Width="30px"></asp:TextBox></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Kód zboží:</td>
                    <td><asp:TextBox ID="txtKod10" runat="server" Width="50px"></asp:TextBox></td>
                    <td>Množství:</td>
                    <td><asp:TextBox ID="txtMnozstvi10" runat="server" Width="30px"></asp:TextBox></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Kód zboží:</td>
                    <td><asp:TextBox ID="txtKod11" runat="server" Width="50px"></asp:TextBox></td>
                    <td>Množství:</td>
                    <td><asp:TextBox ID="txtMnozstvi11" runat="server" Width="30px"></asp:TextBox></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Kód zboží:</td>
                    <td><asp:TextBox ID="txtKod12" runat="server" Width="50px"></asp:TextBox></td>
                    <td>Množství:</td>
                    <td><asp:TextBox ID="txtMnozstvi12" runat="server" Width="30px"></asp:TextBox></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Kód zboží:</td>
                    <td><asp:TextBox ID="txtKod13" runat="server" Width="50px"></asp:TextBox></td>
                    <td>Množství:</td>
                    <td><asp:TextBox ID="txtMnozstvi13" runat="server" Width="30px"></asp:TextBox></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Kód zboží:</td>
                    <td><asp:TextBox ID="txtKod14" runat="server" Width="50px"></asp:TextBox></td>
                    <td>Množství:</td>
                    <td><asp:TextBox ID="txtMnozstvi14" runat="server" Width="30px"></asp:TextBox></td>
                    <td></td>
                </tr>
                <tr>
                    <td>Kód zboží:</td>
                    <td><asp:TextBox ID="txtKod15" runat="server" Width="50px"></asp:TextBox></td>
                    <td>Množství:</td>
                    <td><asp:TextBox ID="txtMnozstvi15" runat="server" Width="30px"></asp:TextBox></td>
                    <td align="right"><asp:Button ID="btnAddHz" runat="server" Text="Přidat" OnClick="OnAddCartHz" CausesValidation="false"></asp:Button></td>
                </tr>

            </table>
        </fieldset>
    </div>
    <ctrls:CartControl runat="server" ID="cartControl" CssClass="dataGrid" FinishUrlFormat="~/user/operator/order.aspx?id={0}&ReturnUrl=/user/operator/orders.aspx?type=ac" />
    <div runat="server" id="divOrderForUser">
        <div class="plus" id="divPlus" onclick="showControl('tblOrdersForUser', 'divPlus', 'divMinus')"></div>
        <div class="minus" id="divMinus" onclick="hideControl('tblOrdersForUser', 'divPlus', 'divMinus')"></div>
        <fieldset id="fdsOrdersAssociation" style="width:95%;padding:10px;">
            <legend style="font-size:16px;color:#EA008A;font-weight:bold;padding-left:30px;"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:EShopStrings, CartControl_CreateOrderAsAnotherAdvisor %>"></asp:Literal></legend>
            <table id="tblOrdersForUser" border="0" width="100%" style="margin-top:10px;display:none;">
                <tr>
                    <td align="center" style="font-size:16px;color:#00B8EC;font-weight:bold;">1. Vyberte poradce</td>
                    <td align="center" style="font-size:16px;color:#00B8EC;font-weight:bold;">2. Vyberte přepravu</td>
                    <td align="center" style="font-size:16px;color:#00B8EC;font-weight:bold;">3. Vytvořte poradci objednávku</td>
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
                                        <asp:Button ID="btnFindAdvisor" runat="server" Text="<%$Resources:Strings, HostLogin_FindAdvisor_ButtonText %>" OnClick="OnFindAdvisor" CausesValidation="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                       <telerik:RadTextBox runat="server" ID="txtCity" onkeydown="onEnterFindAdvisor(event)" EmptyMessage="<%$Resources:Strings, HostLogin_PleaseEneterCity %>" EmptyMessageStyle-Font-Italic="true" EmptyMessageStyle-ForeColor="#DADADA" Width="200px"></telerik:RadTextBox>
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
                            </table> 
                        </ContentTemplate>
                        </asp:UpdatePanel>    
                    </td>
                    <td>
                        <table width="100%">
                            <tr>
                                <%--<td>Přeprava :</td>--%>
                                <td align="center"><asp:DropDownList runat="server" ID="ddlShipment"></asp:DropDownList></td>
                            </tr>
                        </table>
                    </td>
                    <td align="center"><asp:Button ID="btnCreateOrder" runat="server" Text="<%$Resources:EShopStrings, CartControl_CreateOrderButton_Text %>" CssClass="orderButton" OnClick="OnCreateOrderForUser" /></td>
                </tr>
            </table>
        </fieldset>
     </div>
</asp:Content>
