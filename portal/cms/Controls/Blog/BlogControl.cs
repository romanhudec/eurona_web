using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AccountProfileEntity = CMS.Entities.AccountProfile;
using BlogEntity = CMS.Entities.Blog;
using BlogTagEntity = CMS.Entities.BlogTag;
using CMS.Utilities;

namespace CMS.Controls.Blog
{
		public class BlogControl: CmsControl
		{
				[ToolboxItem( false )]
				public class BlogDataControl: WebControl, INamingContainer
				{
						private BlogEntity blog;

						internal BlogDataControl( BlogControl owner, BlogEntity blog )
								: base( "div" )
						{
								this.blog = blog;
						}

						[Bindable( true )]
						public BlogEntity DataItem
						{
								get { return this.blog; }
						}
				}

				public BlogControl()
				{
					this.DefaultTemplateShowHeader = true;
				}

				public string CssRating { get; set; }

				public bool DefaultTemplateShowHeader { get; set; }

				[Browsable( false )]
				[DefaultValue( null )]
				[Description( "Blog template." )]
				[TemplateContainer( typeof( BlogDataControl ) )]
				[PersistenceMode( PersistenceMode.InnerProperty )]
				public virtual ITemplate BlogTemplate { get; set; }

				public string ArchivUrl { get; set; }
				public string CommentsFormatUrl { get; set; }
				/// <summary>
				/// Nastavi názov položky z profilu používateľa, ktorá sa má zobrazovať ako autorblogu.
				/// </summary>
				public string AccountProfileItemName { get; set; }


				/// <summary>
				/// Nastavi blog, ktory sa ma zobrazit.
				/// </summary>
				public int BlogId
				{
						get { return Convert.ToInt32( ViewState["BlogId"] ); }
						set { ViewState["BlogId"] = value; }
				}

				private BlogEntity blog = null;
				public BlogEntity Blog
				{
					get
					{
						if (blog != null) return blog;
						blog = Storage<BlogEntity>.ReadFirst(new BlogEntity.ReadById() { BlogId = this.BlogId });
						return blog;
					}
				}

				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						BlogEntity blog = this.Blog;
						if ( blog == null )
								return;

						//Update ViewCount
						BlogEntity.IncrementViewCountCommand cmd = new CMS.Entities.Blog.IncrementViewCountCommand();
						cmd.BlogId = blog.Id;
						Storage<BlogEntity>.Execute<BlogEntity.IncrementViewCountCommand>( cmd );

						//Select template
						BlogDataControl data = new BlogDataControl( this, blog );
						ITemplate template = this.BlogTemplate;
						if ( template == null ) template = new DefaultTemplate( this, this.ArchivUrl, this.CommentsFormatUrl, this.DefaultTemplateShowHeader );
						template.InstantiateIn( data );
						this.Controls.Add( data );

						this.DataBind();
				}
				public override void DataBind()
				{
						this.ChildControlsCreated = true;
						base.DataBind();
				}
				#endregion

				private class DefaultTemplate: ITemplate
				{
						#region Public properties
						public string CssClass { get; set; }
						public string CssRating { get; set; }
						public string AccountProfileItemName { get; set; }
						public bool ShowHeader { get; set; }
						private string archivUrl;
						private string commentsFormatUrl;
						#endregion

						#region ITemplate Members

						public DefaultTemplate( BlogControl owner, string archivUrl, string commentsFormatUrl, bool showHeader )
						{
								this.CssClass = owner.CssClass;
								this.CssRating = owner.CssRating;
								this.AccountProfileItemName = owner.AccountProfileItemName;
								this.ShowHeader = showHeader;

								if ( !string.IsNullOrEmpty( archivUrl ) )
								{
										AliasUtilities aliasUtils = new AliasUtilities();
										this.archivUrl = aliasUtils.Resolve( archivUrl, owner.Page );
								}

								if ( !string.IsNullOrEmpty( commentsFormatUrl ) )
								{
										string url = string.Format( commentsFormatUrl, owner.BlogId );
										AliasUtilities aliasUtils = new AliasUtilities();
										url = aliasUtils.Resolve( url, owner.Page );

										this.commentsFormatUrl = url + ( url.Contains( "?" ) ? "&" : "?" ) + owner.BuildReturnUrlQueryParam();
								}
						}

						public void InstantiateIn( Control container )
						{
								Control control = CreateDetailControl( container );
								if ( control != null )
										container.Controls.Add( control );
						}
						#endregion

						/// <summary>
						/// Vytvori Control Blog
						/// </summary>
						private Control CreateDetailControl( Control container )
						{
								BlogEntity blog = ( container as BlogDataControl ).DataItem;

								//Zisti sa polozka profilu pouzivatela, ktora sa bude zobrazovat ako autor.
								AccountProfileEntity ap = Storage<AccountProfileEntity>.ReadFirst( new AccountProfileEntity.ReadByAccountAndProfileName { AccountId = blog.AccountId, ProfileName = this.AccountProfileItemName } );

								HtmlGenericControl div = new HtmlGenericControl( "div" );
								div.Attributes.Add( "class", this.CssClass );

								//Title
								Table table = new Table();
								//table.Attributes.Add("border", "1");
								table.Width = Unit.Percentage( 100 );
								TableRow row;
								TableCell cell;
								if (ShowHeader) {
									row = new TableRow();
									cell = new TableCell();
									cell.ColumnSpan = 3;
									cell.CssClass = this.CssClass + "_title";
									cell.Text = blog.Title;
									row.Cells.Add(cell);
									table.Rows.Add(row);
								}

								//Source
								StringBuilder sb = new StringBuilder();
								sb.Append( blog.ReleaseDate.ToShortDateString() );
								if ( !string.IsNullOrEmpty( blog.City ) ) sb.AppendFormat( ", {0}", blog.City );
								if ( !string.IsNullOrEmpty( blog.Country ) ) sb.AppendFormat( ", {0}", blog.Country );
								row = new TableRow();
								cell = new TableCell();
								cell.CssClass = this.CssClass + "_source";
								cell.Text = sb.ToString();
								row.Cells.Add( cell );

								cell = new TableCell();
								cell.HorizontalAlign = HorizontalAlign.Center;
								foreach ( BlogTagEntity at in blog.BlogTags )
								{
										HyperLink hlTag = new HyperLink();
										hlTag.CssClass = this.CssClass + "_tag";
										hlTag.Text = at.Name;
										hlTag.NavigateUrl = container.ResolveUrl( string.Format( "{0}?tag={1}", this.archivUrl, at.TagId ) );
										cell.Controls.Add( hlTag );
										cell.Controls.Add( new LiteralControl( "&nbsp;" ) );
								}
								row.Cells.Add( cell );

								cell = new TableCell();
								cell.HorizontalAlign = HorizontalAlign.Right;
								HyperLink hlUser = new HyperLink();
								hlUser.CssClass = this.CssClass + "_user";
								//hlUser.Text = Resources.Controls.BlogControl_UserBlogs + " '" + ( ( ap != null && ap.Value != string.Empty ) ? ap.Value : blog.UserName ) + "'";
								hlUser.Text = (ap != null && ap.Value != string.Empty) ? ap.Value : blog.UserName;
								hlUser.NavigateUrl = container.ResolveUrl( string.Format( "{0}?user={1}", this.archivUrl, blog.AccountId ) );
								cell.Controls.Add( hlUser );
								row.Cells.Add( cell );
								table.Rows.Add( row );

								//Teaser
								row = new TableRow();
								cell = new TableCell();
								cell.ColumnSpan = 3;
								cell.CssClass = this.CssClass + "_teaser";
								cell.Controls.Add( new LiteralControl( blog.Teaser ) );
								row.Cells.Add( cell );
								table.Rows.Add( row );

								//Content
								row = new TableRow();
								cell = new TableCell();
								cell.ColumnSpan = 3;
								cell.CssClass = this.CssClass + "_content";
								cell.Controls.Add( new LiteralControl( blog.Content ) );
								row.Cells.Add( cell );
								table.Rows.Add( row );

								//Carma & Comment & ViewCount
								row = new TableRow();
								cell = new TableCell();
								cell.CssClass = this.CssClass + "_rating";
								Vote.RaitingControl ratingControl = new Vote.RaitingControl();
								ratingControl.ObjectId = blog.Id;
								ratingControl.ObjectTypeId = (int)BlogEntity.AccountVoteType;
								ratingControl.RatingResult = blog.RatingResult;
								ratingControl.CssClass = this.CssRating;
								ratingControl.OnVote += ( objectId, rating ) =>
								{
										BlogEntity.IncrementVoteCommand cmdVote = new BlogEntity.IncrementVoteCommand();
										cmdVote.AccountId = Security.Account.Id;
										cmdVote.BlogId = objectId;
										cmdVote.Rating = rating;
										Storage<BlogEntity>.Execute( cmdVote );

										BlogEntity comment = Storage<BlogEntity>.ReadFirst( new BlogEntity.ReadById { BlogId = objectId } );
										return comment.RatingResult;
								};
								cell.Controls.Add( ratingControl );
								row.Cells.Add( cell );

								//Comment
								sb = new StringBuilder();
								if ( blog.EnableComments && !string.IsNullOrEmpty( commentsFormatUrl ) )
								{
										sb.AppendFormat( "<a href='{1}' class='{2}'>{0} ({3})</a>&nbsp;|&nbsp;",
												Resources.Controls.ArticleControl_CommentsCount,
												container.ResolveUrl( commentsFormatUrl ),
												this.CssClass + "_comment",
												blog.CommentsCount );
								}
								sb.AppendFormat( "{0} : <b><span class='{1}'>{2}</span></b> x",
										Resources.Controls.BlogControl_ViewCount,
										this.CssClass + "_viewCount",
										blog.ViewCount );
								cell = new TableCell();
								cell.ColumnSpan = 2;
								cell.HorizontalAlign = HorizontalAlign.Right;
								cell.Controls.Add( new LiteralControl( sb.ToString() ) );
								row.Cells.Add( cell );
								table.Rows.Add( row );

								//Archiv link
								row = new TableRow();
								cell = new TableCell();
								cell.ColumnSpan = 3;
								cell.CssClass = this.CssClass + "_archivLink";
								cell.VerticalAlign = VerticalAlign.Bottom;
								HyperLink link = new HyperLink();
								link.NavigateUrl = container.ResolveUrl( this.archivUrl );
								link.Text = Resources.Controls.BlogControl_BlogArchivLink;
								cell.Controls.Add( link );
								row.Cells.Add( cell );
								table.Rows.Add( row );

								div.Controls.Add( table );

								return div;
						}
				}
		}
}
