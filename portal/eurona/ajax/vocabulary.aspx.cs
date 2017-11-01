using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TranslationEntity = CMS.Entities.Translation;
using System.IO;

namespace Eurona.ajax
{
		public partial class vocabularyRequest: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						Response.CacheControl = "no-cache";
						Response.AddHeader( "Pragma", "no-cache" );
						Response.ExpiresAbsolute = DateTime.Now.AddMinutes( -1 );
						Response.Expires = -1;


						int id = -1;
						if ( !Int32.TryParse( Request["id"], out id ) ) return;
						string value = GetContentData();

						TranslationEntity te = Storage<TranslationEntity>.ReadFirst( new TranslationEntity.ReadById { TranslationId = id } );
						if ( te == null )
								return;

						te.Trans = value;
						Storage<TranslationEntity>.Update( te );
						Response.Write( "#OK" );
						Response.End();
						
				}

				private string GetContentData()
				{
						Stream stream = Request.InputStream;
						byte[] data = new byte[stream.Length];
						stream.Read( data, 0, data.Length );
						stream.Close();
						return Request.ContentEncoding.GetString( data );
				}
		}
}
