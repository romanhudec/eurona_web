using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities
{
		public interface IUrlAliasEntity
		{
				int? UrlAliasId { get; set; }
				string Alias { get; set; }
		}
}
