<%@ Page Title="<%$ Resources:EShopStrings, Navigation_DPDPackage %>" Language="C#" MasterPageFile="~/user/advisor/page.master" AutoEventWireup="true" CodeBehind="dpdpackage.aspx.cs" Inherits="Eurona.User.Advisor.DPDPackage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript" language="javascript">
    function onLoad() {
        var elm = document.getElementById('elmDPDContainer');
        elm.style.display = 'block';
    }
    window.onload = onLoad;
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
	<div class="navigation-links">
		<asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" />
		<span>&nbsp;-&nbsp;</span>		
		<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:EShopStrings, Navigation_DPDPackage %>" />
	</div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content_header" runat="server">
    <div class="content-header-container">
        <asp:Label CssClass="title" ID="lblTitle" runat="server" Text="<%$ Resources:EShopStrings, Navigation_DPDPackage %>" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div>
        <asp:Label runat="server" ID="lblBalik" Text="Zadejte číslo balíku (14 míst): "></asp:Label>
        <asp:TextBox CssClass="input" runat="server" ID="txtBalikCislo" MaxLength="14" Width="150px"></asp:TextBox>
        <asp:Button ID="btnPresmeruj" runat="server" Text="Potvrď" OnClick="btnPresmeruj_Click" />
    </div>
</asp:Content>
