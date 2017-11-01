<%@ Page Language="C#" MasterPageFile="~/eshop/default.master" AutoEventWireup="true" CodeBehind="product.aspx.cs" Inherits="Eurona.EShop.Product" %>

<%@ Register Assembly="eurona.common" Namespace="Eurona.Common.Controls.Product" TagPrefix="shpProduct" %>
<%@ Register Assembly="shp" Namespace="SHP.Controls.Category" TagPrefix="shpCategory" %>
<%@ Register Assembly="cms" Namespace="CMS.Controls" TagPrefix="cms" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register src="CartInfoControl.ascx" tagname="CartInfoControl" tagprefix="uc1" %>
<asp:Content ID="head" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .pageView{
            border: 2px solid #66D0F4;
            margin-top: 4px;
            margin-left:-2px;
            height: 100%;
            text-align:left;
        }
    
    </style>
    <script type="text/javascript">
        /* <![CDATA[ */
        var seznam_retargeting_id = 38225;
        /* ]]> */
    </script>
    <script type="text/javascript" src="//c.imedia.cz/js/retargeting.js"></script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="content" runat="server">
    <shpProduct:ProductControl runat="server" ID="productControl" CssClass="product_" CssRating="rating" CommentsFormatUrl="~/eshop/productComments.aspx?id={0}">
        <ProductTemplate />
<%--        <ProductTemplate>    
            <table runat="server" id="tb" >
                <tr>
                    <td><%# Eval("Name%></td>
                </tr>
            </table>
        </ProductTemplate>--%>
        
    </shpProduct:ProductControl>
 <div class="product">
    <table border="0" width="100%">
        <tr>
            <td valign="top">
                <table width="100%" border="0" style="margin-right:10px;">
                    <tr><td colspan="3" align="center"><b style="font-size:16px;color:#E2008B;font-weight:bold;"><%=ProductEntity.Name%></b></td></tr>
                    <tr>
                        <td colspan="3" align="center"><asp:Literal runat="server" ID="Literal9" Text="<%$ Resources:EShopStrings, ProductControl_Code %>"></asp:Literal>&nbsp;<%=ProductEntity.Code%></td>
                    </tr>       
                    <tr>
                        <td colspan="3" align="center">
                            <div style="width:500px;" runat="server" id="imgGalleryContainer"></div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="justify" style="vertical-align:top;text-align:justify;">
                            <%=ProductEntity.Description%>
                        </td>
                    </tr>
                    <tr>
                        <td runat="server" id="tdPiktogramyProduktu">
                            <img runat="server" id="imgPiktogram" src="../images/Piktogramy/piktogram_t.png" alt="Piktogram"/>
                            <asp:Repeater ID="rpPiktogramyProduktu" runat="server" >
                                <ItemTemplate>
                                    <div>
                                    <img src='<%# "../images/Piktogramy/" + Eval( "ImageUrl" )%>' alt="" width="200 px" />
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </td>
                        <td rowspan="2">
                            <div>
                            <a href='#' title="Pošli příteli email" style="text-decoration:none;" onclick="window.open('/sendToFriend.aspx?productId=<%=ProductEntity.Id %>','myWindow','width=400,height=160,toolbar=no, location=no,directories=no,status=yes,menubar=no,scrollbars=yes,copyhistory=no, resizable=no');" target="_self">
                                <img id="Img1" runat="server" src="~/images/sendtofriend.png" alt="Pošli příteli email" style="border:0px none #fff;" />
                            </a>
                            </div>
                            <div>
                            <asp:LinkButton runat="server" ID="btnFacebook" OnClick="OnSendToFacebook" ToolTip="Přidej na svůj facebook">
                                <img id="Img2" runat="server" src="~/images/facebook.png" alt="Přidej na svůj facebook" style="border:0px none #fff;" />
                            </asp:LinkButton>
                            </div>
                        </td>                       
                        <td rowspan="2">
                            <div style="background-image:url(../images/product-cart-bg.png); background-repeat:repeat-x ;width:180px;height:120px;padding-top:10px;">
                                <table cellpadding="5" style="margin-top:10px;">
                                    <tr>
                                        <td colspan="2">
                                            <span style="color:#EA008A;font-weight:bold;">
                                                <span style="color:#00b8ec;">Cena:</span>&nbsp;<%=GetProductPrice( ProductEntity.PriceTotalWVAT, ProductEntity.CurrencySymbol )%>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <span style="color:#EA008A;font-weight:bold;">
                                                <span style="color:#00b8ec;">Body:</span>&nbsp;<%=(int?)ProductEntity.Body%>
                                            </span>
                                        </td>
                                        <td rowspan="2">
                                            <div style="margin-left:20px;">
                                                <asp:ImageButton runat="server" ID="btnAddCart" ImageUrl="~/images/cart-button.png" OnClick="OnAddCart" ToolTip="Do košíku" CausesValidation="false" CommandName="AddCart" CommandArgument='<%=ProductEntity.Id %>' />
                                            </div> 
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtQuantity" Width="30px"></asp:TextBox> Ks   
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>                       
                    </tr>
                    <tr>
                        <td align="left">
                            <div runat="server" id="ratingContainer">
                                <table>
                                    <tr>
                                        <td align="center"><%=(((decimal)ProductEntity.RatingResult * 100m ) / 5).ToString("F2")%>%</td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <%--OnRate="ratingControl_Rate"--%>
                                        <td><telerik:RadRating ID="ratingControl" runat="server" AutoPostBack="true"  ItemCount="5" Value="3" SelectionMode="Continuous" Precision="Half" Orientation="Horizontal" /></td>
                                        <td> Hodnocení :  <%=ProductEntity.TotalRating%></td>
                                    </tr>
                                </table>
                            </div>
                            <div runat="server" id="commentsContainer"></div>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top" align="center" style="width:460px;">
                <div style="float:right;margin-right:7px;">
					<span style="color:#e2008b;font-size:16px;"><asp:Literal runat="server" ID="lblLimitovanaAkce"></asp:Literal></span>
                </div>
                <telerik:RadTabStrip ID="RadTabStrip1" runat="server" Skin="_Vista" MultiPageID="RadMultiPage1"
                    SelectedIndex="0" ReorderTabsOnSelect="true">
                    <Tabs>
                        <telerik:RadTab Text="<%$ Resources:EShopStrings, ProductControl_PrednostiAVlastnosti %>">
                        </telerik:RadTab>
                        <telerik:RadTab Text="<%$ Resources:EShopStrings, ProductControl_AditionalInfo %>">
                        </telerik:RadTab>
                        <telerik:RadTab Text="<%$ Resources:EShopStrings, ProductControl_AlternativeProducts %>">
                        </telerik:RadTab>
                    </Tabs>
                </telerik:RadTabStrip>
                <telerik:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="0" Width="408px" Height="350px" ScrollBars="Auto">
                <telerik:RadPageView ID="RadPageView1" runat="server" CssClass="pageView">
                    <table width="100%" style="padding:5px;">
                    <tr>
                        <td style="vertical-align:top; text-align:justify;">
                            <%-- <span class="property"><asp:Label ID="Label1" runat="server" Text="<%$ Resources:EShopStrings, ProductControl_Description %>"></asp:Label></span>
                            <hr />--%>
                            <%=ProductEntity.DescriptionLong%>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" runat="server" id="tdVlastnostiProduktu" style="vertical-align:top;">
                            <%-- <span class="property"><asp:Label ID="lblVlastnostiProduktu" runat="server" Text="<%$ Resources:EShopStrings, ProductControl_Properties %>"></asp:Label></span>
                            <hr />--%>
                            <asp:Repeater ID="rpVlastnostiProduktu" runat="server" >
                                <ItemTemplate>
                                    <div style="width:370px;">
                                    <%#Eval("Name") %>
                                    <img src='<%# "../images/Vlastnosti/" + Eval( "ImageUrl" )%>' alt="" style="max-width:50%;" />
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
                    </table>
                </telerik:RadPageView>
                <telerik:RadPageView ID="RadPageView2" runat="server" CssClass="pageView">
                    <table width="100%" style="padding:5px;">
                    <tr>
                        <td colspan="2" runat="server" id="tdZadniEtiketa" style="vertical-align:top;">
                            <span class="property"><asp:HyperLink ID="lblZadniEtiketa" runat="server" Text="<%$ Resources:EShopStrings, ProductControl_BackList %>"></asp:HyperLink></span>
                            <br />

                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" runat="server" id="tdAdditionalInformation" style="vertical-align:top;text-align:justify;">
                            <span class="property"><asp:Label ID="Literal3" runat="server" Text="<%$ Resources:EShopStrings, ProductControl_AdditionalInformation %>"></asp:Label></span>
                            <hr />
                            <%=ProductEntity.AdditionalInformation%>
                        </td>
                    </tr>
                    <tr>
                        <td runat="server" id="tdUcinkyProduktu" colspan="2" style="vertical-align:top;">
                            <span class="property"><asp:Label ID="lblUcinkyProduktu" runat="server" Text="<%$ Resources:EShopStrings, ProductControl_SpecialEffects %>"></asp:Label></span>
                            <hr />
                            <asp:Repeater ID="rpUcinkyProduktu" runat="server" >
                                <ItemTemplate>
                                    <img src='<%# "../images/SpecialneUcinky/" + Eval( "ImageUrl" )%>' alt="" width="70px" />
                                </ItemTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
<%--                    <tr>
                        <td colspan="2" runat="server" id="tdPiktogramyProduktu" style="vertical-align:top;">
                            <span class="property"><asp:Label ID="lblPiktogramyProduktu" runat="server" Text="<%$ Resources:EShopStrings, ProductControl_Pictograms %>"></asp:Label></span>
                            <hr />
                            <asp:Repeater ID="rpPiktogramyProduktu" runat="server" >
                                <ItemTemplate>
                                    <%#Eval("Name") %>
                                    <img src='<%# "../images/Piktogramy/" + Eval( "ImageUrl" )%>' alt="" width="70px" />
                                </ItemTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>--%>
                    <tr>
                        <td colspan="2" runat="server" id="tdInstructionsForUse" style="vertical-align:top;text-align:justify;">
                            <span class="property"><asp:Label ID="lblInstructionsForUse" runat="server" Text="<%$ Resources:EShopStrings, ProductControl_InstructionsForUse %>"></asp:Label></span>
                            <hr />
                            <%=ProductEntity.InstructionsForUse%>
                        </td>
                    </tr>
                    </table>                        
                </telerik:RadPageView>
                <telerik:RadPageView ID="RadPageView3" runat="server" CssClass="pageView">
                    <div style="padding:5px;">
                    <asp:ListView runat="server" ID="lvAlternateProducts" GroupItemCount="2">
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
                            <table border="0" style="height:100%;margin:0px 5px 0px 5px;border:0px solid #EEE;">
                            <tr>
                                <td align="center">
                                    <a id="A1" runat="server" href='<%#ResolveUrl(Eval("Alias").ToString() + "?ReturnUrl="+this.Request.RawUrl) %>'>
                                        <%#RenderImage( Convert.ToInt32(Eval("ProductId")) )%>
                                    </a>
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
                    </div>                       
                </telerik:RadPageView>
            </telerik:RadMultiPage>
            <div runat="server" Id="trParfumacie" style="margin-top:5px;text-align:left;">
                <span class="property" style="padding-left:25px;display:block;" ><asp:Label ID="Literal7" runat="server" Text="<%$ Resources:EShopStrings, ProductControl_Parfumation %>"></asp:Label></span>
                <div style="margin-top:3px;margin-left:10px;"><%=RenderParfumacia(ProductEntity.Parfumacia.Value)%></div>
            </div>
            <div style="float:right;margin-top:20px;">
                <a href='<%=productControl.ReturnUrl %>'><asp:Literal ID="Literal8" runat="server" Text="<%$ Resources:EShopStrings, BackToCatalog %>"></asp:Literal></a>
            </div>
            </td>
        </tr>
    </table>
    </div>
<%--    </cms:RoundPanel>--%>
</asp:Content>
