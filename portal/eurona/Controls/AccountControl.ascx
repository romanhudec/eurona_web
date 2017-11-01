<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccountControl.ascx.cs" Inherits="Eurona.Controls.AccountControl" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<head>
    <style type="text/css">
        .multipleRowsColumns .rcbItem, 
        .multipleRowsColumns .rcbHovered
        {
            /*
            float: left;
            margin: 0 1px;
            min-height: 13px;
            overflow: hidden;
            padding: 2px 19px 2px 6px;
            width: 175px;*/
        }
        .rcbHeader ul,
        .rcbFooter ul,
        .rcbItem ul, .rcbHovered ul, .rcbDisabled ul
        {
            width: 100%;
            display:inline-block;
            margin: 0;
            padding: 0;
            list-style-type: none;
        }

        .col1, .col2, .col3
        {
            float: left;
            width: 100px;
            margin: 0;
            padding: 0 3px 0 0;
            line-height: 14px;
            display:inline;
        }        
    </style>
</head>

<script type="text/javascript">
    function UpdateItemCountField(sender, args) {
        //set the footer text
        sender.get_dropDownElement().lastChild.innerHTML = "A total of " + sender.get_items().get_count() + " items";
    }
</script>

<cms:RoundPanel ID="RoundPanel1" runat="server" CssClass="roundPanel">
    <table border="0">
        <tr>
            <td class="form_label_required">
                <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, RegisterControl_LoginLabel %>" />
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtLogin" Width="200px" Enabled="false"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td class="form_label_required">
                <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Strings, RegisterControl_EmailLabel %>" />
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtEmail" CausesValidation="True" Width="200px"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button runat="server" ID="btnSave" Text="<%$ Resources:Strings, SaveButton_Text %>" OnClick="OnSaveClick" />
                <asp:Button runat="server" ID="btnCancel" Text="<%$ Resources:Strings, CancelButton_Text %>" OnClick="OnCancelClick" />
            </td>
        </tr>
    </table>
</cms:RoundPanel>
