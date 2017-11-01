<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PaymentResult.aspx.cs" Inherits="Eurona.pay.gp.PaymentResult" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <link href='../../styles/paymentResult.css' type="text/css" rel="stylesheet" />
</head>
<body >
    <form id="form1" runat="server">

    <div class="result-page" >
    <div class="result-page-inner">
        <asp:ImageMap class="centered" style="width:1484px;height:1112px;" ID="imSuccessCZ" ImageUrl="~/images/pay/success_CZ.jpg" HotSpotMode="Navigate"  runat="server">
            <asp:RectangleHotSpot Top="542" Bottom="597" Left="288" Right="952" AlternateText="E-shop" NavigateUrl ="/eshop/" />
        </asp:ImageMap>
        <asp:ImageMap class="centered" style="width:1484px;height:1112px;" ID="imErrorCZ" ImageUrl="~/images/pay/error_CZ.jpg" HotSpotMode="Navigate"  runat="server">
            <asp:RectangleHotSpot Top="542" Bottom="597" Left="356" Right="726" AlternateText="E-shop" NavigateUrl ="/eshop/" />
        </asp:ImageMap>
              <asp:ImageMap class="centered" style="width:1484px;height:1112px;" ID="imSuccessSK" ImageUrl="~/images/pay/success_SK.jpg" HotSpotMode="Navigate"  runat="server">
            <asp:RectangleHotSpot Top="542" Bottom="597" Left="288" Right="952" AlternateText="E-shop" NavigateUrl ="/eshop/" />
        </asp:ImageMap>
        <asp:ImageMap class="centered" style="width:1484px;height:1112px;" ID="imErrorSK" ImageUrl="~/images/pay/error_SK.jpg" HotSpotMode="Navigate"  runat="server">
           <asp:RectangleHotSpot Top="542" Bottom="597" Left="356" Right="726" AlternateText="E-shop" NavigateUrl ="/eshop/" />
        </asp:ImageMap>
        <asp:ImageMap class="centered" style="width:1484px;height:1112px;" ID="imSuccessPL" ImageUrl="~/images/pay/success_PL.jpg" HotSpotMode="Navigate"  runat="server">
            <asp:RectangleHotSpot Top="542" Bottom="597" Left="286" Right="1086" AlternateText="E-shop" NavigateUrl ="/eshop/" />
        </asp:ImageMap>
        <asp:ImageMap class="centered" style="width:1484px;height:1112px;" ID="imErrorPL" ImageUrl="~/images/pay/error_PL.jpg" HotSpotMode="Navigate"  runat="server">
           <asp:RectangleHotSpot Top="542" Bottom="597" Left="356" Right="812" AlternateText="E-shop" NavigateUrl ="/eshop/" />
        </asp:ImageMap>
    </div>
        </div>
    </form>
</body>
</html>
