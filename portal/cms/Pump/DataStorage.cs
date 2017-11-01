using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Pump
{
		public abstract class DataStorage
		{
				private object dataSource = null;

				public DataStorage( object dataSource )
				{
						this.dataSource = dataSource;
				}

				public object DataSource
				{
						get { return this.dataSource; }
				}
		}
}
