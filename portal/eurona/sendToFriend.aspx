<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="sendToFriend.aspx.cs" Inherits="Eurona.sendToFriend" EnableViewStateMac="false" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Odeslat mému známénu</title>
    <link href="~/styles/cms.css" type="text/css" rel="stylesheet" />
    <link href="~/styles/default.css" type="text/css" rel="Stylesheet" />
</head>
<body style="background-color:#d9f0fe;">
    <form id="form1" runat="server">
    <div>
        <cms:RoundPanel ID="RoundPanel1" runat="server" CssClass="roundPanel" Text="Odeslat mému známénu" Width="350px" Height="150px">
            <div style="padding:10px;">
                <table>
                    <tr>
                        <td>od koho (jmeno) :</td>
                        <td><asp:TextBox Width="200px" runat="server" ID="txtFrom"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>od koho (email) :</td>
                        <td><asp:TextBox  Width="200px" runat="server" ID="txtEmailFrom"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>email komu :</td>
                        <td><asp:TextBox  Width="200px" runat="server" ID="txtEmailTo"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btnOdeslat" runat="server" Text="Odeslat" OnClick="OnSendEmail" />
                        </td>
                    </tr>
                </table>
            </div>
        </cms:RoundPanel>
    </div>
    </form>
</body>
</html>
