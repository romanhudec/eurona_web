using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using ArticleCommentEntity = CMS.Entities.ArticleComment;
using ArticleEntity = CMS.Entities.Article;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing.Design;
using System.ComponentModel;

namespace CMS.Controls.Article
{
		// do not show the StatsData class in the Toolbox...
		[ToolboxItem( false )]
		public class ArticleCommentsData: WebControl, INamingContainer
		{
				private ArticleCommentEntity comment;

				internal ArticleCommentsData( ArticleCommentsControl owner, ArticleCommentEntity comment )
						: base( "div" )
				{
						this.comment = comment;
						this.CssClass = owner.CssClass + "_container";
						this.Childs = new List<ArticleCommentsData>();
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
				public List<ArticleCommentsData> Childs { get; set; }

		}

		public class ArticleCommentsControl: CmsControl
		{
				public int MaxItemsCount { get; set; }
				public string DisplayUrlFormat { get; set; }
				public string CssCarma { get; set; }

				public string CommentFormID
				{
						get { return ViewState["CommentFormID"] == null ? string.Empty : ViewState["CommentFormID"].ToString(); }
						set { ViewState["CommentFormID"] = value; }
				}

				public int ArticleId
				{
						get { return Convert.ToInt32( ViewState["ArticleId"] ); }
						set { ViewState["ArticleId"] = value; }
				}

				[Browsable( false )]
				[DefaultValue( null )]
				[Description( "The comment template." )]
				[TemplateContainer( typeof( ArticleCommentsData ) )]
				[PersistenceMode( PersistenceMode.InnerProperty )]
				public virtual ITemplate ArticleCommentsTemplate { get; set; }

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

				private List<ArticleCommentsData> BuildTreeData( List<ArticleCommentEntity> entityList )
				{
						List<ArticleCommentsData> list = new List<ArticleCommentsData>();
						foreach ( ArticleCommentEntity entity in entityList )
						{
								if ( entity.ParentId != null ) continue;

								ArticleCommentsData data = new ArticleCommentsData( this, entity );
								data.Childs.AddRange( GetChilds( data.CommentId, entityList ) );
								list.Add( data );
						}

						return list;
				}

				private List<ArticleCommentsData> GetChilds( int parentId, List<ArticleCommentEntity> entityList )
				{
						List<ArticleCommentsData> list = new List<ArticleCommentsData>();
						foreach ( ArticleCommentEntity entity in entityList )
						{
								if ( entity.ParentId != parentId ) continue;

								ArticleCommentsData data = new ArticleCommentsData( this, entity );
								data.Childs.AddRange( GetChilds( data.CommentId, entityList ) );
								list.Add( data );
						}

						return list;
				}

				public override void RenderControl( HtmlTextWriter writer )
				{
						writer.Write(string.Format("<div id='{0}' class='{1}'>", this.ClientID, this.CssClass ) );
						base.RenderControl( writer );
						writer.Write("</div>" );
				}
				protected override void CreateChildControls()
				{
						ArticleEntity article = Storage<ArticleEntity>.ReadFirst( new ArticleEntity.ReadById { ArticleId = this.ArticleId } );
						if ( article == null )
								return;

						//Nadpis clanku
						HtmlGenericControl divArticleTitle = new HtmlGenericControl( "div" );
						divArticleTitle.Attributes.Add( "class", this.CssClass + "_title" );
						HyperLink hlArtisleTitle = new HyperLink();
						hlArtisleTitle.Text = article.Title;
						if ( !string.IsNullOrEmpty( article.Alias ) ) hlArtisleTitle.NavigateUrl = Page.ResolveUrl( article.Alias );
						else if ( !string.IsNullOrEmpty( this.DisplayUrlFormat ) )
								hlArtisleTitle.NavigateUrl = string.Format( Page.ResolveUrl( this.DisplayUrlFormat ), this.ArticleId );
						divArticleTitle.Controls.Add( hlArtisleTitle );
						this.Controls.Add( divArticleTitle );

						//BR
						this.Controls.Add( new LiteralControl( "<br/>" ) );

						HtmlGenericControl divReply = new HtmlGenericControl( "div" );
						HyperLink hlReply = new HyperLink();
						hlReply.CssClass = this.CssClass + "_item_reply";
						hlReply.Text = Resources.Controls.ArticleCommentsControl_Reply;
						hlReply.DataBinding += ( sender, e ) =>
						{
								HtmlGenericControl control = sender as HtmlGenericControl;
								hlReply.Attributes.Add( "onclick", string.Format( "showReplyForm('{0}', '{1}', null )",
										this.CommentFormID,
										this.ClientID ) );

						};
						divReply.Controls.Add( hlReply );
						this.Controls.Add( divReply );

						//BR
						this.Controls.Add( new LiteralControl( "<br/>" ) );

						HtmlGenericControl txtContent = new HtmlGenericControl( "div" );
						txtContent.ID = "txtContent";
						txtContent.Attributes.Add( "class", this.CssClass + "_content" );
						txtContent.Style.Add( "overflow", "scroll" );
						txtContent.Style.Add( "border", "solid 1px #DDDDDD" );
						txtContent.InnerHtml = article.Content;
						txtContent.Style.Add( "width", "100%" );
						txtContent.Style.Add( "height", "100px" );
						this.Controls.Add( txtContent );
						
						//BR
						this.Controls.Add( new LiteralControl( "<br/>" ) );
						
						//Template list
						ITemplate template = this.ArticleCommentsTemplate;
						if ( template == null ) template = new DefaultArticleCommentsTemplate( this );

						List<ArticleCommentEntity> list = Storage<ArticleCommentEntity>.Read( new ArticleCommentEntity.ReadByArticleId { ArticleId = this.ArticleId } );
						List<ArticleCommentsData> dataSource = BuildTreeData( list );

						foreach ( ArticleCommentsData data in dataSource )
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

				internal sealed class DefaultArticleCommentsTemplate: ITemplate
				{
						public string CssClass { get; set; }
						public ArticleCommentsControl Owner { get; set; }
						public string CommentFormID { get; set; }

						public DefaultArticleCommentsTemplate( ArticleCommentsControl owner )
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
										ArticleCommentsData data = (ArticleCommentsData)lblTitle.NamingContainer;
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
										ArticleCommentEntity.IncrementVoteCommand cmd = new ArticleCommentEntity.IncrementVoteCommand();
										cmd.AccountId = Security.Account.Id;
										cmd.CommentId = objectId;
										cmd.Rating = rating;
										Storage<ArticleCommentEntity>.Execute( cmd );

										ArticleCommentEntity comment = Storage<ArticleCommentEntity>.ReadFirst( new ArticleCommentEntity.ReadByCommentId { CommentId = objectId } );
										return comment.RatingResult;
								};

								carmaControl.DataBinding += ( sender, e ) =>
								{
										ArticleCommentsData data = (ArticleCommentsData)lblTitle.NamingContainer;
										Vote.CarmaControl control = sender as Vote.CarmaControl;
										control.ObjectId = data.CommentId;
										control.ObjectTypeId = (int)ArticleCommentEntity.AccountVoteType;
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
										ArticleCommentsData data = (ArticleCommentsData)lblContent.NamingContainer;
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
								hlReply.Text = Resources.Controls.ArticleCommentsControl_Reply;
								hlReply.DataBinding += ( sender, e ) =>
								{
										ArticleCommentsData data = (ArticleCommentsData)hlReply.NamingContainer;
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
										ArticleCommentsData data = (ArticleCommentsData)lblUserName.NamingContainer;
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
										ArticleCommentsData data = (ArticleCommentsData)lblDateTime.NamingContainer;
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
										ArticleCommentsData data = (ArticleCommentsData)lblContent.NamingContainer;
										foreach ( ArticleCommentsData child in data.Childs )
										{
												ITemplate template = new DefaultArticleCommentsTemplate( this.Owner );
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
