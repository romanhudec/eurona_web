<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HighlightsProductsControl.ascx.cs" Inherits="Eurona.EShop.HighlightsProductsControl" %>
<%@ Register Assembly="shp" Namespace="SHP.Controls.Product" TagPrefix="shpProduct" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>

<shpProduct:HighlightsProductsControl runat="server" ID="highlightsProductsControl" CssClass="products" DisplayUrlFormat="~/eshop/product.aspx?id={0}">
    <ProductsGroupTemplate>
        <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
    </ProductsGroupTemplate>
    <ProductsItemTemplate>
        <cms:RoundPanel ID="rpProduct" runat="server" CssClass="roundPanel" Text="">
        <table border="0" cellpadding="0" cellspacing="5" width="100%">
            <tr>
                <td style="background-color:#fff;width:100%; height:100%;">
                    <div style="display:inline;position:absolute;"><%#RenderHighlightImage(( Container.DataItem as SHP.Entities.Product ).Id)%></div>
                    <%#RenderImage( ( Container.DataItem as SHP.Entities.Product ).Id )%>
                </td>
            </tr>
            <tr>
                <td style="vertical-align:top;font-weight:bold;">
                    <a id="A1" runat="server" href='<%#(Container.DataItem as SHP.Entities.Product ).Alias + "?ReturnUrl="+this.Request.RawUrl %>'><%#(Container.DataItem as SHP.Entities.Product ).Name %></a>
                </td>
            </tr>
            <tr>
                <td style="vertical-align:top;">
                    <%#(Container.DataItem as SHP.Entities.Product ).Description %>
                </td>
            </tr>
            <tr>
                <td>
                   <span style="font-weight:bold;">
                        <%#GetProductPrice( ( Container.DataItem as SHP.Entities.Product ).PriceTotal )%>
                   </span>
               </td>
            </tr>
            <tr>
                <td>
                   <span style="font-weight:bold;">
                        <%#GetProductPrice( ( Container.DataItem as SHP.Entities.Product ).PriceTotalWVAT )%> s DPH
                   </span>
               </td>
            </tr>                            
            <tr style="height:100%;">
                <td style="vertical-align:top;">
                   <asp:Label ID="Label1" runat="server" Text="Počet kusů :" CssClass="products_item_labelQuantity"/>
                   <asp:TextBox runat="server" ID="txtQuantity" Text="1" CssClass="products_item_inputQuantity" />
                   <asp:Button runat="server" ID="btnAddCart" ToolTip='<%# GetShpResourceString( "AdminProductControl_AddProductToCart_ToolTip") %>' CssClass="products_item_buttonAddCart" OnClick="OnAddCart" CommandName='<%#( Container.DataItem as SHP.Entities.Product ).Name %>' CommandArgument='<%#( Container.DataItem as SHP.Entities.Product ).Id %>' />
                </td>
            </tr>                                                            
        </table>
        </cms:RoundPanel>
    </ProductsItemTemplate>
    <ProductsLayoutTemplate>
        <asp:PlaceHolder runat="server" ID="groupPlaceholder"></asp:PlaceHolder>
    </ProductsLayoutTemplate>	            
</shpProduct:HighlightsProductsControl>