using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace EuronaImportFromTVD.DAL {
    public static class EuronaTVDDAL {
        public static string GetLocale(int jazyk) {
            //lang_CZ=0;
            //lang_EN=1;
            //lang_DE=2;
            //lang_SK=3;
            //lang_PL=7;

            switch (jazyk) {
                case 0:
                    return "cs";
                case 1:
                    return "en";
                case 2:
                    return "de";
                case 3:
                    return "sk";
                case 7:
                    return "pl";
                default:
                    return "cs";
            }
        }

        private static List<int> tvdJazyky = null;
        public static List<int> TVDJazyky {
            get {
                if (tvdJazyky != null) return tvdJazyky;
                tvdJazyky = new List<int>();

                tvdJazyky.AddRange(new int[] { 0, 1, 3, 7 });
                return tvdJazyky;

            }
        }

        public static DataTable GetTVDProducts(CMS.Pump.MSSQLStorage mssqStorageSrc, int instanceId) {
            using (SqlConnection connection = mssqStorageSrc.Connect()) {
                if (instanceId == 1) //Eurona
				{
                    string sql = "SELECT DISTINCT C_Zbo, [Eurona_Produkt], [Intensa_Produkt], [CernyForLife_Produkt]=[Intensa_Produkt], [Dispozice_HR], [Dispozice_HR1] FROM Produkty WHERE Eurona_Produkt=1 ORDER BY C_Zbo DESC";
                    DataTable dt = mssqStorageSrc.Query(connection, sql);
                    return dt;
                } else if (instanceId == 2) //Intensa
				{
                    string sql = "SELECT DISTINCT C_Zbo, [Eurona_Produkt], [Intensa_Produkt], [CernyForLife_Produkt]=[Intensa_Produkt], [Dispozice_HR], [Dispozice_HR1] FROM Produkty WHERE Intensa_Produkt=1 ORDER BY C_Zbo DESC";
                    DataTable dt = mssqStorageSrc.Query(connection, sql);
                    return dt;
                } else if (instanceId == 3) //Cerny for Life
				{
                    string sql = "SELECT DISTINCT C_Zbo, [Eurona_Produkt], [Intensa_Produkt], [CernyForLife_Produkt]=[Intensa_Produkt], [Dispozice_HR], [Dispozice_HR1] FROM Produkty WHERE Intensa_Produkt=1 ORDER BY C_Zbo DESC";
                    DataTable dt = mssqStorageSrc.Query(connection, sql);
                    return dt;
                } else {
                    string sql = "SELECT DISTINCT C_Zbo, [Eurona_Produkt], [Intensa_Produkt], [CernyForLife_Produkt]=[Intensa_Produkt], [Dispozice_HR], [Dispozice_HR1] FROM Produkty ORDER BY C_Zbo DESC";
                    DataTable dt = mssqStorageSrc.Query(connection, sql);
                    return dt;
                }
            }
        }

  
        #region Classifiers
        public static DataTable GetTVDClsSpecialniUcinky(CMS.Pump.MSSQLStorage mssqStorageSrc) {
            using (SqlConnection connection = mssqStorageSrc.Connect()) {
                string sql = @"SELECT Spec_Ucinek_Kod, Obrazek FROM Specialni_Ucinky";
                DataTable dt = mssqStorageSrc.Query(connection, sql);

                return dt;
            }
        }
        public static DataTable GetTVDClsParfemace(CMS.Pump.MSSQLStorage mssqStorageSrc) {
            using (SqlConnection connection = mssqStorageSrc.Connect()) {
                string sql = @"SELECT Parfemace_Id, Obrazek FROM Parfemace";
                DataTable dt = mssqStorageSrc.Query(connection, sql);

                return dt;
            }
        }
        #endregion

        #region Category methods
        /// <summary>
        /// Vrati zoznam Kategorie_Id pre dany produkt.
        /// </summary>
        public static DataTable GetTVDProductKategorie(CMS.Pump.MSSQLStorage mssqStorageSrc, int productId, int instanceId) {
            int shop = instanceId - 1;
            using (SqlConnection connection = mssqStorageSrc.Connect()) {
                string sql = @"SELECT C_Zbo, Kategorie_Id, Shop FROM Produkty_Kategorie WHERE C_Zbo=@ProductId AND Shop=@Shop";
                DataTable dt = mssqStorageSrc.Query(connection, sql,
                        new SqlParameter("@ProductId", productId),
                        new SqlParameter("@Shop", shop));

                return dt;
            }
        }

        /// <summary>
        /// Vrati ID, ParentID TVD kategorii
        /// </summary>
        public static DataTable GetTVDCategories(CMS.Pump.MSSQLStorage mssqStorageSrc) {
            using (SqlConnection connection = mssqStorageSrc.Connect()) {
                string sql = @"SELECT k.Kategorie_Id, k.Kategorie_Parent, k.Shop from Kategorie k ORDER BY k.Kategorie_Parent ASC";
                DataTable dt = mssqStorageSrc.Query(connection, sql);
                return dt;
            }
        }

        /// <summary>
        /// Vrati TVD Kategoriu pre dane ID a Jazyk
        /// </summary>
        public static DataRow GetTVDCategory(CMS.Pump.MSSQLStorage mssqStorageSrc, int instanceId, int categoryId, int jazyk) {
            int shop = instanceId - 1;
            using (SqlConnection connection = mssqStorageSrc.Connect()) {
                string sql = @"SELECT k.Kategorie_Id, k.Kategorie_Parent, kn.Jazyk, kn.Nazev from Kategorie k
										INNER JOIN Kategorie_Nazvy kn ON kn.Kategorie_Id = k.Kategorie_Id
										WHERE k.Kategorie_Id=@CategoryId AND kn.Jazyk=@Jazyk AND kn.Shop=@Shop";
                DataTable dt = mssqStorageSrc.Query(connection, sql,
                        new SqlParameter("@CategoryId", categoryId),
                        new SqlParameter("@Jazyk", jazyk),
                        new SqlParameter("@Shop", shop));

                if (dt.Rows.Count == 0) return null;
                return dt.Rows[0];
            }
        }
        #endregion

        #region Orders
        /*
        SELECT TOP 100 Id_objednavky, Id_odberatele, Zpusob_dodani, Zpusob_platby, Stav_faktury, Celkem_k_uhrade, Dor_nazev_firmy, Dor_misto, Dor_ulice, Dor_psc, Dor_stat, Info_pro_odb_html FROM objednavkyfaktury obfa
select Id_objednavky, Poradi, C_Zbo = Kod_polozky, Mnozstvi, Body_mj, Cena_mj_fakt_sdph from objednavkyfaktury_radky where Id_objednavky = 160211
            */

        public static DataRow GetTVDObjednavka(CMS.Pump.MSSQLStorage mssqStorageSrc, int idObjednavky) {
            using (SqlConnection connection = mssqStorageSrc.Connect()) {
                string sql = @"SELECT Id_objednavky, id_web_objednavky, Id_odberatele, Datum_zapisu_obj, Zpusob_dodani, Zpusob_platby, Stav_faktury, Kod_meny, Celkem_k_uhrade, Celkem_bez_dph = Zaklad_zs, Dor_nazev_firmy, Dor_misto, Dor_ulice, Dor_psc, Dor_stat, Dor_telefon, Dor_email, Info_pro_odb_html,
                zavoz_misto, zavoz_datum, zavoz_psc, Dodat_od, Dodat_do
                FROM objednavkyfaktury obfa
                where Id_objednavky=@idObjednavky";
                DataTable dt = mssqStorageSrc.Query(connection, sql, new SqlParameter("@idObjednavky", idObjednavky));
                if (dt == null) return null;
                if (dt.Rows == null || dt.Rows.Count == 0) return null;
                return dt.Rows[0];
            }
        }
        public static DataTable GetTVDObjednavkaRadky(CMS.Pump.MSSQLStorage mssqStorageSrc, int idObjednavky) {
            using (SqlConnection connection = mssqStorageSrc.Connect()) {
                string sql = @"select Id_objednavky, Poradi, C_Zbo = Kod_polozky, Mnozstvi, Body_mj, Cena_mj_fakt_sdph, Sazba_dph, Cena_mj_fakt_bezdph from objednavkyfaktury_radky where Id_objednavky = @idObjednavky and idakce=1";
                DataTable dt = mssqStorageSrc.Query(connection, sql, new SqlParameter("@idObjednavky", idObjednavky));
                return dt;
            }
        }
        #endregion

        #region Users
        public static void UpdateTVDPoradciStav(CMS.Pump.MSSQLStorage mssqStorageSrc, int poradcaId, int stav) {
            using (SqlConnection connection = mssqStorageSrc.Connect()) {
                string sql = @"UPDATE odberatele SET Stav=@Stav WHERE Id_odberatele = @OderatelId";
                mssqStorageSrc.Exec(connection, sql, new SqlParameter("OderatelId", poradcaId), new SqlParameter("@Stav", stav));
            }
        }
        public static DataTable GetTVDPoradci(CMS.Pump.MSSQLStorage mssqStorageSrc) {
            using (SqlConnection connection = mssqStorageSrc.Connect()) {
                string sql = @"SELECT 
					Id_odberatele ,Kod_odberatele ,Stat_odberatele ,Cislo_nadrizeneho ,Nazev_firmy ,Nazev_firmy_radek ,Ulice ,Psc,
					Misto ,Stat ,Dor_nazev_firmy ,Dor_nazev_firmy_radek ,Dor_ulice ,Dor_psc ,Dor_misto ,Dor_stat ,Telefon ,Mobil,
					E_mail ,Cislo_op ,Ico ,Dic ,P_f ,Banka ,Cislo_uctu ,Skupina ,Login_www ,Heslo_www ,Datum_zahajeni,
					Datum_pozastaveni ,Datum_ukonceni ,Stav_odberatele ,Platce_dph ,Statut ,Odpustit_limit ,Spec_symbol,
					Kod_oblasti ,Datum_narozeni ,Ar_na_dor_adr ,Leadersky_titul ,Telefon_prace ,Fax ,Icq,
					Skype ,Zakazat_www ,Ar_jen_1_strana ,Poznamka ,Stav, Top_manazer, Angel_team_clen, Angel_team_manager, Angel_team_typ_managera
					FROM odberatele WHERE Login_www IS NOT NULL AND Login_www != '' AND Stav_odberatele!='Z' AND Stav=1"; // !!!! LEN TEST
                DataTable dt = mssqStorageSrc.Query(connection, sql);

                return dt;
            }
        }
        public static DataRow GetTVDPoradca(CMS.Pump.MSSQLStorage mssqStorageSrc, int poradcaId) {
            using (SqlConnection connection = mssqStorageSrc.Connect()) {
                string sql = @"SELECT 
					Id_odberatele ,Kod_odberatele ,Stat_odberatele ,Cislo_nadrizeneho ,Nazev_firmy ,Nazev_firmy_radek ,Ulice ,Psc,
					Misto ,Stat ,Dor_nazev_firmy ,Dor_nazev_firmy_radek ,Dor_ulice ,Dor_psc ,Dor_misto ,Dor_stat ,Telefon ,Mobil,
					E_mail ,Cislo_op ,Ico ,Dic ,P_f ,Banka ,Cislo_uctu ,Skupina ,Login_www ,Heslo_www ,Datum_zahajeni,
					Datum_pozastaveni ,Datum_ukonceni ,Stav_odberatele ,Platce_dph ,Statut ,Odpustit_limit ,Spec_symbol,
					Kod_oblasti ,Datum_narozeni ,Ar_na_dor_adr ,Leadersky_titul ,Telefon_prace ,Fax ,Icq,
					Skype ,Zakazat_www ,Ar_jen_1_strana ,Poznamka ,Stav, Top_manazer, Angel_team_clen, Angel_team_manager, Angel_team_typ_managera
					FROM odberatele WHERE Id_odberatele = @OderatelId";
                DataTable dt = mssqStorageSrc.Query(connection, sql, new SqlParameter("@OderatelId", poradcaId));

                if (dt.Rows.Count == 0) return null;
                return dt.Rows[0];
            }
        }
        public static decimal GetTVDPoradcaMarze(CMS.Pump.MSSQLStorage mssqStorageSrc, int poradcaId) {
            using (SqlConnection connection = mssqStorageSrc.Connect()) {
                string sql = @"select Marze_platna from provize_aktualni 
										WHERE Id_odberatele=@Id_odberatele AND RRRRMM=(DATEPART(YEAR, GETDATE())*100 + DATEPART(MONTH, GETDATE()))";
                DataTable dt = mssqStorageSrc.Query(connection, sql, new SqlParameter("@Id_odberatele", poradcaId));

                if (dt.Rows.Count == 0) return 20m;
                return Convert.ToDecimal(dt.Rows[0]["Marze_platna"]);
            }
        }
        #endregion

        #region Helpers methods
        private static object Null(object obj) {
            return Null(obj, DBNull.Value);
        }

        private static object Null(bool condition, object obj) {
            return Null(condition, obj, DBNull.Value);
        }

        private static object Null(object obj, object def) {
            return Null(obj != null, obj, def);
        }

        private static object Null(bool condition, object obj, object def) {
            return condition ? obj : def;
        }
        #endregion
    }
}
