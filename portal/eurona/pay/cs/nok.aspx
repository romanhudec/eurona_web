<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="nok.aspx.cs" Inherits="Eurona.PAY.CS.NOkPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/styles/default.css" type="text/css" rel="Stylesheet" />
    <link href="~/styles/cms.css" type="text/css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
       <table width="100%" style="padding:30px;margin:40px;">
           <tr>
               <td align="center" style="color:#eb0a5b;font-size:14px;">
                   <asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Pay_TransactionWasNotSuccessfully %>"></asp:Literal>
               </td>
           </tr>
           <tr>
               <td align="center" style="padding:10px;">
                   <asp:Button runat="server" ID="btnBackToOrder" Height="30px" Text="Zpět na objednávku" CssClass="button-uhrada-kartou" OnClick="btnBackToOrder_Click" />
               </td>
           </tr>
       </table>
    </form>
</body>
</html>
