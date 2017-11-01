using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Eurona
{
		public partial class ImageGalleryCommentsPage: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						if ( !string.IsNullOrEmpty( this.Request["id"] ) )
						{
								this.imageGalleryCommentsControl.ImageGalleryId = Convert.ToInt32( this.Request["id"] );
								this.imageGalleryCommentsControl.CommentFormID = this.imageGalleryCommentFormControl.ClientID;
								this.imageGalleryCommentFormControl.ImageGalleryId = Convert.ToInt32( this.Request["id"] );
								this.imageGalleryCommentFormControl.RedirectUrl = this.Request.RawUrl;

								//Nastavenie spravnej navratovej url adresy 
								// je to koli tomu, ze ak je galeria zobrazena z filtrovaneho zoznamu,
								// spet sa musi vratit na filtrovany zoznam, nie na vsetky galerie.
								if ( !string.IsNullOrEmpty( this.imageGalleryCommentFormControl.ReturnUrl ) )
										this.returnUrl.HRef = this.imageGalleryCommentFormControl.ReturnUrl;
						}
				}
		}
}
