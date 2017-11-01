<%@ Page Title="" Language="C#" MasterPageFile="~/eshop/admin/admin.master" AutoEventWireup="true" CodeBehind="highlight.aspx.cs" Inherits="Eurona.EShop.Admin.Classifier.HighlightPage" %>
<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
    <div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<a id="A2" href="~/eshop/admin/default.aspx" runat="server"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:EShopStrings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<a id="A3" href="~/eshop/admin/classifiers.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:EShopStrings, Navigation_Classifiers %>" /></a>
        <span>&nbsp;-&nbsp;</span>
		<a id="A5" href="~/eshop/admin/classifier/orderStatuses.aspx" runat="server"><asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:EShopStrings, Navigation_Classifier_Highlights %>" /></a>
		 <span>&nbsp;-&nbsp;</span>
		<asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Navigation_Classifier_Highlight %>" />
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div id="divControls" runat="server">
    </div>
</asp:Content>
