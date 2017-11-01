<%@ Page Title="<%$ Resources:Strings, Navigation_NewAdvisors %>" Language="C#" MasterPageFile="~/user/advisor/page.Master" AutoEventWireup="true" CodeBehind="newAdvisors.aspx.cs" Inherits="Eurona.User.Advisor.NewAdvisorsPage" %>

<%@ Register Assembly="eurona" Namespace="Eurona.Controls" TagPrefix="eurona" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>

<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
	<div class="navigation-links">
		<asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" />
		<span>&nbsp;-&nbsp;</span>		
		<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_NewAdvisors %>" />
	</div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content_header" runat="server">
    <div class="content-header-container">
        <asp:Label CssClass="title" ID="lblTitle" runat="server" Text="<%$ Resources:Strings, Navigation_NewAdvisors %>" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
     <eurona:NewAdvisorsControl runat="server" ID="newAdvisorsControl" CssClass="dataGrid"/>
</asp:Content>
