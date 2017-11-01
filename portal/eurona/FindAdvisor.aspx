<%@ Page Title="" Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" CodeBehind="FindAdvisor.aspx.cs" Inherits="Eurona.FindAdvisor" %>
<%@ Register src="Controls/FindAdvisorControl.ascx" tagname="FindAdvisorControl" tagprefix="uc1" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="banner" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="navigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" runat="server">
    <cms:RoundPanel ID="RoundPanel1" runat="server" CssClass="roundPanel" Text="<%$ Resources:Strings, FindAdvisor %>" Width="900px" Height="100%">
        <uc1:FindAdvisorControl ID="FindAdvisorControl1" runat="server" ContactUrlFromat="~/contactAdvisor.aspx?id={0}" />
    </cms:RoundPanel>
</asp:Content>
