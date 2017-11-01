<%@ Page Title="" Language="C#" MasterPageFile="~/user/advisor/page.master" AutoEventWireup="true" CodeBehind="mimoradnaNabidka.aspx.cs" Inherits="Eurona.User.Advisor.MimoradnaNabidkaPage" %>
<%@ Register assembly="Eurona" namespace="Eurona.Controls" tagprefix="ctrls" %>


<asp:Content ID="Content6" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>				
		<asp:Literal ID="Literal2" runat="server" Text="Mimořádná nabídka" />
	</div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="content_header" runat="server">
    <div class="content-header-container">
        <asp:Label CssClass="title" ID="lblTitle" runat="server" Text="Mimořádná nabídka" />
    </div>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="content" Runat="Server">
    <ctrls:MimoradnaNabidkaControl ID="mimoradnaNabidkaControl" CssClass="news" runat="server" ></ctrls:MimoradnaNabidkaControl>
</asp:Content>