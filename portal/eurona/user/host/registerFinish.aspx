<%@ Page Title="<%$ Resources:Strings, Navigation_Registration %>" Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" CodeBehind="registerFinish.aspx.cs" Inherits="Eurona.User.Host.RegisterFinish" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls.Page" TagPrefix="cmsPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="banner" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server">
			<asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span> <a id="A2" runat="server" href="~/user/register.aspx">
			<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_Registration %>" /></a>
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" runat="server">
	<div>
		<table border="0">
			<tr>
				<td colspan="2">
					<b>
						<asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Strings, UserRegistrationPage_Finished %>" /></b>
				</td>
			</tr>
		</table>
	</div>
</asp:Content>
