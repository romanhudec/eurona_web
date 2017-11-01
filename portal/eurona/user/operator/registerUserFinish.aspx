<%@ Page Title="<%$ Resources:Strings, Navigation_Registration %>" Language="C#" MasterPageFile="~/user/operator/admin.master" AutoEventWireup="true" CodeBehind="registerUserFinish.aspx.cs" Inherits="Eurona.User.Operator.RegisterUserFinish" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls.Page" TagPrefix="cmsPage" %>
<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server">
			<asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_Registration %>" />
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" runat="server">
	<div style="margin:auto;">
		<table border="0" style="margin:auto;">
			<tr>
				<td>
					<b><asp:Literal ID="lblRegisterInfo" runat="server" Text="" /></b>
				</td>
			</tr>
		</table>
	</div>
</asp:Content>
