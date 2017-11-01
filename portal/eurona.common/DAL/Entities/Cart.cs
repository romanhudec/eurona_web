using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Entities;
using SHP.Entities;

namespace Eurona.Common.DAL.Entities
{
		public class Cart: SHP.Entities.Cart
		{
				public decimal? Discount { get; set; }
				public int Status { get; set; }
				//FK

				/// <summary>
				/// Zoznam produktov v nakupnom kosiku.
				/// </summary>
				private List<CartProduct> cartProducts = null;
				public new List<CartProduct> CartProducts
				{
						get
						{
								if ( this.cartProducts != null ) return this.cartProducts;
								this.cartProducts = Storage<CartProduct>.Read( new CartProduct.ReadByCart { CartId = this.Id } );
								return this.cartProducts;
						}
				}

				/// <summary>
				/// Počet produktov v nákupnom košiku
				/// </summary>
				public new int CartProductsCount
				{
						get
						{
								int count = 0;
								foreach ( CartProduct cp in this.CartProducts )
										count += cp.Quantity;

								return count;
						}
				}

				/// <summary>
				/// Vrati true, ak nakupny kosik obsahuje Eurona produkty
				/// </summary>
				public bool HasEuronaProducts
				{
						get
						{
								foreach ( CartProduct cp in this.CartProducts )
								{
										if ( cp.InstanceId == (int)Eurona.Common.Application.InstanceType.Eurona )
												return true;
								}

								return false;
						}
				}

				/// <summary>
				/// Vrati true, ak nakupny kosik obsahuje CernyForLife produkty
				/// </summary>
				public bool HasCernyForLifeProducts
				{
						get
						{
								foreach ( CartProduct cp in this.CartProducts )
								{
										if ( cp.InstanceId == (int)Eurona.Common.Application.InstanceType.CernyForLife )
												return true;
								}

								return false;
						}
				}

				/// <summary>
				/// Celkovy pocet bodov (Katalogovych)
				/// </summary>
				public decimal BodyKatalogTotal
				{
						get
						{
								decimal sum = 0;
								foreach ( CartProduct cp in this.CartProducts )
										sum += cp.BodyCelkem.HasValue ? cp.BodyCelkem.Value : 0m;

								return sum;
						}
				}

                /// <summary>
                /// Doprava vratena z eurosapu
                /// </summary>
                public decimal? DopravneEurosap{ get; set; }

				/// <summary>
				/// Celkovy pocet Bodov vypocitanych eurosapom pre daneho pouzivatela (zo vsetkymi zlavami)
				/// </summary>
				public int BodyEurosapTotal { get; set; }

				/// <summary>
				/// Katalogova cena celkom vypocitanych eurosapom pre daneho pouzivatela (zo vsetkymi zlavami katalogovych cien)
				/// </summary>
				public decimal KatalogovaCenaCelkemByEurosap { get; set; }

				/// <summary>
				/// Celkova katalogova cena produktov
				/// </summary>
				public decimal KatalogovaCenaCelkem
				{
						get
						{
								decimal cena = 0;
								foreach ( CartProduct cp in this.CartProducts )
										cena += cp.PriceTotalWVAT;

								return cena;
						}
				}
		}
}
