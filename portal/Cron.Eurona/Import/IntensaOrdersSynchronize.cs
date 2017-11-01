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
		/// Synchronizacia obrazkou ciselnikou z TVD
		/// </summary>
		public class IntensaOrdersSynchronize: Synchronize
		{
				private int instanceId = 0;
				public IntensaOrdersSynchronize( int instanceId, MSSQLStorage srcSqlStorage, MSSQLStorage dstSqlStorage )
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

												#region TVD->INTENSA
												DataTable td = IntensaDAL.Order.GetTVDOrders( this.SourceDataStorage );
												foreach ( DataRow row in td.Rows )
												{
														int orderId = Convert.ToInt32( row["Id_objednavky"] );
														int orderStatusCode = Convert.ToInt32( row["Stav_objednavky"] );

														try
														{
																IntensaDAL.Order.SyncIntensaOrder( this.DestinationDataStorage, orderId, orderStatusCode );
																OnItemProccessed( rowIndex, rowsCount, string.Format( "Proccessing order '{0}' : ok", orderId ) );
														}
														catch ( Exception ex )
														{
																string errorMessage = string.Format( "Proccessing Orders : failed!", orderId );
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
