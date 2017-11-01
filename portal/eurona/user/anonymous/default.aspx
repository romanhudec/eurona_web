<%@ Page Title="" Language="C#" MasterPageFile="~/user/anonymous/page.master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Eurona.User.Anonymous._default" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls.Page" TagPrefix="cmsPage" %>
<%@ Register Assembly="Eurona" Namespace="Eurona.Controls" TagPrefix="ctrls" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register src="CartControl.ascx" tagname="CartControl" tagprefix="uc1" %>
<%@ Register Assembly="cms" Namespace="CMS.Utilities" TagPrefix="cmsUtilities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        #rn_cart{background-image:url(../../images/anonymous-register-navigation-item-selected.png);}
        #rn_cart a{color:#36AFE2;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="navigation" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="content_header" runat="server">
	<asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_NakupniKosik %>"></asp:Literal>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
 <script type="text/javascript">
     setTimeout("setFocusToCartCodeEdit()", 1000);
     function setFocusToCartCodeEdit() {
         if (document.getElementById)
             document.getElementById("ctl00_content_txtKod").focus();
         else if (document.all)
             document.all("ctl00_content_txtKod").focus();
         return false;
     }
    </script>
    <span style="color:#e2008b;">
	<asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_Vitejte %>"></asp:Literal>
    </span>
    <div>
        <cmsPage:PageControl ID="PageControl1" IsEditing="true" runat="server" CssEditorToolBar="contentEditorToolbar" CssEditorContent="contentEditorContent" NewUrl="" 
	    ManageUrl="" NotFoundUrlFormat="~/notFound.aspx?page={0}" PageName="anonymous-cart-banner-content" PopUpEditorUrlFormat="~/admin/contentEditor.aspx?id={0}" />
    </div>
    <table>
        <tr>
            <td><asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:EShopStrings, CartControl_ProductCode %>"></asp:Literal></td>
            <td><asp:TextBox ID="txtKod" runat="server" Width="50px"></asp:TextBox></td>
            <td><asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:EShopStrings, CartControl_ProductQuantity %>"></asp:Literal></td>
            <td><asp:TextBox ID="txtMnozstvi" runat="server" Width="30px"></asp:TextBox></td>
            <td><asp:Button ID="btnAdd" runat="server" Text="<%$ Resources:EShopStrings, CartControl_Add %>" OnClick="OnAddCart" CssClass="button"></asp:Button></td>
        </tr>
    </table>
    <uc1:CartControl ID="cartControl" runat="server" CssClass="dataGrid" />
	<div style="margin-top:10px; margin-bottom:20px;">
		<span style="color:#00AF00; font-size:16px; font-weight:bold;"><asp:Literal runat="server" ID="lblPostovneInfo"></asp:Literal></span>
	</div>    

    <div style="margin-top:10px;">
        <cmsPage:PageControl ID="PageControl2" IsEditing="true" runat="server" CssEditorToolBar="contentEditorToolbar" CssEditorContent="contentEditorContent" NewUrl="" 
	    ManageUrl="" NotFoundUrlFormat="~/notFound.aspx?page={0}" PageName="anonymous-cart-banner2-content" PopUpEditorUrlFormat="~/admin/contentEditor.aspx?id={0}" />
    </div>

    <div style="margin-top:10px;">
        <span>
            <a class="add-button-150" href='<%=aliasUtilities.Resolve("~/eshop/default.aspx") %>'><asp:Literal runat="server" Text="<%$ Resources:EShopStrings, Anonymous_PridatDalsiVyrobky %>"></asp:Literal></a>
            <asp:HyperLink runat="server" id="btnContinue" class="button" Text="<%$ Resources:EShopStrings, Anonymous_PokracovatVObjednavce %>"></asp:HyperLink>
        </span>
    </div>

	<div>
		<table id="rotatorTable" class="anonymous-rotator-visibled" width="100%" border="0" cellpadding="0" cellspacing="0" >
		<tr>
			<td align="center">
				<div id="newsRotator" class="anonymousNewsRotator">
					<div id="rotatorContainer" class="container">
						<div style="height:50px; width:100%;">
							<span style="width:950px;color:#1FE8FF;font-size:16px;text-transform:uppercase; height:50px;">
								<cmsPage:PageControl ID="PageControl3" IsEditing="true" runat="server" CssEditorToolBar="contentEditorToolbar" CssEditorContent="contentEditorContent" NewUrl="" 
								ManageUrl="" NotFoundUrlFormat="~/notFound.aspx?page={0}" PageName="anonymous-cart-rotator-content" PopUpEditorUrlFormat="~/admin/contentEditor.aspx?id={0}" />
							</span>
						</div>
						<telerik:RadRotator ID="radRotatorNews" runat="server" Width="950px" Height="226px"
							ScrollDuration="1500" FrameDuration="2000" ItemHeight="226px" ItemWidth="300px" >
							<ItemTemplate>
								<div class="outerWrapper">
									<a id="A1" runat="server" href='<%# (Container.DataItem as Eurona.Common.DAL.Entities.Product).Alias +"?ReturnUrl=/" %>'>
										<img style="display:block;border:0 none #fff;max-width:190px; max-height:190px;" src='<%# GetImageSrc((Container.DataItem as  Eurona.Common.DAL.Entities.Product).Id) %>' alt="Customer Image" />
									</a>
									<asp:Literal ID="Literal3" runat="server" Text='<%# (Container.DataItem as Eurona.Common.DAL.Entities.Product).Name %>'></asp:Literal>
									<div>
										<asp:Button runat="server" ID="btnAddCart" CssClass="products_item_buttonAddCart" CommandName='<%#(Container.DataItem as Eurona.Common.DAL.Entities.Product ).Name%>' CommandArgument='<%#( Container.DataItem as Eurona.Common.DAL.Entities.Product ).Id %>' OnClick="OnAddCartFromRotator"  />
									</div>
								</div>
							</ItemTemplate>
						</telerik:RadRotator>
					</div>
				</div>
			</td>
		</tr>
		</table>
	</div>
    <%--UTILITIES--%>
    <cmsUtilities:AliasUtilities ID="aliasUtilities" runat="server" ></cmsUtilities:AliasUtilities>  
</asp:Content>
