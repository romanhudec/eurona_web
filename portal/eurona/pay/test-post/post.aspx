<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="post.aspx.cs" Inherits="Eurona.pay.test_post.post" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>

   <form NAME="MERCHANTFORM" method="POST" action="transaction.aspx">
        <INPUT TYPE="hidden" NAME="merchantid" VALUE="101">
        <INPUT TYPE="hidden" NAME="amount" VALUE="500">
        <INPUT TYPE="hidden" NAME="currency" VALUE="203">
        <INPUT TYPE="hidden" NAME="brand" VALUE="VISA" >
        <INPUT TYPE="hidden" NAME="transactiontype" VALUE="sale" >
        <INPUT TYPE="hidden" NAME="merchantref" VALUE="3453453444" >
        <INPUT TYPE="hidden" NAME="merchantdesc" VALUE="Your Order 3453453444">
        <INPUT TYPE="hidden" NAME="extension_recurringfrequency" VALUE="28">
        <INPUT TYPE="hidden" NAME="extension_recurringenddate" VALUE="20041224">
        <INPUT TYPE="hidden" NAME="language" VALUE="EN">
        <INPUT TYPE="hidden" NAME="emailcustomer" VALUE="cardholder@email.cz">
        <INPUT TYPE="hidden" NAME="merchantvar1" VALUE="ERPFunction1_12">
        <INPUT TYPE="hidden" NAME="merchantvar2" VALUE="ERPRef1_45">
        <INPUT TYPE="hidden" NAME="merchantvar3">
        <INPUT TYPE="hidden" NAME="var1" VALUE="Zbozi: Walkman XYZ">
        <INPUT TYPE="hidden" NAME="var2">
        <INPUT TYPE="hidden" NAME="var3">
        <INPUT TYPE="hidden" NAME="var4">
        <INPUT TYPE="hidden" NAME="var5">
        <INPUT TYPE="hidden" NAME="var6">
        <INPUT TYPE="hidden" NAME="var7" VALUE="Bubenská 1">
        <INPUT TYPE="hidden" NAME="var8" VALUE="150 00 Praha 5">
        <INPUT TYPE="hidden" NAME="var9" VALUE="http://www.obchod.cz">
        <INPUT TYPE=image SRC="button.gif" BORDER=0 VALUE="SSL" Alt="Zaplatit kartou">
        
    </form>
    <form runat="server">
            <asp:Button ID="Button1" runat="server" Text="Zaplatit kartou" OnClick="OnZaplatitKartou" />
    </form>
</body>
</html>
