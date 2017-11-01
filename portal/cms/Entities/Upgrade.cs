using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities
{
		public class Upgrade: Entity
		{
				public class ReadCMSVersion { }
				public class ReadSysVersion { }

				public int VersionMajor { get; set; }
				public int VersionMinor { get; set; }
				public DateTime UpgradeDate { get; set; }

				public string FullVersionString
				{
						get
						{
								return string.Format( "{0}.{1:0#}", this.VersionMajor, this.VersionMinor );
						}
				}
		}
}
