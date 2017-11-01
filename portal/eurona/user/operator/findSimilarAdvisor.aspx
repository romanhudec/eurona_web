<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/empty.master" Inherits="Eurona.Operator.FindSimilarAdvisor" Codebehind="findSimilarAdvisor.aspx.cs" %>
<%@ Register Assembly="Eurona" Namespace="Eurona.User.Operator" TagPrefix="operatorCtrl" %>

<asp:Content ID="Content5" ContentPlaceHolderID="navigation" Runat="Server">
	<div class="navigation-links">
		<a id="A1" href="~/default.aspx" runat="server"><asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Strings, Navigation_Home %>" /></a>
		<span>&nbsp;-&nbsp;</span>		
		<a id="A2" href="~/admin/default.aspx" runat="server"><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, Navigation_Administration %>" /></a>
		<span>&nbsp;-&nbsp;</span>
		<asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, Navigation_Users %>" />
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" Runat="Server">
    <style type="text/css">
           .form_label_required{text-align:left;padding-left:15px; padding-right:5px;}
           .form_control{width:auto!important;}
           .form_control span{color:#0092f3!important;}
           .title{color:#eb0a5b;}
    </style>

    <b class="title">KONTROLOVANÝ PORADCE</b>
    <hr/>
    <div>
        <table>
            <tr>
                <td class="form_label_required">Datum registrace:</td>
                <td class="form_control"><asp:Label runat="server" ID="lblRegisterDate"></asp:Label></td>
                <td colspan="4"></td>
            </tr>
            <tr>
                <td class="form_label_required">Kód:</td>
                <td class="form_control"><asp:Label runat="server" ID="lblCode"></asp:Label></td>
                <td class="form_label_required">Jméno:</td>
                <td class="form_control"><asp:Label runat="server" ID="lblName"></asp:Label></td>
                <td class="form_label_required">Email:</td>
                <td class="form_control"><asp:Label runat="server" ID="lblEmail"></asp:Label></td>
            </tr>
            <tr>
                <td class="form_label_required">Telefon:</td>
                <td class="form_control"><asp:Label runat="server" ID="lblPhone"></asp:Label></td>
                <td class="form_label_required">Mobil:</td>
                <td class="form_control"><asp:Label runat="server" ID="lblMobile"></asp:Label></td>
                <td colspan="2"></td>
            </tr>
            <tr>
                <td class="form_label_required">Adresa sídla:</td>
                <td class="form_control"><asp:Label runat="server" ID="lblRegisteredAddress"></asp:Label></td>
                <td class="form_label_required">Korespondenční adresa:</td>
                <td class="form_control"><asp:Label runat="server" ID="lblCorrespondenceAddress"></asp:Label></td>
                <td colspan="2"></td>
            </tr>
        </table>
    </div>
    <br />
    <b class="title">VYHLEDÁNÍ V AKTUÁLNE PLATNÝCH A POVOLENÝCH REGISTRACÍCH</b>
    <hr />
    <operatorCtrl:FindSimilarAdvisorControl runat="server" ID="findSimilarAdvisorControl" CssClass="dataGrid" />
</asp:Content>

