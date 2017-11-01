<%@ Page Title="Reklamní zásilky" Language="C#" MasterPageFile="~/user/advisor/page.master" AutoEventWireup="true" Inherits="Eurona.User.Advisor.ReklamniZasilkyPage" Codebehind="reklamniZasilky.aspx.cs" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/user/advisor/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>					
		<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, AuthenticatedMenuItem_ReklamniZasilky %>" />
	</div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="content_header" runat="server">
    <div class="content-header-container">
        <asp:Label CssClass="title" ID="lblTitle" runat="server" Text="<%$ Resources:Strings, AuthenticatedMenuItem_ReklamniZasilky %>" />
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">
    <asp:Repeater runat="server" ID="rpReklamniZasilky" OnItemDataBound="OnItemDataBound">
        <HeaderTemplate>
			<table border="0" cellpadding="0" cellspacing="0" class="dataGrid">
			<tr>
				<td class="dataGrid_headerStyle">Reklamní zásilka</td>
				<td class="dataGrid_headerStyle">Souhlas</td>
			</tr>
		</HeaderTemplate>
        <ItemTemplate>
            <tr>
				<td><span><%#Eval("Popis")%></span></td>
				<td><asp:CheckBox Enabled="true" runat="server" ID="cbSouhlas" Text="" CommandArgument='<%#Eval("Id") %>' AutoPostBack="true" OnCheckedChanged="OnSouhlasCheckedChanged" /></td>
            </tr>
        </ItemTemplate>
        <FooterTemplate></table></FooterTemplate>
    </asp:Repeater> 
  
</asp:Content>

