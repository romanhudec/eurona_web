using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Eurona.DAL.Entities;
using CMS;

namespace Eurona.User.Host
{
		public partial class PageMasterPage: System.Web.UI.MasterPage
		{
				protected void Page_Load( object sender, EventArgs e )
				{
						//Visible administrator menu
						this.menuAdmin.Visible = Security.IsInRole( Role.ADMINISTRATOR );
						//if ( !HostSecurity.IsAutenticated( this.Page ) ) cartInfoControl.Visible = false;
				}

				/// <summary>
				/// Update informácie v nákupnom košiku.
				/// </summary>
				public void UpdateCartInfo()
				{
						//this.cartInfoControl.UpdateControl( true );
				}
		}
}