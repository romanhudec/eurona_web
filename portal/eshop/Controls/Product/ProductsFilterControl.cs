using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProductEntity = SHP.Entities.Product;

namespace SHP.Controls.Product
{
		public class ProductsFilterControl: CMS.Controls.CmsControl
		{
				public delegate void FilterEventHandler( ProductEntity.ReadByFilter filter );
				public event FilterEventHandler OnFilter;

				private TextBox txtExpression;
				private TextBox txtManufacturer;
				private TextBox txtPriceFrom;
				private TextBox txtPriceTo;
				private DropDownList ddlSortBy;

				private Button btnFilter = null;
				private Button btnCancel = null;

				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						Control filterControl = CreateFilterControl();
						if ( filterControl != null )
								this.Controls.Add( filterControl );

						string onEnterFindJSFormat = "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + this.btnFilter.ClientID + "').click();return false;}} else {return true}; ";
						this.txtExpression.Attributes.Add( "onkeydown", onEnterFindJSFormat );
						this.txtManufacturer.Attributes.Add( "onkeydown", onEnterFindJSFormat );
						this.txtPriceFrom.Attributes.Add( "onkeydown", onEnterFindJSFormat );
						this.txtPriceTo.Attributes.Add( "onkeydown", onEnterFindJSFormat );

						//Binding
						DataBindCarControl();
				}

				public ProductEntity.ReadByFilter Filter
				{
						get
						{
								EnsureChildControls();

								ProductEntity.ReadByFilter readByFilter = new ProductEntity.ReadByFilter();
								readByFilter.Expression = this.txtExpression.Text;
								readByFilter.Manufacturer = this.txtManufacturer.Text;
								readByFilter.PriceFrom = DecimalFromString( this.txtPriceFrom.Text );
								readByFilter.PriceTo = DecimalFromString( this.txtPriceTo.Text );
								readByFilter.SortBy = (ProductEntity.SortBy)Convert.ToInt32( this.ddlSortBy.SelectedValue );

								return readByFilter;
						}
				}

				/// <summary>
				/// Vytvori Control Adresy
				/// </summary>
				private Control CreateFilterControl()
				{
						#region Create controls
						this.txtExpression = new TextBox();
						this.txtExpression.ID = "txtExpression";
						this.txtExpression.Width = Unit.Pixel( 150 );

						this.txtManufacturer = new TextBox();
						this.txtManufacturer.ID = "txtManufacturer";
						this.txtManufacturer.Width = Unit.Pixel( 100 );

						this.txtPriceFrom = new TextBox();
						this.txtPriceFrom.ID = "txtPriceFrom";
						this.txtPriceFrom.Width = Unit.Pixel( 80 );

						this.txtPriceTo = new TextBox();
						this.txtPriceTo.ID = "txtPriceTo";
						this.txtPriceTo.Width = Unit.Pixel( 80 );

						this.ddlSortBy = new DropDownList();
						this.ddlSortBy.ID = "ddlSortBy";
						this.ddlSortBy.Width = Unit.Pixel( 200 );

						#endregion

						this.btnFilter = new Button();
						this.btnFilter.ID = "btnFilter";
						this.btnFilter.Width = Unit.Pixel( 60 );
						this.btnFilter.CausesValidation = true;
						this.btnFilter.Text = Resources.Controls.FilterControl_FindButtonText;
						this.btnFilter.Click += new EventHandler( OnBtnFilter );
						this.btnCancel = new Button();
						this.btnCancel.Width = Unit.Pixel( 60 );
						this.btnCancel.CausesValidation = false;
						this.btnCancel.Text = Resources.Controls.FilterControl_CancelFindButtonText;
						this.btnCancel.Click += new EventHandler( OnBtnCancel );

						Table table = new Table();
						table.CssClass = this.CssClass;
						//table.Attributes.Add( "border", "1px" );
						table.Width = Unit.Percentage( 100 );
						table.Height = this.Height;

						#region Add control to table
						#region First row labels
						//Expression
						TableRow row = new TableRow();
						TableCell cell = new TableCell();
						cell.VerticalAlign = VerticalAlign.Bottom;
						cell.Font.Bold = true;
						cell.Text = Resources.Controls.ProductsFilterControl_Expression;
						row.Cells.Add( cell );

						//Manufacturer
						cell = new TableCell();
						cell.VerticalAlign = VerticalAlign.Bottom;
						cell.Font.Bold = true;
						cell.Text = Resources.Controls.ProductsFilterControl_Manufacturer;
						row.Cells.Add( cell );

						//Price From
						cell = new TableCell();
						cell.VerticalAlign = VerticalAlign.Bottom;
						cell.Font.Bold = true;
						cell.Text = Resources.Controls.ProductsFilterControl_PriceFrom;
						row.Cells.Add( cell );

						//Price To
						cell = new TableCell();
						cell.VerticalAlign = VerticalAlign.Bottom;
						cell.Font.Bold = true;
						cell.Text = Resources.Controls.ProductsFilterControl_PriceTo;
						row.Cells.Add( cell );

						//Sort By
						cell = new TableCell();
						cell.VerticalAlign = VerticalAlign.Bottom;
						cell.Font.Bold = true;
						cell.Text = Resources.Controls.ProductsFilterControl_Sort;
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.VerticalAlign = VerticalAlign.Bottom;
						cell.HorizontalAlign = HorizontalAlign.Right;
						cell.Width = Unit.Percentage( 100 );
						cell.Controls.Add( this.btnFilter );
						row.Cells.Add( cell );

						table.Rows.Add( row );
						#endregion

						#region 2 row Controls
						row = new TableRow();

						//txtExpression
						cell = new TableCell();
						cell.VerticalAlign = VerticalAlign.Top;
						cell.Controls.Add( this.txtExpression );
						row.Cells.Add( cell );

						//txtManufacturer
						cell = new TableCell();
						cell.VerticalAlign = VerticalAlign.Top;
						cell.Controls.Add( this.txtManufacturer );
						row.Cells.Add( cell );

						//txtPriceFrom
						cell = new TableCell();
						cell.VerticalAlign = VerticalAlign.Top;
						cell.Controls.Add( this.txtPriceFrom );
						row.Cells.Add( cell );

						//txtPriceTo
						cell = new TableCell();
						cell.VerticalAlign = VerticalAlign.Top;
						cell.Controls.Add( this.txtPriceTo );
						row.Cells.Add( cell );

						//ddlSortBy
						cell = new TableCell();
						cell.VerticalAlign = VerticalAlign.Top;
						cell.Controls.Add( this.ddlSortBy );
						row.Cells.Add( cell );

						cell = new TableCell();
						cell.VerticalAlign = VerticalAlign.Bottom;
						cell.HorizontalAlign = HorizontalAlign.Right;
						cell.Width = Unit.Percentage( 100 );
						cell.Controls.Add( this.btnCancel );
						row.Cells.Add( cell );

						table.Rows.Add( row );
						#endregion
						#endregion

						return table;
				}

				private void DataBindCarControl()
				{
						if ( !IsPostBack )
						{
								this.ddlSortBy.Items.Add( new ListItem( Resources.Controls.ProductsFilterControl_SortBy_None_Label, Convert.ToString( (int)ProductEntity.SortBy.Default ) ) );
								this.ddlSortBy.Items.Add( new ListItem( Resources.Controls.ProductsFilterControl_SortBy_PriceASC_Label, Convert.ToString( (int)ProductEntity.SortBy.PriceASC ) ) );
								this.ddlSortBy.Items.Add( new ListItem( Resources.Controls.ProductsFilterControl_SortBy_PriceDESC_Label, Convert.ToString( (int)ProductEntity.SortBy.PriceDESC ) ) );
								this.ddlSortBy.Items.Add( new ListItem( Resources.Controls.ProductsFilterControl_SortBy_NameASC_Label, Convert.ToString( (int)ProductEntity.SortBy.NameASC ) ) );

								this.ddlSortBy.DataBind();
						}
				}

				void OnBtnFilter( object sender, EventArgs e )
				{
						if ( OnFilter != null )
								OnFilter( this.Filter );
				}

				public static decimal? DecimalFromString( string strInt )
				{
						if ( string.IsNullOrEmpty( strInt ) )
								return null;

						decimal value = -1;
						Decimal.TryParse( strInt, out value );
						return value < 0 ? (decimal?)null : value;
				}

				void OnBtnCancel( object sender, EventArgs e )
				{
						this.txtExpression.Text = string.Empty;
						this.txtManufacturer.Text = string.Empty;
						this.txtPriceFrom.Text = string.Empty;
						this.txtPriceTo.Text = string.Empty;
						this.ddlSortBy.SelectedIndex = 0;

						if ( OnFilter != null )
								OnFilter( null );
				}
		}
}
