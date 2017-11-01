using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Cron.Eurona.Import
{
	public static class EuronaTVDDAL
	{
		public static string GetLocale(int jazyk)
		{
			//lang_CZ=0;
			//lang_EN=1;
			//lang_DE=2;
			//lang_SK=3;
			//lang_PL=7;

			switch (jazyk)
			{
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
		public static List<int> TVDJazyky
		{
			get
			{
				if (tvdJazyky != null) return tvdJazyky;
				tvdJazyky = new List<int>();

				tvdJazyky.AddRange(new int[] { 0, 1, 3, 7 });
				return tvdJazyky;

			}
		}

		public static DataTable GetTVDProducts(CMS.Pump.MSSQLStorage mssqStorageSrc, int instanceId)
		{
			using (SqlConnection connection = mssqStorageSrc.Connect())
			{
				if (instanceId == 1) //Eurona
				{
					string sql = "SELECT DISTINCT C_Zbo, [Eurona_Produkt], [Intensa_Produkt], [CernyForLife_Produkt]=[Intensa_Produkt], [Dispozice_HR], [Dispozice_HR1] FROM Produkty WHERE Eurona_Produkt=1";
					DataTable dt = mssqStorageSrc.Query(connection, sql);
					return dt;
				}
				else if (instanceId == 2) //Intensa
				{
					string sql = "SELECT DISTINCT C_Zbo, [Eurona_Produkt], [Intensa_Produkt], [CernyForLife_Produkt]=[Intensa_Produkt], [Dispozice_HR], [Dispozice_HR1] FROM Produkty WHERE Intensa_Produkt=1";
					DataTable dt = mssqStorageSrc.Query(connection, sql);
					return dt;
				}
				else if (instanceId == 3) //Cerny for Life
				{
					string sql = "SELECT DISTINCT C_Zbo, [Eurona_Produkt], [Intensa_Produkt], [CernyForLife_Produkt]=[Intensa_Produkt], [Dispozice_HR], [Dispozice_HR1] FROM Produkty WHERE Intensa_Produkt=1";
					DataTable dt = mssqStorageSrc.Query(connection, sql);
					return dt;
				}
				else
				{
					string sql = "SELECT DISTINCT C_Zbo, [Eurona_Produkt], [Intensa_Produkt], [CernyForLife_Produkt]=[Intensa_Produkt], [Dispozice_HR], [Dispozice_HR1] FROM Produkty";
					DataTable dt = mssqStorageSrc.Query(connection, sql);
					return dt;
				}
			}
		}

		#region Products methods
		public static DataRow GetTVDProduct(CMS.Pump.MSSQLStorage mssqStorageSrc, int productId)
		{
			using (SqlConnection connection = mssqStorageSrc.Connect())
			{
				string sql = @"SELECT C_Zbo,[Kod],[Universal_nazev],[Vat],[Stock_Unit],[Bod_hodnota],
								[Obrazek],[Obrazek2], [Obrazek3], [Obrazek4], [Obrazek5], [Obrazek6], [Obrazek7], [Obrazek8], [Obrazek9], 
								[Novinka],[Doprodej],[Inovace],[Prodej_Ukoncen],[Vyprodano], [Top_Produkt], [Eurona_Produkt], [Intensa_Produkt], [CL_Produkt]=Intensa_Produkt, [Dispozice_HR], [Dispozice_HR1],
								[Megasleva], [Supercena], [CLHit], [Action], [Vyprodej], [On_web],
								Parfumacia = (SELECT MAX(ppa.Parfemace_Id) FROM Produkty_Parfemace ppa WHERE ppa.C_Zbo=@ProductId ),
                                Zadni_etiketa,
                                Zobrazovat_zadni_etiketu
								FROM Produkty WHERE C_Zbo=@ProductId";
				DataTable dt = mssqStorageSrc.Query(connection, sql, new SqlParameter("@ProductId", productId));

				if (dt.Rows.Count == 0) return null;
				return dt.Rows[0];
			}
		}

		public static bool IsTVDProductLocalized(CMS.Pump.MSSQLStorage mssqStorageSrc, int productId)
		{
			using (SqlConnection connection = mssqStorageSrc.Connect())
			{
				string sql = @"SELECT Count(pn.C_Zbo )
								FROM Produkty_Nazvy pn
								LEFT JOIN Produkty_Motto pm ON pm.C_Zbo = pn.C_Zbo AND pm.Jazyk = pn.Jazyk
								LEFT JOIN Produkty_Popisy pp ON pp.C_Zbo = pn.C_Zbo AND pp.Jazyk = pn.Jazyk AND pp.Typ=0
								LEFT JOIN Produkty_Popisy pp1 ON pp1.C_Zbo = pn.C_Zbo AND pp1.Jazyk = pn.Jazyk AND pp1.Typ=1
								LEFT JOIN Produkty_Doplnujici_Info pdi ON pdi.C_Zbo = pn.C_Zbo AND pdi.Jazyk = pn.Jazyk
								WHERE pn.C_Zbo=@ProductId";
				DataTable dt = mssqStorageSrc.Query(connection, sql,
						new SqlParameter("@ProductId", productId));

				int count = Convert.ToInt32(dt.Rows[0][0]);
				return count != 0;
			}
		}
		public static DataRow GetTVDProductLocalize(CMS.Pump.MSSQLStorage mssqStorageSrc, int productId, int jazyk)
		{
			using (SqlConnection connection = mssqStorageSrc.Connect())
			{
				string sql = @"SELECT pn.C_Zbo, pn.Jazyk, Name = pn.Nazev, [Description] = pm.Motto, DescriptionLong=pp.Popis, InstructionsForUse=pp1.Popis, AdditionalInformation=pdi.Info
								FROM Produkty_Nazvy pn
								LEFT JOIN Produkty_Motto pm ON pm.C_Zbo = pn.C_Zbo AND pm.Jazyk = pn.Jazyk
								LEFT JOIN Produkty_Popisy pp ON pp.C_Zbo = pn.C_Zbo AND pp.Jazyk = pn.Jazyk AND pp.Typ=0
								LEFT JOIN Produkty_Popisy pp1 ON pp1.C_Zbo = pn.C_Zbo AND pp1.Jazyk = pn.Jazyk AND pp1.Typ=1
								LEFT JOIN Produkty_Doplnujici_Info pdi ON pdi.C_Zbo = pn.C_Zbo AND pdi.Jazyk = pn.Jazyk
								WHERE pn.C_Zbo=@ProductId AND pn.Jazyk=@Jazyk";
				DataTable dt = mssqStorageSrc.Query(connection, sql,
						new SqlParameter("@ProductId", productId),
						new SqlParameter("@Jazyk", jazyk));

				if (dt.Rows.Count == 0) return null;
				return dt.Rows[0];
			}
		}
		public static DataTable GetTVDProductPiktogramy(CMS.Pump.MSSQLStorage mssqStorageSrc, int productId, int jazyk)
		{
			using (SqlConnection connection = mssqStorageSrc.Connect())
			{
				string sql = @"SELECT ppg.C_Zbo, pg.Jazyk, Name=pg.Popis, ImageUrl=''
								FROM Produkty_Piktogramy ppg
								INNER JOIN Piktogramy_Popisy pg ON pg.Piktogram_Id = ppg.Piktogram_Id
								WHERE ppg.C_Zbo=@ProductId AND pg.Jazyk=@Jazyk";
				DataTable dt = mssqStorageSrc.Query(connection, sql,
						new SqlParameter("@ProductId", productId),
						new SqlParameter("@Jazyk", jazyk));

				return dt;
			}
		}
		public static DataTable GetTVDProductUcinky(CMS.Pump.MSSQLStorage mssqStorageSrc, int productId, int jazyk)
		{
			using (SqlConnection connection = mssqStorageSrc.Connect())
			{
				string sql = @"SELECT psu.C_Zbo, sup.Jazyk, Name=sup.Popis, ImageUrl=su.Obrazek, su.Spec_Ucinek_Kod
								    FROM Produkty_Specialni_Ucinky psu
								    INNER JOIN Specialni_Ucinky su ON su.Spec_Ucinek_Kod = psu.Spec_Ucinek_Kod
								    LEFT JOIN Specialni_Ucinky_Popisy sup ON sup.Spec_Ucinek_Kod = psu.Spec_Ucinek_Kod AND sup.Jazyk=@Jazyk
								    WHERE psu.C_Zbo=@ProductId ";
				DataTable dt = mssqStorageSrc.Query(connection, sql,
						new SqlParameter("@ProductId", productId),
						new SqlParameter("@Jazyk", jazyk));

				return dt;
			}
		}
		public static DataTable GetTVDProductVlastnosti(CMS.Pump.MSSQLStorage mssqStorageSrc, int productId, int jazyk)
		{
			using (SqlConnection connection = mssqStorageSrc.Connect())
			{
				string sql = @"SELECT pv.C_Zbo, pv.Jazyk, Name='', ImageUrl=pv.Obrazek
										FROM Produkty_Vlastnosti pv
										WHERE pv.C_Zbo=@ProductId AND pv.Jazyk=@Jazyk";
				DataTable dt = mssqStorageSrc.Query(connection, sql,
						new SqlParameter("@ProductId", productId),
						new SqlParameter("@Jazyk", jazyk));

				return dt;
			}
		}
		public static DataTable GetTVDProductAlternativni(CMS.Pump.MSSQLStorage mssqStorageSrc, int productId)
		{
			using (SqlConnection connection = mssqStorageSrc.Connect())
			{
				string sql = @"SELECT C_Zbo, Alternativni_C_Zbo FROM Produkty_Alternativni WHERE C_Zbo=@ProductId";
				DataTable dt = mssqStorageSrc.Query(connection, sql, new SqlParameter("@ProductId", productId));

				return dt;
			}
		}

		public static DataRow GetTVDProductCeny(CMS.Pump.MSSQLStorage mssqStorageSrc, int productId, int jazyk)
		{
			string mena = "CZK";
			switch (jazyk)
			{
				case 0: mena = "CZK";
					break;
				case 1: mena = "USD";
					break;
				case 2: mena = "EUR";
					break;
				case 3: mena = "EUR";
					break;
				case 7: mena = "PLN";
					break;
				default: mena = "CZK";
					break;
			}

			string kod = "Kč";
			switch (jazyk)
			{
				case 0: kod = "Kč";
					break;
				case 1: kod = "$";
					break;
				case 2: kod = "€";
					break;
				case 3: kod = "€";
					break;
				case 7: kod = "zł";
					break;
				default: kod = "Kč";
					break;
			}

			using (SqlConnection connection = mssqStorageSrc.Connect())
			{
				string sql = @"SELECT pc.C_Zbo, pc.Mena, Kod=@Kod, pc.Body, pc.Cena, pc.Bezna_cena, pc.Marze_povolena, pc.Marze_povolena_minimalni, pc.Cena_BK
										FROM Produkty_Ceny pc
										WHERE pc.C_Zbo=@ProductId AND pc.Mena = @Mena";
				DataTable dt = mssqStorageSrc.Query(connection, sql,
						new SqlParameter("@ProductId", productId),
						new SqlParameter("@Mena", mena),
						new SqlParameter("@Kod", kod));

				if (dt.Rows.Count == 0) return null;
				return dt.Rows[0];
			}
		}
		#endregion

		#region Classifiers
		public static DataTable GetTVDClsSpecialniUcinky(CMS.Pump.MSSQLStorage mssqStorageSrc)
		{
			using (SqlConnection connection = mssqStorageSrc.Connect())
			{
				string sql = @"SELECT Spec_Ucinek_Kod, Obrazek FROM Specialni_Ucinky";
				DataTable dt = mssqStorageSrc.Query(connection, sql);

				return dt;
			}
		}
		public static DataTable GetTVDClsParfemace(CMS.Pump.MSSQLStorage mssqStorageSrc)
		{
			using (SqlConnection connection = mssqStorageSrc.Connect())
			{
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
		public static DataTable GetTVDProductKategorie(CMS.Pump.MSSQLStorage mssqStorageSrc, int productId, int instanceId)
		{
			int shop = instanceId - 1;
			using (SqlConnection connection = mssqStorageSrc.Connect())
			{
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
		public static DataTable GetTVDCategories(CMS.Pump.MSSQLStorage mssqStorageSrc)
		{
			using (SqlConnection connection = mssqStorageSrc.Connect())
			{
				string sql = @"SELECT k.Kategorie_Id, k.Kategorie_Parent, k.Shop from Kategorie k ORDER BY k.Kategorie_Parent ASC";
				DataTable dt = mssqStorageSrc.Query(connection, sql);
				return dt;
			}
		}

		/// <summary>
		/// Vrati TVD Kategoriu pre dane ID a Jazyk
		/// </summary>
		public static DataRow GetTVDCategory(CMS.Pump.MSSQLStorage mssqStorageSrc, int instanceId, int categoryId, int jazyk)
		{
			int shop = instanceId - 1;
			using (SqlConnection connection = mssqStorageSrc.Connect())
			{
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
		public static DataTable GetTVDFinalOrders(CMS.Pump.MSSQLStorage mssqStorageSrc, int instanceId)
		{
			using (SqlConnection connection = mssqStorageSrc.Connect())
			{
                string sql = @"SELECT orderId = fa.id_prepoctu, fa.cislo_objednavky_eurosap, fa.potvrzeno, obfa.StavK2, obfa.Kod_meny, fa.sdeleni_pro_poradce_html
								FROM objednavkyfaktury obfa
								INNER JOIN www_faktury fa ON fa.cislo_objednavky_eurosap=obfa.Id_objednavky
								WHERE fa.potvrzeno=1 AND obfa.Datum_zmeny > DATEADD(MONTH ,-2, GETDATE()) AND ( obfa.StavK2 IS NOT NULL AND obfa.StavK2 != 0 )";
//                string sql = @"SELECT orderId = fa.id_prepoctu, fa.cislo_objednavky_eurosap, fa.potvrzeno, obfa.StavK2, obfa.Kod_meny, fa.sdeleni_pro_poradce_html
//								FROM objednavkyfaktury obfa
//								INNER JOIN www_faktury fa ON fa.cislo_objednavky_eurosap=obfa.Id_objednavky
//								WHERE fa.potvrzeno=1 AND ( obfa.StavK2 IS NOT NULL AND obfa.StavK2 != 0 )";
				DataTable dt = mssqStorageSrc.Query(connection, sql, new SqlParameter("@InstanceId", instanceId));
				return dt;
			}
		}
		public static DataTable GetTVDFakturyOrders(CMS.Pump.MSSQLStorage mssqStorageSrc, int instanceId)
		{
			using (SqlConnection connection = mssqStorageSrc.Connect())
			{
                string sql = @"SELECT orderId = fa.id_prepoctu, fa.cislo_objednavky_eurosap, fa.potvrzeno, fa.Kod_meny, fa.sdeleni_pro_poradce_html
								FROM www_faktury fa
								WHERE fa.datum_splatnosti > DATEADD(MONTH ,-2, GETDATE()) AND fa.potvrzeno=1";
//                string sql = @"SELECT orderId = fa.id_prepoctu, fa.cislo_objednavky_eurosap, fa.potvrzeno, fa.Kod_meny, fa.sdeleni_pro_poradce_html
//								FROM www_faktury fa
//								WHERE fa.potvrzeno=1";
				DataTable dt = mssqStorageSrc.Query(connection, sql, new SqlParameter("@InstanceId", instanceId));
				return dt;
			}
		}
		#endregion

		#region Users
		public static void UpdateTVDPoradciStav(CMS.Pump.MSSQLStorage mssqStorageSrc, int poradcaId, int stav)
		{
			using (SqlConnection connection = mssqStorageSrc.Connect())
			{
				string sql = @"UPDATE odberatele SET Stav=@Stav WHERE Id_odberatele = @OderatelId";
				mssqStorageSrc.Exec(connection, sql, new SqlParameter("OderatelId", poradcaId), new SqlParameter("@Stav", stav));
			}
		}
		public static DataTable GetTVDPoradci(CMS.Pump.MSSQLStorage mssqStorageSrc)
		{
			using (SqlConnection connection = mssqStorageSrc.Connect())
			{
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
		public static DataRow GetTVDPoradca(CMS.Pump.MSSQLStorage mssqStorageSrc, int poradcaId)
		{
			using (SqlConnection connection = mssqStorageSrc.Connect())
			{
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
		public static decimal GetTVDPoradcaMarze(CMS.Pump.MSSQLStorage mssqStorageSrc, int poradcaId)
		{
			using (SqlConnection connection = mssqStorageSrc.Connect())
			{
				string sql = @"select Marze_platna from provize_aktualni 
										WHERE Id_odberatele=@Id_odberatele AND RRRRMM=(DATEPART(YEAR, GETDATE())*100 + DATEPART(MONTH, GETDATE()))";
				DataTable dt = mssqStorageSrc.Query(connection, sql, new SqlParameter("@Id_odberatele", poradcaId));

				if (dt.Rows.Count == 0) return 20m;
				return Convert.ToDecimal(dt.Rows[0]["Marze_platna"]);
			}
		}
		#endregion

		#region Helpers methods
		private static object Null(object obj)
		{
			return Null(obj, DBNull.Value);
		}

		private static object Null(bool condition, object obj)
		{
			return Null(condition, obj, DBNull.Value);
		}

		private static object Null(object obj, object def)
		{
			return Null(obj != null, obj, def);
		}

		private static object Null(bool condition, object obj, object def)
		{
			return condition ? obj : def;
		}
		#endregion
	}
}
