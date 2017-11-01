using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace CMS.Pump
{
		public interface IDataSynchronize
		{

				DataStorage SourceDataStorage { get; }
				DataStorage DestinationDataStorage { get; }
				object GetErrorResult();

				/// <summary>
				/// Synchronize data.
				/// </summary>
				void Synchronize();
		}

		/// <summary>
		/// Summary description for DataSynchronize.
		/// </summary>
		public abstract class DataSynchronize: IDataSynchronize
		{
				private DataStorage srcDataStorage = null;
				private DataStorage dstDataStorage = null;

				public delegate void ItemProccessedEventHandler( int item, int totalItmes, string message );
				public event ItemProccessedEventHandler ItemProccessed = null;

				public delegate void ErrorEventHandler( string message, Exception e );
				public event ErrorEventHandler Error = null;

				public delegate void InfoEventHandler( string message );
				public event InfoEventHandler Info = null;

				public delegate void FinishEventHandler( int totalItems, int addedItems, int updatedItems, int errorItems, int ignoredItem, object errorResult );
				public event FinishEventHandler Finish = null;

				public DataSynchronize( DataStorage srcDataStorage, DataStorage dstDataStorage )
				{
						this.srcDataStorage = srcDataStorage;
						this.dstDataStorage = dstDataStorage;
				}

				#region IDataSynchronize Members

				public virtual DataStorage SourceDataStorage
				{
						get { return this.srcDataStorage; }
				}

				public virtual DataStorage DestinationDataStorage
				{
						get { return this.dstDataStorage; }
				}

				public virtual void Synchronize()
				{
						OnFinish( 0, 0, 0, 0, 0 );
				}
				#endregion


				protected void OnItemProccessed( int item, int totalItmes, string message )
				{
						if ( ItemProccessed != null )
								ItemProccessed( item, totalItmes, message );
				}
				protected void OnInfo( string message  )
				{
						if ( Info != null )
								Info( message );
				}

				protected void OnError( string message )
				{
						OnError( message, null );
				}

				protected void OnError( string message, Exception ex )
				{
						if ( Error != null )
								Error( message, ex );
				}

				protected void OnFinish( int totalItems, int addedItems, int updatedItems, int errorItems, int ignoredItem )
				{
						if ( Finish != null )
						{
								object errorResult = null;
								if ( errorItems != 0 )
										errorResult = GetErrorResult();

								Finish( totalItems, addedItems, updatedItems, errorItems, ignoredItem, errorResult );
						}
				}

				public virtual object GetErrorResult()
				{
						throw new NotImplementedException();
				}

				#region Helper conversion methods
				protected decimal ToDecimal( object data )
				{
						CultureInfo ci = CultureInfo.CurrentCulture;
						string separator = ci.NumberFormat.NumberDecimalSeparator;

						if ( data == null || data.ToString() == string.Empty )
								throw new InvalidOperationException( "Nesprávný formát vstupného èísla a nebo èíslo není èíslo!" );

						string strDecimal = data.ToString();
						strDecimal = strDecimal.Replace( ",", separator );
						strDecimal = strDecimal.Replace( ".", separator );

						try
						{
								return Convert.ToDecimal( strDecimal );
						}
						catch
						{
								throw new InvalidOperationException( string.Format( "Nesprávný formát vstupného èísla {0}!", strDecimal ) );
						}
				}
				#endregion
		}

		public class SynchronizeError
		{
				public SynchronizeError()
				{
				}

				public int Id { get; set; }
				public string Message { get; set; }
				public string ImportFileId { get; set; }
				public int ImportFileRow { get; set; }
				public DateTime ImportDate { get; set; }
		}
}
