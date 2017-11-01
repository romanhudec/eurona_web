using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;

namespace Eurona.Common.DAL.Entities
{
		public class CenyProduktu: CMS.Entities.Entity
		{
				public class ReadByProduct
				{
						public int ProductId { get; set; }
				}
				//Id
				//[ProductId] [int] NOT NULL,
				public int? CurrencyId { get; set; }
				public string CurrencySymbol { get; set; }
				public string CurrencyCode { get; set; }
				public string Locale { get; set; }
				public decimal Cena { get; set; }
				public decimal BeznaCena { get; set; }

				public bool? DynamickaSleva{ get; set; }
				public decimal? StatickaSleva { get; set; }
				public decimal? CenaBK { get; set; }
		}
}
