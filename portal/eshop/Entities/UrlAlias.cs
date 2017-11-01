using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SHP.Entities
{
		public class UrlAlias: CMS.Entities.UrlAlias
		{
				public new class ReadByAliasType: CMS.Entities.UrlAlias.ReadByAliasType
				{
						public class Categories { }
						public class Products { }
				}
		}
}
