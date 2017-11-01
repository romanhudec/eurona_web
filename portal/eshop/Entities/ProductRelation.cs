using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;

namespace SHP.Entities
{
		public class ProductRelation: Entity
		{
				public enum Relation : int
				{
						None = 0,
						Related = 1, /*Suvisiace produkty*/
						Alternate = 2, /*Alternativne produkty*/ 
				}

				public class ReadById
				{
						public int ProductRelationId { get; set; }
				}

				public class ReadBy
				{
						public int? ParentProductId { get; set; }
						public Relation? RelationType { get; set; }
				}

				public int InstanceId { get; set; }
				public int ParentProductId { get; set; }
				public int ProductId { get; set; }
				public int RelationType { get; set; }

				//Joined properties
				public string ProductName { get; set; }
				public decimal ProductPrice { get; set; }
				public decimal ProductDiscount { get; set; }
				public string ProductAvailability { get; set; }
				public decimal ProductPriceWDiscount{ get; set; }
				public string Alias { get; set; }//UrlAlias na produkt
				/// <summary>
				/// Celkova cena = cena zo zlavou * mnozstvo
				/// </summary>
				public decimal PriceTotal{ get; set; }
				/// <summary>
				/// Celkova cena = cena zo zlavou * mnozstvo s DPH
				/// </summary>
				public decimal PriceTotalWVAT { get; set; }
		}
}
