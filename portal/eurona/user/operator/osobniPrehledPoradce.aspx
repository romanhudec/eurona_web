<%@ Page Title="<%$ Resources:Reports, OsobniPrehledPoradce_Title %>" Language="C#" AutoEventWireup="true" CodeBehind="osobniPrehledPoradce.aspx.cs" Inherits="Eurona.User.Operator.OsobniPrehledPoradceReport" %>
<%@ Register src="../Advisor/Reports/osobniPrehledPoradce.ascx" tagname="osobniPrehledPoradce" tagprefix="uc1" %>
<html>
<head runat="server">
</head>
<body>
    <form runat="server">
    <uc1:osobniPrehledPoradce ID="osobniPrehledPoradce" runat="server" />
    </form>
</body>
</html>
