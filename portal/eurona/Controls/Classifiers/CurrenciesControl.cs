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
using CurrencyEntity = SHP.Entities.Classifiers.Currency;
using Telerik.Web.UI;

namespace Eurona.Controls.Classifiers
{
		public class CurrenciesControl: ClassifiersControl<CurrencyEntity>
		{
				public CurrenciesControl()
				{
				}

				/// <summary>
				/// Vytvori DataGrid control s pozadovanymi stlpcami a 
				/// pripravenym bindingom stlpcou.
				/// </summary>
				protected override RadGrid CreateGridControl()
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
						grid.MasterTableView.CommandItemSettings.AddNewRecordText = global::SHP.Resources.Controls.ClassifiersControl_AddItem;

						GridBoundColumn bf = new GridBoundColumn();
						bf.DataField = "Name";
						bf.HeaderText = global::SHP.Resources.Controls.ClassifiersControl_ColumnName;
						bf.SortExpression = "Name";
						bf.AutoPostBackOnFilter = true;
						bf.CurrentFilterFunction = GridKnownFunction.Contains;
						grid.Columns.Add( bf );

						bf = new GridBoundColumn();
						bf.DataField = "Rate";
						bf.HeaderText = global::SHP.Resources.Controls.ClassifiersControl_ColumnRate;
						bf.SortExpression = "Rate";
						bf.AutoPostBackOnFilter = true;
						bf.CurrentFilterFunction = GridKnownFunction.Contains;
						grid.Columns.Add( bf );

						bf = new GridBoundColumn();
						bf.DataField = "Symbol";
						bf.HeaderText = global::SHP.Resources.Controls.ClassifiersControl_ColumnSymbol;
						bf.SortExpression = "Symbol";
						bf.AutoPostBackOnFilter = true;
						bf.CurrentFilterFunction = GridKnownFunction.Contains;
						grid.Columns.Add( bf );

						GridIconColumn ic = new GridIconColumn();
						ic.DataImageUrlFields = new string[] { "Icon" };
						ic.ImageWidth = Unit.Pixel( 16 );
						ic.ImageHeight = Unit.Pixel( 16 );
						ic.HeaderText = global::SHP.Resources.Controls.ClassifiersControl_ColumnIcon;
						ic.ShowFilterIcon = false;
						grid.Columns.Add( ic );

						bf = new GridBoundColumn();
						bf.DataField = "Notes";
						bf.HeaderText = global::SHP.Resources.Controls.ClassifiersControl_ColumnNotes;
						bf.SortExpression = "Notes";
						grid.Columns.Add( bf );

						GridButtonColumn btnEdit = new GridButtonColumn();
						btnEdit.HeaderStyle.Width = new Unit( 16, UnitType.Pixel );
						btnEdit.ImageUrl = ConfigValue( "CMS:EditButtonImage" );
						btnEdit.Text = global::SHP.Resources.Controls.GridView_ToolTip_EditItem;
						btnEdit.ButtonType = GridButtonColumnType.ImageButton;
						btnEdit.CommandName = EDIT_COMMAND;
						grid.MasterTableView.Columns.Add( btnEdit );

						GridButtonColumn btnDelete = new GridButtonColumn();
						btnDelete.HeaderStyle.Width = new Unit( 16, UnitType.Pixel );
						btnDelete.ImageUrl = ConfigValue( "CMS:DeleteButtonImage" );
						btnDelete.Text = global::SHP.Resources.Controls.GridView_ToolTip_DeleteItem;
						btnDelete.ButtonType = GridButtonColumnType.ImageButton;
						btnDelete.ConfirmTitle = global::SHP.Resources.Controls.DeleteItemQuestion;
						btnDelete.ConfirmText = global::SHP.Resources.Controls.DeleteItemQuestion;
						btnDelete.CommandName = DELETE_COMMAND;
						grid.MasterTableView.Columns.Add( btnDelete );

						grid.ItemCommand += OnRowCommand;

						return grid;
				}
		}
}
