using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona
{
		public partial class ImageGalleryItemCommentsPage: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						if ( !string.IsNullOrEmpty( this.Request["id"] ) )
						{
								this.imageGalleryItemCommentsControl.ImageGalleryItemId = Convert.ToInt32( this.Request["id"] );
								this.imageGalleryItemCommentsControl.CommentFormID = this.imageGalleryItemCommentFormControl.ClientID;
								this.imageGalleryItemCommentFormControl.ImageGalleryItemId = Convert.ToInt32( this.Request["id"] );
								this.imageGalleryItemCommentFormControl.RedirectUrl = this.Request.RawUrl;

								//Nastavenie spravnej navratovej url adresy 
								// je to koli tomu, ze ak je galeria zobrazena z filtrovaneho zoznamu,
								// spet sa musi vratit na filtrovany zoznam, nie na vsetky galerie.
								if ( !string.IsNullOrEmpty( this.imageGalleryItemCommentFormControl.ReturnUrl ) )
										this.returnUrl.HRef = this.imageGalleryItemCommentFormControl.ReturnUrl;
						}
				}
		}
}
