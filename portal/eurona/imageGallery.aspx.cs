using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Controls;

namespace Eurona
{
		public partial class ImageGalleryPage: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						if ( !string.IsNullOrEmpty( Request["id"] ) )
								this.imageGalleryItemsControl.ImageGalleryId = Convert.ToInt32( Request["id"] );

						//Nastavenie spravnej navratovej url adresy 
						// je to koli tomu, ze ak je galeria zobrazena z filtrovaneho zoznamu,
						// spet sa musi vratit na filtrovany zoznam, nie na vsetky galerie.
						if ( !string.IsNullOrEmpty( this.imageGalleryItemsControl.ReturnUrl ) )
								this.returnUrl.HRef = this.imageGalleryItemsControl.ReturnUrl;
				}
		}
}
