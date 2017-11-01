<%@ Page Title="<%$ Resources:Strings, AdvisorDesktop_GrafyLideVSiti %>" Language="C#" MasterPageFile="~/user/advisor/charts/chart.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Eurona.User.Advisor.Charts.Default" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Charting" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content6" ContentPlaceHolderID="content" runat="server">
    <table style="margin:auto;padding-top:10px;">
        <tr>
            <td>
                <telerik:RadChart ID="chartCelekoveProzize" Width="500px" SkinsOverrideStyles="false" runat="server" AutoLayout="true" ChartTitle-TextBlock-Appearance-TextProperties-Font="Helvetica, 14px"
                PlotArea-Appearance-FillStyle-SecondColor="#577491" PlotArea-Appearance-FillStyle-MainColor="#99CCFF" PlotArea-Appearance-FillStyle-FillType="Gradient" >
                </telerik:RadChart>
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadChart ID="chartProvizniHladiny" Width="500px" SkinsOverrideStyles="false" runat="server" AutoLayout="true" ChartTitle-TextBlock-Appearance-TextProperties-Font="Helvetica, 14px"
                PlotArea-Appearance-FillStyle-SecondColor="#577491" PlotArea-Appearance-FillStyle-MainColor="#99CCFF" PlotArea-Appearance-FillStyle-FillType="Gradient" >
                </telerik:RadChart>
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadChart ID="cartVlastniObjednavky" Width="500px" SkinsOverrideStyles="false" runat="server" AutoLayout="true" ChartTitle-TextBlock-Appearance-TextProperties-Font="Helvetica, 14px"
                PlotArea-Appearance-FillStyle-SecondColor="#577491" PlotArea-Appearance-FillStyle-MainColor="#99CCFF" PlotArea-Appearance-FillStyle-FillType="Gradient" >
                </telerik:RadChart>
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadChart ID="cartObratSkupiny" Width="500px" SkinsOverrideStyles="false" runat="server" AutoLayout="true" ChartTitle-TextBlock-Appearance-TextProperties-Font="Helvetica, 14px"
                PlotArea-Appearance-FillStyle-SecondColor="#577491" PlotArea-Appearance-FillStyle-MainColor="#99CCFF" PlotArea-Appearance-FillStyle-FillType="Gradient" >
                </telerik:RadChart>
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadChart ID="cartVyvojSkupinBodu" Width="500px" SkinsOverrideStyles="false" runat="server" AutoLayout="true" ChartTitle-TextBlock-Appearance-TextProperties-Font="Helvetica, 14px"
                PlotArea-Appearance-FillStyle-SecondColor="#577491" PlotArea-Appearance-FillStyle-MainColor="#99CCFF" PlotArea-Appearance-FillStyle-FillType="Gradient" >
                </telerik:RadChart>
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadChart ID="cartLeaderBonus" Width="500px" SkinsOverrideStyles="false" runat="server" AutoLayout="true" ChartTitle-TextBlock-Appearance-TextProperties-Font="Helvetica, 14px"
                PlotArea-Appearance-FillStyle-SecondColor="#577491" PlotArea-Appearance-FillStyle-MainColor="#99CCFF" PlotArea-Appearance-FillStyle-FillType="Gradient" >
                </telerik:RadChart>
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadChart ID="cartVyvojVlastnichNovychRegistraci" Width="500px" SkinsOverrideStyles="false" runat="server" AutoLayout="true" ChartTitle-TextBlock-Appearance-TextProperties-Font="Helvetica, 14px"
                PlotArea-Appearance-FillStyle-SecondColor="#577491" PlotArea-Appearance-FillStyle-MainColor="#99CCFF" PlotArea-Appearance-FillStyle-FillType="Gradient" >
                </telerik:RadChart>
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadChart ID="cartVyvojNovychRegistraciSkupiny" Width="500px" SkinsOverrideStyles="false" runat="server" AutoLayout="true" ChartTitle-TextBlock-Appearance-TextProperties-Font="Helvetica, 14px"
                PlotArea-Appearance-FillStyle-SecondColor="#577491" PlotArea-Appearance-FillStyle-MainColor="#99CCFF" PlotArea-Appearance-FillStyle-FillType="Gradient" >
                </telerik:RadChart>
            </td>
        </tr>
    </table>
</asp:Content>
