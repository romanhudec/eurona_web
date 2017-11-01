using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CategoryEntity = SHP.Entities.Category;
using ProductEntity = Eurona.Common.DAL.Entities.Product;
using System.IO;
using System.Text;

namespace Eurona.EShop
{
		public partial class Default: WebPage
		{
				private List<Banner> banners = null;
				/*
				protected void Page_Load( object sender, EventArgs e )
				{
						//this.Title = this.genericPage.Title;

						string bannerPath = Server.MapPath( "~/userfiles/eshop/exclusiv-banner/" );
						if ( Directory.Exists( bannerPath ) )
						{
								string[] files = Directory.GetFiles( bannerPath );
								List<Banner> banners = new List<Banner>();
								foreach ( string file in files )
								{
										string fileName = Path.GetFileName( file );
										string productCode = Path.GetFileNameWithoutExtension( file );

										Banner banner = new Banner();
										banner.FilePath = file;
										banner.ImageUrl = Page.ResolveUrl( string.Format( "~/userfiles/eshop/exclusiv-banner/{0}", fileName ) );
										//ProductEntity product = Storage<ProductEntity>.ReadFirst( new ProductEntity.ReadByCode { Code = productCode } );
										//if ( product != null ) banner.NavigateUrl = Page.ResolveUrl( product.Alias ) + "?ReturnUrl=/";

										banners.Add( banner );
								}
								this.radRotator.DataSource = banners;
								this.radRotator.DataBind();
						}
				}
				*/
				public List<Banner> Baners
				{
						get
						{
								if ( this.banners != null ) return this.banners;

								this.banners = new List<Banner>();
								string bannerPath = Server.MapPath( "~/userfiles/eshop/exclusiv-banner/" );
								if ( Directory.Exists( bannerPath ) )
								{
										string[] files = Directory.GetFiles( bannerPath );
										foreach ( string file in files )
										{
												string fileName = Path.GetFileName( file );
												string productCode = Path.GetFileNameWithoutExtension( file );

												Banner banner = new Banner();
												banner.FilePath = file;
												banner.ImageUrl = Page.ResolveUrl( string.Format( "~/userfiles/eshop/exclusiv-banner/{0}", fileName ) );
												//Eurona.Common.DAL.Entities.Product product = Storage<Eurona.Common.DAL.Entities.Product>.ReadFirst( new Eurona.Common.DAL.Entities.Product.ReadByCode { Code = productCode } );
												//if ( product != null ) banner.NavigateUrl = Page.ResolveUrl( product.Alias ) + "?ReturnUrl=/";
												this.banners.Add( banner );
										}
								}
								return this.banners;

						}
				}

				protected string RenderFlash()
				{
						//Rotator databinding
						StringBuilder sbDiv = new StringBuilder();
						int index = 0;
						foreach ( Banner banner in this.Baners )
						{
								sbDiv.AppendFormat( "<div class='pane pane{0}'><img alt='' src='{1}'/><a href=''></a></div>", index, banner.ImageUrl );
								index++;
						}
						return sbDiv.ToString();
				}
				protected string RenderButtons()
				{
						//Rotator databinding
						StringBuilder sbTd = new StringBuilder();
						int index = 0;
						foreach ( Banner banner in this.Baners )
						{
								sbTd.AppendFormat( "<td align='center' class='dojoxRotatorNumber dojoxRotatorPane{0} dojoxRotatorFirst' dojoattrid='{0}'><a href='#'><span>{1}</span></a></td>", index, index + 1 );
								index++;
						}
						return sbTd.ToString();
				}
				public class Banner
				{
						public string FilePath { get; set; }
						public string ImageUrl { get; set; }
						public string NavigateUrl { get; set; }
				}
		}
}
