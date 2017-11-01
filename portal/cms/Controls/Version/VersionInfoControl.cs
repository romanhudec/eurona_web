using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using System.Web.UI;

namespace CMS.Controls.Version
{
		public class VersionInfoControl : CmsControl
		{
				protected override void CreateChildControls()
				{
						base.CreateChildControls();

						Upgrade cmsVersion = Storage<Upgrade>.ReadFirst( new Upgrade.ReadCMSVersion() );
						Upgrade sysVersion = Storage<Upgrade>.ReadFirst( new Upgrade.ReadSysVersion() );

						string version = string.Format( "CMS v{0}, Application v{1}", 
								cmsVersion.FullVersionString,
								sysVersion.FullVersionString );

						this.Controls.Add( new LiteralControl( version ) );
				}
		}
}
