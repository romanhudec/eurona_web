using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.UI.WebControls;
using AccountEntity = CMS.Entities.Account;
using Telerik.Web.UI;

namespace CMS.Controls.Account
{
		public class AdminAccountsControl: CmsControl
		{
				private const string DELETE_COMMAND = "DELETE_ITEM";
				private const string EDIT_COMMAND = "EDIT_ITEM";
				private const string CREDIT_COMMAND = "CREDIT_ITEM";
				//private const string ROLES_COMMAND = "ROLES";

				private RadGrid gridView;

				public AdminAccountsControl()
				{
				}

				public string IdentificationUrlFromat { get; set; }
				public string NewUrl { get; set; }
				public string EditUrlFormat { get; set; }
				public string AddCreditUrlFormat { get; set; }
				public string RolesUrlFormat { get; set; }

				//public virtual IStorage<T> GetStorage<T>() where T: CMS.Entities.Entity, new()
				//{
				//    return Storage<T>.Instance;
				//}

				public SortDirection SortDirection
				{
						get { return GetSession<SortDirection>( "AdminAccountsControl-SortDirection", SortDirection.Ascending ); }
						set { SetSession<SortDirection>( "AdminAccountsControl-SortDirection", value ); }
				}

				public string SortExpression
				{
						get { return GetSession<string>( "AdminAccountsControl-SortExpression", "Login" ); }
						set { SetSession<string>( "AdminAccountsControl-SortExpression", value ); }
				}

				public bool HideCredit { get; set; }

				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						gridView = CreateGridView();
						this.Controls.Add( gridView );

						GridViewDataBind( !IsPostBack );
				}

				private void GridViewDataBind( bool bind )
				{
						List<AccountEntity> accounts = Storage<AccountEntity>.Read();
						var ordered = accounts.AsQueryable().OrderBy( SortExpression + " " + SortDirection );
						gridView.DataSource = ordered.ToList();
						if ( bind ) gridView.DataBind();
				}

				private RadGrid CreateGridView()
				{
						RadGrid grid = new RadGrid();
                        grid.CssClass = this.CssClass + "_grid";
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
						grid.MasterTableView.CommandItemSettings.AddNewRecordText = Resources.Controls.AdminAccountsControl_NewAccountButton_Text;

						if ( string.IsNullOrEmpty( this.IdentificationUrlFromat ) )
						{
								grid.Columns.Add( new GridBoundColumn
								{
										DataField = "Login",
										HeaderText = Resources.Controls.AdminAccountsControl_ColumnLogin,
										SortExpression = "Login",
										AutoPostBackOnFilter = true,
										CurrentFilterFunction = GridKnownFunction.Contains
								} );
						}
						else
						{
								grid.Columns.Add( new GridHyperLinkColumn
								{
										DataTextField = "Login",
										HeaderText = Resources.Controls.AdminAccountsControl_ColumnLogin,
										SortExpression = "Login",
										AutoPostBackOnFilter = true,
										CurrentFilterFunction = GridKnownFunction.Contains,
										DataNavigateUrlFields = new string[] { "Id" },
										DataNavigateUrlFormatString = Page.ResolveUrl( this.IdentificationUrlFromat + "&" + base.BuildReturnUrlQueryParam() ),
								} );
						}
						grid.Columns[0].HeaderStyle.Width = Unit.Pixel( 80 );

						grid.Columns.Add( new GridHyperLinkColumn
						{
								DataTextField = "Email",
								HeaderText = Resources.Controls.AdminAccountsControl_ColumnEmail,
								SortExpression = "Email",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains,
								DataTextFormatString = "<a href=mailto:{0:g}>{0:g}</a>",
						} );
						grid.Columns[1].HeaderStyle.Width = Unit.Pixel( 120 );

						grid.Columns.Add( new GridCheckBoxColumn
						{
								DataField = "Enabled",
								HeaderText = Resources.Controls.AdminAccountsControl_ColumnEnabled,
								SortExpression = "Enabled",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );
						grid.Columns[2].HeaderStyle.Width = Unit.Pixel( 60 );

						grid.Columns.Add( new GridCheckBoxColumn
						{
								DataField = "Verified",
								HeaderText = Resources.Controls.AdminAccountsControl_ColumnVerified,
								SortExpression = "Verified",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );
						grid.Columns[3].HeaderStyle.Width = Unit.Pixel( 60 );

						grid.Columns.Add( new GridBoundColumn
						{
								DataField = "RoleStringDisplay",
								HeaderText = Resources.Controls.AdminAccountsControl_ColumnRoles,
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );
						grid.Columns[4].ItemStyle.Wrap = true;

						if ( !HideCredit )
						{
								grid.Columns.Add( new GridBoundColumn
								{
										DataField = "Credit",
										HeaderText = Resources.Controls.AdminAccountsControl_ColumnCredit,
										AutoPostBackOnFilter = true,
										CurrentFilterFunction = GridKnownFunction.Contains
								} );
						}

						if ( !HideCredit )
						{
								GridButtonColumn btnAddCredit = new GridButtonColumn();
								btnAddCredit.HeaderStyle.Width = new Unit( 16, UnitType.Pixel );
								btnAddCredit.ImageUrl = ConfigValue( "CMS:EuroButtonImage" );
								btnAddCredit.Text = Resources.Controls.AdminAccountsControl_ToolTip_AddCredit;
								btnAddCredit.ButtonType = GridButtonColumnType.ImageButton;
								btnAddCredit.CommandName = CREDIT_COMMAND;
								grid.MasterTableView.Columns.Add( btnAddCredit );
						}

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

				void OnRowCommand( object sender, GridCommandEventArgs e )
				{
						if ( e.CommandName == RadGrid.InitInsertCommandName ) OnNewCommand( sender, e );
						if ( e.CommandName == EDIT_COMMAND ) OnEditCommand( sender, e );
						if ( e.CommandName == DELETE_COMMAND ) OnDeleteCommand( sender, e );
						if ( e.CommandName == CREDIT_COMMAND ) OnAddCreditCommand( sender, e );
						//if ( e.CommandName == ROLES_COMMAND ) OnRolesCommand( sender, e );
				}
				private void OnNewCommand( object sender, GridCommandEventArgs e )
				{
						string url = Page.ResolveUrl( NewUrl + "&" + base.BuildReturnUrlQueryParam() );
						Response.Redirect( url );
				}
				private void OnEditCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int accountId = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );
						Response.Redirect( String.Format( EditUrlFormat, accountId ) + "&" + base.BuildReturnUrlQueryParam() );
				}

				private void OnDeleteCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int accountId = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );
						AccountEntity account = Storage<AccountEntity>.ReadFirst( new AccountEntity.ReadById { AccountId = accountId } );
						Storage<AccountEntity>.Delete( account );
						GridViewDataBind( true );
				}
				private void OnAddCreditCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int accountId = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );
						Response.Redirect( String.Format( AddCreditUrlFormat, accountId ) + "&" + base.BuildReturnUrlQueryParam() );
				}
		}
}
