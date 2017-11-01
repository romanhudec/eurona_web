using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using ForumEntity = CMS.Entities.Forum;
using ForumThreadEntity = CMS.Entities.ForumThread;
using System.Web.UI;
using System.Data;
using System.ComponentModel;
using System.Web.UI.HtmlControls;

namespace CMS.Controls.Forum
{
		public class ForumsControl: CmsControl
		{
				private const string LOCK_COMMAND = "LOCK_ITEM";
				private const string UNLOCK_COMMAND = "UNLOCK_ITEM";

				[DefaultValue( 5 )]
				public int PagerSize { get; set; }
				[DefaultValue( true )]
				public bool ShowForumHeader { get; set; }
				/// <summary>
				/// Maximalna dlzka popisneho textu, ktory sa zobrazi v zozname.
				/// </summary>
				public int MaxDescriptionLength { get; set; }

				/// <summary>
				/// Ak je property null, komponenta pracuje v rezime New.
				/// </summary>
				public int ForumThreadId
				{
						get
						{
								object o = ViewState["ForumThreadId"];
								return o != null ? Convert.ToInt32( o ) : 0;
						}
						set { ViewState["ForumThreadId"] = value; }
				}

				/// <summary>
				/// URL na "Obsah Fora" (ForumThreads)
				/// </summary>
				public string ForumContentUrl { get; set; }
				/// <summary>
				/// Url na pre vytvorenie noveho fora.
				/// </summary>
				public string NewUrl { get; set; }
				/// <summary>
				/// URL na RSS
				/// </summary>
				public string RSSFormatUrl { get; set; }

				/// <summary>
				/// Rola Moderator alebo Administrator pre pokrocilejsie operacie.
				/// </summary>
				public string AdvancedCommandsRole { get; set; }

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

				public bool IsEditableForRole( ForumThreadEntity ft )
				{
						if ( string.IsNullOrEmpty( ft.EditableForRole ) ) return true;
						else
						{
								if ( !Security.IsLogged( false ) ) return false;
								if ( Security.IsInRole( CMS.Entities.Role.ADMINISTRATOR ) ) return true;

								foreach ( string role in Security.Account.RoleArray )
								{
										if ( ft.EditableForRole.Contains( role ) )
												return true;
								}

								return false;
						}
				}

				public bool IsVisibleForRole( ForumThreadEntity ft )
				{
						if ( string.IsNullOrEmpty( ft.VisibleForRole ) ) return true;
						else
						{
								if ( !Security.IsLogged( false ) ) return false;
								if ( Security.IsInRole( CMS.Entities.Role.ADMINISTRATOR ) ) return true;

								foreach ( string role in Security.Account.RoleArray )
								{
										if ( ft.VisibleForRole.Contains( role ) )
												return true;
								}

								return false;
						}
				}

				/// <summary>
				/// Filter for forums control
				/// </summary>
				public ForumEntity.ReadBy Filter { get; set; }

				private ForumThreadEntity forumThreadEntity = null;
				public ForumThreadEntity ForumThreadEntity
				{
						get
						{
								if ( forumThreadEntity != null ) return forumThreadEntity;
								forumThreadEntity = Storage<ForumThreadEntity>.ReadFirst( new ForumThreadEntity.ReadById { ForumThreadId = this.ForumThreadId } );
								return forumThreadEntity;
						}
				}

				protected virtual Control CreateForumHeaderControl()
				{
						ForumThreadEntity ft = ForumThreadEntity; // oxy: Storage<ForumThreadEntity>.ReadFirst(new ForumThreadEntity.ReadById { ForumThreadId = this.ForumThreadId });
						HtmlGenericControl div = new HtmlGenericControl( "div" );
						div.Attributes.Add( "class", this.CssClass + "_forumHeader" );

						Table table = new Table();
						TableRow row = new TableRow(); table.Rows.Add( row );

						Utilities.AliasUtilities aliasUtils = new Utilities.AliasUtilities();
						string alias = aliasUtils.Resolve( this.ForumContentUrl, this.Page );
						HyperLink hlObsahFora = new HyperLink();
						hlObsahFora.Text = Resources.Controls.ForumsControl_ColumnForumContent;
						hlObsahFora.NavigateUrl = alias;
						TableCell cell = new TableCell();
						cell.CssClass = this.CssClass + "_forumHeader_forumContentLink";
						cell.Wrap = false;
						cell.Controls.Add( hlObsahFora );
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.CssClass = this.CssClass + "_forumHeader_linkSeparator";
						cell.Wrap = false;
						cell.Controls.Add( new LiteralControl( "»" ) );
						row.Cells.Add( cell );

						if ( ft != null )
						{
								HyperLink hlForumThread = new HyperLink();
								hlForumThread.Text = ft.Name;
								hlForumThread.NavigateUrl = Page.ResolveUrl( ft.Alias );
								cell = new TableCell();
								cell.CssClass = this.CssClass + "_forumHeader_link";
								cell.Wrap = false;
								cell.Controls.Add( hlForumThread );
								row.Cells.Add( cell );

								if ( ft.Locked == false && Security.IsLogged( false ) )
								{
										if ( IsEditableForRole( ft ) )
										{
												Button btnNewForum = new Button();
												btnNewForum.Text = Resources.Controls.AdminForumsControl_NewForum;
												cell = new TableCell();
												cell.Width = Unit.Percentage( 100 );
												cell.HorizontalAlign = HorizontalAlign.Right;
												cell.CssClass = this.CssClass + "_forumHeader_newForum";
												cell.Controls.Add( btnNewForum );
												row.Cells.Add( cell );

												btnNewForum.Click += ( s, e ) =>
												{
														string url = Page.ResolveUrl( string.Format( "{0}&{1}", string.Format( this.NewUrl, this.ForumThreadId ), base.BuildReturnUrlQueryParam() ) );
														Response.Redirect( url );
												};
										}
								}
						}

						div.Controls.Add( table );
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
						nameTemplate.MaxDescriptionLength = this.MaxDescriptionLength;
						tf.ItemTemplate = nameTemplate;
						tf.ItemStyle.Wrap = true;
						tf.HeaderText = Resources.Controls.ForumsControl_ColumnName;
						dg.Columns.Add( tf );

						//RSS
						if ( !string.IsNullOrEmpty( this.RSSFormatUrl ) )
						{
								tf = new TemplateField();
								RSSTemplate rssTemplate = new RSSTemplate( this.RSSFormatUrl );
								rssTemplate.CssClass = this.CssClass;
								tf.ItemTemplate = rssTemplate;
								tf.ItemStyle.Wrap = false;
								dg.Columns.Add( tf );
						}

						//Pinned
						tf = new TemplateField();
						PinnedTemplate pinnedTemplate = new PinnedTemplate();
						pinnedTemplate.CssClass = this.CssClass;
						tf.ItemTemplate = pinnedTemplate;
						tf.ItemStyle.Wrap = false;
						//tf.HeaderText = Resources.Controls.ForumsControl_ColumnPinned;
						dg.Columns.Add( tf );

						//Status
						tf = new TemplateField();
						LockedTemplate lockedTemplate = new LockedTemplate();
						lockedTemplate.CssClass = this.CssClass;
						tf.ItemTemplate = lockedTemplate;
						tf.ItemStyle.Wrap = false;
						tf.HeaderText = Resources.Controls.ForumsControl_ColumnStatus;
						dg.Columns.Add( tf );


						BoundField bf = new BoundField();
						bf.DataField = "ForumPostCount";
						bf.HeaderText = Resources.Controls.ForumThreadsControl_ColumnForumPostCount;
						bf.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
						bf.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
						dg.Columns.Add( bf );

						bf = new BoundField();
						bf.DataField = "ViewCount";
						bf.HeaderText = Resources.Controls.ForumsControl_ColumnViewCount;
						bf.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
						bf.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
						dg.Columns.Add( bf );

						//LastPost
						tf = new TemplateField();
						LastPostTemplate lastPostTemplate = new LastPostTemplate();
						lastPostTemplate.CssClass = this.CssClass;
						tf.ItemTemplate = lastPostTemplate;
						tf.ItemStyle.Wrap = true;
						tf.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
						tf.HeaderStyle.HorizontalAlign = HorizontalAlign.Right;
						tf.HeaderText = Resources.Controls.ForumsControl_ColumnLastPostDate;
						dg.Columns.Add( tf );

						if ( Security.IsLogged( false ) && !String.IsNullOrEmpty( AdvancedCommandsRole ) && Security.IsInRole( AdvancedCommandsRole ) )
						{
								ButtonField btnLock = new ButtonField();
								//btnLock.HeaderStyle.Width = new Unit( 16, UnitType.Pixel );
								//btnLock.ImageUrl = ConfigValue( "CMS:EditButtonImage" );
								btnLock.Text = Resources.Controls.ForumsControl_ColumnLock;
								btnLock.ButtonType = ButtonType.Link;
								btnLock.CommandName = LOCK_COMMAND;
								dg.Columns.Add( btnLock );

								dg.RowCommand += OnRowCommand;
								dg.RowDataBound += OnRowDataBound;
						}

						return dg;
				}

				private List<ForumEntity> GetDataGridData()
				{
						if ( this.Filter != null )
						{
								List<ForumEntity> listByFilter = Storage<ForumEntity>.Read( this.Filter );
								return listByFilter;
						}

						ForumThreadEntity ft = ForumThreadEntity; // oxy: Storage<ForumThreadEntity>.ReadFirst(new ForumThreadEntity.ReadById { ForumThreadId = this.ForumThreadId });
						if ( !IsVisibleForRole( ft ) ) return new List<ForumEntity>();

						List<ForumEntity> list = Storage<ForumEntity>.Read( new ForumEntity.ReadByForumThreadId { ForumThreadId = this.ForumThreadId } );
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
				void OnRowDataBound( object sender, GridViewRowEventArgs e )
				{
						if ( e.Row.RowType != DataControlRowType.DataRow ) return;
						if ( !Security.IsLogged( false ) || String.IsNullOrEmpty( AdvancedCommandsRole ) || !Security.IsInRole( AdvancedCommandsRole ) )
								return;

						ForumEntity forum = (ForumEntity)e.Row.DataItem;

						int lockIndex = e.Row.Cells.Count - 1;
						LinkButton lbLock = e.Row.Cells[lockIndex].Controls[0] as LinkButton;
						lbLock.CommandName = forum.Locked ? UNLOCK_COMMAND : LOCK_COMMAND;
						lbLock.Text = forum.Locked ? Resources.Controls.ForumsControl_ColumnUnLock : Resources.Controls.ForumsControl_ColumnLock;
				}
				void OnRowCommand( object sender, GridViewCommandEventArgs e )
				{
						if ( e.CommandName == LOCK_COMMAND ) OnLockUnlockForumCommand( sender, e );
						if ( e.CommandName == UNLOCK_COMMAND ) OnLockUnlockForumCommand( sender, e );
				}
				void OnLockUnlockForumCommand( object sender, GridViewCommandEventArgs e )
				{
						int rowIndex = Convert.ToInt32( e.CommandArgument );
						int id = Convert.ToInt32( ( sender as GridView ).DataKeys[rowIndex].Value );

						ForumEntity forum = Storage<ForumEntity>.ReadFirst( new ForumEntity.ReadById { ForumId = id } );
						forum.Locked = !forum.Locked;
						Storage<ForumEntity>.Update( forum );

						//Rebind datagrid
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

								string url = string.Format( this.RSSFormatUrl, ( row.DataItem as ForumEntity ).Id, ( row.DataItem as ForumEntity ).Name );
								control.NavigateUrl = control.Page.ResolveUrl( url );
						}

						#endregion
				}
				/// <summary>
				/// Template field na zobrazenie Vzdy hore.
				/// </summary>
				private class PinnedTemplate: ITemplate
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

								if ( ( row.DataItem as ForumEntity ).Pinned )
										control.Attributes.Add( "class", this.CssClass + "_itemPinned" );
								else
										control.Attributes.Add( "class", this.CssClass + "_itemUnPinned" );
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

								control.Attributes.Add( "href", control.Page.ResolveUrl( ( row.DataItem as ForumEntity ).Alias ) );
						}

						void image_DataBinding( object sender, EventArgs e )
						{
								Image control = sender as Image;
								control.ImageAlign = ImageAlign.Middle;
								GridViewRow row = (GridViewRow)control.NamingContainer;

								string icon = ( row.DataItem as ForumEntity ).Icon;
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
						public int MaxDescriptionLength { get; set; }
						#endregion

						#region ITemplate Members

						public void InstantiateIn( Control container )
						{
								Table table = new Table();
								table.CssClass = this.CssClass + "_nameItem";

								TableRow row = new TableRow();

								TableCell cell = new TableCell();
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

								control.Text = ( row.DataItem as ForumEntity ).Name;
								control.NavigateUrl = control.Page.ResolveUrl( ( row.DataItem as ForumEntity ).Alias );
						}

						void OnDescriptionDataBinding( object sender, EventArgs e )
						{
								Label control = sender as Label;
								GridViewRow row = (GridViewRow)control.NamingContainer;

								string description = ( row.DataItem as ForumEntity ).Description;
								if ( !string.IsNullOrEmpty( description ) && this.MaxDescriptionLength > 0 )
								{
										if ( description.Length > this.MaxDescriptionLength )
										{
												description = description.Substring( 0, this.MaxDescriptionLength );
												description += " ...";
										}
								}
								control.Text = description;
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

								if ( ( row.DataItem as ForumEntity ).Locked )
										control.Attributes.Add( "class", this.CssClass + "_itemLocked" );
								else
										control.Attributes.Add( "class", this.CssClass + "_itemUnLocked" );
						}
						#endregion
				}
				/// <summary>
				/// Template field na zobrazenie Posledneho prispevku.
				/// </summary>
				private class LastPostTemplate: ITemplate
				{
						#region Public properties
						public string CssClass { get; set; }
						public int MaxDescriptionLength { get; set; }
						#endregion

						#region ITemplate Members

						public void InstantiateIn( Control container )
						{
								Table table = new Table();
								table.CssClass = this.CssClass + "_lastPostItem";

								TableRow row = new TableRow();

								TableCell cell = new TableCell();
								Label lblDate = new Label();
								lblDate.DataBinding += OnDateDataBinding;
								lblDate.CssClass = this.CssClass + "_lastPostItem_date";
								cell.Controls.Add( lblDate );
								row.Cells.Add( cell );
								table.Rows.Add( row );

								row = new TableRow();
								cell = new TableCell();
								cell.Wrap = true;
								Label lblUser = new Label();
								lblUser.CssClass = this.CssClass + "_lastPostItem_user";
								lblUser.DataBinding += OnUserDataBinding;
								cell.Controls.Add( lblUser );
								row.Cells.Add( cell );
								table.Rows.Add( row );

								container.Controls.Add( table );

						}

						void OnDateDataBinding( object sender, EventArgs e )
						{
								Label control = sender as Label;
								GridViewRow row = (GridViewRow)control.NamingContainer;

								DateTime? lastPost = ( row.DataItem as ForumEntity ).LastPostDate;
								control.Text = lastPost.HasValue ? lastPost.Value.ToString() : string.Empty;
						}

						void OnUserDataBinding( object sender, EventArgs e )
						{
								Label control = sender as Label;
								GridViewRow row = (GridViewRow)control.NamingContainer;

								string accountName = ( row.DataItem as ForumEntity ).LastPostAccountName;
								int atIndex = accountName.IndexOf( "@" );
								if ( atIndex != -1 )
										accountName = accountName.Substring( 0, atIndex + 1 ) + "...";

								control.Text = accountName;
						}

						#endregion
				}
		}
}
