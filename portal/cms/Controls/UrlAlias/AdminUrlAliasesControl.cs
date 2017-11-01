using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.UI.WebControls;
using UrlAliasEntity = CMS.Entities.UrlAlias;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

namespace CMS.Controls.UrlAlias
{
		public class AdminUrlAliasesControl: CmsControl
		{
				internal enum AliasType: int
				{
						All = 0,
						Pages = 1,
						Articles = 2,
						Blogs = 3,
						ImageGalleries = 4,
						News = 5,
						Custom = -1
				}
				private const string EDIT_COMMAND = "EDIT_ITEM";
				private const string DELETE_COMMAND = "DELETE_ITEM";

				protected DropDownList ddlAliasType;
				protected RadGrid gridView;

				public AdminUrlAliasesControl()
				{
				}

				public string EditUrlFormat { get; set; }
				public string UrlAliassUrlFormat { get; set; }
				public string NewUrl { get; set; }

				public SortDirection SortDirection
				{
						get { return GetSession<SortDirection>( "AdminUrlAliasesControl-SortDirection", SortDirection.Ascending ); }
						set { SetSession<SortDirection>( "AdminUrlAliasesControl-SortDirection", value ); }
				}

				public string SortExpression
				{
						get { return GetSession<string>( "AdminUrlAliasesControl-SortExpression", "Name" ); }
						set { SetSession<string>( "AdminUrlAliasesControl-SortExpression", value ); }
				}

				public bool HideCredit { get; set; }

				protected override void CreateChildControls()
				{
						base.CreateChildControls();
						this.ddlAliasType = new DropDownList();
						this.ddlAliasType.ID = "ddlAliasType";
						this.ddlAliasType.AutoPostBack = true;
						this.ddlAliasType.SelectedIndexChanged += ( s, e ) =>
								{
										int aliasTypeId = Convert.ToInt32( this.ddlAliasType.SelectedValue );
										GridViewDataBind( true, aliasTypeId );
								};

						HtmlGenericControl div = new HtmlGenericControl( "div" );
						div.Controls.Add( new LiteralControl( Resources.Controls.AdminUrlAliasesControl_AliasType ) );
						div.Controls.Add( this.ddlAliasType );
						this.Controls.Add( div );
						this.Controls.Add( new LiteralControl( "<br />" ) );

						gridView = CreateGridView();
						this.Controls.Add( gridView );
						BindControls();
				}

				#region Protected virtual methods
				protected virtual void BindControls()
				{
						int aliasTypeId = (int)AliasType.All;
						if ( !string.IsNullOrEmpty( this.ddlAliasType.SelectedValue ) )
								aliasTypeId = Convert.ToInt32( this.ddlAliasType.SelectedValue );
						GridViewDataBind( !this.IsPostBack, aliasTypeId );

						if ( this.IsPostBack )
								return;

						this.ddlAliasType.Items.Add( new ListItem( Resources.Controls.AdminUrlAliasesControl_AliasType_All, ( (int)AliasType.All ).ToString() ) );
						this.ddlAliasType.Items.Add( new ListItem( Resources.Controls.AdminUrlAliasesControl_AliasType_Pages, ((int)AliasType.Pages).ToString() ) );
						this.ddlAliasType.Items.Add( new ListItem( Resources.Controls.AdminUrlAliasesControl_AliasType_Articles, ((int)AliasType.Articles).ToString() ) );
						this.ddlAliasType.Items.Add( new ListItem( Resources.Controls.AdminUrlAliasesControl_AliasType_Blogs, ((int)AliasType.Blogs).ToString() ) );
						this.ddlAliasType.Items.Add( new ListItem( Resources.Controls.AdminUrlAliasesControl_AliasType_ImageGalleries, ((int)AliasType.ImageGalleries).ToString() ) );
						this.ddlAliasType.Items.Add( new ListItem( Resources.Controls.AdminUrlAliasesControl_AliasType_News, ((int)AliasType.News).ToString() ) );
						this.ddlAliasType.Items.Add( new ListItem( Resources.Controls.AdminUrlAliasesControl_AliasType_Custom, ( (int)AliasType.Custom ).ToString() ) );
				}

				protected virtual void GridViewDataBind( bool bind, object type )
				{
						AliasType aliasType = (AliasType)Convert.ToInt32( type );
						object condition = null;
						switch ( aliasType )
						{
								case  AliasType.All :
										condition = null;
										break;

								case AliasType.Pages:
										condition = new UrlAliasEntity.ReadByAliasType.Pages();
										break;

								case AliasType.Articles:
										condition = new UrlAliasEntity.ReadByAliasType.Articles();
										break;

								case AliasType.Blogs:
										condition = new UrlAliasEntity.ReadByAliasType.Blogs();
										break;

								case AliasType.ImageGalleries:
										condition = new UrlAliasEntity.ReadByAliasType.ImageGalleries();
										break;

								case AliasType.News:
										condition = new UrlAliasEntity.ReadByAliasType.News();
										break;

								case AliasType.Custom:
										condition = new UrlAliasEntity.ReadByAliasType.Custom();
										break;

								default:
										condition = null;
										break;
						}

						List<UrlAliasEntity> aliasses = Storage<UrlAliasEntity>.Read( condition );
						var ordered = aliasses.AsQueryable().OrderBy( SortExpression + " " + SortDirection );
						gridView.DataSource = ordered.ToList();
						if ( bind ) gridView.DataBind();
				}

				#endregion

				private RadGrid CreateGridView()
				{
						RadGrid grid = new RadGrid();
						CMS.Utilities.RadGridUtilities.Localize( grid );
						grid.MasterTableView.DataKeyNames = new string[] { "Id" };

						grid.AllowAutomaticInserts = true;
						grid.AllowFilteringByColumn = true;
						grid.ShowStatusBar = false;
						grid.ShowGroupPanel = true;
						grid.GroupingEnabled = true;
						grid.GroupingSettings.ShowUnGroupButton = true;
						grid.ClientSettings.AllowDragToGroup = true;
						grid.ClientSettings.AllowColumnsReorder = true;

						grid.MasterTableView.ShowHeader = true;
						grid.MasterTableView.ShowFooter = false;
						grid.MasterTableView.AllowPaging = true;
						grid.MasterTableView.PageSize = 25;
						grid.MasterTableView.PagerStyle.AlwaysVisible = true;
						grid.MasterTableView.AllowSorting = true;
						grid.MasterTableView.GridLines = GridLines.None;
						grid.MasterTableView.AutoGenerateColumns = false;

						grid.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.TopAndBottom;
						grid.MasterTableView.CommandItemSettings.AddNewRecordText = Resources.Controls.AdminUrlAliasesControl_NewUrlAliasButton_Text;

						grid.Columns.Add( new GridBoundColumn
						{
								DataField = "Name",
								HeaderText = Resources.Controls.AdminUrlAliasesControl_ColumnName,
								SortExpression = "Name",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );
						grid.Columns.Add( new GridBoundColumn
						{
								DataField = "Url",
								HeaderText = Resources.Controls.AdminUrlAliasesControl_ColumnUrl,
								SortExpression = "Url",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );
						grid.Columns.Add( new GridBoundColumn
						{
								DataField = "AliasUI",
								HeaderText = Resources.Controls.AdminUrlAliasesControl_ColumnAlias,
								SortExpression = "AliasUI",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );

						GridButtonColumn btnEdit = new GridButtonColumn();
						btnEdit.HeaderStyle.Width = new Unit( 16, UnitType.Pixel );
						btnEdit.ImageUrl = ConfigValue( "CMS:EditButtonImage" );
						btnEdit.Text = Resources.Controls.GridView_ToolTip_EditItem;
						btnEdit.ButtonType = GridButtonColumnType.ImageButton;
						btnEdit.CommandName = EDIT_COMMAND;
						grid.MasterTableView.Columns.Add( btnEdit );

						GridButtonColumn btnDelete = new GridButtonColumn();
						btnDelete.HeaderStyle.Width = new Unit( 16, UnitType.Pixel );
						btnDelete.ImageUrl = ConfigValue( "CMS:DeleteButtonImage" );
						btnDelete.Text = Resources.Controls.GridView_ToolTip_DeleteItem;
						btnDelete.ButtonType = GridButtonColumnType.ImageButton;
						btnDelete.ConfirmTitle = Resources.Controls.DeleteItemQuestion;
						btnDelete.ConfirmText = Resources.Controls.DeleteItemQuestion;
						btnDelete.CommandName = DELETE_COMMAND;
						grid.MasterTableView.Columns.Add( btnDelete );

						grid.ItemCommand += OnRowCommand;

						return grid;
				}
				void OnRowDataBound( object sender, GridItemEventArgs e )
				{
						if ( e.Item.ItemType != GridItemType.Item && e.Item.ItemType != GridItemType.AlternatingItem )
								return;

						UrlAliasEntity urlAlias = e.Item.DataItem as UrlAliasEntity;

						int nameIndex = 2;//Zacina vzdy indexom 2
						int deleteIndex = e.Item.Cells.Count - 1;
						ImageButton btnDelete = ( e.Item.Cells[deleteIndex].Controls[0] as ImageButton );

						//Systemove urlAlias
						if ( urlAlias.Id < 0 )
						{
								e.Item.Cells[nameIndex].Font.Bold = true;
								btnDelete.Enabled = false;
								btnDelete.ImageUrl = ConfigValue( "CMS:DeleteButtonImageD" );
						}
				}

				void OnRowCommand( object sender, GridCommandEventArgs e )
				{
						if ( e.CommandName == RadGrid.InitInsertCommandName ) OnNewCommand( sender, e );
						if ( e.CommandName == EDIT_COMMAND ) OnEditCommand( sender, e );
						if ( e.CommandName == DELETE_COMMAND ) OnDeleteCommand( sender, e );
				}
				private void OnNewCommand( object sender, GridCommandEventArgs e )
				{
						string url = Page.ResolveUrl( NewUrl + "&" + base.BuildReturnUrlQueryParam() );
						Response.Redirect( url );
						return;
				}
				private void OnEditCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int urlAliasId = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );

						string url = Page.ResolveUrl( string.Format( EditUrlFormat, urlAliasId ) + "&" + base.BuildReturnUrlQueryParam() );
						Response.Redirect( url );
						return;
				}

				private void OnDeleteCommand( object sender, GridCommandEventArgs e )
				{
						int aliasTypeId = Convert.ToInt32( this.ddlAliasType.SelectedValue );

						GridDataItem dataItem = (GridDataItem)e.Item;
						int urlAliasId = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );
						if ( urlAliasId < 0 ) return; //Systemove urlAlias sa nemazu.

						try
						{
								UrlAliasEntity urlAlias = Storage<UrlAliasEntity>.ReadFirst( new UrlAliasEntity.ReadById { UrlAliasId = urlAliasId } );
								Storage<UrlAliasEntity>.Delete( urlAlias );
								GridViewDataBind( true, aliasTypeId );
						}
						catch
						{
								this.Page.ClientScript.RegisterStartupScript( GetType(), "alert", "alert('" + Resources.Controls.AdminUrlAliasesControl_CanNotDeleteUsedUrlAlias_Message + "');", true );
						}
				}
		}
}
