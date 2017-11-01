using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using ForumThreadEntity = CMS.Entities.ForumThread;
using System.Web.UI;
using System.Data;
using System.ComponentModel;
using System.Web.UI.HtmlControls;

namespace CMS.Controls.Forum
{
		public class ForumThreadsControl: CmsControl
		{
				[DefaultValue( 5 )]
				public int PagerSize { get; set; }
				[DefaultValue( true )]
				public bool ShowForumHeader { get; set; }
				public string RSSFormatUrl { get; set; }

				private GridView dataGrid = null;

				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();
						if ( this.ShowForumHeader )
						{
								Control forulHeaderControl = CreateForumHeaderControl();
								this.Controls.Add( forulHeaderControl );
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

				private Control CreateForumHeaderControl()
				{
						HtmlGenericControl div = new HtmlGenericControl( "div" );
						div.Attributes.Add( "class", this.CssClass + "_forumHeader" );

						return div;
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

						dg.AllowPaging = true;
						dg.PageSize = this.PagerSize;
						dg.PagerSettings.Mode = PagerButtons.NumericFirstLast;
						dg.PagerSettings.PageButtonCount = 10;
						dg.PageIndexChanging += OnPageIndexChanging;

						dg.AutoGenerateColumns = false;

						//Icon
						TemplateField tf = new TemplateField();
						IconTemplate iconTemplate = new IconTemplate();
						iconTemplate.CssClass = this.CssClass;
						tf.ItemTemplate = iconTemplate;
						tf.ItemStyle.Wrap = false;
						dg.Columns.Add( tf );

						//Name
						tf = new TemplateField();
						NameTemplate nameTemplate = new NameTemplate();
						nameTemplate.CssClass = this.CssClass;
						tf.ItemTemplate = nameTemplate;
						tf.ItemStyle.Wrap = true;
						tf.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
						tf.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
						tf.HeaderText = Resources.Controls.ForumThreadsControl_ColumnName;
						dg.Columns.Add( tf );

						//RSS
						if ( !string.IsNullOrEmpty( this.RSSFormatUrl ) )
						{
								tf = new TemplateField();
								RSSTemplate rssTemplate = new RSSTemplate( this.RSSFormatUrl );
								rssTemplate.CssClass = this.CssClass;
								tf.ItemTemplate = rssTemplate;
								tf.ItemStyle.Wrap = false;
								tf.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
								tf.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
								dg.Columns.Add( tf );
						}

						//Locked
						tf = new TemplateField();
						LockedTemplate lockedTemplate = new LockedTemplate();
						lockedTemplate.CssClass = this.CssClass;
						tf.ItemTemplate = lockedTemplate;
						tf.ItemStyle.Wrap = true;
						tf.HeaderText = Resources.Controls.ForumThreadsControl_ColumnStatus;
						tf.ItemStyle.HorizontalAlign = HorizontalAlign.Left;
						tf.HeaderStyle.HorizontalAlign = HorizontalAlign.Left;
						dg.Columns.Add( tf );

						BoundField bf = new BoundField();
						bf.DataField = "ForumsCount";
						bf.HeaderText = Resources.Controls.ForumThreadsControl_ColumnForumsCount;
						bf.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
						bf.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
						dg.Columns.Add( bf );

						bf = new BoundField();
						bf.DataField = "ForumPostCount";
						bf.HeaderText = Resources.Controls.ForumThreadsControl_ColumnForumPostCount;
						bf.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
						bf.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
						dg.Columns.Add( bf );

						return dg;
				}

				private List<ForumThreadEntity> GetDataGridData()
				{
						List<ForumThreadEntity> list = Storage<ForumThreadEntity>.Read();
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
				/// Template field na zobrazenie RSS.
				/// </summary>
				private class RSSTemplate: ITemplate
				{
						#region Public properties
						public string CssClass { get; set; }
						public string RSSFormatUrl { get; set; }
						#endregion

						public RSSTemplate( string rssFormatUrl )
						{
								this.RSSFormatUrl = rssFormatUrl;
						}
						#region ITemplate Members

						public void InstantiateIn( Control container )
						{
								HtmlGenericControl div = new HtmlGenericControl( "div" );
								div.Attributes.Add( "class", this.CssClass + "_rss" );

								HyperLink hlRSS = new HyperLink();
								hlRSS.DataBinding += new EventHandler( OnRSSDataBinding );

								hlRSS.Controls.Add( div );
								container.Controls.Add( hlRSS );

						}

						void OnRSSDataBinding( object sender, EventArgs e )
						{
								HyperLink control = sender as HyperLink;
								GridViewRow row = (GridViewRow)control.NamingContainer;

								string url = string.Format( this.RSSFormatUrl, ( row.DataItem as ForumThreadEntity ).Id, ( row.DataItem as ForumThreadEntity ).Name );
								control.NavigateUrl = control.Page.ResolveUrl( url );
						}

						#endregion
				}
				/// <summary>
				/// Template field na zobrazenie Image.
				/// </summary>
				private class IconTemplate: ITemplate
				{
						#region Public properties
						public string CssClass { get; set; }
						#endregion

						#region ITemplate Members

						public void InstantiateIn( Control container )
						{
								HtmlGenericControl div = new HtmlGenericControl( "div" );
								div.Attributes.Add( "class", this.CssClass + "_icon" );

								Image image = new Image();
								image.DataBinding += new EventHandler( image_DataBinding );

								HtmlGenericControl a = new HtmlGenericControl( "a" );
								a.DataBinding += new EventHandler( hlImage_DataBinding );
								a.Controls.Add( image );

								div.Controls.Add( a );
								container.Controls.Add( div );

						}

						void hlImage_DataBinding( object sender, EventArgs e )
						{
								HtmlGenericControl control = sender as HtmlGenericControl;
								GridViewRow row = (GridViewRow)control.NamingContainer;

								control.Attributes.Add( "href", control.Page.ResolveUrl( ( row.DataItem as ForumThreadEntity ).Alias ) );
						}

						void image_DataBinding( object sender, EventArgs e )
						{
								Image control = sender as Image;
								control.ImageAlign = ImageAlign.Middle;
								GridViewRow row = (GridViewRow)control.NamingContainer;

								string icon = ( row.DataItem as ForumThreadEntity ).Icon;
								if ( string.IsNullOrEmpty( icon ) ) control.Visible = false;
								else control.ImageUrl = control.Page.ResolveUrl( icon );
						}

						#endregion
				}
				/// <summary>
				/// Template field na zobrazenie Nazvu.
				/// </summary>
				private class NameTemplate: ITemplate
				{
						#region Public properties
						public string CssClass { get; set; }
						#endregion

						#region ITemplate Members

						public void InstantiateIn( Control container )
						{
								Table table = new Table();
								table.CssClass = this.CssClass + "_nameItem";

								TableRow row = new TableRow();

								TableCell cell = new TableCell();
								cell.Wrap = true;
								HyperLink lblName = new HyperLink();
								lblName.DataBinding += OnNameDataBinding;
								lblName.CssClass = this.CssClass + "_nameItem_name";
								cell.Controls.Add( lblName );
								row.Cells.Add( cell );
								table.Rows.Add( row );

								row = new TableRow();
								cell = new TableCell();
								cell.Wrap = true;
								Label lblDescription = new Label();
								lblDescription.CssClass = this.CssClass + "_nameItem_description";
								lblDescription.DataBinding += OnDescriptionDataBinding;
								cell.Controls.Add( lblDescription );
								row.Cells.Add( cell );
								table.Rows.Add( row );

								container.Controls.Add( table );

						}

						void OnNameDataBinding( object sender, EventArgs e )
						{
								HyperLink control = sender as HyperLink;
								GridViewRow row = (GridViewRow)control.NamingContainer;

								control.Text = ( row.DataItem as ForumThreadEntity ).Name;
								control.NavigateUrl = control.Page.ResolveUrl( ( row.DataItem as ForumThreadEntity ).Alias );
						}

						void OnDescriptionDataBinding( object sender, EventArgs e )
						{
								Label control = sender as Label;
								GridViewRow row = (GridViewRow)control.NamingContainer;

								control.Text = ( row.DataItem as ForumThreadEntity ).Description;
						}

						#endregion
				}

				/// <summary>
				/// Template field na zobrazenie Uzamknutia.
				/// </summary>
				private class LockedTemplate: ITemplate
				{
						#region Public properties
						public string CssClass { get; set; }
						#endregion

						#region ITemplate Members

						public void InstantiateIn( Control container )
						{
								HtmlGenericControl div = new HtmlGenericControl( "div" );
								div.DataBinding += new EventHandler( div_DataBinding );

								container.Controls.Add( div );

						}

						void div_DataBinding( object sender, EventArgs e )
						{
								HtmlGenericControl control = sender as HtmlGenericControl;
								GridViewRow row = (GridViewRow)control.NamingContainer;

								if ( ( row.DataItem as ForumThreadEntity ).Locked )
										control.Attributes.Add( "class", this.CssClass + "_itemLocked" );
								else
										control.Attributes.Add( "class", this.CssClass + "_itemUnLocked" );
						}
						#endregion
				}
		}
}
