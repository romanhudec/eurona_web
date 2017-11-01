using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using FAQEntinty = CMS.Entities.FAQ;
using Telerik.Web.UI;

namespace CMS.Controls.FAQ
{
		public class AdminFAQsControl: CmsControl
		{
				private const string DELETE_COMMAND = "DELETE_ITEM";
				private const string EDIT_COMMAND = "EDIT_ITEM";

				private RadGrid dataGrid = null;

				public SortDirection SortDirection
				{
						get { return GetState<SortDirection>( "AdminFAQsControl-SortDirection", SortDirection.Descending ); }
						set { SetState<SortDirection>( "AdminFAQsControl-SortDirection", value ); }
				}
				public string SortExpression
				{
						get { return GetState<string>( "AdminFAQsControl-SortExpression" ); }
						set { SetState<string>( "AdminFAQsControl-SortExpression", value ); }
				}

				public AdminFAQsControl()
				{
				}

				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						this.dataGrid = CreateGridControl();
						this.Controls.Add( this.dataGrid );

						//Binding
						this.dataGrid.DataSource = GetDataGridData();

						if ( !IsPostBack )
								this.dataGrid.DataBind();

				}
				#endregion

				public string NewUrl { get; set; }
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
						grid.MasterTableView.CommandItemSettings.AddNewRecordText = Resources.Controls.AdminFAQsControl_NewFAQ;

						GridBoundColumn bf = new GridBoundColumn();
						bf.DataField = "Order";
						bf.HeaderText = Resources.Controls.AdminFAQsControl_ColumnOrder;
						bf.SortExpression = "Order";
						bf.AutoPostBackOnFilter = true;
						bf.CurrentFilterFunction = GridKnownFunction.Contains;
						grid.Columns.Add( bf );

						bf = new GridBoundColumn();
						bf.DataField = "Question";
						bf.HeaderText = Resources.Controls.AdminFAQsControl_ColumnQuestion;
						bf.SortExpression = "Question";
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

				private List<FAQEntinty> GetDataGridData()
				{
						List<FAQEntinty> list = Storage<FAQEntinty>.Read();
						SortDirection previous = SortDirection;
						string sortExpression = String.IsNullOrEmpty( SortExpression ) ? "Order" : SortExpression;
						var ordered = list.AsQueryable().OrderBy( sortExpression + " " + ( previous == SortDirection.Ascending ? "ascending" : "descending" ) );
						return ordered.ToList();
				}

				#region Event handlers
				void OnRowCommand( object sender, GridCommandEventArgs e )
				{
						if ( e.CommandArgument == null )
								return;

						//Ak to je neznamy command, nebu se spracovavat tu.
						if ( e.CommandName != EDIT_COMMAND &&
								e.CommandName != DELETE_COMMAND &&
								e.CommandName != RadGrid.InitInsertCommandName )
								return;

						if ( e.CommandName == RadGrid.InitInsertCommandName )
						{
								string url = Page.ResolveUrl( string.Format( "{0}?{1}", this.NewUrl, base.BuildReturnUrlQueryParam() ) );
								Response.Redirect( url );
								return;
						}

						GridDataItem dataItem = (GridDataItem)e.Item;
						int faqId = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );

						if ( e.CommandName == EDIT_COMMAND )
						{
								string url = Page.ResolveUrl( string.Format( EditUrlFormat, faqId ) + "&" + base.BuildReturnUrlQueryParam() );
								Response.Redirect( url );
								return;
						}

						if ( e.CommandName == DELETE_COMMAND )
						{
								FAQEntinty news = Storage<FAQEntinty>.ReadFirst( new FAQEntinty.ReadById { FAQId = faqId } );
								Storage<FAQEntinty>.Delete( news );

								this.dataGrid.DataSource = GetDataGridData();
								this.dataGrid.DataBind();
								return;
						}
				}

				#endregion
		}
}
