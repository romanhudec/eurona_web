using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShpResources = SHP.Resources.Controls;
using ShpCultureUtilities = SHP.Utilities.CultureUtilities;
using ShpProductHighlights = SHP.Entities.ProductHighlights;
using System.IO;
using Eurona.DAL.Entities;
using SHP.Controls.Cart;
using System.Web.UI.HtmlControls;
using System.Text;

namespace Eurona.EShop
{
		public partial class HighlightsProductsControl: Eurona.Controls.UserControl
		{
				protected void Page_Load( object sender, EventArgs e )
				{

				}
				/// <summary>
				/// Maximalny pocet produktov, ktory sa ma zobrazovat
				/// </summary>
				public int? MaxProductsCount
				{
						get
						{
								return this.highlightsProductsControl.MaxProductsCount;
						}
						set
						{
								this.highlightsProductsControl.MaxProductsCount = value;
						}
				}
				/// <summary>
				/// Id zvyraznenia, ktore urci ake produkty s akym zvyraznenim sa budu zobrazovat
				/// </summary>
				public int? HighlightId
				{
						get
						{
								return this.highlightsProductsControl.HighlightId;
						}
						set
						{
								this.highlightsProductsControl.HighlightId = value;
						}
				}

				public string GetShpResourceString( string key )
				{
						return ShpResources.ResourceManager.GetString( key );
				}
				public string RenderImage( int productId )
				{
						string storageUrl = CMS.Utilities.ConfigUtilities.ConfigValue( "SHP:ImageGallery:Product:StoragePath", this.Page );

						string storagePath = string.Format( "{0}{1}/", storageUrl, productId );
						string productImagesPath = this.Server.MapPath( storagePath );

						if ( !Directory.Exists( productImagesPath ) )
								return null;

						DirectoryInfo di = new DirectoryInfo( productImagesPath );
						FileInfo[] fileInfos = di.GetFiles( "*.*" );

						if ( fileInfos.Length == 0 )
								return null;

						string urlThumbnail = storagePath + "_t/" + fileInfos[0].Name;
						string img = string.Format( "<img src='{0}'>", this.ResolveUrl( urlThumbnail ) );

						return img;
				}

				public string RenderHighlightImage( int productId )
				{
						StringBuilder sb = new StringBuilder();
						List<ShpProductHighlights> list = Storage<ShpProductHighlights>.Read( new ShpProductHighlights.ReadByProduct { ProductId = productId, HighlightId = this.HighlightId } );
						foreach ( ShpProductHighlights ph in list )
						{
								string img = string.Format( "<img src='{0}' alt='{1}'>", this.ResolveUrl( ph.Icon ), ph.Name );
								sb.Append( img );
						}
						return sb.ToString();
				}

				public string GetProductOldPrice( decimal discount, decimal price )
				{
						if ( discount == 0 ) return string.Empty;
						return ShpCultureUtilities.CurrencyInfo.ToString( price, this.Session );
				}

				public string GetProductPrice( decimal price )
				{
						string text = ShpCultureUtilities.CurrencyInfo.ToString( price, this.Session );
						return text;
				}

				protected void OnAddCart( object sender, EventArgs e )
				{
						Button btn = ( sender as Button );
						if ( string.IsNullOrEmpty( btn.CommandArgument ) )
								return;
						int quantity = 1;
						int productId = 0;

						TextBox txtQuantity = (TextBox)btn.Parent.FindControl( "txtQuantity" );

						Int32.TryParse( txtQuantity.Text, out quantity );
						Int32.TryParse( btn.CommandArgument, out productId );
						CartHelper.AddProductToCart( this.Page, productId, quantity );

						//Alert s informaciou o pridani do nakupneho kosika
						string js = string.Format( "alert('{0}');",
							 string.Format( ShpResources.AdminProductControl_ProductWasAddedToCart_Message, btn.CommandName, quantity ) );
						this.Page.ClientScript.RegisterStartupScript( this.Page.GetType(), "addProductToCart", js, true );
				}
		}
}