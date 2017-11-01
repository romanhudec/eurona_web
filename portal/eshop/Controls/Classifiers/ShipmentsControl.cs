using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Controls;
using SHP.Entities.Classifiers;
using System.Web.UI.HtmlControls;
using ShipmentEntity = SHP.Entities.Classifiers.Shipment;
using Telerik.Web.UI;

namespace SHP.Controls.Classifiers
{
		public class ShipmentsControl: CMS.Controls.CmsControl
		{
				protected const string DELETE_COMMAND = "DELETE_ITEM";
				protected const string EDIT_COMMAND = "EDIT_ITEM";

				protected RadGrid dataGrid = null;

				public SortDirection SortDirection
				{
						get { return GetSession<SortDirection>( "ShipmentsControl-SortDirection", SortDirection.Ascending ); }
						set { SetSession<SortDirection>( "ShipmentsControl-SortDirection", value ); }
				}
				public string SortExpression
				{
						get { return GetSession<string>( "ShipmentsControl-SortExpression" ); }
						set { SetSession<string>( "ShipmentsControl-SortExpression", value ); }
				}

				public ShipmentsControl()
				{
				}

				#region Protected overrides
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						Control clsControl = CreateClassifiersControl();
						if ( clsControl != null ) this.Controls.Add( clsControl );
						else return;

						//Binding
						this.dataGrid.DataSource = GetDataGridData();
						if ( !IsPostBack )
						{
								this.dataGrid.MasterTableView.DataKeyNames = new string[] { "Id" };
								this.dataGrid.DataBind();
						}

				}
				#endregion

				protected ShipmentEntity ReadFirst( object criteria )
				{
						return Storage<ShipmentEntity>.Read( criteria )[0];
				}

				public string NewUrl { get; set; }
				public string EditUrlFormat { get; set; }

				/// <summary>
				/// Vytvori Control zoznamu poloziek ciselnika
				/// </summary>
				protected virtual Control CreateClassifiersControl()
				{
						HtmlGenericControl div = new HtmlGenericControl( "div" );

						//DataGrid
						this.dataGrid = CreateGridControl();
						div.Controls.Add( this.dataGrid );
						return div;
				}

				/// <summary>
				/// Vytvori DataGrid control s pozadovanymi stlpcami a 
				/// pripravenym bindingom stlpcou.
				/// </summary>
				protected virtual RadGrid CreateGridControl()
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
						grid.MasterTableView.CommandItemSettings.AddNewRecordText = Resources.Controls.ClassifiersControl_AddItem;

						GridBoundColumn bf = new GridBoundColumn();
						bf.DataField = "Name";
						bf.HeaderText = Resources.Controls.ClassifiersControl_ColumnName;
						bf.SortExpression = "Name";
						bf.AutoPostBackOnFilter = true;
						bf.CurrentFilterFunction = GridKnownFunction.Contains;
						grid.Columns.Add( bf );

						bf = new GridPriceColumn();
						bf.DataField = "Price";
						bf.HeaderText = Resources.Controls.ClassifiersControl_ColumnPrice;
						bf.SortExpression = "Price";
						bf.AutoPostBackOnFilter = true;
						bf.CurrentFilterFunction = GridKnownFunction.Contains;
						grid.Columns.Add( bf );

						bf = new GridBoundColumn();
						bf.DataField = "VATDisplay";
						bf.HeaderText = Resources.Controls.ClassifiersControl_ColumnVAT;
						bf.SortExpression = "VATDisplay";
						bf.AutoPostBackOnFilter = true;
						bf.CurrentFilterFunction = GridKnownFunction.Contains;
						grid.Columns.Add( bf );

						bf = new GridPriceColumn();
						bf.DataField = "PriceWVAT";
						bf.DataFormatString = "{0:F2}";
						bf.HeaderText = Resources.Controls.ClassifiersControl_ColumnPriceWithVAT;
						bf.SortExpression = "PriceWVAT";
						bf.AutoPostBackOnFilter = true;
						bf.CurrentFilterFunction = GridKnownFunction.Contains;
						grid.Columns.Add( bf );

						GridIconColumn ic = new GridIconColumn();
						ic.DataImageUrlFields = new string[] { "Icon" };
						ic.ImageWidth = Unit.Pixel( 16 );
						ic.ImageHeight = Unit.Pixel( 16 );
						ic.HeaderText = Resources.Controls.ClassifiersControl_ColumnIcon;
						ic.ShowFilterIcon = false;
						grid.Columns.Add( ic );

						bf = new GridBoundColumn();
						bf.DataField = "Notes";
						bf.HeaderText = Resources.Controls.ClassifiersControl_ColumnNotes;
						bf.SortExpression = "Notes";
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

				protected virtual List<ShipmentEntity> GetDataGridData()
				{
						List<ShipmentEntity> list = Storage<ShipmentEntity>.Read( null );

						SortDirection previous = SortDirection;
						string sortExpression = String.IsNullOrEmpty( SortExpression ) ? "Name" : SortExpression;
						var ordered = list.AsQueryable().OrderBy( sortExpression + " " + ( previous == SortDirection.Ascending ? "ascending" : "descending" ) );
						return ordered.ToList();
				}

				#region Event handlers
				protected void OnRowCommand( object sender, GridCommandEventArgs e )
				{
						if ( e.CommandName == RadGrid.InitInsertCommandName ) OnNewCommand( sender, e );
						if ( e.CommandName == EDIT_COMMAND ) OnEditCommand( sender, e );
						if ( e.CommandName == DELETE_COMMAND ) OnDeleteCommand( sender, e );
				}
				protected void OnNewCommand( object sender, GridCommandEventArgs e )
				{
						string url = Page.ResolveUrl( string.Format( "{0}{1}{2}", this.NewUrl,
								( this.NewUrl.Contains( "?" ) ? "&" : "?" ),
								base.BuildReturnUrlQueryParam() ) );

						Response.Redirect( url );
				}
				protected void OnEditCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int id = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );

						string url = Page.ResolveUrl( string.Format( EditUrlFormat, id ) + "&" + base.BuildReturnUrlQueryParam() );
						Response.Redirect( url );
				}

				protected void OnDeleteCommand( object sender, GridCommandEventArgs e )
				{
						GridDataItem dataItem = (GridDataItem)e.Item;
						int id = Convert.ToInt32( dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"] );

						ShipmentEntity classifier = ReadFirst( new ShipmentEntity.ReadById { Id = id } );
						Storage<ShipmentEntity>.Delete( classifier );

						this.dataGrid.DataSource = GetDataGridData();
						this.dataGrid.DataBind();
				}
				#endregion
		}
}
