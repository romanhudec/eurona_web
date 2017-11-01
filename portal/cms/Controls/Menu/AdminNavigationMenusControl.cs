using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using MenuEntity = CMS.Entities.Menu;
using NavigationMenuEntity = CMS.Entities.NavigationMenu;
using Telerik.Web.UI;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace CMS.Controls.Menu
{
		public class AdminNavigationMenusControl: CmsControl
		{
				private const string DELETE_COMMAND = "DELETE_ITEM";
				private const string EDIT_COMMAND = "EDIT_ITEM";
				private const string SUBMENU_ITEMS_COMMAND = "DOWN_ITEM";
				private const string MOVE_UP_COMMAND = "UP_ITEM";
				private const string MOVE_DOWN_COMMAND = "DOWN_ITEM";

				private DropDownList ddlMenuGroup;
				private RadGrid gridView;

				public string EditUrlFormat { get; set; }
				public string NewUrl { get; set; }
				public string SubMenuUrlFormat { get; set; }

				public SortDirection SortDirection
				{
						get { return GetSession<SortDirection>( "AdminNavigationMenusControl-SortDirection", SortDirection.Ascending ); }
						set { SetSession<SortDirection>( "AdminNavigationMenusControl-SortDirection", value ); }
				}

				public string SortExpression
				{
						get { return GetSession<string>( "AdminNavigationMenusControl-SortExpression", "Order" ); }
						set { SetSession<string>( "AdminNavigationMenusControl-SortExpression", value ); }
				}

				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						this.ddlMenuGroup = new DropDownList();
						this.ddlMenuGroup.ID = "ddlMenuGroup";
						this.ddlMenuGroup.AutoPostBack = true;
						this.ddlMenuGroup.SelectedIndexChanged += new EventHandler( ddlMenuGroup_SelectedIndexChanged );
						this.ddlMenuGroup.DataSource = Storage<MenuEntity>.Read();
						this.ddlMenuGroup.DataTextField = "Name";
						this.ddlMenuGroup.DataValueField = "Id";

						HtmlGenericControl div = new HtmlGenericControl( "div" );
						div.Controls.Add( new LiteralControl( Resources.Controls.AdminMenusControl_MenuGroup ) );
						div.Controls.Add( ddlMenuGroup );
						this.Controls.Add( div );
						this.Controls.Add( new LiteralControl( "<br />" ) );

						//---------------------------------------
						if ( !IsPostBack )
						{
								this.ddlMenuGroup.DataBind();
								object selectedMenuId = Session[this.ddlMenuGroup.ID];
								if ( selectedMenuId != null && !string.IsNullOrEmpty( selectedMenuId.ToString() ) )
								{
										int menuId = 0;
										if ( Int32.TryParse( selectedMenuId.ToString(), out menuId ) )
												this.ddlMenuGroup.SelectedValue = menuId.ToString();
								}
						}
						gridView = CreateGridView();
						this.Controls.Add( gridView );
						GridViewDataBind( !IsPostBack );

				}

				void ddlMenuGroup_SelectedIndexChanged( object sender, EventArgs e )
				{
						Session[this.ddlMenuGroup.ID] = Convert.ToInt32( this.ddlMenuGroup.SelectedValue );
						GridViewDataBind( true );
				}

				private void GridViewDataBind( bool bind )
				{
						int menuId = 0;
						if ( !string.IsNullOrEmpty( this.ddlMenuGroup.SelectedValue ) )
								menuId = Convert.ToInt32( this.ddlMenuGroup.SelectedValue );

						List<NavigationMenuEntity> menus = Storage<NavigationMenuEntity>.Read( new NavigationMenuEntity.ReadByMenuId { MenuId = menuId } );
						var ordered = menus.AsQueryable().OrderBy( SortExpression + " " + SortDirection );
						gridView.DataSource = ordered.ToList();
						if ( bind ) gridView.DataBind();
				}

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
						grid.MasterTableView.CommandItemSettings.AddNewRecordText = Resources.Controls.AdminMenusControl_NewMenuButton_Text;

						grid.Columns.Add( new GridBoundColumn
						{
								DataField = "Order",
								HeaderText = Resources.Controls.AdminMenusControl_ColumnOrder,
								SortExpression = "Order",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );

						grid.Columns.Add( new GridIconColumn
						{
								DataImageUrlFields = new string[] { "Icon" },
								ImageWidth = Unit.Pixel( 16 ),
								ImageHeight = Unit.Pixel( 16 ),
								HeaderText = Resources.Controls.AdminMenusControl_ColumnIcon,
								ShowFilterIcon = false
						} );

						grid.Columns.Add( new GridBoundColumn
						{
								DataField = "Name",
								HeaderText = Resources.Controls.AdminMenusControl_ColumnName,
								SortExpression = "Name",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );
						grid.Columns.Add( new GridBoundColumn
						{
								DataField = "Alias",
								HeaderText = Resources.Controls.AdminMenusControl_ColumnAlias,
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );
						grid.Columns.Add( new GridBoundColumn
						{
								DataField = "RoleName",
								HeaderText = Resources.Controls.AdminAccountsControl_ColumnRole,
								SortExpression = "RoleName",
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

						GridButtonColumn btnSubItems = new GridButtonColumn();
						btnSubItems.HeaderStyle.Width = new Unit( 16, UnitType.Pixel );
						btnSubItems.ImageUrl = ConfigValue( "CMS:MenuItemsImage" );
						btnSubItems.Text = Resources.Controls.NavigationMenusControl_SubMenuToolTip;
						btnSubItems.ButtonType = GridButtonColumnType.ImageButton;
						btnSubItems.CommandName = SUBMENU_ITEMS_COMMAND;
						grid.MasterTableView.Columns.Add( btnSubItems );

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

				void OnRowCommand( object sender, GridCommandEventArgs e )
				{
						if ( e.CommandName == RadGrid.InitInsertCommandName ) OnNewCommand( sender, e );
						if ( e.CommandName == EDIT_COMMAND ) OnEditCommand( sender, e );
						if ( e.CommandName == DELETE_COMMAND ) OnDeleteCommand( sender, e );
						if ( e.CommandName == SUBMENU_ITEMS_COMMAND ) OnSubMenuCommand( sender, e );
				}
				private void OnNewCommand( object sender, GridCommandEventArgs e )
				{
						string url = Page.ResolveUrl( NewUrl + string.Format( "&MenuId={0}&", this.ddlMenuGroup.SelectedValue ) + base.BuildReturnUrlQueryParam() );
						Response.Redirect( url );
				}
				private void OnEditCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int menuId = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );
						Response.Redirect( String.Format( EditUrlFormat, menuId ) + "&" + base.BuildReturnUrlQueryParam() );
				}

				private void OnDeleteCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int menuId = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );
						NavigationMenuEntity menu = Storage<NavigationMenuEntity>.ReadFirst( new NavigationMenuEntity.ReadById { NavigationMenuId = menuId } );
						Storage<NavigationMenuEntity>.Delete( menu );
						GridViewDataBind( true );
				}

				private void OnSubMenuCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int menuId = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );
						Response.Redirect( String.Format( SubMenuUrlFormat, menuId ) + "&" + base.BuildReturnUrlQueryParam() );
				}
		}
}
