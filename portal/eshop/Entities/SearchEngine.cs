using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SHP.Entities
{
		public class ShpSearchEngineEntity: CMS.Entities.SearchEngineBase
		{
				#region Search methods
				public interface ISearch: CMS.Entities.CmsSearchEngineEntity.ISearch { }

				public class SearchProducts: ISearch
				{
						public string Keywords { get; set; }
				}
				#endregion
		}
}
