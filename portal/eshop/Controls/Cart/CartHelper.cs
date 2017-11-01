using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using CartEntity = SHP.Entities.Cart;
using CartProductEntity = SHP.Entities.CartProduct;
using ProductEntity = SHP.Entities.Product;
using CMS;

namespace SHP.Controls.Cart
{
		public static class CartHelper
		{
				public static int GetAnonymousSessionId( Page page )
				{
						return page.Session.SessionID.GetHashCode();
				}

				public static void AddProductToCart( Page page, int productId, int quantity )
				{
						CartEntity cart = null;
						int sessionId = GetAnonymousSessionId( page );

						if ( Security.IsLogged( false ) )
						{
								cart = Storage<CartEntity>.ReadFirst( new CartEntity.ReadOpenByAccount { AccountId = Security.Account.Id } );
								if ( cart == null )
								{
										//Zistim, ci pouzivatel nema vtvoreny kosik ako anonymny pouzivatel.
										cart = Storage<CartEntity>.ReadFirst( new CartEntity.ReadBySessionId { SessionId = sessionId } );
										if ( cart == null )
										{
												cart = new CartEntity();
												cart.AccountId = Security.Account.Id;
												Storage<CartEntity>.Create( cart );
										}
										else
										{
												//Vytvorevie pouzivatelsky nakupny kosik z anonymneho.
												cart.AccountId = Security.Account.Id;
												cart.SessionId = null;
												Storage<CartEntity>.Update( cart );
										}
								}
						}
						else
						{
								//Anonymny nakupny kosik.
								cart = Storage<CartEntity>.ReadFirst( new CartEntity.ReadBySessionId { SessionId = sessionId } );
								if ( cart == null )
								{
										cart = new CartEntity();
										cart.SessionId = sessionId;
										Storage<CartEntity>.Create( cart );
								}
						}

						//Vytvorenie/Update produktu v nakupnom kosiku.
						ProductEntity product = Storage<ProductEntity>.ReadFirst( new ProductEntity.ReadById { ProductId = productId } );
						CartProductEntity cProduct = Storage<CartProductEntity>.ReadFirst( new CartProductEntity.ReadByCartProduct { CartId = cart.Id, ProductId = productId } );
						if ( cProduct == null )
						{
								cProduct = new SHP.Entities.CartProduct();
								cProduct.CartId = cart.Id;
								cProduct.ProductId = productId;
								cProduct.Quantity = quantity;
								cProduct.Price = product.Price;
								cProduct.PriceWVAT = product.PriceWVAT;
								cProduct.VAT = product.VAT;
								if ( product.DiscountTypeId == ProductEntity.DiscountType.Price )
										cProduct.Discount = product.Discount;
								else 
										if ( product.Discount != 0 ) cProduct.Discount = ( product.Price / ( 100m * product.Discount ) );

								cProduct.PriceTotal = cProduct.PriceWithDiscount * quantity;
								cProduct.PriceTotalWVAT = cProduct.PriceWithDiscountWVAT * quantity;

								Storage<CartProductEntity>.Create( cProduct );
						}
						else
						{
								cProduct.Quantity += quantity;

								cProduct.Price = product.Price;
								cProduct.PriceWVAT = product.PriceWVAT;
								cProduct.VAT = product.VAT;
								if ( product.DiscountTypeId == ProductEntity.DiscountType.Price )
										cProduct.Discount = product.Discount;
								else
										if( product.Discount != 0 ) cProduct.Discount = ( product.Price / ( 100m * product.Discount ) );

								cProduct.PriceTotal = cProduct.PriceWithDiscount * cProduct.Quantity;
								cProduct.PriceTotalWVAT = cProduct.PriceWithDiscountWVAT * cProduct.Quantity;

								Storage<CartProductEntity>.Update( cProduct );
						}
				}

				public static void UpdateCartProduct( int cartId, int productId, int quantity )
				{
						//Vytvorenie/Update produktu v nakupnom kosiku.
						ProductEntity product = Storage<ProductEntity>.ReadFirst( new ProductEntity.ReadById { ProductId = productId } );
						CartProductEntity cProduct = Storage<CartProductEntity>.ReadFirst( new CartProductEntity.ReadByCartProduct { CartId = cartId, ProductId = productId } );
						if ( cProduct == null )
						{
								cProduct = new SHP.Entities.CartProduct();
								cProduct.CartId = cartId;
								cProduct.ProductId = productId;
								cProduct.Quantity = quantity;
								cProduct.Price = product.Price;
								cProduct.PriceWVAT = product.PriceWVAT;
								cProduct.VAT = product.VAT;
								if ( product.DiscountTypeId == ProductEntity.DiscountType.Price )
										cProduct.Discount = product.Discount;
								else
										if ( product.Discount != 0 ) cProduct.Discount = ( product.Price / ( 100m * product.Discount ) );

								cProduct.PriceTotal = cProduct.PriceWithDiscount * quantity;
								cProduct.PriceTotalWVAT = cProduct.PriceWithDiscountWVAT * quantity;

								Storage<CartProductEntity>.Create( cProduct );
						}
						else
						{
								cProduct.Quantity = quantity;

								cProduct.Price = product.Price;
								cProduct.PriceWVAT = product.PriceWVAT;
								cProduct.VAT = product.VAT;
								if ( product.DiscountTypeId == ProductEntity.DiscountType.Price )
										cProduct.Discount = product.Discount;
								else
										if ( product.Discount != 0 ) cProduct.Discount = ( product.Price / ( 100m * product.Discount ) );

								cProduct.PriceTotal = cProduct.PriceWithDiscount * cProduct.Quantity;
								cProduct.PriceTotalWVAT = cProduct.PriceWithDiscountWVAT * cProduct.Quantity;

								Storage<CartProductEntity>.Update( cProduct );
						}
				}

				/// <summary>
				/// Vráti nákupný košík aktuálne prihláseného používateľa alebo
				/// anonýmneho používateľa (anonýmny košík)
				/// </summary>
				public static CartEntity GetAccountCart( Page page )
				{
						CartEntity cart = null;
						int sessionId = GetAnonymousSessionId( page );

						if ( Security.IsLogged( false ) )
						{
								cart = Storage<CartEntity>.ReadFirst( new CartEntity.ReadOpenByAccount { AccountId = Security.Account.Id } );
								if ( cart == null )
								{
										//Zistim, ci pouzivatel nema vtvoreny kosik ako anonymny pouzivatel.
										cart = Storage<CartEntity>.ReadFirst( new CartEntity.ReadBySessionId { SessionId = sessionId } );
								}
						}
						else
						{
								//Anonymny nakupny kosik.
								cart = Storage<CartEntity>.ReadFirst( new CartEntity.ReadBySessionId { SessionId = sessionId } );
						}
						return cart;
				}
		}
}
