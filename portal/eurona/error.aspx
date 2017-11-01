<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="error.aspx.cs" Inherits="Eurona.ErrorPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
	<table width="100%">
		<tr>
			<td align="center">
				<div>
                    <%if( locale == "sk" ) {%>
                    <img alt="Chyba" src='<%=Page.ResolveUrl("~/images/error/error_" + errorCode + "_sk.jpg") %>' />
                    <%} else if(locale == "pl" ){ %>
                    <img alt="Chyba" src='<%=Page.ResolveUrl("~/images/error/error_" + errorCode + "_pl.jpg") %>' />
                    <%} else{ %>
                    <img alt="Chyba" src='<%=Page.ResolveUrl("~/images/error/error_" + errorCode + "_cz.jpg") %>' />
                    <%} %>
				</div>
			</td>
		</tr>
	</table>
    </form>
</body>
</html>
