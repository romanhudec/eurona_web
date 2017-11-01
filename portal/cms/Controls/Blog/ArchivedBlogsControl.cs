using System;
using System.Collections.Generic;
using System.ComponentModel;
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
		public class ArchivedBlogsControl: CmsControl
		{
				private GridView dataGrid = null;
				public string DisplayUrlFormat { get; set; }
				public string CommentsFormatUrl { get; set; }
				public string AccountProfileFormatUrl { get; set; }
				public bool ShowHeader { get; set; }
				public bool ShowFooter { get; set; }

				/// <summary>
				/// Nastavi názov položky z profilu používateľa, ktorá sa má zobrazovať ako autorblogu.
				/// </summary>
				public string AccountProfileItemName { get; set; }

				public int? AccountId
				{
						get
						{
								object o = ViewState["AccountId"];
								return o != null ? (int?)Convert.ToInt32( o ) : null;
						}
						set { ViewState["AccountId"] = value; }
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

				public ArchivedBlogsControl()
				{
						this.ShowHeader = true;
						this.ShowFooter = true;
				}

				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

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
				[Description( "Archived blogs template." )]
				[TemplateContainer( typeof( GridViewRow ) )]
				[PersistenceMode( PersistenceMode.InnerProperty )]
				public virtual ITemplate BlogTemplate { get; set; }


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
						dg.ShowHeader = this.ShowHeader;
						dg.ShowFooter = this.ShowFooter;

						dg.AllowPaging = true;
						dg.PageSize = 5;
						dg.PagerSettings.Mode = PagerButtons.NumericFirstLast;
						dg.PagerSettings.PageButtonCount = 10;
						dg.PageIndexChanging += OnPageIndexChanging;

						dg.AutoGenerateColumns = false;

						//Select template
						ITemplate template = this.BlogTemplate;
						if ( template == null ) template = new DefaultTemplate( this );

						TemplateField tf = new TemplateField();
						tf.ItemTemplate = template;
						tf.ItemStyle.Wrap = true;
						dg.Columns.Add( tf );

						return dg;
				}

				private List<BlogEntity> GetDataGridData()
				{
						List<BlogEntity> list = null;
						if ( this.AccountId.HasValue )
								list = Storage<BlogEntity>.Read( new BlogEntity.ReadReleased { AccountId = this.AccountId } );
						else
								list = Storage<BlogEntity>.Read( new BlogEntity.ReadReleased { TagId = this.TagId } );
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
				private class DefaultTemplate: ITemplate
				{
						#region Public properties
						public string CssClass { get; set; }
						private string DisplayAliasUrlFormat { get; set; }
						public string DisplayUrlFormat { get; set; }
						public string AccountProfileItemName { get; set; }
						public string AccountProfileFormatUrl { get; set; }
						public string CommentsFormatUrl { get; set; }
						private string ThisUrl { get; set; }
						private string ReturnUrl { get; set; }
						#endregion

						#region ITemplate Members

						public DefaultTemplate( ArchivedBlogsControl owner )
						{
								this.ReturnUrl = owner.BuildReturnUrlQueryParam();
								this.CssClass = owner.CssClass;
								this.DisplayAliasUrlFormat = "{0}?" + owner.BuildReturnUrlQueryParam();
								this.DisplayUrlFormat = owner.DisplayUrlFormat;
								this.CommentsFormatUrl = owner.CommentsFormatUrl;

								AliasUtilities aliasUtils = new AliasUtilities();
								this.ThisUrl = aliasUtils.Resolve( owner.Page.Request.AppRelativeCurrentExecutionFilePath, owner.Page );
							
								this.AccountProfileItemName = owner.AccountProfileItemName;
								this.AccountProfileFormatUrl = owner.AccountProfileFormatUrl;
						}

						public void InstantiateIn( Control container )
						{
								HtmlGenericControl div = new HtmlGenericControl( "div" );
								div.Attributes.Add( "class", this.CssClass + "_blogItem" );

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
								HyperLink hlUser = new HyperLink();
								hlUser.CssClass = this.CssClass + "_user";
								hlUser.DataBinding += OnUserDataBinding;
								cell.Controls.Add( hlUser );
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

								control.Text = ( row.DataItem as BlogEntity ).Title;
						}

						void OnSourceDataBinding( object sender, EventArgs e )
						{
								Label control = sender as Label;
								GridViewRow row = (GridViewRow)control.NamingContainer;

								BlogEntity blog = ( row.DataItem as BlogEntity );

								StringBuilder sbDate = new StringBuilder();
								sbDate.Append( blog.ReleaseDate.ToShortDateString() );

								if ( !string.IsNullOrEmpty( blog.City ) )
										sbDate.AppendFormat( ", {0}", blog.City );

								if ( !string.IsNullOrEmpty( blog.Country ) )
										sbDate.AppendFormat( ", {0}", blog.Country );

								control.Text = sbDate.ToString();
						}

						void OnTagsDataBinding( object sender, EventArgs e )
						{
								TableCell control = sender as TableCell;
								GridViewRow row = (GridViewRow)control.NamingContainer;

								BlogEntity blog = ( row.DataItem as BlogEntity );

								foreach ( BlogTagEntity at in blog.BlogTags )
								{
										HyperLink hlTag = new HyperLink();
										hlTag.CssClass = this.CssClass + "_tag";
										hlTag.Text = at.Name;
										hlTag.NavigateUrl = control.Page.ResolveUrl( string.Format( "{0}?tag={1}", this.ThisUrl, at.TagId ) );
										control.Controls.Add( hlTag );
										control.Controls.Add( new LiteralControl( "&nbsp;" ) );
								}
						}

						void OnUserDataBinding( object sender, EventArgs e )
						{
								HyperLink control = sender as HyperLink;
								GridViewRow row = (GridViewRow)control.NamingContainer;

								BlogEntity blog = ( row.DataItem as BlogEntity );
								AccountProfileEntity ap = Storage<AccountProfileEntity>.ReadFirst( new AccountProfileEntity.ReadByAccountAndProfileName { AccountId = blog.AccountId, ProfileName = this.AccountProfileItemName } );
								control.Text = ( ap != null && ap.Value != string.Empty ) ? ap.Value : blog.UserName;

								if ( !string.IsNullOrEmpty( this.AccountProfileFormatUrl ) )
										control.NavigateUrl = control.Page.ResolveUrl( string.Format( this.AccountProfileFormatUrl, blog.AccountId ) );
						}

						void OnTeaserDataBinding( object sender, EventArgs e )
						{
								HyperLink control = sender as HyperLink;
								GridViewRow row = (GridViewRow)control.NamingContainer;
								BlogEntity blog = ( row.DataItem as BlogEntity );

								control.Text = blog.Teaser;

								if ( string.IsNullOrEmpty( blog.Alias ) && !string.IsNullOrEmpty( this.DisplayUrlFormat ) )
										control.NavigateUrl = control.Page.ResolveUrl( string.Format( this.DisplayUrlFormat, blog.Id ) );
								else control.NavigateUrl = control.Page.ResolveUrl( string.Format( this.DisplayAliasUrlFormat, blog.Alias ) );
						}

						void OnViewCountDataBinding( object sender, EventArgs e )
						{
								Label control = sender as Label;
								GridViewRow row = (GridViewRow)control.NamingContainer;

								BlogEntity blog = ( row.DataItem as BlogEntity );

								StringBuilder sb = new StringBuilder();
								//Comment & ViewCount
								sb = new StringBuilder();
								if ( blog.EnableComments && !string.IsNullOrEmpty( this.CommentsFormatUrl ) )
								{
										string commentAlias = string.Format( this.CommentsFormatUrl, blog.Id );

										AliasUtilities aliasUtils = new AliasUtilities();
										commentAlias = aliasUtils.Resolve( commentAlias, control.Page );
										commentAlias += ( commentAlias.Contains( "?" ) ? "&" : "?" ) + this.ReturnUrl;

										sb.AppendFormat( "<a href='{1}' class='{2}'>{0} ({3})</a>&nbsp;|&nbsp;",
												Resources.Controls.ArticleControl_CommentsCount,
												commentAlias,
												this.CssClass + "_comment",
												blog.CommentsCount );
								}
								sb.AppendFormat( "{0} : <b>{1}</b> x", Resources.Controls.BlogControl_ViewCount, blog.ViewCount );

								control.Text = sb.ToString();
						}

						#endregion
				}
		}
}
