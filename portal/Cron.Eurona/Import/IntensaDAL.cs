using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Cron.Eurona.Import
{
		public static class IntensaDAL
		{

				public static class Order
				{
						public enum OrderStatus: int
						{
								None = 0,
								WaitingForProccess = -1,
								InProccess = -2,
								Proccessed = -3,
								Storno = -4
						}

						public static void SyncIntensaOrder( CMS.Pump.MSSQLStorage mssqStorageDst, int orderId, int orderStatusCode )
						{
								if ( orderStatusCode != (int)OrderStatus.InProccess && orderStatusCode != (int)OrderStatus.Storno && orderStatusCode != (int)OrderStatus.Proccessed ) return;
								using ( SqlConnection connection = mssqStorageDst.Connect() )
								{
										string sql = @"
												UPDATE tShpOrder WITH (ROWLOCK) SET OrderStatusCode=@OrderStatusCode
												WHERE OrderId=@OrderId";
										mssqStorageDst.Exec( mssqStorageDst.Connection, sql,
												new SqlParameter( "@OrderId", orderId ),
												new SqlParameter( "@OrderStatusCode", orderStatusCode.ToString() ) );
								}
						}
														
						public static void ExportOrder( CMS.Pump.MSSQLStorage mssqStorageDst, int Id_objednavky, string Cislo_objednavky, int Id_zakaznika, DateTime Datum_objednavky, int Stav_objednavky, int Spusob_dodani, int Spusob_platby,
														decimal Cena_prepravy, decimal Cena_prepravy_s_dph, decimal Cena_za_tovar, decimal Cena_za_tovar_s_dph, decimal Sleva_procent, decimal Sleva_suma, decimal Sleva_suma_s_dph, decimal Cena, decimal Cena_s_dph,
														string FA_Jmeno, string FA_Prijmeni, string FA_Firma, string FA_Ulice, string FA_Mesto ,string FA_Psc ,string FA_Stat ,string FA_Poznamka ,string FA_Telefon ,string FA_Ico , string FA_Dic ,
														string DA_Jmeno ,string DA_Prijmeni ,string DA_Firma ,string DA_Ulice ,string DA_Mesto ,string DA_Psc ,string DA_Stat ,string DA_Poznamka ,string DA_Telefon ,string DA_Ico ,string DA_Dic,
								string Kod_meny)
						{
								using ( SqlConnection connection = mssqStorageDst.Connect() )
								{
										string sql = "SELECT Id_objednavky FROM www_intensa_objednavky WHERE Id_objednavky=@Id_objednavky";
										DataTable dt = mssqStorageDst.Query( mssqStorageDst.Connection, sql, new SqlParameter( "@Id_objednavky", Id_objednavky ) );
										bool existst = dt.Rows.Count != 0;
										//INSERT
										if ( !existst )
										{
												sql = @"INSERT INTO www_intensa_objednavky ( Id_objednavky,Cislo_objednavky,Id_zakaznika,Datum_objednavky,Stav_objednavky,Spusob_dodani,Spusob_platby,
														Cena,Cena_s_dph, Cena_prepravy, Cena_prepravy_s_dph, Cena_za_tovar, Cena_za_tovar_s_dph, Sleva_procent, Sleva_suma, Sleva_suma_s_dph,
														FA_Jmeno, FA_Prijmeni, FA_Firma, FA_Ulice, FA_Mesto ,FA_Psc ,FA_Stat ,FA_Poznamka ,FA_Telefon ,FA_Ico ,FA_Dic ,
														DA_Jmeno ,DA_Prijmeni ,DA_Firma ,DA_Ulice ,DA_Mesto ,DA_Psc ,DA_Stat ,DA_Poznamka ,DA_Telefon ,DA_Ico ,DA_Dic, Kod_meny )
												VALUES
												(@Id_objednavky,@Cislo_objednavky,@Id_zakaznika,@Datum_objednavky,@Stav_objednavky,@Spusob_dodani,@Spusob_platby,
														@Cena,@Cena_s_dph, @Cena_prepravy, @Cena_prepravy_s_dph, @Cena_za_tovar, @Cena_za_tovar_s_dph, @Sleva_procent, @Sleva_suma, @Sleva_suma_s_dph,
														@FA_Jmeno, @FA_Prijmeni, @FA_Firma, @FA_Ulice, @FA_Mesto ,@FA_Psc ,@FA_Stat ,@FA_Poznamka ,@FA_Telefon ,@FA_Ico ,@FA_Dic ,
														@DA_Jmeno ,@DA_Prijmeni ,@DA_Firma ,@DA_Ulice ,@DA_Mesto ,@DA_Psc ,@DA_Stat ,@DA_Poznamka ,@DA_Telefon ,@DA_Ico ,@DA_Dic, @Kod_meny)";
												mssqStorageDst.Exec( mssqStorageDst.Connection, sql,
														new SqlParameter( "@Id_objednavky", Id_objednavky ),
														new SqlParameter( "@Cislo_objednavky", Null( Cislo_objednavky ) ),
														new SqlParameter( "@Id_zakaznika", Null( Id_zakaznika ) ),
														new SqlParameter( "@Datum_objednavky", Null( Datum_objednavky ) ),
														new SqlParameter( "@Stav_objednavky", Null( Stav_objednavky ) ),
														new SqlParameter( "@Spusob_dodani", Null( Spusob_dodani ) ),
														new SqlParameter( "@Spusob_platby", Null( Spusob_platby ) ),
														new SqlParameter( "@Cena_prepravy", Null( Cena_prepravy ) ),
														new SqlParameter( "@Cena_prepravy_s_dph", Null( Cena_prepravy_s_dph ) ),
														new SqlParameter( "@Cena_za_tovar", Null( Cena_za_tovar ) ),
														new SqlParameter( "@Cena_za_tovar_s_dph", Null( Cena_za_tovar_s_dph ) ),
														new SqlParameter( "@Sleva_procent", Null( Sleva_procent ) ),
														new SqlParameter( "@Sleva_suma", Null( Sleva_suma ) ),
														new SqlParameter( "@Sleva_suma_s_dph", Null( Sleva_suma_s_dph ) ),

														new SqlParameter( "@Cena", Null( Cena ) ),
														new SqlParameter( "@Cena_s_dph", Null( Cena_s_dph ) ),

														new SqlParameter( "@FA_Jmeno", Null( FA_Jmeno ) ),
														new SqlParameter( "@FA_Prijmeni", Null( FA_Prijmeni ) ),
														new SqlParameter( "@FA_Firma", Null( FA_Firma ) ),
														new SqlParameter( "@FA_Ulice", Null( FA_Ulice ) ),
														new SqlParameter( "@FA_Mesto", Null( FA_Mesto ) ),
														new SqlParameter( "@FA_Psc", Null( FA_Psc ) ),
														new SqlParameter( "@FA_Stat", Null( FA_Stat ) ),
														new SqlParameter( "@FA_Poznamka", Null( FA_Poznamka ) ),
														new SqlParameter( "@FA_Telefon", Null( FA_Telefon ) ),
														new SqlParameter( "@FA_Ico", Null( FA_Ico ) ),
														new SqlParameter( "@FA_Dic", Null( FA_Dic ) ),

														new SqlParameter( "@DA_Jmeno", Null( DA_Jmeno ) ),
														new SqlParameter( "@DA_Prijmeni", Null( DA_Prijmeni ) ),
														new SqlParameter( "@DA_Firma", Null( DA_Firma ) ),
														new SqlParameter( "@DA_Ulice", Null( DA_Ulice ) ),
														new SqlParameter( "@DA_Mesto", Null( DA_Mesto ) ),
														new SqlParameter( "@DA_Psc", Null( DA_Psc ) ),
														new SqlParameter( "@DA_Stat", Null( DA_Stat ) ),
														new SqlParameter( "@DA_Poznamka", Null( DA_Poznamka ) ),
														new SqlParameter( "@DA_Telefon", Null( DA_Telefon ) ),
														new SqlParameter( "@DA_Ico", Null( DA_Ico ) ),
														new SqlParameter( "@DA_Dic", Null( DA_Dic ) ),

														new SqlParameter( "@Kod_meny", Null( Kod_meny ) )
														);
										}
								}
						}

						public static void ExportOrderProduct( CMS.Pump.MSSQLStorage mssqStorageDst, int Id_objednavky, int C_Zbo, int Mnozstvi, decimal Cena_mj, decimal Cena_mj_s_dph, decimal Dph, string Kod_meny )
						{
								using ( SqlConnection connection = mssqStorageDst.Connect() )
								{
										string sql = "SELECT Id_objednavky, C_Zbo FROM www_intensa_objednavky_radky WHERE Id_objednavky=@Id_objednavky AND C_Zbo=@C_Zbo";
										DataTable dt = mssqStorageDst.Query( mssqStorageDst.Connection, sql, new SqlParameter( "@Id_objednavky", Id_objednavky ), new SqlParameter( "@C_Zbo", C_Zbo ) );
										bool exists = dt.Rows.Count != 0;
										//INSERT
										if ( !exists )
										{
												sql = @"INSERT INTO www_intensa_objednavky_radky ( Id_objednavky ,C_Zbo ,Mnozstvi ,Cena_mj ,Cena_mj_s_dph ,Dph, Kod_meny )
												VALUES (@Id_objednavky ,@C_Zbo ,@Mnozstvi ,@Cena_mj ,@Cena_mj_s_dph ,@Dph, @Kod_meny)";
												mssqStorageDst.Exec( mssqStorageDst.Connection, sql,
														new SqlParameter( "@Id_objednavky", Id_objednavky ),
														new SqlParameter( "@C_Zbo", Null( C_Zbo ) ),
														new SqlParameter( "@Mnozstvi", Null( Mnozstvi ) ),
														new SqlParameter( "@Cena_mj", Null( Cena_mj ) ),
														new SqlParameter( "@Cena_mj_s_dph", Null( Cena_mj_s_dph ) ),
														new SqlParameter( "@Dph", Null( Dph ) ),
														new SqlParameter( "@Kod_meny", Null( Kod_meny ) )
														);
										}
								}
						}

						public static DataTable GetTVDOrders( CMS.Pump.MSSQLStorage mssqStorageSrc )
						{
								using ( SqlConnection connection = mssqStorageSrc.Connect() )
								{
										string sql = @"SELECT Id_objednavky, Stav_objednavky FROM www_intensa_objednavky";
										DataTable dt = mssqStorageSrc.Query( mssqStorageSrc.Connection, sql );
										return dt;
								}
						}
						public static DataTable GetOrders( CMS.Pump.MSSQLStorage mssqStorageDst, int instanceId )
						{
								using ( SqlConnection connection = mssqStorageDst.Connect() )
								{
										string sql = @"SELECT Id_objednavky=o.OrderId ,Cislo_objednavky=o.OrderNumber, Id_zakaznika=o.AccountId ,Datum_objednavky=o.OrderDate ,o.CartId ,Stav_objednavky=o.OrderStatusCode ,Spusob_dodani=o.ShipmentCode ,Spusob_platby=o.PaymentCode ,
										Cena=o.Price ,Cena_s_dph=o.PriceWVAT ,
										Cena_prepravy = ISNULL(o.ShipmentPrice, 0), Cena_prepravy_s_dph = ISNULL(o.ShipmentPriceWVAT,0),
										Cena_za_tovar = c.Price, Cena_za_tovar_s_dph = c.PriceWVAT,
										Sleva_procent = c.Discount, Sleva_suma = c.Price - (o.Price - o.ShipmentPrice ), Sleva_suma_s_dph = c.PriceWVAT - (o.PriceWVAT - o.ShipmentPriceWVAT ),
										FA_Jmeno=fa.FirstName ,FA_Prijmeni=fa.LastName,FA_Firma=fa.Organization,FA_Ulice=fa.Street,FA_Mesto=fa.City ,FA_Psc=fa.Zip ,FA_Stat=fa.State ,FA_Poznamka=fa.Notes ,FA_Telefon=fa.Phone ,FA_Ico=fa.Id1 ,FA_Dic=fa.Id2 ,
										DA_Jmeno=da.FirstName ,DA_Prijmeni=da.LastName ,DA_Firma=da.Organization ,DA_Ulice=da.Street ,DA_Mesto=da.City ,DA_Psc=da.Zip ,DA_Stat=da.State ,DA_Poznamka=da.Notes ,DA_Telefon=da.Phone ,DA_Ico=da.Id1 ,DA_Dic =da.Id2,
										Kod_meny = o.CurrencyCode

										FROM vShpOrders o 
										INNER JOIN tShpCart c ON c.CartId = o.CartId
										INNER JOIN tShpAddress fa ON fa.AddressId = o.InvoiceAddressId
										INNER JOIN tShpAddress da ON da.AddressId = o.DeliveryAddressId
										WHERE o.InstanceId=@InstanceId AND o.OrderStatusCode=@OrderStatusCode";
										DataTable dt = mssqStorageDst.Query( mssqStorageDst.Connection, sql,
												new SqlParameter( "@InstanceId", instanceId ),
												new SqlParameter( "@OrderStatusCode", OrderStatus.WaitingForProccess ) );
										return dt;
								}
						}
						public static DataRow GetCart( CMS.Pump.MSSQLStorage mssqStorageDst, int cartId )
						{
								using ( SqlConnection connection = mssqStorageDst.Connect() )
								{
										string sql = @"SELECT c.CartId, c.InstanceId, c.SessionId, c.AccountId, c.Created,c.ShipmentCode,c.PaymentCode,c.DeliveryAddressId,c.InvoiceAddressId,c.Notes,c.Closed
										FROM tShpCart c 
										WHERE c.CartId=@CartId";
										DataTable dt = mssqStorageDst.Query( mssqStorageDst.Connection, sql, new SqlParameter( "@CartId", cartId ) );
										if ( dt.Rows.Count == 0 ) return null;
										return dt.Rows[0];
								}
						}
						public static DataTable GetCartProducts( CMS.Pump.MSSQLStorage mssqStorageDst, int cartId )
						{
								using ( SqlConnection connection = mssqStorageDst.Connect() )
								{
										string sql = @"SELECT cp.CartProductId,cp.InstanceId,cp.CartId,cp.ProductId,cp.Quantity ,cp.Price ,cp.PriceWVAT ,cp.VAT ,cp.Discount ,cp.PriceTotal ,cp.PriceTotalWVAT ,cp.CurrencyId, Kod_meny = c.Code
												FROM tShpCartProduct cp
												INNER JOIN cShpCurrency c ON c.CurrencyId = cp.CurrencyId 
												WHERE CartId=@CartId";
										DataTable dt = mssqStorageDst.Query( mssqStorageDst.Connection, sql, new SqlParameter( "@CartId", cartId ) );
										return dt;
								}
						}
				}

				public static class Account
				{
						public enum AddressType
						{
								Invoice,
								Delivery
						}

						public static DataTable GetAccounts( CMS.Pump.MSSQLStorage mssqStorageSrc )
						{
								using ( SqlConnection connection = mssqStorageSrc.Connect() )
								{
										string sql = @"SELECT Id_zakaznika= a.AccountId, a.Email,
										Jmeno=p.FirstName, Prijmeni=p.LastName, Mobil=p.Mobile, Telefon=p.Phone, Poznamka=p.Notes, Firma=NULL, ICO=NULL, DIC=NULL,
										FA_Mesto=ha.City, FA_Ulice=ha.Street, FA_Psc=ha.Zip, FA_Stat=ha.State,
										DA_Mesto=ta.City, DA_Ulice=ta.Street, DA_Psc=ta.Zip, DA_Stat=ta.State,
										Datum_registrace = a.Created
										FROM vAccounts a 
										INNER JOIN vPersons p ON p.AccountId = a.AccountId
										INNER JOIN vAddresses ha ON ha.AddressId = p.AddressHomeId
										INNER JOIN vAddresses ta ON ta.AddressId = p.AddressTempId
										WHERE a.Enabled=1 AND a.InstanceId = @InstanceId";
										DataTable dt = mssqStorageSrc.Query( mssqStorageSrc.Connection, sql, new SqlParameter( "@InstanceId", 2 ) );
										return dt;
								}
						}
						public static void ExportAccount( CMS.Pump.MSSQLStorage mssqStorageDst, int Id_zakaznika, string Jmeno, string Prijmeni, string Email, string Firma, DateTime Datum_registrace,
							string DIC, string ICO, string Telefon, string Mobil,
							string FA_Ulice, string FA_Mesto, string FA_Psc, string FA_Stat,
							string DA_Ulice, string DA_Mesto, string DA_Psc, string DA_Stat )
						{
								using ( SqlConnection connection = mssqStorageDst.Connect() )
								{
										string sql = "SELECT Id_zakaznika FROM www_intensa_zakaznik WHERE Id_zakaznika=@Id_zakaznika";
										DataTable dt = mssqStorageDst.Query( mssqStorageDst.Connection, sql, new SqlParameter( "@Id_zakaznika", Id_zakaznika ) );
										bool exists = dt.Rows.Count != 0;
										//INSERT
										if ( !exists )
										{
												sql = @"INSERT INTO www_intensa_zakaznik ( Id_zakaznika, Jmeno, Prijmeni, Email, Firma ,Datum_registrace ,ICO ,DIC ,Telefon ,Mobil ,
												FA_Ulice ,FA_Mesto ,FA_Psc ,FA_Stat ,DA_Ulice ,DA_Mesto ,DA_Psc ,DA_Stat)
												VALUES
												(@Id_zakaznika, @Jmeno, @Prijmeni, @Email, @Firma ,@Datum_registrace ,@ICO ,@DIC ,@Telefon ,@Mobil ,
												@FA_Ulice ,@FA_Mesto ,@FA_Psc ,@FA_Stat ,@DA_Ulice ,@DA_Mesto ,@DA_Psc ,@DA_Stat)";
												mssqStorageDst.Exec( mssqStorageDst.Connection, sql,
														new SqlParameter( "@Id_zakaznika", Id_zakaznika ),
														new SqlParameter( "@Jmeno", Null( Jmeno ) ),
														new SqlParameter( "@Prijmeni", Null( Prijmeni ) ),
														new SqlParameter( "@Email", Null( Email ) ),
														new SqlParameter( "@Firma", Null( Firma ) ),
														new SqlParameter( "@Datum_registrace", Null( Datum_registrace ) ),
														new SqlParameter( "@ICO", Null( ICO ) ),
														new SqlParameter( "@DIC", Null( DIC ) ),
														new SqlParameter( "@Telefon", Null( Telefon ) ),
														new SqlParameter( "@Mobil", Null( Mobil ) ),
														new SqlParameter( "@FA_Ulice", Null( FA_Ulice ) ),
														new SqlParameter( "@FA_Mesto", Null( FA_Mesto ) ),
														new SqlParameter( "@FA_Psc", Null( FA_Psc ) ),
														new SqlParameter( "@FA_Stat", Null( FA_Stat ) ),
														new SqlParameter( "@DA_Ulice", Null( DA_Ulice ) ),
														new SqlParameter( "@DA_Mesto", Null( DA_Mesto ) ),
														new SqlParameter( "@DA_Psc", Null( DA_Psc ) ),
														new SqlParameter( "@DA_Stat", Null( DA_Stat ) )
														);
										}
								}
						}
				}

				#region Helpers methods
				private static object Null( object obj )
				{
						return Null( obj, DBNull.Value );
				}

				private static object Null( bool condition, object obj )
				{
						return Null( condition, obj, DBNull.Value );
				}

				private static object Null( object obj, object def )
				{
						return Null( obj != null, obj, def );
				}

				private static object Null( bool condition, object obj, object def )
				{
						return condition ? obj : def;
				}
				#endregion
		}
}
