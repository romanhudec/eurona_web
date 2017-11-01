using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AccountEntity = Eurona.DAL.Entities.Account;
using AddressEntity = CMS.Entities.Address;
using OrganizationEntity = Eurona.Common.DAL.Entities.Organization;
using OrderEntity = Eurona.DAL.Entities.Order;
using CartEntity = Eurona.Common.DAL.Entities.Cart;
using CartProductEntity = Eurona.Common.DAL.Entities.CartProduct;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Web.UI;
using CMS;
using System.Security.Principal;
using System.Diagnostics;
using System.Text;

namespace Eurona.Controls {
    public static class CartOrderHelper {
        private static void ClearTVDTables(int recalcId) {
            string connectionString = ConfigurationManager.ConnectionStrings["TVDConnectionString"].ConnectionString;
            CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(connectionString);
            using (SqlConnection connection = tvdStorage.Connect()) {
                //Clear data
                tvdStorage.Exec(connection, "DELETE FROM www_prepocty_radky WHERE id_prepoctu=@id_prepoctu", new SqlParameter("@id_prepoctu", recalcId));
                tvdStorage.Exec(connection, "DELETE FROM www_faktury_radky  WHERE id_prepoctu=@id_prepoctu", new SqlParameter("@id_prepoctu", recalcId));
                tvdStorage.Exec(connection, "DELETE FROM www_faktury WHERE id_prepoctu=@id_prepoctu", new SqlParameter("@id_prepoctu", recalcId));
                tvdStorage.Exec(connection, "DELETE FROM www_prepocty WHERE id_prepoctu=@id_prepoctu", new SqlParameter("@id_prepoctu", recalcId));
            }
        }

        public static DataTable GetTVDFaktura(OrderEntity order) {
            string connectionString = ConfigurationManager.ConnectionStrings["TVDConnectionString"].ConnectionString;
            CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(connectionString);
            using (SqlConnection connection = tvdStorage.Connect()) {
                //Clear data
                DataTable dt = tvdStorage.Query(connection, "SELECT celkem_k_uhrade=([celkem_k_uhrade]-[castka_dobropis]), kod_meny FROM www_faktury WHERE id_prepoctu=@id_prepoctu", new SqlParameter("@id_prepoctu", order.Id));
                return dt;
            }
        }

        private static void RollBackTVDTransaction() {
            string connectionString = ConfigurationManager.ConnectionStrings["TVDConnectionString"].ConnectionString;
            CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(connectionString);
            using (SqlConnection connection = tvdStorage.Connect()) {
                //Rollback
                tvdStorage.Exec(connection, "IF @@trancount >0 ROLLBACK TRANSACTION");
            }
        }

        /// <summary>
        /// Metóda vykoná samotný prepočet. 
        /// </summary>
        private static string CalRecalculate(CMS.Pump.MSSQLStorage tvdStorage, SqlConnection connection, int recalcId, out bool bSuccess) {
#if __DEBUG_VERSION_WITHOUTTVD
			bSuccess = true;
			return string.Empty;
#endif
            //===============================================================================
            // VYKONANIE PREPOCTU
            //===============================================================================
            //@out_Probehlo OUTPUT    -- 0=chyba, 1=OK
            //,@out_Zprava OUTPUT    -- v případě chyby ev. zpráva k chybě, jinak 'OK'
            //,@out_Id_prepoctu OUTPUT    -- odkaz na hlavní fakturu ve výstupu www_faktury (shodné s hlavní objednávkou na vstupu)

            //,@Id_prepoctu    -- vstupní id_prepoctu z www_prepocty (hlavní objednávka)

            SqlParameter probehlo = new SqlParameter("@out_Probehlo", false);
            probehlo.Direction = ParameterDirection.Output;

            SqlParameter zprava = new SqlParameter("@out_Zprava", string.Empty);
            zprava.Direction = ParameterDirection.Output;
            zprava.SqlDbType = SqlDbType.VarChar;
            zprava.Size = 255;

            SqlParameter id_prepoctu = new SqlParameter("@out_Id_prepoctu", -1);
            id_prepoctu.Direction = ParameterDirection.Output;

            try {
                tvdStorage.ExecProc(connection, "esp_www_prepocet",
                        new SqlParameter("Id_prepoctu", recalcId), // DOPLNIT
                        probehlo, zprava, id_prepoctu);
            } catch (Exception ex) {
                CMS.EvenLog.WritoToEventLog(ex);
                bSuccess = false;
                if (zprava.Value != null) return zprava.Value.ToString();
                return "Eurosap odmítl objednávku spracovat!";
            }

            //===============================================================================
            //Vystupne parametra
            //===============================================================================
            //Probehlo	bit	1=úspěch, 0=chyba		
            //Zprava	varchar(255)	text chyby		
            //id_prepoctu	int	prim. klíč		
            bSuccess = Convert.ToBoolean(probehlo.Value);
            if (zprava != null && zprava.Value != null) {
                string msg = string.Format("TVD Prepocet -> esp_www_prepocet(id_prepoctu:{0}) = {1}", recalcId, zprava.Value.ToString());
                CMS.EvenLog.WritoToEventLog(msg, EventLogEntryType.Information);
            }
            return zprava.Value.ToString();
        }

        /// <summary>
        /// Metóda vráti výsledok prepočtu [TVD->Faktury]
        /// </summary>
        private static DataRow GetRecalcResult(CMS.Pump.MSSQLStorage tvdStorage, SqlConnection connection, int recalcId) {
            string sql = @"SELECT id_odberatele, celkem_k_uhrade=([celkem_k_uhrade]-[castka_dobropis]), zaklad_zs, sdeleni_pro_poradce_html, cislo_objednavky_eurosap, var_symbol_eurosap, potvrzeno, id_sdruzeneho_prepoctu, kod_meny
								FROM www_faktury WHERE id_prepoctu=@id_prepoctu";
            DataTable dt = tvdStorage.Query(connection, sql, new SqlParameter("@id_prepoctu", recalcId));
            if (dt.Rows.Count == 0) return null;
            return dt.Rows[0];
        }
        /// <summary>
        /// Metóda vráti výsledok prepočtu [TVD->Faktury->radky] kosiku
        /// </summary>
        private static DataRow GetRecalcResultRadkySumar(CMS.Pump.MSSQLStorage tvdStorage, SqlConnection connection, int recalcId) {
            string sql = @"SELECT cena_fakt_sdph = SUM(mnozstvi * cena_mj_fakt_sdph ), cena_katalogova = SUM(mnozstvi * cena_mj_katalogova), zapocet_body = SUM(mnozstvi *zapocet_mj_body)
								FROM www_faktury_radky WHERE id_prepoctu=@id_prepoctu";
            DataTable dt = tvdStorage.Query(connection, sql, new SqlParameter("@id_prepoctu", recalcId));
            if (dt.Rows.Count == 0) return null;
            return dt.Rows[0];
        }
        /// <summary>
        /// Metóda vráti výsledok prepočtu [TVD->Faktury_radky] produktov v kosiku
        /// </summary>
        private static DataRowCollection GetRecalcResultRadky(CMS.Pump.MSSQLStorage tvdStorage, SqlConnection connection, int recalcId) {
            string sql = @"SELECT id_prepoctu, C_zbo, mnozstvi, cena_mj_fakt_sdph, cena_mj_katalogova, idakce
								FROM www_faktury_radky WHERE id_prepoctu=@id_prepoctu";
            DataTable dt = tvdStorage.Query(connection, sql, new SqlParameter("@id_prepoctu", recalcId));
            if (dt.Rows.Count == 0) return null;
            return dt.Rows;
        }

        /// <summary>
        /// Metóda vráti výsledok prepočtu [TVD->Faktury_slevy] produktov v kosiku
        /// </summary>
        public static DataTable GetRecalcResultSlevy(int recalcId, int idProduktu) {
            string connectionString = ConfigurationManager.ConnectionStrings["TVDConnectionString"].ConnectionString;
            CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(connectionString);
            using (SqlConnection connection = tvdStorage.Connect()) {
                string sql = @"WITH slevy as (
	                    SELECT id_prepoctu, Kod_polozky, p.Universal_nazev, mnozstvi, Cena_mj_fakt_sdph = Cena_mj_fakt_sdph*mnozstvi, idakce,
	                    C_Zbo = (SELECT Kod_polozky FROM www_faktury_slevy WHERE id_prepoctu = s.id_prepoctu AND Poradi = s.Poradi_vazaneho_radku)
	                    FROM www_faktury_slevy s
	                    INNER JOIN Produkty p ON p.C_Zbo = s.Kod_polozky
	                    WHERE /*Poradi_vazaneho_radku IS NOT NULL AND*/ idakce!=10 AND 
	                    Typ_polozky='S'
                    )
                    SELECT * FROM slevy where id_prepoctu=@id_prepoctu AND (C_Zbo=@id_produktu OR C_Zbo IS NULL)";
                DataTable dt = tvdStorage.Query(connection, sql, new SqlParameter("@id_prepoctu", recalcId), new SqlParameter("@id_produktu", idProduktu));
                if (dt.Rows.Count == 0) return null;
                return dt;
            }
        }
        /// <summary>
        /// Metóda vráti poslednu informáciu pre poradcu
        /// </summary>
        private static string GetRecalcMessage(CMS.Pump.MSSQLStorage tvdStorage, SqlConnection connection, int TVD_Id) {
            string sql = @"SELECT TOP 1 id_odberatele, celkem_k_uhrade=([celkem_k_uhrade]-[castka_dobropis]), zaklad_zs, sdeleni_pro_poradce_html, cislo_objednavky_eurosap, var_symbol_eurosap, potvrzeno, id_sdruzeneho_prepoctu, kod_meny
								FROM www_faktury WHERE id_odberatele=@id_odberatele
								ORDER BY cislo_objednavky_eurosap DESC";
            DataTable dt = tvdStorage.Query(connection, sql, new SqlParameter("@id_odberatele", TVD_Id));
            if (dt.Rows.Count == 0) return string.Empty;
            return dt.Rows[0]["sdeleni_pro_poradce_html"].ToString();
        }

        public static string UhradaDobirkou(OrderEntity order) {
            //===============================================================================
            // ZAPIS PLATBY DOBIRKOU
            //===============================================================================
            /*
            esp_www_platbadobirkou
            (
            @out_Probehlo bit OUTPUT,
            @out_Zprava nvarchar(255) OUTPUT,

            @Id_prepoctu int
            )
            */
            string connectionString = ConfigurationManager.ConnectionStrings["TVDConnectionString"].ConnectionString;
            CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(connectionString);

            SqlParameter probehlo = new SqlParameter("@out_Probehlo", false);
            probehlo.Direction = ParameterDirection.Output;

            SqlParameter zprava = new SqlParameter("@out_Zprava", string.Empty);
            zprava.Direction = ParameterDirection.Output;
            zprava.SqlDbType = SqlDbType.VarChar;
            zprava.Size = 255;
            bool bSuccess = false;
            try {
                using (SqlConnection connection = tvdStorage.Connect()) {
                    tvdStorage.ExecProc(connection, "esp_www_platbadobirkou", new SqlParameter("Id_prepoctu", order.Id), probehlo, zprava);
                }
            } catch (Exception ex) {
                CMS.EvenLog.WritoToEventLog(ex);
                bSuccess = false;
                if (zprava.Value != null) return zprava.Value.ToString();
                return "Eurosap odmítl platbu dobírkou spracovat!";
            }

            //===============================================================================
            //Vystupne parametra
            //===============================================================================
            //Probehlo	bit	1=úspěch, 0=chyba		
            //Zprava	varchar(255)	text chyby		
            //id_prepoctu	int	prim. klíč		
            bSuccess = Convert.ToBoolean(probehlo.Value);
            if (zprava != null && zprava.Value != null) {
                string msg = string.Format("TVD Platba dobírkou -> esp_www_platbadobirkou(Id_prepoctu:{0}) = {1}", order.Id, zprava.Value.ToString());
                CMS.EvenLog.WritoToEventLog(msg, EventLogEntryType.Information);
            }
            return zprava.Value.ToString();
        }
        /// <summary>
        /// Metóda prepočíta nákupný košík v TVD databazi
        /// </summary>
        public static string RecalculateTVDCart(Page page, UpdatePanel up, CartEntity cart, out bool bSuccess) {
            int cartId = -1 * (cart.Id);
            //#if _LOCAL_ORDER
            //            return "OK";
            //#endif
            string message = string.Empty;
            AccountEntity account = Storage<AccountEntity>.ReadFirst(new AccountEntity.ReadById { AccountId = cart.AccountId.Value });
            OrganizationEntity organization = Storage<OrganizationEntity>.ReadFirst(new OrganizationEntity.ReadByAccountId { AccountId = account.Id });
            if (!account.TVD_Id.HasValue) {
                message = "Uživatel není platným uživatelem Eurosap!";
                bSuccess = false;
                return message;
            }
            try {
                bool zadano_operatorem = account.IsInRole(Eurona.DAL.Entities.Role.OPERATOR);

                //Vymazanie starych zaznamov pre toto ID
                ClearTVDTables(cartId);

                //Adresa pre dorucenie
                AddressEntity address = new AddressEntity();
                if (string.IsNullOrEmpty(organization.CorrespondenceAddress.Street)) address = organization.RegisteredAddress;
                else address = organization.CorrespondenceAddress;

                //Marze Intensa
                decimal? procento_marze_intensa = null;
                if (cart.HasCernyForLifeProducts) {
                    procento_marze_intensa = cart.Discount.HasValue ? cart.Discount.Value : 0m;
                    if (procento_marze_intensa == 0m) CMS.EvenLog.WritoToEventLog(new InvalidOperationException("RecalculateTVDCart->procento_marze_intensa == 0m"));
                }


                string connectionString = ConfigurationManager.ConnectionStrings["TVDConnectionString"].ConnectionString;
                CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(connectionString);
                using (SqlConnection connection = tvdStorage.Connect()) {
                    // NAPLNENIE TVD STRUKTUR
                    string sql = @"INSERT INTO www_prepocty (id_prepoctu ,id_odberatele ,datum ,potvrzeno ,
										zpusob_dodani ,zpusob_platby ,poznamka , id_sdruzeneho_prepoctu, dor_telefon, dor_email,
										dor_nazev_firmy ,dor_nazev_firmy_radek ,dor_ulice ,dor_misto ,dor_psc ,dor_stat, zadano_operatorem, bez_postovneho, procento_marze_intensa) 
										VALUES (@id_prepoctu ,@id_odberatele ,@datum ,@potvrzeno ,
										@zpusob_dodani ,@zpusob_platby ,@poznamka , @id_sdruzeneho_prepoctu, @dor_telefon, @dor_email,
										@dor_nazev_firmy ,@dor_nazev_firmy_radek ,@dor_ulice ,@dor_misto ,@dor_psc ,@dor_stat, @zadano_operatorem, @bez_postovneho, @procento_marze_intensa)";
                    tvdStorage.Exec(connection, sql,
                            new SqlParameter("@id_prepoctu", cartId),
                            new SqlParameter("@id_odberatele", account.TVD_Id.Value),
                            new SqlParameter("@datum", Eurona.Common.Application.CurrentOrderDate),//DateTime.Now.AddDays(-1) ),
                            new SqlParameter("@potvrzeno", false),
                            new SqlParameter("@zpusob_dodani", 1/*DBNull.Value*/ ),
                            new SqlParameter("@zpusob_platby", 1/*Zatial vzdy 1*//*DBNull.Value*/ ),
                            new SqlParameter("@poznamka", DBNull.Value),
                            new SqlParameter("@id_sdruzeneho_prepoctu", DBNull.Value),
                            new SqlParameter("@dor_nazev_firmy", GetString(organization.Name, 25)),
                            new SqlParameter("@dor_nazev_firmy_radek", string.Empty),
                            new SqlParameter("@dor_ulice", GetString(address.Street, 25)),
                            new SqlParameter("@dor_misto", GetString(address.City, 25)),
                            new SqlParameter("@dor_psc", address.Zip),
                            new SqlParameter("@dor_stat", address.State),
                            new SqlParameter("@dor_telefon", ""),
                            new SqlParameter("@dor_email", ""),
                            new SqlParameter("@zadano_operatorem", zadano_operatorem),
                            new SqlParameter("@bez_postovneho", false),
                            new SqlParameter("@procento_marze_intensa", procento_marze_intensa.HasValue ? (object)procento_marze_intensa.Value : DBNull.Value)
                            );

                    //Naplnenie riadkov (produktov) pre prepocet
                    foreach (CartProductEntity product in cart.CartProducts) {
                        sql = @"INSERT INTO www_prepocty_radky ( id_prepoctu, C_zbo, mnozstvi, cerpat_BK ) VALUES (@id_prepoctu, @C_zbo, @mnozstvi, @cerpatBK)";
                        tvdStorage.Exec(connection, sql,
                                new SqlParameter("@id_prepoctu", cartId),
                                new SqlParameter("@C_zbo", product.ProductId),
                                new SqlParameter("@mnozstvi", product.Quantity),
                                new SqlParameter("@cerpatBK ", product.CerpatBonusoveKredity)
                                );
                    }

                    //Prepocitanie Nakupneho kosika/Objednavky/Zdruzenej objednavky
                    string errorMessage = CalRecalculate(tvdStorage, connection, cartId, out bSuccess);
                    if (!bSuccess) {
                        string js = string.Format("alert('Synchronizace se vzdáleným servrem (SAP) byla neúspěšná! Chyba: " + errorMessage + "');");
                        if (up == null) page.ClientScript.RegisterStartupScript(page.GetType(), "TVDSyncCart", js, true);
                        else ScriptManager.RegisterStartupScript(up, up.GetType(), "TVDSyncCart", js, true);
                        return null;
                    }

                    //Nacitanie vystupu po prepocitani Nakupneho kosika/Objednavky/Zdruzenej objednavky
                    DataRow row = GetRecalcResult(tvdStorage, connection, cartId);
                    if (row != null) {
                        message = row["sdeleni_pro_poradce_html"].ToString();
                        string kodMeny = row["kod_meny"].ToString();

                        //Update Cart
                        cart.PriceTotalWVAT = Convert.ToDecimal(row["celkem_k_uhrade"]);
                        cart.PriceTotal = Convert.ToDecimal(row["zaklad_zs"]);
                        Storage<CartEntity>.Update(cart);
                    }

                    row = GetRecalcResultRadkySumar(tvdStorage, connection, cartId);
                    if (row != null) {
                        //cena_fakt_sdph = SUM(mnozstvi * cena_mj_fakt_sdph ), 
                        //cena_katalogova = SUM(mnozstvi * cena_mj_katalogova), 
                        //zapocet_body = SUM(mnozstvi *zapocet_mj_body)

                        //Update Cart
                        cart.KatalogovaCenaCelkemByEurosap = Convert.ToDecimal(row["cena_katalogova"]);
                        cart.BodyEurosapTotal = Convert.ToInt32(row["zapocet_body"]);
                        Storage<CartEntity>.Update(cart);
                    }

                    if (cart != null) {
                        DataRowCollection fakturyRadky = GetRecalcResultRadky(tvdStorage, connection, cartId);
                        if (fakturyRadky != null) {
                            foreach (DataRow rowRadek in fakturyRadky) {
                                //id_prepoctu, C_zbo, mnozstvi, cena_mj_fakt_sdph, cena_mj_katalogova
                                int c_zbo = Convert.ToInt32(rowRadek["C_zbo"]);
                                int mnozstvi = Convert.ToInt32(rowRadek["mnozstvi"]);
                                decimal cena_mj_fakt_sdph = Convert.ToDecimal(rowRadek["cena_mj_fakt_sdph"]);
                                decimal cena_mj_katalogova = Convert.ToDecimal(rowRadek["cena_mj_katalogova"]);

                                //Naplnenie riadkov (produktov) pre prepocet
                                foreach (CartProductEntity product in cart.CartProducts) {
                                    if (product.ProductId != c_zbo) continue;

                                    product.PriceWVAT = cena_mj_fakt_sdph;
                                    product.PriceTotalWVAT = (decimal)mnozstvi * cena_mj_fakt_sdph;
                                    Storage<CartProductEntity>.Update(product);
                                }
                            }
                        }
                    }

                }
            } catch (SqlException ex) {
                bSuccess = false;
                CMS.EvenLog.WritoToEventLog(ex);
                if (ex.Number == -2) {

                    RollBackTVDTransaction();

                    string js = string.Format("alert('Synchronizace se vzdáleným servrem (SAP) byla neúspěšná! Chyba: Servere neodpovídá!');");
                    if (up == null) page.ClientScript.RegisterStartupScript(page.GetType(), "TVDSyncCart", js, true);
                    else ScriptManager.RegisterStartupScript(up, up.GetType(), "TVDSyncCart", js, true);
                    return null;
                } else {
                    string js = string.Format("alert('Synchronizace se vzdáleným servrem (SAP) byla neúspěšná!');");
                    if (up == null) page.ClientScript.RegisterStartupScript(page.GetType(), "TVDSyncCart", js, true);
                    else ScriptManager.RegisterStartupScript(up, up.GetType(), "TVDSyncCart", js, true);
                    return null;
                }
            } catch (Exception exx) {
                bSuccess = false;
                CMS.EvenLog.WritoToEventLog(exx);

                string js = string.Format("alert('Synchronizace se vzdáleným servrem (SAP) byla neúspěšná!');");
                if (up == null) page.ClientScript.RegisterStartupScript(page.GetType(), "TVDSyncCart", js, true);
                else ScriptManager.RegisterStartupScript(up, up.GetType(), "TVDSyncCart", js, true);
                return null;
            }

            return message;
        }

        /// <summary>
        /// Metóda prepočíta objednávku v TVD databazi
        /// </summary>
        public static bool RecalculateTVDOrder(Page page, UpdatePanel up, OrderEntity order, bool confirm) {
#if __DEBUG_VERSION_WITHOUTTVD
            return true;
#endif
            if (!SyncTVDOrder(page, up, order, confirm)) return false;

            OrderEntity.ReadByFilter filter = new OrderEntity.ReadByFilter();
            filter.ParentId = order.Id;
            List<OrderEntity> list = Storage<OrderEntity>.Read(filter);
            foreach (OrderEntity o in list) {
                o.ParentId = order.Id;
                o.ShipmentCode = order.ShipmentCode;
                o.OrderStatusCode = order.OrderStatusCode;
                if (!SyncTVDOrder(page, up, o, confirm)) return false;
            }

            string connectionString = ConfigurationManager.ConnectionStrings["TVDConnectionString"].ConnectionString;
            CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(connectionString);
            using (SqlConnection connection = tvdStorage.Connect()) {
                //Prepocitanie Nakupneho kosika/Objednavky/Zdruzenej objednavky
                bool bSuccess = false;
                string errorMessage = CalRecalculate(tvdStorage, connection, order.Id, out bSuccess);
                if (!bSuccess) {
                    string js = string.Format("alert('Synchronizace se vzdáleným servrem (SAP) byla neúspěšná! Chyba: " + errorMessage + "');");
                    if (up == null) page.ClientScript.RegisterStartupScript(page.GetType(), "TVDSyncOrder", js, true);
                    else ScriptManager.RegisterStartupScript(up, up.GetType(), "TVDSyncOrder", js, true);
                    return false;
                }

                //Nacitanie vystupu po prepocitani Nakupneho kosika/Objednavky/Zdruzenej objednavky
                DataRow row = GetRecalcResult(tvdStorage, connection, order.Id);
                if (row != null) {
                    string message = row["sdeleni_pro_poradce_html"].ToString();
                    string kodMeny = row["kod_meny"].ToString();
                    SHP.Entities.Classifiers.Currency currency = Storage<SHP.Entities.Classifiers.Currency>.ReadFirst(new SHP.Entities.Classifiers.Currency.ReadByCode { Code = kodMeny });
                    if (currency != null)
                        order.CurrencyId = currency.Id;

                    //Update Order
                    order.PriceWVAT = Convert.ToDecimal(row["celkem_k_uhrade"]);
                    order.Price = Convert.ToDecimal(row["zaklad_zs"]);
                    order.Notes = message;
                    Storage<OrderEntity>.Update(order);
                }

                CartEntity cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadById { CartId = order.CartId });
                if (cart != null) {
                    DataRowCollection fakturyRadky = GetRecalcResultRadky(tvdStorage, connection, order.Id);
                    if (fakturyRadky != null) {
                        foreach (DataRow rowRadek in fakturyRadky) {
                            //id_prepoctu, C_zbo, mnozstvi, cena_mj_fakt_sdph, cena_mj_katalogova
                            int c_zbo = Convert.ToInt32(rowRadek["C_zbo"]);
                            int mnozstvi = Convert.ToInt32(rowRadek["mnozstvi"]);
                            decimal cena_mj_fakt_sdph = Convert.ToInt32(rowRadek["cena_mj_fakt_sdph"]);
                            decimal cena_mj_katalogova = Convert.ToInt32(rowRadek["cena_mj_katalogova"]);
                            int idakce = Convert.ToInt32(rowRadek["idakce"]);
                            //Naplnenie riadkov (produktov) pre prepocet
                            foreach (CartProductEntity product in cart.CartProducts) {
                                if (product.ProductId != c_zbo) continue;

                                product.PriceWVAT = cena_mj_fakt_sdph;
                                product.PriceTotalWVAT = mnozstvi * cena_mj_fakt_sdph;
                                Storage<CartProductEntity>.Update(product);
                            }
                        }
                    }
                }
            }

            return true;
        }

        private static bool SyncTVDOrder(Page page, UpdatePanel up, OrderEntity order, bool confirm) {
            string message = string.Empty;
            AccountEntity account = Storage<AccountEntity>.ReadFirst(new AccountEntity.ReadById { AccountId = order.AccountId });
            OrganizationEntity organization = Storage<OrganizationEntity>.ReadFirst(new OrganizationEntity.ReadByAccountId { AccountId = account.Id });
            if (!account.TVD_Id.HasValue) {
                message = "Uživatel není platným uživatelem Eurosap!";
                string js = string.Format("alert('{0}');", message);
                if (up == null) page.ClientScript.RegisterStartupScript(page.GetType(), "SyncTVDOrder", js, true);
                else ScriptManager.RegisterStartupScript(up, up.GetType(), "SyncTVDOrder", js, true);
                return false;
            }
            try {
                //GET KOSUK OBJEDNACKY
                CartEntity cart = Storage<CartEntity>.ReadFirst(new CartEntity.ReadById { CartId = order.CartId });
                decimal? procento_marze_intensa = null;
                if (cart.HasCernyForLifeProducts) {
                    procento_marze_intensa = cart.Discount.HasValue ? cart.Discount.Value : 0m;
                    if (procento_marze_intensa == 0m) CMS.EvenLog.WritoToEventLog(new InvalidOperationException("SyncTVDOrder->procento_marze_intensa == 0m"));
                }

                AccountEntity accountCreated = Storage<AccountEntity>.ReadFirst(new AccountEntity.ReadById { AccountId = order.CreatedByAccountId });
                bool zadano_operatorem = accountCreated.IsInRole(Eurona.DAL.Entities.Role.OPERATOR);

                //Vymazanie starych zaznamov pre toto ID
                ClearTVDTables(order.Id);

                //Adresa pre dorucenie
                AddressEntity address = new AddressEntity();
                if (string.IsNullOrEmpty(organization.CorrespondenceAddress.Street))
                    address = organization.RegisteredAddress;
                else address = organization.CorrespondenceAddress;

                string connectionString = ConfigurationManager.ConnectionStrings["TVDConnectionString"].ConnectionString;
                CMS.Pump.MSSQLStorage tvdStorage = new CMS.Pump.MSSQLStorage(connectionString);
                using (SqlConnection connection = tvdStorage.Connect()) {
                    // NAPLNENIE TVD STRUKTUR
                    string sql = @"INSERT INTO www_prepocty (id_prepoctu ,id_odberatele ,datum ,potvrzeno ,
										zpusob_dodani ,zpusob_platby ,poznamka , id_sdruzeneho_prepoctu , dor_telefon, dor_email,
										dor_nazev_firmy ,dor_nazev_firmy_radek ,dor_ulice ,dor_misto ,dor_psc ,dor_stat, dodat_od, dodat_do, zadano_operatorem, bez_postovneho, procento_marze_intensa, id_web_objednavky) 
										VALUES (@id_prepoctu ,@id_odberatele ,@datum ,@potvrzeno ,
										@zpusob_dodani ,@zpusob_platby ,@poznamka , @id_sdruzeneho_prepoctu , @dor_telefon, @dor_email,
										@dor_nazev_firmy ,@dor_nazev_firmy_radek ,@dor_ulice ,@dor_misto ,@dor_psc ,@dor_stat, @dodat_od, @dodat_do, @zadano_operatorem, @bez_postovneho, @procento_marze_intensa, @id_web_objednavky)";
                    tvdStorage.Exec(connection, sql,
                            new SqlParameter("@id_prepoctu", order.Id),
                            new SqlParameter("@id_odberatele", account.TVD_Id.Value),
                            new SqlParameter("@datum", /*order.OrderDate*/DateTime.Now),
                            new SqlParameter("@potvrzeno", confirm),
                            new SqlParameter("@zpusob_dodani", order.ShipmentCode),
                            new SqlParameter("@zpusob_platby", 1/*Zatial vzdy 1*//*DBNull.Value*/ ),
                            new SqlParameter("@poznamka", order.DeliveryAddress.Notes),
                            new SqlParameter("@id_sdruzeneho_prepoctu", order.ParentId.HasValue ? (object)order.ParentId.Value : DBNull.Value),
                            new SqlParameter("@dor_nazev_firmy", GetString(organization.Name, 25)),
                            new SqlParameter("@dor_nazev_firmy_radek", organization.Name != order.DeliveryAddress.Organization ? GetString(order.DeliveryAddress.Organization, 25) : string.Empty),
                            new SqlParameter("@dor_ulice", GetString(order.DeliveryAddress.Street, 25)),
                            new SqlParameter("@dor_misto", GetString(order.DeliveryAddress.City, 25)),
                            new SqlParameter("@dor_psc", order.DeliveryAddress.Zip),
                            new SqlParameter("@dor_stat", order.DeliveryAddress.State),
                            new SqlParameter("@dodat_od", order.ShipmentFrom.HasValue ? order.ShipmentFrom.Value : (object)DBNull.Value),
                            new SqlParameter("@dodat_do", order.ShipmentTo.HasValue ? order.ShipmentTo.Value : (object)DBNull.Value),
                            new SqlParameter("@dor_telefon", order.DeliveryAddress.Phone),
                            new SqlParameter("@dor_email", order.DeliveryAddress.Email),
                            new SqlParameter("@zadano_operatorem", zadano_operatorem),
                            new SqlParameter("@bez_postovneho", order.NoPostage),
                            new SqlParameter("@procento_marze_intensa", procento_marze_intensa.HasValue ? (object)procento_marze_intensa.Value : DBNull.Value),
                            new SqlParameter("@id_web_objednavky", order.OrderNumber)
                            );

                    //Naplnenie riadkov (produktov) pre prepocet
                    foreach (CartProductEntity product in cart.CartProducts) {
                        sql = @"INSERT INTO www_prepocty_radky ( id_prepoctu, C_zbo, mnozstvi, cerpat_BK ) VALUES (@id_prepoctu, @C_zbo, @mnozstvi, @cerpatBK)";
                        tvdStorage.Exec(connection, sql,
                                new SqlParameter("@id_prepoctu", order.Id),
                                new SqlParameter("@C_zbo", product.ProductId),
                                new SqlParameter("@mnozstvi", product.Quantity),
                                new SqlParameter("@cerpatBK ", product.CerpatBonusoveKredity)
                                );
                    }

                    //Nacitanie vystupu po prepocitani Nakupneho kosika/Objednavky/Zdruzenej objednavky
                    DataRow row = GetRecalcResult(tvdStorage, connection, order.Id);
                    if (row != null) {
                        message = row["sdeleni_pro_poradce_html"].ToString();
                        string kodMeny = row["kod_meny"].ToString();

                        //Update Order
                        order.PriceWVAT = Convert.ToDecimal(row["celkem_k_uhrade"]);
                        order.Price = Convert.ToDecimal(row["zaklad_zs"]);
                        Storage<OrderEntity>.Update(order);
                    }

                    if (cart != null) {
                        DataRowCollection fakturyRadky = GetRecalcResultRadky(tvdStorage, connection, order.Id);
                        if (fakturyRadky != null) {
                            foreach (DataRow rowRadek in fakturyRadky) {
                                //id_prepoctu, C_zbo, mnozstvi, cena_mj_fakt_sdph, cena_mj_katalogova
                                int c_zbo = Convert.ToInt32(rowRadek["C_zbo"]);
                                int mnozstvi = Convert.ToInt32(rowRadek["mnozstvi"]);
                                decimal cena_mj_fakt_sdph = Convert.ToInt32(rowRadek["cena_mj_fakt_sdph"]);
                                decimal cena_mj_katalogova = Convert.ToInt32(rowRadek["cena_mj_katalogova"]);
                                int idakce = Convert.ToInt32(rowRadek["idakce"]);
                                //Naplnenie riadkov (produktov) pre prepocet
                                foreach (CartProductEntity product in cart.CartProducts) {
                                    if (product.ProductId != c_zbo) continue;

                                    product.PriceWVAT = cena_mj_fakt_sdph;
                                    product.PriceTotalWVAT = mnozstvi * cena_mj_fakt_sdph;
                                    Storage<CartProductEntity>.Update(product);
                                }
                            }
                        }
                    }
                }
            } catch (SqlException ex) {
                CMS.EvenLog.WritoToEventLog(ex);
                if (ex.Number == -2) {
                    RollBackTVDTransaction();

                    string js = string.Format("alert('Synchronizace se vzdáleným servrem (SAP) byla neúspěšná! Chyba: Servere neodpovídá!');");
                    if (up == null) page.ClientScript.RegisterStartupScript(page.GetType(), "TVDSyncOrder", js, true);
                    else ScriptManager.RegisterStartupScript(up, up.GetType(), "TVDSyncOrder", js, true);
                    return false;
                } else {
                    string js = string.Format("alert('Synchronizace se vzdáleným servrem (SAP) byla neúspěšná!');");
                    if (up == null) page.ClientScript.RegisterStartupScript(page.GetType(), "TVDSyncOrder", js, true);
                    else ScriptManager.RegisterStartupScript(up, up.GetType(), "TVDSyncOrder", js, true);
                    return false;
                }
            } catch (Exception exx) {
                CMS.EvenLog.WritoToEventLog(exx);

                string js = string.Format("alert('Synchronizace se vzdáleným servrem (SAP) byla neúspěšná!');");
                if (up == null) page.ClientScript.RegisterStartupScript(page.GetType(), "TVDSyncOrder", js, true);
                else ScriptManager.RegisterStartupScript(up, up.GetType(), "TVDSyncOrder", js, true);
                return false;
            }

            return true;
        }

        private static string GetString(string value, int maxLength) {
            if (string.IsNullOrEmpty(value)) return value;
            if (value.Length <= maxLength) return value;

            return value.Substring(0, maxLength);
        }

        public static void RecalculateDopravne(CartEntity cart, string shipmentCode) {
            cart.DopravneEurosap = 0;
            //Nastavenie dopravneho
            if (cart.CartProducts.Count != 0) {
                //if (shipmentCode.ToUpper() == "2"/*DPD*/ || shipmentCode.ToUpper() == "3"/*GLS*/) {
                if (shipmentCode.ToUpper() != "1"/*Osobni odber*/) {
                    CartProductEntity product = cart.CartProducts[0];
                    int currencyId = product.CurrencyId.Value;
                    SHP.Entities.Classifiers.Currency currency = Storage<SHP.Entities.Classifiers.Currency>.ReadFirst(new SHP.Entities.Classifiers.Currency.ReadById { Id = currencyId });
                    decimal sumaBezPostovneho = Common.DAL.Entities.OrderSettings.GetFreePostageSuma(Security.Account.Locale);
                    if (cart.KatalogovaCenaCelkem >= sumaBezPostovneho) {
                        cart.DopravneEurosap = 0;
                    } else {
                        if (currency.Code.ToUpper() == "CZK" && cart.KatalogovaCenaCelkemByEurosap < 1500)
                            cart.DopravneEurosap = 90;
                        else if (currency.Code.ToUpper() == "EUR" && cart.KatalogovaCenaCelkemByEurosap < 63)
                            cart.DopravneEurosap = 5;
                        else if (currency.Code.ToUpper() == "PLN" && cart.KatalogovaCenaCelkemByEurosap < 350)
                            cart.DopravneEurosap = 23;
                    }
                }
            }
            Storage<CartEntity>.Update(cart);
        }
    }
}