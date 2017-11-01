using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using NewsEntity = CMS.Entities.News;
using System.Web.UI;
using System.Data;
using System.ComponentModel;
using System.Web.UI.HtmlControls;

namespace CMS.Controls.News
{
		public class ArchivedNewsControl: ContentEditorCmsControl
		{
				private GridView dataGrid = null;

				public string DisplayUrlFormat { get; set; }

                public int PageSize { get; set; }

                public ArchivedNewsControl()
                {
                    this.PageSize = 5;
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
				[Description( "Archived articles template." )]
				[TemplateContainer( typeof( GridViewRow ) )]
				[PersistenceMode( PersistenceMode.InnerProperty )]
				public virtual ITemplate NewsTemplate { get; set; }

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
						dg.PageSize = this.PageSize;
						dg.PagerSettings.Mode = PagerButtons.NumericFirstLast;
						dg.PagerSettings.PageButtonCount = 10;
						dg.PageIndexChanging += OnPageIndexChanging;

						dg.AutoGenerateColumns = false;

						//Select template
						ITemplate template = this.NewsTemplate;
						if ( template == null ) template = new DefaultTemplate( this );

						TemplateField tf = new TemplateField();
						tf.ItemTemplate = template;
						tf.ItemStyle.Wrap = true;
						dg.Columns.Add( tf );

						return dg;
				}

				private List<NewsEntity> GetDataGridData()
				{
						List<NewsEntity> list = Storage<NewsEntity>.Read();
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
				/// Template field na zobrazenie noviniek.
				/// </summary>
				private class DefaultTemplate: ITemplate
				{
						#region Public properties
						public string CssClass { get; set; }
						private string DisplayAliasUrlFormat { get; set; }
						public string DisplayUrlFormat { get; set; }
						#endregion

						public DefaultTemplate( ArchivedNewsControl owner )
						{
								this.CssClass = owner.CssClass;
								this.DisplayAliasUrlFormat = "{0}?" + owner.BuildReturnUrlQueryParam();
								this.DisplayUrlFormat = owner.DisplayUrlFormat;
						}

						#region ITemplate Members

						public void InstantiateIn( Control container )
						{
								HtmlGenericControl div = new HtmlGenericControl( "div" );
								div.Attributes.Add( "class", this.CssClass + "_articleItem" );

								Table tableArchiv = new Table();
								TableRow row = new TableRow();

								TableCell cell = new TableCell();
								cell.ColumnSpan = 2;
								Label lblHead = new Label();
								lblHead.DataBinding += OnHeadDataBinding;
								lblHead.CssClass = this.CssClass + "_title";
								cell.Controls.Add( lblHead );
								row.Cells.Add( cell );
								tableArchiv.Rows.Add( row );

								row = new TableRow();
								cell = new TableCell();
								cell.ColumnSpan = 2;
								Label lblDate = new Label();
								lblDate.CssClass = this.CssClass + "_date";
								lblDate.DataBinding += OnDateDataBinding;
								cell.Controls.Add( lblDate );
								row.Cells.Add( cell );
								tableArchiv.Rows.Add( row );

								row = new TableRow();
								cell = new TableCell();
								HtmlGenericControl divIcon = new HtmlGenericControl( "div" );
								divIcon.Attributes.Add( "class", this.CssClass + "_icon" );
								divIcon.DataBinding += OnIconDataBinding;
								cell.Controls.Add( divIcon );
								row.Cells.Add( cell );
								tableArchiv.Rows.Add( row );

								cell = new TableCell();
								HyperLink hlDescirption = new HyperLink();
								hlDescirption.CssClass = this.CssClass + "_description";
								hlDescirption.DataBinding += OnDescriptionDataBinding;
								cell.Controls.Add( hlDescirption );
								row.Cells.Add( cell );
								tableArchiv.Rows.Add( row );

								div.Controls.Add( tableArchiv );
								container.Controls.Add( div );
						}

						void OnHeadDataBinding( object sender, EventArgs e )
						{
								Label control = sender as Label;
								GridViewRow row = (GridViewRow)control.NamingContainer;

								control.Text = ( row.DataItem as NewsEntity ).Title;
						}

						void OnDateDataBinding( object sender, EventArgs e )
						{
								Label control = sender as Label;
								GridViewRow row = (GridViewRow)control.NamingContainer;

								control.Text = ( row.DataItem as NewsEntity ).Date.Value.ToShortDateString();
						}
						void OnIconDataBinding( object sender, EventArgs e )
						{
								HtmlGenericControl control = sender as HtmlGenericControl;
								GridViewRow row = (GridViewRow)control.NamingContainer;
								NewsEntity news = ( row.DataItem as NewsEntity );
								
								if ( !string.IsNullOrEmpty( news.Icon ) )
								{
										Image img = new Image();
										img.ImageUrl = control.Page.ResolveUrl( news.Icon );
										control.Controls.Add( img );
								}
						}
						void OnDescriptionDataBinding( object sender, EventArgs e )
						{
								HyperLink control = sender as HyperLink;
								GridViewRow row = (GridViewRow)control.NamingContainer;
								NewsEntity news = ( row.DataItem as NewsEntity );
								control.Text = news.Teaser;

								if ( string.IsNullOrEmpty( news.Alias ) && !string.IsNullOrEmpty( this.DisplayUrlFormat ) )
										control.NavigateUrl = control.Page.ResolveUrl( string.Format( this.DisplayUrlFormat, news.Id ) );
								else control.NavigateUrl = control.Page.ResolveUrl( string.Format( this.DisplayAliasUrlFormat, news.Alias ) );
						}

						#endregion
				}
		}
}
