using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eurona.DAL.Entities;

namespace Eurona
{
		public partial class ContactAdvisor: WebPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						if ( !string.IsNullOrEmpty( Request["id"] ) )
						{
								Account account = Storage<Account>.ReadFirst( new Account.ReadById { AccountId = Convert.ToInt32( Request["id"] ) } );

								this.contactFormControl.BccEmail = CMS.Utilities.ConfigUtilities.ConfigValue( "SHP:SMTP:ContactAdvisor", this );
								this.contactFormControl.DestinationEmail = account.Email;
						}
				}
		}
}