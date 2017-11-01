<%@ Page Title="" Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" Inherits="Eurona.NotFound" Codebehind="notFound.aspx.cs" %>

<asp:Content ID="Content4" ContentPlaceHolderID="content" runat="Server">
	<div id="message" runat="server">
	</div>
	<br />
	<asp:Repeater ID="pages" runat="server">
		<ItemTemplate>
			<div>
				<img src="images/locale/<%#Eval("Locale")%>.png" alt="<%#Eval("Locale")%>" />
				<asp:LinkButton runat="server" ID="switch" OnClick="OnSwitchLocale" NavigateUrl='<%# "page.aspx?name=" + Eval("Name")%>'
					Text='<%#Eval("Title")%>' ToolTip='<%#Eval("Locale")%>'
					CommandArgument='<%#Eval("Locale")%>' />
				<br />
			</div>
		</ItemTemplate>
	</asp:Repeater>
</asp:Content>
