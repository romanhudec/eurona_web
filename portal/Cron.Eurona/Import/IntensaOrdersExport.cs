using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Pump;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Drawing;

namespace Cron.Eurona.Import
{
		public class IntensaOrdersExport: Synchronize
		{
				private int instanceId = 0;
				public IntensaOrdersExport( int instanceId, MSSQLStorage srcSqlStorage, MSSQLStorage dstSqlStorage )
						: base( srcSqlStorage, dstSqlStorage )
				{
						this.instanceId = instanceId;
				}

				public int InstanceId { get { return this.instanceId; } }

				public override void Synchronize()
				{
						int addedItems = 0;
						int updatedItems = 0;
						int errorItems = 0;
						int ignoredItems = 0;

						int rowsCount = 0;
						using ( SqlConnection connection = this.DestinationDataStorage.Connect() )
						{
								try
								{
										int rowIndex = 0;
										try
										{
												string sql = string.Empty;

												#region INTENSA->TVD
												DataTable td = IntensaDAL.Order.GetOrders( this.SourceDataStorage, this.InstanceId );
												foreach ( DataRow row in td.Rows )
												{
														int Id_objednavky = Convert.ToInt32( row["Id_objednavky"] );
														int cartId = Convert.ToInt32( row["CartId"] );

														string Cislo_objednavky = GetString( row["CartId"] );
														int Id_zakaznika = Convert.ToInt32( row["Id_zakaznika"] );
														DateTime Datum_objednavky = Convert.ToDateTime( row["Datum_objednavky"] );
														int Stav_objednavky = Convert.ToInt32( row["Stav_objednavky"] );
														int Spusob_dodani = Convert.ToInt32( row["Spusob_dodani"] );
														int Spusob_platby = Convert.ToInt32( row["Spusob_platby"] );

														decimal Cena_prepravy = Convert.ToDecimal( row["Cena_prepravy"] );
														decimal Cena_prepravy_s_dph = Convert.ToDecimal( row["Cena_prepravy_s_dph"] );
														decimal Cena_za_tovar = Convert.ToDecimal( row["Cena_za_tovar"] );
														decimal Cena_za_tovar_s_dph = Convert.ToDecimal( row["Cena_za_tovar_s_dph"] );
														decimal Sleva_procent = Convert.ToDecimal( row["Sleva_procent"] );
														decimal Sleva_suma = Convert.ToDecimal( row["Sleva_suma"] == DBNull.Value ? 0 : row["Sleva_suma"] );
														decimal Sleva_suma_s_dph = Convert.ToDecimal( row["Sleva_suma_s_dph"] == DBNull.Value ? 0 : row["Sleva_suma_s_dph"] );
														
														decimal Cena = Convert.ToDecimal( row["Cena"] );
														decimal Cena_s_dph = Convert.ToDecimal( row["Cena_s_dph"] );
														string FA_Jmeno = GetString( row["FA_Jmeno"] );
														string FA_Prijmeni = GetString( row["FA_Prijmeni"] );
														string FA_Firma = GetString( row["FA_Firma"] );
														string FA_Ulice = GetString( row["FA_Ulice"] );
														string FA_Mesto = GetString( row["FA_Mesto"] );
														string FA_Psc = GetString( row["FA_Psc"] );
														string FA_Stat = GetString( row["FA_Stat"] );
														string FA_Poznamka = GetString( row["FA_Poznamka"] );
														string FA_Telefon = GetString( row["FA_Telefon"] );
														string FA_Ico = GetString( row["FA_Ico"] );
														string FA_Dic = GetString( row["FA_Dic"] );
														string DA_Jmeno = GetString( row["DA_Jmeno"] );
														string DA_Prijmeni = GetString( row["DA_Prijmeni"] );
														string DA_Firma = GetString( row["DA_Firma"] );
														string DA_Ulice = GetString( row["DA_Ulice"] );
														string DA_Mesto = GetString( row["DA_Mesto"] );
														string DA_Psc = GetString( row["DA_Psc"] );
														string DA_Stat = GetString( row["DA_Stat"] );
														string DA_Poznamka = GetString( row["DA_Poznamka"] );
														string DA_Telefon = GetString( row["DA_Telefon"] );
														string DA_Ico = GetString( row["DA_Ico"] );
														string DA_Dic = GetString( row["DA_Dic"] );
														string Kod_meny = GetString( row["Kod_meny"] ); 
						

														DataRow cartRow = IntensaDAL.Order.GetCart( this.SourceDataStorage, cartId );
														DataTable dtCartProduct = IntensaDAL.Order.GetCartProducts( this.SourceDataStorage, cartId );

														try
														{
																if ( Stav_objednavky == (int)IntensaDAL.Order.OrderStatus.WaitingForProccess )
																		Stav_objednavky = (int)IntensaDAL.Order.OrderStatus.InProccess;

																IntensaDAL.Order.ExportOrder( this.DestinationDataStorage, Id_objednavky, Cislo_objednavky, Id_zakaznika, Datum_objednavky, Stav_objednavky, Spusob_dodani, Spusob_platby,
																		Cena_prepravy, Cena_prepravy_s_dph, Cena_za_tovar, Cena_za_tovar_s_dph, Sleva_procent, Sleva_suma, Sleva_suma_s_dph, Cena, Cena_s_dph, 
																		FA_Jmeno, FA_Prijmeni, FA_Firma, FA_Ulice, FA_Mesto, FA_Psc, FA_Stat, FA_Poznamka, FA_Telefon, FA_Ico, FA_Dic,
																		DA_Jmeno, DA_Prijmeni, DA_Firma, DA_Ulice, DA_Mesto, DA_Psc, DA_Stat, DA_Poznamka, DA_Telefon, DA_Ico, DA_Dic, Kod_meny );
																foreach ( DataRow dr in dtCartProduct.Rows )
																{
																		//CartProductId,InstanceId,CartId,ProductId,Quantity ,Price ,PriceWVAT ,VAT ,Discount ,PriceTotal ,PriceTotalWVAT ,CurrencyId
																		int C_Zbo = Convert.ToInt32( dr["ProductId"] );
																		int Mnozstvi = Convert.ToInt32( dr["Quantity"] );
																		decimal CenaMj = Convert.ToDecimal( dr["Price"] );
																		decimal CenaMjSDPH = Convert.ToDecimal( dr["PriceWVAT"] );
																		decimal Dph = Convert.ToDecimal( dr["VAT"] );
																		Kod_meny = GetString( dr["Kod_meny"] );
																		IntensaDAL.Order.ExportOrderProduct( this.DestinationDataStorage, Id_objednavky, C_Zbo, Mnozstvi, CenaMj, CenaMjSDPH, Dph, Kod_meny );
																}
																OnItemProccessed( rowIndex, rowsCount, string.Format( "Proccessing order '{0}' : ok", Id_objednavky ) );
														}
														catch ( Exception ex )
														{
																string errorMessage = string.Format( "Proccessing Orders : failed!", Id_objednavky );
																StringBuilder sbMessage = new StringBuilder();
																sbMessage.Append( errorMessage );
																sbMessage.AppendLine( ex.Message );
																if ( ex.InnerException != null ) sbMessage.AppendLine( ex.InnerException.Message );
																sbMessage.AppendLine( ex.StackTrace );

																OnError( errorMessage, ex );
#if !_OFFLINE_DEBUG
                        SendEmail( errorMessage, sbMessage.ToString() );
#endif
														}
												}
												#endregion
										}
										catch ( Exception ex )
										{
												string errorMessage = "Proccessing orders : failed!";
												StringBuilder sbMessage = new StringBuilder();
												sbMessage.Append( errorMessage );
												sbMessage.AppendLine( ex.Message );
												if ( ex.InnerException != null ) sbMessage.AppendLine( ex.InnerException.Message );
												sbMessage.AppendLine( ex.StackTrace );

												OnError( errorMessage, ex );
#if !_OFFLINE_DEBUG
												SendEmail( errorMessage, sbMessage.ToString() );
#endif
												errorItems++;
										}
										finally
										{
												rowIndex++;
										}

								}
								finally
								{
										OnFinish( rowsCount, addedItems, updatedItems, errorItems, ignoredItems );
								}
						}
				}

				private string GetString( object obj )
				{
						if ( obj == null ) return null;
						return obj.ToString().Trim();
				}

		}
}
