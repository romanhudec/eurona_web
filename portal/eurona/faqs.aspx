<%@ Page Title="<%$ Resources:Strings, Navigation_FAQs %>" Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" Inherits="Eurona.Faqs" Codebehind="faqs.aspx.cs" %>

<%@ Register Assembly="cms" Namespace="CMS.Controls.FAQ" TagPrefix="cmsFAQ" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_FAQs %>" />
	</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">
	<cmsFAQ:FAQControl ID="faqControl" runat="server" CssClass="faq" />
</asp:Content>

