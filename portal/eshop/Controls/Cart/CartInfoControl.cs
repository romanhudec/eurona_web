using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CartEntity = SHP.Entities.Cart;
using CMS.Controls;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CMS;
using CMS.Utilities;

namespace SHP.Controls.Cart
{
		public class CartInfoControl: CmsControl
		{
				/// <summary>
				/// Url Adresa detailu nákupného košika
				/// </summary>
				public string CartUrl { get; set; }

				protected override void CreateChildControls()
				{
						base.CreateChildControls();
						CartEntity cart = CartHelper.GetAccountCart( this.Page );
						Control ctrl = CreateControl( cart );
						if ( ctrl != null )
								this.Controls.Add( ctrl );

				}

				private Control CreateControl( CartEntity cart )
				{
						decimal price = cart != null ? (cart.PriceTotal.HasValue ? cart.PriceTotal.Value : 0m) : 0m;
						HtmlGenericControl div = new HtmlGenericControl( "div" );
						div.Attributes.Add( "class", this.CssClass );
						HtmlGenericControl span = new HtmlGenericControl( "span" );

						//Button Add cart
						HtmlGenericControl imgCart = new HtmlGenericControl( "a" );
						imgCart.Attributes.Add( "class", this.CssClass + "_cartImage" );
						span.Controls.Add( imgCart );

						// TextBox Cart
						TextBox txtPrice = new TextBox();
						txtPrice.CssClass = this.CssClass + "_inputPrice";
						txtPrice.Enabled = false;
						txtPrice.Text = Utilities.CultureUtilities.CurrencyInfo.ToString( price, this.Session );
						span.Controls.Add( txtPrice );

						div.Controls.Add( span );

						//Hyperlink
						HyperLink hlCartProductsDetail = new HyperLink();
						hlCartProductsDetail.NavigateUrl = string.Empty;
						hlCartProductsDetail.CssClass = this.CssClass + "_cartDetailLink";
						hlCartProductsDetail.Text = Resources.Controls.CartControl_ShowCart;

						if ( !string.IsNullOrEmpty( this.CartUrl ) )
						{
								//Check na URL Alias
								AliasUtilities aliasUtils = new AliasUtilities();
								string url = aliasUtils.Resolve( this.CartUrl, this.Page );
								hlCartProductsDetail.NavigateUrl = url;

						}
						div.Controls.Add( hlCartProductsDetail );

						return div;

				}
		}
}
