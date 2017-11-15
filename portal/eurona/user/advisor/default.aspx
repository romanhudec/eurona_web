<%@ Page Title="" Language="C#" MasterPageFile="~/user/advisor/page.master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Eurona.User.Advisor.DefaultPage" %>
<%@ Register src="CategoryNavigation.ascx" tagname="CategoryNavigation" tagprefix="uc1" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls.Translations" TagPrefix="cmsVocabulary" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls.Page" TagPrefix="cmsPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .content_header{background-image:none!important; border: 0px none #fff!important;}
        .content_content{background-image:none;border:0px none #fff;padding:0px}
        .item-content .report_item{font-size:11px!important; text-decoration:none;color:#878787;}
    </style>
    <script type="text/javascript" language="JavaScript">
        function blink(elmId, containerElmId) {
            blink_x = 1
            blink_y = 1
            ticToc(elmId, containerElmId);
        }
        function ticToc(elmId, containerElmId) {
            var textElm = document.getElementById(elmId);
            var containerElm = document.getElementById(containerElmId);

            if (blink_x == 1) {
                blink_x = 0;
                blink_y++;
                
                containerElm.className  = 'item-blink';
                textElm.className  = 'item-header-blink';
            } else {
                blink_x = 1;
                blink_y++;
                
                containerElm.className  = 'item';
                textElm.className  = 'item-header';
            }
            setTimeout('ticToc("' + elmId + '", "' + containerElmId + '")', 300);
            //if (blink_y < 20) { setTimeout('ticToc("'+elmId +'")', 200); }
        } 
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="category_navigation" runat="server">
    <div class="category-sitemenu-container">
        <uc1:CategoryNavigation ID="categoryNavigation" runat="server" CssClass="category-sitemenu" MenuItemSeparatorImageUrl="~/images/category-menu-item-separator.png" RemoveLastSeparator="true" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="navigation" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content_header" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content" runat="server">
    <cmsPage:PageControl ID="PageControl1" IsEditing="true" runat="server" CssEditorToolBar="contentEditorToolbar" CssEditorContent="contentEditorContent" NewUrl="" 
	ManageUrl="" NotFoundUrlFormat="~/notFound.aspx?page={0}" PageName="advisor-banner-content" PopUpEditorUrlFormat="~/admin/contentEditor.aspx?id={0}" />
<table style="width:100%;" class="advisor-desktop" cellpadding="5" cellspacing="5">
    <tr>
        <td align="center">
            <div class="item">
            <table width="100%" cellpadding="0">
                <tr><td align="center" class="item-header"><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Strings, AdvisorDesktop_AktualniStav %>" /></td></tr>
                <tr>
                    <td class="item-content">
                        <table style="margin:auto;" cellpadding="3" cellspacing="0">
							<tr runat="server" id="trPrehledSkupinyATP" visible="false"><td align="left"><asp:Hyperlink ID="hlPrehledSkupinyATP" CssClass="report_item" runat="server" NavigateUrl="~/user/advisor/reports/prehledProdukceSkupinyATP.aspx" Text="<%$ Resources:Reports, PrehledProdukceSkupinyATP %>"></asp:Hyperlink></td></tr>
                            <tr><td align="left"><asp:Hyperlink ID="Hyperlink2" CssClass="report_item" runat="server" NavigateUrl="~/user/advisor/reports/osobniPrehledPoradce.aspx" Text="<%$ Resources:Reports, OsobniPrehledPoradce %>"></asp:Hyperlink></td></tr>
                            <tr><td align="left"><asp:Hyperlink ID="Hyperlink3" CssClass="report_item" runat="server" NavigateUrl="~/user/advisor/reports/noviPoradci.aspx" Text="<%$ Resources:Reports, NoviPoradci %>"></asp:Hyperlink></td></tr>
                            <tr><td align="left"><asp:Hyperlink ID="Hyperlink4" CssClass="report_item" runat="server" NavigateUrl="~/user/advisor/reports/aktivityReportPoradce.aspx" Text="<%$ Resources:Reports, AktivityReportPoradce %>"></asp:Hyperlink></td></tr>
                            <%--<tr><td align="left"><asp:Hyperlink ID="Hyperlink6" CssClass="report_item" runat="server" NavigateUrl="~/user/advisor/reports/prehledObjednavek.aspx" Text="<%$ Resources:Reports, PrehledObjednavek %>"></asp:Hyperlink></td></tr>
                            <tr><td align="left"><asp:Hyperlink ID="Hyperlink7" CssClass="report_item" runat="server" NavigateUrl="~/user/advisor/reports/historieObjednavek.aspx" Text="<%$ Resources:Reports, HistorieObjednavek %>"></asp:Hyperlink></td></tr>--%>
                        </table>
                    </td>
                </tr>
            </table>
            </div>
        </td>
        <td align="center">
            <div class="item">
            <table width="100%">
                <tr><td align="center" class="item-header"><asp:Literal ID="Literal3" runat="server" Text="Bonusové kredity" /></td></tr>
                <tr><td align="center" class="item-content"><a href="~/user/advisor/bonusovekredity.aspx" runat="server"><img src="../../images/BK_ikona.jpg" width="70px" /></a></td></tr>
                <tr><td align="center"><span style="color:#ff0000;font-weight:bold;font-size:14px;">Stav k čerpání :</span> <span style="color:#ff0000;font-weight:bold;font-size:14px;" runat="server" ID="lblStavBK"></span></td></tr>
            </table>
            </div>
        </td>
        <td align="center">
            <div class="item" id="divOrdersToAssociate">
                <a id="A2" class="item-navigation-container" runat="server" href="~/user/advisor/orderstoassociate.aspx">
                    <table width="100%">
                        <tr><td align="center" id="tdOrdersToAssociate" class="item-header"><asp:Label ID="lblOrdersToAssociate" runat="server" Text="<%$ Resources:Strings, AdvisorDesktop_ObjednavkyProZdruzeni %>" /></td></tr>
                        <tr><td align="center" class="item-content"><img src="../../images/advisor-desktop-objednavky.png" /></td></tr>
                        <tr><td align="center"><asp:Button ID="btnOrdersToAssociate" runat="server" CssClass="item-button" Text="<%$ Resources:Strings, Vice %>" OnClick="OnOrdersToAssociate" /></td></tr>
                    </table>
                </a>
            </div>
            
        </td>
    </tr>
    <tr>
        <td align="center">
            <div class="item">
            <table width="100%">
                <tr><td align="center" class="item-header"><asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:Strings, AdvisorDesktop_AktualniStav %>" /></td></tr>
                <tr>
                    <td class="item-content">
                        <table style="margin:auto;" cellpadding="3" cellspacing="0">
                            <tr><td align="left"><asp:Hyperlink ID="Hyperlink5" CssClass="report_item" runat="server" NavigateUrl="~/user/advisor/reports/prehledObjednavek.aspx" Text="<%$ Resources:Reports, PrehledObjednavek %>"></asp:Hyperlink></td></tr>
                            <tr><td align="left"><asp:Hyperlink ID="Hyperlink1" CssClass="report_item" runat="server" NavigateUrl="~/user/advisor/reports/historieObjednavek.aspx" Text="<%$ Resources:Reports, HistorieObjednavek %>"></asp:Hyperlink></td></tr>
                        </table>
                    </td>
                </tr>
            </table>
            </div>
        </td>
        <td align="center">
            <div class="item">
            <table width="100%">
                <tr><td align="center" class="item-header"><asp:Literal ID="Literal6" runat="server" Text="<%$ Resources:Strings, AdvisorDesktop_GrafyLideVSiti %>" /></td></tr>
                <tr><td align="center"><asp:HyperLink runat="server" NavigateUrl="~/user/advisor/charts/default.aspx"><img src="../../images/advisor-desktop-graf.png" /></asp:HyperLink></td></tr>
            </table>
            </div>
        </td>
        <td align="center">
            <div class="item">
            <table width="100%" runat="server" id="tableMimoradnaNabidka">
                <tr><td align="center" class="item-header"><asp:Literal ID="lblMimoradnaNabidkaTitle" runat="server" /></td></tr>
                <tr><td align="center" class="item-content"><img style="max-height:60px;"  runat="server" id="imgMimoradnaNabidka" alt="Momořádná nabídka"></td></tr>
                <tr><td align="center"><asp:Button ID="btnMimoradnaNabidka" runat="server" CssClass="item-button" Text="<%$ Resources:Strings, Vice %>" OnClick="OnMimoradnaNabidkaClick" /></td></tr>
            </table>
            </div>
        </td>
    </tr>
</table>
<table id="rotatorTable" class="rotator-visibled" width="100%" border="0" cellpadding="0" cellspacing="0" >
<tr>
    <td align="center">
        <div id="newsRotator" class="newsRotator">
            <div id="rotatorContainer" class="container">
                <div><span style="width:660px;color:#1FE8FF;font-size:16px;text-transform:uppercase;"><asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Strings, AdvisorDesktop_YourTopBuyProducts %>" /></span></div>
                <telerik:RadRotator ID="radRotatorNews" runat="server" Width="660px" Height="226px"
                    ScrollDuration="1500" FrameDuration="2000" ItemHeight="226px" ItemWidth="205.8" >
                    <ItemTemplate>
                        <div class="outerWrapper">
                            <a id="A1" runat="server" href='<%# (Container.DataItem as Eurona.Common.DAL.Entities.Product).Alias +"?ReturnUrl=/" %>'>
                                <img style="display:block;border:0 none #fff;max-width:190px; max-height:190px;" src='<%# GetImageSrc((Container.DataItem as  Eurona.Common.DAL.Entities.Product).Id) %>' alt="Customer Image" />
                            </a>
                            <asp:Literal ID="Literal1" runat="server" Text='<%# (Container.DataItem as Eurona.Common.DAL.Entities.Product).Name %>'></asp:Literal>
                            <div>
                                <asp:Button runat="server" ID="btnAddCart" CssClass="products_item_buttonAddCart" CommandName='<%#(Container.DataItem as Eurona.Common.DAL.Entities.Product ).Name%>' CommandArgument='<%#( Container.DataItem as Eurona.Common.DAL.Entities.Product ).Id %>' OnClick="OnAddCart"  />
                            </div>
                        </div>
                    </ItemTemplate>
                </telerik:RadRotator>
            </div>
        </div>
    </td>
</tr>
</table>
</asp:Content>
