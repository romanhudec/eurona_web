<%@ Page Title="Moje cesta ke hvězdám" Language="C#" MasterPageFile="~/user/advisor/angel-team/page.master" AutoEventWireup="true" CodeBehind="mojecesta.aspx.cs" Inherits="Eurona.User.Advisor.AngelTeam.MojeCestaPage" %>
<%@ Register assembly="cms" namespace="CMS.Controls.Page" tagprefix="cmsPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #atmi_mojecesta{background-image: url(../../../images/angel-menuitem-cesta-selected-bg.png);}
        .way-item{font-size:16px; color:#317cb4; font-weight:bold;margin-bottom:10px;}
        .way-item-stars{margin-bottom:70px;}
        .way-item-stars span{font-size:16px;line-height:25px;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <table width="100%">
        <tr>
            <td align="center">
                <div>
                    <div class="way-item">
						<asp:Literal runat="server" Text="<%$ Resources:EShopStrings, ATP_MojePoziceNaCesteKeHvezdam %>"></asp:Literal><br />
                        <asp:Literal runat="server" Text="<%$ Resources:EShopStrings, ATP_PocetMychEuronaStar %>"></asp:Literal>
                    </div>
                    <div class="way-item-stars">
                        <table border="0">
                            <tr>
                                <td valign="middle"><span><asp:Literal runat="server" ID="lblPocetEuronaStar"></asp:Literal></span></td>
                                <td valign="top">
                                    <img id="Img1" runat="server" src="~/images/angel-star.png" width="20" height="19" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
            <td align="center">
                <div>
                    <div class="way-item">
						<asp:Literal runat="server" Text="<%$ Resources:EShopStrings, ATP_PocetEuronaStarProVstup %>"></asp:Literal><br />
                        <asp:Literal runat="server" Text="<%$ Resources:EShopStrings, ATP_DoVIPATP %>"></asp:Literal>
                    </div>
                    <div class="way-item-stars">
                        <table border="0">
                            <tr>
                                <td valign="middle"><span><asp:Literal runat="server" ID="lblPocetEuronaStarProVstup"></asp:Literal></span></td>
                                <td valign="top">
                                    <img id="Img3" runat="server" src="~/images/angel-star.png" width="20" height="19" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
            <td align="center">
                <div>
                    <div class="way-item">
						<asp:Literal runat="server" Text="<%$ Resources:EShopStrings, ATP_PocetEuronaStarProUdrzeni %>"></asp:Literal><br />
                        <asp:Literal runat="server" Text="<%$ Resources:EShopStrings, ATP_VeVIPATP %>"></asp:Literal>
                    </div>
                    <div class="way-item-stars">
                        <table border="0">
                            <tr>
                                <td valign="middle"><span><asp:Literal runat="server" ID="lblPocetEuronaStarProUdrzeni"></asp:Literal></span></td>
                                <td valign="top">
                                    <img id="Img2" runat="server" src="~/images/angel-star.png" width="20" height="19" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="3" align="center">
                <div runat="server" id="divGratulace">
                    <div style="margin:auto;">
                    <div style="text-align:left; font-size:14px;">
						<asp:Literal runat="server" Text="<%$ Resources:EShopStrings, ATP_Gratulujeme1 %>"></asp:Literal><br />
                        <asp:Literal runat="server" Text="<%$ Resources:EShopStrings, ATP_Gratulujeme2 %>"></asp:Literal><br />
                        <asp:Literal runat="server" Text="<%$ Resources:EShopStrings, ATP_VyhodyJakoVIPATP %>"></asp:Literal><br />
                    </div>
                    <div style="width:100%;margin-top:20px;">
                        <img runat="server" src="~/images/angel-cesta.png" width="300" height="198" />
                    </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <div style="margin:20px;">
    	<cmsPage:PageControl ID="genericPage" PageName="angel-team-profesional-mojecesta-content" IsEditing="false" Visible="false" runat="server" CssEditorToolBar="contentEditorToolbar" CssEditorContent="contentEditorContent" NewUrl="~/admin/page.aspx" 
	    ManageUrl="~/admin/pages.aspx" NotFoundUrlFormat="~/notFound.aspx?page={0}" PopUpEditorUrlFormat="~/admin/contentEditor.aspx?id={0}" />
    </div>
</asp:Content>
