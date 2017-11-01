using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using ArticleEntity = CMS.Entities.Article;
using ArticleTagEntity = CMS.Entities.ArticleTag;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Drawing.Design;
using System.ComponentModel;
using CMS.Utilities;

namespace CMS.Controls.Article
{
		public class ArticleControl: CmsControl
		{
				[ToolboxItem( false )]
				public class ArticleDataControl: WebControl, INamingContainer
				{
						private ArticleEntity article;

						internal ArticleDataControl( ArticleControl owner, ArticleEntity article )
								: base( "div" )
						{
								this.article = article;
						}

						[Bindable( true )]
						public ArticleEntity DataItem
						{
								get { return this.article; }
						}
				}

				public ArticleControl()
				{
						this.DefaultTemplateShowHeader = true;
				}

                public delegate string ContentProcessorHandler(ArticleControl sender, string content);
                public event ContentProcessorHandler ContentProcessor;

				public string CssRating { get; set; }

				public bool DefaultTemplateShowHeader { get; set; }

				[Browsable( false )]
				[DefaultValue( null )]
				[Description( "Article template." )]
				[TemplateContainer( typeof( ArticleDataControl ) )]
				[PersistenceMode( PersistenceMode.InnerProperty )]
				public virtual ITemplate ArticleTemplate { get; set; }

				public string ArchivUrl { get; set; }
				public string CommentsFormatUrl { get; set; }


				/// <summary>
				/// Nastavi clanok, ktora sa ma zobrazit.
				/// </summary>
				public int ArticleId
				{
						get { return Convert.ToInt32( ViewState["ArticleId"] ); }
						set { ViewState["ArticleId"] = value; }
				}

				private ArticleEntity article = null;
				public ArticleEntity Article
				{
						get
						{
								if ( article != null ) return article;
								article = Storage<ArticleEntity>.ReadFirst( new ArticleEntity.ReadById() { ArticleId = this.ArticleId } );
								return article;
						}
				}

				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						ArticleEntity article = this.Article;
						if ( article == null )
								return;

						//Update ViewCount
						ArticleEntity.IncrementViewCountCommand cmd = new CMS.Entities.Article.IncrementViewCountCommand();
						cmd.ArticleId = article.Id;
						Storage<ArticleEntity>.Execute<ArticleEntity.IncrementViewCountCommand>( cmd );

						//Select template
						ArticleDataControl data = new ArticleDataControl( this, article );
                        // oxy: potrebujem aby som pocas InstantiateIn mohol pouzit Aliasutilities.ResolveUrl
                        data.Page = this.Page;
						ITemplate template = this.ArticleTemplate;
						if ( template == null ) template = new DefaultTemplate( this, this.ArchivUrl, this.CommentsFormatUrl, this.DefaultTemplateShowHeader );
						template.InstantiateIn( data );
						this.Controls.Add( data );
				}
				#endregion

                public string ApplyContentProcessor(string content)
                {
                    if (ContentProcessor != null) content = ContentProcessor(this, content);
                    return content;
                }

				private class DefaultTemplate: ITemplate
				{
                        public readonly ArticleControl Owner;

						#region Public properties
						public string CssClass { get; set; }
						public string CssRating { get; set; }
						public bool ShowHeader { get; set; }
						private string archivUrl;
						private string returnUrl;
						private string commentsFormatUrl;
						#endregion

						#region ITemplate Members

						public DefaultTemplate( ArticleControl owner, string archivUrl, string commentsFormatUrl, bool showHeader )
						{
                                this.Owner = owner;
								this.CssClass = owner.CssClass;
								this.CssRating = owner.CssRating;
								this.ShowHeader = showHeader;

								if ( !string.IsNullOrEmpty( archivUrl ) )
								{
										AliasUtilities aliasUtils = new AliasUtilities();
										this.archivUrl = aliasUtils.Resolve( archivUrl, owner.Page );
								}

								if ( !string.IsNullOrEmpty( commentsFormatUrl ) )
								{
										string url = string.Format( commentsFormatUrl, owner.ArticleId );
										AliasUtilities aliasUtils = new AliasUtilities();
										url = aliasUtils.Resolve( url, owner.Page );

										this.commentsFormatUrl = url + ( url.Contains( "?" ) ? "&" : "?" ) + owner.BuildReturnUrlQueryParam();
								}

								this.returnUrl = owner.ReturnUrl;
						}

						public void InstantiateIn( Control container )
						{
								Control control = CreateDetailControl( container );
								if ( control != null )
										container.Controls.Add( control );
						}
						#endregion

						/// <summary>
						/// Vytvori Control Clanok
						/// </summary>
						private Control CreateDetailControl( Control container )
						{
								ArticleEntity article = ( container as ArticleDataControl ).DataItem;
                                AliasUtilities aliasUtils = new AliasUtilities();

								HtmlGenericControl div = new HtmlGenericControl( "div" );
								div.Attributes.Add( "class", this.CssClass );

								//Title
								Table table = new Table();
                                table.Attributes.Add("class", this.CssClass + "_table");
								//table.Attributes.Add("border", "1");
								//table.Width = Unit.Percentage( 100 );
								TableRow row;
								TableCell cell;
								if ( ShowHeader )
								{
										row = new TableRow();
										cell = new TableCell();
										cell.ColumnSpan = 3;
										cell.CssClass = this.CssClass + "_title";
										cell.Text = article.Title;
										row.Cells.Add( cell );
										table.Rows.Add( row );
								}

								//Source
								StringBuilder sb = new StringBuilder();
								sb.Append( article.ReleaseDate.ToShortDateString() );
								if ( !string.IsNullOrEmpty( article.City ) ) sb.AppendFormat( ", {0}", article.City );
								if ( !string.IsNullOrEmpty( article.Country ) ) sb.AppendFormat( ", {0}", article.Country );
								row = new TableRow();
								cell = new TableCell();
								cell.CssClass = this.CssClass + "_source";
								cell.Text = sb.ToString();
								row.Cells.Add( cell );

								cell = new TableCell();
								cell.HorizontalAlign = HorizontalAlign.Center;
								foreach ( ArticleTagEntity at in article.ArticleTags )
								{
										HyperLink hlTag = new HyperLink();
										hlTag.CssClass = this.CssClass + "_tag";
										hlTag.Text = at.Name;
                                        // oxy: pridanie aliasov
										//hlTag.NavigateUrl = container.ResolveUrl( string.Format( "{0}?tag={1}", this.archivUrl, at.TagId ) );
                                        hlTag.NavigateUrl = aliasUtils.Resolve(String.Format("{0}?tag={1}", this.archivUrl, at.TagId), container.Page);
										cell.Controls.Add( hlTag );
										cell.Controls.Add( new LiteralControl( "&nbsp;" ) );
								}
								row.Cells.Add( cell );

								cell = new TableCell();
								cell.HorizontalAlign = HorizontalAlign.Right;
								HyperLink hlCategory = new HyperLink();
								hlCategory.CssClass = this.CssClass + "_category";
								hlCategory.Text = article.ArticleCategoryName;
                                // oxy: pridanie aliasov v defaultnej sablone
                                //hlCategory.NavigateUrl = container.ResolveUrl( string.Format( "{0}?category={1}", this.archivUrl, article.ArticleCategoryId ) );
                                hlCategory.NavigateUrl = aliasUtils.Resolve(String.Format("{0}?category={1}", this.archivUrl, article.ArticleCategoryId), container.Page);
								cell.Controls.Add( hlCategory );
								row.Cells.Add( cell );
								table.Rows.Add( row );

								//Teaser
								row = new TableRow();
								cell = new TableCell();
								cell.ColumnSpan = 3;
								cell.CssClass = this.CssClass + "_teaser";
								cell.Controls.Add( new LiteralControl( article.Teaser ) );
								row.Cells.Add( cell );
								table.Rows.Add( row );

								//Content
								row = new TableRow();
								cell = new TableCell();
								cell.ColumnSpan = 3;
								cell.CssClass = this.CssClass + "_content";
                                cell.Controls.Add(new LiteralControl(this.Owner.ApplyContentProcessor(article.Content)));
								row.Cells.Add( cell );
								table.Rows.Add( row );

								//Carma & Comment & ViewCount
								row = new TableRow();
								cell = new TableCell();
								cell.CssClass = this.CssClass + "_rating";
								Vote.RaitingControl ratingControl = new Vote.RaitingControl();
								ratingControl.ObjectId = article.Id;
								ratingControl.ObjectTypeId = (int)ArticleEntity.AccountVoteType;
								ratingControl.RatingResult = article.RatingResult;
								ratingControl.CssClass = this.CssRating;
								ratingControl.OnVote += ( objectId, rating ) =>
								{
										ArticleEntity.IncrementVoteCommand cmdVote = new ArticleEntity.IncrementVoteCommand();
										cmdVote.AccountId = Security.Account.Id;
										cmdVote.ArticleId = objectId;
										cmdVote.Rating = rating;
										Storage<ArticleEntity>.Execute( cmdVote );

										ArticleEntity comment = Storage<ArticleEntity>.ReadFirst( new ArticleEntity.ReadById { ArticleId = objectId } );
										return comment.RatingResult;
								};
								cell.Controls.Add( ratingControl );
								row.Cells.Add( cell );

								//Comment
								sb = new StringBuilder();
								if ( article.EnableComments && !string.IsNullOrEmpty( commentsFormatUrl ) )
								{
										sb.AppendFormat( "<a href='{1}' class='{2}'>{0} ({3})</a>&nbsp;|&nbsp;",
												Resources.Controls.ArticleControl_CommentsCount,
												container.ResolveUrl( commentsFormatUrl ),
												this.CssClass + "_comment",
												article.CommentsCount );
								}
								sb.AppendFormat( "{0} : <b><span class='{1}'>{2}</span></b> x",
										Resources.Controls.ArticleControl_ViewCount,
										this.CssClass + "_viewCount",
										article.ViewCount );
								cell = new TableCell();
								cell.ColumnSpan = 2;
								cell.Wrap = false;
								cell.HorizontalAlign = HorizontalAlign.Right;
								cell.Controls.Add( new LiteralControl( sb.ToString() ) );
								row.Cells.Add( cell );
								table.Rows.Add( row );

								//Back, Archiv link
								row = new TableRow();

								//Back
								cell = new TableCell();
								cell.Width = Unit.Percentage( 100 );
								cell.ColumnSpan = 2;
								cell.HorizontalAlign = HorizontalAlign.Right;
								cell.VerticalAlign = VerticalAlign.Bottom;
								if ( !string.IsNullOrEmpty( this.returnUrl ) )
										cell.Controls.Add( new LiteralControl( string.Format( "<a href='{0}'>{1}<a>", container.ResolveUrl( this.returnUrl ), CMS.Resources.Controls.BackLink ) ) );
								row.Cells.Add( cell );

								//Archiv
								cell = new TableCell();
								cell.Wrap = false;
								cell.ColumnSpan = 1;
								cell.CssClass = this.CssClass + "_archivLink";
								cell.VerticalAlign = VerticalAlign.Bottom;
								HyperLink link = new HyperLink();
								//link.NavigateUrl = container.ResolveUrl( this.archivUrl );
                                link.NavigateUrl = aliasUtils.Resolve(this.archivUrl, container.Page);
								link.Text = Resources.Controls.ArticleControl_ArticleArchivLink;
								cell.Controls.Add( link );
								row.Cells.Add( cell );
								table.Rows.Add( row );

								div.Controls.Add( table );

								return div;
						}
				}
		}
}
