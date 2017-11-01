<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderControl.ascx.cs" Inherits="Eurona.User.Anonymous.OrderControl" %>
<div runat="server" id="gridDivControl">
</div>
<asp:UpdatePanel runat="server" ID="updatePanelCart">
    <ContentTemplate>
    <div runat="server" id="buttonsDivControl">
    </div>
    </ContentTemplate>
</asp:UpdatePanel>