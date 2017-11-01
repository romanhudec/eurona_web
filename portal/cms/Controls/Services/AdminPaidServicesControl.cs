using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using CMS.Entities.Classifiers;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

namespace CMS.Controls.Services
{
		public class AdminPaidServicesControl: CmsControl
		{
				private const string EDIT_COMMAND = "EDIT_ITEM";

				protected RadGrid dataGrid = null;

				public SortDirection SortDirection
				{
						get { return GetSession<SortDirection>( "AdminPaidServicesControl-SortDirection", SortDirection.Ascending ); }
						set { SetSession<SortDirection>( "AdminPaidServicesControl-SortDirection", value ); }
				}
				public string SortExpression
				{
						get { return GetSession<string>( "AdminPaidServicesControl-SortExpression" ); }
						set { SetSession<string>( "AdminPaidServicesControl-SortExpression", value ); }
				}

				public AdminPaidServicesControl()
				{
				}

				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						HtmlGenericControl div = new HtmlGenericControl( "div" );
						//DataGrid
						this.dataGrid = CreateGridControl();
						div.Controls.Add( this.dataGrid );
						this.Controls.Add( div );

						//Binding
						this.dataGrid.DataSource = GetDataGridData();
						if ( !IsPostBack )
								this.dataGrid.DataBind();

				}
				#endregion

				public string NewUrlFormat { get; set; }
				public string EditUrlFormat { get; set; }

				/// <summary>
				/// Vytvori DataGrid control s pozadovanymi stlpcami a 
				/// pripravenym bindingom stlpcou.
				/// </summary>
				private RadGrid CreateGridControl()
				{
						RadGrid grid = new RadGrid();
						CMS.Utilities.RadGridUtilities.Localize( grid );
						grid.MasterTableView.DataKeyNames = new string[] { "Id" };

						grid.AllowAutomaticInserts = false;
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
						
						GridBoundColumn bf = new GridBoundColumn();
						bf.DataField = "Name";
						bf.HeaderText = Resources.Controls.AdminPaidServicesControl_ColumnName;
						bf.SortExpression = "Name";
						bf.AutoPostBackOnFilter = true;
						bf.CurrentFilterFunction = GridKnownFunction.Contains;
						grid.Columns.Add( bf );

						bf = new GridBoundColumn();
						bf.DataField = "Notes";
						bf.HeaderText = Resources.Controls.AdminPaidServicesControl_ColumnNotes;
						bf.SortExpression = "Notes";
						bf.AutoPostBackOnFilter = true;
						bf.CurrentFilterFunction = GridKnownFunction.Contains;
						grid.Columns.Add( bf );

						bf = new GridBoundColumn();
						bf.DataField = "CreditCost";
						bf.HeaderText = Resources.Controls.AdminPaidServicesControl_ColumnCreditCost;
						bf.SortExpression = "CreditCost";
						bf.AutoPostBackOnFilter = true;
						bf.CurrentFilterFunction = GridKnownFunction.Contains;
						grid.Columns.Add( bf );

						GridButtonColumn btnEdit = new GridButtonColumn();
						btnEdit.HeaderStyle.Width = new Unit( 16, UnitType.Pixel );
						btnEdit.ImageUrl = ConfigValue( "CMS:EditButtonImage" );
						btnEdit.Text = Resources.Controls.GridView_ToolTip_EditItem;
						btnEdit.ButtonType = GridButtonColumnType.ImageButton;
						btnEdit.CommandName = EDIT_COMMAND;
						grid.MasterTableView.Columns.Add( btnEdit );

						grid.ItemCommand += OnRowCommand;

						return grid;
				}

				protected virtual Object GetDataGridData()
				{
						List<PaidService> list = Storage<PaidService>.Read();

						SortDirection previous = SortDirection;
						string sortExpression = String.IsNullOrEmpty( SortExpression ) ? "Name" : SortExpression;
						var ordered = list.AsQueryable().OrderBy( sortExpression + " " + ( previous == SortDirection.Ascending ? "ascending" : "descending" ) );
						return ordered.ToList();

				}

				#region Event handlers
				void OnRowCommand( object sender, GridCommandEventArgs e )
				{
						if ( e.CommandArgument == null )
								return;

						//Ak to je neznamy command, nebu se spracovavat tu.
						if ( e.CommandName != EDIT_COMMAND )
								return;

						GridDataItem dataItem = (GridDataItem)e.Item;
						int id = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );

						if ( e.CommandName == EDIT_COMMAND )
						{
								string url = Page.ResolveUrl( string.Format( EditUrlFormat, id ) + "&" + base.BuildReturnUrlQueryParam() );
								Response.Redirect( url );
								return;
						}
				}
				#endregion
		}
}
