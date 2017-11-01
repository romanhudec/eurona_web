<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Register.ascx.cs" Inherits="Eurona.Controls.Register" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<head>
    <style type="text/css">
        .multipleRowsColumns .rcbItem, 
        .multipleRowsColumns .rcbHovered{}

        .col1{width:150px; font-style:italic;}
        .col2{width:180px; font-weight:bold;}     
    </style>
</head>

<script language="javascript" type="text/javascript">
    function UpdateItemCountField(sender, args) {
        //set the footer text
        sender.get_dropDownElement().lastChild.innerHTML = "A total of " + sender.get_items().get_count() + " items";
    }
    function AcceptTermsAndConditions(checkbox) {
        var btnAccept = document.getElementById("<%=btnContinue.ClientID%>");
        if (btnAccept == null) return;

        btnAccept.disabled = !checkbox.checked;
    }
</script>

<cms:RoundPanel ID="RoundPanel1" runat="server" CssClass="roundPanel" Width="400px" >
    <table border="0">
        <tr>
            <td class="form_label_required">
                <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, RegisterControl_LoginLabel %>" />
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtLogin" Width="200px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtLogin" Display="Dynamic"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td class="form_label_required">
                <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, RegisterControl_PasswordLabel %>" />
            </td>
            <td>
                <asp:TextBox runat="server" TextMode="Password" ID="txtPassword" Width="200px"></asp:TextBox>
                <asp:RequiredFieldValidator ID="passwordRequired" runat="server" ControlToValidate="txtPassword" Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPassword" Display="Dynamic" ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$" ErrorMessage="Heslo musí obsahovat čisla, malá a velká písmena.<br/>Minimální délka je 8 znaků!"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td class="form_label_required">
                <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, RegisterControl_PasswordConfirmLabel %>" />
            </td>
            <td>
                <asp:TextBox runat="server" TextMode="Password" ID="txtConfirmPassword" Width="200px"
                    CausesValidation="True"></asp:TextBox>
                <asp:CompareValidator ID="passwordMatch" runat="server" ControlToCompare="txtConfirmPassword"
                    ControlToValidate="txtPassword" Display="Dynamic" ErrorMessage="!"></asp:CompareValidator>
            </td>
        </tr>
        <tr>
            <td class="form_label_required">
                <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Strings, RegisterControl_EmailLabel %>" />
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtEmail" CausesValidation="True" Width="200px"></asp:TextBox>
                <asp:RegularExpressionValidator ID="emailValidator" runat="server" ControlToValidate="txtEmail"
                    ErrorMessage="!" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"></asp:RegularExpressionValidator>
                <asp:RequiredFieldValidator ID="emailRequired" runat="server" ControlToValidate="txtEmail"
                    ErrorMessage="*"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:Label ID="lblAlreadyExists" runat="server" ForeColor="Red" Font-Size="16px" Font-Bold="true" Text="<%$ Resources:Strings, RegisterControl_AccountAlreadyExists %>" Visible="False"></asp:Label>
            </td>
        </tr>
        <tr runat="server" id="advisorRow">
            <td class="form_label_required">
                <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:Strings, RegisterControl_AdvisorLabel %>" />
            </td>
            <td>
                <telerik:RadComboBox runat="server" ID="ddlAdvisor" Skin="Default" Height="190px" Width="330px" 
                    MarkFirstMatch="false" EnableLoadOnDemand="true" HighlightTemplatedItems="true"
                    OnClientItemsRequested="UpdateItemCountField" OnDataBound="OnDdlAdvisor_DataBound"
                    OnItemDataBound="OnDdlAdvisor_ItemDataBound" OnItemsRequested="OnDdlAdvisor_ItemsRequested"
                    DropDownCssClass="multipleRowsColumns" >
                    <ItemTemplate>
                     <table style="border-bottom: 1px dotted #EFEFEF; margin-bottom: 10px; font-size: 11px;" width="98%">
                        <tr>
                          <td class="col1"><%# DataBinder.Eval( Container.DataItem, "RegisteredAddressString" )%></td>
                          <td class="col2"><%# DataBinder.Eval( Container.DataItem, "Name" )%></td>
                        </tr>
                      </table>
                     </ItemTemplate>

<%--                    <HeaderTemplate>
                        <ul>
                            <li class="col1"><asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:Strings, HostLogin_StateLabel %>" /></li>
                            <li class="col1"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, HostLogin_CityLabel %>" /></li>
                            <li class="col1" style="width:200px;"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, HostLogin_AdvisorLabel %>" /></li>
                        </ul>
                    </HeaderTemplate>--%>

                    <FooterTemplate>
                    A total of <asp:Literal runat="server" ID="RadComboItemsCount" /> items
                    </FooterTemplate>
                </telerik:RadComboBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div>
                    <asp:CheckBox ID="CheckBox1" runat="server" Text="<%$ Resources:Strings, RegisterControl_AcceptTermsAndConditions %>" onclick="AcceptTermsAndConditions(this)" /><br />
                    <asp:HyperLink ID="hlObchodniPodminky" runat="server" NavigateUrl="" Text="<%$ Resources:Strings, RegisterControl_TermsAndConditions %>" Target="_blank" ></asp:HyperLink>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <telerik:RadCaptcha ID="capcha" runat="server">
                </telerik:RadCaptcha>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button runat="server" ID="btnContinue" Enabled="false" Text="<%$ Resources:Strings, RegisterControl_ContinueButton %>" OnClick="OnContinueClick" />
            </td>
        </tr>
    </table>
</cms:RoundPanel>
