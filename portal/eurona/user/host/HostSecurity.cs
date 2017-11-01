using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eurona.User.Host
{
		public static class HostSecurity
		{
				public static string HostNameSessionName { get { return "HostName"; } }
				public static string HostAdvisorCodeSessionName { get { return "HostAdvisor"; } }

				public static bool IsAutenticated( System.Web.UI.Page page )
				{
						if ( page.Session[HostNameSessionName] == null ) return false;
						if ( page.Session[HostAdvisorCodeSessionName] == null ) return false;

						return !string.IsNullOrEmpty( page.Session[HostNameSessionName].ToString() ) &&
								!string.IsNullOrEmpty( page.Session[HostAdvisorCodeSessionName].ToString() );
				}

				public static void LoginHost( System.Web.UI.Page page )
				{
						page.Response.Redirect( page.ResolveUrl( "~/user/host/login.aspx" ) );
				}
		}
}