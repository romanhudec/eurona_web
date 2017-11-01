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

[assembly: WebResource( "CMS.Controls.ImageGallery.lightbox.effects.js", "application/x-javascript" )]
[assembly: WebResource( "CMS.Controls.ImageGallery.lightbox.lightbox.js", "application/x-javascript" )]
[assembly: WebResource( "CMS.Controls.ImageGallery.lightbox.prototype.js", "application/x-javascript" )]
[assembly: WebResource( "CMS.Controls.ImageGallery.lightbox.scriptaculous.js?load=effects", "application/x-javascript" )]
[assembly: WebResource( "CMS.Controls.ImageGallery.lightbox.lightbox.css", "text/css" )]
namespace CMS.Controls.ImageGallery
{
		/// <summary>
		/// Control na zobrazenie obsahu galerie, teda zobrazi samotne fotografie danej galerie.
		/// </summary>
		public class ImageGalleryItemsControl: CmsControl
		{
				public ImageGalleryItemsControl()
				{
						this.RepeatColumns = 3;
						this.AllowPaging = true;
						this.PageSize = 20;
						this.ImageAttributes = new List<ImageGalleryControl.ImageAttribute>();
				}

				public int ImageGalleryId
				{
						get
						{
								object o = ViewState["ImageGalleryId"];
								return o != null ? (int)Convert.ToInt32( o ) : 0;
						}
						set { ViewState["ImageGalleryId"] = value; }
				}

				private ImageGalleryEntity gallery = null;
				public ImageGalleryEntity Gallery
				{
						get
						{
								if ( gallery != null ) return gallery;
								gallery = Storage<ImageGalleryEntity>.ReadFirst( new ImageGalleryEntity.ReadById { ImageGalleryId = this.ImageGalleryId } );
								return gallery;
						}
				}

				public List<ImageGalleryControl.ImageAttribute> ImageAttributes { get; set; }

				public string CssRating { get; set; }
				public string CommentsFormatUrl { get; set; }

				public int RepeatColumns { get; set; }
				public bool AllowPaging { get; set; }
				public int PageSize { get; set; }

				[Browsable( false )]
				[DefaultValue( null )]
				[Description( "Image gallery item Layout template." )]
				[TemplateContainer( typeof( ListViewDataItem ) )]
				[PersistenceMode( PersistenceMode.InnerProperty )]
				public virtual ITemplate ImageGalleryItemLayoutTemplate { get; set; }

				[Browsable( false )]
				[DefaultValue( null )]
				[Description( "Image gallery Group template." )]
				[TemplateContainer( typeof( ListViewDataItem ) )]
				[PersistenceMode( PersistenceMode.InnerProperty )]
				public virtual ITemplate ImageGalleryItemGroupTemplate { get; set; }

				[Browsable( false )]
				[DefaultValue( null )]
				[Description( "Image gallery Item template." )]
				[TemplateContainer( typeof( ListViewDataItem ) )]
				[PersistenceMode( PersistenceMode.InnerProperty )]
				public virtual ITemplate ImageGalleryItemItemTemplate { get; set; }

				#region Protected overrides

				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						//ImageGalleryEntity gallery = Storage<ImageGalleryEntity>.ReadFirst( new ImageGalleryEntity.ReadById { ImageGalleryId = this.ImageGalleryId } );
						ImageGalleryEntity gallery = this.Gallery;
						if ( gallery == null )
								return;

						//Update ViewCount
						ImageGalleryEntity.IncrementViewCountCommand cmd = new ImageGalleryEntity.IncrementViewCountCommand();
						cmd.ImageGalleryId = gallery.Id;
						Storage<ImageGalleryEntity>.Execute<ImageGalleryEntity.IncrementViewCountCommand>( cmd );

						//Create template control
						HtmlGenericControl div = new HtmlGenericControl( "div" );
						div.Attributes.Add( "class", this.CssClass );

						//Select template
						ITemplate layoutTemplate = this.ImageGalleryItemLayoutTemplate;
						if ( layoutTemplate == null ) layoutTemplate = new DefaultImageGalleryItemLayoutTemplate( this );

						//Select template
						ITemplate groupTemplate = this.ImageGalleryItemGroupTemplate;
						if ( groupTemplate == null ) groupTemplate = new DefaultImageGalleryItemGroupTemplate();

						//Select template
						ITemplate itemTemplate = this.ImageGalleryItemItemTemplate;
						if ( itemTemplate == null ) itemTemplate = new DefaultImageGalleryItemItemTemplate( this, gallery.EnableComments, gallery.EnableVotes );

						HtmlGenericControl divName = new HtmlGenericControl( "div" );
						divName.Attributes.Add( "class", this.CssClass + "_galleryName" );
						HyperLink hlGalleryName = new HyperLink();
						hlGalleryName.Text = gallery.Name;
						divName.Controls.Add( hlGalleryName );
						div.Controls.Add( divName );

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
						div.Controls.Add( listView ); ;

						this.Controls.Add( div );

						//Binding
						listView.DataSource = this.GetDataListData();

				}

				protected override void Render( HtmlTextWriter writer )
				{

						string js = string.Format( "<script language='javascript' type='text/javascript'>var fileLoadingImage = '{0}';var fileBottomNavCloseImage='{1}';</script>",
								Page.ResolveUrl( "~/images/lightbox/loading.gif" ), Page.ResolveUrl( "~/images/lightbox/closelabel.gif" ) );
						ClientScriptManager cs = this.Page.ClientScript;
						Type cstype = this.Page.GetType();

						if ( !cs.IsStartupScriptRegistered( cstype, "lightBoxImagesMemebrs" ) )
								cs.RegisterStartupScript( cstype, "lightBoxImagesMemebrs", js );

						base.Render( writer );
				}
				#endregion

				private List<ImageGalleryItemEntity> GetDataListData()
				{
						List<ImageGalleryItemEntity> list = Storage<ImageGalleryItemEntity>.Read( new ImageGalleryItemEntity.ReadByImageGalleryId { ImageGalleryId = this.ImageGalleryId } );
						return list;
				}

				#region Templates
				private class DefaultImageGalleryItemLayoutTemplate: ITemplate
				{
						private ImageGalleryItemsControl owner = null;
						public DefaultImageGalleryItemLayoutTemplate( ImageGalleryItemsControl owner )
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
				private class DefaultImageGalleryItemGroupTemplate: ITemplate
				{
						#region ITemplate Members
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
				private class DefaultImageGalleryItemItemTemplate: ITemplate
				{
						#region Public properties
						private string CssClass { get; set; }
						private string CssRating { get; set; }
						private string CommentsFormatUrl { get; set; }
						private string ReturnUrl { get; set; }

						private List<ImageGalleryControl.ImageAttribute> ImageAttributes { get; set; }

						private bool EnableComments { get; set; }
						private bool EnableVotes { get; set; }
						#endregion

						#region ITemplate Members

						public DefaultImageGalleryItemItemTemplate( ImageGalleryItemsControl owner, bool enableComments, bool enableVotes )
						{
								this.ReturnUrl = owner.BuildReturnUrlQueryParam();
								this.CssClass = owner.CssClass + "_item";
								this.CssRating = owner.CssRating;
								this.CommentsFormatUrl = owner.CommentsFormatUrl;

								this.EnableComments = enableComments;
								this.EnableVotes = enableVotes;

								this.ImageAttributes = owner.ImageAttributes;

								//Include LightBox javascripts
								ClientScriptManager cs = owner.Page.ClientScript;
								Type cstype = this.GetType();

								string urlInclude = cs.GetWebResourceUrl( cstype, "CMS.Controls.ImageGallery.lightbox.prototype.js" );
								cs.RegisterClientScriptInclude( cstype, "lightbox.prototype.js", urlInclude );

								urlInclude = cs.GetWebResourceUrl( cstype, "CMS.Controls.ImageGallery.lightbox.effects.js" );
								cs.RegisterClientScriptInclude( cstype, "lightbox.effects.js", urlInclude );

								urlInclude = cs.GetWebResourceUrl( cstype, "CMS.Controls.ImageGallery.lightbox.lightbox.js" );
								cs.RegisterClientScriptInclude( cstype, "lightbox.lightbox.js", urlInclude );

								urlInclude = cs.GetWebResourceUrl( cstype, "CMS.Controls.ImageGallery.lightbox.scriptaculous.js?load=effects" );
								cs.RegisterClientScriptInclude( cstype, "lightbox.scriptaculous.js", urlInclude );

								string urlCss = cs.GetWebResourceUrl( cstype, "CMS.Controls.ImageGallery.lightbox.lightbox.css" );
								HtmlLink cssLink = new HtmlLink();
								cssLink.Href = urlCss;
								cssLink.Attributes.Add( "rel", "stylesheet" );
								cssLink.Attributes.Add( "type", "text/css" );
								owner.Controls.Add( cssLink );
						}

						public void InstantiateIn( Control container )
						{
								HtmlGenericControl td = new HtmlGenericControl( "td" );
								td.Attributes.Add( "class", this.CssClass );

								Table table = new Table();
								table.Width = Unit.Percentage( 100 );
								//table.CellPadding = 0;
								//table.CellSpacing = 0;
								TableRow row = null;

								//Image
								row = new TableRow();
								TableCell cell = new TableCell();
								cell.HorizontalAlign = HorizontalAlign.Center;
								cell.CssClass = this.CssClass + "_imageContainer";
								cell.Controls.Add( CreateImageControl() );
								row.Cells.Add( cell );
								table.Rows.Add( row );

								//Description
								row = new TableRow();
								cell = new TableCell();
								cell.HorizontalAlign = HorizontalAlign.Center;
								cell.Controls.Add( CreateDescriptionControl() );
								row.Cells.Add( cell );
								table.Rows.Add( row );

								#region Vote controls
								if ( this.EnableVotes )
								{
										row = new TableRow();
										cell = new TableCell();
										cell.Controls.Add( new LiteralControl( "<hr />" ) );
										cell.CssClass = this.CssClass + "_rating";
										Vote.RaitingControl ratingControl = new Vote.RaitingControl();
										ratingControl.CssClass = this.CssRating;
										ratingControl.ObjectTypeId = (int)ImageGalleryItemEntity.AccountVoteType;
										ratingControl.DataBinding += ( s, e ) =>
										{
												ImageGalleryItemEntity item = ( ( (ListViewDataItem)ratingControl.NamingContainer ).DataItem as ImageGalleryItemEntity );
												ratingControl.ObjectId = item.Id;
												ratingControl.RatingResult = item.RatingResult;
										};

										ratingControl.OnVote += ( objectId, rating ) =>
										{
												ImageGalleryItemEntity.IncrementVoteCommand cmdVote = new ImageGalleryItemEntity.IncrementVoteCommand();
												cmdVote.AccountId = Security.Account.Id;
												cmdVote.ImageGalleryItemId = objectId;
												cmdVote.Rating = rating;
												Storage<ImageGalleryItemEntity>.Execute( cmdVote );

												ImageGalleryItemEntity comment = Storage<ImageGalleryItemEntity>.ReadFirst( new ImageGalleryItemEntity.ReadById { ImageGalleryItemId = objectId } );
												return comment.RatingResult;
										};
										cell.Controls.Add( ratingControl );
										row.Cells.Add( cell );
										table.Rows.Add( row );
								}
								#endregion

								//Comments
								if ( this.EnableComments )
								{
										row = new TableRow();
										cell = new TableCell();
										cell.Controls.Add( new LiteralControl( "<hr />" ) );
										cell.HorizontalAlign = HorizontalAlign.Left;
										cell.Controls.Add( CreateCommentsControl() );
										row.Cells.Add( cell );
										table.Rows.Add( row );
								}

								td.Controls.Add( table );
								container.Controls.Add( td );
						}

						private Control CreateImageControl()
						{
								HtmlGenericControl imageControl = new HtmlGenericControl( "a" );

								if ( this.ImageAttributes.Count != 0 )
								{
										foreach ( ImageGalleryControl.ImageAttribute attr in this.ImageAttributes )
												imageControl.Attributes.Add( attr.Key, attr.Value );
								}

								Image img = new Image();
								img.CssClass = this.CssClass + "_image";
								imageControl.Controls.Add( img );
								img.DataBinding += ( s, e ) =>
								{
										ImageGalleryItemEntity item = ( ( (ListViewDataItem)img.NamingContainer ).DataItem as ImageGalleryItemEntity );
										img.ImageUrl = img.Page.ResolveUrl( item.VirtualThumbnailPath );

										string imageUrl = item.VirtualPath.StartsWith( "~" ) ? img.Page.ResolveUrl( item.VirtualPath ) : item.VirtualPath;
										string imageUrlThumbnail = item.VirtualThumbnailPath.StartsWith( "~" ) ? img.Page.ResolveUrl( item.VirtualThumbnailPath ) : item.VirtualThumbnailPath;
										img.ImageUrl = imageUrlThumbnail;
										imageControl.Attributes.Add( "href", imageUrl );
										imageControl.Attributes.Add( "title", string.Empty );

										imageControl.Attributes.Add( "rel", string.Format( "lightbox[galery{0}]", item.ImageGalleryId ) );
								};

								return imageControl;
						}

						private Control CreateDescriptionControl()
						{
								Label lblDescription = new Label();
								lblDescription.CssClass = this.CssClass + "_description";
								lblDescription.DataBinding += ( s, e ) =>
								{
										ImageGalleryItemEntity item = ( ( (ListViewDataItem)lblDescription.NamingContainer ).DataItem as ImageGalleryItemEntity );
										lblDescription.Text = item.Description;
								};
								return lblDescription;
						}

						private Control CreateCommentsControl()
						{
								HtmlGenericControl span = new HtmlGenericControl( "span" );
								span.Attributes.Add( "class", this.CssClass + "_comments" );
								//span.Controls.Add(new LiteralControl(string.Format("{0}: ", Resources.Controls.ImageGalleriesControl_CommentsCount)));

								HyperLink hlComments = new HyperLink();
								span.Controls.Add( hlComments );
								hlComments.DataBinding += ( s, e ) =>
								{
										ImageGalleryItemEntity item = ( ( (ListViewDataItem)hlComments.NamingContainer ).DataItem as ImageGalleryItemEntity );
										hlComments.Text = String.Format( "{0}: {1}", Resources.Controls.ImageGalleriesControl_CommentsCount, item.CommentsCount );

										if ( !string.IsNullOrEmpty( this.CommentsFormatUrl ) )
										{
												string commentAlias = string.Format( this.CommentsFormatUrl, item.Id );
												AliasUtilities aliasUtils = new AliasUtilities();
												commentAlias = aliasUtils.Resolve( commentAlias, hlComments.Page );

												commentAlias += ( commentAlias.Contains( "?" ) ? "&" : "?" ) + this.ReturnUrl;
												hlComments.NavigateUrl = hlComments.Page.ResolveUrl( commentAlias );
										}

								};
								return span;
						}


						#endregion
				}
				#endregion
		}
}
