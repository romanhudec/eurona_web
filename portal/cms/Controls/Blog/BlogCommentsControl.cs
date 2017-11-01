using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using BlogCommentEntity = CMS.Entities.BlogComment;
using BlogEntity = CMS.Entities.Blog;
using AccountProfileEntity = CMS.Entities.AccountProfile;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing.Design;
using System.ComponentModel;

namespace CMS.Controls.Blog
{
		// do not show the StatsData class in the Toolbox...
		[ToolboxItem( false )]
		public class BlogCommentsData: WebControl, INamingContainer
		{
				private BlogCommentEntity comment;
				private BlogCommentsControl owner;

				internal BlogCommentsData( BlogCommentsControl owner, BlogCommentEntity comment )
						: base( "div" )
				{
						this.owner = owner;
						this.comment = comment;
						this.CssClass = owner.CssClass + "_container";
						this.Childs = new List<BlogCommentsData>();
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
				public List<BlogCommentsData> Childs { get; set; }

		}

		public class BlogCommentsControl: CmsControl
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

				public int BlogId
				{
						get { return Convert.ToInt32( ViewState["BlogId"] ); }
						set { ViewState["BlogId"] = value; }
				}

				[Browsable( false )]
				[DefaultValue( null )]
				[Description( "The comment template." )]
				[TemplateContainer( typeof( BlogCommentsData ) )]
				[PersistenceMode( PersistenceMode.InnerProperty )]
				public virtual ITemplate BlogCommentsTemplate { get; set; }

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

				private List<BlogCommentsData> BuildTreeData( List<BlogCommentEntity> entityList )
				{
						List<BlogCommentsData> list = new List<BlogCommentsData>();
						foreach ( BlogCommentEntity entity in entityList )
						{
								if ( entity.ParentId != null ) continue;

								BlogCommentsData data = new BlogCommentsData( this, entity );
								data.Childs.AddRange( GetChilds( data.CommentId, entityList ) );
								list.Add( data );
						}

						return list;
				}

				private List<BlogCommentsData> GetChilds( int parentId, List<BlogCommentEntity> entityList )
				{
						List<BlogCommentsData> list = new List<BlogCommentsData>();
						foreach ( BlogCommentEntity entity in entityList )
						{
								if ( entity.ParentId != parentId ) continue;

								BlogCommentsData data = new BlogCommentsData( this, entity );
								data.Childs.AddRange( GetChilds( data.CommentId, entityList ) );
								list.Add( data );
						}

						return list;
				}

				public override void RenderControl( HtmlTextWriter writer )
				{
						writer.Write( string.Format( "<div id='{0}' class='{1}'>", this.ClientID, this.CssClass ) );
						base.RenderControl( writer );
						writer.Write( "</div>" );
				}

				protected override void CreateChildControls()
				{
						BlogEntity blog = Storage<BlogEntity>.ReadFirst( new BlogEntity.ReadById { BlogId = this.BlogId } );
						if ( blog == null )
								return;

						//Nadpis clanku
						HtmlGenericControl divBlogTitle = new HtmlGenericControl( "div" );
						divBlogTitle.Attributes.Add( "class", this.CssClass + "_title" );
						HyperLink hlBlogTitle = new HyperLink();
						hlBlogTitle.Text = blog.Title;
						if ( !string.IsNullOrEmpty( blog.Alias ) ) hlBlogTitle.NavigateUrl = Page.ResolveUrl( blog.Alias );
						else if ( !string.IsNullOrEmpty( this.DisplayUrlFormat ) )
								hlBlogTitle.NavigateUrl = string.Format( Page.ResolveUrl( this.DisplayUrlFormat ), this.BlogId );
						divBlogTitle.Controls.Add( hlBlogTitle );
						this.Controls.Add( divBlogTitle );

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
						txtContent.InnerHtml = blog.Content;
						txtContent.Style.Add( "width", "100%" );
						txtContent.Style.Add( "height", "100px" );
						this.Controls.Add( txtContent );

						//BR
						this.Controls.Add( new LiteralControl( "<br/>" ) );


						//Template list
						ITemplate template = this.BlogCommentsTemplate;
						if ( template == null ) template = new DefaultBlogCommentsTemplate( this );

						List<BlogCommentEntity> list = Storage<BlogCommentEntity>.Read( new BlogCommentEntity.ReadByBlogId { BlogId = this.BlogId } );
						List<BlogCommentsData> dataSource = BuildTreeData( list );

						foreach ( BlogCommentsData data in dataSource )
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

				internal sealed class DefaultBlogCommentsTemplate: ITemplate
				{
						public string CssClass { get; set; }
						public BlogCommentsControl Owner { get; set; }
						public string CommentFormID { get; set; }

						public DefaultBlogCommentsTemplate( BlogCommentsControl owner )
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
										BlogCommentsData data = (BlogCommentsData)lblTitle.NamingContainer;
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
										BlogCommentEntity.IncrementVoteCommand cmd = new BlogCommentEntity.IncrementVoteCommand();
										cmd.AccountId = Security.Account.Id;
										cmd.CommentId = objectId;
										cmd.Rating = rating;
										Storage<BlogCommentEntity>.Execute( cmd );

										BlogCommentEntity comment = Storage<BlogCommentEntity>.ReadFirst( new BlogCommentEntity.ReadByCommentId { CommentId = objectId } );
										return comment.RatingResult;
								};

								carmaControl.DataBinding += ( sender, e ) =>
								{
										BlogCommentsData data = (BlogCommentsData)lblTitle.NamingContainer;
										Vote.CarmaControl control = sender as Vote.CarmaControl;
										control.ObjectId = data.CommentId;
										control.ObjectTypeId = (int)BlogCommentEntity.AccountVoteType;
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
										BlogCommentsData data = (BlogCommentsData)lblContent.NamingContainer;
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
								hlReply.Text = Resources.Controls.BlogCommentsControl_Reply;
								hlReply.DataBinding += ( sender, e ) =>
								{
										BlogCommentsData data = (BlogCommentsData)hlReply.NamingContainer;
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
										BlogCommentsData data = (BlogCommentsData)lblUserName.NamingContainer;
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
										BlogCommentsData data = (BlogCommentsData)lblDateTime.NamingContainer;
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
										BlogCommentsData data = (BlogCommentsData)lblContent.NamingContainer;
										foreach ( BlogCommentsData child in data.Childs )
										{
												ITemplate template = new DefaultBlogCommentsTemplate( this.Owner );
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
