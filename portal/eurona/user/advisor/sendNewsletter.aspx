<%@ Page Title="" Language="C#" MasterPageFile="~/user/advisor/page.master" AutoEventWireup="true" CodeBehind="sendNewsletter.aspx.cs" Inherits="Eurona.User.Advisor.sendNewsletter" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="banner" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="category_navigation" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content_header" runat="server">
    <div class="content-header-container">
		<asp:Label CssClass="title" ID="lblTitle" runat="server" Text="Zasílání tiskových a elektronických materiálů" />
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="content" runat="server">
	<div style="margin:auto;">
	<cms:RoundPanel ID="rpAccount" runat="server" CssClass="roundPanel" Width="250px">
		<table border="0">
			<tr>
				<td align="right"><asp:Literal ID="Literal3" runat="server" Text="Zasílání katalogu"></asp:Literal></td>
				<td align="left"><asp:CheckBox ID="cbZasilaniKatalogu" runat="server" /></td>
			</tr>
			<tr>
				<td align="right"><asp:Literal ID="Literal1" runat="server" Text="Zasílání tiskových materiálů"></asp:Literal></td>
				<td align="left"><asp:CheckBox ID="cbZasilaniTiskovin" runat="server" /></td>
			</tr>
			<tr>
				<td align="right"><asp:Literal ID="Literal2" runat="server" Text="Zasílání elektronických materiálů"></asp:Literal></td>
				<td align="left"><asp:CheckBox ID="cbZasilaniNewsletter" runat="server" /></td>
			</tr>
			<tr>
				<td colspan="2" align="center">
					<asp:Button runat="server" ID="btnSave" Text="<%$ Resources:Strings, SaveButton_Text %>" OnClick="OnSaveClick" />
					<asp:Button runat="server" ID="btnCancel" Text="<%$ Resources:Strings, CancelButton_Text %>" OnClick="OnCancelClick" />
				</td>
			</tr>
		</table>
	</cms:RoundPanel>
    </div>
</asp:Content>
