using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Controls;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ProductsEntity = SHP.Entities.Product;
using System.IO;
using SHP.Controls.Cart;

namespace SHP.Controls.Product
{
		public class ProductsControl: CmsControl
		{
				//private ProductsFilterControl filterControl = null;
				private ListView listView = null;
				private Label lblResultCount = null;

				public ProductsControl()
				{
						this.RepeatColumns = 1;
						this.AllowPaging = true;
						this.PageSize = 20;
				}

				public string DisplayUrlFormat { get; set; }
				public int RepeatColumns { get; set; }
				public bool AllowPaging { get; set; }
				public int PageSize { get; set; }

				public int CategoryId
				{
						get
						{
								object o = ViewState["CategoryId"];
								return o != null ? (int)Convert.ToInt32( o ) : 0;
						}
						set { ViewState["CategoryId"] = value; }
				}

				public ProductsEntity.ReadByFilter Filter { get; set; }
				//public ProductsEntity.ReadHighlights Highlights { get; set; }

				[Browsable( false )]
				[DefaultValue( null )]
				[Description( "Products Layout template." )]
				[TemplateContainer( typeof( ListViewDataItem ) )]
				[PersistenceMode( PersistenceMode.InnerProperty )]
				public virtual ITemplate ProductsLayoutTemplate { get; set; }

				[Browsable( false )]
				[DefaultValue( null )]
				[Description( "Products Group template." )]
				[TemplateContainer( typeof( ListViewDataItem ) )]
				[PersistenceMode( PersistenceMode.InnerProperty )]
				public virtual ITemplate ProductsGroupTemplate { get; set; }

				[Browsable( false )]
				[DefaultValue( null )]
				[Description( "Products Group Separator template." )]
				[TemplateContainer( typeof( ListViewDataItem ) )]
				[PersistenceMode( PersistenceMode.InnerProperty )]
				public virtual ITemplate ProductsGroupSeparatorTemplate { get; set; }

				[Browsable( false )]
				[DefaultValue( null )]
				[Description( "Products Item template." )]
				[TemplateContainer( typeof( ListViewDataItem ) )]
				[PersistenceMode( PersistenceMode.InnerProperty )]
				public virtual ITemplate ProductsItemTemplate { get; set; }

				[Browsable( false )]
				[DefaultValue( null )]
				[Description( "Products Item separator template." )]
				[TemplateContainer( typeof( ListViewDataItem ) )]
				[PersistenceMode( PersistenceMode.InnerProperty )]
				public virtual ITemplate ProductsItemSeparatorTemplate { get; set; }

				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();
						HtmlGenericControl div = new HtmlGenericControl( "div" );
						div.Attributes.Add( "class", this.CssClass );

						this.lblResultCount = new Label();
						this.lblResultCount.CssClass = this.CssClass + "_resultMessage";

						//Select template
						ITemplate layoutTemplate = this.ProductsLayoutTemplate;
						if ( layoutTemplate == null ) layoutTemplate = new DefaultProductsLayoutTemplate( this );

						//Select template
						ITemplate groupTemplate = this.ProductsGroupTemplate;
						if ( groupTemplate == null ) groupTemplate = new DefaultProductsGroupTemplate();

						//Select template
						ITemplate groupSeparatorTemplate = this.ProductsGroupSeparatorTemplate;
						if ( groupSeparatorTemplate == null ) groupSeparatorTemplate = new DefaultProductsGroupSeparatorTemplate();

						//Select template
						ITemplate itemTemplate = this.ProductsItemTemplate;
						if ( itemTemplate == null ) itemTemplate = new DefaultProductsItemTemplate( this );

						//Item separator template
						ITemplate itemSeparatorTemplate = this.ProductsItemSeparatorTemplate;
						if ( itemSeparatorTemplate == null ) itemSeparatorTemplate = new DefaultProductsItemSeparatorTemplate();

						this.listView = new ListView();
						this.listView.DataKeyNames = new string[] { "Id" };
						this.listView.GroupItemCount = this.RepeatColumns;
						this.listView.LayoutTemplate = layoutTemplate;
						this.listView.GroupTemplate = groupTemplate;
						this.listView.GroupSeparatorTemplate = groupSeparatorTemplate;
						this.listView.ItemTemplate = itemTemplate;
						this.listView.ItemSeparatorTemplate = itemSeparatorTemplate;
						this.listView.PreRender += ( s, e ) =>
						{
								this.listView.DataBind();
						};
						div.Controls.Add( lblResultCount );
						div.Controls.Add( listView );

						this.Controls.Add( div );

						//Binding
						List<ProductsEntity> dataSource = this.GetDataListData( this.Filter );
						this.listView.DataSource = dataSource;
						this.lblResultCount.Visible = false;
						if ( dataSource.Count == 0 )
						{
								this.lblResultCount.Text = Resources.Controls.ProductsControl_NoProductsFound_Message;
								this.lblResultCount.Visible = true;
						}
				}

				public void OnFilter( SHP.Entities.Product.ReadByFilter filter )
				{
						//Binding
						EnsureChildControls();
						List<ProductsEntity> dataSource = this.GetDataListData( filter );
						listView.DataSource = dataSource;
						this.listView.DataBind();

						this.lblResultCount.Visible = false;
						if ( dataSource.Count == 0 )
						{
								this.lblResultCount.Text = Resources.Controls.ProductsControl_NoProductsFound_Message;
								this.lblResultCount.Visible = true;
						}
				}
				#endregion

				private List<ProductsEntity> GetDataListData( ProductsEntity.ReadByFilter filter )
				{
						List<ProductsEntity> list = new List<ProductsEntity>();

						if ( this.CategoryId != 0 )
						{
								ProductsEntity.ReadAllInCategory allInCategory = new ProductsEntity.ReadAllInCategory { CategoryId = this.CategoryId };
								if ( filter != null )
										allInCategory.ByFilter = filter;

								list = Storage<ProductsEntity>.Read( allInCategory );
						}else if ( filter != null )
								list = Storage<ProductsEntity>.Read( filter );

						return list;
				}

				#region Templates
				private class DefaultProductsLayoutTemplate: ITemplate
				{
						private ProductsControl owner = null;
						public DefaultProductsLayoutTemplate( ProductsControl owner )
						{
								this.owner = owner;
						}
						#region ITemplate Members
						public void InstantiateIn( Control container )
						{
								HtmlGenericControl table = new HtmlGenericControl( "table" );
								table.Attributes.Add( "class", owner.CssClass + "_list" );

								//Item placeHolder
								PlaceHolder ph = new PlaceHolder();
								ph.ID = "groupPlaceholder";
								table.Controls.Add( ph );
								container.Controls.Add( table );

								if ( this.owner.AllowPaging )
								{
										HtmlGenericControl div = new HtmlGenericControl( "div" );
										div.Attributes.Add( "class", owner.CssClass + "_dataPager" );

										//Pager
										DataPager dataPager = new DataPager();
										dataPager.ID = "dataPager";
										dataPager.PageSize = this.owner.PageSize;
										dataPager.Fields.Add( new NumericPagerField() { ButtonCount = 10 } );
										dataPager.PagedControlID = container.ID;
										div.Controls.Add( dataPager );

										container.Controls.Add( div );
								}
						}
						#endregion
				}
				private class DefaultProductsGroupTemplate: ITemplate
				{
						#region ITemplate Members

						public DefaultProductsGroupTemplate()
						{
						}

						public void InstantiateIn( Control container )
						{
								HtmlGenericControl tr = new HtmlGenericControl( "tr" );

								//Item placeHolder
								PlaceHolder ph = new PlaceHolder();
								ph.ID = "itemPlaceHolder";
								tr.Controls.Add( ph );
								container.Controls.Add( tr );
						}

						#endregion
				}
				private class DefaultProductsGroupSeparatorTemplate: ITemplate
				{
						#region ITemplate Members

						public DefaultProductsGroupSeparatorTemplate()
						{
						}

						public void InstantiateIn( Control container )
						{
						}

						#endregion
				}
				private class DefaultProductsItemTemplate: ITemplate
				{
						#region Public properties
						private ProductsControl owner = null;
						public string CssClass { get; set; }
						private string DisplayAliasUrlFormat { get; set; }
						private string DisplayUrlFormat { get; set; }

						#endregion

						#region ITemplate Members

						public DefaultProductsItemTemplate( ProductsControl owner )
						{
								this.owner = owner;
								this.CssClass = owner.CssClass + "_item";
								this.DisplayAliasUrlFormat = "{0}?" + owner.BuildReturnUrlQueryParam();
								this.DisplayUrlFormat = owner.DisplayUrlFormat + "&" + owner.BuildReturnUrlQueryParam();
						}

						public void InstantiateIn( Control container )
						{
								HtmlGenericControl td = new HtmlGenericControl( "td" );
								//td.Attributes.Add( "class", this.CssClass );

								RoundPanel rp = new RoundPanel();
								rp.CssClass = this.CssClass;
								rp.DataBinding += OnRoundPanelDataBinding;
								rp.Controls.Add( CreateDetailControl() );
								td.Controls.Add( rp );

								container.Controls.Add( td );
						}

						private Control CreateDetailControl()
						{
								HtmlGenericControl div = new HtmlGenericControl( "div" );
								div.Style.Add( "width", "100%" );
								Table table = new Table();
								table.Style.Add( "width", "100%" );
								table.CellPadding = 0;
								table.CellSpacing = 0;
								//table.Attributes.Add( "border", "1" );
								TableRow row = new TableRow();

								//Image
								TableCell cell = new TableCell();
								cell.RowSpan = 2;
								cell.HorizontalAlign = HorizontalAlign.Left;
								cell.Controls.Add( CreateImageControl() );
								row.Cells.Add( cell );

								//Description
								Label lblDescription = new Label();
								lblDescription.DataBinding += ( s, e ) =>
								{
										ProductsEntity product = ( ( (ListViewDataItem)lblDescription.NamingContainer ).DataItem as ProductsEntity );
										lblDescription.Text = product.Description;
								};
								cell = new TableCell();
								cell.ColumnSpan = 3;
								cell.HorizontalAlign = HorizontalAlign.Left;
								cell.Controls.Add( lblDescription );
								row.Cells.Add( cell );
								table.Rows.Add( row );

								//Avalilability, Price, Cart
								row = new TableRow();
								cell = new TableCell();
								cell.Wrap = false;
								cell.HorizontalAlign = HorizontalAlign.Left;
								cell.VerticalAlign = VerticalAlign.Bottom;
								cell.Controls.Add( new LiteralControl( Resources.Controls.AdminProductControl_Availability ) );
								cell.Controls.Add( CreateAvailabilityControl() );
								row.Cells.Add( cell );

								cell = new TableCell();
								cell.Wrap = false;
								cell.HorizontalAlign = HorizontalAlign.Right;
								cell.VerticalAlign = VerticalAlign.Bottom;
								cell.Controls.Add( CreateCartControl() );
								row.Cells.Add( cell );

								cell = new TableCell();
								cell.Wrap = false;
								cell.CssClass = this.CssClass + "_priceContainer";
								cell.HorizontalAlign = HorizontalAlign.Right;
								cell.VerticalAlign = VerticalAlign.Bottom;
								cell.Controls.Add( CreatePriceControl() );
								row.Cells.Add( cell );
								table.Rows.Add( row );

								div.Controls.Add( table );
								return div;
						}

						private Control CreateImageControl()
						{
								HyperLink hlImage = new HyperLink();
								Image img = new Image();
								hlImage.Controls.Add( img );

								img.CssClass = this.CssClass + "_image";
								img.DataBinding += ( s, e ) =>
								{
										ProductsEntity product = ( ( (ListViewDataItem)img.NamingContainer ).DataItem as ProductsEntity );

										string navigateUrl = string.Empty;
										if ( string.IsNullOrEmpty( product.Alias ) && !string.IsNullOrEmpty( this.DisplayUrlFormat ) )
												navigateUrl = img.Page.ResolveUrl( string.Format( this.DisplayUrlFormat, product.Id ) );
										else navigateUrl = img.Page.ResolveUrl( string.Format( this.DisplayAliasUrlFormat, product.Alias ) );
										if ( !string.IsNullOrEmpty( navigateUrl ) ) hlImage.NavigateUrl = navigateUrl;

										SetThubnailUrl( img, product.Id, this.owner.ConfigValue( "SHP:ImageGallery:Product:StoragePath" ) );
								};

								return hlImage;
						}

						private void SetThubnailUrl( Image img, int productId, string storageUrl )
						{
								string storagePath = string.Format( "{0}{1}/", storageUrl, productId );
								string productImagesPath = img.Page.Server.MapPath( storagePath );

								if ( !Directory.Exists( productImagesPath ) )
								{
										img.Visible = false;
										return;
								}

								DirectoryInfo di = new DirectoryInfo( productImagesPath );
								FileInfo[] fileInfos = di.GetFiles( "*.*" );

								if ( fileInfos.Length == 0 )
								{
										img.Visible = false;
										return;
								}

								string urlThumbnail = storagePath + "_t/" + fileInfos[0].Name;
								img.ImageUrl = this.owner.Page.ResolveUrl( urlThumbnail );
						}

						private Control CreateAvailabilityControl()
						{
								Label lblAvailability = new Label();
								lblAvailability.CssClass = this.CssClass + "_labelAvailability";
								lblAvailability.DataBinding += ( s, e ) =>
								{
										ProductsEntity product = ( ( (ListViewDataItem)lblAvailability.NamingContainer ).DataItem as ProductsEntity );
										lblAvailability.Text = product.Availability;
								};

								return lblAvailability;
						}

						private Control CreatePriceControl()
						{
								HtmlGenericControl span = new HtmlGenericControl( "span" );
								Label lblOldPrice = new Label();
								lblOldPrice.CssClass = this.CssClass + "_labelOlPrice";
								lblOldPrice.DataBinding += ( s, e ) =>
								{
										ProductsEntity product = ( ( (ListViewDataItem)lblOldPrice.NamingContainer ).DataItem as ProductsEntity );
										lblOldPrice.Text = Utilities.CultureUtilities.CurrencyInfo.ToString( product.Price, lblOldPrice.Page.Session );
										if ( product.Discount == 0 )
												lblOldPrice.Visible = false;
								};

								Label lblPrice = new Label();
								lblPrice.CssClass = this.CssClass + "_labelPrice";
								lblPrice.DataBinding += ( s, e ) =>
								{
										ProductsEntity product = ( ( (ListViewDataItem)lblPrice.NamingContainer ).DataItem as ProductsEntity );
										if ( product.VAT == 0m )
												lblPrice.Text = Utilities.CultureUtilities.CurrencyInfo.ToString( product.PriceTotal, lblPrice.Page.Session );
										else 
												lblPrice.Text = string.Format( "{0}/{1} {2}",
												Utilities.CultureUtilities.CurrencyInfo.ToString( product.PriceTotal, lblPrice.Page.Session ),
												Resources.Controls.WithVAT,
												Utilities.CultureUtilities.CurrencyInfo.ToString( product.PriceTotalWVAT, lblPrice.Page.Session ) );
								};

								span.Controls.Add( lblOldPrice );
								span.Controls.Add( lblPrice );

								return span;
						}

						private Control CreateCartControl()
						{
								HtmlGenericControl span = new HtmlGenericControl( "span" );

								//Label tQuantity
								Label lblQuantity = new Label();
								lblQuantity.Text = Resources.Controls.AdminProductControl_Quantity;
								lblQuantity.CssClass = this.CssClass + "_labelQuantity";
								span.Controls.Add( lblQuantity );

								// TextBox Cart
								TextBox txtQuantity = new TextBox();
								txtQuantity.CssClass = this.CssClass + "_inputQuantity";
								txtQuantity.Text = "1";
								span.Controls.Add( txtQuantity );

								//Button Add cart
								Button btnAddCart = new Button();
								btnAddCart.CssClass = this.CssClass + "_buttonAddCart";
								btnAddCart.DataBinding += ( s, e ) =>
								{
										ProductsEntity product = ( ( (ListViewDataItem)btnAddCart.NamingContainer ).DataItem as ProductsEntity );
										btnAddCart.CommandArgument = product.Id.ToString();
										btnAddCart.CommandName = product.Name;
										btnAddCart.ToolTip = Resources.Controls.AdminProductControl_AddProductToCart_ToolTip;
								};
								btnAddCart.Click += ( s, e ) =>
								{
										if ( string.IsNullOrEmpty( btnAddCart.CommandArgument ) )
												return;
										int quantity = 1;
										int productId = 0;

										Int32.TryParse( txtQuantity.Text, out quantity );
										Int32.TryParse( btnAddCart.CommandArgument, out productId );
										CartHelper.AddProductToCart( btnAddCart.Page, productId, quantity );

										//Alert s informaciou o pridani do nakupneho kosika
										Page page = btnAddCart.Page;
										string js = string.Format( "alert('{0}');window.location.href=window.location.href+'{1}nocache='+(new Date()).getSeconds();",
											 string.Format( Resources.Controls.AdminProductControl_ProductWasAddedToCart_Message, btnAddCart.CommandName, quantity ),
												this.owner.Request.RawUrl.Contains( "?" ) ? "&" : "?" );
										page.ClientScript.RegisterStartupScript( page.GetType(), "addProductToCart", js, true );

								};
								span.Controls.Add( btnAddCart );

								return span;
						}

						void OnRoundPanelDataBinding( object sender, EventArgs e )
						{
								RoundPanel control = sender as RoundPanel;
								ListViewDataItem item = (ListViewDataItem)control.NamingContainer;
								ProductsEntity product = ( item.DataItem as ProductsEntity );

								string navigateUrl = string.Empty;
								if ( string.IsNullOrEmpty( product.Alias ) && !string.IsNullOrEmpty( this.DisplayUrlFormat ) )
										navigateUrl = control.Page.ResolveUrl( string.Format( this.DisplayUrlFormat, product.Id ) );
								else navigateUrl = control.Page.ResolveUrl( string.Format( this.DisplayAliasUrlFormat, product.Alias ) );

								if ( !string.IsNullOrEmpty( navigateUrl ) )
										control.Text = string.Format( "<a href='{0}'>{1}</a>", navigateUrl, product.Name );
								else
										control.Text = product.Name;
						}
						#endregion
				}
				private class DefaultProductsItemSeparatorTemplate: ITemplate
				{
						#region ITemplate Members

						public DefaultProductsItemSeparatorTemplate()
						{
						}

						public void InstantiateIn( Control container )
						{
						}

						#endregion
				}
				#endregion
		}
}
