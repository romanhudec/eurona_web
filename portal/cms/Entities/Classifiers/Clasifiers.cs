using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CMS.Entities.Classifiers
{
		public class ArticleCategory: ClassifierBase
		{
				public int ArticlesInCategory { get; set; }
				public string Display
				{
						get
						{
								return string.Format( "{0}", this.Name );
						}
				}
		}

		public class UrlAliasPrefix: ClassifierBase { }
		public class SupportedLocale: ClassifierBase { }

}
