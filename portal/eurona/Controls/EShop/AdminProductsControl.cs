using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.UI.WebControls;
using ProductEntity = Eurona.Common.DAL.Entities.Product;
using CMS.Controls;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using Eurona.Common;

namespace Eurona.Controls.Product
{
	public class AdminProductsControl : CmsControl
	{
		private const string EDIT_COMMAND = "EDIT_ITEM";

		private RadGrid gridView;

		public string EditUrlFormat { get; set; }

		public SortDirection SortDirection
		{
			get { return GetSession<SortDirection>("AdminProductsControl-SortDirection", SortDirection.Ascending); }
			set { SetSession<SortDirection>("AdminProductsControl-SortDirection", value); }
		}

		public string SortExpression
		{
			get { return GetSession<string>("AdminProductsControl-SortExpression", "Order"); }
			set { SetSession<string>("AdminProductsControl-SortExpression", value); }
		}

		protected override void CreateChildControls()
		{
			base.CreateChildControls();
			gridView = CreateGridView();
			this.Controls.Add(gridView);
			GridViewDataBind(!IsPostBack);
		}

		private void GridViewDataBind(bool bind)
		{
			List<ProductEntity> products = Storage<ProductEntity>.Read();

			var ordered = products.AsQueryable().OrderBy(SortExpression + " " + SortDirection);
			gridView.DataSource = ordered.ToList();
			if (bind) gridView.DataBind();
		}

		private RadGrid CreateGridView()
		{
			RadGrid grid = new RadGrid();
			CMS.Utilities.RadGridUtilities.Localize(grid);
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

			grid.MasterTableView.CommandItemDisplay = GridCommandItemDisplay.None;
			//grid.MasterTableView.CommandItemSettings.AddNewRecordText = SHP.Resources.Controls.AdminProductsControl_NewProductButton_Text;

			grid.MasterTableView.Columns.Add(new GridBoundColumn
			{
				DataField = "Code",
				HeaderText = SHP.Resources.Controls.AdminProductsControl_ColumnCode,
				SortExpression = "Code",
				AutoPostBackOnFilter = true,
				CurrentFilterFunction = GridKnownFunction.Contains
			});
			grid.MasterTableView.Columns.Add(new GridBoundColumn
			{
				DataField = "Name",
				HeaderText = SHP.Resources.Controls.AdminProductsControl_ColumnName,
				SortExpression = "Name",
				AutoPostBackOnFilter = true,
				CurrentFilterFunction = GridKnownFunction.Contains
			});
			grid.MasterTableView.Columns.Add(new Eurona.Common.Controls.GridPriceColumn
			{
				DataField = "Price",
				CurrencySymbolDataField = "CurrencySymbol",
				HeaderText = SHP.Resources.Controls.AdminProductsControl_ColumnPrice,
				SortExpression = "Price",
				AutoPostBackOnFilter = true,
				CurrentFilterFunction = GridKnownFunction.Contains
			});

			if (Security.IsInRole(CMS.Entities.Role.ADMINISTRATOR))
			{
                grid.MasterTableView.Columns.Add(new GridBoundColumn {
                    DataField = "MinimalniPocetVBaleni",
                    HeaderText = "Min. počet",
                    SortExpression = "MinimalniPocetVBaleni",
                    AutoPostBackOnFilter = true,
                    CurrentFilterFunction = GridKnownFunction.Contains
                });

				grid.MasterTableView.Columns.Add(new GridBoundColumn
				{
					DataField = "MaximalniPocetVBaleni",
					HeaderText = "Max. počet",
					SortExpression = "MaximalniPocetVBaleni",
					AutoPostBackOnFilter = true,
					CurrentFilterFunction = GridKnownFunction.Contains
				});
			
				grid.MasterTableView.Columns.Add(new GridBoundColumn
				{
					DataField = "VamiNejviceNakupovane",
					HeaderText = "Vámi nejvice nakupované",
					SortExpression = "VamiNejviceNakupovane",
					AutoPostBackOnFilter = true,
					CurrentFilterFunction = GridKnownFunction.Contains
				});

                //grid.MasterTableView.Columns.Add(new GridBoundColumn
                //{
                //    DataField = "DarkovySet",
                //    HeaderText = "Dárkový set",
                //    SortExpression = "DarkovySet",
                //    AutoPostBackOnFilter = true,
                //    CurrentFilterFunction = GridKnownFunction.Contains
                //});
                grid.MasterTableView.Columns.Add(new GridCheckBoxColumn {
                    DataField = "BSR",
                    HeaderText = "BSR Produkt",
                    SortExpression = "BSR",
                    AutoPostBackOnFilter = true,
                    CurrentFilterFunction = GridKnownFunction.Contains
                });

				grid.MasterTableView.Columns.Add(new GridBoundColumn
				{
					DataField = "InternalStorageCount",
					HeaderText = "Interní stav skladu",
					SortExpression = "InternalStorageCount",
					AutoPostBackOnFilter = true,
					CurrentFilterFunction = GridKnownFunction.Contains
				});

                //grid.MasterTableView.Columns.Add(new GridBoundColumn
                //{
                //    DataField = "StorageCount",
                //    HeaderText = "Stav skladu K2",
                //    SortExpression = "StorageCount",
                //    AutoPostBackOnFilter = true,
                //    CurrentFilterFunction = GridKnownFunction.Contains
                //});
                grid.MasterTableView.Columns.Add(new GridBoundColumn {
                    DataField = "Order",
                    HeaderText = "Pořadí",
                    SortExpression = "Order",
                    AutoPostBackOnFilter = true,
                    CurrentFilterFunction = GridKnownFunction.Contains
                });


				grid.MasterTableView.Columns.Add(new GridBoundColumn
				{
					DataField = "LimitDate",
					HeaderText = "Datum odpočtu",
					SortExpression = "LimitDate",
					AutoPostBackOnFilter = true,
					CurrentFilterFunction = GridKnownFunction.Contains
				});
			}
			GridButtonColumn btnEdit = new GridButtonColumn();
			btnEdit.HeaderStyle.Width = new Unit(16, UnitType.Pixel);
			btnEdit.ImageUrl = ConfigValue("CMS:EditButtonImage");
			btnEdit.Text = SHP.Resources.Controls.GridView_ToolTip_EditItem;
			btnEdit.ButtonType = GridButtonColumnType.ImageButton;
			btnEdit.CommandName = EDIT_COMMAND;
			grid.MasterTableView.Columns.Add(btnEdit);

			//GridButtonColumn btnDelete = new GridButtonColumn();
			//btnDelete.HeaderStyle.Width = new Unit( 16, UnitType.Pixel );
			//btnDelete.ImageUrl = ConfigValue( "CMS:DeleteButtonImage" );
			//btnDelete.Text = Resources.Controls.GridView_ToolTip_DeleteItem;
			//btnDelete.ButtonType = GridButtonColumnType.ImageButton;
			//btnDelete.ConfirmTitle = Resources.Controls.DeleteItemQuestion;
			//btnDelete.ConfirmText = Resources.Controls.DeleteItemQuestion;
			//btnDelete.CommandName = DELETE_COMMAND;
			//grid.MasterTableView.Columns.Add( btnDelete );

			grid.ItemCommand += OnRowCommand;

			return grid;
		}

		void OnRowCommand(object source, GridCommandEventArgs e)
		{
			if (e.CommandName == EDIT_COMMAND) OnEditCommand(source, e);
		}
		private void OnEditCommand(object sender, GridCommandEventArgs e)
		{
			GridDataItem dataItem = (GridDataItem)e.Item;
			int productId = Convert.ToInt32(dataItem.OwnerTableView.DataKeyValues[dataItem.ItemIndex]["Id"]);
			Response.Redirect(String.Format(EditUrlFormat, productId) + "&" + base.BuildReturnUrlQueryParam());
		}
	}
}
