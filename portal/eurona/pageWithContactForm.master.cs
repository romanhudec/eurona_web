using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS.Entities;
using CMS;
using System.Configuration;

namespace Eurona
{
		public partial class PageWithContactFormMasterPage: System.Web.UI.MasterPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						//Contact form settings
						this.cfc.DestinationEmail = ConfigurationManager.AppSettings["SMTP:CentralInbox"];
				}
		}
}
