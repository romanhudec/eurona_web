<%@ Page Language="C#" MasterPageFile="~/eshop/default.master" AutoEventWireup="true" CodeBehind="product.aspx.cs" Inherits="Eurona.EShop.Product" %>

<%@ Register Assembly="eurona.common" Namespace="Eurona.Common.Controls.Product" TagPrefix="shpProduct" %>
<%@ Register Assembly="shp" Namespace="SHP.Controls.Category" TagPrefix="shpCategory" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <style type="text/css">
        .pageView{
            border: 1px solid #898c95;
            border-top: none;
            margin-top: -1px;
            height: 100%;
            padding:5px;
        }
    
    </style>

<%--    <shpCategory:CategoryPathControl runat="server" ID="categoryPathControl" />--%>
    <div class="product">
    <shpProduct:ProductControl runat="server" ID="productControl" CssClass="product" CssRating="rating" CommentsFormatUrl="~/eshop/productComments.aspx?id={0}">
        <ProductTemplate />
<%--        <ProductTemplate>    
            <table runat="server" id="tb" >
                <tr>
                    <td><%# Eval("Name%></td>
                </tr>
            </table>
        </ProductTemplate>--%>
        
    </shpProduct:ProductControl>
        <table border="0">
            <tr>
                <td>
                    <b style="font-size:18px;"><%=ProductEntity.Name%></b>
                </td>
            </tr>
            <tr>
                <td>
                    <img src="../images/Piktogramy/piktogram_t.png" alt="Piktogram"/>
                </td>
            </tr>
            <tr style="height:30px;">
                <td align="left">
                    <div runat="server" id="ratingContainer"></div>
                    <div runat="server" id="commentsContainer"></div>
                </td>
            </tr>
            <tr>
                <td align="center"><asp:Literal runat="server" ID="Literal9" Text="<%$ Resources:EShopStrings, ProductControl_Code %>"></asp:Literal>&nbsp;<%=ProductEntity.Code%></td>
            </tr>       
            <tr>
                <td align="center">
                    <div style="width:330px;" runat="server" id="imgGalleryContainer"></div>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center" style="vertical-align:top;">
                    <%=ProductEntity.Description%>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center" style="vertical-align:top;">
                    <span>
                        <b><%=GetProductPrice( ProductEntity.PriceTotalWVAT, ProductEntity.CurrencySymbol )%></b>
                    </span>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center" style="vertical-align:top;">
                    <span>
                        <asp:Literal runat="server" ID="Literal10" Text="<%$ Resources:EShopStrings, ProductControl_Points %>"></asp:Literal>&nbsp;<%=(int?)ProductEntity.Body%>
                    </span>
                </td>
            </tr>


            <tr runat="server" Id="trParfumacie">
                <td colspan="2" style="vertical-align:top;">
                    <span class="property"><asp:Label ID="Literal7" runat="server" Text="<%$ Resources:EShopStrings, ProductControl_Parfumation %>"></asp:Label></span>
                    <hr />
                    <%=RenderParfumacia(ProductEntity.Parfumacia.Value)%>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="vertical-align:top;">
                    <span class="property"><asp:Label runat="server" Text="<%$ Resources:EShopStrings, ProductControl_Description %>"></asp:Label></span>
                    <hr />
                    <%=ProductEntity.DescriptionLong%>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="vertical-align:top;">
                    <span class="property"><asp:Label ID="Literal3" runat="server" Text="<%$ Resources:EShopStrings, ProductControl_AdditionalInformation %>"></asp:Label></span>
                    <hr />
                    <%=ProductEntity.AdditionalInformation%>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="vertical-align:top;">
                    <span class="property"><asp:Label ID="Literal4" runat="server" Text="<%$ Resources:EShopStrings, ProductControl_SpecialEffects %>"></asp:Label></span>
                    <hr />
                    <asp:Repeater ID="rpUcinkyProduktu" runat="server" >
                        <ItemTemplate>
                            <img src='<%# "../images/SpecialneUcinky/" + Eval( "ImageUrl" )%>' alt="" width="70px" />
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="vertical-align:top;">
                    <span class="property"><asp:Label ID="Literal5" runat="server" Text="<%$ Resources:EShopStrings, ProductControl_Pictograms %>"></asp:Label></span>
                    <hr />
                    <asp:Repeater ID="rpPiktogramyProduktu" runat="server" >
                        <ItemTemplate>
                            <%#Eval("Name") %>
                            <img src='<%# "../images/Piktogramy/" + Eval( "ImageUrl" )%>' alt="" width="70px" />
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="vertical-align:top;">
                    <span class="property"><asp:Label ID="Literal6" runat="server" Text="<%$ Resources:EShopStrings, ProductControl_Properties %>"></asp:Label></span>
                    <hr />
                    <asp:Repeater ID="rpVlastnostiProduktu" runat="server" >
                        <ItemTemplate>
                            <%#Eval("Name") %>
                            <img src='<%# "../images/Vlastnosti/" + Eval( "ImageUrl" )%>' alt="" width="70px" />
                        </ItemTemplate>
                    </asp:Repeater>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="vertical-align:top;">
                    <span class="property"><asp:Label ID="Label1" runat="server" Text="<%$ Resources:EShopStrings, ProductControl_InstructionsForUse %>"></asp:Label></span>
                    <hr />
                    <%=ProductEntity.InstructionsForUse%>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="vertical-align:top;">
                    <span class="property"><asp:Label ID="Label2" runat="server" Text="<%$ Resources:EShopStrings, ProductControl_AlternateProducts %>"></asp:Label></span>
                    <hr />
                    <asp:ListView runat="server" ID="lvAlternateProducts" GroupItemCount="3">
                        <LayoutTemplate>
                            <table>
                                <asp:PlaceHolder runat="server" ID="groupPlaceholder"></asp:PlaceHolder>
                            </table>
                        </LayoutTemplate>
                        <GroupTemplate>
                            <tr>
                                <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                            </tr>
                        </GroupTemplate>
                        <ItemTemplate>
                            <td style="width:30%; height:300px;">
                            <table border="0" style="height:100%;margin:0px 5px 0px 5px;border:1px solid #EEE;">
                            <tr>
                                <td align="center">
                                    <%#RenderImage( Convert.ToInt32(Eval("ProductId")) )%>
                                </td>
                            </tr>
                            <tr>
                                <td style="vertical-align:top;">
                                    <table border="0">
                                    <tr>
                                        <td style="vertical-align:top;padding-bottom:5px;text-align:justify;">
                                            <asp:Literal ID="Literal2" runat="server" Text='<%#Eval("ProductName") %>'></asp:Literal>
                                        </td>
                                    </tr>          
                                    <tr>
                                        <td style="vertical-align:top;" align="center">
                                            <span>
                                                <b><%#GetProductPrice( Convert.ToDecimal( Eval( "PriceTotalWVAT" ) ), Eval( "CurrencySymbol" ).ToString() )%></b>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <a href='<%#ResolveUrl(Eval("Alias").ToString() + "?ReturnUrl="+this.Request.RawUrl) %>'>podrobnosti</a>
                                        </td>
                                    </tr>
                                    </table>
                                </td>
                            </tr>
                            </table>
                            </td>
                        </ItemTemplate>
                        <EmptyDataTemplate>
                            <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:EShopStrings, ProductControl_NoProductFound %>"></asp:Literal>
                        </EmptyDataTemplate>                        
                    </asp:ListView>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right"><a href='<%=productControl.ReturnUrl %>'><asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:strings, BackLink %>"></asp:Literal></a></td>
            </tr> 
        </table>
    </div>
<%--    </cms:RoundPanel>--%>
</asp:Content>
