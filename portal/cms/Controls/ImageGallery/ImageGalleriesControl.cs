using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using ImageGalleryEntity = CMS.Entities.ImageGallery;
using ImageGalleryItemEntity = CMS.Entities.ImageGalleryItem;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.HtmlControls;
using CMS.Utilities;

namespace CMS.Controls.ImageGallery
{
		/// <summary>
		/// Control zobrazi zoznam galerii obrazkov.
		/// </summary>
		public class ImageGalleriesControl: ContentEditorCmsControl
		{
				public ImageGalleriesControl()
				{
						this.RepeatColumns = 2;
						this.AllowPaging = true;
						this.PageSize = 20;
				}

				public int? TagId
				{
						get
						{
								object o = ViewState["TagId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["TagId"] = value; }
				}

				public string DisplayUrlFormat { get; set; }
				public string CommentsFormatUrl { get; set; }

				public int RepeatColumns { get; set; }
				public bool AllowPaging { get; set; }
				public int PageSize { get; set; }

				[Browsable( false )]
				[DefaultValue( null )]
				[Description( "Image gallery Layout template." )]
				[TemplateContainer( typeof( ListViewDataItem ) )]
				[PersistenceMode( PersistenceMode.InnerProperty )]
				public virtual ITemplate ImageGalleryLayoutTemplate { get; set; }

				[Browsable( false )]
				[DefaultValue( null )]
				[Description( "Image gallery Group template." )]
				[TemplateContainer( typeof( ListViewDataItem ) )]
				[PersistenceMode( PersistenceMode.InnerProperty )]
				public virtual ITemplate ImageGalleryGroupTemplate { get; set; }

				[Browsable( false )]
				[DefaultValue( null )]
				[Description( "Image gallery Item template." )]
				[TemplateContainer( typeof( ListViewDataItem ) )]
				[PersistenceMode( PersistenceMode.InnerProperty )]
				public virtual ITemplate ImageGalleryItemTemplate { get; set; }

				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();
						HtmlGenericControl div = new HtmlGenericControl( "div" );
						div.Attributes.Add( "class", this.CssClass );

						//Select template
						ITemplate layoutTemplate = this.ImageGalleryLayoutTemplate;
						if ( layoutTemplate == null ) layoutTemplate = new DefaultImageGalleryLayoutTemplate( this );

						//Select template
						ITemplate groupTemplate = this.ImageGalleryGroupTemplate;
						if ( groupTemplate == null ) groupTemplate = new DefaultImageGalleryGroupTemplate();

						//Select template
						ITemplate itemTemplate = this.ImageGalleryItemTemplate;
						if ( itemTemplate == null ) itemTemplate = new DefaultImageGalleryItemTemplate( this );

						ListView listView = new ListView();
						listView.DataKeyNames = new string[] { "Id" };
						listView.GroupItemCount = this.RepeatColumns;
						listView.LayoutTemplate = layoutTemplate;
						listView.GroupTemplate = groupTemplate;
						listView.ItemTemplate = itemTemplate;
						listView.PreRender += ( s, e ) =>
						{
								listView.DataBind();
						};
						div.Controls.Add( listView );

						this.Controls.Add( div );

						//Binding
						listView.DataSource = this.GetDataListData();
				}
				#endregion

				private List<ImageGalleryEntity> GetDataListData()
				{
						List<ImageGalleryEntity> list = Storage<ImageGalleryEntity>.Read( new ImageGalleryEntity.ReadForCurrentAccount { TagId = this.TagId } );
						return list;
				}

				#region Templates
				private class DefaultImageGalleryLayoutTemplate: ITemplate
				{
						private ImageGalleriesControl owner = null;
						public DefaultImageGalleryLayoutTemplate( ImageGalleriesControl owner )
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
				private class DefaultImageGalleryGroupTemplate: ITemplate
				{
						#region ITemplate Members

						public DefaultImageGalleryGroupTemplate()
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
				private class DefaultImageGalleryItemTemplate: ITemplate
				{
						#region Public properties
						public string CssClass { get; set; }
						private string DisplayAliasUrlFormat { get; set; }
						private string DisplayUrlFormat { get; set; }
						private string CommentsFormatUrl { get; set; }
						#endregion

						#region ITemplate Members

						public DefaultImageGalleryItemTemplate( ImageGalleriesControl owner )
						{
								this.CssClass = owner.CssClass + "_item";
								this.DisplayAliasUrlFormat = "{0}?" + owner.BuildReturnUrlQueryParam();
								this.DisplayUrlFormat = owner.DisplayUrlFormat + "&" + owner.BuildReturnUrlQueryParam();
								this.CommentsFormatUrl = owner.CommentsFormatUrl;
						}

						public void InstantiateIn( Control container )
						{
								HtmlGenericControl td = new HtmlGenericControl( "td" );
								td.Attributes.Add( "class", this.CssClass );

								HtmlGenericControl div = new HtmlGenericControl( "div" );
								div.Attributes.Add( "class", this.CssClass + "_galleryName" );
								HyperLink hlGalleryName = new HyperLink();
								hlGalleryName.DataBinding += OnGalleryNameDataBinding;
								div.Controls.Add( hlGalleryName );
								td.Controls.Add( div );

								Table table = new Table();
								table.CssClass = this.CssClass + "_galleryTable";
								table.CellPadding = 0;
								table.CellSpacing = 0;
								//table.Attributes.Add( "border", "1" );
								TableRow row = new TableRow();

								//Image
								TableCell cell = new TableCell();
								cell.CssClass = this.CssClass + "_galleryTable_imageCell";
								cell.HorizontalAlign = HorizontalAlign.Left;
								cell.Controls.Add( CreateImageControl() );
								row.Cells.Add( cell );

								//Info
								cell = new TableCell();
								cell.Wrap = false;
								cell.CssClass = this.CssClass + "_galleryTable_infoCell";
								cell.VerticalAlign = VerticalAlign.Top;
								cell.HorizontalAlign = HorizontalAlign.Right;
								cell.Controls.Add( CreateInfoControl() );
								row.Cells.Add( cell );
								table.Rows.Add( row );

								//Description
								row = new TableRow();
								cell = new TableCell();
								cell.CssClass = this.CssClass + "_galleryTable_descriptionCell";
								cell.HorizontalAlign = HorizontalAlign.Justify;
								cell.Controls.Add( CreateDescriptionControl() );
								row.Cells.Add( cell );
								table.Rows.Add( row );

								td.Controls.Add( table );
								container.Controls.Add( td );
						}

						private Control CreateImageControl()
						{
								HyperLink hlImage = new HyperLink();
								Image img = new Image();
								hlImage.Controls.Add( img );

								img.CssClass = this.CssClass + "_image";
								img.DataBinding += ( s, e ) =>
								{
										ImageGalleryEntity imageGallery = ( ( (ListViewDataItem)img.NamingContainer ).DataItem as ImageGalleryEntity );
										ImageGalleryItemEntity item = Storage<ImageGalleryItemEntity>.ReadFirst( new ImageGalleryItemEntity.ReadTopByImageGalleryId { ImageGalleryId = imageGallery.Id, Top = 1 } );

										string navigateUrl = string.Empty;
										if ( string.IsNullOrEmpty( imageGallery.Alias ) && !string.IsNullOrEmpty( this.DisplayUrlFormat ) )
												navigateUrl = img.Page.ResolveUrl( string.Format( this.DisplayUrlFormat, imageGallery.Id ) );
										else navigateUrl = img.Page.ResolveUrl( string.Format( this.DisplayAliasUrlFormat, imageGallery.Alias ) );
										if ( !string.IsNullOrEmpty( navigateUrl ) ) hlImage.NavigateUrl = navigateUrl;

										if ( item != null ) img.ImageUrl = item.VirtualThumbnailPath.StartsWith( "~" ) ? img.Page.ResolveUrl( item.VirtualThumbnailPath ) : item.VirtualThumbnailPath;
										else img.Visible = false;
								};

								return hlImage;
						}

						private Control CreateInfoControl()
						{
								HtmlGenericControl div = new HtmlGenericControl( "div" );
								div.Attributes.Add( "class", this.CssClass + "_info" );
								div.DataBinding += ( s, e ) =>
								{
										ImageGalleryEntity imageGallery = ( ( (ListViewDataItem)div.NamingContainer ).DataItem as ImageGalleryEntity );

										Table table = new Table();
										table.CellPadding = 3;
										table.Width = Unit.Percentage( 100 );
										TableRow row = null;

										TableCell cell = new TableCell();
										cell.HorizontalAlign = HorizontalAlign.Left;
										cell.Controls.Add( new LiteralControl( string.Format( "{0}: {1:d}", Resources.Controls.ImageGalleriesControl_Date, imageGallery.Date ) ) );
										row = new TableRow();
										row.Cells.Add( cell );
										table.Rows.Add( row );

										cell = new TableCell();
										cell.HorizontalAlign = HorizontalAlign.Left;
										cell.Controls.Add( new LiteralControl( string.Format( "{0}: {1}", Resources.Controls.ImageGalleriesControl_ViewCount, imageGallery.ViewCount ) ) );
										row = new TableRow();
										row.Cells.Add( cell );
										table.Rows.Add( row );

										cell = new TableCell();
										cell.HorizontalAlign = HorizontalAlign.Left;
										cell.Controls.Add( new LiteralControl( string.Format( "{0}: {1}", Resources.Controls.ImageGalleriesControl_PhotosCount, imageGallery.ItemsCount ) ) );
										row = new TableRow();
										row.Cells.Add( cell );
										table.Rows.Add( row );

										if ( imageGallery.EnableComments )
										{

												string commentAlias = string.Format( this.CommentsFormatUrl, imageGallery.Id );
												if ( !string.IsNullOrEmpty( this.CommentsFormatUrl ) )
												{
														AliasUtilities aliasUtils = new AliasUtilities();
														commentAlias = aliasUtils.Resolve( commentAlias, div.Page );
												}

												HyperLink hlComments = new HyperLink();
												hlComments.Text = String.Format( "{0}: {1}", Resources.Controls.ImageGalleriesControl_CommentsCount, imageGallery.CommentsCount );

												if ( !String.IsNullOrEmpty( this.CommentsFormatUrl ) )
														hlComments.NavigateUrl = commentAlias;

												cell = new TableCell();
												cell.HorizontalAlign = HorizontalAlign.Left;
												cell.Controls.Add( hlComments );
												row = new TableRow();
												row.Cells.Add( cell );
												table.Rows.Add( row );
										}

										div.Controls.Add( table );
								};
								return div;
						}

						private Control CreateDescriptionControl()
						{
								HtmlGenericControl div = new HtmlGenericControl( "div" );
								div.Attributes.Add( "class", this.CssClass + "_desciption" );
								div.DataBinding += ( s, e ) =>
								{
										ImageGalleryEntity imageGallery = ( ( (ListViewDataItem)div.NamingContainer ).DataItem as ImageGalleryEntity );
										div.Controls.Add( new LiteralControl( imageGallery.Description ) );
								};
								return div;
						}

						void OnGalleryNameDataBinding( object sender, EventArgs e )
						{
								HyperLink control = sender as HyperLink;
								ListViewDataItem item = (ListViewDataItem)control.NamingContainer;
								ImageGalleryEntity imageGallery = ( item.DataItem as ImageGalleryEntity );

								control.Text = imageGallery.Name;

								if ( string.IsNullOrEmpty( imageGallery.Alias ) && !string.IsNullOrEmpty( this.DisplayUrlFormat ) )
										control.NavigateUrl = control.Page.ResolveUrl( string.Format( this.DisplayUrlFormat, imageGallery.Id ) );
								else control.NavigateUrl = control.Page.ResolveUrl( string.Format( this.DisplayAliasUrlFormat, imageGallery.Alias ) );

						}
						#endregion
				}
				#endregion
		}
}
