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
		/// <summary>
		/// Synchronizacia poradcou z TVD
		/// </summary>
		public class IntensaAccoutsExport: Synchronize
		{
				private int instanceId = 0;
				public IntensaAccoutsExport( int instanceId, MSSQLStorage srcSqlStorage, MSSQLStorage dstSqlStorage )
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
												//INTENSA->TVD
												DataTable td = IntensaDAL.Account.GetAccounts( this.SourceDataStorage );
												foreach ( DataRow row in td.Rows )
												{
														int Id_zakaznika = Convert.ToInt32( row["Id_zakaznika"] );
														string Jmeno = GetString( row["Jmeno"] );
														string Prijmeni = GetString( row["Prijmeni"] );
														string Email = GetString( row["Email"] );
														string Firma = GetString( row["Firma"] );
														DateTime Datum_registrace = Convert.ToDateTime( row["Datum_registrace"] );
														string ICO = GetString( row["ICO"] );
														string DIC = GetString( row["DIC"] );
														string Telefon = GetString( row["Telefon"] );
														string Mobil = GetString( row["Mobil"] );
														string FA_Ulice = GetString( row["FA_Ulice"] );
														string FA_Mesto = GetString( row["FA_Mesto"] );
														string FA_Psc = GetString( row["FA_Psc"] );
														string FA_Stat = GetString( row["FA_Stat"] );
														string DA_Ulice = GetString( row["DA_Ulice"] );
														string DA_Mesto = GetString( row["DA_Mesto"] );
														string DA_Psc = GetString( row["DA_Psc"] );
														string DA_Stat = GetString( row["DA_Stat"] );

														try
														{
																IntensaDAL.Account.ExportAccount( this.DestinationDataStorage, Id_zakaznika, Jmeno, Prijmeni, Email, Firma, Datum_registrace, ICO, DIC, Telefon, Mobil,
																		FA_Ulice, FA_Mesto, FA_Psc, FA_Stat, DA_Ulice, DA_Mesto, DA_Psc, DA_Stat );
																OnItemProccessed( rowIndex, rowsCount, string.Format( "Proccessing account TVD_Id '{0}' : ok", Id_zakaznika ) );
														}
														catch ( Exception ex )
														{
																string errorMessage = string.Format( "Proccessing account Id_zakaznika : {0} failed!", Id_zakaznika );
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

										}
										catch ( Exception ex )
										{
												string errorMessage = "Proccessing accounts : failed!";
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
						if ( obj == null || obj == DBNull.Value ) return null;
						return obj.ToString().Trim();
				}

		}
}
