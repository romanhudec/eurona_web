using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Telerik.Web.UI.Calendar.View;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Entities;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

namespace CMS.Controls.SearchEngine
{
		public class SearchEngineResultControl: CmsControl
		{
				private GridView dataGrid = null;
				private Label lblResultHeader = null;

				public string DisplayUrlFormat { get; set; }
				public string SearchKeywords { get; set; }
				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						HtmlGenericControl div = new HtmlGenericControl( "div" );
						div.Attributes.Add( "class", this.CssClass + "_resutHeader" );
						this.lblResultHeader = new Label();
						this.lblResultHeader.CssClass = this.CssClass + "_resutHeader_Text";
						div.Controls.Add( this.lblResultHeader );
						this.Controls.Add( div );
						//this.lblResultText.Id = "";

						this.dataGrid = CreateGridControl();
						this.Controls.Add( this.dataGrid );

						//Binding
						this.dataGrid.PagerTemplate = null;
                        List<SearchEngineBase> list = GetResultData(this.SearchKeywords).OrderByDescending(x => x.Relevance).ToList();
						this.dataGrid.DataSource = list;
						this.lblResultHeader.Text = string.Format( Resources.Controls.SearchEngineResultControl_ResultFormatText, list.Count, this.SearchKeywords );

						if ( !IsPostBack )
						{
								this.dataGrid.DataKeyNames = new string[] { "Id" };
								this.dataGrid.DataBind();
						}

				}
				#endregion

				[Browsable( false )]
				[DefaultValue( null )]
				[Description( "Search template." )]
				[TemplateContainer( typeof( GridViewRow ) )]
				[PersistenceMode( PersistenceMode.InnerProperty )]
				public virtual ITemplate SearchTemplate { get; set; }

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

						dg.AllowPaging = true;
						dg.PageSize = 25;
						dg.PagerSettings.Mode = PagerButtons.NumericFirstLast;
						dg.PagerSettings.PageButtonCount = 10;
						dg.PageIndexChanging += OnPageIndexChanging;

						dg.AutoGenerateColumns = false;

						//Select template
						ITemplate template = this.SearchTemplate;
						if ( template == null ) template = new DefaultTemplate( this );

						TemplateField tf = new TemplateField();
						tf.ItemTemplate = template;
						tf.ItemStyle.Wrap = true;
						dg.Columns.Add( tf );

						return dg;
				}

				protected virtual List<SearchEngineBase> GetResultData( string keywords )
				{
						List<SearchEngineBase> listPages = Storage<SearchEngineBase>.Read( new CmsSearchEngineEntity.SearchPages { Keywords = keywords } );
						List<SearchEngineBase> listArticles = Storage<SearchEngineBase>.Read( new CmsSearchEngineEntity.SearchArticles { Keywords = keywords } );
						List<SearchEngineBase> listBlogs = Storage<SearchEngineBase>.Read( new CmsSearchEngineEntity.SearchBlogs { Keywords = keywords } );
						List<SearchEngineBase> listNews = Storage<SearchEngineBase>.Read( new CmsSearchEngineEntity.SearchNews { Keywords = keywords } );
						List<SearchEngineBase> listImageGalleries = Storage<SearchEngineBase>.Read( new CmsSearchEngineEntity.SearchImageGalleries { Keywords = keywords } );

						List<SearchEngineBase> listForums = Storage<SearchEngineBase>.Read( new CmsSearchEngineEntity.SearchForums { Keywords = keywords } );
						List<SearchEngineBase> listForumPosts = Storage<SearchEngineBase>.Read( new CmsSearchEngineEntity.SearchForumPosts { Keywords = keywords } );
						//Comments
						List<SearchEngineBase> listArticleComments = Storage<SearchEngineBase>.Read( new CmsSearchEngineEntity.SearchArticleComments { Keywords = keywords, CommentAliasPostFix = Resources.Controls.Comment_AliasText } );
						List<SearchEngineBase> listBlogComments = Storage<SearchEngineBase>.Read( new CmsSearchEngineEntity.SearchBlogComments { Keywords = keywords, CommentAliasPostFix = Resources.Controls.Comment_AliasText } );
						List<SearchEngineBase> listIGComments = Storage<SearchEngineBase>.Read( new CmsSearchEngineEntity.SearchImageGalleryComments { Keywords = keywords, CommentAliasPostFix = Resources.Controls.Comment_AliasText } );
						List<SearchEngineBase> listIGIComments = Storage<SearchEngineBase>.Read( new CmsSearchEngineEntity.SearchImageGalleryItemComments { Keywords = keywords, CommentAliasPostFix = Resources.Controls.Comment_AliasText } );

						List<SearchEngineBase> list = new List<SearchEngineBase>();
						list.AddRange( listPages );
						list.AddRange( listArticles );
						list.AddRange( listBlogs );
						list.AddRange( listNews );
						list.AddRange( listImageGalleries );
						list.AddRange( listForums );
						list.AddRange( listForumPosts );

						list.AddRange( listArticleComments );
						list.AddRange( listBlogComments );
						list.AddRange( listIGComments );
						list.AddRange( listIGIComments );

						List<SearchEngineBase> resultList = new List<SearchEngineBase>();
						foreach ( SearchEngineBase se in list )
						{
								//Ak entita nema definovanu rolu
								if ( !se.RoleId.HasValue && string.IsNullOrEmpty( se.RoleString ) )
								{
										resultList.Add( se );
										continue;
								}

								//If is logeg, check role
								if ( Security.IsLogged( false ) )
								{
										foreach ( Entities.Role role in Security.Account.Roles )
										{
												if ( se.RoleString.ToLower().Contains( role.Name.ToLower() ) )
												{
														resultList.Add( se );
														break;
												}

												if ( se.RoleId.HasValue && se.RoleId.Value == role.Id )
												{
														resultList.Add( se );
														break;
												}
										}
								}
						}

						return resultList;
				}

				#region Event handlers
				/// <summary>
				/// Implementacia strankovania zaznamov v gride.
				/// </summary>
				void OnPageIndexChanging( object sender, GridViewPageEventArgs e )
				{
						this.dataGrid.PageIndex = e.NewPageIndex;

                        this.dataGrid.DataSource = GetResultData(this.SearchKeywords).OrderByDescending(x => x.Relevance).ToList();
						this.dataGrid.DataKeyNames = new string[] { "Id" };
						this.dataGrid.DataBind();
				}

				#endregion

				/// <summary>
				/// Template field na zobrazenie noviniek.
				/// </summary>
				private class DefaultTemplate: ITemplate
				{
						#region Public properties
						public string CssClass { get; set; }
						private string DisplayAliasUrlFormat { get; set; }
						public string DisplayUrlFormat { get; set; }
						public string SearchKeywords { get; set; }
						private SearchEngineResultControl owner = null;
						#endregion

						public DefaultTemplate( SearchEngineResultControl owner )
						{
								this.owner = owner;
								this.CssClass = owner.CssClass;
								this.DisplayAliasUrlFormat = "{0}?" + owner.BuildReturnUrlQueryParam();
								this.DisplayUrlFormat = owner.DisplayUrlFormat;
								this.SearchKeywords = owner.SearchKeywords;
						}

						#region ITemplate Members

						public void InstantiateIn( Control container )
						{
								HtmlGenericControl div = new HtmlGenericControl( "div" );
								div.Attributes.Add( "class", this.CssClass + "_resultItem" );

								Table tableArchiv = new Table();
								TableRow row = new TableRow();

								TableCell cell = new TableCell();
								HyperLink hlTitle = new HyperLink();
								hlTitle.DataBinding += OnTitleDataBinding;
								hlTitle.CssClass = this.CssClass + "_title";
								cell.ColumnSpan = 2;
								cell.Controls.Add( hlTitle );
								row.Cells.Add( cell );
								tableArchiv.Rows.Add( row );

								row = new TableRow();
								cell = new TableCell();
								Image img = new Image();
								img.CssClass = this.CssClass + "_image";
								img.DataBinding += OnImageDataBinding;
								cell.Controls.Add( img );
								row.Cells.Add( cell );

								cell = new TableCell();
								Label lblContent = new Label();
								lblContent.CssClass = this.CssClass + "_content";
								lblContent.DataBinding += OnContentDataBinding;
								cell.Width = Unit.Percentage( 100 );
								cell.Controls.Add( lblContent );
								row.Cells.Add( cell );
								tableArchiv.Rows.Add( row );

								row = new TableRow();
								cell = new TableCell();
								HyperLink hlUrl = new HyperLink();
								hlUrl.CssClass = this.CssClass + "_url";
								hlUrl.DataBinding += OnUrlDataBinding;
								cell.ColumnSpan = 2;
								cell.Width = Unit.Percentage( 100 );
								cell.Controls.Add( hlUrl );
								row.Cells.Add( cell );
								tableArchiv.Rows.Add( row );

								div.Controls.Add( tableArchiv );
								container.Controls.Add( div );
						}

						void OnTitleDataBinding( object sender, EventArgs e )
						{
								HyperLink control = sender as HyperLink;
								GridViewRow row = (GridViewRow)control.NamingContainer;
								SearchEngineBase searchEntity = ( row.DataItem as SearchEngineBase );
								control.Text = Highlight( this.SearchKeywords, searchEntity.Title );
								if ( !string.IsNullOrEmpty( searchEntity.UrlAlias ) )
								{
										if ( searchEntity.UrlAlias.Contains( "#" ) )
												control.NavigateUrl = control.Page.ResolveUrl( searchEntity.UrlAlias );
										else
										{
												string andOr = searchEntity.UrlAlias.Contains( "?" ) ? "&" : "?";
												control.NavigateUrl = control.Page.ResolveUrl( searchEntity.UrlAlias + andOr + owner.BuildReturnUrlQueryParam() );
										}
								}
						}

						void OnImageDataBinding( object sender, EventArgs e )
						{
								Image control = sender as Image;
								GridViewRow row = (GridViewRow)control.NamingContainer;
								SearchEngineBase searchEntity = ( row.DataItem as SearchEngineBase );
								if ( string.IsNullOrEmpty( searchEntity.ImageUrl ) )
								{
										control.Visible = false;
										return;
								}

								string url = searchEntity.ImageUrl.StartsWith( "~" ) ? control.Page.ResolveUrl( searchEntity.ImageUrl ) : searchEntity.ImageUrl;
								control.ImageUrl = url;
						}

						void OnContentDataBinding( object sender, EventArgs e )
						{
								Label control = sender as Label;
								GridViewRow row = (GridViewRow)control.NamingContainer;
								SearchEngineBase searchEntity = ( row.DataItem as SearchEngineBase );
								control.Text = Highlight( this.SearchKeywords, searchEntity.Content );
						}

						void OnUrlDataBinding( object sender, EventArgs e )
						{
								HyperLink control = sender as HyperLink;
								GridViewRow row = (GridViewRow)control.NamingContainer;
								SearchEngineBase searchEntity = ( row.DataItem as SearchEngineBase );
								if ( string.IsNullOrEmpty( searchEntity.UrlAlias ) )
								{
										control.Visible = false;
										return;
								}
								control.Text = control.Page.ResolveUrl( searchEntity.UrlAlias );
								if ( searchEntity.UrlAlias.Contains( "#" ) )
								{
										string text = Regex.Replace( searchEntity.UrlAlias, "(#.*)", string.Empty );
										control.Text = text;
										control.NavigateUrl = control.Page.ResolveUrl( searchEntity.UrlAlias );
								}
								else
								{
										string andOr = searchEntity.UrlAlias.Contains( "?" ) ? "&" : "?";
										control.NavigateUrl = control.Page.ResolveUrl( searchEntity.UrlAlias + andOr + owner.BuildReturnUrlQueryParam() );
								}
										
						}

						private string Highlight( string keywords, string text )
						{
								const int max_chars = 255;
								// Swap out the ,<space> for pipes and add the braces
								Regex r = new Regex( @", ?" );
								keywords = "(" + r.Replace( keywords, @"|" ) + ")";
								r = new Regex( keywords, RegexOptions.Singleline | RegexOptions.IgnoreCase );

								text = text.Replace( '\n', ' ' );
								if ( text.Length <= max_chars )
										return r.Replace( text, new MatchEvaluator( ReplaceKeywords ) );

								string result = string.Empty;
								//Split koniec riadku alebo celej vety.
								string[] lines = Regex.Split( text, @"[.]" );
								foreach ( string line in lines )
								{
										if ( line.Length == 0 ) continue;

										Match m = r.Match( line );
										if ( m.Success ) result += line + ". ";

										if ( result.Length < max_chars ) continue;

										//koniec
										result += " ...";
										break;
								}

								// samotny replace
								result = r.Replace( result, new MatchEvaluator( ReplaceKeywords ) );
								return result;
						}

						private string ReplaceKeywords( Match m )
						{
								if ( m.Groups[1].Success )
										return string.Format( "<span class='highlight'>{0}</span>", m.Value );
								return string.Empty;
						}

						#endregion
				}
		}
}
