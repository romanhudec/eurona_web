using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using ImageGalleryItemCommentEntity = CMS.Entities.ImageGalleryItemComment;
using ImageGalleryEntity = CMS.Entities.ImageGallery;
using ImageGalleryItemEntity = CMS.Entities.ImageGalleryItem;
using AccountProfileEntity = CMS.Entities.AccountProfile;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing.Design;
using System.ComponentModel;

namespace CMS.Controls.ImageGallery
{
		// do not show the StatsData class in the Toolbox...
		[ToolboxItem( false )]
		public class ImageGalleryItemCommentsData: WebControl, INamingContainer
		{
				private ImageGalleryItemCommentEntity comment;
				private ImageGalleryItemCommentsControl owner;

				internal ImageGalleryItemCommentsData( ImageGalleryItemCommentsControl owner, ImageGalleryItemCommentEntity comment )
						: base( "div" )
				{
						this.owner = owner;
						this.comment = comment;
						this.CssClass = owner.CssClass + "_container";
						this.Childs = new List<ImageGalleryItemCommentsData>();
				}

				[Bindable( true )]
				public int Id
				{
						get { return this.comment.Id; }
				}

				[Bindable( true )]
				public int CommentId
				{
						get { return this.comment.CommentId; }
				}

				[Bindable( true )]
				public int? ParentId
				{
						get { return this.comment.ParentId; }
				}

				[Bindable( true )]
				public int AccountId
				{
						get { return this.comment.AccountId; }
				}

				[Bindable( true )]
				public DateTime Date
				{
						get { return this.comment.Date; }
				}

				[Bindable( true )]
				public string UserName
				{
						get
						{
								//Zisti sa polozka profilu pouzivatela, ktora sa bude zobrazovat ako autor.
								AccountProfileEntity ap = Storage<AccountProfileEntity>.ReadFirst( new AccountProfileEntity.ReadByAccountAndProfileName { AccountId = this.comment.AccountId, ProfileName = owner.AccountProfileItemName } );
								return (ap != null && ap.Value != string.Empty ) ? ap.Value : this.comment.AccountName;
						}
				}

				[Bindable( true )]
				public string Title
				{
						get { return this.comment.Title; }
				}

				[Bindable( true )]
				public string Content
				{
						get { return this.comment.Content; }
				}

				[Bindable( true )]
				public double RatingResult
				{
						get { return this.comment.RatingResult; }
				}

				[Bindable( true )]
				public List<ImageGalleryItemCommentsData> Childs { get; set; }

		}

		public class ImageGalleryItemCommentsControl: CmsControl
		{
				public int MaxItemsCount { get; set; }
				public string DisplayUrlFormat { get; set; }
				public string CssCarma { get; set; }

				/// <summary>
				/// Nastavi názov položky z profilu používateľa, ktorá sa má zobrazovať ako autorblogu.
				/// </summary>
				public string AccountProfileItemName { get; set; }

				public string CommentFormID
				{
						get { return ViewState["CommentFormID"] == null ? string.Empty : ViewState["CommentFormID"].ToString(); }
						set { ViewState["CommentFormID"] = value; }
				}

				public int ImageGalleryItemId
				{
						get { return Convert.ToInt32( ViewState["ImageGalleryItemId"] ); }
						set { ViewState["ImageGalleryItemId"] = value; }
				}

				[Browsable( false )]
				[DefaultValue( null )]
				[Description( "The comment template." )]
				[TemplateContainer( typeof( ImageGalleryItemCommentsData ) )]
				[PersistenceMode( PersistenceMode.InnerProperty )]
				public virtual ITemplate ImageGalleryItemCommentsTemplate { get; set; }

				// For composite controls, the Controls collection needs to be overriden to
				// ensure that the child controls have been created before the Controls
				// property can be modified by the page developer...
				public override ControlCollection Controls
				{
						get
						{
								this.EnsureChildControls();
								return base.Controls;
						}
				}

				private List<ImageGalleryItemCommentsData> BuildTreeData( List<ImageGalleryItemCommentEntity> entityList )
				{
						List<ImageGalleryItemCommentsData> list = new List<ImageGalleryItemCommentsData>();
						foreach ( ImageGalleryItemCommentEntity entity in entityList )
						{
								if ( entity.ParentId != null ) continue;

								ImageGalleryItemCommentsData data = new ImageGalleryItemCommentsData( this, entity );
								data.Childs.AddRange( GetChilds( data.CommentId, entityList ) );
								list.Add( data );
						}

						return list;
				}

				private List<ImageGalleryItemCommentsData> GetChilds( int parentId, List<ImageGalleryItemCommentEntity> entityList )
				{
						List<ImageGalleryItemCommentsData> list = new List<ImageGalleryItemCommentsData>();
						foreach ( ImageGalleryItemCommentEntity entity in entityList )
						{
								if ( entity.ParentId != parentId ) continue;

								ImageGalleryItemCommentsData data = new ImageGalleryItemCommentsData( this, entity );
								data.Childs.AddRange( GetChilds( data.CommentId, entityList ) );
								list.Add( data );
						}

						return list;
				}

				protected override void CreateChildControls()
				{
						ImageGalleryItemEntity item = Storage<ImageGalleryItemEntity>.ReadFirst( new ImageGalleryItemEntity.ReadById { ImageGalleryItemId = this.ImageGalleryItemId } );
						if ( item == null )
								return;

						ImageGalleryEntity gallery = Storage<ImageGalleryEntity>.ReadFirst( new ImageGalleryEntity.ReadById { ImageGalleryId = item.ImageGalleryId } );
						if ( gallery == null )
								return;

						//Nazov galerie
						HtmlGenericControl divImageGalleryItemTitle = new HtmlGenericControl( "div" );
						divImageGalleryItemTitle.Attributes.Add( "class", this.CssClass + "_title" );
						HyperLink hlGalleryName = new HyperLink();
						hlGalleryName.Text = gallery.Name;
						if ( !string.IsNullOrEmpty( gallery.Alias ) ) hlGalleryName.NavigateUrl = Page.ResolveUrl( gallery.Alias );
						else if ( !string.IsNullOrEmpty( this.DisplayUrlFormat ) )
								hlGalleryName.NavigateUrl = string.Format( Page.ResolveUrl( this.DisplayUrlFormat ), gallery.Id );
						divImageGalleryItemTitle.Controls.Add( hlGalleryName );
						this.Controls.Add( divImageGalleryItemTitle );

						//Obrazok
						HtmlGenericControl divImageGalleryItemImage = new HtmlGenericControl( "div" );
						divImageGalleryItemImage.Attributes.Add( "class", this.CssClass + "_image" );
						Image image = new Image();
						image.ImageUrl = Page.ResolveUrl( item.VirtualThumbnailPath );
						divImageGalleryItemImage.Controls.Add( image );
						this.Controls.Add( divImageGalleryItemImage );

						//Popis
						HtmlGenericControl divImageGalleryItemDescription = new HtmlGenericControl( "div" );
						divImageGalleryItemDescription.Attributes.Add( "class", this.CssClass + "_description" );
						Label lblDescription = new Label();
						lblDescription.Text = item.Description;
						divImageGalleryItemDescription.Controls.Add( lblDescription );
						this.Controls.Add( divImageGalleryItemDescription );

						//BR
						this.Controls.Add( new LiteralControl( "<br/>" ) );


						//Template list
						ITemplate template = this.ImageGalleryItemCommentsTemplate;
						if ( template == null ) template = new DefaultImageGalleryItemCommentsTemplate( this );

						List<ImageGalleryItemCommentEntity> list = Storage<ImageGalleryItemCommentEntity>.Read( new ImageGalleryItemCommentEntity.ReadByImageGalleryItemId { ImageGalleryItemId = this.ImageGalleryItemId } );
						List<ImageGalleryItemCommentsData> dataSource = BuildTreeData( list );

						foreach ( ImageGalleryItemCommentsData data in dataSource )
						{
								template.InstantiateIn( data );
								Controls.Add( data );
						}

						this.DataBind();
				}

				public override void DataBind()
				{
						this.ChildControlsCreated = true;
						base.DataBind();
				}

				internal sealed class DefaultImageGalleryItemCommentsTemplate: ITemplate
				{
						public string CssClass { get; set; }
						public ImageGalleryItemCommentsControl Owner { get; set; }
						public string CommentFormID { get; set; }

						public DefaultImageGalleryItemCommentsTemplate( ImageGalleryItemCommentsControl owner )
						{
								this.Owner = owner;
								this.CssClass = owner.CssClass;
								this.CommentFormID = owner.CommentFormID;
						}

						void ITemplate.InstantiateIn( Control container )
						{
								HtmlGenericControl divReplyFormContainer = new HtmlGenericControl( "div" );

								HtmlGenericControl div = new HtmlGenericControl( "div" );
								div.Attributes.Add( "class", this.CssClass + "_item" );

								Table table = new Table();
								//table.Attributes.Add( "border", "1" );

								//Title
								TableRow row = new TableRow();
								TableCell cell = new TableCell();
								cell = new TableCell();
								cell.VerticalAlign = VerticalAlign.Top;
								cell.CssClass = this.CssClass + "_item_title";
								Label lblTitle = new Label();
								lblTitle.DataBinding += ( sender, e ) =>
								{
										ImageGalleryItemCommentsData data = (ImageGalleryItemCommentsData)lblTitle.NamingContainer;
										Label control = sender as Label;
										control.Text = data.Title;
								};
								cell.Controls.Add( lblTitle );
								row.Cells.Add( cell );

								//Carma
								cell = new TableCell();
								cell.HorizontalAlign = HorizontalAlign.Right;
								cell.VerticalAlign = VerticalAlign.Top;
								cell.CssClass = this.CssClass + "_item_carma";
								Vote.CarmaControl carmaControl = new Vote.CarmaControl();
								carmaControl.CssClass = this.Owner.CssCarma;
								carmaControl.OnVote += ( objectId, rating ) =>
								{
										ImageGalleryItemCommentEntity.IncrementVoteCommand cmd = new ImageGalleryItemCommentEntity.IncrementVoteCommand();
										cmd.AccountId = Security.Account.Id;
										cmd.CommentId = objectId;
										cmd.Rating = rating;
										Storage<ImageGalleryItemCommentEntity>.Execute( cmd );

										ImageGalleryItemCommentEntity comment = Storage<ImageGalleryItemCommentEntity>.ReadFirst( new ImageGalleryItemCommentEntity.ReadByCommentId { CommentId = objectId } );
										return comment.RatingResult;
								};

								carmaControl.DataBinding += ( sender, e ) =>
								{
										ImageGalleryItemCommentsData data = (ImageGalleryItemCommentsData)lblTitle.NamingContainer;
										Vote.CarmaControl control = sender as Vote.CarmaControl;
										control.ObjectId = data.CommentId;
										control.ObjectTypeId = (int)ImageGalleryItemCommentEntity.AccountVoteType;
										control.RatingResult = data.RatingResult;
								};
								cell.Controls.Add( carmaControl );
								row.Cells.Add( cell );

								table.Rows.Add( row );

								//Content
								row = new TableRow();
								cell = new TableCell();
								cell.ColumnSpan = 2;
								Label lblContent = new Label();
								lblContent.CssClass = this.CssClass + "_item_content";
								lblContent.DataBinding += ( sender, e ) =>
								{
										ImageGalleryItemCommentsData data = (ImageGalleryItemCommentsData)lblContent.NamingContainer;
										Label control = sender as Label;
										control.Text = data.Content;
								};
								cell.Controls.Add( lblContent );
								row.Cells.Add( cell );
								table.Rows.Add( row );

								#region Footer
								//Reply button
								row = new TableRow();
								cell = new TableCell();
								HyperLink hlReply = new HyperLink();
								hlReply.CssClass = this.CssClass + "_item_reply";
								hlReply.Text = Resources.Controls.ImageGalleryItemCommentsControl_Reply;
								hlReply.DataBinding += ( sender, e ) =>
								{
										ImageGalleryItemCommentsData data = (ImageGalleryItemCommentsData)hlReply.NamingContainer;
										HtmlGenericControl control = sender as HtmlGenericControl;
										divReplyFormContainer.Attributes.Add( "id", string.Format( "formContainer_{0}", data.CommentId ) );
										hlReply.Attributes.Add( "onclick", string.Format( "showReplyForm('{0}', '{1}', {2} )",
												this.CommentFormID,
												divReplyFormContainer.ClientID,
												data.CommentId ) );

								};
								cell.Controls.Add( hlReply );
								row.Cells.Add( cell );

								//User name
								cell = new TableCell();
								cell.HorizontalAlign = HorizontalAlign.Right;
								Label lblUserName = new Label();
								lblUserName.CssClass = this.CssClass + "_item_user";
								lblUserName.DataBinding += ( sender, e ) =>
								{
										ImageGalleryItemCommentsData data = (ImageGalleryItemCommentsData)lblUserName.NamingContainer;
										Label control = sender as Label;
										control.Text = data.UserName;
								};
								cell.Controls.Add( lblUserName );

								cell.Controls.Add( new LiteralControl( "|" ) );

								//DateTime
								Label lblDateTime = new Label();
								lblDateTime.CssClass = this.CssClass + "_item_date";
								lblDateTime.DataBinding += ( sender, e ) =>
								{
										ImageGalleryItemCommentsData data = (ImageGalleryItemCommentsData)lblDateTime.NamingContainer;
										Label control = sender as Label;
										control.Text = data.Date.ToString();
								};
								cell.Controls.Add( lblDateTime );
								row.Cells.Add( cell );

								table.Rows.Add( row );
								#endregion

								//Reply Form container
								row = new TableRow();
								cell = new TableCell();
								cell.ColumnSpan = 2;
								cell.Controls.Add( divReplyFormContainer );
								row.Cells.Add( cell );
								table.Rows.Add( row );

								//Childs
								row = new TableRow();
								cell = new TableCell();
								cell.ColumnSpan = 2;
								HtmlGenericControl divChilds = new HtmlGenericControl( "div" );
								divChilds.Attributes.Add( "class", this.CssClass + "_item_children" );
								divChilds.DataBinding += ( sender, e ) =>
								{
										ImageGalleryItemCommentsData data = (ImageGalleryItemCommentsData)lblContent.NamingContainer;
										foreach ( ImageGalleryItemCommentsData child in data.Childs )
										{
												ITemplate template = new DefaultImageGalleryItemCommentsTemplate( this.Owner );
												template.InstantiateIn( child );
												divChilds.Controls.Add( child );
										}
								};
								cell.Controls.Add( divChilds );
								row.Cells.Add( cell );
								table.Rows.Add( row );

								div.Controls.Add( table );
								container.Controls.Add( div );
						}
				}

		}
}
