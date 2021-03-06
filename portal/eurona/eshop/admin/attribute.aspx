﻿<%@ Page Title="" Language="C#" MasterPageFile="~/eshop/admin/admin.master" AutoEventWireup="true" CodeBehind="attribute.aspx.cs" Inherits="Eurona.EShop.Admin.Attribute" %>
<%@ Register Assembly="shp" Namespace="SHP.Controls.Attribute" TagPrefix="eshopAttribute" %>

<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<a id="A2" href="~/eshop/admin/default.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:EShopStrings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<a id="A3" href="~/eshop/admin/categories.aspx" runat="server"><asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:EShopStrings, Navigation_Categories %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:EShopStrings, Navigation_Attributes %>" />
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <eshopAttribute:AdminAttributeControl runat="server" ID="adminAttribute"/>
</asp:Content>
