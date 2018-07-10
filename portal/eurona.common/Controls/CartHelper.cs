using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using CartEntity = Eurona.Common.DAL.Entities.Cart;
using CartProductEntity = Eurona.Common.DAL.Entities.CartProduct;
using ProductEntity = Eurona.Common.DAL.Entities.Product;
using CurrencyEntity = SHP.Entities.Classifiers.Currency;
using CMS;
using Eurona.Common.DAL.Entities;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace Eurona.Common.Controls.Cart {
    public static class EuronaCartHelper {
        public static int GetAnonymousSessionId(Page page) {
            return page.Session.SessionID.GetHashCode();
        }

        public static bool ValidateProductBeforeAddingToChart(string productCode, ProductEntity product, int quantity, bool cerpatBK, Control owner, bool isOperator) {
            if (product == null) {
                string alert = string.Format("alert('Produkt s kódem {0}, nebyl nalezen!');", productCode);
                owner.Page.ClientScript.RegisterStartupScript(owner.GetType(), "addProductToCart", alert, true);
                return false;
            }
            if (cerpatBK && (!product.BonusovyKredit.HasValue || product.BonusovyKredit.Value == 0)) {
                string jsAlert = string.Format("alert('{0}');", string.Format("Tento produkt nelze koupit za bonusové kredity!"));
                owner.Page.ClientScript.RegisterStartupScript(owner.Page.GetType(), "addProductToCart", jsAlert, true);
                return false;
            }
            if (product.ProdejUkoncen == true) {
                string alert = string.Format("alert('Prodej produktu s kódem {0}, byl ukončen!');", product.Code);
                owner.Page.ClientScript.RegisterStartupScript(owner.GetType(), "addProductToCart", alert, true);
                return false;
            }
            if (product.Vyprodano == true) {
                string alert = string.Format("alert('Produktu s kódem {0}, je vyprodán!');", product.Code);
                owner.Page.ClientScript.RegisterStartupScript(owner.GetType(), "addProductToCart", alert, true);
                return false;
            }

            //Kontrola na internal storage
            if (product.InternalStorageCount != ProductEntity.INTERNAL_STORAGE_NOT_AVAILABLE && product.InternalStorageCount < quantity) {
                string jsAlert = string.Format("alert('{0}');", string.Format("Omlouváme se, ale tento produkt momentálne na sklade není!"));
                owner.Page.ClientScript.RegisterStartupScript(owner.Page.GetType(), "addProductToCart", jsAlert, true);
                return false;
            }

            //Ak sa jedna o produkt CL
            if (product.InstanceId == (int)Eurona.Common.Application.InstanceType.CernyForLife) {
                //24.06.2013 CL produkty su povolene vsetkym
                /* 
				if (!isOperator)
				{
					string alert = string.Format("alert('Cerny For Life produkty je možné nakupovat pouze v elektronickém obchodu Cerny For Life!');", productCode);
					owner.Page.ClientScript.RegisterStartupScript(owner.GetType(), "addProductToCart", alert, true);
					return false;
				}
                 * */
                //Kontrola na internal storage
                if (product.InternalStorageCount != ProductEntity.INTERNAL_STORAGE_NOT_AVAILABLE && product.InternalStorageCount < quantity) {
                    string jsAlert = string.Format("alert('{0}');", string.Format("Omlouváme se, ale tento produkt momentálne na sklade není!"));
                    owner.Page.ClientScript.RegisterStartupScript(owner.Page.GetType(), "addProductToCart", jsAlert, true);
                    return false;
                }
                //Ak sa nema internal storage pouzivat tak sa kontroluje Storage
                if (product.InternalStorageCount == ProductEntity.INTERNAL_STORAGE_NOT_AVAILABLE) {
                    if (product.StorageCount <= 0 || product.StorageCount < quantity) {
                        string jsAlert = string.Format("alert('{0}');", string.Format("Omlouváme se, ale tento produkt momentálne na sklade není!"));
                        owner.Page.ClientScript.RegisterStartupScript(owner.Page.GetType(), "addProductToCart", jsAlert, true);
                        return false;
                    }
                }
            }

            if (product.MinimalniPocetVBaleni.HasValue && quantity < product.MinimalniPocetVBaleni.Value) {
                //Alert s informaciou o maximalnom objednatelnom pocte ks.
                VocabularyUtilities vu = new VocabularyUtilities("TextoveHlaseni");
                CMS.Entities.Translation translation = vu.Translate("NedostatecnyMinimalniPocetObjednatelnehoPoctu");
                string jsAlert = string.Format("alert('{0}');", string.Format(translation.Trans, product.MinimalniPocetVBaleni.Value));
                owner.Page.ClientScript.RegisterStartupScript(owner.Page.GetType(), "addProductToCart", jsAlert, true);
                return false;
            }


            if (product.MaximalniPocetVBaleni.HasValue && quantity > product.MaximalniPocetVBaleni.Value) {
                //Alert s informaciou o maximalnom objednatelnom pocte ks.
                VocabularyUtilities vu = new VocabularyUtilities("TextoveHlaseni");
                CMS.Entities.Translation translation = vu.Translate("PrekroceniMaximalnihoObjednatelnehoPoctu");
                string jsAlert = string.Format("alert('{0}');", string.Format(translation.Trans, product.MaximalniPocetVBaleni.Value));
                owner.Page.ClientScript.RegisterStartupScript(owner.Page.GetType(), "addProductToCart", jsAlert, true);
                return false;
            }

            if (product.LimitDate.HasValue && (product.LimitDate.Value - DateTime.Now).TotalMinutes < 0) {
                //Alert s informaciou o ukonceni predaja limitovanej akcie.
                VocabularyUtilities vu = new VocabularyUtilities("TextoveHlaseni");
                CMS.Entities.Translation translation = vu.Translate("LimitovanaAkceUkoncena");
                string jsAlert = string.Format("alert('{0}');", translation.Trans);
                owner.Page.ClientScript.RegisterStartupScript(owner.Page.GetType(), "addProductToCart", jsAlert, true);
                return false;
            }

            return true;
        }
        public static bool ValidateProductBeforeAddingToChart(string productCode, ProductEntity product, int quantity, Control owner) {
            return EuronaCartHelper.ValidateProductBeforeAddingToChart(productCode, product, quantity, false, owner, false);
        }
        public static bool AddProductToCart(Page page, int productId, int quantity, bool cerpatBK, Control owner, bool isOperator) {
            CartEntity cart = null;
            int sessionId = GetAnonymousSessionId(page);

            if (Security.IsLogged(false)) {
                cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadOpenByAccount { AccountId = Security.Account.Id });
                if (cart == null) {
                    //Zistim, ci pouzivatel nema vtvoreny kosik ako anonymny pouzivatel.
                    cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadBySessionId { SessionId = sessionId });
                    if (cart == null) {
                        cart = new CartEntity();
                        cart.AccountId = Security.Account.Id;
                        Storage<CartEntity>.Create(cart);
                    } else {
                        //Vytvorevie pouzivatelsky nakupny kosik z anonymneho.
                        cart.AccountId = Security.Account.Id;
                        cart.SessionId = null;
                        Storage<CartEntity>.Update(cart);
                    }
                }
            } else {
                //Anonymny nakupny kosik.
                cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadBySessionId { SessionId = sessionId });
                if (cart == null) {
                    cart = new CartEntity();
                    cart.SessionId = sessionId;
                    Storage<CartEntity>.Create(cart);
                }
            }

            //Vytvorenie/Update produktu v nakupnom kosiku.
            ProductEntity product = Storage<ProductEntity>.ReadFirst(new ProductEntity.ReadById { ProductId = productId });
            CartProductEntity cProduct = Storage<CartProductEntity>.ReadFirst(new CartProductEntity.ReadByCartProduct { CartId = cart.Id, ProductId = productId, CerpatBK = cerpatBK });
            if (cProduct == null) {
                cProduct = new CartProductEntity();
                cProduct.InstanceId = product.InstanceId;
                cProduct.CartId = cart.Id;
                cProduct.ProductId = productId;
                cProduct.Quantity = quantity;
                cProduct.Price = product.Price;
                cProduct.PriceWVAT = product.PriceWVAT;
                cProduct.VAT = product.VAT;
                cProduct.CerpatBonusoveKredity = cerpatBK;
                if (product.DiscountTypeId == ProductEntity.DiscountType.Price)
                    cProduct.Discount = product.Discount;
                else
                    if (product.Discount != 0) cProduct.Discount = (product.Price / (100m * product.Discount));

                cProduct.PriceTotal = cProduct.PriceWithDiscount * quantity;
                cProduct.PriceTotalWVAT = cProduct.PriceWithDiscountWVAT * quantity;
                cProduct.CurrencyId = product.CurrencyId;

                Storage<CartProductEntity>.Create(cProduct);
            } else {
                cProduct.Quantity += quantity;
                //Kontrola na dodrzanie celkoveho mnozstva v kosiku
                if (!ValidateProductBeforeAddingToChart(cProduct.ProductCode, product, cProduct.Quantity, cerpatBK, owner, isOperator))
                    return false;

                cProduct.Price = product.Price;
                cProduct.PriceWVAT = product.PriceWVAT;
                cProduct.VAT = product.VAT;
                if (product.DiscountTypeId == ProductEntity.DiscountType.Price)
                    cProduct.Discount = product.Discount;
                else
                    if (product.Discount != 0) cProduct.Discount = (product.Price / (100m * product.Discount));

                cProduct.PriceTotal = cProduct.PriceWithDiscount * cProduct.Quantity;
                cProduct.PriceTotalWVAT = cProduct.PriceWithDiscountWVAT * cProduct.Quantity;
                cProduct.CurrencyId = product.CurrencyId;

                Storage<CartProductEntity>.Update(cProduct);
            }

            //Update Internal Storage Count if Internal storage is Available
            if (product.InternalStorageCount != ProductEntity.INTERNAL_STORAGE_NOT_AVAILABLE) {
                product.InternalStorageCount = product.InternalStorageCount - quantity;
                if (product.InternalStorageCount < 0) product.InternalStorageCount = 0;
                Storage<ProductEntity>.Update(product);
            }

            //Zisti sa, ci kosik obsahuje produkty CL
            bool hasCernyForLifeProduct = false;
            foreach (CartProductEntity cp in cart.CartProducts) {
                if (cp.InstanceId == (int)Application.InstanceType.CernyForLife) {
                    hasCernyForLifeProduct = true;
                    break;
                }
            }
            //Metoda prepocita Intensa produkty v kosiku.
            Eurona.Common.EuronaUserMarzeInfo umi = null;
            if (hasCernyForLifeProduct && cart.AccountId.HasValue) umi = UpdateIntensaProductInCart(page, cart.Id, cart.AccountId.Value);

            cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadById { CartId = cart.Id });

            //Update Cart Price
            decimal price = 0m;
            decimal priceWVAT = 0m;
            foreach (CartProductEntity cp in cart.CartProducts) {
                price += cp.PriceTotal;
                priceWVAT += cp.PriceTotalWVAT;
            }
            cart.PriceTotal = price;
            cart.PriceTotalWVAT = priceWVAT;
            Storage<CartEntity>.Update(cart);

            return true;
        }
        public static bool AddProductToCart(Page page, int productId, int quantity, Control owner) {
            return AddProductToCart(page, productId, quantity, false, owner, false);
        }

        public static Eurona.Common.EuronaUserMarzeInfo UpdateCartProduct(Page page, int cartId, int productId, int quantity, bool updateOnly = true) {
            Eurona.Common.EuronaUserMarzeInfo umi = null;

            //Vytvorenie/Update produktu v nakupnom kosiku.
            ProductEntity product = Storage<ProductEntity>.ReadFirst(new ProductEntity.ReadById { ProductId = productId });
            CartProductEntity cProduct = Storage<CartProductEntity>.ReadFirst(new CartProductEntity.ReadByCartProduct { CartId = cartId, ProductId = productId });
            if (cProduct == null) {
                cProduct = new CartProductEntity();
                cProduct.CartId = cartId;
                cProduct.InstanceId = product.InstanceId;
                cProduct.ProductId = productId;
                cProduct.Quantity = quantity;
                cProduct.Price = product.Price;
                cProduct.PriceWVAT = product.PriceWVAT;
                cProduct.VAT = product.VAT;
                if (product.DiscountTypeId == ProductEntity.DiscountType.Price)
                    cProduct.Discount = product.Discount;
                else
                    if (product.Discount != 0) cProduct.Discount = (product.Price / (100m * product.Discount));

                cProduct.PriceTotal = cProduct.PriceWithDiscount * quantity;
                cProduct.PriceTotalWVAT = cProduct.PriceWithDiscountWVAT * quantity;
                cProduct.CurrencyId = product.CurrencyId;

                Storage<CartProductEntity>.Create(cProduct);
            } else {
                if( updateOnly )cProduct.Quantity = quantity;
                else cProduct.Quantity += quantity;

                cProduct.Price = product.Price;
                cProduct.PriceWVAT = product.PriceWVAT;
                cProduct.VAT = product.VAT;
                if (product.DiscountTypeId == ProductEntity.DiscountType.Price)
                    cProduct.Discount = product.Discount;
                else
                    if (product.Discount != 0) cProduct.Discount = (product.Price / (100m * product.Discount));

                cProduct.PriceTotal = cProduct.PriceWithDiscount * cProduct.Quantity;
                cProduct.PriceTotalWVAT = cProduct.PriceWithDiscountWVAT * cProduct.Quantity;
                cProduct.CurrencyId = product.CurrencyId;

                Storage<CartProductEntity>.Update(cProduct);
            }

            CartEntity cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadById { CartId = cartId });
            //Zisti sa, ci kosik obsahuje produkty Intensa
            bool hasCernyForLifeProduct = false;
            foreach (CartProductEntity cp in cart.CartProducts) {
                if (cp.InstanceId == (int)Application.InstanceType.CernyForLife) {
                    hasCernyForLifeProduct = true;
                    break;
                }
            }
            //Metoda prepocita Intensa produkty v kosiku.
            if (hasCernyForLifeProduct && cart.AccountId.HasValue) umi = UpdateIntensaProductInCart(page, cartId, cart.AccountId.Value);

            //Update Cart Price
            cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadById { CartId = cartId });
            decimal price = 0m;
            decimal priceWVAT = 0m;
            foreach (CartProductEntity cp in cart.CartProducts) {
                price += cp.PriceTotal;
                priceWVAT += cp.PriceTotalWVAT;
            }
            cart.PriceTotal = price;
            cart.PriceTotalWVAT = priceWVAT;
            Storage<CartEntity>.Update(cart);

            return umi;
        }

        public static bool RemoveProductFromCart(int cartProductId) {
            CartProductEntity cartProduct = Storage<CartProductEntity>.ReadFirst(new CartProductEntity.ReadById { CartProductId = cartProductId });
            if (cartProduct != null) {
                ProductEntity product = Storage<ProductEntity>.ReadFirst(new ProductEntity.ReadById { ProductId = cartProduct.ProductId });
                Storage<CartProductEntity>.Delete(cartProduct);

                if (product.InternalStorageCount != ProductEntity.INTERNAL_STORAGE_NOT_AVAILABLE) {
                    product.InternalStorageCount = product.InternalStorageCount + cartProduct.Quantity;
                    Storage<ProductEntity>.Update(product);
                }

                return true;
            }

            return false;
        }
        /// <summary>
        /// Vráti nákupný košík aktuálne prihláseného používateľa alebo
        /// anonýmneho používateľa (anonýmny košík)
        /// </summary>
        public static CartEntity GetAccountCart(Page page) {
            CartEntity cart = null;
            int sessionId = GetAnonymousSessionId(page);

            if (Security.IsLogged(false)) {
                cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadOpenByAccount { AccountId = Security.Account.Id });
                if (cart == null) {
                    //Zistim, ci pouzivatel nema vtvoreny kosik ako anonymny pouzivatel.
                    cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadBySessionId { SessionId = sessionId });
                    if (cart == null) return null;
                    if (!cart.AccountId.HasValue) {
                        //Vytvorevie pouzivatelsky nakupny kosik z anonymneho.
                        cart.AccountId = Security.Account.Id;
                        cart.SessionId = null;
                        Storage<CartEntity>.Update(cart);
                    }
                }
            } else {
                //Anonymny nakupny kosik.
                int cartId = 0;
                int instanceId = 0;
                Int32.TryParse(ConfigurationManager.AppSettings["InstanceId"], out instanceId);
                string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                CMS.Pump.MSSQLStorage euronaStorage = new CMS.Pump.MSSQLStorage(connectionString);
                using (SqlConnection connection = euronaStorage.Connect()) {
                    string sql = @"SELECT c.CartId FROM tShpCart c WITH (NOLOCK)WHERE c.InstanceId = @InstanceId AND c.SessionId = @SessionId";                 
                    DataTable dt = euronaStorage.Query(connection, sql, 
                        new SqlParameter("@InstanceId", instanceId),
                        new SqlParameter("@SessionId", sessionId));

                    if (dt.Rows.Count != 0) {
                        cartId = Convert.ToInt32(dt.Rows[0]["CartId"]);
                    }
                }
                if (cartId != 0) {
                    cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadById { CartId = cartId});
                }
                //TODO: zakomentovane 29.06.2018 - najprv rychly select na ID a az potom select na cely kosik!!!!
                //cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadBySessionId { SessionId = sessionId });
            }
            return cart;
        }

        /// <summary>
        /// Prepocitanie kosika
        /// </summary>
        /// <param name="page"></param>
        public static Eurona.Common.EuronaUserMarzeInfo RecalculateCart(Page page, int cartId) {
            CartEntity cart = cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadById { CartId = cartId });

            //Metoda prepocita Intensa produkty v kosiku.
            Eurona.Common.EuronaUserMarzeInfo umi = UpdateIntensaProductInCart(page, cart.Id, cart.AccountId);

            cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadById { CartId = cart.Id });

            //Update Cart Price
            decimal price = 0m;
            decimal priceWVAT = 0m;
            foreach (CartProductEntity cp in cart.CartProducts) {
                price += cp.PriceTotal;
                priceWVAT += cp.PriceTotalWVAT;
            }
            cart.PriceTotal = price;
            cart.PriceTotalWVAT = priceWVAT;
            Storage<CartEntity>.Update(cart);

            return umi;
        }

        public static Eurona.Common.EuronaUserMarzeInfo RecalculateOpenCart(Page page) {
            CartEntity cart = null;
            int sessionId = GetAnonymousSessionId(page);

            if (Security.IsLogged(false)) {
                cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadOpenByAccount { AccountId = Security.Account.Id });
                if (cart == null) {
                    //Zistim, ci pouzivatel nema vtvoreny kosik ako anonymny pouzivatel.
                    cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadBySessionId { SessionId = sessionId });
                    if (cart == null) {
                        cart = new CartEntity();
                        cart.AccountId = Security.Account.Id;
                        Storage<CartEntity>.Create(cart);
                    } else {
                        //Vytvorevie pouzivatelsky nakupny kosik z anonymneho.
                        cart.AccountId = Security.Account.Id;
                        cart.SessionId = null;
                        Storage<CartEntity>.Update(cart);
                    }
                }
            } else {
                //Anonymny nakupny kosik.
                cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadBySessionId { SessionId = sessionId });
                if (cart == null) {
                    cart = new CartEntity();
                    cart.SessionId = sessionId;
                    Storage<CartEntity>.Create(cart);
                }
            }

            //Metoda prepocita Intensa produkty v kosiku.
            Eurona.Common.EuronaUserMarzeInfo umi = UpdateIntensaProductInCart(page, cart.Id, cart.AccountId);
            cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadById { CartId = cart.Id });

            //Update Cart Price
            decimal price = 0m;
            decimal priceWVAT = 0m;
            foreach (CartProductEntity cp in cart.CartProducts) {
                price += cp.PriceTotal;
                priceWVAT += cp.PriceTotalWVAT;
            }
            cart.PriceTotal = price;
            cart.PriceTotalWVAT = priceWVAT;
            Storage<CartEntity>.Update(cart);

            return umi;
        }

        /// <summary>
        /// Metoda prepocita Intensa produkty v kosiku.
        /// </summary>
        public static Eurona.Common.EuronaUserMarzeInfo UpdateIntensaProductInCart(Page page, int cartId, int? accountId) {
            //Ak je pouzivatel prihlaseny
            if (Security.IsLogged(false) || accountId.HasValue) {
                Organization org = Storage<Organization>.ReadFirst(new Organization.ReadByAccountId { AccountId = accountId.Value });
                if (org == null || org.TVD_Id.HasValue == false) return null;

                string message = string.Empty;
                Eurona.Common.EuronaUserMarzeInfo umi = new EuronaUserMarzeInfo(page, org.TVD_Id.Value);
                umi.GetMarzeInfoFromEurosap(cartId, out message);
                return umi;
            }

            return null;
        }
    }

    public static class CernyForLifeCartHelper {
        public static int GetAnonymousSessionId(Page page) {
            return page.Session.SessionID.GetHashCode();
        }

        public static bool ValidateProductBeforeAddingToChart(string productCode, ProductEntity product, int quantity, bool cerpatBK, Control owner, bool isOperator) {
            if (product == null) {
                string alert = string.Format("alert('Produkt s kódem {0}, nebyl nalezen!');", productCode);
                owner.Page.ClientScript.RegisterStartupScript(owner.GetType(), "addProductToCart", alert, true);
                return false;
            }
            if (cerpatBK && (!product.BonusovyKredit.HasValue || product.BonusovyKredit.Value == 0)) {
                string jsAlert = string.Format("alert('{0}');", string.Format("Tento produkt nelze koupit za bonusové kredity!"));
                owner.Page.ClientScript.RegisterStartupScript(owner.Page.GetType(), "addProductToCart", jsAlert, true);
                return false;
            }

            //Kontrola na internal storage
            if (product.InternalStorageCount != ProductEntity.INTERNAL_STORAGE_NOT_AVAILABLE && product.InternalStorageCount < quantity) {
                string jsAlert = string.Format("alert('{0}');", string.Format("Omlouváme se, ale tento produkt momentálne na sklade není!"));
                owner.Page.ClientScript.RegisterStartupScript(owner.Page.GetType(), "addProductToCart", jsAlert, true);
                return false;
            }

            //Ak sa nema internal storage pouzivat tak sa kontroluje Storage
            if (product.InternalStorageCount == ProductEntity.INTERNAL_STORAGE_NOT_AVAILABLE) {
                if (product.StorageCount <= 0 || product.StorageCount < quantity) {
                    string jsAlert = string.Format("alert('{0}');", string.Format("Omlouváme se, ale tento produkt momentálne na sklade není!"));
                    owner.Page.ClientScript.RegisterStartupScript(owner.Page.GetType(), "addProductToCart", jsAlert, true);
                    return false;
                }
            }

            if (product.ProdejUkoncen == true) {
                string alert = string.Format("alert('Prodej produktu s kódem {0}, byl ukončen!');", product.Code);
                owner.Page.ClientScript.RegisterStartupScript(owner.GetType(), "addProductToCart", alert, true);
                return false;
            }
            if (product.Vyprodano == true) {
                string alert = string.Format("alert('Produktu s kódem {0}, je vyprodán!');", product.Code);
                owner.Page.ClientScript.RegisterStartupScript(owner.GetType(), "addProductToCart", alert, true);
                return false;
            }
            if (!isOperator) {
                if (product.InstanceId == (int)Eurona.Common.Application.InstanceType.Eurona) {
                    string alert = "alert('Eurona produkty je možné nakupovat pouze v elektronickém obchodu Eurona!');";
                    owner.Page.ClientScript.RegisterStartupScript(owner.GetType(), "addProductToCart", alert, true);
                    return false;
                }
            }

            if (product.MinimalniPocetVBaleni.HasValue && quantity < product.MinimalniPocetVBaleni.Value) {
                //Alert s informaciou o maximalnom objednatelnom pocte ks.
                VocabularyUtilities vu = new VocabularyUtilities("TextoveHlaseni");
                CMS.Entities.Translation translation = vu.Translate("NedostatecnyMinimalniPocetObjednatelnehoPoctu");
                string jsAlert = string.Format("alert('{0}');", string.Format(translation.Trans, product.MinimalniPocetVBaleni.Value));
                owner.Page.ClientScript.RegisterStartupScript(owner.Page.GetType(), "addProductToCart", jsAlert, true);
                return false;
            }

            if (product.MaximalniPocetVBaleni.HasValue && quantity > product.MaximalniPocetVBaleni.Value) {
                //Alert s informaciou o maximalnom objednatelnom pocte ks.
                VocabularyUtilities vu = new VocabularyUtilities("TextoveHlaseni");
                CMS.Entities.Translation translation = vu.Translate("PrekroceniMaximalnihoObjednatelnehoPoctu");
                string jsAlert = string.Format("alert('{0}');", string.Format(translation.Trans, product.MaximalniPocetVBaleni.Value));
                owner.Page.ClientScript.RegisterStartupScript(owner.Page.GetType(), "addProductToCart", jsAlert, true);
                return false;
            }

            return true;
        }

        public static bool ValidateProductBeforeAddingToChart(string productCode, ProductEntity product, int quantity, Control owner) {
            return CernyForLifeCartHelper.ValidateProductBeforeAddingToChart(productCode, product, quantity, false, owner, false);
        }
        public static bool AddProductToCart(Page page, int productId, int quantity, Control owner) {
            return AddProductToCart(page, productId, quantity, false, owner, false);
        }
        public static bool AddProductToCart(Page page, int productId, int quantity, bool cerpatBK, Control owner, bool isOperator) {
            CartEntity cart = null;
            int sessionId = GetAnonymousSessionId(page);

            if (Security.IsLogged(false)) {
                cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadOpenByAccount { AccountId = Security.Account.Id });
                if (cart == null) {
                    //Zistim, ci pouzivatel nema vtvoreny kosik ako anonymny pouzivatel.
                    cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadBySessionId { SessionId = sessionId });
                    if (cart == null) {
                        cart = new CartEntity();
                        cart.AccountId = Security.Account.Id;
                        Storage<CartEntity>.Create(cart);
                    } else {
                        //Vytvorevie pouzivatelsky nakupny kosik z anonymneho.
                        cart.AccountId = Security.Account.Id;
                        cart.SessionId = null;
                        Storage<CartEntity>.Update(cart);
                    }
                }
            } else {
                //Anonymny nakupny kosik.
                cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadBySessionId { SessionId = sessionId });
                if (cart == null) {
                    cart = new CartEntity();
                    cart.SessionId = sessionId;
                    Storage<CartEntity>.Create(cart);
                }
            }

            //Vytvorenie/Update produktu v nakupnom kosiku.
            ProductEntity product = Storage<ProductEntity>.ReadFirst(new ProductEntity.ReadById { ProductId = productId });
            CartProductEntity cProduct = Storage<CartProductEntity>.ReadFirst(new CartProductEntity.ReadByCartProduct { CartId = cart.Id, ProductId = productId });
            if (cProduct == null) {
                cProduct = new CartProductEntity();
                cProduct.InstanceId = product.InstanceId;
                cProduct.CartId = cart.Id;
                cProduct.ProductId = productId;
                cProduct.Quantity = quantity;
                cProduct.Price = product.Price;
                cProduct.PriceWVAT = product.PriceWVAT;
                cProduct.VAT = product.VAT;
                cProduct.CerpatBonusoveKredity = cerpatBK;
                if (product.DiscountTypeId == ProductEntity.DiscountType.Price)
                    cProduct.Discount = product.Discount;
                else
                    if (product.Discount != 0) cProduct.Discount = (product.Price / (100m * product.Discount));

                //CP Zlava na zaklade Dynamickej zlavy
                if (product.DynamickaSleva == false && product.StatickaSleva.HasValue && product.StatickaSleva.Value != 0) {
                    cProduct.Discount = (product.Price / (100m * product.StatickaSleva.Value));
                }

                cProduct.PriceTotal = cProduct.PriceWithDiscount * quantity;
                cProduct.PriceTotalWVAT = cProduct.PriceWithDiscountWVAT * quantity;
                cProduct.CurrencyId = product.CurrencyId;

                Storage<CartProductEntity>.Create(cProduct);
            } else {
                cProduct.Quantity += quantity;
                //Kontrola na dodrzanie celkoveho mnozstva v kosiku
                if (!ValidateProductBeforeAddingToChart(cProduct.ProductCode, product, cProduct.Quantity, cerpatBK, owner, isOperator))
                    return false;

                cProduct.Price = product.Price;
                cProduct.PriceWVAT = product.PriceWVAT;
                cProduct.VAT = product.VAT;
                if (product.DiscountTypeId == ProductEntity.DiscountType.Price)
                    cProduct.Discount = product.Discount;
                else
                    if (product.Discount != 0) cProduct.Discount = (product.Price / (100m * product.Discount));

                //CP Zlava na zaklade Dynamickej zlavy
                if (product.DynamickaSleva == false && product.StatickaSleva.HasValue && product.StatickaSleva.Value != 0) {
                    cProduct.Discount = (product.Price / (100m * product.StatickaSleva.Value));
                }

                cProduct.PriceTotal = cProduct.PriceWithDiscount * cProduct.Quantity;
                cProduct.PriceTotalWVAT = cProduct.PriceWithDiscountWVAT * cProduct.Quantity;
                cProduct.CurrencyId = product.CurrencyId;

                Storage<CartProductEntity>.Update(cProduct);
            }

            //Update Internal Storage Count if Internal storage is Available
            if (product.InternalStorageCount != ProductEntity.INTERNAL_STORAGE_NOT_AVAILABLE) {
                product.InternalStorageCount = product.InternalStorageCount - quantity;
                if (product.InternalStorageCount < 0) product.InternalStorageCount = 0;
                Storage<ProductEntity>.Update(product);
            }

            //Zisti sa, ci kosik obsahuje produkty Eurona
            bool hasEuronaProduct = false;
            foreach (CartProductEntity cp in cart.CartProducts) {
                if (cp.InstanceId == (int)Application.InstanceType.Eurona) {
                    hasEuronaProduct = true;
                    break;
                }
            }
            //Metoda prepocita Eurona produkty v kosiku.
            if (hasEuronaProduct) UpdateEuronaProductInCart(page, cart.Id, cart.AccountId.Value);
            cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadById { CartId = cart.Id });

            //Update Cart Price
            decimal price = 0m;
            decimal priceWVAT = 0m;
            foreach (CartProductEntity cp in cart.CartProducts) {
                price += cp.PriceTotal;
                priceWVAT += cp.PriceTotalWVAT;
            }
            cart.PriceTotal = price;
            cart.PriceTotalWVAT = priceWVAT;
            Storage<CartEntity>.Update(cart);

            return true;
        }

        public static Eurona.Common.CernyForLifeUserMarzeInfo UpdateCartProduct(Page page, int cartId, int productId, int quantity) {
            Eurona.Common.CernyForLifeUserMarzeInfo umi = null;

            //Vytvorenie/Update produktu v nakupnom kosiku.
            ProductEntity product = Storage<ProductEntity>.ReadFirst(new ProductEntity.ReadById { ProductId = productId });
            CartProductEntity cProduct = Storage<CartProductEntity>.ReadFirst(new CartProductEntity.ReadByCartProduct { CartId = cartId, ProductId = productId });
            if (cProduct == null) {
                cProduct = new CartProductEntity();
                cProduct.CartId = cartId;
                cProduct.InstanceId = product.InstanceId;
                cProduct.ProductId = productId;
                cProduct.Quantity = quantity;
                cProduct.Price = product.Price;
                cProduct.PriceWVAT = product.PriceWVAT;
                cProduct.VAT = product.VAT;
                if (product.DiscountTypeId == ProductEntity.DiscountType.Price)
                    cProduct.Discount = product.Discount;
                else
                    if (product.Discount != 0) cProduct.Discount = (product.Price / (100m * product.Discount));

                //CP Zlava na zaklade Dynamickej zlavy
                if (product.DynamickaSleva == false && product.StatickaSleva.HasValue && product.StatickaSleva.Value != 0) {
                    cProduct.Discount = (product.Price / (100m * product.StatickaSleva.Value));
                }

                cProduct.PriceTotal = cProduct.PriceWithDiscount * quantity;
                cProduct.PriceTotalWVAT = cProduct.PriceWithDiscountWVAT * quantity;
                cProduct.CurrencyId = product.CurrencyId;

                Storage<CartProductEntity>.Create(cProduct);
            } else {
                cProduct.Quantity = quantity;

                cProduct.Price = product.Price;
                cProduct.PriceWVAT = product.PriceWVAT;
                cProduct.VAT = product.VAT;
                if (product.DiscountTypeId == ProductEntity.DiscountType.Price)
                    cProduct.Discount = product.Discount;
                else
                    if (product.Discount != 0) cProduct.Discount = (product.Price / (100m * product.Discount));

                //CP Zlava na zaklade Dynamickej zlavy
                if (product.DynamickaSleva == false && product.StatickaSleva.HasValue && product.StatickaSleva.Value != 0) {
                    cProduct.Discount = (product.Price / (100m * product.StatickaSleva.Value));
                }

                cProduct.PriceTotal = cProduct.PriceWithDiscount * cProduct.Quantity;
                cProduct.PriceTotalWVAT = cProduct.PriceWithDiscountWVAT * cProduct.Quantity;
                cProduct.CurrencyId = product.CurrencyId;

                Storage<CartProductEntity>.Update(cProduct);
            }

            //Zisti sa, ci kosik obsahuje produkty Eurona
            bool hasEuronaProduct = false;
            CartEntity cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadById { CartId = cartId });
            foreach (CartProductEntity cp in cart.CartProducts) {
                if (cp.InstanceId == (int)Application.InstanceType.Eurona) {
                    hasEuronaProduct = true;
                    break;
                }
            }
            //Metoda prepocita Eurona produkty v kosiku.
            if (hasEuronaProduct) umi = UpdateEuronaProductInCart(page, cartId, cart.AccountId.Value);

            //Update Cart Price
            cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadById { CartId = cartId });
            decimal price = 0m;
            decimal priceWVAT = 0m;
            foreach (CartProductEntity cp in cart.CartProducts) {
                price += cp.PriceTotal;
                priceWVAT += cp.PriceTotalWVAT;
            }
            cart.PriceTotal = price;
            cart.PriceTotalWVAT = priceWVAT;
            Storage<CartEntity>.Update(cart);

            return umi;
        }

        public static bool RemoveProductFromCart(int cartProductId) {
            CartProductEntity cartProduct = Storage<CartProductEntity>.ReadFirst(new CartProductEntity.ReadById { CartProductId = cartProductId });
            if (cartProduct != null) {
                ProductEntity product = Storage<ProductEntity>.ReadFirst(new ProductEntity.ReadById { ProductId = cartProduct.ProductId });
                Storage<CartProductEntity>.Delete(cartProduct);

                if (product.InternalStorageCount != ProductEntity.INTERNAL_STORAGE_NOT_AVAILABLE) {
                    product.InternalStorageCount = product.InternalStorageCount + cartProduct.Quantity;
                    Storage<ProductEntity>.Update(product);
                }

                return true;
            }

            return false;
        }
        /// <summary>
        /// Vráti nákupný košík aktuálne prihláseného používateľa alebo
        /// anonýmneho používateľa (anonýmny košík)
        /// </summary>
        public static CartEntity GetAccountCart(Page page) {
            CartEntity cart = null;
            int sessionId = GetAnonymousSessionId(page);

            if (Security.IsLogged(false)) {
                cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadOpenByAccount { AccountId = Security.Account.Id });
                if (cart == null) {
                    //Zistim, ci pouzivatel nema vtvoreny kosik ako anonymny pouzivatel.
                    cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadBySessionId { SessionId = sessionId });
                    if (cart == null) return null;
                    if (!cart.AccountId.HasValue) {
                        //Vytvorevie pouzivatelsky nakupny kosik z anonymneho.
                        cart.AccountId = Security.Account.Id;
                        cart.SessionId = null;
                        Storage<CartEntity>.Update(cart);
                    }
                }
            } else {
                //Anonymny nakupny kosik.
                cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadBySessionId { SessionId = sessionId });
            }
            return cart;
        }

        /// <summary>
        /// Prepocitanie kosika
        /// </summary>
        /// <param name="page"></param>
        public static Eurona.Common.CernyForLifeUserMarzeInfo RecalculateCart(Page page, int cartId) {
            CartEntity cart = cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadById { CartId = cartId });

            //Metoda prepocita Eurona produkty v kosiku.
            Eurona.Common.CernyForLifeUserMarzeInfo umi = UpdateEuronaProductInCart(page, cart.Id, cart.AccountId);
            cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadById { CartId = cart.Id });

            //Update Cart Price
            decimal price = 0m;
            decimal priceWVAT = 0m;
            foreach (CartProductEntity cp in cart.CartProducts) {
                price += cp.PriceTotal;
                priceWVAT += cp.PriceTotalWVAT;
            }
            cart.PriceTotal = price;
            cart.PriceTotalWVAT = priceWVAT;
            Storage<CartEntity>.Update(cart);

            return umi;
        }

        public static Eurona.Common.CernyForLifeUserMarzeInfo RecalculateOpenCart(Page page) {
            CartEntity cart = null;
            int sessionId = GetAnonymousSessionId(page);

            if (Security.IsLogged(false)) {
                cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadOpenByAccount { AccountId = Security.Account.Id });
                if (cart == null) {
                    //Zistim, ci pouzivatel nema vtvoreny kosik ako anonymny pouzivatel.
                    cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadBySessionId { SessionId = sessionId });
                    if (cart == null) {
                        cart = new CartEntity();
                        cart.AccountId = Security.Account.Id;
                        Storage<CartEntity>.Create(cart);
                    } else {
                        //Vytvorevie pouzivatelsky nakupny kosik z anonymneho.
                        cart.AccountId = Security.Account.Id;
                        cart.SessionId = null;
                        Storage<CartEntity>.Update(cart);
                    }
                }
            } else {
                //Anonymny nakupny kosik.
                cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadBySessionId { SessionId = sessionId });
                if (cart == null) {
                    cart = new CartEntity();
                    cart.SessionId = sessionId;
                    Storage<CartEntity>.Create(cart);
                }
            }

            //Metoda prepocita Eurona produkty v kosiku.
            Eurona.Common.CernyForLifeUserMarzeInfo umi = UpdateEuronaProductInCart(page, cart.Id, cart.AccountId);
            cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadById { CartId = cart.Id });

            //Update Cart Price
            decimal price = 0m;
            decimal priceWVAT = 0m;
            foreach (CartProductEntity cp in cart.CartProducts) {
                price += cp.PriceTotal;
                priceWVAT += cp.PriceTotalWVAT;
            }
            cart.PriceTotal = price;
            cart.PriceTotalWVAT = priceWVAT;
            Storage<CartEntity>.Update(cart);

            return umi;
        }

        /// <summary>
        /// Metoda prepocita Eurona produkty v kosiku.
        /// </summary>
        public static Eurona.Common.CernyForLifeUserMarzeInfo UpdateEuronaProductInCart(Page page, int cartId, int? accountId) {
            //!!! ZATIAL JE TATO VETVA ODPOJENA !!!
            /*
            //Ak je pouzivatel prihlaseny
            if ( Security.IsLogged( false ) || accountId.HasValue )
            {
                    Organization org = Storage<Organization>.ReadFirst( new Organization.ReadByAccountId { AccountId = accountId.Value } );
                    if ( org == null || org.TVD_Id.HasValue == false ) return null;

                    string message = string.Empty;
                    Eurona.Common.CernyForLifeUserMarzeInfo umi = new CernyForLifeUserMarzeInfo( page, org.TVD_Id.Value );
                    umi.GetMarzeInfoFromEurosap( cartId, out message );
                    return umi;
            }
            */
            return null;
        }
    }
}
