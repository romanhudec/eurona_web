using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CMS.Pump;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Net;
using System.Drawing;

namespace Cron.Eurona.Import
{
	public class CernyForLifeProductStockSynchronize : Synchronize
	{
		private int instanceId = 0;
		private int productId = 0;
		private int stockCount = 0;

		public CernyForLifeProductStockSynchronize(int instanceId, int productId, int stockCount, MSSQLStorage srcSqlStorage, MSSQLStorage dstSqlStorage)
			: base(srcSqlStorage, dstSqlStorage)
		{
			this.productId = productId;
			this.instanceId = instanceId;
			this.stockCount = stockCount;
		}

		public int ProductId { get { return this.productId; } }
		public int InstanceId { get { return this.instanceId; } }

		public override void Synchronize()
		{
			int addedItems = 0;
			int updatedItems = 0;
			int errorItems = 0;
			int ignoredItems = 0;

			using (SqlConnection connection = this.DestinationDataStorage.Connect())
			{
				int rowsCount = 0;

				try
				{
					int rowIndex = 0;
					try
					{
						string sql = string.Empty;
						EuronaDAL.Product.SyncProductStock(this.DestinationDataStorage, this.ProductId, this.InstanceId, this.stockCount );
						OnItemProccessed(rowIndex, rowsCount, string.Format("Proccessing product '{0}' : ok", this.ProductId));

					}
					catch (Exception ex)
					{
						string errorMessage = string.Format("Proccessing product '{0}' : failed!", this.ProductId);
						StringBuilder sbMessage = new StringBuilder();
						sbMessage.Append(errorMessage);
						sbMessage.AppendLine(ex.Message);
						if (ex.InnerException != null) sbMessage.AppendLine(ex.InnerException.Message);
						sbMessage.AppendLine(ex.StackTrace);

						OnError(errorMessage, ex);
#if !_OFFLINE_DEBUG
						SendEmail(errorMessage, sbMessage.ToString());
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
					OnFinish(rowsCount, addedItems, updatedItems, errorItems, ignoredItems);
				}
			}
		}
	}
}
