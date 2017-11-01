<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true"
	CodeBehind="classifiers.aspx.cs" Inherits="Eurona.Admin.Classifiers" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A2" href="~/default.aspx" runat="server">
			<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span> <a id="A3" href="~/admin/default.aspx" runat="server">
			<asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span> <a id="A4" href="~/admin/classifiers.aspx" runat="server">
			<asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Strings, Navigation_Classifiers %>" /></a>
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
	<cms:RoundPanel ID="RoundPanel1" runat="server" CssClass="roundPanel" Text="<%$ Resources:Strings, ClassifiersPage_CommonGroup%>">
		<div style="margin: 10px">
            <a id="A1" href="~/admin/classifier/articleCategories.aspx" runat="server">
				<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_Classifier_ArticleCategories %>" />
			</a>
			&nbsp;&nbsp;
            <a id="A5" href="~/admin/classifier/urlAliasPrefixes.aspx" runat="server">
				<asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:Strings, Navigation_Classifier_UrlAliasPrefixes %>" />
			</a>
			&nbsp;&nbsp;
            <a id="A6" href="~/admin/classifier/supportedLocales.aspx" runat="server">
				<asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:Strings, Navigation_Classifier_SupportedLocales %>" />
			</a>	            						
		</div>
	</cms:RoundPanel>
</asp:Content>
