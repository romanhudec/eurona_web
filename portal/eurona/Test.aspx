<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="Eurona.Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <%--<asp:LinkButton ID="btnlnk" runat="server" Text='Order' OnClientClick='showOrder("20180300001");return false;'></asp:LinkButton>--%>
        <a ID="btnlnk" Text='Order' href='<%= ResolveUrl("~/user/advisor/reports/Objednavka.aspx?") + GetHash("20180300001")%>'>Order</a>
    </div>
    </form>
</body>
</html>
