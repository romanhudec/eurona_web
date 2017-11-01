<%@ Page Title="<%$ Resources:Strings, Navigation_Registration %>" Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" CodeBehind="registerUserFinish.aspx.cs" Inherits="Eurona.User.Advisor.RegisterUserFinish" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls.Page" TagPrefix="cmsPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="banner" runat="server">
</asp:Content>
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
					<b><asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Strings, UserRegistrationPage_Finished %>" /></b>
				</td>
			</tr>
			<tr>
				<td>
					<b><asp:Label ID="Literal1" ForeColor="#eb0a5b" runat="server" Text="<%$ Resources:Strings, UserRegistrationPage_Warning %>" /></b>
				</td>
			</tr>
			<tr>
				<td align="center">
					<b><asp:LinkButton ID="lbMyOffice" runat="server" Text="<%$ Resources:Strings, UserRegistrationPage_GotoMyOffice %>" OnClick="OnGoToMyOffice" /></b>
				</td>
			</tr>
		</table>
	</div>
</asp:Content>
