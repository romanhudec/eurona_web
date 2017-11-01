using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Utilities;
using CMS.Entities;

namespace Eurona.User.Host
{
		public partial class RegisterPage: System.Web.UI.Page
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						registerControl.Continue += OnContinueClick;
				}

				protected void OnContinueClick( object sender, Account account )
				{
						string token = Cryptographer.Encrypt( account.Id.ToString() );
						token = Server.UrlEncode( token );

						if( Request["type"] == "1" )
								Response.Redirect( String.Format( "~/user/host/registerOrganization.aspx?token={0}", token ) );
						else if( Request["type"] == "2" )
								Response.Redirect( String.Format( "~/user/host/registerPerson.aspx?token={0}", token ) );
				}

		}
}
