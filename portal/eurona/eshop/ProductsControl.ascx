<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductsControl.ascx.cs" Inherits="Eurona.EShop.ProductsControl" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>
<%@ Register Assembly="shp" Namespace="SHP.Controls.Category" TagPrefix="shpCategory" %>
<%@ Register Assembly="eurona.common" Namespace="Eurona.Common.Controls.Product" TagPrefix="shpProduct" %>

<script language="javascript" type="text/javascript">
    function doAddToCart(e, sender) {
        if (!e) var e = window.event;
        if (e.which || e.keyCode) {
            if ((e.which == 13) || (e.keyCode == 13)) {
                var buttonId = sender.id.replace('txtQuantity', 'btnAddCart');

                var button = document.getElementById(buttonId);
                button.click();
                //__doPostBack( button.uniqueID,''); 
                return false;
            }
        } else return true
    }
</script>
 <shpProduct:ProductsControl runat="server" ID="productsControl" CssClass="products" RepeatColumns="4" AllowPaging="true" PageSize="3" DisplayUrlFormat="~/eshop/product.aspx?id={0}" >
    <ProductsGroupTemplate>
        <tr><asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder></tr>
    </ProductsGroupTemplate>
    <ProductsItemTemplate>
        <td valign="top" style="width:18%;padding:10px 20px 0px 20px;">
            <%--<table border="0" width="200px" style="margin:auto;height:100%;">--%>
            <table border="0" style="margin:auto;height:100%;width:155px;">
            <tr>
                <td colspan="2" align="center"><asp:Literal runat="server" ID="Literal9" Text="<%$ Resources:EShopStrings, ProductControl_Code %>"></asp:Literal>&nbsp;<%#(Container.DataItem as SHP.Entities.Product ).Code%></td>
            </tr>
            <tr>
                <td colspan="2" align="center" style="height:150px;">
                    <a runat="server" href='<%#(Container.DataItem as SHP.Entities.Product ).Alias + "?ReturnUrl="+this.Request.RawUrl %>'>
                    <%#RenderImage( ( Container.DataItem as SHP.Entities.Product ).Id )%>
                    </a>
                </td>
            </tr>
            <tr>
                <td align="justify" colspan="2" style="color:#102f71;vertical-align:top;padding-bottom:3px;height:30px;text-align:center;">
                    <asp:Literal ID="Literal1" runat="server" Text="<%#(Container.DataItem as SHP.Entities.Product ).Name %>"></asp:Literal>
                </td>
            </tr>             
            <tr style="height:100%;">
                <td runat="server" colspan="0" style="vertical-align:top; line-height:20px;" align="center" >
                    <asp:LinkButton ID="LinkButton1" runat="server" Text="info" CommandArgument='<%#(Container.DataItem as SHP.Entities.Product ).Alias + "?ReturnUrl="+this.Request.RawUrl %>' OnClick="OnDetail"></asp:LinkButton>
                </td>
                <td runat="server" style="vertical-align:top; line-height:20px;" align="right">
                    <asp:Literal runat="server" Text="<%$ Resources:EShopStrings, ProductControl_Quantity %>"></asp:Literal><asp:TextBox runat="server" ID="txtQuantity" onkeydown="" Width="30px"></asp:TextBox>
                    <asp:Button runat="server" ID="btnAddCart" CssClass="products_item_buttonAddCart" CommandName='<%#(Container.DataItem as SHP.Entities.Product ).Name%>' CommandArgument='<%#( Container.DataItem as SHP.Entities.Product ).Id %>' OnClick="OnAddCart"  />
                </td>
            </tr> 
        </table>                                                           
        </td>
    </ProductsItemTemplate>
    <ProductsLayoutTemplate>
        <div style="padding:10px 5px 10px 20px;">
            <table border="0" style="width:100%;height:400px">
                <tr>
                    <%--PAGER LEFT--%>
                    <td><div class="product-pager-left"></div></td>
                    <td valign="middle">
                        <asp:DataPager runat="server" ID="dataPager" PageSize="8" PagedControlID="<%#Container.ID %>">
                            <Fields>
                                <asp:NextPreviousPagerField ButtonType="Image" RenderDisabledButtonsAsLabels="true" PreviousPageText="" PreviousPageImageUrl="~/images/rrButtonLeft.png" ShowFirstPageButton="false" ShowPreviousPageButton="true" ShowLastPageButton="false"  ShowNextPageButton ="false"/>
                                    <asp:TemplatePagerField> 
                                        <PagerTemplate></PagerTemplate>
                                </asp:TemplatePagerField>
                            </Fields>
                        </asp:DataPager>
                    </td>
                    <%--PRODUCTS--%>
                    <td valign="top" style="width:100%;">
                        <table border="0" cellpadding="0" cellspacing="0" >
                            <asp:PlaceHolder runat="server" ID="groupPlaceholder"></asp:PlaceHolder>
                        </table>
                    </td>
                    <%--PAGER RIGHT--%>
                    <td valign="middle">
                        <asp:DataPager runat="server" ID="dataPager1" PageSize="8" PagedControlID="<%#Container.ID %>">
                            <Fields>
                                <asp:NextPreviousPagerField ButtonType="Image" RenderDisabledButtonsAsLabels="true" NextPageText="" NextPageImageUrl="~/images/rrButtonRight.png" ShowFirstPageButton="false" ShowPreviousPageButton="false" ShowLastPageButton="false"  ShowNextPageButton ="true"/>
                                    <asp:TemplatePagerField> 
                                        <PagerTemplate></PagerTemplate>
                                </asp:TemplatePagerField>
                            </Fields>
                        </asp:DataPager>
                    </td>
                    <td><div class="product-pager-right"></div></td>
                </tr>
            </table>
        </div>
    </ProductsLayoutTemplate>
</shpProduct:ProductsControl>   