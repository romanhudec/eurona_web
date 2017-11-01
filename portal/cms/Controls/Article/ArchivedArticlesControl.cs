using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ArticleCategoryEntity = CMS.Entities.Classifiers.ArticleCategory;
using ArticleEntity = CMS.Entities.Article;
using ArticleTagEntity = CMS.Entities.ArticleTag;
using CMS.Utilities;

namespace CMS.Controls.Article
{
		public class ArchivedArticlesControl: ContentEditorCmsControl
		{
				private GridView dataGrid = null;
				public string DisplayUrlFormat { get; set; }
				public string CommentsFormatUrl { get; set; }
				public int? CategoryId
				{
						get
						{
								object o = ViewState["CategoryId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["CategoryId"] = value; }
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

                public bool ShowFilter { get; set; }
				public bool ShowHeader { get; set; }
				public bool ShowFooter { get; set; }
                public int PageSize { get; set; }

				public ArchivedArticlesControl()
				{
						this.ShowHeader = true;
						this.ShowFooter = true;
                        this.ShowFilter = true;
                        this.PageSize = 5;
				}

				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

                        if (this.ShowFilter)
                        {
                            Control filter = CreateNavigationControl();
                            if (filter != null)
                                this.Controls.Add(filter);
                        }

						this.dataGrid = CreateGridControl();
						this.Controls.Add( this.dataGrid );

						//Binding
						this.dataGrid.PagerTemplate = null;
						this.dataGrid.DataSource = GetDataGridData();

						if ( !IsPostBack )
						{
								this.dataGrid.DataKeyNames = new string[] { "Id" };
								this.dataGrid.DataBind();
						}

				}
				#endregion

				[Browsable( false )]
				[DefaultValue( null )]
				[Description( "Article category template." )]
				[TemplateContainer( typeof( DataListItem ) )]
				[PersistenceMode( PersistenceMode.InnerProperty )]
				public virtual ITemplate ArticleCategoryTemplate { get; set; }

				[Browsable( false )]
				[DefaultValue( null )]
				[Description( "Archived articles template." )]
				[TemplateContainer( typeof( GridViewRow ) )]
				[PersistenceMode( PersistenceMode.InnerProperty )]
				public virtual ITemplate ArticleTemplate { get; set; }

				/// <summary>
				/// Vytvorí navigačný control pre filtrovanie článkov
				/// podľa kategórie.
				/// </summary>
				private Control CreateNavigationControl()
				{
						List<ArticleCategoryEntity> categories = Storage<ArticleCategoryEntity>.Read();

						//Select template
						ITemplate template = this.ArticleCategoryTemplate;
						if ( template == null ) template = new DefaultCategoryTemplate( this, this.CategoryId );

						DataList dl = new DataList();
						dl.CssClass = this.CssClass + "_filter";
						dl.CellSpacing = 5;
						dl.RepeatColumns = 3;
						dl.DataSource = categories;
						dl.ItemTemplate = template;
						dl.DataBind();

						return dl;
				}

				private GridView CreateGridControl()
				{
						GridView dg = new GridView();
						dg.EnableViewState = true;
						dg.GridLines = GridLines.None;

						dg.CssClass = CssClass;
						dg.RowStyle.CssClass = CssClass + "_rowStyle";
						dg.FooterStyle.CssClass = CssClass + "_footerStyle";
						dg.PagerStyle.CssClass = CssClass + "_pagerStyle";
						dg.PagerStyle.HorizontalAlign = HorizontalAlign.Center;
						dg.HeaderStyle.CssClass = CssClass + "_headerStyle";
						dg.EditRowStyle.CssClass = CssClass + "_editRowStyle";
						dg.AlternatingRowStyle.CssClass = CssClass + "_alternatingRowStyle";
						dg.ShowHeader = ShowHeader;
						dg.ShowFooter = ShowFooter;

						dg.AllowPaging = true;
						dg.PageSize = this.PageSize;
						dg.PagerSettings.Mode = PagerButtons.NumericFirstLast;
						dg.PagerSettings.PageButtonCount = 10;
						dg.PageIndexChanging += OnPageIndexChanging;

						dg.AutoGenerateColumns = false;

						//Select template
						ITemplate template = this.ArticleTemplate;
						if ( template == null ) template = new DefaultTemplate( this );

						TemplateField tf = new TemplateField();
						tf.ItemTemplate = template;
						tf.ItemStyle.Wrap = true;
						dg.Columns.Add( tf );


						return dg;
				}

				private List<ArticleEntity> GetDataGridData()
				{
						List<ArticleEntity> list = null;
						if ( this.CategoryId.HasValue )
								list = Storage<ArticleEntity>.Read( new ArticleEntity.ReadReleased { CategoryId = this.CategoryId } );
						else
								list = Storage<ArticleEntity>.Read( new ArticleEntity.ReadReleased { TagId = this.TagId } );
						return list;
				}

				#region Event handlers
				/// <summary>
				/// Implementacia strankovania zaznamov v gride.
				/// </summary>
				void OnPageIndexChanging( object sender, GridViewPageEventArgs e )
				{
						this.dataGrid.PageIndex = e.NewPageIndex;

						this.dataGrid.DataSource = GetDataGridData();
						this.dataGrid.DataKeyNames = new string[] { "Id" };
						this.dataGrid.DataBind();
				}

				#endregion

				/// <summary>
				/// Template field na zobrazenie článkov.
				/// </summary>
				private class DefaultCategoryTemplate: ITemplate
				{
						#region Properties
						public string CssClass { get; set; }
						private string navigateUrl = string.Empty;
						private int? articleCategoryId = null;
                        private readonly ArchivedArticlesControl Owner;
                        private readonly AliasUtilities aliasUtils;
						#endregion

						#region ITemplate Members

						public DefaultCategoryTemplate( ArchivedArticlesControl owner, int? articleCategoryId )
						{
								this.CssClass = owner.CssClass + "_filterItem";
                                this.Owner = owner;

								aliasUtils = new AliasUtilities();
								this.navigateUrl = aliasUtils.Resolve( owner.Page.Request.AppRelativeCurrentExecutionFilePath, owner.Page );

								this.articleCategoryId = articleCategoryId;
						}

						public void InstantiateIn( Control container )
						{
								HyperLink hlNavigation = new HyperLink();
								hlNavigation.DataBinding += OnLinkDataBinding;

								container.Controls.Add( hlNavigation );

						}

						void OnLinkDataBinding( object sender, EventArgs e )
						{
								HyperLink control = sender as HyperLink;
								DataListItem item = (DataListItem)control.NamingContainer;
								ArticleCategoryEntity category = ( item.DataItem as ArticleCategoryEntity );

								control.Text = category.Display;
                                control.NavigateUrl = aliasUtils.Resolve(String.Format("{0}?category={1}", /*this.navigateUrl*/Owner.Page.Request.AppRelativeCurrentExecutionFilePath, category.Id), Owner.Page);
								if ( articleCategoryId.HasValue && category.Id == articleCategoryId.Value )
										control.Font.Bold = true;

						}
						#endregion
				}
				/// <summary>
				/// Template field na zobrazenie článkov.
				/// </summary>
				private class DefaultTemplate: ITemplate
				{
						#region Public properties
						public string CssClass { get; set; }
						private string DisplayAliasUrlFormat { get; set; }
						public string DisplayUrlFormat { get; set; }
						public string CommentsFormatUrl { get; set; }
						private string ThisUrl { get; set; }
						private string ReturnUrl { get; set; }
						#endregion

						#region ITemplate Members

						public DefaultTemplate( ArchivedArticlesControl owner )
						{
								this.ReturnUrl = owner.BuildReturnUrlQueryParam();
								this.CssClass = owner.CssClass;
								this.DisplayAliasUrlFormat = "{0}?" + owner.BuildReturnUrlQueryParam();
								this.DisplayUrlFormat = owner.DisplayUrlFormat;
								this.CommentsFormatUrl = owner.CommentsFormatUrl;
								this.ThisUrl = owner.Page.ResolveUrl( owner.Page.Request.AppRelativeCurrentExecutionFilePath );
						}

						public void InstantiateIn( Control container )
						{
								HtmlGenericControl div = new HtmlGenericControl( "div" );
								div.Attributes.Add( "class", this.CssClass + "_articleItem" );

								Table tableArchiv = new Table();
								TableRow row = new TableRow();

								TableCell cell = new TableCell();
								cell.ColumnSpan = 3;
								Label lblHead = new Label();
								lblHead.DataBinding += OnTitleDataBinding;
								lblHead.CssClass = this.CssClass + "_title";
								cell.Controls.Add( lblHead );
								row.Cells.Add( cell );
								tableArchiv.Rows.Add( row );

								row = new TableRow();
								cell = new TableCell();
								Label lblSource = new Label();
								lblSource.CssClass = this.CssClass + "_source";
								lblSource.DataBinding += OnSourceDataBinding;
								cell.Controls.Add( lblSource );
								row.Cells.Add( cell );

								cell = new TableCell();
								cell.HorizontalAlign = HorizontalAlign.Center;
								cell.DataBinding += OnTagsDataBinding;
								row.Cells.Add( cell );

								cell = new TableCell();
								cell.HorizontalAlign = HorizontalAlign.Right;
								Label lblCategory = new Label();
								lblCategory.CssClass = this.CssClass + "_category";
								lblCategory.DataBinding += OnCategoryDataBinding;
								cell.Controls.Add( lblCategory );
								row.Cells.Add( cell );
								tableArchiv.Rows.Add( row );

								row = new TableRow();
								cell = new TableCell();
								cell.ColumnSpan = 3;
								HyperLink hlTeaser = new HyperLink();
								hlTeaser.CssClass = this.CssClass + "_teaser";
								hlTeaser.DataBinding += OnTeaserDataBinding;
								cell.Controls.Add( hlTeaser );
								row.Cells.Add( cell );
								tableArchiv.Rows.Add( row );

								row = new TableRow();
								cell = new TableCell();
								cell.ColumnSpan = 3;
								cell.HorizontalAlign = HorizontalAlign.Right;
								Label lblViewCount = new Label();
								lblViewCount.CssClass = this.CssClass + "_viewCount";
								lblViewCount.DataBinding += OnViewCountDataBinding;
								cell.Controls.Add( lblViewCount );
								row.Cells.Add( cell );
								tableArchiv.Rows.Add( row );

								div.Controls.Add( tableArchiv );
								container.Controls.Add( div );

						}

						void OnTitleDataBinding( object sender, EventArgs e )
						{
								Label control = sender as Label;
								GridViewRow row = (GridViewRow)control.NamingContainer;

								control.Text = ( row.DataItem as ArticleEntity ).Title;
						}

						void OnSourceDataBinding( object sender, EventArgs e )
						{
								Label control = sender as Label;
								GridViewRow row = (GridViewRow)control.NamingContainer;

								ArticleEntity article = ( row.DataItem as ArticleEntity );

								StringBuilder sbDate = new StringBuilder();
								sbDate.Append( article.ReleaseDate.ToShortDateString() );

								if ( !string.IsNullOrEmpty( article.City ) )
										sbDate.AppendFormat( ", {0}", article.City );

								if ( !string.IsNullOrEmpty( article.Country ) )
										sbDate.AppendFormat( ", {0}", article.Country );

								control.Text = sbDate.ToString();
						}

						void OnTagsDataBinding( object sender, EventArgs e )
						{
								TableCell control = sender as TableCell;
								GridViewRow row = (GridViewRow)control.NamingContainer;

								ArticleEntity article = ( row.DataItem as ArticleEntity );

								foreach ( ArticleTagEntity at in article.ArticleTags )
								{
										HyperLink hlTag = new HyperLink();
										hlTag.CssClass = this.CssClass + "_tag";
										hlTag.Text = at.Name;
										hlTag.NavigateUrl = control.Page.ResolveUrl( string.Format( "{0}?tag={1}", this.ThisUrl, at.TagId ) );
										control.Controls.Add( hlTag );
										control.Controls.Add( new LiteralControl( "&nbsp;" ) );
								}
						}

						void OnCategoryDataBinding( object sender, EventArgs e )
						{
								Label control = sender as Label;
								GridViewRow row = (GridViewRow)control.NamingContainer;

								ArticleEntity article = ( row.DataItem as ArticleEntity );
								control.Text = article.ArticleCategoryName;
						}

						void OnTeaserDataBinding( object sender, EventArgs e )
						{
								HyperLink control = sender as HyperLink;
								GridViewRow row = (GridViewRow)control.NamingContainer;
								ArticleEntity article = ( row.DataItem as ArticleEntity );

								control.Text = article.Teaser;

								if ( string.IsNullOrEmpty( article.Alias ) && !string.IsNullOrEmpty( this.DisplayUrlFormat ) )
										control.NavigateUrl = control.Page.ResolveUrl( string.Format( this.DisplayUrlFormat, article.Id ) );
								else control.NavigateUrl = control.Page.ResolveUrl( string.Format( this.DisplayAliasUrlFormat, article.Alias ) );
						}

						void OnViewCountDataBinding( object sender, EventArgs e )
						{
								Label control = sender as Label;
								GridViewRow row = (GridViewRow)control.NamingContainer;

								ArticleEntity article = ( row.DataItem as ArticleEntity );

								StringBuilder sb = new StringBuilder();
								//Comment & ViewCount
								sb = new StringBuilder();
								if ( article.EnableComments && !string.IsNullOrEmpty( this.CommentsFormatUrl ) )
								{
										string commentAlias = string.Format( this.CommentsFormatUrl, article.Id );
										AliasUtilities aliasUtils = new AliasUtilities();
										commentAlias = aliasUtils.Resolve( commentAlias, control.Page );
										commentAlias += ( commentAlias.Contains( "?" ) ? "&" : "?" ) + this.ReturnUrl;

										sb.AppendFormat( "<a href='{1}' class='{2}'>{0} ({3})</a>&nbsp;|&nbsp;",
												Resources.Controls.ArticleControl_CommentsCount,
												commentAlias,
												this.CssClass + "_comment",
												article.CommentsCount );
								}
								sb.AppendFormat( "{0} : <b>{1}</b> x", Resources.Controls.ArticleControl_ViewCount, article.ViewCount );

								control.Text = sb.ToString();
						}

						#endregion
				}
		}
}
