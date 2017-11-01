<%@ Page Title="" Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" CodeBehind="ContactAdvisor.aspx.cs" Inherits="Eurona.ContactAdvisor" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>
<%@ Register assembly="eurona" namespace="Eurona.Controls.ContactForm" tagprefix="cmsContactForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="banner" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="navigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" runat="server">
    <cms:RoundPanel ID="RoundPanel1" runat="server" CssClass="roundPanel" Text="<%$ Resources:Strings, ContactAdvisor %>" Width="550px" Height="100%">
        <cmsContactForm:ContactFormControl ID="contactFormControl" CssClass="contactForm" runat="server" Width="500px" Height="300px" />
    </cms:RoundPanel>
</asp:Content>
