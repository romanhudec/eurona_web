using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProductEntity = SHP.Entities.Product;

namespace Eurona.EShop
{
		public partial class ProductsFilter: System.Web.UI.UserControl
		{
				public delegate void FilterEventHandler( ProductEntity.ReadByFilter filter );
				public event FilterEventHandler OnFilter;

				protected void Page_Load( object sender, EventArgs e )
				{
						string onEnterFindJSFormat = "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + this.btnFilter.ClientID + "').click();return false;}} else {return true}; ";
						this.txtExpression.Attributes.Add( "onkeydown", onEnterFindJSFormat );
						this.txtManufacturer.Attributes.Add( "onkeydown", onEnterFindJSFormat );
						this.txtPriceFrom.Attributes.Add( "onkeydown", onEnterFindJSFormat );
						this.txtPriceTo.Attributes.Add( "onkeydown", onEnterFindJSFormat );

						//Binding
						DataBindCarControl();
				}

				private void DataBindCarControl()
				{
						if ( !IsPostBack )
						{
								this.ddlSortBy.Items.Add( new ListItem( global::SHP.Resources.Controls.ProductsFilterControl_SortBy_None_Label, Convert.ToString( (int)ProductEntity.SortBy.Default ) ) );
								this.ddlSortBy.Items.Add( new ListItem( global::SHP.Resources.Controls.ProductsFilterControl_SortBy_PriceASC_Label, Convert.ToString( (int)ProductEntity.SortBy.PriceASC ) ) );
								this.ddlSortBy.Items.Add( new ListItem( global::SHP.Resources.Controls.ProductsFilterControl_SortBy_PriceDESC_Label, Convert.ToString( (int)ProductEntity.SortBy.PriceDESC ) ) );
								this.ddlSortBy.Items.Add( new ListItem( global::SHP.Resources.Controls.ProductsFilterControl_SortBy_NameASC_Label, Convert.ToString( (int)ProductEntity.SortBy.NameASC ) ) );

								this.ddlSortBy.DataBind();
						}
				}

				public ProductEntity.ReadByFilter Filter
				{
						get
						{
								EnsureChildControls();

								ProductEntity.ReadByFilter readByFilter = new ProductEntity.ReadByFilter();
								readByFilter.Expression = this.txtExpression.Text;
								readByFilter.Manufacturer = this.txtManufacturer.Text;
								readByFilter.PriceFrom = global::SHP.Controls.Product.ProductsFilterControl.DecimalFromString( this.txtPriceFrom.Text );
								readByFilter.PriceTo = global::SHP.Controls.Product.ProductsFilterControl.DecimalFromString( this.txtPriceTo.Text );
								readByFilter.SortBy = (ProductEntity.SortBy)Convert.ToInt32( this.ddlSortBy.SelectedValue );

								return readByFilter;
						}
				}

				protected void btnFilter_Click( object sender, EventArgs e )
				{
						if ( OnFilter != null )
								OnFilter( this.Filter );
				}

				protected void btnCancelFilter_Click( object sender, EventArgs e )
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