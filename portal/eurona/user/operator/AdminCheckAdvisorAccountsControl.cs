using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using CMS.Controls;
using AccountEntity = Eurona.DAL.Entities.AdvisorAccount;

namespace Eurona.User.Operator
{
		public class AdminCheckAdvisorAccountsControl: CmsControl
		{
				private const string DELETE_COMMAND = "DELETE_ITEM";
				private const string ENABLE_COMMAND = "ENABLE_ITEM";
				private const string VERIFY_COMMAND = "VERIFY_ITEM";

				private RadGrid gridView;

				public AdminCheckAdvisorAccountsControl()
				{
				}

				public string IdentificationUrlFromat { get; set; }
				public string NewUrl { get; set; }
				public string EditUrlFormat { get; set; }
				public string AddCreditUrlFormat { get; set; }
				public string RolesUrlFormat { get; set; }

				public SortDirection SortDirection
				{
						get { return GetSession<SortDirection>( "AdminCheckAdvisorAccountsControl-SortDirection", SortDirection.Ascending ); }
						set { SetSession<SortDirection>( "AdminCheckAdvisorAccountsControl-SortDirection", value ); }
				}

				public string SortExpression
				{
						get { return GetSession<string>( "AdminCheckAdvisorAccountsControl-SortExpression", "Login" ); }
						set { SetSession<string>( "AdminCheckAdvisorAccountsControl-SortExpression", value ); }
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
						List<AccountEntity> accounts = Storage<AccountEntity>.Read( new AccountEntity.ReadDisabled() );
						var ordered = accounts.AsQueryable().OrderBy( SortExpression + " " + SortDirection );
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
						grid.GroupingSettings.CaseSensitive = false;
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

						//grid.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.TopAndBottom;
						//grid.MasterTableView.CommandItemSettings.AddNewRecordText = global::CMS.Resources.Controls.AdminAccountsControl_NewAccountButton_Text;

						GridBoundColumn gbc = new GridBoundColumn();
						gbc.DataField = "Created";
						gbc.HeaderText = "Datum registrace";
						gbc.SortExpression = "Created";
						gbc.AutoPostBackOnFilter = true;
						gbc.DataFormatString = "{0:d}";
						gbc.CurrentFilterFunction = GridKnownFunction.Contains;
						gbc.HeaderStyle.Width = Unit.Pixel( 90 );
						grid.Columns.Add( gbc );

						grid.Columns.Add( new GridBoundColumn
						{
								DataField = "AdvisorCode",
								HeaderText = "Reg. číslo",
								SortExpression = "AdvisorCode",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains
						} );

						if ( string.IsNullOrEmpty( this.IdentificationUrlFromat ) )
						{
								grid.Columns.Add( new GridBoundColumn
								{
										DataField = "Login",
										HeaderText = global::CMS.Resources.Controls.AdminAccountsControl_ColumnLogin,
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
										HeaderText = global::CMS.Resources.Controls.AdminAccountsControl_ColumnLogin,
										SortExpression = "Login",
										AutoPostBackOnFilter = true,
										CurrentFilterFunction = GridKnownFunction.Contains,
										DataNavigateUrlFields = new string[] { "Id" },
										DataNavigateUrlFormatString = Page.ResolveUrl( this.IdentificationUrlFromat + "&" + base.BuildReturnUrlQueryParam() ),
								} );
						}

						grid.Columns.Add( new GridHyperLinkColumn
						{
								DataTextField = "Email",
								HeaderText = global::CMS.Resources.Controls.AdminAccountsControl_ColumnEmail,
								SortExpression = "Email",
								AutoPostBackOnFilter = true,
								CurrentFilterFunction = GridKnownFunction.Contains,
								DataTextFormatString = "<a href=mailto:{0:g}>{0:g}</a>",
						} );

						grid.Columns.Add( new GridCheckBoxColumn
						{
								DataField = "Enabled",
								HeaderText = "Povolen",
								SortExpression = "Enabled",
								AllowFiltering = false
						} );

						grid.Columns.Add( new GridCheckBoxColumn
						{
								DataField = "Verified",
								HeaderText = "Ověřen",
								SortExpression = "Verified",
								AllowFiltering = false
						} );

						grid.Columns.Add( new GridHyperLinkColumn
						{
								DataTextField = "Id",
								AllowFiltering = false,
								DataTextFormatString = "<a href='findSimilarAdvisor.aspx?id={0}' target='_blank'>Zkontrolovat</a>",
						} );


						GridButtonColumn btnEdit = new GridButtonColumn();
						btnEdit.HeaderStyle.Width = new Unit( 16, UnitType.Pixel );
						btnEdit.ImageUrl = ConfigValue( "CMS:OkButtonImage" );
						btnEdit.Text = "Povolit";
						btnEdit.ButtonType = GridButtonColumnType.ImageButton;
						btnEdit.ConfirmText = "Skutečne má být tento účet povolen?";
						btnEdit.CommandName = ENABLE_COMMAND;
						grid.MasterTableView.Columns.Add( btnEdit );

						GridButtonColumn btnVerify = new GridButtonColumn();
						btnVerify.HeaderStyle.Width = new Unit( 16, UnitType.Pixel );
						btnVerify.ImageUrl = ConfigValue( "CMS:VerifyButtonImage" );
						btnVerify.Text = "Ověřit";
						btnVerify.ButtonType = GridButtonColumnType.ImageButton;
						btnVerify.ConfirmText = "Skutečne je tento uživatel ověřen?";
						btnVerify.CommandName = VERIFY_COMMAND;
						grid.MasterTableView.Columns.Add( btnVerify );

						GridButtonColumn btnDelete = new GridButtonColumn();
						btnDelete.HeaderStyle.Width = new Unit( 16, UnitType.Pixel );
						btnDelete.ImageUrl = ConfigValue( "CMS:DeleteButtonImage" );
						btnDelete.Text = global::CMS.Resources.Controls.GridView_ToolTip_DeleteItem;
						btnDelete.ButtonType = GridButtonColumnType.ImageButton;
						btnDelete.ConfirmTitle = global::CMS.Resources.Controls.DeleteItemQuestion;
						btnDelete.ConfirmText = global::CMS.Resources.Controls.DeleteItemQuestion;
						btnDelete.CommandName = DELETE_COMMAND;
						grid.MasterTableView.Columns.Add( btnDelete );

						grid.ItemCommand += OnRowCommand;
						grid.ItemDataBound += OnRowDataBound;

						return grid;
				}

				void OnRowDataBound( object sender, GridItemEventArgs e )
				{
						if ( e.Item.ItemType != GridItemType.Item && e.Item.ItemType != GridItemType.AlternatingItem )
								return;

						AccountEntity account = e.Item.DataItem as AccountEntity;
						int codeIndex = 3;
						int deleteIndex = e.Item.Cells.Count - 1;
						int verifyIndex = e.Item.Cells.Count - 2;
						int enableIndex = e.Item.Cells.Count - 3;
					
						ImageButton btnDelete = ( e.Item.Cells[deleteIndex].Controls[0] as ImageButton );
						ImageButton btnVerify = ( e.Item.Cells[verifyIndex].Controls[0] as ImageButton );
						ImageButton btnEnable = ( e.Item.Cells[enableIndex].Controls[0] as ImageButton );

						btnVerify.Enabled = !account.Verified && account.IsValidRegistered;
						btnEnable.Enabled = account.IsValidRegistered;

						if ( !btnVerify.Enabled ) btnVerify.ImageUrl = ConfigValue( "CMS:VerifyButtonImageD" );
						if ( !btnEnable.Enabled ) btnEnable.ImageUrl = ConfigValue( "CMS:OkButtonImageD" );


						if ( !account.IsValidRegistered )
						{
								GridTableCell cell = ( e.Item.Cells[codeIndex] as GridTableCell );
								Label lblCode = new Label();
								lblCode.Text = "Nedokončená registrace!";
								lblCode.ForeColor = System.Drawing.Color.Red;
								cell.Controls.Add( lblCode );
						}

				}

				void OnRowCommand( object sender, GridCommandEventArgs e )
				{
						if ( e.CommandName == ENABLE_COMMAND ) OnEnableCommand( sender, e );
						if ( e.CommandName == VERIFY_COMMAND ) OnVerifyCommand( sender, e );
						if ( e.CommandName == DELETE_COMMAND ) OnDeleteCommand( sender, e );
				}

				private void OnEnableCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int accountId = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );
						AccountEntity account = Storage<AccountEntity>.ReadFirst( new AccountEntity.ReadById { AccountId = accountId } );
						account.Enabled = true;
						Storage<AccountEntity>.Update( account );

						CMS.EmailNotification email = new CMS.EmailNotification();
						email.To = account.Email;
						email.Subject = "Povolení registrace EURONA";
						email.Message = "Dobrý den<br/>právě Vám byl povolen účet na EURONA.cz<br/>Přejeme příjemné nakupování.<br/>S pozdravem<br/>EURONA.cz";
						email.Notify(true);

						GridViewDataBind( true );
				}

				private void OnVerifyCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int accountId = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );
						AccountEntity account = Storage<AccountEntity>.ReadFirst( new AccountEntity.ReadById { AccountId = accountId } );
						account.Verified = true;
						Storage<AccountEntity>.Update( account );
						GridViewDataBind( true );
				}

				private void OnDeleteCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int accountId = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );
						AccountEntity account = Storage<AccountEntity>.ReadFirst( new AccountEntity.ReadById { AccountId = accountId } );
						Storage<AccountEntity>.Delete( account );

						CMS.EmailNotification email = new CMS.EmailNotification();
						email.To = account.Email;
						email.Subject = "Zrušení registrace EURONA";
						email.Message = "Dobrý den<br/>Vaše registrace byla zrušena!<br/>Pro více informací kontaktujte naše operátorky.<br/>S pozdravem<br/>EURONA.cz";
						email.Notify( true );

						GridViewDataBind( true );
				}
		}
}
