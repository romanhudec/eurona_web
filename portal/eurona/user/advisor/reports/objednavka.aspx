<%@ Page Title="<%$ Resources:EShopStrings, Navigation_Order %>" Language="C#" AutoEventWireup="true" CodeBehind="objednavka.aspx.cs" Inherits="Eurona.User.Advisor.Reports.Objednavka" %>
<%@ Register Assembly="eurona" Namespace="Eurona.Controls" TagPrefix="shpOrder" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="~/styles/default.css" type="text/css" rel="Stylesheet" />
    <link href="~/styles/cms.css" type="text/css" rel="stylesheet" />
    <link href="~/styles/advisor.css" type="text/css" rel="stylesheet" />
    <link href="~/styles/ASPxDatePicker.css" type="text/css" rel="stylesheet" />
    <link href="~/styles/ASPxMultipleFileUpload.css" rel="stylesheet" type="text/css" />
    <link href="~/styles/eshop.css" type="text/css" rel="Stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager id="ScriptManager" runat="server"/>
    <div>
        <shpOrder:AdminOrderControl runat="server" ID="adminOrderControl" IsEditing="true" CssClass="adminOrderControl" CssGridView="dataGrid" FinishUrlFormat="~/user/advisor/orderFinish.aspx?id={0}" />
    </div>
    </form>
</body>
</html>
