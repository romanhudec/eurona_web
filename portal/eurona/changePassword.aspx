<%@ Page Title="<%$ Resources:Strings, Navigation_ChangePassword %>" Language="C#" MasterPageFile="~/page.master" AutoEventWireup="true" Inherits="Eurona.ChangePassword" Codebehind="changePassword.aspx.cs" %>

<%@ Register Assembly="eurona" Namespace="Eurona.Controls" TagPrefix="cmsAcount" %>

<asp:Content ID="Content4" ContentPlaceHolderID="content" Runat="Server">
    <div style="margin:auto;width:100%;">
        <div style="margin:auto; background:#FFFFFF;width:450px;padding:20px;">
            <h1>Je vyžadována změna hesla</h1>
            <cmsAcount:ChangePasswordControl ID="changePassword" runat="server" Width="450px" />	
        </div>
    </div>
</asp:Content>

