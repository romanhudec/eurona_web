using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using ImageGalleryCommentEntity = CMS.Entities.ImageGalleryComment;
using ImageGalleryEntity = CMS.Entities.ImageGallery;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing.Design;
using System.ComponentModel;

namespace CMS.Controls.ImageGallery
{
		// do not show the StatsData class in the Toolbox...
		[ToolboxItem( false )]
		public class ImageGalleryCommentsData: WebControl, INamingContainer
		{
				private ImageGalleryCommentEntity comment;

				internal ImageGalleryCommentsData( ImageGalleryCommentsControl owner, ImageGalleryCommentEntity comment )
						: base( "div" )
				{
						this.comment = comment;
						this.CssClass = owner.CssClass + "_container";
						this.Childs = new List<ImageGalleryCommentsData>();
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
				public string AccountName
				{
						get { return this.comment.AccountName; }
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
				public List<ImageGalleryCommentsData> Childs { get; set; }

		}

		/// <summary>
		/// Control zobrazi komentare pre danu galeriu.
		/// </summary>
		public class ImageGalleryCommentsControl: CmsControl
		{
				public int MaxItemsCount { get; set; }
				public string DisplayUrlFormat { get; set; }
				public string CssCarma { get; set; }

				public string CommentFormID
				{
						get { return ViewState["CommentFormID"] == null ? string.Empty : ViewState["CommentFormID"].ToString(); }
						set { ViewState["CommentFormID"] = value; }
				}

				public int ImageGalleryId
				{
						get { return Convert.ToInt32( ViewState["ImageGalleryId"] ); }
						set { ViewState["ImageGalleryId"] = value; }
				}

				[Browsable( false )]
				[DefaultValue( null )]
				[Description( "The comment template." )]
				[TemplateContainer( typeof( ImageGalleryCommentsData ) )]
				[PersistenceMode( PersistenceMode.InnerProperty )]
				public virtual ITemplate ImageGalleryCommentsTemplate { get; set; }

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

				private List<ImageGalleryCommentsData> BuildTreeData( List<ImageGalleryCommentEntity> entityList )
				{
						List<ImageGalleryCommentsData> list = new List<ImageGalleryCommentsData>();
						foreach ( ImageGalleryCommentEntity entity in entityList )
						{
								if ( entity.ParentId != null ) continue;

								ImageGalleryCommentsData data = new ImageGalleryCommentsData( this, entity );
								data.Childs.AddRange( GetChilds( data.CommentId, entityList ) );
								list.Add( data );
						}

						return list;
				}

				private List<ImageGalleryCommentsData> GetChilds( int parentId, List<ImageGalleryCommentEntity> entityList )
				{
						List<ImageGalleryCommentsData> list = new List<ImageGalleryCommentsData>();
						foreach ( ImageGalleryCommentEntity entity in entityList )
						{
								if ( entity.ParentId != parentId ) continue;

								ImageGalleryCommentsData data = new ImageGalleryCommentsData( this, entity );
								data.Childs.AddRange( GetChilds( data.CommentId, entityList ) );
								list.Add( data );
						}

						return list;
				}

				protected override void CreateChildControls()
				{
						ImageGalleryEntity imageGallery = Storage<ImageGalleryEntity>.ReadFirst( new ImageGalleryEntity.ReadById { ImageGalleryId = this.ImageGalleryId } );
						if ( imageGallery == null )
								return;

						//Nazov galerie
						HtmlGenericControl divImageGalleryTitle = new HtmlGenericControl( "div" );
						divImageGalleryTitle.Attributes.Add( "class", this.CssClass + "_title" );
						HyperLink hlGalleryName = new HyperLink();
						hlGalleryName.Text = imageGallery.Name;
						if( !string.IsNullOrEmpty(imageGallery.Alias ) ) hlGalleryName.NavigateUrl = Page.ResolveUrl( imageGallery.Alias );
						else if ( !string.IsNullOrEmpty( this.DisplayUrlFormat ) )
								hlGalleryName.NavigateUrl = string.Format( Page.ResolveUrl( this.DisplayUrlFormat ), this.ImageGalleryId );
						divImageGalleryTitle.Controls.Add( hlGalleryName );
						this.Controls.Add( divImageGalleryTitle );

						//BR
						this.Controls.Add( new LiteralControl( "<br/>" ) );


						//Template list
						ITemplate template = this.ImageGalleryCommentsTemplate;
						if ( template == null ) template = new DefaultImageGalleryCommentsTemplate( this );

						List<ImageGalleryCommentEntity> list = Storage<ImageGalleryCommentEntity>.Read( new ImageGalleryCommentEntity.ReadByImageGalleryId { ImageGalleryId = this.ImageGalleryId } );
						List<ImageGalleryCommentsData> dataSource = BuildTreeData( list );

						foreach ( ImageGalleryCommentsData data in dataSource )
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

				internal sealed class DefaultImageGalleryCommentsTemplate: ITemplate
				{
						public string CssClass { get; set; }
						public ImageGalleryCommentsControl Owner { get; set; }
						public string CommentFormID { get; set; }

						public DefaultImageGalleryCommentsTemplate( ImageGalleryCommentsControl owner )
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
										ImageGalleryCommentsData data = (ImageGalleryCommentsData)lblTitle.NamingContainer;
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
										ImageGalleryCommentEntity.IncrementVoteCommand cmd = new ImageGalleryCommentEntity.IncrementVoteCommand();
										cmd.AccountId = Security.Account.Id;
										cmd.CommentId = objectId;
										cmd.Rating = rating;
										Storage<ImageGalleryCommentEntity>.Execute( cmd );

										ImageGalleryCommentEntity comment = Storage<ImageGalleryCommentEntity>.ReadFirst( new ImageGalleryCommentEntity.ReadByCommentId{ CommentId=objectId});
										return comment.RatingResult;
								};

								carmaControl.DataBinding += ( sender, e ) =>
								{
										ImageGalleryCommentsData data = (ImageGalleryCommentsData)lblTitle.NamingContainer;
										Vote.CarmaControl control = sender as Vote.CarmaControl;
										control.ObjectId = data.CommentId;
										control.ObjectTypeId = (int)ImageGalleryCommentEntity.AccountVoteType;
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
										ImageGalleryCommentsData data = (ImageGalleryCommentsData)lblContent.NamingContainer;
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
								hlReply.Text = Resources.Controls.ImageGalleryCommentsControl_Reply;
								hlReply.DataBinding += ( sender, e ) =>
								{
										ImageGalleryCommentsData data = (ImageGalleryCommentsData)hlReply.NamingContainer;
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
										ImageGalleryCommentsData data = (ImageGalleryCommentsData)lblUserName.NamingContainer;
										Label control = sender as Label;
										control.Text = data.AccountName;
								};
								cell.Controls.Add( lblUserName );

								cell.Controls.Add( new LiteralControl( "|" ) );

								//DateTime
								Label lblDateTime = new Label();
								lblDateTime.CssClass = this.CssClass + "_item_date";
								lblDateTime.DataBinding += ( sender, e ) =>
								{
										ImageGalleryCommentsData data = (ImageGalleryCommentsData)lblDateTime.NamingContainer;
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
										ImageGalleryCommentsData data = (ImageGalleryCommentsData)lblContent.NamingContainer;
										foreach ( ImageGalleryCommentsData child in data.Childs )
										{
												ITemplate template = new DefaultImageGalleryCommentsTemplate( this.Owner );
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
