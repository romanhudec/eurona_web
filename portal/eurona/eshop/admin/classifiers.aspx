<%@ Page Title="" Language="C#" MasterPageFile="~/eshop/admin/admin.master" AutoEventWireup="true"
	CodeBehind="classifiers.aspx.cs" Inherits="Eurona.EShop.Admin.Classifiers" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A2" href="~/default.aspx" runat="server">
			<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span> <a id="A3" href="~/eshop/admin/default.aspx" runat="server">
			<asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:EShopStrings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span> <a id="A4" href="~/eshop/admin/classifiers.aspx" runat="server">
			<asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:EShopStrings, Navigation_Classifiers %>" /></a>
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
	<cms:RoundPanel ID="RoundPanel1" runat="server" CssClass="roundPanel" Text="<%$ Resources:EShopStrings, ClassifiersPage_EshopGroup%>">
		<div style="margin: 10px">
<%--            <a id="A5" href="~/eshop/admin/classifier/vats.aspx" runat="server">
				<asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:EShopStrings, Navigation_Classifier_VATs %>" />
			</a>
            &nbsp;&nbsp;            
            <a id="A6" href="~/eshop/admin/classifier/currencies.aspx" runat="server">
				<asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:EShopStrings, Navigation_Classifier_Currencies %>" />
			</a>	
            &nbsp;&nbsp;            
            <a href="~/eshop/admin/classifier/shipments.aspx" runat="server">
				<asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Navigation_Classifier_Shipments %>" />
			</a>
            &nbsp;&nbsp;
            <a id="A1" href="~/eshop/admin/classifier/payments.aspx" runat="server">
				<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:EShopStrings, Navigation_Classifier_Payments %>" />
			</a>
            &nbsp;&nbsp; --%>   
                    
            <a href="~/eshop/admin/classifier/orderStatuses.aspx" runat="server">
				<asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Navigation_Classifier_OrderStatuses %>" />
			</a>	
            &nbsp;&nbsp;            
            <a href="~/eshop/admin/classifier/highlights.aspx" runat="server">
				<asp:Literal ID="Literal7" runat="server" Text="<%$ Resources:EShopStrings, Navigation_Classifier_Highlights %>" />
			</a>		
            &nbsp;&nbsp;            
            <a id="A1" href="~/eshop/admin/classifier/shipments.aspx" runat="server">
				<asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:EShopStrings, Navigation_Classifier_Shipments %>" />
			</a>            														
		</div>
	</cms:RoundPanel>
</asp:Content>
