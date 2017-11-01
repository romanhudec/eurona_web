using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Eurona;
using Eurona.DAL.Entities;
using System.Globalization;
using System.Threading;
using System.Configuration;
using CMS.Utilities;

public partial class EmptyMasterPage: System.Web.UI.MasterPage
{
		private string root = String.Empty;
		protected string Root
		{
				get
				{
						if ( !String.IsNullOrEmpty( root ) ) return root;
						root = Utilities.Root( Request );
						return root;
				}
		}

		protected override void OnInit( EventArgs e )
		{
				if ( !string.IsNullOrEmpty( Request.ServerVariables["http_user_agent"] ) )
				{
						if ( Request.ServerVariables["http_user_agent"].IndexOf( "Safari", StringComparison.CurrentCultureIgnoreCase ) != -1 )
								Page.ClientTarget = "uplevel";
				}
				base.OnInit( e );
		}

		protected void Page_Load( object sender, EventArgs e )
		{
		}
}
