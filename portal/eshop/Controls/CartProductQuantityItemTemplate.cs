using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CartProductEntity = SHP.Entities.CartProduct;

namespace SHP.Controls
{
		public class CartProductQuantityItemTemplate: ITemplate
		{
				private TextBox txtQuantity = null;
				private ImageButton btnRefresh = null;
				#region ITemplate Members

				public delegate void RefreshEventHandler( int id, int quantity );
				public event RefreshEventHandler OnRefresh;

				public void InstantiateIn( Control container )
				{
						HtmlGenericControl div = new HtmlGenericControl( "div" );
						div.Attributes.Add( "class", "cpQuantityItemTemplate" );

						this.txtQuantity = new TextBox();
						this.txtQuantity.CssClass = "quantity";
						this.txtQuantity.DataBinding += new EventHandler( txtQuantity_DataBinding );
						this.txtQuantity.ID = "txtQuantity";
						div.Controls.Add( this.txtQuantity );

						this.btnRefresh = new ImageButton();
						this.btnRefresh.CssClass = "refresh";
						this.btnRefresh.Click += new ImageClickEventHandler( btnRefresh_Click );
						this.btnRefresh.DataBinding += new EventHandler( btnRefresh_DataBinding );
						div.Controls.Add( this.btnRefresh );

						container.Controls.Add( div );
				}

				void btnRefresh_Click( object sender, ImageClickEventArgs e )
				{
						ImageButton control = sender as ImageButton;
						GridViewRow row = (GridViewRow)control.NamingContainer;

						TextBox txt = (TextBox)row.FindControl( "txtQuantity" );
						if ( txt == null ) return;

						int id = 0;
						int quantity = 0;
						Int32.TryParse( control.CommandArgument, out id );
						Int32.TryParse( txt.Text, out quantity );

						if ( id == 0 ) return;
						if ( OnRefresh != null )
								OnRefresh( id, quantity );
				}

				void btnRefresh_DataBinding( object sender, EventArgs e )
				{
						ImageButton control = sender as ImageButton;
						GridViewRow row = (GridViewRow)control.NamingContainer;
						CartProductEntity cp = row.DataItem as CartProductEntity;

						string imgUrl = CMS.Utilities.ConfigUtilities.ConfigValue( "SHP:RefreshImage" );
						if ( !string.IsNullOrEmpty( imgUrl ) )
								control.ImageUrl = control.Page.ResolveUrl( imgUrl );
						control.CommandArgument = cp.Id.ToString();
						control.CommandName = "UpdateCartProductQuantity";
				}

				void txtQuantity_DataBinding( object sender, EventArgs e )
				{
						TextBox control = sender as TextBox;
						GridViewRow row = (GridViewRow)control.NamingContainer;
						CartProductEntity cp = row.DataItem as CartProductEntity;
						control.Text = cp.Quantity.ToString();
				}

				#endregion
		}
}
